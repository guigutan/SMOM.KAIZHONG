using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.Common.Logs
{
    /// <summary>
    /// 下载任务时间戳明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("任务时间戳明细")]
    public partial class DownloadJobTimeDetail : DataEntity
    {
        #region 请求状态 State
        /// <summary>
        /// 请求状态
        /// </summary>
        [Label("请求状态")]
        public static readonly Property<RequestState> StateProperty = P<DownloadJobTimeDetail>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public RequestState State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region ERP Batch ID ErpBatchId
        /// <summary>
        /// ERP Batch ID
        /// </summary>
        [Label("ERP Batch ID")]
        public static readonly Property<double> ErpBatchIdProperty = P<DownloadJobTimeDetail>.Register(e => e.ErpBatchId);

        /// <summary>
        /// ERP Batch ID
        /// </summary>
        public double ErpBatchId
        {
            get { return this.GetProperty(ErpBatchIdProperty); }
            set { this.SetProperty(ErpBatchIdProperty, value); }
        }
        #endregion

        #region 返回编码 ResponseCode
        /// <summary>
        /// 返回编码
        /// </summary>
        [Label("返回编码")]
        public static readonly Property<string> ResponseCodeProperty = P<DownloadJobTimeDetail>.Register(e => e.ResponseCode);

        /// <summary>
        /// 返回编码
        /// </summary>
        public string ResponseCode
        {
            get { return this.GetProperty(ResponseCodeProperty); }
            set { this.SetProperty(ResponseCodeProperty, value); }
        }
        #endregion

        #region 返回状态说明 ResponseMessage
        /// <summary>
        /// 返回状态说明
        /// </summary>
        [Label("返回状态说明")]
        public static readonly Property<string> ResponseMessageProperty = P<DownloadJobTimeDetail>.Register(e => e.ResponseMessage);

        /// <summary>
        /// 返回状态说明
        /// </summary>
        public string ResponseMessage
        {
            get { return this.GetProperty(ResponseMessageProperty); }
            set { this.SetProperty(ResponseMessageProperty, value); }
        }
        #endregion

        #region 请求报文 RequestStr
        /// <summary>
        /// 请求报文
        /// </summary>
        [Label("请求报文")]
        public static readonly Property<string> RequestStrProperty = P<DownloadJobTimeDetail>.Register(e => e.RequestStr);

        /// <summary>
        /// 请求报文
        /// </summary>
        public string RequestStr
        {
            get { return this.GetProperty(RequestStrProperty); }
            set { this.SetProperty(RequestStrProperty, value); }
        }
        #endregion

        #region 接收报文 ResponseStr
        /// <summary>
        /// 接收报文
        /// </summary>
        [Label("接收报文")]
        public static readonly Property<string> ResponseStrProperty = P<DownloadJobTimeDetail>.Register(e => e.ResponseStr);

        /// <summary>
        /// 接收报文
        /// </summary>
        public string ResponseStr
        {
            get { return this.GetProperty(ResponseStrProperty); }
            set { this.SetProperty(ResponseStrProperty, value); }
        }
        #endregion

        #region 请求时间 RequestDate
        /// <summary>
        /// 请求时间
        /// </summary>
        [Label("请求时间")]
        public static readonly Property<DateTime> RequestDateProperty = P<DownloadJobTimeDetail>.Register(e => e.RequestDate);

        /// <summary>
        /// 请求时间
        /// </summary>
        public DateTime RequestDate
        {
            get { return this.GetProperty(RequestDateProperty); }
            set { this.SetProperty(RequestDateProperty, value); }
        }
        #endregion

        #region 接收时间 ResponseDate
        /// <summary>
        /// 接收时间
        /// </summary>
        [Label("接收时间")]
        public static readonly Property<DateTime> ResponseDateProperty = P<DownloadJobTimeDetail>.Register(e => e.ResponseDate);

        /// <summary>
        /// 接收时间
        /// </summary>
        public DateTime ResponseDate
        {
            get { return this.GetProperty(ResponseDateProperty); }
            set { this.SetProperty(ResponseDateProperty, value); }
        }
        #endregion

        #region 任务下载时间 DownloadJobTime
        /// <summary>
        /// 任务下载时间Id
        /// </summary>
        [Label("任务下载时间")]
        public static readonly IRefIdProperty DownloadJobTimeIdProperty =
            P<DownloadJobTimeDetail>.RegisterRefId(e => e.DownloadJobTimeId, ReferenceType.Parent);

        /// <summary>
        /// 任务下载时间Id
        /// </summary>
        public double DownloadJobTimeId
        {
            get { return (double)this.GetRefId(DownloadJobTimeIdProperty); }
            set { this.SetRefId(DownloadJobTimeIdProperty, value); }
        }

        /// <summary>
        /// 任务下载时间
        /// </summary>
        public static readonly RefEntityProperty<DownloadJobTime> DownloadJobTimeProperty =
            P<DownloadJobTimeDetail>.RegisterRef(e => e.DownloadJobTime, DownloadJobTimeIdProperty);

        /// <summary>
        /// 任务下载时间
        /// </summary>
        public DownloadJobTime DownloadJobTime
        {
            get { return this.GetRefEntity(DownloadJobTimeProperty); }
            set { this.SetRefEntity(DownloadJobTimeProperty, value); }
        }
        #endregion

        #region 是否手动 IsManual
        /// <summary>
        /// 是否手动
        /// </summary>
        [Label("是否手动")]
        public static readonly Property<bool> IsManualProperty = P<DownloadJobTimeDetail>.Register(e => e.IsManual);

        /// <summary>
        /// 是否手动
        /// </summary>
        public bool IsManual
        {
            get { return this.GetProperty(IsManualProperty); }
            set { this.SetProperty(IsManualProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 任务下载时间 实体配置
    /// </summary>
    internal class DownloadJobTimeDetailConfig : EntityConfig<DownloadJobTimeDetail>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(DownloadJobTimeDetail.ResponseMessageProperty, new StringLengthRangeRule() { Max = 4000 });
        }

        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("DL_JOB_TIME_DTL").MapAllProperties();
            Meta.Property(DownloadJobTimeDetail.ResponseMessageProperty).MapColumn().HasLength("4000");
            Meta.Property(DownloadJobTimeDetail.RequestStrProperty).MapColumn().HasLength("MAX");
            Meta.Property(DownloadJobTimeDetail.ResponseStrProperty).MapColumn().HasLength("MAX");
            Meta.EnablePhantoms();
        }
    }
}