using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Dock.Datas
{
    /// <summary>
    /// AccessToken数据
    /// </summary>
    public class AccessTokenData
    {
        /// <summary>
        /// 获取到的凭证
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// 凭证有效时间。单位：秒
        /// </summary>
        public string expires_in { get; set; }
    }

    public class MessageTemplateSendDataContentDto
    {
        /// <summary>
        /// 文本内容
        /// </summary>
        public string value { get; set; }

        /// <summary>
        /// 文本颜色
        /// </summary>
        public string color { get; set; }
    }

    public class MessageTemplateSendDataDto
    {
        public MessageTemplateSendDataContentDto first { get; set; }
        public MessageTemplateSendDataContentDto keyword1 { get; set; }
        public MessageTemplateSendDataContentDto keyword2 { get; set; }
        public MessageTemplateSendDataContentDto keyword3 { get; set; }
        public MessageTemplateSendDataContentDto remark { get; set; }
    }

    public class MessageTemplateSendDto
    {
        /// <summary>
        /// 必填
        /// 接收者openid
        /// </summary>
        public string touser { get; set; }

        /// <summary>
        /// 必填
        /// 模板ID
        /// </summary>
        public string template_id { get; set; }

        /// <summary>
        /// 模板跳转链接（海外帐号没有跳转能力）
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 跳小程序所需数据，不需跳小程序可不用传该数据
        /// </summary>
        public MiniprogramDto miniprogram { get; set; }

        /// <summary>
        /// 必填
        /// 模板数据
        /// </summary>
        public MessageTemplateSendDataDto data { get; set; }

        /// <summary>
        /// 模板内容字体颜色，不填默认为黑色
        /// </summary>
        public string color { get; set; }

        /// <summary>
        /// 防重入id。对于同一个openid + client_msg_id, 只发送一条消息,10分钟有效,超过10分钟不保证效果。若无防重入需求，可不填
        /// </summary>
        public string client_msg_id { get; set; }
    }

    public class MiniprogramDto
    {
        /// <summary>
        /// 必填
        /// 所需跳转到的小程序appid（该小程序 appid 必须与发模板消息的公众号是绑定关联关系，暂不支持小游戏）
        /// </summary>
        public string appid { get; set; }

        /// <summary>
        /// 所需跳转到小程序的具体页面路径，支持带参数,（示例index? foo = bar），要求该小程序已发布，暂不支持小游戏
        /// </summary>
        public string pagepath { get; set; }
    }

    /// <summary>
    /// Ticket数据
    /// </summary>
    public class WeChatTicketData
    {
        /// <summary>
        /// 错误编码
        /// </summary>
        public string errcode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string errmsg { get; set; }

        /// <summary>
        /// Ticket
        /// </summary>
        public string ticket { get; set; }

        /// <summary>
        /// 凭证有效时间。单位：秒
        /// </summary>
        public string expires_in { get; set; }
    }

    /// <summary>
    /// 微信签名数据
    /// </summary>
    public class WeChatSignatureData
    {
        /// <summary>
        /// 时间戳字符串
        /// </summary>
        public string TimeStamp { get; set; }

        /// <summary>
        /// Accsee_Token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Ticket
        /// </summary>
        public string Ticket { get; set; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string NonceStr { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string Signature { get; set; }
    }
}
