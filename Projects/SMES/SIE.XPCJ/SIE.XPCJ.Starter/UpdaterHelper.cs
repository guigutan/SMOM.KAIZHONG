using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace SIE.XPCJ.Starter
{
    public class UpdaterHelper
    {
        /// <summary>
        /// 用来进行UI线程访问
        /// </summary>
        private delegate void OnApicallbakDelegate();

        /// <summary>
        /// 异步调用的回调委托声明
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="apiType"></param>
        /// <param name="method"></param>
        /// <param name="postData"></param>
        public delegate void delegateApiCallback<T>(UpdaterResult<T> result, string apiType, string method, string postData);



        public static void GetUrlContent(Control sender, string url, delegateApiCallback<string> onApiCallback)
        {
            try
            {
                string actUrl = url;
                if (!actUrl.StartsWith("http://") && !actUrl.StartsWith("https://"))
                    actUrl = "http://" + actUrl;
                var wbRequest = (HttpWebRequest)WebRequest.Create(actUrl);
                wbRequest.Timeout = 30000;
                WebResponse response = wbRequest.GetResponse();

                // 打开流读取响应数据
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(responseStream, Encoding.UTF8))
                    {

                        // 读取文本内容
                        string res = reader.ReadToEnd();
                        UpdaterResult<string> apiResult = new UpdaterResult<string>();

                        if (res.IndexOf("<body>Your browse does not support frame!</body>") >= 0)
                        {
                            apiResult.Success = false;
                            apiResult.Message = "Your browse does not support frame!";
                        }
                        else
                        {
                            apiResult.Success = true;
                            apiResult.Result = res;
                        }
                        if (sender != null)
                        {
                            sender.Invoke(new OnApicallbakDelegate(() => { onApiCallback(apiResult, "", "", ""); }));
                        }
                        else
                        {
                            onApiCallback(apiResult, "", "", "");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UpdaterResult<string> apiResult = new UpdaterResult<string>();
                apiResult.Success = false;
                apiResult.Message = ex.Message;
                onApiCallback(apiResult, "", "", "");
            }
        }

        /// <summary>
        /// 异步Post Url
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="url"></param>
        /// <param name="onApiCallback"></param>
        public static void PostUrlAsync(Control sender, string url, delegateApiCallback<string> onApiCallback)
        {
            string actUrl = url;
            if (!actUrl.StartsWith("http://") && !actUrl.StartsWith("https://"))
                actUrl = "http://" + actUrl;

            byte[] byteArray = Encoding.UTF8.GetBytes("");
            var wbRequest = (HttpWebRequest)WebRequest.Create(actUrl);
            wbRequest.Timeout = 30000;

            wbRequest.BeginGetRequestStream(ar =>
            {
                try
                {
                    using (Stream requestStream = wbRequest.EndGetRequestStream(ar))
                    {
                        requestStream.Write(byteArray, 0, byteArray.Length);
                    }

                    // 发起异步响应请求
                    wbRequest.BeginGetResponse(responseResult =>
                    {
                        try
                        {
                            using (HttpWebResponse response = (HttpWebResponse)wbRequest.EndGetResponse(responseResult))
                            using (Stream responseStream = response.GetResponseStream())
                            using (StreamReader reader = new StreamReader(responseStream))
                            {
                                string res = reader.ReadToEnd();
                                UpdaterResult<string> apiResult = new UpdaterResult<string>();

                                if (res.IndexOf("<body>Your browse does not support frame!</body>") >= 0)
                                {
                                    apiResult.Success = false;
                                    apiResult.Message = "Your browse does not support frame!";
                                }
                                else
                                {
                                    apiResult.Success = true;
                                    apiResult.Result = res;
                                }
                                if (sender != null)
                                {
                                    sender.Invoke(new OnApicallbakDelegate(() => { onApiCallback(apiResult, "", "", ""); }));
                                }
                                else
                                {
                                    onApiCallback(apiResult, "", "", "");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            UpdaterResult<string> apiResult = new UpdaterResult<string>();
                            apiResult.Success = false;
                            apiResult.Message = ex.Message;
                            onApiCallback(apiResult, "", "", "");
                        }
                    }, null);
                }
                catch (Exception ex)
                {
                    UpdaterResult<string> apiResult = new UpdaterResult<string>();
                    apiResult.Success = false;
                    apiResult.Message = ex.Message;
                    onApiCallback(apiResult, "", "", "");
                }
            }, null);
        }

        public static void DowloadFile(Control sender, string fileUrl, string savePath, delegateApiCallback<string> onApiCallback)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(fileUrl);

            // 发送异步请求
            try
            {
                request.BeginGetResponse(result =>
                {
                    try
                    {
                        using (HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result))
                        {
                            using (Stream responseStream = response.GetResponseStream())
                            {
                                using (FileStream fileStream = File.Create(savePath))
                                {
                                    byte[] buffer = new byte[4096];
                                    int bytesRead;
                                    while ((bytesRead = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                                    {
                                        fileStream.Write(buffer, 0, bytesRead);
                                    }
                                }
                            }
                        }
                        UpdaterResult<string> apiResult = new UpdaterResult<string>();
                        apiResult.Success = true;
                        onApiCallback(apiResult, "", "", "");
                    }
                    catch (Exception ex)
                    {
                        UpdaterResult<string> apiResult = new UpdaterResult<string>();
                        apiResult.Success = false;
                        apiResult.Message = ex.Message;
                        onApiCallback(apiResult, "", "", "");
                    }
                },
            null);
            }
            catch (Exception ex)
            {
                UpdaterResult<string> apiResult = new UpdaterResult<string>();
                apiResult.Success = false;
                apiResult.Message = ex.Message;
                onApiCallback(apiResult, "", "", "");
            }
        }
    }
}
