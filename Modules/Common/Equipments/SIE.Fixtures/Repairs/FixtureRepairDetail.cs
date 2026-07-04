using SIE.Common;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Fixtures.Enums;
using SIE.Fixtures.Fixtures.Abnormals;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.Fixtures.Repairs
{
    /// <summary>
	/// 工治具异常详情
	/// </summary>
	[ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("工治具异常详情")]
    public partial class FixtureRepairDetail : DataEntity
    {
        #region 维修内容 Content
        /// <summary>
        /// 维修内容
        /// </summary>
        [Label("维修内容")]
        public static readonly Property<string> ContentProperty = P<FixtureRepairDetail>.Register(e => e.Content);

        /// <summary>
        /// 维修内容
        /// </summary>
        public string Content
        {
            get { return GetProperty(ContentProperty); }
            set { SetProperty(ContentProperty, value); }
        }
        #endregion

        #region 维修部位 Part
        /// <summary>
        /// 维修部位
        /// </summary>
        [Label("维修部位")]
        public static readonly Property<string> PartProperty = P<FixtureRepairDetail>.Register(e => e.Part);

        /// <summary>
        /// 维修部位
        /// </summary>
        public string Part
        {
            get { return GetProperty(PartProperty); }
            set { SetProperty(PartProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<FixtureRepairDetail>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 报修前状态 RepairBeforeState
        /// <summary>
        /// 报修前状态
        /// </summary>
        [Label("报修前状态")]
        public static readonly Property<RepairBeforeState> RepairBeforeStateProperty = P<FixtureRepairDetail>.Register(e => e.RepairBeforeState);

        /// <summary>
        /// 报修前状态
        /// </summary>
        public RepairBeforeState RepairBeforeState
        {
            get { return GetProperty(RepairBeforeStateProperty); }
            set { SetProperty(RepairBeforeStateProperty, value); }
        }
        #endregion
        
        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<int> QtyProperty = P<FixtureRepairDetail>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public int Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 备件详情 SpareList
        /// <summary>
        /// 备件详情
        /// </summary>
        public static readonly ListProperty<EntityList<FixtureRepairRecord>> SpareListProperty = P<FixtureRepairDetail>.RegisterList(e => e.SpareList);
        /// <summary>
        /// 备件详情
        /// </summary>
        public EntityList<FixtureRepairRecord> SpareList
        {
            get { return this.GetLazyList(SpareListProperty); }
        }
        #endregion

        #region 项目保养结论 InspectionResult
        /// <summary>
        /// 项目保养结论
        /// </summary>
        [Label("项目保养结论")]
        public static readonly Property<InspectionResult?> InspectionResultProperty = P<FixtureRepairDetail>.Register(e => e.InspectionResult);

        /// <summary>
        /// 项目保养结论
        /// </summary>
        public InspectionResult? InspectionResult
        {
            get { return GetProperty(InspectionResultProperty); }
            set { SetProperty(InspectionResultProperty, value); }
        }
        #endregion

        #region 数据库保养结论 Result
        /// <summary>
        /// 数据库保养结论
        /// </summary>
        [Label("数据库保养结论")]
        public static readonly Property<InspectionResult?> ResultProperty = P<FixtureRepairDetail>.Register(e => e.Result);

        /// <summary>
        /// 数据库保养结论
        /// </summary>
        public InspectionResult? Result
        {
            get { return GetProperty(ResultProperty); }
            set { SetProperty(ResultProperty, value); }
        }
        #endregion

        #region 工治具台帐 FixtureAccount
        /// <summary>
        /// 工治具台帐Id
        /// </summary>
        public static readonly IRefIdProperty FixtureAccountIdProperty = P<FixtureRepairDetail>.RegisterRefId(e => e.FixtureAccountId, ReferenceType.Normal);

        /// <summary>
        /// 工治具台帐Id
        /// </summary>
        public double FixtureAccountId
        {
            get { return (double)GetRefId(FixtureAccountIdProperty); }
            set { SetRefId(FixtureAccountIdProperty, value); }
        }

        /// <summary>
        /// 工治具台帐
        /// </summary>
        public static readonly RefEntityProperty<FixtureAccount> FixtureAccountProperty = P<FixtureRepairDetail>.RegisterRef(e => e.FixtureAccount, FixtureAccountIdProperty);

        /// <summary>
        /// 工治具治具台账
        /// </summary>
        public FixtureAccount FixtureAccount
        {
            get { return GetRefEntity(FixtureAccountProperty); }
            set { SetRefEntity(FixtureAccountProperty, value); }
        }
        #endregion

        #region 工治具仓库 Warehouse
        /// <summary>
        /// 工治具仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty FixtureWarehouseIdProperty = P<FixtureRepairDetail>.RegisterRefId(e => e.FixtureWarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 工治具仓库Id
        /// </summary>
        public double? FixtureWarehouseId
        {
            get { return (double?)GetRefNullableId(FixtureWarehouseIdProperty); }
            set { SetRefNullableId(FixtureWarehouseIdProperty, value); }
        }

        /// <summary>
        /// 工治具仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> FixtureWarehouseProperty = P<FixtureRepairDetail>.RegisterRef(e => e.Warehouse, FixtureWarehouseIdProperty);

        /// <summary>
        /// 工治具仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(FixtureWarehouseProperty); }
            set { SetRefEntity(FixtureWarehouseProperty, value); }
        }
        #endregion

        #region 工治具治具库位 StorageLocation(当报修前状态为在库时，需要先出库，再报修）
        /// <summary>
        /// 工治具治具库位Id
        /// </summary>
        [Label("库位")]
        public static readonly IRefIdProperty FixtureStorageLocationIdProperty = P<FixtureRepairDetail>.RegisterRefId(e => e.FixtureStorageLocationId, ReferenceType.Normal);

        /// <summary>
        /// 工治具治具库位Id
        /// </summary>
        public double? FixtureStorageLocationId
        {
            get { return (double?)GetRefNullableId(FixtureStorageLocationIdProperty); }
            set { SetRefNullableId(FixtureStorageLocationIdProperty, value); }
        }

        /// <summary>
        /// 工治具治具库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> FixtureStorageLocationProperty = P<FixtureRepairDetail>.RegisterRef(e => e.StorageLocation, FixtureStorageLocationIdProperty);

        /// <summary>
        /// 工治具治具库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return GetRefEntity(FixtureStorageLocationProperty); }
            set { SetRefEntity(FixtureStorageLocationProperty, value); }
        }
        #endregion

        #region 故障现象 Fault
        /// <summary>
        /// 故障现象Id
        /// </summary>
        [Label("故障现象")]
        public static readonly IRefIdProperty FaultIdProperty = P<FixtureRepairDetail>.RegisterRefId(e => e.FaultId, ReferenceType.Normal);

        /// <summary>
        /// 故障现象Id
        /// </summary>
        public double? FaultId
        {
            get { return (double?)GetRefNullableId(FaultIdProperty); }
            set { SetRefNullableId(FaultIdProperty, value); }
        }

        /// <summary>
        /// 故障现象
        /// </summary>
        public static readonly RefEntityProperty<FixtureAbnormal> FaultProperty = P<FixtureRepairDetail>.RegisterRef(e => e.Fault, FaultIdProperty);

        /// <summary>
        /// 故障现象
        /// </summary>
        public FixtureAbnormal Fault
        {
            get { return GetRefEntity(FaultProperty); }
            set { SetRefEntity(FaultProperty, value); }
        }
        #endregion

        #region 异常现象 Abnormal
        /// <summary>
        /// 异常现象Id
        /// </summary>
        [Label("异常现象")]
        public static readonly IRefIdProperty AbnormalIdProperty = P<FixtureRepairDetail>.RegisterRefId(e => e.AbnormalId, ReferenceType.Normal);

        /// <summary>
        /// 异常现象Id
        /// </summary>
        public double AbnormalId
        {
            get { return (double)GetRefId(AbnormalIdProperty); }
            set { SetRefId(AbnormalIdProperty, value); }
        }

        /// <summary>
        /// 异常现象
        /// </summary>
        public static readonly RefEntityProperty<FixtureAbnormal> AbnormalProperty = P<FixtureRepairDetail>.RegisterRef(e => e.Abnormal, AbnormalIdProperty);

        /// <summary>
        /// 异常现象
        /// </summary>
        public FixtureAbnormal Abnormal
        {
            get { return GetRefEntity(AbnormalProperty); }
            set { SetRefEntity(AbnormalProperty, value); }
        }
        #endregion

        #region 图片 Attachments
        /// <summary>
        /// 图片
        /// </summary>
        public static readonly ListProperty<EntityList<FixtureRepairAttachment>> AttachmentsProperty = P<FixtureRepairDetail>.RegisterList(e => e.Attachments);
        /// <summary>
        /// 图片
        /// </summary>
        public EntityList<FixtureRepairAttachment> Attachments
        {
            get { return this.GetLazyList(AttachmentsProperty); }
        }
        #endregion

        #region 工治具报修 FixtureRepair
        /// <summary>
        /// 工治具报修Id
        /// </summary>
        [Label("工治具报修")]
        public static readonly IRefIdProperty FixtureRepairIdProperty = P<FixtureRepairDetail>.RegisterRefId(e => e.FixtureRepairId, ReferenceType.Parent);

        /// <summary>
        /// 工治具报修Id
        /// </summary>
        public double FixtureRepairId
        {
            get { return (double)GetRefId(FixtureRepairIdProperty); }
            set { SetRefId(FixtureRepairIdProperty, value); }
        }

        /// <summary>
        /// 工治具报修
        /// </summary>
        public static readonly RefEntityProperty<FixtureRepair> FixtureRepairProperty = P<FixtureRepairDetail>.RegisterRef(e => e.FixtureRepair, FixtureRepairIdProperty);

        /// <summary>
        /// 工治具报修
        /// </summary>
        public FixtureRepair FixtureRepair
        {
            get { return GetRefEntity(FixtureRepairProperty); }
            set { SetRefEntity(FixtureRepairProperty, value); }
        }
        #endregion

        #region 是否已维修 IsRepair
        /// <summary>
        /// 是否已维修
        /// </summary>
        public static readonly Property<bool> IsRepairProperty = P<FixtureRepairDetail>.Register(e => e.IsRepair);

        /// <summary>
        /// 是否已维修
        /// </summary>
        public bool IsRepair
        {
            get { return GetProperty(IsRepairProperty); }
            set { SetProperty(IsRepairProperty, value); }
        }
        #endregion

        #region 报修前质量状态 RepairBeforeState
        /// <summary>
        /// 报修前质量状态
        /// </summary>
        [Label("报修前质量状态")]
        public static readonly Property<FixtureQualityState?> RepairBeforeQualityStatusProperty = P<FixtureRepairDetail>.Register(e => e.RepairBeforeQualityStatus);

        /// <summary>
        /// 报修前质量状态
        /// </summary>
        public FixtureQualityState? RepairBeforeQualityStatus
        {
            get { return GetProperty(RepairBeforeQualityStatusProperty); }
            set { SetProperty(RepairBeforeQualityStatusProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty = P<FixtureRepairDetail>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return (double?)GetRefNullableId(WorkOrderIdProperty); }
            set { SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<FixtureRepairDetail>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 入库仓库 InWarehouse
        /// <summary>
        /// 入库仓库Id
        /// </summary>
        [Label("入库仓库")]
        public static readonly IRefIdProperty InWarehouseIdProperty = P<FixtureRepairDetail>.RegisterRefId(e => e.InWarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 入库仓库Id
        /// </summary>
        public double? InWarehouseId
        {
            get { return (double?)GetRefNullableId(InWarehouseIdProperty); }
            set { SetRefNullableId(InWarehouseIdProperty, value); }
        }

        /// <summary>
        /// 入库仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> InWarehouseProperty = P<FixtureRepairDetail>.RegisterRef(e => e.InWarehouse, InWarehouseIdProperty);

        /// <summary>
        /// 入库仓库
        /// </summary>
        public Warehouse InWarehouse
        {
            get { return GetRefEntity(InWarehouseProperty); }
            set { SetRefEntity(InWarehouseProperty, value); }
        }
        #endregion

        #region 维修后去向 RepairWhereaboutStatus
        /// <summary>
        /// 维修后去向
        /// </summary>
        [Label("维修后去向")]
        public static readonly Property<RepairWhereabout?> RepairWhereaboutProperty = P<FixtureRepairDetail>.Register(e => e.RepairWhereaboutStatus);

        /// <summary>
        /// 维修后去向
        /// </summary>
        public RepairWhereabout? RepairWhereaboutStatus
        {
            get { return GetProperty(RepairWhereaboutProperty); }
            set { SetProperty(RepairWhereaboutProperty, value); }
        }
        #endregion


        #region 注册视图
        #region 维修单号
        /// <summary>
        /// 维修单号
        /// </summary>
        [Label("维修单号")]
        public static readonly Property<string> RepairNoProperty = P<FixtureRepairDetail>.RegisterView(e => e.RepairNo, p => p.FixtureRepair.No);

        /// <summary>
        /// 维修单号
        /// </summary>
        public string RepairNo
        {
            get { return this.GetProperty(RepairNoProperty); }
        }
        #endregion

        #region 报修人
        /// <summary>
        /// 报修人
        /// </summary>
        [Label("报修人")]
        public static readonly Property<string> RepairApplyByProperty = P<FixtureRepairDetail>.RegisterView(e => e.RepairApplyBy, p => p.FixtureRepair.ApplyBy.Name);

        /// <summary>
        /// 报修人
        /// </summary>
        public string RepairApplyBy
        {
            get { return this.GetProperty(RepairApplyByProperty); }
        }
        #endregion

        #region 报修时间
        /// <summary>
        /// 报修时间
        /// </summary>
        [Label("报修时间")]
        public static readonly Property<DateTime> RepairApplyDateProperty = P<FixtureRepairDetail>.RegisterView(e => e.RepairApplyDate, p => p.FixtureRepair.ApplyDate);

        /// <summary>
        /// 报修时间
        /// </summary>
        public DateTime RepairApplyDate
        {
            get { return this.GetProperty(RepairApplyDateProperty); }
        }
        #endregion

        #region 维修人
        /// <summary>
        /// 维修人
        /// </summary>
        [Label("维修人")]
        public static readonly Property<string> RepairByNameProperty = P<FixtureRepairDetail>.RegisterView(e => e.RepairByName, p => p.FixtureRepair.RepairBy.Name);

        /// <summary>
        /// 维修人
        /// </summary>
        public string RepairByName
        {
            get { return this.GetProperty(RepairByNameProperty); }
        }
        #endregion

        #region 维修时间
        /// <summary>
        /// 维修时间
        /// </summary>
        [Label("维修时间")]
        public static readonly Property<DateTime?> RepairDateProperty = P<FixtureRepairDetail>.RegisterView(e => e.RepairDate, p => p.FixtureRepair.RepairDate);

        /// <summary>
        /// 维修时间
        /// </summary>
        public DateTime? RepairDate
        {
            get { return this.GetProperty(RepairDateProperty); }
        }
        #endregion

        #region 工治具ID
        /// <summary>
        /// 工治具ID
        /// </summary>
        [Label("工治具ID")]
        public static readonly Property<string> FixtureAccountCodeProperty = P<FixtureRepairDetail>.RegisterView(e => e.FixtureAccountCode, p => p.FixtureAccount.Code);

        /// <summary>
        /// 工治具ID
        /// </summary>
        public string FixtureAccountCode
        {
            get { return this.GetProperty(FixtureAccountCodeProperty); }
        }

        #endregion

        #region 工治具编码
        /// <summary>
        /// 工治具编码
        /// </summary>
        [Label("工治具编码")]
        public static readonly Property<string> FixtureEncodeCodeCodeProperty = P<FixtureRepairDetail>.RegisterView(e => e.FixtureEncodeCode, p => p.FixtureAccount.FixtureEncode.Code);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public string FixtureEncodeCode
        {
            get { return this.GetProperty(FixtureEncodeCodeCodeProperty); }
        }

        #endregion

        #region 型号编码 FixtureModelCode
        /// <summary>
        /// 型号编码
        /// </summary>
        [Label("型号编码")]
        public static readonly Property<string> FixtureModelCodeProperty = P<FixtureRepairDetail>.RegisterView(e => e.FixtureModelCode, p => p.FixtureAccount.FixtureEncode.FixtureModel.Code);

        /// <summary>
        /// 型号编码
        /// </summary>
        public string FixtureModelCode
        {
            get { return this.GetProperty(FixtureModelCodeProperty); }
        }

        #endregion

        #region 型号名称 FixtureModelName
        /// <summary>
        /// 型号名称
        /// </summary>
        [Label("型号名称")]
        public static readonly Property<string> FixtureModelNameProperty = P<FixtureRepairDetail>.RegisterView(e => e.FixtureModelName, p => p.FixtureAccount.FixtureEncode.FixtureModel.Name);

        /// <summary>
        /// 型号名称
        /// </summary>
        public string FixtureModelName
        {
            get { return this.GetProperty(FixtureModelNameProperty); }
        }

        #endregion

        #region 工治具类型 FixtureModelType
        /// <summary>
        /// 工治具类型
        /// </summary>
        [Label("工治具类型")]
        public static readonly Property<string> FixtureModelTypeProperty = P<FixtureRepairDetail>.RegisterView(e => e.FixtureModelType, p => p.FixtureAccount.FixtureEncode.FixtureModel.FixtureType.Code);

        /// <summary>
        /// 工治具类型
        /// </summary>
        public string FixtureModelType
        {
            get { return this.GetProperty(FixtureModelTypeProperty); }
        }

        #endregion

        #region 管理方式 ManageMode
        /// <summary>
        /// 管理方式
        /// </summary>
        [Label("管理方式")]
        public static readonly Property<ManageMode> ManageModeProperty = P<FixtureRepairDetail>.RegisterView(e => e.ManageMode, p => p.FixtureAccount.FixtureEncode.FixtureModel.ManageMode);

        /// <summary>
        /// 管理方式
        /// </summary>
        public ManageMode ManageMode
        {
            get { return this.GetProperty(ManageModeProperty); }
        }

        #endregion

        #region 异常现象编码 AbnormalCode
        /// <summary>
        /// 异常现象编码
        /// </summary>
        [Label("异常现象编码")]
        public static readonly Property<string> AbnormalCodeProperty = P<FixtureRepairDetail>.RegisterView(e => e.AbnormalCode, p => p.Abnormal.Code);

        /// <summary>
        /// 异常现象编码
        /// </summary>
        public string AbnormalCode
        {
            get { return this.GetProperty(AbnormalCodeProperty); }
        }

        #endregion

        #region 异常现象描述 AbnormalDescription
        /// <summary>
        /// 异常现象描述
        /// </summary>
        [Label("异常现象描述")]
        public static readonly Property<string> AbnormalDescriptionProperty = P<FixtureRepairDetail>.RegisterView(e => e.AbnormalDescription, p => p.Abnormal.Description);

        /// <summary>
        /// 异常现象描述
        /// </summary>
        public string AbnormalDescription
        {
            get { return this.GetProperty(AbnormalDescriptionProperty); }
        }

        #endregion

        #region 故障类型编码 FaultCode
        /// <summary>
        /// 故障类型编码
        /// </summary>
        [Label("故障类型编码")]
        public static readonly Property<string> FaultCodeProperty = P<FixtureRepairDetail>.RegisterView(e => e.FaultCode, p => p.Fault.Code);

        /// <summary>
        /// 故障类型编码
        /// </summary>
        public string FaultCode
        {
            get { return this.GetProperty(FaultCodeProperty); }
        }

        #endregion

        #region 故障类型描述 FaultDescriptionProperty
        /// <summary>
        /// 故障类型描述
        /// </summary>
        [Label("故障类型描述")]
        public static readonly Property<string> FaultDescriptionProperty = P<FixtureRepairDetail>.RegisterView(e => e.FaultDescription, p => p.Fault.Description);

        /// <summary>
        /// 故障类型描述
        /// </summary>
        public string FaultDescription
        {
            get { return this.GetProperty(FaultDescriptionProperty); }
        }

        #endregion

        #region 库位名称 LocationName
        /// <summary>
        /// 库位名称
        /// </summary>
        [Label("库位名称")]
        public static readonly Property<string> LocationNameProperty = P<FixtureRepairDetail>.RegisterView(e => e.LocationName, p => p.StorageLocation.Name);

        /// <summary>
        /// 库位名称
        /// </summary>
        public string LocationName
        {
            get { return this.GetProperty(LocationNameProperty); }
            set { SetProperty(LocationNameProperty, value); }
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// 治具异常详情 实体配置
    /// </summary>
    internal class FixtureRepairDetailConfig : EntityConfig<FixtureRepairDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ELEC_FIX_REPAIR_DTL").MapAllPropertiesExcept(FixtureRepairDetail.ResultProperty, FixtureRepairDetail.IsRepairProperty);
            Meta.EnablePhantoms();
        }
    }

    /// <summary>
    /// ID类工治具台帐维修履历扩展属性
    /// </summary>
    [CompiledPropertyDeclarer]
    public static class IDAccountRepairProperty
    {
        /// <summary>
        /// ID类工治具台帐维修履历属性
        /// </summary>
        public static readonly ListProperty<EntityList<FixtureRepairDetail>> RepairDetailListProperty =
            P<FixtureIDAccount>.RegisterExtensionList<EntityList<FixtureRepairDetail>>("RepairDetailList", typeof(IDAccountRepairProperty));

        /// <summary>
        /// 获取ID类工治具台帐维修履历对象
        /// </summary>
        /// <param name="me">ID类工治具台帐对象</param>
        /// <returns>返回ID类工治具台帐维修履历对象</returns>
        public static EntityList<FixtureRepairDetail> GetRepairDetailList(FixtureIDAccount me)
        {
            return me.GetProperty(RepairDetailListProperty);
        }

        /// <summary>
        /// 设置ID类工治具台帐维修履历对象
        /// </summary>
        /// <param name="me">ID类工治具台帐对象</param>
        /// <param name="value">需要设置的ID类工治具台帐保养履历对象</param>
        public static void SetFixtureRepairDetailList(FixtureIDAccount me, EntityList<FixtureRepairDetail> value)
        {
            me.SetProperty(RepairDetailListProperty, value);
        }
    }

    /// <summary>
    /// ID类工治具台帐维修履历 实体配置
    /// </summary>
    internal class IDAccountRepairPropertyConfig : EntityConfig<FixtureIDAccount>
    {
        /// <summary>
        /// 属性元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.Property(IDAccountRepairProperty.RepairDetailListProperty).DontMapColumn();
        }
    }

    /// <summary>
    /// 编码类工治具台帐维修履历扩展属性
    /// </summary>
    [CompiledPropertyDeclarer]
    public static class CodeAccountRepairProperty
    {
        /// <summary>
        /// 编码类工治具台帐维修履历属性
        /// </summary>
        public static readonly ListProperty<EntityList<FixtureRepairDetail>> RepairDetailListProperty =
            P<FixtureCodeAccount>.RegisterExtensionList<EntityList<FixtureRepairDetail>>("RepairDetailList", typeof(CodeAccountRepairProperty));

        /// <summary>
        /// 获取编码类工治具台帐维修履历对象
        /// </summary>
        /// <param name="me">编码类工治具台帐对象</param>
        /// <returns>返回编码类工治具台帐维修履历对象</returns>
        public static EntityList<FixtureRepairDetail> GetRepairDetailList(FixtureCodeAccount me)
        {
            return me.GetProperty(RepairDetailListProperty);
        }

        /// <summary>
        /// 设置工治具台帐维修履历对象
        /// </summary>
        /// <param name="me">编码类工治具台帐对象</param>
        /// <param name="value">需要设置的编码类工治具台帐维修履历对象</param>
        public static void SetRepairDetailList(FixtureCodeAccount me, EntityList<FixtureRepairDetail> value)
        {
            me.SetProperty(RepairDetailListProperty, value);
        }
    }

    /// <summary>
    /// 编码类工治具台帐维修履历 实体配置
    /// </summary>
    internal class CodeAccountRepairPropertyConfig : EntityConfig<FixtureCodeAccount>
    {
        /// <summary>
        /// 属性元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.Property(CodeAccountRepairProperty.RepairDetailListProperty).DontMapColumn();
        }
    }
}
