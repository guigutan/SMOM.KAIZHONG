using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Datas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace SIE.ERPInterface.Ebs.Connection
{
    /// <summary>
    /// Soap帮助类
    /// </summary>
    public static class SoapHelper
    {
        /// <summary>
        /// 获取SOAP配置
        /// </summary>
        /// <returns></returns>
        public static SoapParameter GetSoapParameter()
        {
            var SoapPara = new SoapParameter();
            SoapPara.Uri = RT.Config.Get<string>("ERP.Uri");
            SoapPara.User = RT.Config.Get<string>("ERP.User");
            SoapPara.Password = RT.Config.Get<string>("ERP.Password");
            SoapPara.Responsibility = RT.Config.Get<string>("ERP.Responsibility");
            SoapPara.RespApplication = RT.Config.Get<string>("ERP.RespApplication");
            SoapPara.SecurityGroup = RT.Config.Get<string>("ERP.SecurityGroup");
            SoapPara.NLSLanguage = RT.Config.Get<string>("ERP.NLSLanguage");
            SoapPara.P_ORIG_SYSTEM = RT.Config.Get<string>("ERP.P_ORIG_SYSTEM");

            return SoapPara;
        }

        /// <summary>
        /// POST ERP接口
        /// </summary>
        /// <param name="para"></param>
        /// <param name="isDownLoad"></param>
        /// <returns></returns>
        public static SoapResult ExecuteSoap(SoapParameter para, bool isDownLoad)
        {
            var result = new SoapResult();

            //构建请求报文
            var cdataStr = GenerateCDATA(para, isDownLoad);
            var requestStr = GenerateRequestStr(para, cdataStr);
            result.RequestStr = requestStr;

            byte[] byteArray = Encoding.UTF8.GetBytes(requestStr);

            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(para.Uri));
            webReq.Method = "POST";
            webReq.ContentType = "text/xml;charset=utf-8";
            webReq.Timeout = 1200000;//设置超时时间(批量处理，数据量大时，时间可能会很长)
            webReq.MediaType = "text/xml;charset=utf-8";

            try
            {
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);
                newStream.Close();

                result.RequestDate = DateTime.Now;  //记录请求时间
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                result.ResponseDate = DateTime.Now; //记录接收时间
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                var responseStr = sr.ReadToEnd();

                sr.Close();
                response.Close();

                AnalyzeResponse(result, responseStr);//解析接收报文
            }
            catch (Exception ex)
            {
                throw new ValidationException("ERP接口程序错误：{0}".L10nFormat(ex.GetBaseException().Message));
            }

            return result;
        }

        /// <summary>
        /// 解析接收报文
        /// </summary>
        /// <param name="result">结果</param>
        /// <param name="responseStr">接收报文</param>
        private static void AnalyzeResponse(SoapResult result, string responseStr)
        {
            //转换格式
            var xdoc = new XmlDocument();
            //记载xml字符串
            xdoc.LoadXml(responseStr);
            //解析xml文件
            XmlNode xmlStatus = xdoc.GetElementsByTagName("X_RESPONSE_STATUS")[0];
            XmlNode xmlCode = xdoc.GetElementsByTagName("X_RESPONSE_CODE")[0];
            XmlNode xmlMessage = xdoc.GetElementsByTagName("X_RESPONSE_MESSAGE")[0];
            //构建返回结果

            result.ResponseStr = responseStr;
            result.X_RESPONSE_STATUS = xmlStatus.InnerText;
            result.X_RESPONSE_CODE = xmlCode.InnerText;
            result.X_RESPONSE_MESSAGE = xmlMessage.InnerText;
            result.X_RESPONSE_DATA = xdoc.GetElementsByTagName("X_RESPONSE_DATA")[0].InnerText;
        }


        /// <summary>
        /// 构建SOAP报文
        /// </summary>
        /// <returns></returns>
        private static string GenerateRequestStr(SoapParameter para, string cdata)
        {
            //创建XML文档
            XmlDocument xmlDoc = new XmlDocument();
            var stream = RT.GetResourceStream("SoapXmlFile.xml");
            xmlDoc.Load(stream);

            #region Header

            //用户名
            XmlNode username = xmlDoc.GetElementsByTagName("wsse:Username")[0];
            username.InnerText = para.User;

            //密码
            XmlNode password = xmlDoc.GetElementsByTagName("wsse:Password")[0];
            password.InnerText = para.Password;

            //职责信息
            XmlNode responsibility = xmlDoc.GetElementsByTagName("cux:Responsibility")[0];
            responsibility.InnerText = para.RespApplication;

            //职责应用短码
            XmlNode respApplication = xmlDoc.GetElementsByTagName("cux:RespApplication")[0];
            respApplication.InnerText = para.RespApplication;

            //安全组
            XmlNode securityGroup = xmlDoc.GetElementsByTagName("cux:SecurityGroup")[0];
            securityGroup.InnerText = para.SecurityGroup;

            //语种
            XmlNode nlsLanguage = xmlDoc.GetElementsByTagName("cux:NLSLanguage")[0];
            nlsLanguage.InnerText = para.NLSLanguage;

            #endregion

            #region Body

            //调用接口名称
            XmlNode serviceName = xmlDoc.GetElementsByTagName("gat:P_SERVICE_NAME")[0];
            serviceName.InnerText = para.P_SERVICE_NAME;

            //调用系统名称
            XmlNode orgiSystem = xmlDoc.GetElementsByTagName("gat:P_ORIG_SYSTEM")[0];
            orgiSystem.InnerText = para.P_ORIG_SYSTEM;

            //ERP接口LOG唯一ID
            XmlNode batchId = xmlDoc.GetElementsByTagName("gat:P_BATCH_ID")[0];
            batchId.InnerText = para.P_BATCH_ID.ToString();

            #endregion

            #region Data

            //DATA XML
            XmlNode data = xmlDoc.GetElementsByTagName("gat:P_REQUEST_XML")[0];
            data.InnerText = cdata;

            #endregion

            return xmlDoc.InnerXml;
        }

        /// <summary>
        /// 构建CDATA
        /// </summary>
        /// <param name="soapPara"></param>
        /// <param name="isDownLoad"></param>
        /// <returns></returns>
        private static string GenerateCDATA<T>(T soapPara, bool isDownLoad) where T : SoapParameter
        {
            var xmlDataStr = isDownLoad ? GenerateDownloadXMLData(soapPara.SoapDownloadParameter) : GenerateUploadXMLData(soapPara.SoapUploadParameterDtlList);
            StringBuilder cdataStr = new StringBuilder();
            cdataStr.Append("<![CDATA[{0}]]>".FormatArgs(xmlDataStr));

            return cdataStr.ToString();
        }

        /// <summary>
        /// 构建下载XML Data
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        private static string GenerateDownloadXMLData(SoapDownloadParameter para)
        {
            //  创建XML文档
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.AppendChild(dec);
            //  创建根结点
            XmlElement xeBody = doc.CreateElement("BODY");
            doc.AppendChild(xeBody);

            //创建信息节点
            XmlElement xeHeader = doc.CreateElement("HEADER");
            xeBody.AppendChild(xeHeader);

            XmlElement xeCode = doc.CreateElement(para.Code_Label);
            xeCode.InnerText = para.Code;
            xeHeader.AppendChild(xeCode);

            XmlElement xeOrg = doc.CreateElement("ORGANIZATION_ID");
            xeOrg.InnerText = RT.InvOrg.ToString();
            xeHeader.AppendChild(xeOrg);

            XmlElement xeDateTo = doc.CreateElement("LAST_UPDATE_DATE_TO");
            xeDateTo.InnerText = para.LAST_UPDATE_DATE_TO;
            xeHeader.AppendChild(xeDateTo);

            XmlElement xeDateFrom = doc.CreateElement("LAST_UPDATE_DATE_FROM");
            xeDateFrom.InnerText = para.LAST_UPDATE_DATE_FROM;
            xeHeader.AppendChild(xeDateFrom);

            return doc.InnerXml;
        }

        /// <summary>
        /// 构建上传XML Data
        /// </summary>
        /// <param name="dtlList"></param>
        /// <returns></returns>
        private static string GenerateUploadXMLData(List<SoapUploadParameterDtl> dtlList)
        {
            //创建XML文档
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.AppendChild(dec);
            //创建根结点
            XmlElement xeBody = doc.CreateElement("BODY");
            doc.AppendChild(xeBody);

            //创建信息节点
            XmlElement xeHeader = doc.CreateElement("HEADER");
            xeBody.AppendChild(xeHeader);

            foreach (var dtl in dtlList)
            {
                XmlElement xeLine = doc.CreateElement("LINE");
                xeHeader.AppendChild(xeLine);

                XmlElement xeLineId = doc.CreateElement("LINE_SEQUENCE");
                xeLineId.InnerText = dtl.LINE_ID;
                xeLine.AppendChild(xeLineId);

                XmlElement xeWoNo = doc.CreateElement("WIP_ENTITY_NAME");
                xeWoNo.InnerText = dtl.WIP_ENTITY_NAME;
                xeLine.AppendChild(xeWoNo);

                XmlElement xeOrg = doc.CreateElement("ORGANIZATION_ID");
                xeOrg.InnerText = RT.InvOrg.ToString();
                xeLine.AppendChild(xeOrg);

                XmlElement xeQty = doc.CreateElement("MOVE_QUANTITY");
                xeQty.InnerText = dtl.MOVE_QUANTITY;
                xeLine.AppendChild(xeQty);

                XmlElement xeType = doc.CreateElement("MOVE_TYPE");
                xeType.InnerText = dtl.MOVE_TYPE;
                xeLine.AppendChild(xeType);
            }

            return doc.InnerXml;
        }
    }
}
