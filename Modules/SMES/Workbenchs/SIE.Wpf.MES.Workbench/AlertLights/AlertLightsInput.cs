using SIE.Wpf.Common.Diagram;
using System.ComponentModel;

namespace SIE.Wpf.MES.Workbench.AlertLights
{
    /// <summary>
    /// 安灯呼叫--输入参数类
    /// </summary>
    public class AlertLightsInput : ComponentInput<AlertLightsControl>
    {
        /// <summary>
        /// 输入参数-员工Id
        /// </summary>
        [DisplayName("员工ID")]
        [Description("工序工位组件选择用户后传入")]
        public virtual double EmployeeId { get; set; }

        /// <summary>
        /// 输入参数-工位Id
        /// </summary>
        [DisplayName("工位Id")]
        [Description("工序工位组件选择工位后传入")]
        public virtual double StationId { get; set; }

        /// <summary>
        /// 输入参数-产品ID 
        /// </summary>
        [DisplayName("产品ID")]
        [Description("检验采集产品切换后传入")]
        public virtual double ProductId { get; set; }

        /// <summary>
        /// 输入参数-工单ID
        /// </summary>
        [DisplayName("工单ID")]
        [Description("检验采集产品切换后传入")]
        public virtual double WorkOrderId { get; set; }
    }
}