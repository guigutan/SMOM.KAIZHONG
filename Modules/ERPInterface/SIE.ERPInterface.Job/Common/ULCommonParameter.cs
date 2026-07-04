using SIE.Common.Schdules;
using SIE.Domain;
using SIE.ERPInterface.Common.UploadTransactionRules;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.Job.Common
{
    /// <summary>
    /// ERP上传接口调度通用参数
    /// </summary>
    [RootEntity, Serializable]
    public class ULCommonParameter : JobParameter
    {
        #region 交易上传规则 UploadTransactionRule
        /// <summary>
        /// 交易上传规则Id
        /// </summary>
        [Label("交易上传规则")]
        public static readonly IRefIdProperty UploadTransactionRuleIdProperty =
            P<ULCommonParameter>.RegisterRefId(e => e.UploadTransactionRuleId, ReferenceType.Normal);

        /// <summary>
        /// 交易上传规则Id
        /// </summary>
        public double? UploadTransactionRuleId
        {
            get { return (double?)this.GetRefNullableId(UploadTransactionRuleIdProperty); }
            set { this.SetRefNullableId(UploadTransactionRuleIdProperty, value); }
        }

        /// <summary>
        /// 交易上传规则
        /// </summary>
        public static readonly RefEntityProperty<UploadTransactionRule> UploadTransactionRuleProperty =
            P<ULCommonParameter>.RegisterRef(e => e.UploadTransactionRule, UploadTransactionRuleIdProperty);

        /// <summary>
        /// 交易上传规则
        /// </summary>
        public UploadTransactionRule UploadTransactionRule
        {
            get { return this.GetRefEntity(UploadTransactionRuleProperty); }
            set { this.SetRefEntity(UploadTransactionRuleProperty, value); }
        }
        #endregion
    }

    class ULCommonParameterWebViewConfig : WebViewConfig<ULCommonParameter>
    {
        protected override void ConfigView()
        {
            View.Property(p => p.UploadTransactionRuleId).Show();
        }
    }
    /// <summary>
    /// 参数视图配置
    /// </summary>
    class ULCommonParameterWPFViewConfig : WPFViewConfig<ULCommonParameter>
    {
        protected override void ConfigView()
        {
            View.Property(p => p.UploadTransactionRule).Show();
        }
    }
}
