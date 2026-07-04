using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Core.Configs
{
    /// <summary>
    /// 电子签名配置项来源数据
    /// </summary>
    public enum ElecSignType
    {
        /// <summary>
        /// 登录密码
        /// </summary>
        [Label("登录密码")]
        UserPwd = 5,
        /// <summary>
        /// 授权密码
        /// </summary>
        [Label("授权密码")]
        AuthorizePwd = 10,
    }
}
