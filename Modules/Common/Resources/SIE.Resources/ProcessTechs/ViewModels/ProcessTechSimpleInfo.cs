using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Resources.ProcessTechs.ViewModels
{
    /// <summary>
    /// 制程简易信息类
    /// </summary>
    [Serializable]
    public class ProcessTechSimpleInfo
    {
        /// <summary>
        /// 制程Id
        /// </summary>
        public double ProcessTechId { get; set; }

        /// <summary>
        /// 制程编码
        /// </summary>
        public string ProcessTechCode { get; set; }

        /// <summary>
        /// 制程名称
        /// </summary>
        public string ProcessTechName { get; set; }
    }
}
