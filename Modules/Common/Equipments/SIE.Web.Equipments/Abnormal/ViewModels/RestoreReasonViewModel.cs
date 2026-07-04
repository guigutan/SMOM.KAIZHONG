using SIE.Domain;
using SIE.Equipments.Abnormal;
using SIE.ObjectModel;
using System;

namespace SIE.Web.Equipments.Abnormal.ViewModels
{
    /// <summary>
    /// 停线解除ViewModel
    /// </summary>
    [RootEntity, Serializable]
    public class RestoreReasonViewModel : ViewModel
    {
        #region 停线管理 AbnormalCause
        /// <summary>
        /// 停线管理Id
        /// </summary>
        [Label("停线管理")]
        public static readonly IRefIdProperty AbnormalCauseIdProperty =
            P<RestoreReasonViewModel>.RegisterRefId(e => e.AbnormalCauseId, ReferenceType.Normal);

        /// <summary>
        /// 停线管理Id
        /// </summary>
        public double AbnormalCauseId
        {
            get { return (double)this.GetRefId(AbnormalCauseIdProperty); }
            set { this.SetRefId(AbnormalCauseIdProperty, value); }
        }

        /// <summary>
        /// 停线管理
        /// </summary>
        public static readonly RefEntityProperty<AbnormalCause> AbnormalCauseProperty =
            P<RestoreReasonViewModel>.RegisterRef(e => e.AbnormalCause, AbnormalCauseIdProperty);

        /// <summary>
        /// 停线管理
        /// </summary>
        public AbnormalCause AbnormalCause
        {
            get { return this.GetRefEntity(AbnormalCauseProperty); }
            set { this.SetRefEntity(AbnormalCauseProperty, value); }
        }
        #endregion


        #region 原因 Reason
        /// <summary>
        /// 原因
        /// </summary>
        [Label("原因")]
        public static readonly Property<string> ReasonProperty = P<RestoreReasonViewModel>.Register(e => e.Reason);

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
    /// 解除原因ViewModel视图配置
    /// </summary>
    internal class RestoreReasonViewModelViewConfig : WebViewConfig<RestoreReasonViewModel>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("停线解除").HasDelegate(RestoreReasonViewModel.ReasonProperty);
            using (View.OrderProperties())
            {
                View.Property(p => p.Reason).HasLabel("解除原因").UseMemoEditor().ShowInDetail(rowSpan: 3);
            }
        }
    }
}
