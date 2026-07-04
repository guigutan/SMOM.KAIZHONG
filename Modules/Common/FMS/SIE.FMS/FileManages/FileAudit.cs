using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using System;

namespace SIE.FMS
{
    /// <summary>
    /// 文件流审核
    /// </summary>
    [ChildEntity, Serializable]
    [DisplayMember(nameof(State))]
    [Label("文件流审核")]
    public partial class FileAudit : DataEntity
    {
        #region 操作 Operation
        /// <summary>
        /// 操作
        /// </summary>
        [Label("操作")]
        public static readonly Property<OperationType> OperationProperty = P<FileAudit>.Register(e => e.Operation);

        /// <summary>
        /// 操作
        /// </summary>
        public OperationType Operation
        {
            get { return GetProperty(OperationProperty); }
            set { SetProperty(OperationProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<FileAudit>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 驳回原因 RejectReason
        /// <summary>
        /// 驳回原因
        /// </summary>
        [Label("驳回原因")]
        [MaxLength(500)]
        public static readonly Property<string> RejectReasonProperty = P<FileAudit>.Register(e => e.RejectReason);

        /// <summary>
        /// 驳回原因
        /// </summary>
        public string RejectReason
        {
            get { return GetProperty(RejectReasonProperty); }
            set { SetProperty(RejectReasonProperty, value); }
        }
        #endregion

        #region 审核人 Auditor
        /// <summary>
        /// 审核人Id
        /// </summary>
        [Label("审核人")]
        public static readonly IRefIdProperty AuditorIdProperty = P<FileAudit>.RegisterRefId(e => e.AuditorId, ReferenceType.Normal);

        /// <summary>
        /// 审核人Id
        /// </summary>
        public double AuditorId
        {
            get { return (double)GetRefId(AuditorIdProperty); }
            set { SetRefId(AuditorIdProperty, value); }
        }

        /// <summary>
        /// 审核人
        /// </summary>
        public static readonly RefEntityProperty<Employee> AuditorProperty = P<FileAudit>.RegisterRef(e => e.Auditor, AuditorIdProperty);

        /// <summary>
        /// 审核人
        /// </summary>
        public Employee Auditor
        {
            get { return GetRefEntity(AuditorProperty); }
            set { SetRefEntity(AuditorProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("文件流审核状态")]
        public static readonly Property<FileAuditState> StateProperty = P<FileAudit>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public FileAuditState State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 审核 File
        /// <summary>
        /// 审核Id
        /// </summary>
        [Label("文件")]
        public static readonly IRefIdProperty FileIdProperty = P<FileAudit>.RegisterRefId(e => e.FileId, ReferenceType.Parent);

        /// <summary>
        /// 审核Id
        /// </summary>
        public double FileId
        {
            get { return (double)GetRefId(FileIdProperty); }
            set { SetRefId(FileIdProperty, value); }
        }

        /// <summary>
        /// 审核
        /// </summary>
        public static readonly RefEntityProperty<FileManage> FileProperty = P<FileAudit>.RegisterRef(e => e.File, FileIdProperty);

        /// <summary>
        /// 审核
        /// </summary>
        public FileManage File
        {
            get { return GetRefEntity(FileProperty); }
            set { SetRefEntity(FileProperty, value); }
        }
        #endregion

        #region 是否有效 IsEnabled
        /// <summary>
        /// 是否有效
        /// </summary>
        [Label("是否有效")]
        public static readonly Property<bool> IsEnabledProperty = P<FileAudit>.Register(e => e.IsEnabled);

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsEnabled
        {
            get { return this.GetProperty(IsEnabledProperty); }
            set { this.SetProperty(IsEnabledProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 文件流审核 实体配置
    /// </summary>
    internal class FileAuditConfig : EntityConfig<FileAudit>
    {
        protected override void ConfigMeta()
        {

            Meta.MapTable("FMS_FILE_AUDIT").MapAllProperties();
            Meta.Property(FileAudit.RejectReasonProperty).ColumnMeta.HasLength(1600);
            Meta.EnablePhantoms();
        }
    }
}