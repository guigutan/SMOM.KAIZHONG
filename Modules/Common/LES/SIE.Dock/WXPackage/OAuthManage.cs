using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using SIE.Api;
using System.Net;
using System.IO;
using System.Web;
using System;
using SIE.Core.Common;
using SIE.Dock.Datas;
using SIE.Domain.Validation;
using Nest;
using SIE.Utils;
using System.Security.Cryptography;
using SIE.Dock.Common;

namespace SIE.Dock.WXPackage
{
    /// <summary>
    /// 获取微信信息
    /// </summary>
    public class OAuthManage : DomainController
    {
        /// <summary>
        /// 获取CODE,后期用于获取微信用户openid
        /// </summary>
        /// <param name="appid">appid</param>
        /// <param name="redirect_url">url</param>
        /// <returns>用户url信息</returns>
        [ApiService("获取用户Url")]
        [return: ApiReturn("用户Url")]
        public virtual string GetCodeBase(string appid, string redirect_url)
        {
            const int state = 1;
            string redirectUrl = "https://open.weixin.qq.com/connect/oauth2/authorize?"
                  + "appid=" + appid
                  + "&redirect_uri=" + redirect_url
                  + "&response_type=code&scope=snsapi_base"
                  + "&state=" + state + "#wechat_redirect";

            return redirectUrl;
        }

        /// <summary>
        /// 获取OpenId
        /// </summary>
        /// <param name="code">用户Code</param>
        /// <param name="appid">appid</param>
        /// <param name="secret">secret</param>
        /// <returns>OpenId</returns>
        [ApiService("获取OpenId")]
        [return: ApiReturn("返回OpenId")]
        public virtual string GetOpenId(string code, string appid, string secret)
        {
            string strResult = "";
            string openid = "";
            string strurl = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + appid + "&secret=" + secret + "&code=" + code + "&grant_type=authorization_code";
            try
            {
                HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(new Uri(strurl));
                HttpWebResponse httpWebResp = (HttpWebResponse)myReq.GetResponse();
                Stream myStream = httpWebResp.GetResponseStream();
                StreamReader sr = new StreamReader(myStream,
                    Encoding.UTF8);
                StringBuilder strBuilder = new StringBuilder();
                while (-1 != sr.Peek())
                {
                    strBuilder.Append(sr.ReadLine());
                }
                strResult = strBuilder.ToString();
                JObject obj = (JObject)JsonConvert.DeserializeObject(HttpUtility.UrlDecode(strResult));
                openid = obj["openid"].ToString().Replace("\"", "");
                return openid;
            }
            catch
            {
                throw new ValidationException("获取OPENID发生错误".L10N());
            }
        }

        /// <summary>
        /// 获取用户信息，包含头像和昵称等
        /// </summary>
        /// <param name="open_id">open_id</param>
        /// <param name="access_token">access_token</param>
        /// <returns>用户信息</returns>
        [ApiService("获取用户信息")]
        [return: ApiReturn("返回用户信息")]
        public virtual JObject GetUserInfo(string open_id, string access_token)
        {
            string strResult = "";
            string strurl = "https://api.weixin.qq.com/sns/userinfo?access_token=" + access_token + "&openid=" + open_id + "&lang=zh_CN";
            try
            {
                HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(new Uri(strurl));
                HttpWebResponse httpWebResp = (HttpWebResponse)myReq.GetResponse();
                Stream myStream = httpWebResp.GetResponseStream();
                StreamReader sr = new StreamReader(myStream, Encoding.UTF8);
                StringBuilder strBuilder = new StringBuilder();
                while (-1 != sr.Peek())
                {
                    strBuilder.Append(sr.ReadLine());
                }
                strResult = strBuilder.ToString();
                //更新weixin_user_info表中微信用户信息
                JObject tmpObj = (JObject)JsonConvert.DeserializeObject(HttpUtility.UrlDecode(strResult));
                return tmpObj;
            }
            catch
            {
                strResult = "err";
            }

            JObject obj = (JObject)JsonConvert.DeserializeObject(HttpUtility.UrlDecode(strResult));
            return obj;
        }

        

        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetCurrTimeStamp()
        {
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }

        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <returns>Guid随机字符串</returns>
        public static string GetRandomGuid()
        {
            string result = System.Guid.NewGuid().ToString().Replace("-", "");
            return result;
        }

        /// <summary>
        /// 获取微信签名
        /// </summary>
        /// <param name="appid">appId</param>
        /// <param name="appSecret">secret</param>
        /// <param name="url"></param>
        /// <returns></returns>
        [ApiService("获取微信签名")]
        [return: ApiReturn("返回微信签名数据")]
        public virtual WeChatSignatureData GetWeChatSignature(string appid, string appSecret, string url)
        {
            var WxSenderController = RT.Service.Resolve<WechatSenderController>();
            WeChatSignatureData signatureData = new WeChatSignatureData();
            var accessTokenData = WxSenderController.GetAccessToken(appid, appSecret);
            string nonceStr = GetRandomGuid();
            string timeStamp = GetCurrTimeStamp();
            var ticketData = GetWeChatTicketData(accessTokenData.access_token);
            string signatureStr = "jsapi_ticket=" + ticketData.ticket + "&noncestr=" + nonceStr + "&timestamp=" + timeStamp + "&url=" + url;
            var sha1 = new SHA1CryptoServiceProvider();
            byte[] str01 = Encoding.Default.GetBytes(signatureStr);
            byte[] str02 = sha1.ComputeHash(str01);
            var signature = BitConverter.ToString(str02).Replace("-", "");
            signatureData.TimeStamp = timeStamp;
            signatureData.NonceStr = nonceStr;
            signatureData.AccessToken = accessTokenData.access_token;
            signatureData.Ticket = ticketData.ticket;
            signatureData.Signature = signature;
            return signatureData;
        }

        /// <summary>
        /// 获取Ticket数据
        /// </summary>
        /// <param name="accessToken">AccssToken</param>
        /// <returns>Ticket</returns>
        /// <exception cref="ValidationException">异常信息</exception>
        public static WeChatTicketData GetWeChatTicketData(string accessToken)
        {
            const string accessTokenUrl = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=ACCESS_TOKEN&type=jsapi";
            string requestUrl = accessTokenUrl.Replace("ACCESS_TOKEN", accessToken);
            WeChatTicketData ticketData = new WeChatTicketData();
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(new Uri(requestUrl));
            try
            {
                req.Method = "GET";
                using (WebResponse wr = req.GetResponse())
                {
                    StreamReader reader = new StreamReader(wr.GetResponseStream(), Encoding.UTF8);
                    string content = reader.ReadToEnd();//在这里对Access_token 赋值  
                    WeChatTicketData ticketJson = JsonHelper.ToJsonObjectCore<WeChatTicketData>(content);
                    ticketData.errcode = ticketJson.errcode;
                    ticketData.errmsg = ticketJson.errmsg;
                    ticketData.ticket = ticketJson.ticket;
                    ticketData.expires_in = ticketJson.expires_in;
                }
            }
            catch (Exception e)
            {
                throw new ValidationException(e.Message);
            }

            return ticketData;
        }
    }
}
