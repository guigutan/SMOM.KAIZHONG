using SIE.Common.Schdules;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.Job.Common
{
    /// <summary>
    /// ERP下载接口调度通用参数
    /// </summary>
    [RootEntity, Serializable]
    public class DLCommonParameter : JobParameter
    {
        #region 是否下载中间表 IsDownloadInf
        /// <summary>
        /// 是否下载中间表
        /// </summary>
        [Label("是否下载中间表")]
        public static readonly Property<bool> IsDownloadInfProperty = P<DLCommonParameter>.Register(e => e.IsDownloadInf);

        /// <summary>
        /// 是否下载中间表
        /// </summary>
        public bool IsDownloadInf
        {
            get { return this.GetProperty(IsDownloadInfProperty); }
            set { this.SetProperty(IsDownloadInfProperty, value); }
        }
        #endregion
        
        #region 最大重试次数 MaxRetryQty

        /// <summary>
        /// 最大重试次数
        /// </summary>
        [Label("最大重试次数")] 
        public static readonly Property<int?> MaxRetryQtyProperty = P<DLCommonParameter>.Register(e => e.MaxRetryQty);

        /// <summary>
        /// 最大重试次数
        /// </summary>
        public int? MaxRetryQty
        {
            get { return GetProperty(MaxRetryQtyProperty); }
            set { SetProperty(MaxRetryQtyProperty, value); }
        }

        #endregion
        
        #region 最大处理数量 MaxBatchQty

        /// <summary>
        /// 最大处理数量
        /// </summary>
        [Label("最大处理数量")] 
        public static readonly Property<int?> MaxBatchQtyProperty = P<DLCommonParameter>.Register(e => e.MaxBatchQty);

        /// <summary>
        /// 最大处理数量
        /// </summary>
        public int? MaxBatchQty
        {
            get { return GetProperty(MaxBatchQtyProperty); }
            set { SetProperty(MaxBatchQtyProperty, value); }
        }

        #endregion
    }

    class DLCommonParameterWebViewConfig : WebViewConfig<DLCommonParameter>
    {
        protected override void ConfigView()
        {
            View.Property(p => p.IsDownloadInf).Show();
            View.Property(p => p.MaxRetryQty).Show();
            View.Property(p => p.MaxBatchQty).Show();
        }
    }
    /// <summary>
    /// 参数视图配置
    /// </summary>
    class DLCommonParameterWPFViewConfig : WPFViewConfig<DLCommonParameter>
    {
        protected override void ConfigView()
        {
            View.Property(p => p.IsDownloadInf).Show();
            View.Property(p => p.MaxRetryQty).Show();
            View.Property(p => p.MaxBatchQty).Show();
        }
    }
}
