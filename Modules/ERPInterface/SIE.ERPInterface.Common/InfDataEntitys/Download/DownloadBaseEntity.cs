using SIE.Domain;
using SIE.ERPInterface.Common.Enums;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;

namespace SIE.ERPInterface.Common.InfDataEntitys.Download
{
    /// <summary>
    /// 下载接口中间表基类
    /// </summary>
    [RootEntity, Serializable]
    //[CriteriaQuery]
    [Label("下载接口中间表基类")]
    public partial class DownloadBaseEntity : StringEntity
    {
        #region 处理时间 ProcessDate
        /// <summary>
        /// 处理时间
        /// </summary>
        [Label("处理时间")]
        public static readonly Property<DateTime?> ProcessDateProperty = P<DownloadBaseEntity>.Register(e => e.ProcessDate);

        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime? ProcessDate
        {
            get { return GetProperty(ProcessDateProperty); }
            set { SetProperty(ProcessDateProperty, value); }
        }
        #endregion

        #region 重试次数 RetryCount
        /// <summary>
        /// 重试次数
        /// </summary>
        [Required]
        [Label("重试次数")]
        public static readonly Property<int> RetryCountProperty = P<DownloadBaseEntity>.Register(e => e.RetryCount);

        /// <summary>
        /// 重试次数
        /// </summary>
        public int RetryCount
        {
            get { return GetProperty(RetryCountProperty); }
            set { SetProperty(RetryCountProperty, value); }
        }
        #endregion

        #region 最后更新时间 LastUpdateDate
        /// <summary>
        /// 最后更新时间
        /// </summary>
        [Label("最后更新时间")]
        public static readonly Property<DateTime?> LastUpdateDateProperty = P<DownloadBaseEntity>.Register(e => e.LastUpdateDate);

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? LastUpdateDate
        {
            get { return GetProperty(LastUpdateDateProperty); }
            set { SetProperty(LastUpdateDateProperty, value); }
        }
        #endregion

        #region ERP主键 ErpKey
        /// <summary>
        /// ERP主键
        /// </summary>
        [Required]
        [Label("ERP主键")]
        public static readonly Property<string> ErpKeyProperty = P<DownloadBaseEntity>.Register(e => e.ErpKey);

        /// <summary>
        /// ERP主键
        /// </summary>
        public string ErpKey
        {
            get { return GetProperty(ErpKeyProperty); }
            set { SetProperty(ErpKeyProperty, value); }
        }
        #endregion

        #region 最大重试次数 MaxRetryCount
        /// <summary>
        /// 最大重试次数
        /// </summary>
        [Label("最大重试次数")]
        public static readonly Property<int> MaxRetryCountProperty = P<DownloadBaseEntity>.Register(e => e.MaxRetryCount);

        /// <summary>
        /// 最大重试次数
        /// </summary>
        public int MaxRetryCount
        {
            get { return GetProperty(MaxRetryCountProperty); }
            set { SetProperty(MaxRetryCountProperty, value); }
        }
        #endregion

        #region 处理状态 State
        /// <summary>
        /// 处理状态
        /// </summary>
        [Label("处理状态")]
        public static readonly Property<ProcessState> StateProperty = P<DownloadBaseEntity>.Register(e => e.State);

        /// <summary>
        /// 处理状态
        /// </summary>
        public ProcessState State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 是否删除 IsDelete
        /// <summary>
        /// 是否删除
        /// </summary>
        [Label("是否删除")]
        public static readonly Property<bool> IsDeleteProperty = P<DownloadBaseEntity>.Register(e => e.IsDelete);

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete
        {
            get { return this.GetProperty(IsDeleteProperty); }
            set { this.SetProperty(IsDeleteProperty, value); }
        }
        #endregion

        #region 是否手动下载 IsManual
        /// <summary>
        /// 是否手动下载
        /// </summary>
        [Label("是否手动下载")]
        public static readonly Property<bool> IsManualProperty = P<DownloadBaseEntity>.Register(e => e.IsManual);

        /// <summary>
        /// 是否手动下载
        /// </summary>
        public bool IsManual
        {
            get { return this.GetProperty(IsManualProperty); }
            set { this.SetProperty(IsManualProperty, value); }
        }
        #endregion

        /// <summary>
        /// 子数据集合
        /// </summary>
        public List<DownloadBaseEntity> Children { get; set; } = new List<DownloadBaseEntity>();

    }
}