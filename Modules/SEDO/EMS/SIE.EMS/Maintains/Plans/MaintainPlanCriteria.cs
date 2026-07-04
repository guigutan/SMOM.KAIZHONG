using SIE.Core.Enums;
using SIE.Domain;
using SIE.EMS.Maintains.Controller;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Tech.Processs;
using System;

namespace SIE.EMS.Maintains.Plans
{
    /// <summary>
    /// 保养计划查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("保养计划查询实体")]
    public partial class MaintainPlanCriteria : Criteria
    {
        /// <summary>
        /// 设备类别快码组
        /// </summary>
        public const string EquipTypeCatalogType = "EQUIP_TYPE";

        #region 设备编码 EquipCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipCodeProperty = P<MaintainPlanCriteria>.Register(e => e.EquipCode);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipCode
        {
            get { return GetProperty(EquipCodeProperty); }
            set { SetProperty(EquipCodeProperty, value); }
        }
        #endregion

        #region 设备编码 EquipAccount
        /// <summary>
        /// 设备编码Id
        /// </summary>
        [Label("设备编码")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<MaintainPlanCriteria>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备编码Id
        /// </summary>
        public double? EquipAccountId
        {
            get { return (double?)this.GetRefId(EquipAccountIdProperty); }
            set { this.SetRefId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备编码
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty =
            P<MaintainPlanCriteria>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备编码
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 机台号 MachineNo
        /// <summary>
        /// 机台号
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> MachineNoProperty = P<MaintainPlanCriteria>.Register(e => e.MachineNo);

        /// <summary>
        /// 机台号
        /// </summary>
        public string MachineNo
        {
            get { return GetProperty(MachineNoProperty); }
            set { SetProperty(MachineNoProperty, value); }
        }
        #endregion

        #region 设备型号 EquipModel
        /// <summary>
        /// 设备型号Id
        /// </summary>
        [Label("设备型号")]
        public static readonly IRefIdProperty EquipModelIdProperty =
            P<MaintainPlanCriteria>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

        /// <summary>
        /// 设备型号Id
        /// </summary>
        public double? EquipModelId
        {
            get { return (double?)this.GetRefId(EquipModelIdProperty); }
            set { this.SetRefId(EquipModelIdProperty, value); }
        }

        /// <summary>
        /// 设备型号
        /// </summary>
        public static readonly RefEntityProperty<EquipModel> EquipModelProperty =
            P<MaintainPlanCriteria>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

        /// <summary>
        /// 设备型号
        /// </summary>
        public EquipModel EquipModel
        {
            get { return this.GetRefEntity(EquipModelProperty); }
            set { this.SetRefEntity(EquipModelProperty, value); }
        }
        #endregion

        #region 设备类别 EquipTypeCategory
        /// <summary>
        /// 设备类别
        /// </summary>
        [Label("设备类别")]
        public static readonly Property<string> EquipTypeCategoryProperty = P<MaintainPlanCriteria>.Register(e => e.EquipTypeCategory);

        /// <summary>
        /// 设备类别
        /// </summary>
        public string EquipTypeCategory
        {
            get { return this.GetProperty(EquipTypeCategoryProperty); }
            set { this.SetProperty(EquipTypeCategoryProperty, value); }
        }
        #endregion

        #region 周期 ProjectCycle
        /// <summary>
        /// 周期
        /// </summary>
        [Label("周期")]
        public static readonly Property<decimal?> ProjectCycleProperty = P<MaintainPlanCriteria>.Register(e => e.ProjectCycle);

        /// <summary>
        /// 周期
        /// </summary>
        public decimal? ProjectCycle
        {
            get { return GetProperty(ProjectCycleProperty); }
            set { SetProperty(ProjectCycleProperty, value); }
        }
        #endregion

        #region 年份 Year
        /// <summary>
        /// 年份
        /// </summary>
        [Label("年份")]
        public static readonly Property<DateTime?> YearProperty = P<MaintainPlanCriteria>.Register(e => e.Year);

        /// <summary>
        /// 年份
        /// </summary>
        public DateTime? Year
        {
            get { return GetProperty(YearProperty); }
            set { SetProperty(YearProperty, value); }
        }
        #endregion

        #region 车间 Workshop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkshopIdProperty = P<MaintainPlanCriteria>.RegisterRefId(e => e.WorkshopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkshopId
        {
            get { return (double?)GetRefNullableId(WorkshopIdProperty); }
            set { SetRefNullableId(WorkshopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkshopProperty = P<MaintainPlanCriteria>.RegisterRef(e => e.Workshop, WorkshopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise Workshop
        {
            get { return GetRefEntity(WorkshopProperty); }
            set { SetRefEntity(WorkshopProperty, value); }
        }
        #endregion

        #region 产线 WipResource
        /// <summary>
        /// 产线Id
        /// </summary>
        [Label("产线")]
        public static readonly IRefIdProperty WipResourceIdProperty =
            P<MaintainPlanCriteria>.RegisterRefId(e => e.WipResourceId, ReferenceType.Normal);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double? WipResourceId
        {
            get { return (double?)this.GetRefNullableId(WipResourceIdProperty); }
            set { this.SetRefNullableId(WipResourceIdProperty, value); }
        }

        /// <summary>
        /// 产线
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WipResourceProperty =
            P<MaintainPlanCriteria>.RegisterRef(e => e.WipResource, WipResourceIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public Enterprise WipResource
        {
            get { return this.GetRefEntity(WipResourceProperty); }
            set { this.SetRefEntity(WipResourceProperty, value); }
        }
        #endregion

        #region 管理状态 UseState
        /// <summary>
        /// 管理状态
        /// </summary>
        [Label("管理状态")]
        public static readonly Property<AccountUseState?> UseStateProperty = P<MaintainPlanCriteria>.Register(e => e.UseState);

        /// <summary>
        /// 管理状态
        /// </summary>
        public AccountUseState? UseState
        {
            get { return this.GetProperty(UseStateProperty); }
            set { this.SetProperty(UseStateProperty, value); }
        }
        #endregion

        #region 是否显示子设备 IsShowChildEquip
        /// <summary>
        /// 是否显示子设备
        /// </summary>
        [Label("是否显示子设备")]
        public static readonly Property<YesNo?> IsShowChildEquipProperty = P<MaintainPlanCriteria>.Register(e => e.IsShowChildEquip);

        /// <summary>
        /// 是否显示子设备
        /// </summary>
        public YesNo? IsShowChildEquip
        {
            get { return this.GetProperty(IsShowChildEquipProperty); }
            set { this.SetProperty(IsShowChildEquipProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
           return RT.Service.Resolve<MaintainController>().GetEquipMaintainPlans(this);
        }
    }
}