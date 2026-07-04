using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Tech.Processs;
using System;

namespace SIE.EMS.Purchases.EquipmentSetups.ViewModels
{
    /// <summary>
    /// 选择设备台账 ViewModel
    /// </summary> 
    [Serializable, RootEntity]
    [ConditionQueryType(typeof(SelEquipDetailCriteriaViewModel))]
    [Label("选择设备台账")]
    public class SelEquipDetailViewModel : Entity<double>
    {
        #region 设备编码 Code
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> CodeProperty = P<SelEquipDetailViewModel>.Register(e => e.Code);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 设备名称 Name
        /// <summary>
        /// 设备名称
        /// </summary>        
        [Label("设备名称")]
        public static readonly Property<string> NameProperty = P<SelEquipDetailViewModel>.Register(e => e.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 设备别名 Alias
        /// <summary>
        /// 设备别名
        /// </summary>
        [Label("设备别名")]
        public static readonly Property<string> AliasProperty = P<SelEquipDetailViewModel>.Register(e => e.Alias);

        /// <summary>
        /// 设备别名
        /// </summary>
        public string Alias
        {
            get { return this.GetProperty(AliasProperty); }
            set { this.SetProperty(AliasProperty, value); }
        }
        #endregion

        #region 设备型号 EquipAccountModelCode
        /// <summary>
        /// 设备型号
        /// </summary>
        [Label("设备型号")]
        public static readonly Property<string> EquipAccountModelCodeProperty = P<SelEquipDetailViewModel>.Register(e => e.EquipAccountModelCode);

        /// <summary>
        /// 设备型号
        /// </summary>
        public string EquipAccountModelCode
        {
            get { return this.GetProperty(EquipAccountModelCodeProperty); }
            set { this.SetProperty(EquipAccountModelCodeProperty, value); }
        }
        #endregion

        #region 型号名称 EquipAccountModelName
        /// <summary>
        /// 型号名称
        /// </summary>
        [Label("型号名称")]
        public static readonly Property<string> EquipAccountModelNameProperty = P<SelEquipDetailViewModel>.Register(e => e.EquipAccountModelName);

        /// <summary>
        /// 型号名称
        /// </summary>
        public string EquipAccountModelName
        {
            get { return this.GetProperty(EquipAccountModelNameProperty); }
            set { this.SetProperty(EquipAccountModelNameProperty, value); }
        }
        #endregion

        #region 技术规格 Specifications
        /// <summary>
        /// 技术规格
        /// </summary>
        [Label("技术规格")]
        public static readonly Property<string> SpecificationsProperty = P<SelEquipDetailViewModel>.Register(e => e.Specifications);

        /// <summary>
        /// 技术规格
        /// </summary>
        public string Specifications
        {
            get { return this.GetProperty(SpecificationsProperty); }
            set { this.SetProperty(SpecificationsProperty, value); }
        }
        #endregion

        #region 管理部门 ManageDepartmentName
        /// <summary>
        /// 管理部门
        /// </summary>
        [Label("管理部门")]
        public static readonly Property<string> ManageDepartmentNameProperty = P<SelEquipDetailViewModel>.Register(e => e.ManageDepartmentName);

        /// <summary>
        /// 管理部门
        /// </summary>
        public string ManageDepartmentName
        {
            get { return this.GetProperty(ManageDepartmentNameProperty); }
            set { this.SetProperty(ManageDepartmentNameProperty, value); }
        }
        #endregion

        #region 使用部门 UseDepartmentName
        /// <summary>
        /// 使用部门
        /// </summary>
        [Label("使用部门")]
        public static readonly Property<string> UseDepartmentNameProperty = P<SelEquipDetailViewModel>.Register(e => e.UseDepartmentName);

        /// <summary>
        /// 使用部门
        /// </summary>
        public string UseDepartmentName
        {
            get { return this.GetProperty(UseDepartmentNameProperty); }
            set { this.SetProperty(UseDepartmentNameProperty, value); }
        }
        #endregion

        #region 生产厂家 Manufacturer
        /// <summary>
        /// 生产厂家
        /// </summary>
        [Label("生产厂家")]
        public static readonly Property<string> ManufacturerProperty = P<SelEquipDetailViewModel>.Register(e => e.Manufacturer);

        /// <summary>
        /// 生产厂家
        /// </summary>
        public string Manufacturer
        {
            get { return this.GetProperty(ManufacturerProperty); }
            set { this.SetProperty(ManufacturerProperty, value); }
        }
        #endregion

        #region 原厂序列号 OriginalSerialNumber
        /// <summary>
        /// 原厂序列号
        /// </summary>
        [Label("原厂序列号")]
        public static readonly Property<string> OriginalSerialNumberProperty = P<SelEquipDetailViewModel>.Register(e => e.OriginalSerialNumber);

        /// <summary>
        /// 原厂序列号
        /// </summary>
        public string OriginalSerialNumber
        {
            get { return this.GetProperty(OriginalSerialNumberProperty); }
            set { this.SetProperty(OriginalSerialNumberProperty, value); }
        }
        #endregion

        #region 保修期 WarrantyPeriod
        /// <summary>
        /// 保修期
        /// </summary>
        [Label("保修期")]
        public static readonly Property<DateTime?> WarrantyPeriodProperty = P<SelEquipDetailViewModel>.Register(e => e.WarrantyPeriod);

        /// <summary>
        /// 保修期
        /// </summary>
        public DateTime? WarrantyPeriod
        {
            get { return this.GetProperty(WarrantyPeriodProperty); }
            set { this.SetProperty(WarrantyPeriodProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 选择设备台账查询
    /// </summary>
    [QueryEntity, Serializable]
    [Label("选择设备台账查询")]
    public class SelEquipDetailCriteriaViewModel : Criteria
    {
        #region 设备编码 Code
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> CodeProperty = P<SelEquipDetailCriteriaViewModel>.Register(e => e.Code);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 设备名称 Name
        /// <summary>
        /// 设备名称
        /// </summary>        
        [Label("设备名称")]
        public static readonly Property<string> NameProperty = P<SelEquipDetailCriteriaViewModel>.Register(e => e.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 型号编码 ModelCode
        /// <summary>
        /// 型号编码
        /// </summary>
        [Label("型号编码")]
        public static readonly Property<string> ModelCodeProperty = P<SelEquipDetailCriteriaViewModel>.Register(e => e.ModelCode);

        /// <summary>
        /// 型号编码
        /// </summary>
        public string ModelCode
        {
            get { return this.GetProperty(ModelCodeProperty); }
            set { this.SetProperty(ModelCodeProperty, value); }
        }
        #endregion

        #region 型号名称 ModelName
        /// <summary>
        /// 型号名称
        /// </summary>
        [Label("型号名称")]
        public static readonly Property<string> ModelNameProperty = P<SelEquipDetailCriteriaViewModel>.Register(e => e.ModelName);

        /// <summary>
        /// 型号名称
        /// </summary>
        public string ModelName
        {
            get { return this.GetProperty(ModelNameProperty); }
            set { this.SetProperty(ModelNameProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkShopIdProperty = P<SelEquipDetailCriteriaViewModel>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkShopId
        {
            get { return (double?)GetRefNullableId(WorkShopIdProperty); }
            set { SetRefNullableId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty = P<SelEquipDetailCriteriaViewModel>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return GetRefEntity(WorkShopProperty); }
            set { SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 产线 Resource
        /// <summary>
        /// 产线Id
        /// </summary>
        [Label("产线")]
        public static readonly IRefIdProperty ResourceIdProperty
            = P<SelEquipDetailCriteriaViewModel>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)GetRefNullableId(ResourceIdProperty); }
            set { SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 产线
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> ResourceProperty
            = P<SelEquipDetailCriteriaViewModel>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public Enterprise Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<SelEquipDetailCriteriaViewModel>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)GetRefNullableId(ProcessIdProperty); }
            set { SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<SelEquipDetailCriteriaViewModel>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 采购订单 PurchaseOrder
        /// <summary>
        /// 采购订单
        /// </summary>
        [Label("采购订单")]
        public static readonly Property<string> PurchaseOrderProperty = P<SelEquipDetailCriteriaViewModel>.Register(e => e.PurchaseOrder);

        /// <summary>
        /// 采购订单
        /// </summary>
        public string PurchaseOrder
        {
            get { return this.GetProperty(PurchaseOrderProperty); }
            set { this.SetProperty(PurchaseOrderProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询逻辑
        /// </summary>
        /// <returns>返回查询后的数据</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<EquipmentSetupController>().CriteriaSelEquipDetails(this);
        }
    }
}
