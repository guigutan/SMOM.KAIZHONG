using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.Common.Logs
{
    /// <summary>
    /// 事务上传记录
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("事务上传记录")]
    public partial class UploadTransactionLog : DataEntity
    {
        #region ERP Batch ID ErpBatchId
        /// <summary>
        /// ERP Batch ID
        /// </summary>
        [Label("ERP Batch ID")]
        public static readonly Property<double?> ErpBatchIdProperty = P<UploadTransactionLog>.Register(e => e.ErpBatchId);

        /// <summary>
        /// ERP Batch ID
        /// </summary>
        public double? ErpBatchId
        {
            get { return GetProperty(ErpBatchIdProperty); }
            set { SetProperty(ErpBatchIdProperty, value); }
        }
        #endregion

        #region 返回编码 ResponseCode
        /// <summary>
        /// 返回编码
        /// </summary>
        [Label("返回编码")]
        public static readonly Property<string> ResponseCodeProperty = P<UploadTransactionLog>.Register(e => e.ResponseCode);

        /// <summary>
        /// 返回编码
        /// </summary>
        public string ResponseCode
        {
            get { return GetProperty(ResponseCodeProperty); }
            set { SetProperty(ResponseCodeProperty, value); }
        }
        #endregion

        #region 返回状态说明 ResponseMessage
        /// <summary>
        /// 返回状态说明
        /// </summary>
        [Label("返回状态说明")]
        public static readonly Property<string> ResponseMessageProperty = P<UploadTransactionLog>.Register(e => e.ResponseMessage);

        /// <summary>
        /// 返回状态说明
        /// </summary>
        public string ResponseMessage
        {
            get { return GetProperty(ResponseMessageProperty); }
            set { SetProperty(ResponseMessageProperty, value); }
        }
        #endregion

        #region 请求报文 RequestStr
        /// <summary>
        /// 请求报文
        /// </summary>
        [Label("请求报文")]
        public static readonly Property<string> RequestStrProperty = P<UploadTransactionLog>.Register(e => e.RequestStr);

        /// <summary>
        /// 请求报文
        /// </summary>
        public string RequestStr
        {
            get { return GetProperty(RequestStrProperty); }
            set { SetProperty(RequestStrProperty, value); }
        }
        #endregion

        #region 接收报文 ResponseStr
        /// <summary>
        /// 接收报文
        /// </summary>
        [Label("接收报文")]
        public static readonly Property<string> ResponseStrProperty = P<UploadTransactionLog>.Register(e => e.ResponseStr);

        /// <summary>
        /// 接收报文
        /// </summary>
        public string ResponseStr
        {
            get { return GetProperty(ResponseStrProperty); }
            set { SetProperty(ResponseStrProperty, value); }
        }
        #endregion

        #region 请求时间 RequestDate
        /// <summary>
        /// 请求时间
        /// </summary>
        [Label("请求时间")]
        public static readonly Property<DateTime> RequestDateProperty = P<UploadTransactionLog>.Register(e => e.RequestDate);

        /// <summary>
        /// 请求时间
        /// </summary>
        public DateTime RequestDate
        {
            get { return GetProperty(RequestDateProperty); }
            set { SetProperty(RequestDateProperty, value); }
        }
        #endregion

        #region 接收时间 ResponseDate
        /// <summary>
        /// 接收时间
        /// </summary>
        [Label("接收时间")]
        public static readonly Property<DateTime> ResponseDateProperty = P<UploadTransactionLog>.Register(e => e.ResponseDate);

        /// <summary>
        /// 接收时间
        /// </summary>
        public DateTime ResponseDate
        {
            get { return GetProperty(ResponseDateProperty); }
            set { SetProperty(ResponseDateProperty, value); }
        }
        #endregion

        #region 事务上传记录明细 UploadTransactionLogDtlList
        /// <summary>
        /// 事务上传记录明细
        /// </summary>
        [Label("事务上传记录明细")]
        public static readonly ListProperty<EntityList<UploadTransactionLogDtl>> UploadTransactionLogDtlListProperty = P<UploadTransactionLog>.RegisterList(e => e.UploadTransactionLogDtlList);

        /// <summary>
        /// 事务上传记录明细
        /// </summary>
        public EntityList<UploadTransactionLogDtl> UploadTransactionLogDtlList
        {
            get { return this.GetLazyList(UploadTransactionLogDtlListProperty); }
        }
        #endregion

    }

    /// <summary>
    /// 事务上传记录 实体配置
    /// </summary>
    internal class UploadTransactionLogConfig : EntityConfig<UploadTransactionLog>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(UploadTransactionLog.ResponseMessageProperty, new StringLengthRangeRule() { Max = 4000 });
        }

        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("UL_TRANS_LOG").MapAllProperties();
            Meta.Property(UploadTransactionLog.ResponseCodeProperty).ColumnMeta.HasLength("MAX");
            Meta.Property(UploadTransactionLog.ResponseMessageProperty).ColumnMeta.HasLength(4000);
            Meta.Property(UploadTransactionLog.RequestStrProperty).ColumnMeta.HasLength("MAX");
            Meta.Property(UploadTransactionLog.ResponseStrProperty).ColumnMeta.HasLength("MAX");
            Meta.EnablePhantoms();
        }
    }
}