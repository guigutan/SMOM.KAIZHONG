using SIE.Domain;
using SIE.ObjectModel;
using SIE.Wpf.MES.PanelBindings.Commands;
using SIE.Wpf.MES.PanelBindings.ViewBehaviors;
using System;

namespace SIE.Wpf.MES.PanelBindings
{
    /// <summary>
    /// 拼板码视图模型
    /// </summary>
    [RootEntity]
    [Label("拼板码视图模型")]
    public class PanelViewModel : ViewModel
    {
        #region 拼板码 Panel
        /// <summary>
        /// 拼板码
        /// </summary>
        [Label("拼板码")]
        public static readonly Property<string> PanelProperty = P<PanelViewModel>.Register(e => e.Panel);

        /// <summary>
        /// 拼板码
        /// </summary>
        public string Panel
        {
            get { return this.GetProperty(PanelProperty); }
            set { this.SetProperty(PanelProperty, value); }
        }
        #endregion

        #region 可绑定产品数量 CanBindQty
        /// <summary>
        /// 可绑定产品数量
        /// </summary>
        [Label("可绑定产品数量")]
        public static readonly Property<int> CanBindQtyProperty = P<PanelViewModel>.Register(e => e.CanBindQty);

        /// <summary>
        /// 可绑定产品数量
        /// </summary>
        public int CanBindQty
        {
            get { return this.GetProperty(CanBindQtyProperty); }
            set { this.SetProperty(CanBindQtyProperty, value); }
        }
        #endregion

        #region 绑定数量 BindingQty
        /// <summary>
        /// 绑定数量
        /// </summary>
        [Label("绑定数量")]
        public static readonly Property<decimal> BindingQtyProperty = P<PanelViewModel>.Register(e => e.BindingQty);

        /// <summary>
        /// 绑定数量
        /// </summary>
        public decimal BindingQty
        {
            get { return this.GetProperty(BindingQtyProperty); }
            set { this.SetProperty(BindingQtyProperty, value); }
        }
        #endregion

        #region 绑定时间 BindingDate
        /// <summary>
        /// 绑定时间
        /// </summary>
        [Label("绑定时间")]
        public static readonly Property<DateTime?> BindingDateProperty = P<PanelViewModel>.Register(e => e.BindingDate);

        /// <summary>
        /// 绑定时间
        /// </summary>
        public DateTime? BindingDate
        {
            get { return this.GetProperty(BindingDateProperty); }
            set { this.SetProperty(BindingDateProperty, value); }
        }
        #endregion

        #region 操作人 OperatorName
        /// <summary>
        /// 操作人
        /// </summary>
        [Label("操作人")]
        public static readonly Property<string> OperatorNameProperty = P<PanelViewModel>.Register(e => e.OperatorName);

        /// <summary>
        /// 操作人
        /// </summary>
        public string OperatorName
        {
            get { return this.GetProperty(OperatorNameProperty); }
            set { this.SetProperty(OperatorNameProperty, value); }
        }
        #endregion

        #region 完成绑定 IsBindComplete
        /// <summary>
        /// 完成绑定
        /// </summary>
        [Label("完成绑定")]
        public static readonly Property<bool> IsBindCompleteProperty = P<PanelViewModel>.Register(e => e.IsBindComplete);

        /// <summary>
        /// 完成绑定
        /// </summary>
        public bool IsBindComplete
        {
            get { return this.GetProperty(IsBindCompleteProperty); }
            set { this.SetProperty(IsBindCompleteProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 拼板码视图模型视图配置
    /// </summary>
    internal class PanelViewModelViewConfig : WPFViewConfig<PanelViewModel>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior(typeof(PanelBindingBehavior));
            View.UseCommands(typeof(RemovePanelCommand), typeof(ManualBindingCommand));
            View.Property(p => p.Panel);
            View.Property(p => p.CanBindQty);
            View.Property(p => p.BindingQty);
            View.Property(p => p.BindingDate);
            View.Property(p => p.OperatorName);
        }
    }
}