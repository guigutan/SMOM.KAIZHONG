using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Web.RedCardManagment.RedCardApplyBills.ViewModels
{
    /// <summary>
    /// 取消ViewModel
    /// </summary>
    [RootEntity, Serializable]
    public class CancelReasonViewModel : ViewModel
    {
        #region 红牌申请单Id RedCardApplyBillId
        /// <summary>
        /// 红牌申请单Id
        /// </summary>
        [Label("红牌申请单Id")]
        public static readonly Property<double> RedCardApplyBillIdProperty = P<CancelReasonViewModel>.Register(e => e.RedCardApplyBillId);

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
        public static readonly Property<string> ReasonProperty = P<CancelReasonViewModel>.Register(e => e.Reason);

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
    /// 取消ViewModel视图配置
    /// </summary>
    internal class CancelReasonViewModelViewConfig : WebViewConfig<CancelReasonViewModel>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("取消").HasDelegate(CancelReasonViewModel.ReasonProperty);
            using (View.OrderProperties())
            {
                View.Property(p => p.Reason).HasLabel("取消原因").UseMemoEditor().ShowInDetail(rowSpan: 3);
            }
        }
    }
}