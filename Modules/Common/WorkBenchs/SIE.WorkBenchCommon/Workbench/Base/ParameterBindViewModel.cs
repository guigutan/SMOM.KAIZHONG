using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.WorkBenchCommon.Workbench.Base
{
    /// <summary>
    /// 组件参数绑定VIEWMODEL
    /// </summary>
    [Serializable]
    public class ParameterBindViewModel
    {
        /// <summary>
        /// 输出参数
        /// </summary>
        public string OutputParam { get; set; }

        /// <summary>
        /// 输入参数
        /// </summary>
        public string InputParam { get; set; }
    }
}
