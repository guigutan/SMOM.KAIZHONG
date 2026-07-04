using Newtonsoft.Json;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Datas;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace SIE.ERPInterface.Sap.Connection
{
    /// <summary>
    /// Http协议接口帮助类
    /// </summary>
    public class HttpHelper : DomainController
    {
        /// <summary>
        /// 上传地址
        /// </summary>
        public static string Url = AppRuntime.Config.Get("SAP.Upload.Url");
        /// <summary>
        /// 登录账号
        /// </summary>
        public static string AuthUser = AppRuntime.Config.Get("SAP.AuthUser");
        /// <summary>
        /// 登录密码
        /// </summary>
        public static string AuthPassword = AppRuntime.Config.Get("SAP.AuthPassword");
        /// <summary>
        /// 超时设置分钟
        /// </summary>
        public static string TimeOutMin = AppRuntime.Config.Get("SAP.TimeOutMin");

        public virtual SapResult InvokeSapAPI(string interfaceName, string json, Dictionary<string, string> headers = null)
        { 
                        //1 获取sap连接定义           
            if (Url.IsNullOrEmpty())
                throw new ValidationException("SAP接口地址未配置,SAP.Upload.Url没有配置值".L10N());
            var uri = Path.Combine(Url, interfaceName);
            //2 保存上传数据日志
            //string json = JsonConvert.SerializeObject(inParams, this.JsonSerializerSettings());
            SapResult sapResult = new SapResult()
            {
                RequestStr = json,
                RequestDate = DateTime.Now,
            };
            //3 调用http接口
            //验证服务器证书的有效性。
            //在这个代码片段中，回调函数始终返回true，即始终接受服务器证书，无论其有效性如何。
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            string user = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", AuthUser, AuthPassword)));

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "POST";
            request.Accept = "*/*";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Basic " + user);
            if (headers != null && headers.Any())
            {
                headers.ForEach(f =>
                {
                    request.Headers.Add(f.Key, f.Value);
                });
            }
            if (int.TryParse(TimeOutMin, out int timeMin))
                request.Timeout = 1000 * 60 * timeMin;
            else
                request.Timeout = 1000 * 60 * 5;//设置5分钟
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string outJson = reader.ReadToEnd();
                    sapResult.ResponseStr = outJson;
                }
            }
            catch (Exception ex)
            {
                sapResult.IsSuccess = false;
                sapResult.ResponseStr = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return sapResult;

        }

        /// <summary>
        /// 调用SAP的HTTP POST接口
        /// </summary>
        /// <typeparam name="IN">输入泛型，包含参数与表</typeparam>
        /// <param name="interfaceName">接口名称</param>      
        /// <param name="inParams"></param>
        /// <param name="headers">头部参数</param>    
        public virtual SapResult InvokeSapAPI<IN>(string interfaceName, IN inParams, Dictionary<string, string> headers = null)
        {
            //1 获取sap连接定义           
            if (Url.IsNullOrEmpty())
                throw new ValidationException("SAP接口地址未配置,SAP.Upload.Url没有配置值".L10N());
            var uri = Path.Combine(Url, interfaceName);
            //2 保存上传数据日志
            string json = JsonConvert.SerializeObject(inParams, this.JsonSerializerSettings());
            SapResult sapResult = new SapResult()
            {
                RequestStr = json,
                RequestDate = DateTime.Now,
            };
            //3 调用http接口
            //验证服务器证书的有效性。
            //在这个代码片段中，回调函数始终返回true，即始终接受服务器证书，无论其有效性如何。
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            string user = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", AuthUser, AuthPassword)));

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "POST";
            request.Accept = "*/*";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Basic " + user);
            if (headers != null && headers.Any())
            {
                headers.ForEach(f =>
                {
                    request.Headers.Add(f.Key, f.Value);
                });
            }
            if (int.TryParse(TimeOutMin, out int timeMin))
                request.Timeout = 1000 * 60 * timeMin;
            else
                request.Timeout = 1000 * 60 * 5;//设置5分钟
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {

                    string outJson = reader.ReadToEnd();
                    sapResult.ResponseStr = outJson;
                    sapResult.SapUploadResultData = DeserializeObject<SapUploadResultData>(outJson);
                    sapResult.IsSuccess = sapResult.SapUploadResultData.type.ToUpper() == "S";
                }
            }
            catch (Exception ex)
            {
                sapResult.IsSuccess = false;
                sapResult.ResponseStr = ex.Message;
            }
            return sapResult;
        }

        /// <summary>
        /// 序列化
        /// </summary>

        private readonly Func<JsonSerializerSettings> JsonSerializerSettings = () =>
        {
            var st = new JsonSerializerSettings();
            st.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            //st.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            //st.DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind; //忽略时区
            st.Formatting = Newtonsoft.Json.Formatting.Indented; //格式化
            st.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; //忽略循环引用
            st.NullValueHandling = NullValueHandling.Ignore; //忽略空值
            return st;
        };

        /// <summary>
        /// 序列化处理
        /// </summary>      
        internal T DeserializeObject<T>(string value)
        {
            Check.NotNullOrEmpty(value, nameof(value));
            return JsonConvert.DeserializeObject<T>(value, this.JsonSerializerSettings());
        }


        /// <summary>
        /// 调用SAP的HTTP POST接口获取SAP数据
        /// </summary>
        /// <typeparam name="IN">输入泛型，包含参数与表</typeparam>      
        /// <typeparam name="OUT">输出泛型</typeparam>
        /// <param name="interfaceName">接口名称</param>  
        /// <param name="inParams"></param>
        /// <param name="result">输出结果</param>
        /// <param name="headers">头部参数</param>    
        public virtual string DownLoadSapAPI<IN, OUT>(string interfaceName, IN inParams, ref OUT result, Dictionary<string, string> headers = null)
        {
            //1 获取sap连接定义           
            if (Url.IsNullOrEmpty())
                throw new ValidationException("SAP接口地址未配置,请设置SAP.Upload.Url没有配置值".L10N());
            var uri = Path.Combine(Url, interfaceName);
            //2 保存上传数据日志
            string json = JsonConvert.SerializeObject(inParams, this.JsonSerializerSettings());

            //3 调用http接口
            //验证服务器证书的有效性。
            //在这个代码片段中，回调函数始终返回true，即始终接受服务器证书，无论其有效性如何。
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            string user = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", AuthUser, AuthPassword)));

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "POST";
            request.Accept = "*/*";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Basic " + user);
            if (headers != null && headers.Any())
            {
                headers.ForEach(f =>
                {
                    request.Headers.Add(f.Key, f.Value);
                });
            }
            if (int.TryParse(TimeOutMin, out int timeMin))
                request.Timeout = 1000 * 60 * timeMin;
            else
                request.Timeout = 1000 * 60 * 5;//设置5分钟
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string outJson = reader.ReadToEnd();
                    result = DeserializeObject<OUT>(outJson);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return string.Empty;
        }
    }
}
