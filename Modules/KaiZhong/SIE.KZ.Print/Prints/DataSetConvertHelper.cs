using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;

namespace SIE.Common.Prints
{
    public class DataSetConvertHelper
    {
        // Token: 0x0600016E RID: 366 RVA: 0x00007C5C File Offset: 0x00005E5C
        public static DataSet ConvertXMLFileToDataSet(string xmlFile)
        {
            XmlTextReader xmlTextReader = null;
            DataSet dataSet2;
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(xmlFile);
                DataSet dataSet = new DataSet();
                xmlTextReader = new XmlTextReader(new StringReader(xmlDocument.InnerXml));
                dataSet.ReadXml(xmlTextReader);
                dataSet2 = dataSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (xmlTextReader != null)
                {
                    xmlTextReader.Close();
                }
            }
            return dataSet2;
        }

        // Token: 0x0600016F RID: 367 RVA: 0x00007CC4 File Offset: 0x00005EC4
        public static void ConvertDataSetToXMLFile(DataSet xmlDS, string xmlFile)
        {
            MemoryStream memoryStream = null;
            XmlTextWriter xmlTextWriter = null;
            byte[] array = null;
            try
            {
                using (FileStream fs = new FileStream(xmlFile, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>".Trim());
                        xmlDS.WriteXml(writer, XmlWriteMode.WriteSchema); // 或者 XmlWriteMode.IgnoreSchema 根据需要选择模式
                    }
                }

                //using (memoryStream = new MemoryStream())
                //{
                //    using (xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8))
                //    {
                //        xmlDS.WriteXml(xmlTextWriter, XmlWriteMode.WriteSchema);
                //        //int num = (int)memoryStream.Length;
                //        //array = new byte[num];
                //        //memoryStream.Seek(0L, SeekOrigin.Begin);
                //        //memoryStream.Read(array, 0, num);
                //        //memoryStream.Flush();
                //        //memoryStream.Position = 0;

                //        array = memoryStream.ToArray();

                //        //Thread.Sleep(200);
                //        using (var streamWriter = new StreamWriter(xmlFile))
                //        {
                //            streamWriter.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>".Trim());
                //            //streamWriter.WriteLine(Encoding.UTF8.GetString(array).TrimStart('\ufeff'));
                //        }
                //    }
                //}

                //Thread.Sleep(100);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                xmlTextWriter?.Close();
            }
        }
    }
}
