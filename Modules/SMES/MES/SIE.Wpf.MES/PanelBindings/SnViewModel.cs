using SIE.Domain;
using SIE.ObjectModel;
using SIE.Wpf.MES.PanelBindings.Commands;

namespace SIE.Wpf.MES.PanelBindings
{
    /// <summary>
    /// 条码视图模型
    /// </summary>
    [RootEntity]
    [Label("条码视图模型")]
    public class SnViewModel : ViewModel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SnViewModel()
        {
            IsBinding = YesNo.No;
        }
        
        #region SN条码 Sn
        /// <summary>
        /// SN条码
        /// </summary>
        [Label("SN条码")]
        public static readonly Property<string> SnProperty = P<SnViewModel>.Register(e => e.Sn);

        /// <summary>
        /// SN条码
        /// </summary>
        public string Sn
        {
            get { return this.GetProperty(SnProperty); }
            set { this.SetProperty(SnProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<SnViewModel>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion 

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<SnViewModel>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<SnViewModel>.Register(e => e.ProductCode);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
            set { this.SetProperty(ProductCodeProperty, value); }
        }
        #endregion

        #region 产品名称 ProducName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProducNameProperty = P<SnViewModel>.Register(e => e.ProducName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProducName
        {
            get { return this.GetProperty(ProducNameProperty); }
            set { this.SetProperty(ProducNameProperty, value); }
        }
        #endregion

        #region 是否已绑定 IsBinding
        /// <summary>
        /// 是否已绑定
        /// </summary>
        [Label("是否已绑定")]
        public static readonly Property<YesNo> IsBindingProperty = P<SnViewModel>.Register(e => e.IsBinding);

        /// <summary>
        /// 是否已绑定
        /// </summary>
        public YesNo IsBinding
        {
            get { return this.GetProperty(IsBindingProperty); }
            set { this.SetProperty(IsBindingProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 条码视图模型视图配置
    /// </summary>
    internal class SnViewModelViewConfig : WPFViewConfig<SnViewModel>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(RemoveSnCommand));
            View.Property(p => p.Sn);
            View.Property(p => p.Qty);
            View.Property(p => p.WorkOrderNo);
            View.Property(p => p.ProductCode);
            View.Property(p => p.ProducName);
        }
    }
}