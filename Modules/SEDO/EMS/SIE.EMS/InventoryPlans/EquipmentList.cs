using SIE.Core.Enums;
using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EMS.InventoryPlans
{
    /// <summary>
    /// 设备清单
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备清单")]
    public class EquipmentList : DataEntity
    {

        #region 盘点计划 InventoryPlan
        /// <summary>
        /// 盘点计划Id
        /// </summary>
        [Label("盘点计划")]
        public static readonly IRefIdProperty InventoryPlanIdProperty =
            P<EquipmentList>.RegisterRefId(e => e.InventoryPlanId, ReferenceType.Normal);

        /// <summary>
        /// 盘点计划Id
        /// </summary>
        public double InventoryPlanId
        {
            get { return (double)this.GetRefId(InventoryPlanIdProperty); }
            set { this.SetRefId(InventoryPlanIdProperty, value); }
        }

        /// <summary>
        /// 盘点计划
        /// </summary>
        public static readonly RefEntityProperty<InventoryPlan> InventoryPlanProperty =
            P<EquipmentList>.RegisterRef(e => e.InventoryPlan, InventoryPlanIdProperty);

        /// <summary>
        /// 盘点计划
        /// </summary>
        public InventoryPlan InventoryPlan
        {
            get { return this.GetRefEntity(InventoryPlanProperty); }
            set { this.SetRefEntity(InventoryPlanProperty, value); }
        }
        #endregion

        #region 设备台账 EquipAccout
        /// <summary>
        /// 设备台账Id
        /// </summary>
        [Label("设备台账")]
        public static readonly IRefIdProperty EquipAccoutIdProperty =
            P<EquipmentList>.RegisterRefId(e => e.EquipAccoutId, ReferenceType.Normal);

        /// <summary>
        /// 设备台账Id
        /// </summary>
        public double? EquipAccoutId
        {
            get { return (double?)this.GetRefId(EquipAccoutIdProperty); }
            set { this.SetRefId(EquipAccoutIdProperty, value); }
        }

        /// <summary>
        /// 设备台账
        /// </summary>
        public static readonly RefEntityProperty<EquipAccount> EquipAccoutProperty =
            P<EquipmentList>.RegisterRef(e => e.EquipAccout, EquipAccoutIdProperty);

        /// <summary>
        /// 设备台账
        /// </summary>
        public EquipAccount EquipAccout
        {
            get { return this.GetRefEntity(EquipAccoutProperty); }
            set { this.SetRefEntity(EquipAccoutProperty, value); }
        }
        #endregion

        #region 设备型号 EquipModel
        /// <summary>
        /// 设备型号Id
        /// </summary>
        [Label("设备型号")]
        public static readonly IRefIdProperty EquipModelIdProperty = P<EquipmentList>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

        /// <summary>
        /// 设备型号Id
        /// </summary>
        public double? EquipModelId
        {
            get { return (double?)GetRefNullableId(EquipModelIdProperty); }
            set { SetRefNullableId(EquipModelIdProperty, value); }
        }

        /// <summary>
        /// 设备型号
        /// </summary>
        public static readonly RefEntityProperty<EquipModel> EquipModelProperty = P<EquipmentList>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

        /// <summary>
        /// 设备型号
        /// </summary>
        public EquipModel EquipModel
        {
            get { return GetRefEntity(EquipModelProperty); }
            set { SetRefEntity(EquipModelProperty, value); }
        }
        #endregion

        #region 设备类型 EquipType
        /// <summary>
        /// 设备类型Id
        /// </summary>
        [Label("设备类型")]
        public static readonly IRefIdProperty EquipTypeIdProperty = P<EquipmentList>.RegisterRefId(e => e.EquipTypeId, ReferenceType.Normal);

        /// <summary>
        /// 设备类型Id
        /// </summary>
        public double? EquipTypeId
        {
            get { return (double?)GetRefNullableId(EquipTypeIdProperty); }
            set { SetRefNullableId(EquipTypeIdProperty, value); }
        }

        /// <summary>
        /// 设备类型
        /// </summary>
        public static readonly RefEntityProperty<EquipType> EquipTypeProperty = P<EquipmentList>.RegisterRef(e => e.EquipType, EquipTypeIdProperty);

        /// <summary>
        /// 设备类型
        /// </summary>
        public EquipType EquipType
        {
            get { return GetRefEntity(EquipTypeProperty); }
            set { SetRefEntity(EquipTypeProperty, value); }
        }
        #endregion

        #region 使用部门 UseDept
        /// <summary>
        /// 使用部门Id
        /// </summary>
        [Label("使用部门")]
        public static readonly IRefIdProperty UseDeptIdProperty = P<EquipmentList>.RegisterRefId(e => e.UseDeptId, ReferenceType.Normal);

        /// <summary>
        /// 使用部门Id
        /// </summary>
        public double? UseDeptId
        {
            get { return (double?)GetRefNullableId(UseDeptIdProperty); }
            set { SetRefNullableId(UseDeptIdProperty, value); }
        }

        /// <summary>
        /// 使用部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> UseDeptProperty = P<EquipmentList>.RegisterRef(e => e.UseDept, UseDeptIdProperty);

        /// <summary>
        /// 使用部门
        /// </summary>
        public Enterprise UseDept
        {
            get { return GetRefEntity(UseDeptProperty); }
            set { SetRefEntity(UseDeptProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkShopIdProperty = P<EquipmentList>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty = P<EquipmentList>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return GetRefEntity(WorkShopProperty); }
            set { SetRefEntity(WorkShopProperty, value); }
        }
        #endregion


        #region 视图属性

        #region 设备编码 EquipCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipCodeProperty = P<EquipmentList>.RegisterView(e => e.EquipCode, p => p.EquipAccout.Code);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipCode
        {
            get { return this.GetProperty(EquipCodeProperty); }
        }
        #endregion

        #region 设备名称 EquipName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipNameProperty = P<EquipmentList>.RegisterView(e => e.EquipName, p => p.EquipAccout.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipName
        {
            get { return this.GetProperty(EquipNameProperty); }
        }
        #endregion

        #region 设备别名 EquipAlias
        /// <summary>
        /// 设备别名
        /// </summary>
        [Label("设备别名")]
        public static readonly Property<string> EquipAliasProperty = P<EquipmentList>.RegisterView(e => e.EquipAlias, p => p.EquipAccout.Alias);

        /// <summary>
        /// 设备别名
        /// </summary>
        public string EquipAlias
        {
            get { return this.GetProperty(EquipAliasProperty); }
        }
        #endregion

        #region 设备型号编码 EquipModelCode
        /// <summary>
        /// 设备型号编码
        /// </summary>
        [Label("设备型号编码")]
        public static readonly Property<string> EquipModelCodeProperty = P<EquipmentList>.RegisterView(e => e.EquipModelCode, p => p.EquipModel.Code);

        /// <summary>
        /// 设备型号编码
        /// </summary>
        public string EquipModelCode
        {
            get { return this.GetProperty(EquipModelCodeProperty); }
        }
        #endregion

        #region 设备型号名称 EquipModelName
        /// <summary>
        /// 设备型号名称
        /// </summary>
        [Label("设备型号名称")]
        public static readonly Property<string> EquipModelNameProperty = P<EquipmentList>.RegisterView(e => e.EquipModelName, p => p.EquipModel.Name);

        /// <summary>
        /// 设备型号名称
        /// </summary>
        public string EquipModelName
        {
            get { return this.GetProperty(EquipModelNameProperty); }
        }
        #endregion

        #region 管理状态 EquipUseState
        /// <summary>
        /// 管理状态
        /// </summary>
        [Label("管理状态")]
        public static readonly Property<AccountUseState> EquipUseStateProperty = P<EquipmentList>.RegisterView(e => e.EquipUseState, p => p.EquipAccout.UseState);

        /// <summary>
        /// 管理状态
        /// </summary>
        public AccountUseState EquipUseState
        {
            get { return this.GetProperty(EquipUseStateProperty); }
        }
        #endregion

        #region 设备类型编码 EquipTypeCode
        /// <summary>
        /// 设备类型编码
        /// </summary>
        [Label("设备类型编码")]
        public static readonly Property<string> EquipTypeCodeProperty = P<EquipmentList>.RegisterView(e => e.EquipTypeCode, p => p.EquipType.TypeCode);

        /// <summary>
        /// 设备类型编码
        /// </summary>
        public string EquipTypeCode
        {
            get { return this.GetProperty(EquipTypeCodeProperty); }
        }
        #endregion

        #region 设备类型名称 EquipTypeName
        /// <summary>
        /// 设备类型名称
        /// </summary>
        [Label("设备类型名称")]
        public static readonly Property<string> EquipTypeNameProperty = P<EquipmentList>.RegisterView(e => e.EquipTypeName, p => p.EquipType.TypeName);

        /// <summary>
        /// 设备类型名称
        /// </summary>
        public string EquipTypeName
        {
            get { return this.GetProperty(EquipTypeNameProperty); }
        }
        #endregion

        #region 设备类别 EquipModelCategory
        /// <summary>
        /// 设备类别
        /// </summary>
        [Label("设备类别")]
        public static readonly Property<string> EquipModelCategoryProperty = P<EquipmentList>.RegisterView(e => e.EquipModelCategory, p => p.EquipModel.TypeCategory);

        /// <summary>
        /// 设备类别
        /// </summary>
        public string EquipModelCategory
        {
            get { return this.GetProperty(EquipModelCategoryProperty); }
        }
        #endregion

        #region 生产厂家 Manufacturer
        /// <summary>
        /// 生产厂家
        /// </summary>
        [Label("生产厂家")]
        public static readonly Property<string> ManufacturerProperty = P<EquipmentList>.RegisterView(e => e.Manufacturer, p => p.EquipAccout.Manufacturer);

        /// <summary>
        /// 生产厂家
        /// </summary>
        public string Manufacturer
        {
            get { return this.GetProperty(ManufacturerProperty); }
        }
        #endregion

        #region 入场日期 EnterDate
        /// <summary>
        /// 入场日期
        /// </summary>
        [Label("入场日期")]
        public static readonly Property<DateTime?> EnterDateProperty = P<EquipmentList>.RegisterView(e => e.EnterDate, p => p.EquipAccout.EnterDate);

        /// <summary>
        /// 入场日期
        /// </summary>
        public DateTime? EnterDate
        {
            get { return this.GetProperty(EnterDateProperty); }
        }
        #endregion

        #region 位置 InstallationLocation
        /// <summary>
        /// 位置
        /// </summary>
        [Label("位置")]
        public static readonly Property<string> InstallationLocationProperty = P<EquipmentList>.RegisterView(e => e.InstallationLocation, p => p.EquipAccout.InstallationLocation);

        /// <summary>
        /// 位置
        /// </summary>
        public string InstallationLocation
        {
            get { return this.GetProperty(InstallationLocationProperty); }
        }
        #endregion

        #endregion

    }

    /// <summary>
    /// 设备清单 实体配置
    /// </summary>
    internal class EquipmentListConfig : EntityConfig<EquipmentList>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_INV_EQUIP_LIST").MapAllProperties();
            //Meta.EnablePhantoms();
        }
    }

}
