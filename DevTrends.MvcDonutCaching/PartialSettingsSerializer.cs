using DevTrends.MvcDonutCaching.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml;

namespace DevTrends.MvcDonutCaching
{
    public class PartialSettingsSerializer : IPartialSettingsSerializer
    {
        private readonly BinaryFormatter formatter = new BinaryFormatter();

        public PartialSettingsSerializer()
        {
            BinaryFormatter formatter = new BinaryFormatter();
        }

        public string Serialize(PartialSettings partialSettings)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                try
                {
                    formatter.Serialize(ms, partialSettings);
                }
                catch (Exception e)
                {
                    throw;
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public PartialSettings Deserialize(string serializedPartialSettings)
        {
            using (var memoryStream = new MemoryStream(Convert.FromBase64String(serializedPartialSettings)))
            {
                return (PartialSettings)formatter.Deserialize(memoryStream);
            }
        }
    }


    public class PartialSettingsDCSerializer : IPartialSettingsSerializer
    {
        private readonly DataContractSerializer _serialiser;

        public PartialSettingsDCSerializer(params Type[] cacheTypes)
        {
            _serialiser = new DataContractSerializer(typeof(PartialSettings), cacheTypes);
        }

        public string Serialize(PartialSettings partialSettings)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (XmlDictionaryWriter writer = System.Xml.XmlDictionaryWriter.CreateBinaryWriter(ms))
                {
                    try
                    {
                        _serialiser.WriteObject(writer, partialSettings);
                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                    writer.Flush();
                    return Convert.ToBase64String(ms.ToArray());
                }

                //try
                //{
                //    _serialiser.WriteObject(ms, partialSettings);
                //}
                //catch (Exception e)
                //{
                //    throw;
                //}
                //return Convert.ToBase64String(ms.ToArray());
            }
        }

        public PartialSettings Deserialize(string serializedPartialSettings)
        {
            using (var memoryStream = new MemoryStream(Convert.FromBase64String(serializedPartialSettings)))
            {
                using (var reader = XmlDictionaryReader.CreateBinaryReader(memoryStream, XmlDictionaryReaderQuotas.Max))
                {
                    return (PartialSettings)_serialiser.ReadObject(reader);
                }
                //return (PartialSettings)_serialiser.ReadObject(memoryStream);
            }
        }
    }
}
