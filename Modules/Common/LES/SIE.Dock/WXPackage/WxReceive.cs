using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace SIE.Dock.WXPackage
{
    /// <summary>
    /// 微信对接URL
    /// </summary>
    [Route("api/WxReceive")]
    public class WxReceive: Controller
    {
        /// <summary>
        /// 微信接入接口(必须是GET方法)
        /// </summary>
        /// <param name="signature">微信加密签名，signature结合了开发者填写的 token 参数和请求中的 timestamp 参数、nonce参数。</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <param name="echostr">随机字符串</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetWxMessage")]
        public string GetWxMessage(string signature, string timestamp,string nonce,string echostr)
        {
            //var params1 = signature;
            //var params2 = timestamp;
            //var params3 = nonce;
            //var params4 = echostr;
            if (!echostr.IsNullOrEmpty())
            {
                return echostr;
            }
            else
            {
                return "";
            }
            
        }
    }
}
