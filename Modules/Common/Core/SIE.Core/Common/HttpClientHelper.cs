using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Core.Common
{
    /// <summary>
    /// Http请求帮助类
    /// </summary>
    public static class HttpClientHelper
    {
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">链接</param>
        /// <param name="parameters">参数</param>
        /// <param name="contentType">内容类型</param>
        /// <returns></returns>
        public static async Task<string> PostAsync(string url, string parameters, string contentType = "application/json")
        {
            using (var client = new HttpClient())
            {
                HttpContent httpContent = new StringContent(parameters);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                HttpResponseMessage resp = await client.PostAsync(url, httpContent);
                return await resp.Content.ReadAsStringAsync();
            }
        }

        /// <summary>
        /// POST请求（异步）
        /// </summary>
        /// <param name="url">链接</param>
        /// <param name="parameters">参数</param>
        /// <param name="contentType">内容类型</param>
        /// <returns></returns>
        public static async Task<string> Post(string url, string parameters, string contentType = "application/json")
        {
            using (var client = new HttpClient())
            {
                HttpContent httpContent = new StringContent(parameters);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                HttpResponseMessage resp = await client.PostAsync(url, httpContent);
                return await resp.Content.ReadAsStringAsync();
            }
        }

        /// <summary>
        /// POST请求（同步）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static string HttpPost(string url, string body, string contentType = "application/json")
        {
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = contentType;
            request.Timeout = 600000;

            byte[] buffer = encoding.GetBytes(body);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// POST方法--带Headers
        /// </summary>
        /// <param name="posturl"></param>
        /// <param name="requestStr"></param>
        /// <returns></returns>
        public static string HttpPostWithTocken(string posturl, string requestStr, Dictionary<string, string> headers, string contentType = "application/json")
        {
            string responseStr = string.Empty;

            byte[] byteArray = Encoding.UTF8.GetBytes(requestStr);
            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(posturl));
            webReq.Method = "POST";
            webReq.ContentType = contentType;
            webReq.Timeout = 1200000;//设置超时时间(批量处理，数据量大时，时间可能会很长)
            webReq.MediaType = "text/xml;charset=utf-8";
            webReq.Headers[HttpRequestHeader.CacheControl] = "no-cache";
            //添加Authorization到HTTP头
            foreach (var key in headers.Keys)
            {
                webReq.Headers.Add(key, headers[key]);
            }
            try
            {
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);
                newStream.Close();

                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                responseStr = sr.ReadToEnd();

                sr.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                responseStr = ex.Message;
            }
            finally
            {

                if (webReq != null)
                {
                    webReq.Abort();
                }
            }
            return responseStr;
        }

        /// <summary>
        /// POST方法--application/x-www-form-urlencoded
        /// </summary>
        /// <param name="posturl"></param>
        /// <param name="requestDic"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static string HttpPostWithUrlencoded(string posturl, Dictionary<string, string> requestDic, Dictionary<string, string> headers)
        {

            string result = "";
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(posturl);
                req.Method = "POST";
                req.Timeout = 12000;//请求超时时间
                //添加Authorization到HTTP头
                foreach (var key in headers.Keys)
                {
                    req.Headers.Add(key, headers[key]);
                }
                var buff = new StringBuilder(string.Empty);
                var postData = "";
                foreach (var key in requestDic.Keys)
                {
                    buff.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(requestDic[key] + "") + "&");
                }
                postData = buff.ToString().Trim('&');
                byte[] data = Encoding.UTF8.GetBytes(postData);
                req.ContentLength = data.Length;
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);
                    reqStream.Close();
                }
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                Stream stream = resp.GetResponseStream();
                //获取响应内容
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;

        }



        /// <summary>
        /// 使用multipart/form-data方式上传文件及其他数据
        /// </summary>
        /// <param name="requestUrl">请求接口地址</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="fileContent">文件字节</param>
        /// <returns></returns>
        public static string PostMultipartFormDataAsync(string requestUrl, string fileName, byte[] fileContent)
        {
            using (var client = new HttpClient())
            {
                using (var formContent = new MultipartFormDataContent())
                {
                    var content = new ByteArrayContent(fileContent);
                    content.Headers.Add("Content-Type", "application/octet-stream");

                    formContent.Add(content, "file", fileName);

                    var response = client.PostAsync(requestUrl, formContent).Result;
                    return response.Content.ReadAsStringAsync().Result;
                }
            }
        }

        /// <summary>
        /// get请求,无参
        /// </summary>
        /// <param name="url">请求链接</param>
        /// <returns></returns>
        public static async Task<string> GetAsync(string url, Dictionary<string, string> headers)
        {
            HttpClient client = new HttpClient();
            foreach (var key in headers.Keys)
            {
                client.DefaultRequestHeaders.Add(key, headers[key]);
            }
            HttpResponseMessage resp = await client.GetAsync(url);
            //获取响应状态
            //respMsg.StatusCode==200请求成功
            //获取请求内容
            HttpContent respContent = resp.Content;
            return await respContent.ReadAsStringAsync();
        }

        /// <summary>
        /// Get 请求，指定参数
        /// </summary>
        /// <param name="url">请求链接</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public static async Task<string> GetAsync(string url, Dictionary<string, string> headers, Dictionary<string, string> param)
        {
            //参数处理
            StringBuilder builder = new StringBuilder();
            builder.Append(url);
            if (param.Count > 0)
            {
                builder.Append("?");
                int i = 0;
                foreach (var item in param)
                {
                    if (i > 0)
                        builder.Append("&");
                    builder.AppendFormat("{0}={1}", item.Key, item.Value);
                    i++;
                }
            }
            return await GetAsync(builder.ToString(), headers);
        }

    }
}
