using DevTrends.MvcDonutCaching.Interfaces;
using JetBrains.Annotations;
using ProtoBuf;
using ProtoBuf.Meta;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.UI.WebControls.WebParts;

namespace DevTrends.MvcDonutCaching
{
    public static partial class HtmlHelperExtensions
    {
        private static IPartialSettingsSerializer _partialSerializer;
        public static IPartialSettingsSerializer PartialSerializer
        {
            get
            {
                if (_partialSerializer == null)
                {
                    //var cList = OutputCacheManager.CacheTypes.ToList();
                    //cList.Add(typeof(ViewDataDictionary));
                    // _partialSerializer = new PartialSettingsDCSerializer(cList.ToArray());
                    _partialSerializer = new PartialSettingsSerializer();
                }
                return _partialSerializer;
            }
            set
            {
                _partialSerializer = value;
            }
        }

        public static MvcHtmlString Partial(this HtmlHelper h, string partialViewName, bool excludeFromParentCache)
        {
            return h.Partial(partialViewName, null, excludeFromParentCache);
        }
        public static MvcHtmlString Partial(this HtmlHelper h, string partialViewName, object model, bool excludeFromParentCache)
        {
            var actionName = h.ViewContext.RouteData.Values["action"].ToString();
            var controllerName = h.ViewContext.Controller.GetType().Name;
            var routeValues = h.ViewContext.RouteData.Values;

            if (excludeFromParentCache)
            {
                object modelInfo = model ?? h.ViewData.Model;
                var serializedPartialSettings = GetSerialisedPartialSettings(partialViewName, modelInfo, h.ViewData);
                MvcHtmlString fragment = h.Partial(partialViewName, modelInfo, h.ViewData); 
                return new MvcHtmlString(string.Format(DonutHoleFiller.DonutMarker, DonutHoleFiller.PartialMarker, serializedPartialSettings, fragment));
            }

            return h.Partial(partialViewName);
        }

        private static string GetSerialisedPartialSettings(string partialViewName, object model, ViewDataDictionary viewData)
        {
            var partialSettings = new PartialSettings
            {
                PartialViewName = partialViewName,
                Model = model,
                ModelTypeName = viewData.Model.GetType().FullName
            };

            return PartialSerializer.Serialize(partialSettings);
        }

        /*
        private static MvcHtmlString _Partial(this HtmlHelper htmlHelper, string partialViewName, object model, ViewDataDictionary viewData)
        {
            using (StringWriter writer = new StringWriter(CultureInfo.CurrentCulture))
            {
                htmlHelper.RenderPartialInternal(partialViewName, viewData, model, writer, ViewEngines.Engines);
                return MvcHtmlString.Create(writer.ToString());
            }
        }


        public static void RenderPartialView(this HtmlHelper h, [AspMvcAction] string actionName, [AspMvcController] string controllerName, RouteValueDictionary routeValues, bool excludeFromParentCache)
        {
            if (excludeFromParentCache)
            {
                var serialisedActionSettings = GetSerialisedActionSettings(actionName, controllerName, routeValues);

                h.ViewContext.Writer.Write("<!--Donut#{0}#-->", serialisedActionSettings);
            }

            h.RenderAction(actionName, controllerName, routeValues);

            if (excludeFromParentCache)
            {
                h.ViewContext.Writer.Write("<!--EndDonut-->");
            }
        }


        private static void RenderPartialInternal(this HtmlHelper htmlHelper, string partialViewName, ViewDataDictionary viewData, object model, TextWriter writer, ViewEngineCollection viewEngineCollection)
        {
            if (string.IsNullOrEmpty(partialViewName))
            {
                throw new ArgumentException("RenderPartialInternal error - partial view is null or empty", "partialViewName");
            }

            ViewDataDictionary newViewData = null;

            if (model == null)
            {
                if (viewData == null)
                {
                    newViewData = new ViewDataDictionary(htmlHelper.ViewData);
                }
                else
                {
                    newViewData = new ViewDataDictionary(viewData);
                }
            }
            else
            {
                if (viewData == null)
                {
                    newViewData = new ViewDataDictionary(model);
                }
                else
                {
                    newViewData = new ViewDataDictionary(viewData) { Model = model };
                }
            }

            ViewContext newViewContext = new ViewContext(htmlHelper.ViewContext, htmlHelper.ViewContext.View, newViewData, htmlHelper.ViewContext.TempData, writer);
            IView view = FindPartialView(newViewContext, partialViewName, viewEngineCollection);
            view.Render(newViewContext, writer);
        }


        private static IView FindPartialView(ViewContext viewContext, string partialViewName, ViewEngineCollection viewEngineCollection)
        {
            ViewEngineResult result = viewEngineCollection.FindPartialView(viewContext, partialViewName);
            if (result.View != null)
            {
                return result.View;
            }

            StringBuilder locationsText = new StringBuilder();
            foreach (string location in result.SearchedLocations)
            {
                locationsText.AppendLine();
                locationsText.Append(location);
            }

            throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, "FindPartialView error - partial view is null or empty", partialViewName, locationsText));
        }
        */


    }
}
