using Newtonsoft.Json;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Enums;
using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Andon.MessageSendJob
{
    public class WeComMessageSender
    {
        /// <summary>
        /// 企业ID
        /// </summary>
        private readonly string _corpId;
        /// <summary>
        /// 凭证密钥
        /// </summary>
        private readonly string _corpSecret;
        /// <summary>
        /// 应用ID
        /// </summary>
        private readonly int _agentId;
        /// <summary>
        /// url
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="corpId">企业ID</param>
        /// <param name="corpSecret">应用Secret</param>
        /// <param name="agentId">应用AgentId</param>
        public WeComMessageSender(string corpId, string corpSecret, int agentId)
        {
            _corpId = corpId ?? throw new ArgumentNullException(nameof(corpId));
            _corpSecret = corpSecret ?? throw new ArgumentNullException(nameof(corpSecret));
            _agentId = agentId;
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// 发送Markdown格式消息给指定用户
        /// </summary>
        /// <param name="userIds">接收消息的用户ID，多个用'|'分隔</param>
        /// <param name="content">消息内容</param>
        /// <returns></returns>
        public void SendMarkdownMessageAsync(string userIds, string content)
        {
            if (string.IsNullOrWhiteSpace(userIds))
                throw new ValidationException("安灯发送消息失败:用户ID不能为空"/*, nameof(userIds)*/);

            if (string.IsNullOrWhiteSpace(content))
                throw new ValidationException("安灯发送消息失败:消息内容不能为空"/*, nameof(content)*/);

            try
            { 
                // 1. 获取AccessToken
                string token = GetAccessTokenAsync();

                // 2. 发送消息
                SendMessageAsync(token, userIds, content);
            }
            catch (Exception ex)
            {
                throw new Exception("企业微信消息发送失败", ex);
            }
        }

        /// <summary>
        /// 获取企业微信AccessToken
        /// </summary>
        /// <returns></returns>
        private dynamic GetAccessTokenAsync()
        {
            var url = RT.Config.Get<string>("QV.URL");
            url += $"/gettoken?corpid={_corpId}&corpsecret={_corpSecret}";

            //var response = await _httpClient.GetStringAsync(url);
            //dynamic result = JsonConvert.DeserializeObject(response);
            string response = "";
            try
            {
                response = _httpClient.GetStringAsync(url).Result;
            }
            catch (Exception ex)
            {

            }
            dynamic result = JsonConvert.DeserializeObject(response);

            if (result.errcode != 0)
                throw new Exception($"获取Token失败: {result.errmsg}");

            return result.access_token;

        }

        /// <summary>
        /// 发送消息到企业微信
        /// </summary>
        /// <param name="token">AccessToken</param>
        /// <param name="userIds">接收消息的用户ID</param>
        /// <param name="content">消息内容</param>
        /// <returns></returns>
        public void SendMessageAsync(string token, string userIds, string content)
        {
            var url = RT.Config.Get<string>("QV.URL");
            url += $"/message/send?access_token={token}";

            var message = new
            {
                touser = userIds,
                msgtype = "markdown",
                agentid = _agentId,
                markdown = new
                {
                    content = $"**【安灯异常】**\n{content}\n\n本消息为自动发送，无需回复"
                }
            };

            var json = JsonConvert.SerializeObject(message);

            var erpDataInfLog = RT.Service.Resolve<InfDataLogController>().SaveErpDataInfLog(InfType.QvPush, json, DateTime.Now, CallDirection.MesToQv, CallResult.UnSave, 1);

            try
            {
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var response = _httpClient.PostAsync(url, data).Result;
                response.EnsureSuccessStatusCode();
                var responseText = response.Content.ReadAsStringAsync().Result;
                erpDataInfLog.ResponseContent = responseText;

                dynamic result = JsonConvert.DeserializeObject(responseText);
                if (result.errcode != 0)
                {
                    //erpDataInfLog.ErrorMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    //erpDataInfLog.CallResult = CallResult.Fail;
                    //erpDataInfLog.EndDate = DateTime.Now;
                    //erpDataInfLog.PersistenceStatus = PersistenceStatus.Modified;
                    //RF.Save(erpDataInfLog);
                    throw new ValidationException($"发送失败: {result.errmsg}");
                }

                else
                {
                    erpDataInfLog.CallResult = CallResult.Success;
                    //erpDataInfLog.EndDate = DateTime.Now;
                    //erpDataInfLog.PersistenceStatus = PersistenceStatus.Modified;
                    //RF.Save(erpDataInfLog);
                }
            }
            catch (Exception ex)
            {
                erpDataInfLog.ErrorMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                erpDataInfLog.CallResult = CallResult.Fail;
                //throw;
            }
            finally
            {
                erpDataInfLog.PersistenceStatus = PersistenceStatus.Modified;
                erpDataInfLog.EndDate = DateTime.Now;
                RF.Save(erpDataInfLog);
            }
        }
    }
}
