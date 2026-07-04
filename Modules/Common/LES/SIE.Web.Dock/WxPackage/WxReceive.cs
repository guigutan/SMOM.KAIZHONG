using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SIE.Web.Dock.WxPackage
{
    /// <summary>
    /// 微信推送接受接口、只能用Get请求 且不能封装
    /// </summary>
    [Route("api/WxReceive")]
    public class WxReceive : Controller
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
        public string GetWxMessage(string signature, string timestamp, string nonce, string echostr)
        {
            const string Tooken = "1234567";
            List<string> SortList = new List<string> { timestamp, nonce, Tooken };
            SortList.Sort();
            var str = String.Join("", SortList.ToArray());
            var sha1 = new SHA1CryptoServiceProvider();
            byte[] str01 = Encoding.Default.GetBytes(str);
            byte[] str02 = sha1.ComputeHash(str01);
            var now_signature = BitConverter.ToString(str02).Replace("-", "");
            if (now_signature.ToLower() == signature)
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
