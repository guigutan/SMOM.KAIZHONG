using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using SIE.Domain;
using SIE.Rbac.InvOrgs;

namespace SIE.Core.ApiLogs
{
    /// <summary>
    /// API日志数据库表记录器
    /// </summary>
    public class ApiLogDbLogger : IApiLogLogger
    {
        /// <summary>
        /// 释放时记录
        /// </summary>
        /// <param name="logReq"></param>
        public void LogDispose(ApiRequestLog logReq)
        {
            var allOrg = RF.GetAll<InvOrg>();
            var log = new ApiLog
            {
                ApiName = logReq.ApiServiceName,
                Controller = logReq.ApiServiceController,
                InvOrgId = logReq.InvOrgId,
                IsSuccess = logReq.ApiResponse.Success ? YesNo.Yes : YesNo.No,
                LogId = logReq.LogId,
                Method = logReq.ApiServiceMethodName,
                Request = logReq.ApiRequestJsonStr,
                Response = logReq.ApiResponseJsonStr,
                HasException = logReq.HasException,
                StartTime = logReq.StartTime,
                EndTime = logReq.EndTime,
                TimeSpanMilliseconds = logReq.TotalMilliseconds,
                EmployeeId = logReq.EmployeeId,
                UserId = logReq.UserId,
                InvOrgName = allOrg.FirstOrDefault(a => a.Code == logReq.InvOrgId)?.Name,
            };
            if (logReq.RequestKeyValueDict.Count > 0)
            {
                var jObject = JObject.Parse(logReq.ApiRequestJsonStr);
                logReq.RequestKeyValueDict.ForEach(pair =>
                {
                    SetLogKeyValue(log, pair.Key, pair.Value);
                });
            }
            RF.Save(log);
        }

        /// <summary>
        /// 设置关键字的值
        /// </summary>
        /// <param name="log"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        private void SetLogKeyValue(ApiLog log, int index, string value)
        {
            value = SubstringKeyValue(value);
            switch (index)
            {
                case 1:
                    log.Key1 = value;
                    break;
                case 2:
                    log.Key2 = value;
                    break;
                case 3:
                    log.Key3 = value;
                    break;
                case 4:
                    log.Key4 = value;
                    break;
                case 5:
                    log.Key5 = value;
                    break;
            }
        }

        /// <summary>
        /// 截断关键字值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string SubstringKeyValue(string str)
        {
            return str.Substring(0, Math.Min(str.Length, 2000));
        }
    }
}
