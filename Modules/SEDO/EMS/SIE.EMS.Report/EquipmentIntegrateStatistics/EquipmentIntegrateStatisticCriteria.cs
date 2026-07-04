using SIE.Domain;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.Equipments.Models;
using SIE.EMS.Report.EquipCostAnalyses;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.Report.EquipmentIntegrateStatistics
{
    /// <summary>
    /// 设备综合统计报表查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("设备综合统计报表查询实体")]
    public class EquipmentIntegrateStatisticCriteria : Criteria
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EquipmentIntegrateStatisticCriteria()
        {
            Year = DateTime.Now.Date;
        }

        #region 设备类型 EquipType
        /// <summary>
        /// 设备类型Id
        /// </summary>
        [Label("设备类型")]
        public static readonly IRefIdProperty EquipTypeIdProperty =
            P<EquipmentIntegrateStatisticCriteria>.RegisterRefId(e => e.EquipTypeId, ReferenceType.Normal);

        /// <summary>
        /// 设备类型Id
        /// </summary>
        public double? EquipTypeId
        {
            get { return (double?)this.GetRefNullableId(EquipTypeIdProperty); }
            set { this.SetRefNullableId(EquipTypeIdProperty, value); }
        }

        /// <summary>
        /// 设备类型
        /// </summary>
        public static readonly RefEntityProperty<EquipType> EquipTypeProperty =
            P<EquipmentIntegrateStatisticCriteria>.RegisterRef(e => e.EquipType, EquipTypeIdProperty);

        /// <summary>
        /// 设备类型
        /// </summary>
        public EquipType EquipType
        {
            get { return this.GetRefEntity(EquipTypeProperty); }
            set { this.SetRefEntity(EquipTypeProperty, value); }
        }
        #endregion

        #region 设备型号 EquipModel
        /// <summary>
        /// 设备型号Id
        /// </summary>
        [Label("设备型号")]
        public static readonly IRefIdProperty EquipModelIdProperty =
            P<EquipmentIntegrateStatisticCriteria>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

        /// <summary>
        /// 设备型号Id
        /// </summary>
        public double? EquipModelId
        {
            get { return (double?)this.GetRefNullableId(EquipModelIdProperty); }
            set { this.SetRefNullableId(EquipModelIdProperty, value); }
        }

        /// <summary>
        /// 设备型号
        /// </summary>
        public static readonly RefEntityProperty<EquipModel> EquipModelProperty =
            P<EquipmentIntegrateStatisticCriteria>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

        /// <summary>
        /// 设备型号
        /// </summary>
        public EquipModel EquipModel
        {
            get { return this.GetRefEntity(EquipModelProperty); }
            set { this.SetRefEntity(EquipModelProperty, value); }
        }
        #endregion

        #region 设备编码 EquipCode
        /// <summary>
        /// 设备编码Id
        /// </summary>
        [Label("设备编码")]
        public static readonly IRefIdProperty EquipCodeIdProperty =
            P<EquipmentIntegrateStatisticCriteria>.RegisterRefId(e => e.EquipCodeId, ReferenceType.Normal);

        /// <summary>
        /// 设备编码Id
        /// </summary>
        public double? EquipCodeId
        {
            get { return (double?)this.GetRefNullableId(EquipCodeIdProperty); }
            set { this.SetRefNullableId(EquipCodeIdProperty, value); }
        }

        /// <summary>
        /// 设备编码
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipCodeProperty =
            P<EquipmentIntegrateStatisticCriteria>.RegisterRef(e => e.EquipCode, EquipCodeIdProperty);

        /// <summary>
        /// 设备编码
        /// </summary>
        public EquipAccountSelect EquipCode
        {
            get { return this.GetRefEntity(EquipCodeProperty); }
            set { this.SetRefEntity(EquipCodeProperty, value); }
        }
        #endregion

        #region 使用部门 UseDepartment
        /// <summary>
        /// 使用部门Id
        /// </summary>
        [Label("使用部门")]
        public static readonly IRefIdProperty UseDepartmentIdProperty =
            P<EquipmentIntegrateStatisticCriteria>.RegisterRefId(e => e.UseDepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 使用部门Id
        /// </summary>
        public double? UseDepartmentId
        {
            get { return (double?)this.GetRefNullableId(UseDepartmentIdProperty); }
            set { this.SetRefNullableId(UseDepartmentIdProperty, value); }
        }

        /// <summary>
        /// 使用部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> UseDepartmentProperty =
            P<EquipmentIntegrateStatisticCriteria>.RegisterRef(e => e.UseDepartment, UseDepartmentIdProperty);

        /// <summary>
        /// 使用部门
        /// </summary>
        public Enterprise UseDepartment
        {
            get { return this.GetRefEntity(UseDepartmentProperty); }
            set { this.SetRefEntity(UseDepartmentProperty, value); }
        }
        #endregion

        #region 车间 WorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkShopIdProperty =
            P<EquipmentIntegrateStatisticCriteria>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkShopId
        {
            get { return (double?)this.GetRefNullableId(WorkShopIdProperty); }
            set { this.SetRefNullableId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty =
            P<EquipmentIntegrateStatisticCriteria>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return this.GetRefEntity(WorkShopProperty); }
            set { this.SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 产线 WipResource
        /// <summary>
        /// 产线Id
        /// </summary>
        [Label("产线")]
        public static readonly IRefIdProperty WipResourceIdProperty =
            P<EquipmentIntegrateStatisticCriteria>.RegisterRefId(e => e.WipResourceId, ReferenceType.Normal);

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
            P<EquipmentIntegrateStatisticCriteria>.RegisterRef(e => e.WipResource, WipResourceIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public Enterprise WipResource
        {
            get { return this.GetRefEntity(WipResourceProperty); }
            set { this.SetRefEntity(WipResourceProperty, value); }
        }
        #endregion

        #region 年月 YearAndMonth
        /// <summary>
        /// 年月
        /// </summary>
        [Label("年月")]
        public static readonly Property<DateTime?> YearAndMonthProperty
            = P<EquipmentIntegrateStatisticCriteria>.Register(e => e.Year);

        /// <summary>
        /// 年月
        /// </summary>
        public DateTime? Year
        {
            get { return GetProperty(YearAndMonthProperty); }
            set { SetProperty(YearAndMonthProperty, value); }
        }
        #endregion

        #region 利用率标准（%） UtilizationRate
        /// <summary>
        /// 利用率标准（%）
        /// </summary>
        [Label("利用率标准（%）")]
        public static readonly Property<int> UtilizationRateProperty 
            = P<EquipmentIntegrateStatisticCriteria>.Register(e => e.UtilizationRate);

        /// <summary>
        /// 利用率标准（%）
        /// </summary>
        public int UtilizationRate
        {
            get { return this.GetProperty(UtilizationRateProperty); }
            set { this.SetProperty(UtilizationRateProperty, value); }
        }
        #endregion

        #region 设备名称 EquipName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipNameProperty = P<EquipmentIntegrateStatisticCriteria>.Register(e => e.EquipName);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipName
        {
            get { return this.GetProperty(EquipNameProperty); }
            set { this.SetProperty(EquipNameProperty,value); }
        }
        #endregion


        #region 月份 Month
        /// <summary>
        /// 月份
        /// </summary>
        [Label("月份")]
        public static readonly Property<Month> MonthProperty = P<EquipmentIntegrateStatisticCriteria>.Register(e => e.Month);

        /// <summary>
        /// 月份
        /// </summary>
        public Month Month
        {
            get { return this.GetProperty(MonthProperty); }
            set { this.SetProperty(MonthProperty, value); }
        }
        #endregion


    }
}