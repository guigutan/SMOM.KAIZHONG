using SIE.Wpf.Common.Diagram;
using System.ComponentModel;

namespace SIE.Wpf.MES.Workbench.AndonAbnormals
{
    /// <summary>
    /// 安灯异常输入类
    /// </summary>
    public class AndonAbnormalInput : ComponentInput<AndonAbnormalControl>
    {
        /// <summary>
        /// 输入参数--资源Id(产线)
        /// </summary>
        [DisplayName("资源Id")]
        [Description("工序工位组件选择资源后传入")]
        public virtual double ResourceId { get; set; }
    }
}