using SIE.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.DataAuth
{
    /// <summary>
    /// 版本查询接口实现
    /// </summary>
    public class SVersion : IVersion
    {
       
        /// <summary>
        /// 获取版本信息
        /// </summary>
        /// <returns></returns>
        public string GetVesion()
        {
           return SMOM.AssemblyInformation.FileVersion;
        }
     
    }
}
