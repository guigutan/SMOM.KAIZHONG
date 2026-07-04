using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.WorkBenchCommon.Workbench.Base
{
    /// <summary>
    /// 组件信息VIEWMODEL
    /// </summary>
    [Serializable]
    public  class ComponentBindViewModel
    {
        /// <summary>
        /// 绑定面板
        /// </summary>
        public string Panel { get; set; }
        /// <summary>
        /// 绑定组件
        /// </summary>
        public string ComponentCode { get; set; }
        /// <summary>
        /// 刷新时间
        /// </summary>
        public int RefreshInterval { get; set; }

        /// <summary>
        /// 组件绑定参数集合 
        /// </summary>
        public List<ParameterBindViewModel> BindOutputparam { get; set; } = new List<ParameterBindViewModel>();
    }
}
