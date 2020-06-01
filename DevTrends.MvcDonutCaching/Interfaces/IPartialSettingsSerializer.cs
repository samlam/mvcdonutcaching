using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTrends.MvcDonutCaching.Interfaces
{
    public interface IPartialSettingsSerializer
    {
        /// <summary>
        /// Implementations should serialize as string the specified action settings.
        /// </summary>
        /// <param name="partialSettings">The action settings.</param>
        /// <returns>A string representing the given <see cref="partialSettings"/></returns>
        string Serialize(PartialSettings partialSettings);

        /// <summary>
        /// Implementations should deserializes the specified serialized action settings.
        /// </summary>
        /// <param name="serializedPartialSettings">The serialized action settings.</param>
        /// <returns>An <see cref="PartialSettings"/> object</returns>
        PartialSettings Deserialize(string serializedPartialSettings);
    }
}
