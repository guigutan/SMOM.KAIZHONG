using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.Common.Logs
{
    /// <summary>
    /// 事务上传记录明细
    /// </summary>
    [RootEntity, Serializable]
    //[CriteriaQuery]
    [Label("事务上传记录明细")]
    public partial class UploadTransactionLogDtl : DataEntity
    {
        #region 事务上传记录 UploadTransactionLog
        /// <summary>
        /// 事务上传记录Id
        /// </summary>
        [Label("事务上传记录")]
        public static readonly IRefIdProperty UploadTransactionLogIdProperty =
            P<UploadTransactionLogDtl>.RegisterRefId(e => e.UploadTransactionLogId, ReferenceType.Parent);

        /// <summary>
        /// 事务上传记录Id
        /// </summary>
        public double UploadTransactionLogId
        {
            get { return (double)this.GetRefId(UploadTransactionLogIdProperty); }
            set { this.SetRefId(UploadTransactionLogIdProperty, value); }
        }

        /// <summary>
        /// 事务上传记录
        /// </summary>
        public static readonly RefEntityProperty<UploadTransactionLog> UploadTransactionLogProperty =
            P<UploadTransactionLogDtl>.RegisterRef(e => e.UploadTransactionLog, UploadTransactionLogIdProperty);

        /// <summary>
        /// 事务上传记录
        /// </summary>
        public UploadTransactionLog UploadTransactionLog
        {
            get { return this.GetRefEntity(UploadTransactionLogProperty); }
            set { this.SetRefEntity(UploadTransactionLogProperty, value); }
        }
        #endregion

        #region 事务上传ID UploadTransactionId
        /// <summary>
        /// 事务上传ID
        /// </summary>
        [Label("事务上传ID")]
        public static readonly Property<double> UploadTransactionIdProperty = P<UploadTransactionLogDtl>.Register(e => e.UploadTransactionId);

        /// <summary>
        /// 事务上传ID
        /// </summary>
        public double UploadTransactionId
        {
            get { return this.GetProperty(UploadTransactionIdProperty); }
            set { this.SetProperty(UploadTransactionIdProperty, value); }
        }
        #endregion

        #region 处理状态 ProcessState
        /// <summary>
        /// 处理状态
        /// </summary>
        [Label("处理状态")]
        public static readonly Property<ProcessState> ProcessStateProperty = P<UploadTransactionLogDtl>.Register(e => e.ProcessState);

        /// <summary>
        /// 处理状态
        /// </summary>
        public ProcessState ProcessState
        {
            get { return this.GetProperty(ProcessStateProperty); }
            set { this.SetProperty(ProcessStateProperty, value); }
        }
        #endregion

        #region 验证信息 ValidateMessage
        /// <summary>
        /// 验证信息
        /// </summary>
        [Label("验证信息")]
        public static readonly Property<string> ValidateMessageProperty = P<UploadTransactionLogDtl>.Register(e => e.ValidateMessage);

        /// <summary>
        /// 验证信息
        /// </summary>
        public string ValidateMessage
        {
            get { return this.GetProperty(ValidateMessageProperty); }
            set { this.SetProperty(ValidateMessageProperty, value); }
        }
        #endregion

        #region 处理信息 ProcessMessage
        /// <summary>
        /// 处理信息
        /// </summary>
        [Label("处理信息")]
        public static readonly Property<string> ProcessMessageProperty = P<UploadTransactionLogDtl>.Register(e => e.ProcessMessage);

        /// <summary>
        /// 处理信息
        /// </summary>
        public string ProcessMessage
        {
            get { return this.GetProperty(ProcessMessageProperty); }
            set { this.SetProperty(ProcessMessageProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 事务上传记录明细 实体配置
    /// </summary>
    internal class UploadTransactionLogDtlConfig : EntityConfig<UploadTransactionLogDtl>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(UploadTransactionLogDtl.ValidateMessageProperty, new StringLengthRangeRule() { Max = 4000 });
            rules.AddRule(UploadTransactionLogDtl.ProcessMessageProperty, new StringLengthRangeRule() { Max = 4000 });
        }

        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("UL_TRANS_LOG_DTL").MapAllProperties();
            Meta.Property(UploadTransactionLogDtl.ProcessMessageProperty).ColumnMeta.HasLength(4000);
            Meta.Property(UploadTransactionLogDtl.ValidateMessageProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}