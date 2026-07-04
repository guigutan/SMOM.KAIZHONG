using SIE.Domain;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.Equipments.Models;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;

namespace SIE.EMS.Report.EquipCostAnalyses
{
    /// <summary>
    /// 设备成本分析查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("设备成本分析查询实体")]
    public class EquipCostAnalyseCriteria : Criteria
    {
        #region 设备类型 EquipType
        /// <summary>
        /// 设备类型Id
        /// </summary>
        [Label("设备类型")]
        public static readonly IRefIdProperty EquipTypeIdProperty = P<EquipCostAnalyseCriteria>.RegisterRefId(e => e.EquipTypeId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<EquipType> EquipTypeProperty = P<EquipCostAnalyseCriteria>.RegisterRef(e => e.EquipType, EquipTypeIdProperty);

        /// <summary>
        /// 设备类型
        /// </summary>
        public EquipType EquipType
        {
            get { return GetRefEntity(EquipTypeProperty); }
            set { SetRefEntity(EquipTypeProperty, value); }
        }
        #endregion

        #region 设备型号 EquipModel
        /// <summary>
        /// 设备型号Id
        /// </summary>
        [Label("设备型号")]
        public static readonly IRefIdProperty EquipModelIdProperty = P<EquipCostAnalyseCriteria>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<EquipModel> EquipModelProperty = P<EquipCostAnalyseCriteria>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

        /// <summary>
        /// 设备型号
        /// </summary>
        public EquipModel EquipModel
        {
            get { return GetRefEntity(EquipModelProperty); }
            set { SetRefEntity(EquipModelProperty, value); }
        }
        #endregion

        #region 设备编码 EquipAccount
        /// <summary>
        /// 设备编码Id
        /// </summary>
        [Label("设备编码")]
        public static readonly IRefIdProperty EquipAccountIdProperty = P<EquipCostAnalyseCriteria>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备编码Id
        /// </summary>
        public double? EquipAccountId
        {
            get { return (double?)GetRefNullableId(EquipAccountIdProperty); }
            set { SetRefNullableId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备编码
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty = P<EquipCostAnalyseCriteria>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备编码
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return GetRefEntity(EquipAccountProperty); }
            set { SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 使用部门 Department
        /// <summary>
        /// 使用部门Id
        /// </summary>
        [Label("使用部门")]
        public static readonly IRefIdProperty DepartmentIdProperty = P<EquipCostAnalyseCriteria>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 使用部门Id
        /// </summary>
        public double? DepartmentId
        {
            get { return (double?)GetRefNullableId(DepartmentIdProperty); }
            set { SetRefNullableId(DepartmentIdProperty, value); }
        }

        /// <summary>
        /// 使用部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> DepartmentProperty = P<EquipCostAnalyseCriteria>.RegisterRef(e => e.Department, DepartmentIdProperty);

        /// <summary>
        /// 使用部门
        /// </summary>
        public Enterprise Department
        {
            get { return GetRefEntity(DepartmentProperty); }
            set { SetRefEntity(DepartmentProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkShopIdProperty = P<EquipCostAnalyseCriteria>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty = P<EquipCostAnalyseCriteria>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

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
        public static readonly IRefIdProperty ResourceIdProperty = P<EquipCostAnalyseCriteria>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Enterprise> ResourceProperty = P<EquipCostAnalyseCriteria>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public Enterprise Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 年份 Year
        /// <summary>
        /// 年份
        /// </summary>
        [Label("年份")]
        public static readonly Property<DateTime?> YearProperty = P<EquipCostAnalyseCriteria>.Register(e => e.Year);

        /// <summary>
        /// 年份
        /// </summary>
        public DateTime? Year
        {
            get { return this.GetProperty(YearProperty); }
            set { this.SetProperty(YearProperty, value); }
        }
        #endregion

        #region 开始月份 BeginMonth
        /// <summary>
        /// 时间
        /// </summary>
        [Label("开始月份")]
        public static readonly Property<Month?> BeginMonthProperty = P<EquipCostAnalyseCriteria>.Register(e => e.BeginMonth);

        /// <summary>
        /// 开始月份
        /// </summary>
        public Month? BeginMonth
        {
            get { return GetProperty(BeginMonthProperty); }
            set { SetProperty(BeginMonthProperty, value); }
        }
        #endregion

        #region 结束月份 EndMonth
        /// <summary>
        /// 结束月份
        /// </summary>
        [Label("结束月份")]
        public static readonly Property<Month?> EndMonthProperty = P<EquipCostAnalyseCriteria>.Register(e => e.EndMonth);

        /// <summary>
        /// 开始月份
        /// </summary>
        public Month? EndMonth
        {
            get { return GetProperty(EndMonthProperty); }
            set { SetProperty(EndMonthProperty, value); }
        }
        #endregion

        #region 设备名称 EquipName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipNameProperty = P<EquipCostAnalyseCriteria>.Register(e => e.EquipName);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipName
        {
            get { return this.GetProperty(EquipNameProperty); }
            set { this.SetProperty(EquipNameProperty, value); }
        }
        #endregion
    }
}
