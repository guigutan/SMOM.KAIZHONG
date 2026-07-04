using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Report.EquipCostAnalyses
{
    /// <summary>
    /// 设备成本分析
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备成本分析")]
    public class EquipCostInfo : ViewModel
    {

        #region 设备编码 EquipAccount
        /// <summary>
        /// 设备编码Id
        /// </summary>
        [Label("设备编码")]
        public static readonly IRefIdProperty EquipAccountIdProperty = P<EquipCostInfo>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<EquipAccount> EquipAccountProperty = P<EquipCostInfo>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备编码
        /// </summary>
        public EquipAccount EquipAccount
        {
            get { return GetRefEntity(EquipAccountProperty); }
            set { SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 设备名称 EquipName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipNameProperty = P<EquipCostInfo>.RegisterView(e => e.EquipName, p => p.EquipAccount.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipName
        {
            get { return this.GetProperty(EquipNameProperty); }
            set { this.SetProperty(EquipNameProperty, value); }
        }
        #endregion

        #region 设备名称 EquipCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipCodeProperty = P<EquipCostInfo>.RegisterView(e => e.EquipCode, p => p.EquipAccount.Code);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipCode
        {
            get { return this.GetProperty(EquipCodeProperty); }
            set { this.SetProperty(EquipCodeProperty, value); }
        }
        #endregion

        #region 维修工时 RepairCost
        /// <summary>
        /// 维修工时
        /// </summary>
        [Label("维修工时（h）")]
        public static readonly Property<decimal> RepairCostProperty = P<EquipCostInfo>.Register(e => e.RepairCost);

        /// <summary>
        /// 维修工时
        /// </summary>
        public decimal RepairCost
        {
            get { return this.GetProperty(RepairCostProperty); }
            set { this.SetProperty(RepairCostProperty, value); }
        }
        #endregion

        #region 保养工时 MaintainCost
        /// <summary>
        /// 保养工时
        /// </summary>
        [Label("保养工时（h）")]
        public static readonly Property<decimal> MaintainCostProperty = P<EquipCostInfo>.Register(e => e.MaintainCost);

        /// <summary>
        /// 保养工时
        /// </summary>
        public decimal MaintainCost
        {
            get { return this.GetProperty(MaintainCostProperty); }
            set { this.SetProperty(MaintainCostProperty, value); }
        }
        #endregion

        #region 备件成本 SparePartCost
        /// <summary>
        /// 备件成本
        /// </summary>
        [Label("备件成本（元）")]
        public static readonly Property<decimal> SparePartCostProperty = P<EquipCostInfo>.Register(e => e.SparePartCost);

        /// <summary>
        /// 备件成本
        /// </summary>
        public decimal SparePartCost
        {
            get { return this.GetProperty(SparePartCostProperty); }
            set { this.SetProperty(SparePartCostProperty, value); }
        }
        #endregion

        #region 委外维修 OutsourceCost
        /// <summary>
        /// 委外维修
        /// </summary>
        [Label("委外维修（元）")]
        public static readonly Property<decimal> OutsourceCostProperty = P<EquipCostInfo>.Register(e => e.OutsourceCost);

        /// <summary>
        /// 委外维修
        /// </summary>
        public decimal OutsourceCost
        {
            get { return this.GetProperty(OutsourceCostProperty); }
            set { this.SetProperty(OutsourceCostProperty, value); }
        }
        #endregion

        #region 能耗成本 EnergyConsumptionCost
        /// <summary>
        /// 能耗成本
        /// </summary>
        [Label("能耗成本（元）")]
        public static readonly Property<decimal> EnergyConsumptionCostProperty = P<EquipCostInfo>.Register(e => e.EnergyConsumptionCost);

        /// <summary>
        /// 能耗成本
        /// </summary>
        public decimal EnergyConsumptionCost
        {
            get { return this.GetProperty(EnergyConsumptionCostProperty); }
            set { this.SetProperty(EnergyConsumptionCostProperty, value); }
        }
        #endregion

        #region 折旧成本 DepreciationCost
        /// <summary>
        /// 折旧成本
        /// </summary>
        [Label("折旧成本（元）")]
        public static readonly Property<decimal> DepreciationCostProperty = P<EquipCostInfo>.Register(e => e.DepreciationCost);

        /// <summary>
        /// 折旧成本
        /// </summary>
        public decimal DepreciationCost
        {
            get { return this.GetProperty(DepreciationCostProperty); }
            set { this.SetProperty(DepreciationCostProperty, value); }
        }
        #endregion


        #region 工时成本合计 TotalWokerHourCost
        /// <summary>
        /// 工时成本合计
        /// </summary>
        [Label("工时成本合计（h）")]
        public static readonly Property<decimal> TotalWokerHourCostProperty = P<EquipCostInfo>.Register(e => e.TotalWokerHourCost);

        /// <summary>
        /// 工时成本合计
        /// </summary>
        public decimal TotalWokerHourCost
        {
            get { return this.GetProperty(TotalWokerHourCostProperty); }
            set { this.SetProperty(TotalWokerHourCostProperty, value); }
        }
        #endregion

        #region 费用成本合计 TotalCost
        /// <summary>
        /// 费用成本合计
        /// </summary>
        [Label("费用成本合计（元）")]
        public static readonly Property<decimal> TotalCostProperty = P<EquipCostInfo>.Register(e => e.TotalCost);

        /// <summary>
        /// 费用成本合计
        /// </summary>
        public decimal TotalCost
        {
            get { return this.GetProperty(TotalCostProperty); }
            set { this.SetProperty(TotalCostProperty, value); }
        }
        #endregion


        #region 月份 Month
        /// <summary>
        /// 月份（用于记录是哪个月份的）
        /// </summary>
        [Label("月份")]
        public static readonly Property<int> MonthProperty = P<EquipCostInfo>.Register(e => e.Month);

        /// <summary>
        /// 月份
        /// </summary>
        public int Month
        {
            get { return this.GetProperty(MonthProperty); }
            set { this.SetProperty(MonthProperty, value); }
        }
        #endregion



    }
}
