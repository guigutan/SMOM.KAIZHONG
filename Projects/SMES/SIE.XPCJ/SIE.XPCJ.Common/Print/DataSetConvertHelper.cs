using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace SIE.XPCJ.Common.Print
{
    /// <summary>
    /// 数据集转换帮助类
    /// </summary>
    public class DataSetConvertHelper
    {
        /// <summary>
        /// 将xml文件转换为DataSet
        /// </summary>
        /// <param name="xmlFile">xml文件路径</param>
        /// <returns>数据集</returns>
        public static DataSet ConvertXMLFileToDataSet(string xmlFile)
        {
            StringReader stream = null;
            XmlTextReader reader = null;
            try
            {
                XmlDocument xmld = new XmlDocument();
                xmld.Load(xmlFile);

                DataSet xmlDS = new DataSet();
                stream = new StringReader(xmld.InnerXml);
                reader = new XmlTextReader(stream);
                xmlDS.ReadXml(reader);
                return xmlDS;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        /// <summary>
        /// 将DataSet转换为xml文件
        /// </summary>
        /// <param name="xmlDS">数据集</param>
        /// <param name="xmlFile">xml文件名</param>
        public static void ConvertDataSetToXMLFile(DataSet xmlDS, string xmlFile)
        {
            MemoryStream stream = null;
            XmlTextWriter writer = null;
            byte[] arr = null;
            try
            {
                using (stream = new MemoryStream())
                {
                    //从stream装载到XmlTextReader
                    using (writer = new XmlTextWriter(stream, Encoding.UTF8))
                    {
                        //用WriteXml方法写入文件.
                        xmlDS.WriteXml(writer, XmlWriteMode.WriteSchema);
                        int count = (int)stream.Length;
                        arr = new byte[count];
                        stream.Seek(0, SeekOrigin.Begin);
                        stream.Read(arr, 0, count);
                    }
                }

                using (StreamWriter sw = new StreamWriter(xmlFile))
                {
                    sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>".Trim());
                    sw.WriteLine(Encoding.UTF8.GetString(arr).TrimStart('﻿'));
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }
    }
}
