using Newtonsoft.Json;
using SIE.XPCJ.Common.Log;
using SIE.XPCJ.Common.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIE.XPCJ.Common.ApiCall
{
    public class ApiHelper
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
        public delegate void delegateApiCallback<T>(ApiResult<T> result, string apiType, string method, string postData);

        /// <summary>
        /// 异步Get调用回调委托声明
        /// </summary>
        /// <param name="result"></param>
        public delegate void delegateApiGetCallback(bool isOk, string result, string url);

        /// <summary>
        /// 登录，这个是异步调用
        /// </summary>
        /// <param name="onApiCallback"></param>
        public static void Login(Control sender, delegateApiCallback<LoginInfo> onApiCallback)
        {
            delegateApiCallback<LoginInfo> callback = new delegateApiCallback<LoginInfo>((result, apiType, method, postData)=> {
                if (result.Success)
                {
                    LoginInfo.Instance.UserId = result.Result.UserId;
                    LoginInfo.Instance.UserName = result.Result.UserName;
                    LoginInfo.Instance.EmployeeId = result.Result.EmployeeId;
                    LoginInfo.Instance.InvOrgId = double.Parse(result.Context.InvOrgId);
                    LoginInfo.Instance.Ticket = result.Context.Ticket;
                    LoginInfo.Instance.Context = "{'Ticket':'" + result.Context.Ticket + "','InvOrgId':'" + result.Context.InvOrgId.ToString()
                        + "','Language':'" + (Global.Language == null ? "zh-CN" : Global.Language.Code) + "'}";
                }
                else
                {
                    LoginInfo.Instance.UserId = 0;
                    LoginInfo.Instance.UserName = result.Message;
                }

                if (sender != null)
                {
                    sender.Invoke(new OnApicallbakDelegate(() => { onApiCallback(result, apiType, method, postData); }));
                }
                else
                {
                    onApiCallback(result, apiType, method, postData);
                }
            });
            PostAsync<LoginInfo>(sender, "AuthenticationController", "Login", callback, LoginInfo.Instance.UserCode, LoginInfo.Instance.Password);
        }

        private static string ReplaceTicket(string postData, string newTicket)
        {
            postData = "{'ApiType':'WinFormReworkApiController','Method':'GetCurrentInfo','Parameters':[{\"Value\":18.0},{\"Value\":1253.0},{\"Value\":1741.0},{\"Value\":19887.0}],'Context':{'Ticket':'wGPhvJP7/m+C0b8SeKHXFyYK5iCIZfTqlwhPqe+W4TA7Nv4mJInGiQFMJIDSTucM','InvOrgId':'100'}}";
            int startIndex = postData.IndexOf("'Ticket':'");
            int endIndex = postData.IndexOf("','InvOrgId':");

            string pre = postData.Substring(0, startIndex + 10);
            string last = postData.Substring(endIndex);

            return pre + newTicket + last;
        }

        private static ApiResult<T> ReLogin<T>(string preUrl, string prePostData, ApiResult<T> preResult)
        {
            var loginResult = Post<LoginInfo>("AuthenticationController", "Login", LoginInfo.Instance.UserCode, LoginInfo.Instance.Password);

            if (loginResult.Success)
            {
                LoginInfo.Instance.UserId = loginResult.Result.UserId;
                LoginInfo.Instance.UserName = loginResult.Result.UserName;
                LoginInfo.Instance.EmployeeId = loginResult.Result.EmployeeId;
                LoginInfo.Instance.InvOrgId = loginResult.Result.InvOrgId;
                LoginInfo.Instance.Ticket = loginResult.Context.Ticket;
                LoginInfo.Instance.Context = "{'Ticket':'" + loginResult.Context.Ticket + "','InvOrgId':'" + loginResult.Context.InvOrgId.ToString()
                   + "','Language':'" + (Global.Language == null ? "zh-CN" : Global.Language.Code) + "'}";

                prePostData = ReplaceTicket(prePostData, loginResult.Context.Ticket);
            }
            else
            {
                LoginInfo.Instance.UserId = 0;
                LoginInfo.Instance.UserName = loginResult.Message;
                return preResult;
            }

            string res = "";
            HttpWebResponse wbResponse = _Post(preUrl, "重连后重新提交", "重连后重新提交", prePostData, AppSettings.Instance.ApiTimeOut, contentType: "application/json", log: AppSettings.Instance.ApiLog);
            using (Stream responseStream = wbResponse.GetResponseStream())
            {
                using (var sread = new StreamReader(responseStream))
                {
                    res = sread.ReadToEnd();
                }
            }
            res = preDeserialize<T>(res);
            var result = JsonConvert.DeserializeObject<ApiResult<T>>(res);
            if (loginResult.Success && !string.IsNullOrEmpty(loginResult.Context.Ticket) && LoginInfo.Instance.Ticket != loginResult.Context.Ticket)
            {
                LoginInfo.Instance.Context = "{'Ticket':'" + loginResult.Context.Ticket + "','InvOrgId':'" + loginResult.Context.InvOrgId.ToString()
                    + "','Language':'" + (Global.Language == null ? "zh-CN" : Global.Language.Code) + "'}";
            }
            return result;
        }

        /// <summary>
        /// 同步调用SMOM API
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiType"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static ApiResult<T> Post<T>(string apiType, string method, params object[] parameters)
        {
            StringBuilder sbParameters = new StringBuilder();
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    //sbParameters.Append("{'Value':'" + parameters[i] + "'},"); //这里有问题，如果 parameters[i] 是List或者其他类似的类型
                    sbParameters.Append($"{ApiParameter.FromParam(parameters[i])},");
                }
                sbParameters.Remove(sbParameters.Length - 1, 1);
            }

            string postData =
                "{" +
                    "'ApiType':'" + apiType + "'," +
                    "'Method':'" + method + "'," +
                    "'Parameters':[" + sbParameters.ToString() + "]," +
                    "'Context':" + LoginInfo.Instance.Context +
                 "}";

            int radInt = new Random().Next(int.MaxValue);
            string url = "http://" + AppSettings.Instance.ApiUrl + "/api/dataportal/" + "invoke?n=" + radInt;

            string res = "";
            HttpWebResponse wbResponse = _Post(url, apiType, method, postData, AppSettings.Instance.ApiTimeOut, contentType: "application/json", log: AppSettings.Instance.ApiLog);
            using (Stream responseStream = wbResponse.GetResponseStream())
            {
                using (var sread = new StreamReader(responseStream))
                {
                    res = sread.ReadToEnd();
                }
            }

            res = preDeserialize<T>(res);

            var result = JsonConvert.DeserializeObject<ApiResult<T>>(res);


            if (!result.Success && result.Message?.IndexOf("Ticket无效") >= 0 && method != "Login")
            {
                return ReLogin(url, postData, result); //断线重联
            }
            if (result.Success && !string.IsNullOrEmpty(result.Context.Ticket) && LoginInfo.Instance.Ticket != result.Context.Ticket)
            {
                LoginInfo.Instance.Context = "{'Ticket':'" + result.Context.Ticket + "','InvOrgId':'" + result.Context.InvOrgId.ToString()
                    + "','Language':'" + (Global.Language == null ? "zh-CN" : Global.Language.Code) + "'}";
            }
            return result;
        }

        /// <summary>
        /// 异步调用SOM API
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiType"></param>
        /// <param name="method"></param>
        /// <param name="onApiCallback"></param>
        /// <param name="parameters"></param>
        public static void PostAsync<T>(Control sender, string apiType, string method, delegateApiCallback<T> onApiCallback, params object[] parameters)
        {
            StringBuilder sbParameters = new StringBuilder();
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    //sbParameters.Append("{'Value':'" + parameters[i] + "'},"); //这里有问题，如果 parameters[i] 是List或者其他类似的类型
                    sbParameters.Append($"{ApiParameter.FromParam(parameters[i])},");
                }
                sbParameters.Remove(sbParameters.Length - 1, 1);
            }

            string postData =
                "{" +
                    "'ApiType':'" + apiType + "'," +
                    "'Method':'" + method + "'," +
                    "'Parameters':[" + sbParameters.ToString() + "]," +
                    "'Context':" + LoginInfo.Instance.Context +
                 "}";

            int radInt = new Random().Next(int.MaxValue);
            string url = "http://" + AppSettings.Instance.ApiUrl + "/api/dataportal/" + "invoke?n=" + radInt;

            _PostAsync(sender, url, apiType, method, postData, onApiCallback, AppSettings.Instance.ApiTimeOut, contentType: "application/json", log: AppSettings.Instance.ApiLog);
        }

        #region 私有方法
        private static string preDeserialize<T>(string res)
        {
            //{"Success":false,"Message":"密码错误.","Result":null,"Context":{}}
            //处理当web api调用失败的时候，Result:null 时，反序列化失败
            if (res?.IndexOf("\"Success\":false,") >= 0)
            {
                if (typeof(T) == typeof(int) || typeof(T) == typeof(long) || typeof(T) == typeof(double)
                        || typeof(T) == typeof(float))
                {
                    res = res.Replace("\"Result\":null,", $"\"Result\":{int.MinValue},");
                }
                else if (typeof(T) == typeof(bool))
                {
                    res = res.Replace("\"Result\":null,", "\"Result\":false,");
                }
                else if (typeof(T) == typeof(DateTime))
                {
                    res = res.Replace("\"Result\":null,", $"\"Result\":{DateTime.MinValue},");
                }
            }

            if (res?.IndexOf("\"Success\":true,") >= 0)
            {
                if (typeof(T) == typeof(int) || typeof(T) == typeof(long) || typeof(T) == typeof(double)
                        || typeof(T) == typeof(float))
                {
                    res = res.Replace(":null,", $":{int.MinValue},");
                }
                else if (typeof(T) == typeof(bool))
                {
                    res = res.Replace(":null,", ":false,");
                }
                else if (typeof(T) == typeof(DateTime))
                {
                    res = res.Replace(":null,", $":{DateTime.MinValue},");
                }
            }
            return res;
        }

        private static HttpWebResponse _Post(string url, string apiType, string method, string data, int timeout = 30000, Dictionary<string, string> headerDic = null, string contentType = "application/x-www-form-urlencoded", bool log=true)
        {
            if (log)
            {
                LogingHelper.Debug("PostWebApi", apiType, method, url, data);
            }
            byte[] byteArray = Encoding.UTF8.GetBytes(data);
            var wbRequest = (HttpWebRequest)WebRequest.Create(url);
            wbRequest.Timeout = timeout;
            wbRequest.Method = "POST";
            wbRequest.ContentType = contentType;
            wbRequest.ContentLength = byteArray.Length;
            if (headerDic != null && headerDic.Count > 0)
            {
                foreach (var item in headerDic)
                {
                    wbRequest.Headers.Add(item.Key, item.Value);
                }
            }
            Stream requestStream = wbRequest.GetRequestStream();
            requestStream.Write(byteArray, 0, byteArray.Length);
            requestStream.Close();
            return (HttpWebResponse)wbRequest.GetResponse();
        }

        private static void _PostAsync<T>(Control sender, string url, string apiType, string method, string data, delegateApiCallback<T> onApiCallback, int timeout = 30000, Dictionary<string, string> headerDic = null, string contentType = "application/x-www-form-urlencoded", bool log = true)
        {
            if (log)
            {
                LogingHelper.Debug("PostWebApi", apiType, method, url, data);
            }
            byte[] byteArray = Encoding.UTF8.GetBytes(data);
            var wbRequest = (HttpWebRequest)WebRequest.Create(url);
            wbRequest.Timeout = timeout;
            wbRequest.Method = "POST";
            wbRequest.ContentType = contentType;
            wbRequest.ContentLength = byteArray.Length;
            if (headerDic != null && headerDic.Count > 0)
            {
                foreach (var item in headerDic)
                {
                    wbRequest.Headers.Add(item.Key, item.Value);
                }
            }

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
                                //if (method.Equals("GetWpfFirstPermissionByUserId"))
                                //{
                                //}
                                string res = reader.ReadToEnd();
                                res = preDeserialize<T>(res);

                                ApiResult<T> apiResult = JsonConvert.DeserializeObject<ApiResult<T>>(res);

                                if (!apiResult.Success && apiResult.Message?.IndexOf("Ticket无效") >= 0)
                                {
                                    if (sender != null)
                                    {
                                        sender.Invoke(new OnApicallbakDelegate(() => { onApiCallback(ReLogin<T>(url, data, apiResult), apiType, method, data); }));
                                    }
                                    else
                                    {
                                        onApiCallback(ReLogin<T>(url, data, apiResult), apiType, method, data); //断线重联
                                    }
                                }
                                if (apiResult.Success && !string.IsNullOrEmpty(apiResult.Context.Ticket) && LoginInfo.Instance.Ticket != apiResult.Context.Ticket)
                                {
                                    LoginInfo.Instance.Context = "{'Ticket':'" + apiResult.Context.Ticket + "','InvOrgId':'" + apiResult.Context.InvOrgId.ToString()
                                        + "','Language':'" + (Global.Language == null ? "zh-CN" : Global.Language.Code) + "'}";
                                }
                                if (sender != null)
                                {
                                    sender.Invoke(new OnApicallbakDelegate(() => { onApiCallback(apiResult, apiType, method, data); }));
                                }
                                else
                                {
                                    onApiCallback(apiResult, apiType, method, data);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if (log)
                            {
                                LogingHelper.Error("PostWebApi", apiType, method, url, ex);
                            }
                            ApiResult<T> apiResult = new ApiResult<T>();
                            apiResult.Success = false;
                            apiResult.Message = ex.Message;
                            onApiCallback(apiResult, apiType, method, data);
                        }
                    }, null);
                }
                catch (Exception ex)
                {
                    if (log)
                    {
                        LogingHelper.Error("PostWebApi", apiType, method, url, ex);
                    }
                    ApiResult<T> apiResult = new ApiResult<T>();
                    apiResult.Success = false;
                    apiResult.Message = ex.Message;
                    onApiCallback(apiResult, apiType, method, data);
                }
            }, null);

        }
        #endregion

        /// <summary>
        /// 异步Post Url
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="url"></param>
        /// <param name="onApiCallback"></param>
        public static void PostUrlAsync(Control sender, string url, delegateApiCallback<string> onApiCallback)
        {
            //_PostAsync(sender, url, "", "", "", onApiCallback, AppSettings.Instance.ApiTimeOut, contentType: "application/json", log: AppSettings.Instance.ApiLog);

            if (AppSettings.Instance.ApiLog)
            {
                LogingHelper.Debug("PostWebApi", "", "", url, "");
            }
            byte[] byteArray = Encoding.UTF8.GetBytes("");
            var wbRequest = (HttpWebRequest)WebRequest.Create(url);
            wbRequest.Timeout = AppSettings.Instance.ApiTimeOut;
            wbRequest.Method = "POST";
            wbRequest.ContentType = "application/json";
            wbRequest.ContentLength = byteArray.Length;

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
                                ApiResult<string> apiResult = new ApiResult<string>();
                                apiResult.Success = true;
                                apiResult.Result = res;
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
                            if (AppSettings.Instance.ApiLog)
                            {
                                LogingHelper.Error("PostWebApi", "", "", url, ex);
                            }
                            ApiResult<string> apiResult = new ApiResult<string>();
                            apiResult.Success = false;
                            apiResult.Message = ex.Message;
                            onApiCallback(apiResult, "", "", "");
                        }
                    }, null);
                }
                catch (Exception ex)
                {
                    if (AppSettings.Instance.ApiLog)
                    {
                        LogingHelper.Error("PostWebApi", "", "", url, ex);
                    }
                    ApiResult<string> apiResult = new ApiResult<string>();
                    apiResult.Success = false;
                    apiResult.Message = ex.Message;
                    onApiCallback(apiResult, "", "", "");
                }
            }, null);
        }

        /// <summary>
        /// Get 方法
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Get(string url)
        {
            if (!url.StartsWith("http::") && !url.StartsWith("https::"))
                url = "http://" + url;

            if (AppSettings.Instance.ApiGetLog)
            {
                LogingHelper.Debug("Get", null, null, url);
            }
            var wbRequest = (HttpWebRequest)WebRequest.Create(url);
            wbRequest.Method = "GET";
            HttpWebResponse responseResult = (HttpWebResponse)wbRequest.GetResponse();

            using (Stream responseStream = responseResult.GetResponseStream())
            using (StreamReader reader = new StreamReader(responseStream))
            {
                string res = reader.ReadToEnd();
                return res;
            }
        }

        public static void GetAsync(Control sender, string url, delegateApiGetCallback onApiGetCallback)
        {
            if (!url.StartsWith("http::") && !url.StartsWith("https::"))
                url = "http://" + url;

            delegateApiGetCallback callback = new delegateApiGetCallback((isOK, r, u) => {
                if (sender != null)
                {
                    sender.Invoke(new OnApicallbakDelegate(() => { onApiGetCallback(isOK, r, u); }));
                }
                else
                {
                    onApiGetCallback(isOK, r, u);
                }
            });;

            if (AppSettings.Instance.ApiGetLog)
            {
                LogingHelper.Debug("Get", null, null, url);
            }
            var wbRequest = (HttpWebRequest)WebRequest.Create(url);
            wbRequest.Method = "GET";

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
                        callback(true, res, url);
                    }
                }
                catch (Exception ex)
                {
                    if (AppSettings.Instance.ApiGetLog)
                    {
                        LogingHelper.Error("PostWebApi", null, null, url, ex);
                    }
                    callback(false, ex.Message, url);
                }
            }, null);
        }
    }
}
