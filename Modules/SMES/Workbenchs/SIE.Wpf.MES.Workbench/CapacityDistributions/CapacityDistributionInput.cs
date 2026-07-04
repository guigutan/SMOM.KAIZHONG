using SIE.Wpf.Common.Diagram;
using System.ComponentModel;

namespace SIE.Wpf.MES.Workbench.CapacityDistributions
{
    /// <summary>
    /// 当日产能分布--输入类
    /// </summary>
    public class CapacityDistributionInput : ComponentInput<CapacityDisbuControl>
    {
        /// <summary>
        /// 资源ID
        /// </summary>
        [DisplayName("资源ID")]
        [Description("员工管理组件选择资源后传入")]
        public virtual double ResourceId { get; set; }

        /// <summary>
        /// 班次ID
        /// </summary>
        [DisplayName("班次ID")]
        [Description("员工管理组件选择班次后输出")]
        public virtual double ShiftId { get; set; }
    }
}