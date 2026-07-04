using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIE.Common.Sender;
using SIE.Core;
using SIE.Core.Common;
using SIE.Dock.Datas;
using SIE.Dock.DockAppoints;
using SIE.Dock.DockQueues;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Senders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
//using SIE.Senders.WeChat;
namespace SIE.Dock.Common
{
    /// <summary>
    /// 微信公众号推送类
    /// </summary>
    [Serializable]
    public class WechatSenderController: DomainController
    {
        private const string baseUrl = "http://test.zmc188.com:8020/9.1/WebBoardChartAttachment/8001/";

        private const string AppointTemplate_id = "MiBTKwI6iEDlhyrlV78KUe9HiBfzVY95x8MrZN5rnr0";

        //private const string QueueTemplate_id = "";

        private const string templeteColor = "#173177";


        /// <summary>
        /// 推送模板消息给微信平台
        /// </summary>
        /// <param name="sendData">模板数据</param>
        /// <exception cref="ValidationException"></exception>
        public virtual void SendMessage(MessageTemplateSendDto sendData)
        {
            try
            {
                var config = GetWxSenderConfig();
                if (config == null)
                {
                    throw new ValidationException("推送模块设置为空".L10N());
                }
                if (config.Secret.IsNullOrEmpty())
                {
                    throw new ValidationException("AppScreet密码为空，请在推送模块处设置AppScreet".L10N());
                }
                if (config.Appid.IsNullOrEmpty())
                {
                    throw new ValidationException("appid为空，请在推送模块处设置appid".L10N());
                }
                var accessTokenData = GetAccessToken(config.Appid, config.Secret);
                string access_token = accessTokenData.access_token;
                //根据access_token构建推送接口
                string sendUrl = $@"https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={access_token}";
                if (sendData == null)
                {
                    throw new ValidationException("推送的模板数据为空".L10N());
                }
                string sendstr = JsonConvert.SerializeObject(sendData);
                byte[] postData = System.Text.Encoding.UTF8.GetBytes(sendstr);
                var webClient = new WebClient();
                webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                byte[] responseData = webClient.UploadData(sendUrl, "POST", postData);
                string srcString = System.Text.Encoding.UTF8.GetString(responseData);
                ////41001	缺少access_token参数
                ////41006   access_token超时
                var result = (JObject)JsonConvert.DeserializeObject(srcString);
                var errcode = result["errcode"].ToString();
                var errmsg = result["errmsg"].ToString();
                if (errcode == "41001" || errcode == "41006" || errcode == "40001" || errcode == "40014" || errcode == "42001")
                    throw new ValidationException("errcode:{0};errmsg:{1}".L10nFormat(errcode, errmsg));
                if (errcode != "0") //0为成功返回码
                    throw new ValidationException("errcode:{0};errmsg:{1}".L10nFormat(errcode, errmsg));
            }
            catch
            {
                throw new ValidationException("推送微信公众号信息失败请重新操作!.".L10N());
            }
            
        }

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="appid">APPID</param>
        /// <param name="appSecret">SECRET</param>
        /// <returns>AccessToken</returns>
        /// <exception cref="ValidationException">异常信息</exception>
        public virtual AccessTokenData GetAccessToken(string appid, string appSecret)
        {
            const string accessTokenUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=APPID&secret=SECRET";
            string requestUrl = accessTokenUrl.Replace("APPID", appid).Replace("SECRET", appSecret);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(new Uri(requestUrl));
            AccessTokenData result = new AccessTokenData();
            try
            {
                req.Method = "GET";
                using (WebResponse wr = req.GetResponse())
                {
                    StreamReader reader = new StreamReader(wr.GetResponseStream(), Encoding.UTF8);
                    string content = reader.ReadToEnd();//在这里对Access_token 赋值  
                    AccessTokenData token = JsonHelper.ToJsonObjectCore<AccessTokenData>(content);
                    result.access_token = token.access_token;
                    result.expires_in = token.expires_in;
                }
            }
            catch (Exception e)
            {
                throw new ValidationException(e.Message);
            }

            return result;
        }

        /// <summary>
        /// 获取预约模板数据
        /// </summary>
        /// <param name="dockAppoint">预约数据</param>
        /// <param name="openId">openID</param>
        /// <returns></returns>
        
        public virtual MessageTemplateSendDto GetWxSenderDataByAppoint(DockAppoint dockAppoint,string openId)
        {
            string msg = "预约成功!预约单号为[{0}]".L10nFormat(dockAppoint.No);
            string remark = "预约时间:{0} {1}至{0} {2}".L10nFormat(dockAppoint.AppointDate.ToString(DateTimeFormat.YYYMMdd2), dockAppoint.AppointStartDate.ToString(DateTimeFormat.HHmm), dockAppoint.AppointEndDate.ToString(DateTimeFormat.HHmm));
            if (dockAppoint.IsCancelAppoint)
            {
                msg = "预约单号[{0}]已取消预约成功!".L10nFormat(dockAppoint.No);
            }
            //var color = templeteColor;
            var rst = new MessageTemplateSendDto()
            {
                touser = openId,
                template_id = AppointTemplate_id,
                //type =4:排队详情 type=5:预约详情
                url = baseUrl + "?type=" + 5 + "&Id=" + dockAppoint.Id,
                data = new MessageTemplateSendDataDto() {
                    first = new MessageTemplateSendDataContentDto()
                    {
                        value = msg,
                        color = templeteColor
                    },
                    keyword1 = new MessageTemplateSendDataContentDto()
                    {
                        value = dockAppoint.Contacts,
                        color = templeteColor
                    },
                    keyword2 = new MessageTemplateSendDataContentDto()
                    {
                        value = dockAppoint.ContactNum,
                        color = templeteColor
                    },
                    remark = new MessageTemplateSendDataContentDto() { 
                        value = remark,
                        color = templeteColor
                    }
                }
            };
            return rst;
        }

        /// <summary>
        /// 获取排队模板数据
        /// </summary>
        /// <param name="dockQueue">排队数据</param>
        /// <param name="openId">openID</param>
        /// <returns></returns>

        public virtual MessageTemplateSendDto GetWxSenderDataByQueue(DockQueue dockQueue, string openId)
        {
            string msg = "排队成功!排队单号为[{0}]".L10nFormat(dockQueue.No);
            if (dockQueue.QueueState == QueueState.Cancel)
            {
                msg = "排队单号[{0}]已取消排队成功!".L10nFormat(dockQueue.No);
            }
            //var color = templeteColor;
            var rst = new MessageTemplateSendDto()
            {
                touser = openId,
                template_id = AppointTemplate_id,
                //type =4:排队详情 type=5:预约详情
                url= baseUrl+"?type="+4+"&Id="+ dockQueue.Id,
                data = new MessageTemplateSendDataDto()
                {
                    first = new MessageTemplateSendDataContentDto()
                    {
                        value = msg,
                        color = templeteColor
                    },
                    keyword1 = new MessageTemplateSendDataContentDto()
                    {
                        value = dockQueue.Contacts,
                        color = templeteColor
                    },
                    keyword2 = new MessageTemplateSendDataContentDto()
                    {
                        value = dockQueue.ContactNum,
                        color = templeteColor
                    },
                    remark = new MessageTemplateSendDataContentDto()
                    {
                        value = "",
                        color = templeteColor
                    }
                }
            };
            return rst;
        }

        /// <summary>
        /// 获取分配模板数据
        /// </summary>
        /// <param name="dockQueue">排队数据</param>
        /// <param name="openId">openId</param>
        /// <param name="DockName">月台名称</param>
        /// <returns></returns>
        public virtual MessageTemplateSendDto GetWxSenderDataByAssign(DockQueue dockQueue, string openId,string DockName)
        {
            string msg = "分配月台成功!排队号[{1}]分配月台为[{0}]。".L10nFormat(DockName, dockQueue.No);
            //var color = templeteColor;
            var rst = new MessageTemplateSendDto()
            {
                touser = openId,
                template_id = AppointTemplate_id,
                //type =4:排队详情 type=5:预约详情
                url = baseUrl + "?type=" + 4 + "&Id=" + dockQueue.Id,
                data = new MessageTemplateSendDataDto()
                {
                    first = new MessageTemplateSendDataContentDto()
                    {
                        value = msg,
                        color = templeteColor
                    },
                    keyword1 = new MessageTemplateSendDataContentDto()
                    {
                        value = dockQueue.Contacts,
                        color = templeteColor
                    },
                    keyword2 = new MessageTemplateSendDataContentDto()
                    {
                        value = dockQueue.ContactNum,
                        color = templeteColor
                    },
                    remark = new MessageTemplateSendDataContentDto()
                    {
                        value = "",
                        color = templeteColor
                    }
                }
            };
            return rst;
        }

        /// <summary>
        /// 获取微信插件
        /// </summary>
        /// <returns></returns>
        public virtual PushPlug GetPushPlug()
        {
            return Query<PushPlug>().Where(p => p.PushClass == typeof(WeChatSender).ToString()).FirstOrDefault();
        }

        /// <summary>
        /// 获取微信插件config
        /// </summary>
        /// <returns></returns>
        private WeChatSenderConfig GetWxSenderConfig()
        {
            var plug = GetPushPlug();
            if (plug == null)
            {
                throw new ValidationException("推送管理模块没有类型为[{0}]的微信公众号推送方式！".L10nFormat(typeof(WeChatSender).ToString()));
            }
            var config = JsonConvert.DeserializeObject<WeChatSenderConfig>(plug.Config);
            return config;
        }
    }
}
