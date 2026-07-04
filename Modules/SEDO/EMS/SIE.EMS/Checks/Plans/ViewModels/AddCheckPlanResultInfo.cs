using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Checks.Plans.ViewModels
{
    /// <summary>
    /// 添加点检计划返回信息
    /// </summary>
    [Serializable]
    public class AddCheckPlanResultInfo
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg { get; set; }
    }
}
