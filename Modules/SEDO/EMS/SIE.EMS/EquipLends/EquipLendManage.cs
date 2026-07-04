using SIE.Common.Configs;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.EMS.EquipLends.Configs;
using SIE.EMS.EquipLends.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.EquipLends
{
    /// <summary>
    /// 设备借还管理
    /// </summary>
    [EntityWithConfig(typeof(EquipLendManageConfig))]
    [ConditionQueryType(typeof(EquipLendManageCriteria))]
    [RootEntity, Serializable]
    [Label("设备借还管理")]
    public class EquipLendManage : DataEntity
    {
        #region 借还单号 No
        /// <summary>
        /// 借还单号
        /// </summary>
        [Label("借还单号")]
        public static readonly Property<string> NoProperty = P<EquipLendManage>.Register(e => e.No);

        /// <summary>
        /// 借还单号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 设备 EquipAccount
        /// <summary>
        /// 设备Id
        /// </summary>
        [Label("设备")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<EquipLendManage>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备Id
        /// </summary>
        public double EquipAccountId
        {
            get { return (double)this.GetRefId(EquipAccountIdProperty); }
            set { this.SetRefId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备
        /// </summary>
        public static readonly RefEntityProperty<EquipAccount> EquipAccountProperty =
            P<EquipLendManage>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备
        /// </summary>
        public EquipAccount EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 状态 LendState
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<LendState> LendStateProperty = P<EquipLendManage>.Register(e => e.LendState);

        /// <summary>
        /// 状态
        /// </summary>
        public LendState LendState
        {
            get { return this.GetProperty(LendStateProperty); }
            set { this.SetProperty(LendStateProperty, value); }
        }
        #endregion

        #region 借机对象 LendObject
        /// <summary>
        /// 借机对象
        /// </summary>
        [Label("借机对象")]
        public static readonly Property<LendObject> LendObjectProperty = P<EquipLendManage>.Register(e => e.LendObject);

        /// <summary>
        /// 借机对象
        /// </summary>
        public LendObject LendObject
        {
            get { return this.GetProperty(LendObjectProperty); }
            set { this.SetProperty(LendObjectProperty, value); }
        }
        #endregion

        #region 借机部门 LendEnterprise
        /// <summary>
        /// 借机部门Id
        /// </summary>
        [Label("借机部门")]
        public static readonly IRefIdProperty LendEnterpriseIdProperty =
            P<EquipLendManage>.RegisterRefId(e => e.LendEnterpriseId, ReferenceType.Normal);

        /// <summary>
        /// 借机部门Id
        /// </summary>
        public double? LendEnterpriseId
        {
            get { return (double?)this.GetRefNullableId(LendEnterpriseIdProperty); }
            set { this.SetRefNullableId(LendEnterpriseIdProperty, value); }
        }

        /// <summary>
        /// 借机部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> LendEnterpriseProperty =
            P<EquipLendManage>.RegisterRef(e => e.LendEnterprise, LendEnterpriseIdProperty);

        /// <summary>
        /// 借机部门
        /// </summary>
        public Enterprise LendEnterprise
        {
            get { return this.GetRefEntity(LendEnterpriseProperty); }
            set { this.SetRefEntity(LendEnterpriseProperty, value); }
        }
        #endregion

        #region 借机人 LendEmployee
        /// <summary>
        /// 借机人Id
        /// </summary>
        [Label("借机人")]
        public static readonly IRefIdProperty LendEmployeeIdProperty =
            P<EquipLendManage>.RegisterRefId(e => e.LendEmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 借机人Id
        /// </summary>
        public double? LendEmployeeId
        {
            get { return (double?)this.GetRefNullableId(LendEmployeeIdProperty); }
            set { this.SetRefNullableId(LendEmployeeIdProperty, value); }
        }

        /// <summary>
        /// 借机人
        /// </summary>
        public static readonly RefEntityProperty<Employee> LendEmployeeProperty =
            P<EquipLendManage>.RegisterRef(e => e.LendEmployee, LendEmployeeIdProperty);

        /// <summary>
        /// 借机人
        /// </summary>
        public Employee LendEmployee
        {
            get { return this.GetRefEntity(LendEmployeeProperty); }
            set { this.SetRefEntity(LendEmployeeProperty, value); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        [Label("供应商")]
        public static readonly IRefIdProperty SupplierIdProperty =
            P<EquipLendManage>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

        /// <summary>
        /// 供应商Id
        /// </summary>
        public double? SupplierId
        {
            get { return (double?)this.GetRefNullableId(SupplierIdProperty); }
            set { this.SetRefNullableId(SupplierIdProperty, value); }
        }

        /// <summary>
        /// 供应商
        /// </summary>
        public static readonly RefEntityProperty<Supplier> SupplierProperty =
            P<EquipLendManage>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return this.GetRefEntity(SupplierProperty); }
            set { this.SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(2000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<EquipLendManage>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 借出原因 Reason
        /// <summary>
        /// 借出原因
        /// </summary>
        [MaxLength(2000)]
        [Label("借出原因")]
        public static readonly Property<string> ReasonProperty = P<EquipLendManage>.Register(e => e.Reason);

        /// <summary>
        /// 借出原因
        /// </summary>
        public string Reason
        {
            get { return this.GetProperty(ReasonProperty); }
            set { this.SetProperty(ReasonProperty, value); }
        }
        #endregion

        #region 归还说明 ReturnRemark
        /// <summary>
        /// 归还说明
        /// </summary>
        [MaxLength(2000)]
        [Label("归还说明")]
        public static readonly Property<string> ReturnRemarkProperty = P<EquipLendManage>.Register(e => e.ReturnRemark);

        /// <summary>
        /// 归还说明
        /// </summary>
        public string ReturnRemark
        {
            get { return this.GetProperty(ReturnRemarkProperty); }
            set { this.SetProperty(ReturnRemarkProperty, value); }
        }
        #endregion

        #region 审核记录 ExamineRecordList
        /// <summary>
        /// 审核记录
        /// </summary>
        [Label("审核记录")]
        public static readonly ListProperty<EntityList<EquipLendExamineRecord>> ExamineRecordListProperty = P<EquipLendManage>.RegisterList(e => e.ExamineRecordList);

        /// <summary>
        /// 审核记录
        /// </summary>
        public EntityList<EquipLendExamineRecord> ExamineRecordList
        {
            get { return this.GetLazyList(ExamineRecordListProperty); }
        }
        #endregion

        #region 附件 AttachmentList
        /// <summary>
        /// 附件
        /// </summary>
        [Label("附件")]
        public static readonly ListProperty<EntityList<EquipLendAttachment>> AttachmentListProperty = P<EquipLendManage>.RegisterList(e => e.AttachmentList);

        /// <summary>
        /// 附件
        /// </summary>
        public EntityList<EquipLendAttachment> AttachmentList
        {
            get { return this.GetLazyList(AttachmentListProperty); }
        }
        #endregion

        #region 视图属性
        #region 设备编码 EquipAccountCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipAccountCodeProperty = P<EquipLendManage>.RegisterView(e => e.EquipAccountCode, p => p.EquipAccount.Code);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipAccountCode
        {
            get { return this.GetProperty(EquipAccountCodeProperty); }
        }
        #endregion

        #region 设备名称 EquipAccountName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipAccountNameProperty = P<EquipLendManage>.RegisterView(e => e.EquipAccountName, p => p.EquipAccount.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipAccountName
        {
            get { return this.GetProperty(EquipAccountNameProperty); }
        }
        #endregion

        #region 部门名称 EnterpriseName
        /// <summary>
        /// 部门名称
        /// </summary>
        [Label("部门名称")]
        public static readonly Property<string> EnterpriseNameProperty = P<EquipLendManage>.RegisterView(e => e.EnterpriseName, p => p.LendEnterprise.Name);

        /// <summary>
        /// 部门名称
        /// </summary>
        public string EnterpriseName
        {
            get { return this.GetProperty(EnterpriseNameProperty); }
        }
        #endregion

        #region 借机人名称 EmployeeName
        /// <summary>
        /// 借机人名称
        /// </summary>
        [Label("借机人名称")]
        public static readonly Property<string> EmployeeNameProperty = P<EquipLendManage>.RegisterView(e => e.EmployeeName, p => p.LendEmployee.Name);

        /// <summary>
        /// 借机人名称
        /// </summary>
        public string EmployeeName
        {
            get { return this.GetProperty(EmployeeNameProperty); }
        }
        #endregion

        #region 供应商编码 SupplierCode
        /// <summary>
        /// 供应商编码
        /// </summary>
        [Label("供应商编码")]
        public static readonly Property<string> SupplierCodeProperty = P<EquipLendManage>.RegisterView(e => e.SupplierCode, p => p.Supplier.Code);

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode
        {
            get { return this.GetProperty(SupplierCodeProperty); }
        }
        #endregion

        #region 供应商名称 SupplierName
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Label("供应商名称")]
        public static readonly Property<string> SupplierNameProperty = P<EquipLendManage>.RegisterView(e => e.SupplierName, p => p.Supplier.Name);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get { return this.GetProperty(SupplierNameProperty); }
        }
        #endregion

        #region 资产编码 FixedAssetCode
        /// <summary>
        /// 资产编码
        /// </summary>
        [Label("资产编码")]
        public static readonly Property<string> FixedAssetCodeProperty = P<EquipLendManage>.RegisterView(e => e.FixedAssetCode, p => p.EquipAccount.FixedAssetsAccount.Code);

        /// <summary>
        /// 资产编码
        /// </summary>
        public string FixedAssetCode
        {
            get { return this.GetProperty(FixedAssetCodeProperty); }
        }
        #endregion

        #region RFID RFID
        /// <summary>
        /// RFID
        /// </summary>
        [Label("RFID")]
        public static readonly Property<string> RFIDProperty = P<EquipLendManage>.RegisterView(e => e.RFID, p => p.EquipAccount.RFID);

        /// <summary>
        /// RFID
        /// </summary>
        public string RFID
        {
            get { return this.GetProperty(RFIDProperty); }
        }
        #endregion

        #region 型号编码 ModelCode
        /// <summary>
        /// 型号编码
        /// </summary>
        [Label("型号编码")]
        public static readonly Property<string> ModelCodeProperty = P<EquipLendManage>.RegisterView(e => e.ModelCode, p => p.EquipAccount.EquipModel.Code);

        /// <summary>
        /// 型号编码
        /// </summary>
        public string ModelCode
        {
            get { return this.GetProperty(ModelCodeProperty); }
        }
        #endregion

        #region 型号名称 ModelName
        /// <summary>
        /// 型号名称
        /// </summary>
        [Label("型号名称")]
        public static readonly Property<string> ModelNameProperty = P<EquipLendManage>.RegisterView(e => e.ModelName, p => p.EquipAccount.EquipModel.Name);

        /// <summary>
        /// 型号名称
        /// </summary>
        public string ModelName
        {
            get { return this.GetProperty(ModelNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 实体元配置
    /// </summary>
    public class EquipLendManageEntityConfig : EntityConfig<EquipLendManage>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_LEND").MapAllProperties();
            Meta.Property(EquipLendManage.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.Property(EquipLendManage.ReasonProperty).ColumnMeta.HasLength(4000);
            Meta.Property(EquipLendManage.ReturnRemarkProperty).ColumnMeta.HasLength(4000);
        }
    }
}
