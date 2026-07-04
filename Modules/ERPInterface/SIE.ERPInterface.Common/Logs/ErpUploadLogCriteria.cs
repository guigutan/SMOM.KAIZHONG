using SIE.Core.Enums;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.Common.Logs
{
    /// <summary>
    /// 事物上传ERP记录查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("事物上传ERP记录查询实体")]
    public partial class ErpUploadLogCriteria: Criteria
    {
        #region 单号 OrderNo
        /// <summary>
        /// 单号
        /// </summary>
        [Label("单号")]
        public static readonly Property<string> OrderNoProperty = P<ErpUploadLogCriteria>.Register(e => e.OrderNo);

        /// <summary>
        /// 单号
        /// </summary>
        public string OrderNo
        {
            get { return this.GetProperty(OrderNoProperty); }
            set { this.SetProperty(OrderNoProperty, value); }
        }
        #endregion

        #region 单据类型 OrderType
        /// <summary>
        /// 单据类型
        /// </summary>
        [Label("单据类型")]
        public static readonly Property<string> OrderTypeProperty = P<ErpUploadLogCriteria>.Register(e => e.OrderType);

        /// <summary>
        /// 单据类型
        /// </summary>
        public string OrderType
        {
            get { return this.GetProperty(OrderTypeProperty); }
            set { this.SetProperty(OrderTypeProperty, value); }
        }
        #endregion

        #region 是否成功 IsSuccess
        /// <summary>
        /// 是否成功
        /// </summary>
        [Label("是否成功")]
        public static readonly Property<bool?> IsSuccessProperty = P<ErpUploadLogCriteria>.Register(e => e.IsSuccess);

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool? IsSuccess
        {
            get { return this.GetProperty(IsSuccessProperty); }
            set { this.SetProperty(IsSuccessProperty, value); }
        }
        #endregion

        #region 上传时间 LogDate
        /// <summary>
        /// 上传时间
        /// </summary>
        [Label("上传时间")]
        public static readonly Property<DateRange> LogDateProperty = P<ErpUploadLogCriteria>.Register(e => e.LogDate);

        /// <summary>
        /// 上传时间
        /// </summary>
        public DateRange LogDate
        {
            get { return this.GetProperty(LogDateProperty); }
            set { this.SetProperty(LogDateProperty, value); }
        }
        #endregion

        #region 事务交易ID TransactionId
        /// <summary>
        /// 事务交易ID
        /// </summary>
        [Label("事务交易ID")]
        public static readonly Property<double?> TransactionIdProperty = P<ErpUploadLogCriteria>.Register(e => e.TransactionId);

        /// <summary>
        /// 事务交易ID
        /// </summary>
        public double? TransactionId
        {
            get { return this.GetProperty(TransactionIdProperty); }
            set { this.SetProperty(TransactionIdProperty, value); }
        }
        #endregion

        /// <summary>
		/// 重写此方法实现查询
		/// </summary>
		protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ErpUploadLogController>().GetErpUploadLogs(this);
        }
    }
}
