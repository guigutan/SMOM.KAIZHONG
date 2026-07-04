using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Statistics.Fpy
{
    /// <summary>
    /// 缺陷统计
    /// </summary>
    [RootEntity, Serializable]
    [Label("缺陷统计")]
    public partial class DefectStatistics : DataEntity
    {
        #region 产线ID ResourceId
        /// <summary>
        /// 产线ID
        /// </summary>
        [Label("产线ID")]
        public static readonly Property<double> ResourceIdProperty = P<DefectStatistics>.Register(e => e.ResourceId);

        /// <summary>
        /// 产线ID
        /// </summary>
        public double ResourceId
        {
            get { return GetProperty(ResourceIdProperty); }
            set { SetProperty(ResourceIdProperty, value); }
        }
        #endregion

        #region 产线名称 ResourceName
        /// <summary>
        /// 产线名称
        /// </summary>
        [Label("产线名称")]
        public static readonly Property<string> ResourceNameProperty = P<DefectStatistics>.Register(e => e.ResourceName);

        /// <summary>
        /// 产线名称
        /// </summary>
        public string ResourceName
        {
            get { return GetProperty(ResourceNameProperty); }
            set { SetProperty(ResourceNameProperty, value); }
        }
        #endregion

        #region 产品ID ProductId
        /// <summary>
        /// 产品ID
        /// </summary>
        [Label("产品ID")]
        public static readonly Property<double> ProductIdProperty = P<DefectStatistics>.Register(e => e.ProductId);

        /// <summary>
        /// 产品ID
        /// </summary>
        public double ProductId
        {
            get { return GetProperty(ProductIdProperty); }
            set { SetProperty(ProductIdProperty, value); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<DefectStatistics>.Register(e => e.ProductName);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return GetProperty(ProductNameProperty); }
            set { SetProperty(ProductNameProperty, value); }
        }
        #endregion

        #region 工序ID ProcessId
        /// <summary>
        /// 工序ID
        /// </summary>
        [Label("工序ID")]
        public static readonly Property<double> ProcessIdProperty = P<DefectStatistics>.Register(e => e.ProcessId);

        /// <summary>
        /// 工序ID
        /// </summary>
        public double ProcessId
        {
            get { return GetProperty(ProcessIdProperty); }
            set { SetProperty(ProcessIdProperty, value); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<DefectStatistics>.Register(e => e.ProcessName);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return GetProperty(ProcessNameProperty); }
            set { SetProperty(ProcessNameProperty, value); }
        }
        #endregion

        #region 班次ID ShiftId
        /// <summary>
        /// 班次ID
        /// </summary>
        [Label("班次ID")]
        public static readonly Property<double> ShiftIdProperty = P<DefectStatistics>.Register(e => e.ShiftId);

        /// <summary>
        /// 班次ID
        /// </summary>
        public double ShiftId
        {
            get { return GetProperty(ShiftIdProperty); }
            set { SetProperty(ShiftIdProperty, value); }
        }
        #endregion

        #region 班次名称 ShiftName
        /// <summary>
        /// 班次名称
        /// </summary>
        [Label("班次名称")]
        public static readonly Property<string> ShiftNameProperty = P<DefectStatistics>.Register(e => e.ShiftName);

        /// <summary>
        /// 班次名称
        /// </summary>
        public string ShiftName
        {
            get { return GetProperty(ShiftNameProperty); }
            set { SetProperty(ShiftNameProperty, value); }
        }
        #endregion

        #region 缺陷代码分类ID CategoryId
        /// <summary>
        /// 缺陷代码分类ID
        /// </summary>
        [Label("缺陷代码分类ID")]
        public static readonly Property<double> CategoryIdProperty = P<DefectStatistics>.Register(e => e.CategoryId);

        /// <summary>
        /// 缺陷代码分类ID
        /// </summary>
        public double CategoryId
        {
            get { return GetProperty(CategoryIdProperty); }
            set { SetProperty(CategoryIdProperty, value); }
        }
        #endregion

        #region 缺陷代码分类名称 CategoryName
        /// <summary>
        /// 缺陷代码分类名称
        /// </summary>
        [Label("缺陷代码分类名称")]
        public static readonly Property<string> CategoryNameProperty = P<DefectStatistics>.Register(e => e.CategoryName);

        /// <summary>
        /// 缺陷代码分类名称
        /// </summary>
        public string CategoryName
        {
            get { return GetProperty(CategoryNameProperty); }
            set { SetProperty(CategoryNameProperty, value); }
        }
        #endregion

        #region 缺陷代码ID DefectId
        /// <summary>
        /// 缺陷代码ID
        /// </summary>
        [Label("缺陷代码ID")]
        public static readonly Property<double> DefectIdProperty = P<DefectStatistics>.Register(e => e.DefectId);

        /// <summary>
        /// 缺陷代码ID
        /// </summary>
        public double DefectId
        {
            get { return GetProperty(DefectIdProperty); }
            set { SetProperty(DefectIdProperty, value); }
        }
        #endregion

        #region 缺陷代码名称 DefectName
        /// <summary>
        /// 缺陷代码名称
        /// </summary>
        [Label("缺陷代码名称")]
        public static readonly Property<string> DefectNameProperty = P<DefectStatistics>.Register(e => e.DefectName);

        /// <summary>
        /// 缺陷代码名称
        /// </summary>
        public string DefectName
        {
            get { return GetProperty(DefectNameProperty); }
            set { SetProperty(DefectNameProperty, value); }
        }
        #endregion

        #region 采集日期 CollectedDate
        /// <summary>
        /// 采集日期
        /// </summary>
        [Label("采集日期")]
        public static readonly Property<DateTime> CollectedDateProperty = P<DefectStatistics>.Register(e => e.CollectedDate);

        /// <summary>
        /// 采集日期
        /// </summary>
        public DateTime CollectedDate
        {
            get { return GetProperty(CollectedDateProperty); }
            set { SetProperty(CollectedDateProperty, value); }
        }
        #endregion

        #region 班次日期 ShiftDate
        /// <summary>
        /// 班次日期
        /// </summary>
        [Label("班次日期")]
        public static readonly Property<DateTime> ShiftDateProperty = P<DefectStatistics>.Register(e => e.ShiftDate);

        /// <summary>
        /// 班次日期
        /// </summary>
        public DateTime ShiftDate
        {
            get { return GetProperty(ShiftDateProperty); }
            set { SetProperty(ShiftDateProperty, value); }
        }
        #endregion

        #region 缺陷数量 Qty
        /// <summary>
        /// 缺陷数量
        /// </summary>
        [Label("缺陷数量")]
        public static readonly Property<decimal> QtyProperty = P<DefectStatistics>.Register(e => e.Qty);

        /// <summary>
        /// 缺陷数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 缺陷统计 实体配置
    /// </summary>
    internal class DefectStatisticsConfig : EntityConfig<DefectStatistics>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_STATS_DEFECT").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.IndexGroupOnProperties(DefectStatistics.ResourceIdProperty, DefectStatistics.ProductIdProperty, DefectStatistics.ProcessIdProperty, DefectStatistics.DefectIdProperty);
        }
    }
}