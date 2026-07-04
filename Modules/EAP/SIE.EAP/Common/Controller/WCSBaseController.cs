using Newtonsoft.Json;
using SIE.Core.Common;
using SIE.Domain;
using SIE.EAP;
using SIE.EAP.Common.Controller;
using SIE.EAP.Common.Enums;
using SIE.EAP.Common.Logs;
using SIE.EventMessages.WMS.StereoWarhouses;
using SIE.EventMessages.WMS.StereoWarhouses.Datas;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace SIE.EAP.Common.Controller
{
    public class WCSBaseController : InfBaseController, IWcsController
    {
        /// <summary>
        /// 推送立库 Common 方法（使用通用响应处理）
        /// </summary>
        /// <typeparam name="TRequest">请求参数类型</typeparam>
        /// <param name="apiMethod">调用的api方法</param>
        /// <param name="request">上传数据</param>
        /// <param name="direction">任务方向 6：立库到业务表 7:业务表到立库</param>
        /// <param name="desc">日志描述</param>
        public string RequestWcsCommon<TRequest>(string apiMethod, TRequest request
            , int direction, out bool isSuccessful, string desc = null)
        {
            isSuccessful = true;
            return RequestWcsCommon<TRequest, ReturnData>(apiMethod, request, CommonResponseHandler, direction, out isSuccessful, desc);
        }

        /// <summary>
        /// 推送立库 Common 方法
        /// </summary>
        /// <typeparam name="TRequest">请求参数类型</typeparam>
        /// <typeparam name="TResponse">返回结果类型</typeparam>
        /// <param name="apiMethod">调用的api方法</param>
        /// <param name="request">上传数据</param>
        /// <param name="direction">任务方向 6：立库到业务表 7:业务表到立库</param>
        /// <param name="desc">日志描述</param>
        /// <param name="responseHandle">响应结果处理</param>
        public string RequestWcsCommon<TRequest, TResponse>(string apiMethod, TRequest request, Action<TRequest, TResponse> responseHandle
            , int direction, out bool isSuccessful, string desc = null)
        {
            isSuccessful = true;
            var sTime = RF.Find<EAPInterfaceLog>().GetDbTime();
            // 立库接口地址
            string wcsApiUrl;
            try
            {
                //string asrsApiBaseUrl = RT.Config.Get("API.WcsUrl");
                string asrsApiBaseUrl = "http://4y30w67459.zicp.vip:14787/JehaitWS/interfaceServices/SiEWMS/"; //测试的时候先写死
                wcsApiUrl = FileHelper.CombinePath(asrsApiBaseUrl, apiMethod);
            }
            catch
            {
                isSuccessful = false;
                throw new Exception("未配置立库接口地址".L10N());
            }

            // 请求内容
            var requestJsonStr = JsonConvert.SerializeObject(request);
            //log.Massage = requestJsonStr;

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
                HttpResponseMessage httpResponse = client.PostAsync(wcsApiUrl, content).Result;

                // 解析报文
                response = httpResponse.Content.ReadAsStringAsync().Result;
                TResponse responseJsonObj = JsonConvert.DeserializeObject<TResponse>(response);

                // 结果处理委托
                responseHandle.Invoke(request, responseJsonObj);
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
            return response;
        }

        /// <summary>
        /// 通用响应处理
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="request"></param>
        /// <param name="response"></param>
        private void CommonResponseHandler<TRequest>(TRequest request, ReturnData response)
        {
            // 错误信息
            bool hasError = false;
            string errorMsg = null;

            if (response == null)
            {
                throw new Exception("上传立库失败，立库没有响应");
            }

            //0 成功 其他失败
            switch (response.Code)
            {
                case "0":
                    hasError = false;
                    errorMsg = response.Message;
                    break;

                default:
                    hasError = true;
                    errorMsg = response.Message;
                    break;
            }

            if (hasError)
            {
                if (errorMsg.IsNullOrEmpty())
                    errorMsg = "获取立库数据失败，立库没有返回具体错误信息".L10N();
                else
                    errorMsg = "获取立库数据失败，立库返回信息：" + errorMsg;
                throw new Exception(errorMsg);
            }

        }
    }
}
