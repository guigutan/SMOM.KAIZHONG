using Newtonsoft.Json;
using SIE.Core.Common;
using SIE.Domain;
using SIE.EAP.Common.Enums;
using SIE.EAP.Common.Logs;
using SIE.EventMessages.EAP.Infs;
using SIE.EventMessages.EAP.Infs.Datas.Common;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace SIE.EAP.Common.Controller
{
    /// <summary>
    /// Eap基础控制器
    /// </summary>
    public class EapBaseController : InfBaseController, IEapController
    {
        #region 接口实现

        public string RequestEapCommon<TRequest>(string apiMethod, TRequest request, int direction,
            out bool isSuccessful, string desc = null)
        {
            isSuccessful = true;
            var response = RequestEapCommon<TRequest, CommonResult<CommonReturn>>(apiMethod, request, CommonResponseHandler, direction, out isSuccessful, desc);
            return JsonConvert.SerializeObject(response);
        }

        public TResponse RequestEapCommon<TRequest, TResponse>(string apiMethod, TRequest request,
            Action<TRequest, TResponse> responseHandle, int direction, out bool isSuccessful, string desc)
        {
            isSuccessful = true;
            TResponse responseJsonObj = default(TResponse);
            var sTime = RF.Find<EAPInterfaceLog>().GetDbTime();
            //eap接口地址
            string eapApiUrl;
            try
            {
                //string asrsApiBaseUrl = RT.Config.Get("API.EapUrl");
                //string asrsApiBaseUrl = "http://192.168.8.143:7666/dataService/webapi/Agv/"; //eap现场公开的地址
                string asrsApiBaseUrl = "http://10.20.31.114:7666/dataService/webapi/Agv/"; //现场测试环境
                eapApiUrl = FileHelper.CombinePath(asrsApiBaseUrl, apiMethod);
            }
            catch (Exception)
            {
                isSuccessful = false;
                throw new Exception("未配置EAP接口地址".L10N());
            }

            // 请求内容
            var requestJsonStr = JsonConvert.SerializeObject(request);
            // 响应结果
            string response = string.Empty;
            // 错误信息
            string errorMsg = string.Empty;
            try
            {
                // 发送请求
                HttpContent content = new StringContent(requestJsonStr);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                HttpClient client = new HttpClient();
                HttpResponseMessage httpResponse = client.PostAsync(eapApiUrl, content).Result;

                // 解析报文
                response = httpResponse.Content.ReadAsStringAsync().Result;
                responseJsonObj = JsonConvert.DeserializeObject<TResponse>(response);

                // 结果处理委托
            }
            catch (Exception ex)
            {
                isSuccessful = false;
                errorMsg = ex.GetBaseException().Message;
            }
            // 保存接口日志
            using (var trans = DB.AutonomousTransactionScope(EAPEntityDataProvider.ConnectionStringName))
            {
                var eTime = RF.Find<EAPInterfaceLog>().GetDbTime();
                SaveEAPInfLog(desc, (JobDirection)direction, sTime, eTime
                    , errorMsg, requestJsonStr, response);
                trans.Complete();
            }

            return responseJsonObj;
        }

        public void SaveEAPInfLog(string desc, int direction, DateTime beginDate, DateTime endDate, string remark = null, string requestContent = null, string responseContent = null)
        {
            SaveEAPInfLog(desc, (JobDirection)direction, beginDate, endDate
                    , remark, requestContent, responseContent);
        }

        #endregion

        /// <summary>
        /// 通用响应处理方法
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="request"></param>
        /// <param name="response"></param>
        private void CommonResponseHandler<TRequest>(TRequest request, CommonResult<CommonReturn> response)
        {
            // 错误信息
            bool hasError = false;
            string errorMsg = null;

            if (response == null)
            {
                throw new Exception("调用EAP失败，EAP没有响应");
            }

            //0 成功 其他失败
            
            if (response.IsSuccess)
            {
                hasError = false;
            }
            else
            {
                hasError = true;
                errorMsg = JsonConvert.SerializeObject(response.Error);
            }

            if (hasError)
            {
                if (errorMsg.IsNullOrEmpty())
                    errorMsg = "调用EAP接口失败，立库没有返回具体错误信息".L10N();
                else
                    errorMsg = "调用EAP接口失败，立库返回信息：" + errorMsg;
                throw new Exception(errorMsg);
            }
        }
    }
}
