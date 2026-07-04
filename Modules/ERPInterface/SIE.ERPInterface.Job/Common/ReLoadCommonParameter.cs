using SIE.Common.Schdules;
using SIE.Domain;
using SIE.ERPInterface.Common.UploadTransactionRules;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.Job.Common
{
    /// <summary>
    /// ERP上传接口最大重传次数
    /// </summary>
    [RootEntity, Serializable]
    public class ReLoadCommonParameter : JobParameter
    {
        #region 最大重传ERP次数 MaxUploadCount
        /// <summary>
        /// 最大重传ERP次数
        /// </summary>
        [Label("最大重传ERP次数")]
        public static readonly Property<int> MaxUploadCountProperty = P<ReLoadCommonParameter>.Register(e => e.MaxUploadCount);

        /// <summary>
        /// 最大重传ERP次数
        /// </summary>
        public int MaxUploadCount
        {
            get { return this.GetProperty(MaxUploadCountProperty); }
            set { this.SetProperty(MaxUploadCountProperty, value); }
        }
        #endregion

    }

    class ReLoadCommonParameterWebViewConfig : WebViewConfig<ReLoadCommonParameter>
    {
        protected override void ConfigView()
        {
            View.Property(p => p.MaxUploadCount).Show();
        }
    }
    /// <summary>
    /// 参数视图配置
    /// </summary>
    class ReLoadCommonParameterWPFViewConfig : WPFViewConfig<ReLoadCommonParameter>
    {
        protected override void ConfigView()
        {
            View.Property(p => p.MaxUploadCount).Show();
        }
    }
}
