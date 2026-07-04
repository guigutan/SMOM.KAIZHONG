using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.Common.Logs
{
    /// <summary>
    /// 接口下载异常管理
    /// </summary>
    [RootEntity, Serializable]
    [Label("接口下载异常管理")]
    public partial class DownloadExcViewModel : ViewModel
    {
        #region 任务类型 JobType
        /// <summary>
        /// 任务类型
        /// </summary>
        [Label("任务类型")]
        public static readonly Property<string> JobTypeProperty = P<DownloadExcViewModel>.Register(e => e.JobType);

        /// <summary>
        /// 任务类型
        /// </summary>
        public string JobType
        {
            get { return this.GetProperty(JobTypeProperty); }
            set { this.SetProperty(JobTypeProperty, value); }
        }
        #endregion

        #region 数据量 DataCount
        /// <summary>
        /// 数据量
        /// </summary>
        [Label("数据量")]
        public static readonly Property<int> DataCountProperty = P<DownloadExcViewModel>.Register(e => e.DataCount);

        /// <summary>
        /// 数据量
        /// </summary>
        public int DataCount
        {
            get { return GetProperty(DataCountProperty); }
            set { SetProperty(DataCountProperty, value); }
        }
        #endregion

        #region 成功数 SuccessCount
        /// <summary>
        /// 成功数
        /// </summary>
        [Label("成功数")]
        public static readonly Property<int> SuccessCountProperty = P<DownloadExcViewModel>.Register(e => e.SuccessCount);

        /// <summary>
        /// 成功数
        /// </summary>
        public int SuccessCount
        {
            get { return GetProperty(SuccessCountProperty); }
            set { SetProperty(SuccessCountProperty, value); }
        }
        #endregion

        #region 失败数 FailCount
        /// <summary>
        /// 失败数
        /// </summary>
        [Label("失败数")]
        public static readonly Property<int> FailCountProperty = P<DownloadExcViewModel>.Register(e => e.FailCount);

        /// <summary>
        /// 失败数
        /// </summary>
        public int FailCount
        {
            get { return GetProperty(FailCountProperty); }
            set { SetProperty(FailCountProperty, value); }
        }
        #endregion

        #region 未处理数 UntreatedCount
        /// <summary>
        /// 未处理数
        /// </summary>
        [Label("未处理数")]
        public static readonly Property<int> UntreatedCountProperty = P<DownloadExcViewModel>.Register(e => e.UntreatedCount);

        /// <summary>
        /// 未处理数
        /// </summary>
        public int UntreatedCount
        {
            get { return this.GetProperty(UntreatedCountProperty); }
            set { this.SetProperty(UntreatedCountProperty, value); }
        }
        #endregion

    }
}
