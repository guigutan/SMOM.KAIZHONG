using SIE.Domain;
using SIE.MES.TaskManagement.WipProgress;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TaskManagement.DailyOutputReports
{
    /// <summary>
    /// 日产出报表
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(DailyOutputReportCriteria))]
    [Label("日产出报表")]
    public class DailyOutputReport : ViewModel
    {
        #region 日期 Date
        /// <summary>
        /// 日期
        /// </summary>
        [Label("日期")]
        public static readonly Property<DateTime> DateProperty = P<DailyOutputReport>.Register(e => e.Date);

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date
        {
            get { return this.GetProperty(DateProperty); }
            set { this.SetProperty(DateProperty, value); }
        }
        #endregion

        #region 事业部 Division
        /// <summary>
        /// 事业部
        /// </summary>
        [Label("事业部")]
        public static readonly Property<string> DivisionProperty = P<DailyOutputReport>.Register(e => e.Division);

        /// <summary>
        /// 事业部
        /// </summary>
        public string Division
        {
            get { return this.GetProperty(DivisionProperty); }
            set { this.SetProperty(DivisionProperty, value); }
        }
        #endregion

        #region 部门 Department
        /// <summary>
        /// 部门
        /// </summary>
        [Label("部门")]
        public static readonly Property<string> DepartmentProperty = P<DailyOutputReport>.Register(e => e.Department);

        /// <summary>
        /// 部门
        /// </summary>
        public string Department
        {
            get { return this.GetProperty(DepartmentProperty); }
            set { this.SetProperty(DepartmentProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public static readonly Property<string> FactoryProperty = P<DailyOutputReport>.Register(e => e.Factory);

        /// <summary>
        /// 工厂
        /// </summary>
        public string Factory
        {
            get { return this.GetProperty(FactoryProperty); }
            set { this.SetProperty(FactoryProperty, value); }
        }
        #endregion

        #region MRP控制者 MrpController
        /// <summary>
        /// MRP控制者
        /// </summary>
        [Label("MRP控制者")]
        public static readonly Property<string> MrpControllerProperty = P<DailyOutputReport>.Register(e => e.MrpController);

        /// <summary>
        /// MRP控制者
        /// </summary>
        public string MrpController
        {
            get { return this.GetProperty(MrpControllerProperty); }
            set { this.SetProperty(MrpControllerProperty, value); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<DailyOutputReport>.Register(e => e.ProductCode);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
            set { this.SetProperty(ProductCodeProperty, value); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<DailyOutputReport>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
            set { this.SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 旧物料号 ShortDescription
        /// <summary>
        /// 旧物料号
        /// </summary>
        [Label("旧物料号")]
        public static readonly Property<string> ShortDescriptionProperty = P<DailyOutputReport>.Register(e => e.ShortDescription);

        /// <summary>
        /// 旧物料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
            set { this.SetProperty(ShortDescriptionProperty, value); }
        }
        #endregion

        #region 父级旧物料号 ParentOldItem
        /// <summary>
        /// 父级旧物料号
        /// </summary>
        [Label("父级旧物料号")]
        public static readonly Property<string> ParentOldItemProperty = P<DailyOutputReport>.Register(e => e.ParentOldItem);

        /// <summary>
        /// 父级旧物料号
        /// </summary>
        public string ParentOldItem
        {
            get { return this.GetProperty(ParentOldItemProperty); }
            set { this.SetProperty(ParentOldItemProperty, value); }
        }
        #endregion

        #region 资源编码 ResourceCode
        /// <summary>
        /// 资源编码
        /// </summary>
        [Label("资源编码")]
        public static readonly Property<string> ResourceCodeProperty = P<DailyOutputReport>.Register(e => e.ResourceCode);

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode
        {
            get { return this.GetProperty(ResourceCodeProperty); }
            set { this.SetProperty(ResourceCodeProperty, value); }
        }
        #endregion

        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> ResourceNameProperty = P<DailyOutputReport>.Register(e => e.ResourceName);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
            set { this.SetProperty(ResourceNameProperty, value); }
        }
        #endregion

        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<DailyOutputReport>.Register(e => e.ProcessCode);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
            set { this.SetProperty(ProcessCodeProperty, value); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<DailyOutputReport>.Register(e => e.ProcessName);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
            set { this.SetProperty(ProcessNameProperty, value); }
        }
        #endregion

        #region 标准产能(H) Capacity
        /// <summary>
        /// 标准产能(H)
        /// </summary>
        [Label("标准产能(H)")]
        public static readonly Property<decimal> CapacityProperty = P<DailyOutputReport>.Register(e => e.Capacity);

        /// <summary>
        /// 标准产能(H)
        /// </summary>
        public decimal Capacity
        {
            get { return this.GetProperty(CapacityProperty); }
            set { this.SetProperty(CapacityProperty, value); }
        }
        #endregion

        #region 排程数量 TaskQty
        /// <summary>
        /// 排程数量
        /// </summary>
        [Label("排程数量")]
        public static readonly Property<decimal> TaskQtyProperty = P<DailyOutputReport>.Register(e => e.TaskQty);

        /// <summary>
        /// 排程数量
        /// </summary>
        public decimal TaskQty
        {
            get { return this.GetProperty(TaskQtyProperty); }
            set { this.SetProperty(TaskQtyProperty, value); }
        }
        #endregion

        #region 报工数量 ReportedQty
        /// <summary>
        /// 报工数量
        /// </summary>
        [Label("报工数量")]
        public static readonly Property<decimal> ReportedQtyProperty = P<DailyOutputReport>.Register(e => e.ReportedQty);

        /// <summary>
        /// 报工数量
        /// </summary>
        public decimal ReportedQty
        {
            get { return this.GetProperty(ReportedQtyProperty); }
            set { this.SetProperty(ReportedQtyProperty, value); }
        }
        #endregion

        #region 差异数量 DiffQty
        /// <summary>
        /// 差异数量
        /// </summary>
        [Label("差异数量")]
        public static readonly Property<decimal> DiffQtyProperty = P<DailyOutputReport>.RegisterReadOnly(
            e => e.DiffQty, e => e.GetDiffQty(), DailyOutputReport.TaskQtyProperty, DailyOutputReport.ReportedQtyProperty);
        /// <summary>
        /// 差异数量
        /// </summary>

        public decimal DiffQty
        {
            get { return this.GetProperty(DiffQtyProperty); }
        }
        private decimal GetDiffQty()
        {
            return ReportedQty - TaskQty;
        }
        #endregion

        #region 达成率 AchievementRate
        /// <summary>
        /// 达成率
        /// </summary>
        [Label("达成率")]
        public static readonly Property<string> AchievementRateProperty = P<DailyOutputReport>.RegisterReadOnly(
            e => e.AchievementRate, e => e.GetAchievementRate(), TaskQtyProperty, ReportedQtyProperty);
        /// <summary>
        /// 达成率
        /// </summary>

        public string AchievementRate
        {
            get { return this.GetProperty(AchievementRateProperty); }
        }
        private string GetAchievementRate()
        {
            if (TaskQty == 0) return "0%";
            return (ReportedQty / TaskQty).ToString("P2");
        }
        #endregion

        #region 白班差异

        #region 排程数量 DayShiftTaskQty
        /// <summary>
        /// 排程数量
        /// </summary>
        [Label("排程数量")]
        public static readonly Property<decimal> DayShiftTaskQtyProperty = P<DailyOutputReport>.Register(e => e.DayShiftTaskQty);

        /// <summary>
        /// 排程数量
        /// </summary>
        public decimal DayShiftTaskQty
        {
            get { return this.GetProperty(DayShiftTaskQtyProperty); }
            set { this.SetProperty(DayShiftTaskQtyProperty, value); }
        }
        #endregion

        #region 报工数量 DayShiftReportedQty
        /// <summary>
        /// 报工数量
        /// </summary>
        [Label("报工数量")]
        public static readonly Property<decimal> DayShiftReportedQtyProperty = P<DailyOutputReport>.Register(e => e.DayShiftReportedQty);

        /// <summary>
        /// 报工数量
        /// </summary>
        public decimal DayShiftReportedQty
        {
            get { return this.GetProperty(DayShiftReportedQtyProperty); }
            set { this.SetProperty(DayShiftReportedQtyProperty, value); }
        }
        #endregion

        #region 差异数量 DayShiftDiffQty
        /// <summary>
        /// 差异数量
        /// </summary>
        [Label("差异数量")]
        public static readonly Property<decimal> DayShiftDiffQtyProperty = P<DailyOutputReport>.RegisterReadOnly(
            e => e.DayShiftDiffQty, e => e.GetDayShiftDiffQty(), DailyOutputReport.DayShiftTaskQtyProperty, DailyOutputReport.DayShiftReportedQtyProperty);
        /// <summary>
        /// 差异数量
        /// </summary>

        public decimal DayShiftDiffQty
        {
            get { return this.GetProperty(DayShiftDiffQtyProperty); }
        }
        private decimal GetDayShiftDiffQty()
        {
            return DayShiftReportedQty - DayShiftTaskQty;
        }
        #endregion

        #endregion

        #region 夜班差异

        #region 排程数量 NightShiftTaskQty
        /// <summary>
        /// 排程数量
        /// </summary>
        [Label("排程数量")]
        public static readonly Property<decimal> NightShiftTaskQtyProperty = P<DailyOutputReport>.Register(e => e.NightShiftTaskQty);

        /// <summary>
        /// 排程数量
        /// </summary>
        public decimal NightShiftTaskQty
        {
            get { return this.GetProperty(NightShiftTaskQtyProperty); }
            set { this.SetProperty(NightShiftTaskQtyProperty, value); }
        }
        #endregion

        #region 报工数量 NightShiftReportedQty
        /// <summary>
        /// 报工数量
        /// </summary>
        [Label("报工数量")]
        public static readonly Property<decimal> NightShiftReportedQtyProperty = P<DailyOutputReport>.Register(e => e.NightShiftReportedQty);

        /// <summary>
        /// 报工数量
        /// </summary>
        public decimal NightShiftReportedQty
        {
            get { return this.GetProperty(NightShiftReportedQtyProperty); }
            set { this.SetProperty(NightShiftReportedQtyProperty, value); }
        }
        #endregion

        #region 差异数量 NightShiftDiffQty
        /// <summary>
        /// 差异数量
        /// </summary>
        [Label("差异数量")]
        public static readonly Property<decimal> NightShiftDiffQtyProperty = P<DailyOutputReport>.RegisterReadOnly(
            e => e.NightShiftDiffQty, e => e.GetNightShiftDiffQty(), DailyOutputReport.NightShiftTaskQtyProperty, DailyOutputReport.NightShiftReportedQtyProperty);
        /// <summary>
        /// 差异数量
        /// </summary>

        public decimal NightShiftDiffQty
        {
            get { return this.GetProperty(NightShiftDiffQtyProperty); }
        }
        private decimal GetNightShiftDiffQty()
        {
            return NightShiftReportedQty - NightShiftTaskQty;
        }
        #endregion

        #endregion

        #region 白班小时产出

        #region 8:00-10:00 OuputQty_08to10
        /// <summary>
        /// 8:00-10:00
        /// </summary>
        [Label("8:00-10:00")]
        public static readonly Property<decimal> OuputQty_08to10Property = P<DailyOutputReport>.Register(e => e.OuputQty_08to10);

        /// <summary>
        /// 8:00-10:00
        /// </summary>
        public decimal OuputQty_08to10
        {
            get { return this.GetProperty(OuputQty_08to10Property); }
            set { this.SetProperty(OuputQty_08to10Property, value); }
        }
        #endregion

        #region 10:00-12:00 OuputQty_10to12
        /// <summary>
        /// 10:00-12:00
        /// </summary>
        [Label("10:00-12:00")]
        public static readonly Property<decimal> OuputQty_10to12Property = P<DailyOutputReport>.Register(e => e.OuputQty_10to12);

        /// <summary>
        /// 10:00-12:00
        /// </summary>
        public decimal OuputQty_10to12
        {
            get { return this.GetProperty(OuputQty_10to12Property); }
            set { this.SetProperty(OuputQty_10to12Property, value); }
        }
        #endregion

        #region 12:00-15:00 OuputQty_12to15
        /// <summary>
        /// 12:00-15:00
        /// </summary>
        [Label("12:00-15:00")]
        public static readonly Property<decimal> OuputQty_12to15Property = P<DailyOutputReport>.Register(e => e.OuputQty_12to15);

        /// <summary>
        /// 12:00-15:00
        /// </summary>
        public decimal OuputQty_12to15
        {
            get { return this.GetProperty(OuputQty_12to15Property); }
            set { this.SetProperty(OuputQty_12to15Property, value); }
        }
        #endregion

        #region 15:00-17:00 OuputQty_15to17
        /// <summary>
        /// 15:00-17:00
        /// </summary>
        [Label("15:00-17:00")]
        public static readonly Property<decimal> OuputQty_15to17Property = P<DailyOutputReport>.Register(e => e.OuputQty_15to17);

        /// <summary>
        /// 15:00-17:00
        /// </summary>
        public decimal OuputQty_15to17
        {
            get { return this.GetProperty(OuputQty_15to17Property); }
            set { this.SetProperty(OuputQty_15to17Property, value); }
        }
        #endregion

        #region 17:00-20:00 OuputQty_17to20
        /// <summary>
        /// 17:00-20:00
        /// </summary>
        [Label("17:00-20:00")]
        public static readonly Property<decimal> OuputQty_17to20Property = P<DailyOutputReport>.Register(e => e.OuputQty_17to20);

        /// <summary>
        /// 17:00-20:00
        /// </summary>
        public decimal OuputQty_17to20
        {
            get { return this.GetProperty(OuputQty_17to20Property); }
            set { this.SetProperty(OuputQty_17to20Property, value); }
        }
        #endregion

        #region 8:00-9:00 OuputQty_08to09
        /// <summary>
        /// 8:00-9:00
        /// </summary>
        [Label("8:00-9:00")]
        public static readonly Property<decimal> OuputQty_08to09Property = P<DailyOutputReport>.Register(e => e.OuputQty_08to09);

        /// <summary>
        /// 8:00-9:00
        /// </summary>
        public decimal OuputQty_08to09
        {
            get { return this.GetProperty(OuputQty_08to09Property); }
            set { this.SetProperty(OuputQty_08to09Property, value); }
        }
        #endregion

        #region 9:00-10:00 OuputQty_09to10
        /// <summary>
        /// 9:00-10:00
        /// </summary>
        [Label("8:00-9:00")]
        public static readonly Property<decimal> OuputQty_09to10Property = P<DailyOutputReport>.Register(e => e.OuputQty_09to10);

        /// <summary>
        /// 9:00-10:00
        /// </summary>
        public decimal OuputQty_09to10
        {
            get { return this.GetProperty(OuputQty_09to10Property); }
            set { this.SetProperty(OuputQty_09to10Property, value); }
        }
        #endregion

        #region 10:00-11:00 OuputQty_10to11
        /// <summary>
        /// 10:00-11:00
        /// </summary>
        [Label("10:00-11:00")]
        public static readonly Property<decimal> OuputQty_10to11Property = P<DailyOutputReport>.Register(e => e.OuputQty_10to11);

        /// <summary>
        /// 10:00-11:00
        /// </summary>
        public decimal OuputQty_10to11
        {
            get { return this.GetProperty(OuputQty_10to11Property); }
            set { this.SetProperty(OuputQty_10to11Property, value); }
        }
        #endregion

        #region 11:00-12:00 OuputQty_11to12
        /// <summary>
        /// 11:00-12:00
        /// </summary>
        [Label("11:00-12:00")]
        public static readonly Property<decimal> OuputQty_11to12Property = P<DailyOutputReport>.Register(e => e.OuputQty_11to12);

        /// <summary>
        /// 11:00-12:00
        /// </summary>
        public decimal OuputQty_11to12
        {
            get { return this.GetProperty(OuputQty_11to12Property); }
            set { this.SetProperty(OuputQty_11to12Property, value); }
        }
        #endregion

        #region 12:00-13:00 OuputQty_12to13
        /// <summary>
        /// 12:00-13:00
        /// </summary>
        [Label("12:00-13:00")]
        public static readonly Property<decimal> OuputQty_12to13Property = P<DailyOutputReport>.Register(e => e.OuputQty_12to13);

        /// <summary>
        /// 12:00-13:00
        /// </summary>
        public decimal OuputQty_12to13
        {
            get { return this.GetProperty(OuputQty_12to13Property); }
            set { this.SetProperty(OuputQty_12to13Property, value); }
        }
        #endregion

        #region 13:00-14:00 OuputQty_13to14
        /// <summary>
        /// 13:00-14:00
        /// </summary>
        [Label("13:00-14:00")]
        public static readonly Property<decimal> OuputQty_13to14Property = P<DailyOutputReport>.Register(e => e.OuputQty_13to14);

        /// <summary>
        /// 13:00-14:00
        /// </summary>
        public decimal OuputQty_13to14
        {
            get { return this.GetProperty(OuputQty_13to14Property); }
            set { this.SetProperty(OuputQty_13to14Property, value); }
        }
        #endregion

        #region 14:00-15:00 OuputQty_14to15
        /// <summary>
        /// 14:00-15:00
        /// </summary>
        [Label("14:00-15:00")]
        public static readonly Property<decimal> OuputQty_14to15Property = P<DailyOutputReport>.Register(e => e.OuputQty_14to15);

        /// <summary>
        /// 14:00-15:00
        /// </summary>
        public decimal OuputQty_14to15
        {
            get { return this.GetProperty(OuputQty_14to15Property); }
            set { this.SetProperty(OuputQty_14to15Property, value); }
        }
        #endregion

        #region 15:00-16:00 OuputQty_15to16
        /// <summary>
        /// 15:00-16:00
        /// </summary>
        [Label("15:00-16:00")]
        public static readonly Property<decimal> OuputQty_15to16Property = P<DailyOutputReport>.Register(e => e.OuputQty_15to16);

        /// <summary>
        /// 15:00-16:00
        /// </summary>
        public decimal OuputQty_15to16
        {
            get { return this.GetProperty(OuputQty_15to16Property); }
            set { this.SetProperty(OuputQty_15to16Property, value); }
        }
        #endregion

        #region 16:00-17:00 OuputQty_16to17
        /// <summary>
        /// 16:00-17:00
        /// </summary>
        [Label("16:00-17:00")]
        public static readonly Property<decimal> OuputQty_16to17Property = P<DailyOutputReport>.Register(e => e.OuputQty_16to17);

        /// <summary>
        /// 16:00-17:00
        /// </summary>
        public decimal OuputQty_16to17
        {
            get { return this.GetProperty(OuputQty_16to17Property); }
            set { this.SetProperty(OuputQty_16to17Property, value); }
        }
        #endregion

        #region 17:00-18:00 OuputQty_17to18
        /// <summary>
        /// 17:00-18:00
        /// </summary>
        [Label("17:00-18:00")]
        public static readonly Property<decimal> OuputQty_17to18Property = P<DailyOutputReport>.Register(e => e.OuputQty_17to18);

        /// <summary>
        /// 17:00-18:00
        /// </summary>
        public decimal OuputQty_17to18
        {
            get { return this.GetProperty(OuputQty_17to18Property); }
            set { this.SetProperty(OuputQty_17to18Property, value); }
        }
        #endregion

        #region 18:00-19:00 OuputQty_18to19
        /// <summary>
        /// 18:00-19:00
        /// </summary>
        [Label("18:00-19:00")]
        public static readonly Property<decimal> OuputQty_18to19Property = P<DailyOutputReport>.Register(e => e.OuputQty_18to19);

        /// <summary>
        /// 18:00-19:00
        /// </summary>
        public decimal OuputQty_18to19
        {
            get { return this.GetProperty(OuputQty_18to19Property); }
            set { this.SetProperty(OuputQty_18to19Property, value); }
        }
        #endregion

        #region 19:00-20:00 OuputQty_19to20
        /// <summary>
        /// 19:00-20:00
        /// </summary>
        [Label("19:00-20:00")]
        public static readonly Property<decimal> OuputQty_19to20Property = P<DailyOutputReport>.Register(e => e.OuputQty_19to20);

        /// <summary>
        /// 19:00-20:00
        /// </summary>
        public decimal OuputQty_19to20
        {
            get { return this.GetProperty(OuputQty_19to20Property); }
            set { this.SetProperty(OuputQty_19to20Property, value); }
        }
        #endregion

        #endregion

        #region 夜班小时产出

        #region 20:00-22:00 OuputQty_20to22
        /// <summary>
        /// 20:00-22:00
        /// </summary>
        [Label("20:00-22:00")]
        public static readonly Property<decimal> OuputQty_20to22Property = P<DailyOutputReport>.Register(e => e.OuputQty_20to22);

        /// <summary>
        /// 20:00-22:00
        /// </summary>
        public decimal OuputQty_20to22
        {
            get { return this.GetProperty(OuputQty_20to22Property); }
            set { this.SetProperty(OuputQty_20to22Property, value); }
        }
        #endregion

        #region 22:00-0:00 OuputQty_22to00
        /// <summary>
        /// 22:00-0:00
        /// </summary>
        [Label("22:00-0:00")]
        public static readonly Property<decimal> OuputQty_22to00Property = P<DailyOutputReport>.Register(e => e.OuputQty_22to00);

        /// <summary>
        /// 22:00-0:00
        /// </summary>
        public decimal OuputQty_22to00
        {
            get { return this.GetProperty(OuputQty_22to00Property); }
            set { this.SetProperty(OuputQty_22to00Property, value); }
        }
        #endregion

        #region 0:00-3:00 OuputQty_00to03
        /// <summary>
        /// 0:00-3:00
        /// </summary>
        [Label("0:00-3:00")]
        public static readonly Property<decimal> OuputQty_00to03Property = P<DailyOutputReport>.Register(e => e.OuputQty_00to03);

        /// <summary>
        /// 0:00-3:00
        /// </summary>
        public decimal OuputQty_00to03
        {
            get { return this.GetProperty(OuputQty_00to03Property); }
            set { this.SetProperty(OuputQty_00to03Property, value); }
        }
        #endregion

        #region 3:00-5:00 OuputQty_03to05
        /// <summary>
        /// 3:00-5:00
        /// </summary>
        [Label("3:00-5:00")]
        public static readonly Property<decimal> OuputQty_03to05Property = P<DailyOutputReport>.Register(e => e.OuputQty_03to05);

        /// <summary>
        /// 3:00-5:00
        /// </summary>
        public decimal OuputQty_03to05
        {
            get { return this.GetProperty(OuputQty_03to05Property); }
            set { this.SetProperty(OuputQty_03to05Property, value); }
        }
        #endregion

        #region 5:00-8:00 OuputQty_05to08
        /// <summary>
        /// 5:00-8:00
        /// </summary>
        [Label("5:00-8:00")]
        public static readonly Property<decimal> OuputQty_05to08Property = P<DailyOutputReport>.Register(e => e.OuputQty_05to08);

        /// <summary>
        /// 5:00-8:00
        /// </summary>
        public decimal OuputQty_05to08
        {
            get { return this.GetProperty(OuputQty_05to08Property); }
            set { this.SetProperty(OuputQty_05to08Property, value); }
        }
        #endregion

        #region 20:00-21:00 OuputQty_20to21
        /// <summary>
        /// 20:00-21:00
        /// </summary>
        [Label("20:00-21:00")]
        public static readonly Property<decimal> OuputQty_20to21Property = P<DailyOutputReport>.Register(e => e.OuputQty_20to21);

        /// <summary>
        /// 20:00-21:00
        /// </summary>
        public decimal OuputQty_20to21
        {
            get { return this.GetProperty(OuputQty_20to21Property); }
            set { this.SetProperty(OuputQty_20to21Property, value); }
        }
        #endregion

        #region 21:00-22:00 OuputQty_21to22
        /// <summary>
        /// 20:00-21:00
        /// </summary>
        [Label("20:00-21:00")]
        public static readonly Property<decimal> OuputQty_21to22Property = P<DailyOutputReport>.Register(e => e.OuputQty_21to22);

        /// <summary>
        /// 20:00-21:00
        /// </summary>
        public decimal OuputQty_21to22
        {
            get { return this.GetProperty(OuputQty_21to22Property); }
            set { this.SetProperty(OuputQty_21to22Property, value); }
        }
        #endregion

        #region 22:00-23:00 OuputQty_22to23
        /// <summary>
        /// 22:00-23:00
        /// </summary>
        [Label("22:00-23:00")]
        public static readonly Property<decimal> OuputQty_22to23Property = P<DailyOutputReport>.Register(e => e.OuputQty_22to23);

        /// <summary>
        /// 22:00-23:00
        /// </summary>
        public decimal OuputQty_22to23
        {
            get { return this.GetProperty(OuputQty_22to23Property); }
            set { this.SetProperty(OuputQty_22to23Property, value); }
        }
        #endregion

        #region 23:00-00:00 OuputQty_23to00
        /// <summary>
        /// 23:00-00:00
        /// </summary>
        [Label("23:00-00:00")]
        public static readonly Property<decimal> OuputQty_23to00Property = P<DailyOutputReport>.Register(e => e.OuputQty_23to00);

        /// <summary>
        /// 23:00-00:00
        /// </summary>
        public decimal OuputQty_23to00
        {
            get { return this.GetProperty(OuputQty_23to00Property); }
            set { this.SetProperty(OuputQty_23to00Property, value); }
        }
        #endregion

        #region 00:00-1:00 OuputQty_00to01
        /// <summary>
        /// 00:00-1:00
        /// </summary>
        [Label("00:00-1:00")]
        public static readonly Property<decimal> OuputQty_00to01Property = P<DailyOutputReport>.Register(e => e.OuputQty_00to01);

        /// <summary>
        /// 00:00-1:00
        /// </summary>
        public decimal OuputQty_00to01
        {
            get { return this.GetProperty(OuputQty_00to01Property); }
            set { this.SetProperty(OuputQty_00to01Property, value); }
        }
        #endregion

        #region 1:00-2:00 OuputQty_01to02
        /// <summary>
        /// 1:00-2:00
        /// </summary>
        [Label("1:00-2:00")]
        public static readonly Property<decimal> OuputQty_01to02Property = P<DailyOutputReport>.Register(e => e.OuputQty_01to02);

        /// <summary>
        /// 1:00-2:00
        /// </summary>
        public decimal OuputQty_01to02
        {
            get { return this.GetProperty(OuputQty_01to02Property); }
            set { this.SetProperty(OuputQty_01to02Property, value); }
        }
        #endregion

        #region 2:00-3:00 OuputQty_02to03
        /// <summary>
        /// 2:00-3:00
        /// </summary>
        [Label("2:00-3:00")]
        public static readonly Property<decimal> OuputQty_02to03Property = P<DailyOutputReport>.Register(e => e.OuputQty_02to03);

        /// <summary>
        /// 2:00-3:00
        /// </summary>
        public decimal OuputQty_02to03
        {
            get { return this.GetProperty(OuputQty_02to03Property); }
            set { this.SetProperty(OuputQty_02to03Property, value); }
        }
        #endregion

        #region 3:00-4:00 OuputQty_03to04
        /// <summary>
        /// 3:00-4:00
        /// </summary>
        [Label("3:00-4:00")]
        public static readonly Property<decimal> OuputQty_03to04Property = P<DailyOutputReport>.Register(e => e.OuputQty_03to04);

        /// <summary>
        /// 3:00-4:00
        /// </summary>
        public decimal OuputQty_03to04
        {
            get { return this.GetProperty(OuputQty_03to04Property); }
            set { this.SetProperty(OuputQty_03to04Property, value); }
        }
        #endregion

        #region 4:00-5:00 OuputQty_04to05
        /// <summary>
        /// 4:00-5:00
        /// </summary>
        [Label("4:00-5:00")]
        public static readonly Property<decimal> OuputQty_04to05Property = P<DailyOutputReport>.Register(e => e.OuputQty_04to05);

        /// <summary>
        /// 4:00-5:00
        /// </summary>
        public decimal OuputQty_04to05
        {
            get { return this.GetProperty(OuputQty_04to05Property); }
            set { this.SetProperty(OuputQty_04to05Property, value); }
        }
        #endregion

        #region 05:00-6:00 OuputQty_05to06
        /// <summary>
        /// 05:00-6:00
        /// </summary>
        [Label("05:00-6:00")]
        public static readonly Property<decimal> OuputQty_05to06Property = P<DailyOutputReport>.Register(e => e.OuputQty_05to06);

        /// <summary>
        /// 05:00-6:00
        /// </summary>
        public decimal OuputQty_05to06
        {
            get { return this.GetProperty(OuputQty_05to06Property); }
            set { this.SetProperty(OuputQty_05to06Property, value); }
        }
        #endregion

        #region 6:00-7:00 OuputQty_06to07
        /// <summary>
        /// 6:00-7:00
        /// </summary>
        [Label("6:00-7:00")]
        public static readonly Property<decimal> OuputQty_06to07Property = P<DailyOutputReport>.Register(e => e.OuputQty_06to07);

        /// <summary>
        /// 6:00-7:00
        /// </summary>
        public decimal OuputQty_06to07
        {
            get { return this.GetProperty(OuputQty_06to07Property); }
            set { this.SetProperty(OuputQty_06to07Property, value); }
        }
        #endregion

        #region 7:00-8:00 OuputQty_07to08
        /// <summary>
        /// 7:00-8:00
        /// </summary>
        [Label("7:00-8:00")]
        public static readonly Property<decimal> OuputQty_07to08Property = P<DailyOutputReport>.Register(e => e.OuputQty_07to08);

        /// <summary>
        /// 7:00-8:00
        /// </summary>
        public decimal OuputQty_07to08
        {
            get { return this.GetProperty(OuputQty_07to08Property); }
            set { this.SetProperty(OuputQty_07to08Property, value); }
        }
        #endregion

        #endregion

        #region 未达成原因分类 ReasonCategory
        /// <summary>
        /// 未达成原因分类
        /// </summary>
        [Label("未达成原因分类")]
        public static readonly Property<string> ReasonCategoryProperty = P<DailyOutputReport>.Register(e => e.ReasonCategory);

        /// <summary>
        /// 未达成原因分类
        /// </summary>
        public string ReasonCategory
        {
            get { return this.GetProperty(ReasonCategoryProperty); }
            set { this.SetProperty(ReasonCategoryProperty, value); }
        }
        #endregion

        #region 未达成原因分析 ReasonAnalysis
        /// <summary>
        /// 未达成原因分析
        /// </summary>
        [Label("未达成原因分析")]
        public static readonly Property<string> ReasonAnalysisProperty = P<DailyOutputReport>.Register(e => e.ReasonAnalysis);

        /// <summary>
        /// 未达成原因分析
        /// </summary>
        public string ReasonAnalysis
        {
            get { return this.GetProperty(ReasonAnalysisProperty); }
            set { this.SetProperty(ReasonAnalysisProperty, value); }
        }
        #endregion

    }
}
