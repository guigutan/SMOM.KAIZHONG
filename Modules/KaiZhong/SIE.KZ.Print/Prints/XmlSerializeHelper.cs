using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SIE.Common.Prints
{
    public class XmlSerializeHelper
    {
        // Token: 0x0600015E RID: 350 RVA: 0x000079CE File Offset: 0x00005BCE
        public static string Serialize<T>(T obj)
        {
            return XmlSerializeHelper.Serialize<T>(obj, Encoding.UTF8);
        }

        // Token: 0x0600015F RID: 351 RVA: 0x000079DC File Offset: 0x00005BDC
        public static string Serialize<T>(T obj, Encoding encoding)
        {
            string text;
            try
            {
                if (obj == null)
                {
                    throw new ArgumentNullException("obj");
                }
                XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, encoding))
                    {
                        xmlTextWriter.Formatting = Formatting.Indented;
                        xmlSerializer.Serialize(xmlTextWriter, obj);
                    }
                    text = encoding.GetString(memoryStream.ToArray()).Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "").Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return text;
        }

        // Token: 0x06000160 RID: 352 RVA: 0x00007AA0 File Offset: 0x00005CA0
        public static T DeSerialize<T>(string xml) where T : new()
        {
            return XmlSerializeHelper.DeSerialize<T>(xml, Encoding.UTF8);
        }

        // Token: 0x06000161 RID: 353 RVA: 0x00007AB0 File Offset: 0x00005CB0
        public static T DeSerialize<T>(string xml, Encoding encoding) where T : new()
        {
            T t;
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                using (MemoryStream memoryStream = new MemoryStream(encoding.GetBytes(xml)))
                {
                    using (StreamReader streamReader = new StreamReader(memoryStream, encoding))
                    {
                        t = (T)((object)xmlSerializer.Deserialize(streamReader));
                    }
                }
            }
            catch (Exception)
            {
                t = default(T);
            }
            return t;
        }
    }
}
