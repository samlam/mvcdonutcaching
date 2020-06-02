using DevTrends.MvcDonutCaching.Interfaces;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace DevTrends.MvcDonutCaching
{
    public class DonutHoleFiller : IDonutHoleFiller
    {
        // marker has 3 parts - type/ settings/ content; type can be Action or Partial
        public const string DonutMarker = @"<!--Donut#{0}#{1}#-->{2}<!--EndDonut-->";
        public const string ActionMarker = "action";
        public const string PartialMarker = "partial";

        private static readonly Regex DonutHoles = new Regex("<!--Donut#(.*?)#(.*?)#-->(.*?)<!--EndDonut-->", RegexOptions.Compiled | RegexOptions.Singleline);

        private readonly IActionSettingsSerialiser _actionSettingsSerialiser;
        private readonly IPartialSettingsSerializer _partialSettingsSerializer;

        public DonutHoleFiller(IActionSettingsSerialiser actionSettingsSerialiser, IPartialSettingsSerializer partialSettingsSerializer)
        {
            if (actionSettingsSerialiser == null)
            {
                throw new ArgumentNullException("actionSettingsSerialiser");
            }

            _actionSettingsSerialiser = actionSettingsSerialiser;
            _partialSettingsSerializer = partialSettingsSerializer;
        }

        public string RemoveDonutHoleWrappers(string content, ControllerContext filterContext, OutputCacheOptions options)
        {
            if (
                filterContext.IsChildAction &&
                (options & OutputCacheOptions.ReplaceDonutsInChildActions) != OutputCacheOptions.ReplaceDonutsInChildActions)
            {
                return content;
            }

            return DonutHoles.Replace(content, match => match.Groups[3].Value);
        }

        public string ReplaceDonutHoleContent(string content, ControllerContext filterContext, OutputCacheOptions options)
        {
            if (
                filterContext.IsChildAction &&
                (options & OutputCacheOptions.ReplaceDonutsInChildActions) != OutputCacheOptions.ReplaceDonutsInChildActions)
            {
                return content;
            }

            return DonutHoles.Replace(content, match =>
            {
                var type = match.Groups[1].Value;
                var fragment = match.Groups[2].Value;
                if (type.Equals(ActionMarker, StringComparison.OrdinalIgnoreCase))
                {
                    var actionSettings = _actionSettingsSerialiser.Deserialise(fragment);

                    return InvokeAction(
                        filterContext.Controller,
                        actionSettings.ActionName,
                        actionSettings.ControllerName,
                        actionSettings.RouteValues
                    );
                }

                var partialSettings = _partialSettingsSerializer.Deserialize(fragment);

                return InvokePartial(filterContext.Controller
                    , partialSettings.PartialViewName
                    , new ViewDataDictionary(partialSettings.Model) { { "Title", partialSettings.Model } });
            });
        }

        private static string InvokeAction(ControllerBase controller, string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            var viewContext = new ViewContext(
                controller.ControllerContext,
                new WebFormView(controller.ControllerContext, "tmp"),
                controller.ViewData,
                controller.TempData,
                TextWriter.Null
            );

            var htmlHelper = new HtmlHelper(viewContext, new ViewPage());

            return htmlHelper.Action(actionName, controllerName, routeValues).ToString();
        }

        private static string InvokePartial(ControllerBase controller, string partialViewName, ViewDataDictionary viewData)
        {
            var viewContext = new ViewContext(
                controller.ControllerContext,
                new WebFormView(controller.ControllerContext, "tmp"),
                controller.ViewData,
                controller.TempData,
                TextWriter.Null
            );

            var htmlHelper = new HtmlHelper(viewContext, new ViewPage());

            return htmlHelper.Partial(partialViewName, viewData).ToString();
        }
    }
}
