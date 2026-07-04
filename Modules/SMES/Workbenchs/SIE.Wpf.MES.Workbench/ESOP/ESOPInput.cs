using SIE.Wpf.Common.Diagram;
using System.ComponentModel;

namespace SIE.Wpf.MES.Workbench.ESOP
{
    /// <summary>
    /// 生产工作台ESOP-输入参数
    /// </summary>
    public class ESOPInput : ComponentInput<ESOPControl>
    {
        /// <summary>
        /// 产品Id
        /// </summary>
        [Description("产品ID")]
        [DisplayName("检验采集产品切换时传入")]
        public virtual double ProductId { get; set; }
    }
}