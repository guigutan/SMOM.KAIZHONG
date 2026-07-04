using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIE.Api;
using SIE.DataPortal;
using SIE.Security;

namespace SIE.Core.ApiLogs
{
    /// <summary>
    /// API请求日志
    /// </summary>
    public class ApiRequestLog : IDisposable
    {
        #region 属性
        /// <summary>
        /// 日志ID
        /// </summary>
        public string LogId { get; private set; }
        /// <summary>
        /// 请求开始时间
        /// </summary>
        public DateTime StartTime { get; private set; }
        /// <summary>
        /// 请求结束时间
        /// </summary>
        public DateTime EndTime { get; private set; }
        /// <summary>
        /// 请求耗时
        /// </summary>
        public double TotalMilliseconds { get; private set; }
        /// <summary>
        /// 方法
        /// </summary>
        public MethodInfo Method { get; private set; }
        /// <summary>
        /// API类型
        /// </summary>
        public ApiTypeInfo ApiTypeInfo { get; private set; }
        /// <summary>
        /// API方法
        /// </summary>
        public ApiMethodInfo ApiMethodInfo { get; private set; }
        /// <summary>
        /// API服务名
        /// </summary>
        public string ApiServiceName { get { return LogAttrInfo?.MethodDescription; } }
        /// <summary>
        /// API服务控制器
        /// </summary>
        public string ApiServiceController { get { return ApiTypeInfo?.Type.Name; } }
        /// <summary>
        /// API方法名
        /// </summary>
        public string ApiServiceMethodName { get { return ApiMethodInfo?.Method.Name; } }
        /// <summary>
        /// API日志特性信息
        /// </summary>
        public ApiLogAttrInfo LogAttrInfo { get; private set; }
        /// <summary>
        /// 请求关键字值字典
        /// </summary>
        public Dictionary<int, string> RequestKeyValueDict { get; private set; }
        /// <summary>
        /// 是否启用API日志
        /// </summary>
        public bool EnableLog { get { return LogAttrInfo != null; } }
        /// <summary>
        /// 是否有异常
        /// </summary>
        public YesNo HasException { get; set; }
        /// <summary>
        /// API请求JSON字符串
        /// </summary>
        public string ApiRequestJsonStr { get; private set; }
        /// <summary>
        /// API请求
        /// </summary>
        public ApiRequest ApiRequest { get; private set; }
        /// <summary>
        /// API响应JSON字符串
        /// </summary>
        public string ApiResponseJsonStr { get; private set; }
        /// <summary>
        /// API响应
        /// </summary>
        public ApiResponse ApiResponse { get; private set; }
        /// <summary>
        /// 库存组织
        /// </summary>
        public int? InvOrgId { get; set; }
        /// <summary>
        /// 员工Id
        /// </summary>
        public double? EmployeeId { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public double? UserId { get; set; }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="request">请求</param>
        public ApiRequestLog(ApiRequest request)
        {
            LogId = Guid.NewGuid().ToString();
            StartTime = DateTime.Now;
            RequestKeyValueDict = new Dictionary<int, string>();
            var apiType = ApiManager.GetApiType(request.ApiType);
            Method = ApiManager.GetApiMethod(apiType, request.Method);
            if (Method == null)
                throw new ArgumentException("类型[{0}]找不到方法[{1}]".L10nFormat(request.ApiType, request.Method));

            ApiTypeInfo = ApiManager.ApiTypeInfos.First(p => p.Type == apiType);
            ApiMethodInfo = ApiTypeInfo.Methods.First(p => p.Value.Method == Method).Value;
            LogAttrInfo = ApiLogAttrManager.TryGetApiLogAttrInfo(request.ApiType, request.Method);
            SetApiRequest(request);
        }

        /// <summary>
        /// 设置API请求
        /// </summary>
        /// <param name="value"></param>
        public void SetApiRequest(ApiRequest value)
        {
            if (!EnableLog)
                return;
            ApiRequest = value;
            var ticket = ApiRequest.Context.GetValue<string>("Ticket");
            SetContextByTicket(ticket);
            ApiRequestJsonStr = ApiRequest == null ? string.Empty : JsonConvert.SerializeObject(ApiRequest);
            RequestKeyValueDict.Clear();
            if (LogAttrInfo.KeyIndexJsonPathDict.Count > 0)
            {
                var jObject = JObject.Parse(ApiRequestJsonStr);
                LogAttrInfo.KeyIndexJsonPathDict.ForEach(pair =>
                {
                    var keyItems = new List<string>();
                    pair.Value.ForEach(jsonPath =>
                    {
                        if (jsonPath.Contains('◇'))
                        {
                            var arrs = jsonPath.Split('◇');
                            var jtokens = jObject.SelectTokens(arrs[0]);
                            var keyItem = string.Join(",", jtokens);
                            if (keyItem.IsNotEmpty())
                                keyItems.Add(arrs[1] + "：" + keyItem);
                        }
                        else
                        {
                            var jtokens = jObject.SelectTokens(jsonPath);
                            var keyItem = string.Join(",", jtokens);
                            if (keyItem.IsNotEmpty())
                                keyItems.Add(keyItem);
                        }
                    });
                    var keyItemStr = string.Join(";", keyItems);
                    RequestKeyValueDict.Add(pair.Key, keyItemStr);
                });
            }
        }

        /// <summary>
        /// 设置API响应
        /// </summary>
        /// <param name="value"></param>
        public void SetApiResponse(ApiResponse value)
        {
            if (!EnableLog)
                return;
            ApiResponse = value;
            if (ApiResponse == null)
            {
                ApiResponseJsonStr = string.Empty;
            }
            else
            {
                ApiResponseJsonStr = JsonConvert.SerializeObject(ApiResponse);
            }
        }

        /// <summary>
        /// 根据Ticket，参照框架源码解析员工、用户、库存组织
        /// </summary>
        /// <param name="ticket"></param>
        private void SetContextByTicket(string ticket)
        {
            if (ticket.IsNotEmpty())
            {
                string key = AppRuntime.Config.Get("EncryptKey", "XaNjkgbSh04BpyjeUo3QGtre");
                byte[] array = CryptographyHelper.Decrypt(ticket, key, "yz/YrE0WZrwahV+I");
                string str = Encoding.Default.GetString(array);
                var infos = str.Split(',');
                this.EmployeeId = double.Parse(infos[1]);
                this.UserId = double.Parse(infos[2]);
                this.InvOrgId = infos[4].IsNotEmpty() ? int.Parse(infos[4]) : default(int?);
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            EndTime = DateTime.Now;
            TotalMilliseconds = (EndTime - StartTime).TotalMilliseconds;
            if (EnableLog && TotalMilliseconds > LogAttrInfo.FetchOverTime)
            {
                LogAttrInfo.Logger.LogDispose(this);
            }

            Method = null;
            ApiTypeInfo = null;
            ApiMethodInfo = null;
        }
    }
}
