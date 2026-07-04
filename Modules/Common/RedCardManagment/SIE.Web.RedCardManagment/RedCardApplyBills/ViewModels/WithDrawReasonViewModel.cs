using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Web.RedCardManagment.RedCardApplyBills.ViewModels
{
    /// <summary>
    /// 撤回ViewModel
    /// </summary>
    [RootEntity, Serializable]
    public class WithDrawReasonViewModel : ViewModel
    {
        #region 红牌申请单Id RedCardApplyBillId
        /// <summary>
        /// 红牌申请单Id
        /// </summary>
        [Label("红牌申请单Id")]
        public static readonly Property<double> RedCardApplyBillIdProperty = P<WithDrawReasonViewModel>.Register(e => e.RedCardApplyBillId);

        /// <summary>
        /// 红牌申请单Id
        /// </summary>
        public double RedCardApplyBillId
        {
            get { return this.GetProperty(RedCardApplyBillIdProperty); }
            set { this.SetProperty(RedCardApplyBillIdProperty, value); }
        }
        #endregion

        #region 原因 Reason
        /// <summary>
        /// 原因
        /// </summary>
        [Label("原因")]
        [MaxLength(1000)]
        public static readonly Property<string> ReasonProperty = P<WithDrawReasonViewModel>.Register(e => e.Reason);

        /// <summary>
        /// 原因
        /// </summary>
        public string Reason
        {
            get { return this.GetProperty(ReasonProperty); }
            set { this.SetProperty(ReasonProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 撤回ViewModel视图配置
    /// </summary>
    internal class WithDrawReasonViewModelViewConfig : WebViewConfig<WithDrawReasonViewModel>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("撤回").HasDelegate(WithDrawReasonViewModel.ReasonProperty);
            using (View.OrderProperties())
            {
                View.Property(p => p.Reason).HasLabel("撤回原因").UseMemoEditor().ShowInDetail(rowSpan: 3);
            }
        }
    }
}