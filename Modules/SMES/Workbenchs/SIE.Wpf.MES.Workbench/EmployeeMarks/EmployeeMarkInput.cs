using SIE.Wpf.Common.Diagram;
using System.ComponentModel;

namespace SIE.Wpf.MES.Workbench.EmployeeMarks
{
    /// <summary>
    /// 综合评分输入参数
    /// </summary>
    public class EmployeeMarkInput : ComponentInput<EmployeeMarkControl>
    {
        /// <summary>
        /// 员工Id
        /// </summary> 
        [DisplayName("员工ID")]
        [Description("工序工位组件选择人员后输入")]
        public virtual double EmployeeId { get; set; }

        /// <summary>
        /// 资源Id(产线)
        /// </summary>
        [DisplayName("产线ID")]
        [Description("工序工位组件选择资源后输入")]
        public virtual double ResourceId { get; set; }
    }
}