using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Enums;
using SIE.Inventory.TransactionProcessing;
using SIE.Inventory.Transactions;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.Common.Logs
{
    /// <summary>
    /// 事务上传记录
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ErpUploadLogCriteria))]
    [Label("事务上传记录")]
    public partial class ErpUploadLog : DataEntity
    {
        #region 单号 OrderNo
        /// <summary>
        /// 单号
        /// </summary>
        [Label("单号")]
        public static readonly Property<string> OrderNoProperty = P<ErpUploadLog>.Register(e => e.OrderNo);

        /// <summary>
        /// 单号
        /// </summary>
        public string OrderNo
        {
            get { return this.GetProperty(OrderNoProperty); }
            set { this.SetProperty(OrderNoProperty, value); }
        }
        #endregion

        #region 明细行号 LineNo
        /// <summary>
        /// 明细行号
        /// </summary>
        [Label("明细行号")]
        public static readonly Property<string> LineNoProperty = P<ErpUploadLog>.Register(e => e.LineNo);

        /// <summary>
        /// 明细行号
        /// </summary>
        public string LineNo
        {
            get { return this.GetProperty(LineNoProperty); }
            set { this.SetProperty(LineNoProperty, value); }
        }
        #endregion

        #region 事务交易Id TransactionId
        /// <summary>
        /// 事务交易Id
        /// </summary>
        [Label("事务交易Id")]
        public static readonly Property<double?> TransactionIdProperty = P<ErpUploadLog>.Register(e => e.TransactionId);

        /// <summary>
        /// 事务交易Id
        /// </summary>
        public double? TransactionId
        {
            get { return this.GetProperty(TransactionIdProperty); }
            set { this.SetProperty(TransactionIdProperty, value); }
        }
        #endregion

        #region ERP主键 ErpDetailId
        /// <summary>
        /// ERP主键
        /// </summary>
        [Label("ERP主键")]
        public static readonly Property<string> ErpDetailIdProperty = P<ErpUploadLog>.Register(e => e.ErpDetailId);

        /// <summary>
        /// ERP主键
        /// </summary>
        public string ErpDetailId
        {
            get { return this.GetProperty(ErpDetailIdProperty); }
            set { this.SetProperty(ErpDetailIdProperty, value); }
        }
        #endregion

        #region ERP子库 ErpWhCode
        /// <summary>
        /// ERP子库
        /// </summary>
        [Label("ERP子库编码")]
        public static readonly Property<string> ErpWhCodeProperty = P<ErpUploadLog>.Register(e => e.ErpWhCode);

        /// <summary>
        /// ERP子库
        /// </summary>
        public string ErpWhCode
        {
            get { return this.GetProperty(ErpWhCodeProperty); }
            set { this.SetProperty(ErpWhCodeProperty, value); }
        }
        #endregion

        #region 单据类型 OrderType
        /// <summary>
        /// 单据类型
        /// </summary>
        [Label("单据类型")]
        public static readonly Property<OrderType> OrderTypeProperty = P<ErpUploadLog>.Register(e => e.OrderType);

        /// <summary>
        /// 单据类型
        /// </summary>
        public OrderType OrderType
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
        public static readonly Property<bool> IsSuccessProperty = P<ErpUploadLog>.Register(e => e.IsSuccess);

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess
        {
            get { return this.GetProperty(IsSuccessProperty); }
            set { this.SetProperty(IsSuccessProperty, value); }
        }
        #endregion

        #region 失败原因 ResponseMessage
        /// <summary>
        /// 失败原因
        /// </summary>
        [Label("失败原因")]
        public static readonly Property<string> ResponseMessageProperty = P<ErpUploadLog>.Register(e => e.ResponseMessage);

        /// <summary>
        /// 失败原因
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
        public static readonly Property<string> RequestStrProperty = P<ErpUploadLog>.Register(e => e.RequestStr);

        /// <summary>
        /// 请求报文
        /// </summary>
        public string RequestStr
        {
            get { return this.GetProperty(RequestStrProperty); }
            set { this.SetProperty(RequestStrProperty, value); }
        }
        #endregion

        #region 返回报文 ResponseStr
        /// <summary>
        /// 返回报文
        /// </summary>
        [Label("返回报文")]
        public static readonly Property<string> ResponseStrProperty = P<ErpUploadLog>.Register(e => e.ResponseStr);

        /// <summary>
        /// 返回报文
        /// </summary>
        public string ResponseStr
        {
            get { return this.GetProperty(ResponseStrProperty); }
            set { this.SetProperty(ResponseStrProperty, value); }
        }
        #endregion

        #region 接口名称 InterfaceName
        /// <summary>
        /// 接口名称
        /// </summary>
        [Label("接口名称")]
        public static readonly Property<string> InterfaceNameProperty = P<ErpUploadLog>.Register(e => e.InterfaceName);

        /// <summary>
        /// 接口名称
        /// </summary>
        public string InterfaceName
        {
            get { return this.GetProperty(InterfaceNameProperty); }
            set { this.SetProperty(InterfaceNameProperty, value); }
        }
        #endregion

        #region 重传次数 ReloadCount
        /// <summary>
        /// 重传次数
        /// </summary>
        [Label("重传次数")]
        public static readonly Property<int> ReloadCountProperty = P<ErpUploadLog>.Register(e => e.ReloadCount);

        /// <summary>
        /// 重传次数
        /// </summary>
        public int ReloadCount
        {
            get { return this.GetProperty(ReloadCountProperty); }
            set { this.SetProperty(ReloadCountProperty, value); }
        }
        #endregion

        #region 接口编码 InterfaceCode
        /// <summary>
        /// 接口编码
        /// </summary>
        [Label("接口编码")]
        public static readonly Property<string> InterfaceCodeProperty = P<ErpUploadLog>.Register(e => e.InterfaceCode);

        /// <summary>
        /// 接口编码
        /// </summary>
        public string InterfaceCode
        {
            get { return this.GetProperty(InterfaceCodeProperty); }
            set { this.SetProperty(InterfaceCodeProperty, value); }
        }
        #endregion

        #region 任务类型 JobType
        /// <summary>
        /// 任务类型
        /// </summary>
        [Label("任务类型")]
        public static readonly Property<JobType?> JobTypeProperty = P<ErpUploadLog>.Register(e => e.JobType);

        /// <summary>
        /// 任务类型
        /// </summary>
        public JobType? JobType
        {
            get { return GetProperty(JobTypeProperty); }
            set { SetProperty(JobTypeProperty, value); }
        }
        #endregion

        #region 日志状态 ProcessState
        /// <summary>
        /// 处理状态
        /// </summary>
        [Label("日志状态")]
        public static readonly Property<ProcessState> StateProperty = P<ErpUploadLog>.Register(e => e.State);

        /// <summary>
        /// 处理状态
        /// </summary>
        public ProcessState State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 凭证信息 SapKeyMsg
        /// <summary>
        /// 凭证信息
        /// </summary>
        [Label("凭证信息")]
        public static readonly Property<string> SapKeyMsgProperty = P<ErpUploadLog>.Register(e => e.SapKeyMsg);

        /// <summary>
        /// 凭证信息
        /// </summary>
        public string SapKeyMsg
        {
            get { return this.GetProperty(SapKeyMsgProperty); }
            set { this.SetProperty(SapKeyMsgProperty, value); }
        }
        #endregion

        #region 事务上传 UploadTransaction
        /// <summary>
        /// 事务上传Id
        /// </summary>
        [Label("事务上传")]
        public static readonly IRefIdProperty UploadTransactionIdProperty =
            P<ErpUploadLog>.RegisterRefId(e => e.UploadTransactionId, ReferenceType.Normal);

        /// <summary>
        /// 事务上传Id
        /// </summary>
        public double UploadTransactionId
        {
            get { return (double)this.GetRefId(UploadTransactionIdProperty); }
            set { this.SetRefId(UploadTransactionIdProperty, value); }
        }

        /// <summary>
        /// 事务上传
        /// </summary>
        public static readonly RefEntityProperty<UploadTransaction> UploadTransactionProperty =
            P<ErpUploadLog>.RegisterRef(e => e.UploadTransaction, UploadTransactionIdProperty);

        /// <summary>
        /// 事务上传
        /// </summary>
        public UploadTransaction UploadTransaction
        {
            get { return this.GetRefEntity(UploadTransactionProperty); }
            set { this.SetRefEntity(UploadTransactionProperty, value); }
        }
        #endregion

        #region 视图
        #region 事务上传状态 UploadTransactionState
        /// <summary>
        /// 事务上传状态
        /// </summary>
        [Label("事务上传状态")]
        public static readonly Property<ProcessState> UploadTransactionStateProperty = P<ErpUploadLog>.RegisterView(e => e.UploadTransactionState, p => p.UploadTransaction.State);

        /// <summary>
        /// 事务上传状态
        /// </summary>
        public ProcessState UploadTransactionState
        {
            get { return this.GetProperty(UploadTransactionStateProperty); }
        }
        #endregion

        #region 创建人 CreateName
        /// <summary>
        /// 创建人
        /// </summary>
        [Label("创建人")]
        public static readonly Property<string> CreateNameProperty = P<ErpUploadLog>.Register(e => e.CreateName);

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateName
        {
            get { return this.GetProperty(CreateNameProperty); }
            set { this.SetProperty(CreateNameProperty, value); }
        }
        #endregion

        #region 交易类型 TransactionType
        /// <summary>
        /// 交易类型
        /// </summary>
        [Label("交易类型")]
        public static readonly Property<string> TransactionTypeProperty = P<ErpUploadLog>.RegisterView(e => e.TransactionType, p => p.UploadTransaction.TransactionType);

        /// <summary>
        /// 交易类型
        /// </summary>
        public string TransactionType
        {
            get { return this.GetProperty(TransactionTypeProperty); }
        }
        #endregion


        #endregion

    }

    /// <summary>
    /// 事务上传 实体配置
    /// </summary>
    internal class ErpUploadLogConfig : EntityConfig<ErpUploadLog>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(ErpUploadLog.ResponseMessageProperty, new StringLengthRangeRule() { Max = 4000 });
        }

        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ERP_UPLOAD_LOG").MapAllProperties();            
            Meta.Property(ErpUploadLog.ResponseMessageProperty).ColumnMeta.HasLength(4000);
            Meta.Property(ErpUploadLog.RequestStrProperty).ColumnMeta.HasLength("MAX");
            Meta.Property(ErpUploadLog.ResponseStrProperty).ColumnMeta.HasLength("MAX");
            Meta.EnablePhantoms();
        }
    }
}
