using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Workbench.KeyPerformances.PlanReachedPercentages
{
    /// <summary>
    /// 车间计划达成率数据视图
    /// </summary>
    public class LinePlanReachedViewModel : ViewModel
    {
        #region 车间Id ShopId
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间Id")]
        public static readonly Property<double> WorkShopIdProperty = P<LinePlanReachedViewModel>.Register(e => e.WorkShopId);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double WorkShopId
        {
            get { return this.GetProperty(WorkShopIdProperty); }
            set { this.SetProperty(WorkShopIdProperty, value); }
        }
        #endregion

        #region 车间名称 ShopName
        /// <summary>
        /// 车间名称
        /// </summary>
        [Label("车间名称")]
        public static readonly Property<string> ShopNameProperty = P<LinePlanReachedViewModel>.Register(e => e.ShopName);

        /// <summary>
        /// 车间名称
        /// </summary>
        public string ShopName
        {
            get { return this.GetProperty(ShopNameProperty); }
            set { this.SetProperty(ShopNameProperty, value); }
        }
        #endregion

        #region 产线Id LineId
        /// <summary>
        /// 产线Id
        /// </summary>
        [Label("产线Id")]
        public static readonly Property<double> LineIdProperty = P<LinePlanReachedViewModel>.Register(e => e.LineId);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double LineId
        {
            get { return this.GetProperty(LineIdProperty); }
            set { this.SetProperty(LineIdProperty, value); }
        }
        #endregion

        #region 产线名称 LineName
        /// <summary>
        /// 产线名称
        /// </summary>
        [Label("产线名称")]
        public static readonly Property<string> LineNameProperty = P<LinePlanReachedViewModel>.Register(e => e.LineName);

        /// <summary>
        /// 产线名称
        /// </summary>
        public string LineName
        {
            get { return this.GetProperty(LineNameProperty); }
            set { this.SetProperty(LineNameProperty, value); }
        }
        #endregion

        #region 产线生产计划数量 PlanQty
        /// <summary>
        /// 产线生产计划数量
        /// </summary>
        [Label("计划数量")]
        public static readonly Property<decimal> PlanQtyProperty = P<LinePlanReachedViewModel>.Register(e => e.PlanQty);

        /// <summary>
        /// 产线生产计划数量
        /// </summary>
        public decimal PlanQty
        {
            get { return this.GetProperty(PlanQtyProperty); }
            set { this.SetProperty(PlanQtyProperty, value); }
        }
        #endregion

        #region 产线生产实际数量 ActualQty
        /// <summary>
        /// 产线生产实际数量
        /// </summary>
        [Label("实际数量")]
        public static readonly Property<decimal> ActualQtyProperty = P<LinePlanReachedViewModel>.Register(e => e.ActualQty);

        /// <summary>
        /// 产线生产实际数量
        /// </summary>
        public decimal ActualQty
        {
            get { return this.GetProperty(ActualQtyProperty); }
            set { this.SetProperty(ActualQtyProperty, value); }
        }
        #endregion

        #region 生产日期 ProductDate
        /// <summary>
        /// 生产日期
        /// </summary>
        [Label("生产日期")]
        public static readonly Property<DateTime> ProductDateProperty = P<LinePlanReachedViewModel>.Register(e => e.Date);

        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime Date
        {
            get { return this.GetProperty(ProductDateProperty); }
            set { this.SetProperty(ProductDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 达成率
        /// </summary>
        public double Percentage
        {
            get { return (double)(PlanQty > 0 ? ActualQty / PlanQty : 0); }
        }
    }
}
