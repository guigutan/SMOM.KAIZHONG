using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Workbench.KeyPerformances
{
    /// <summary>
    /// OEE效率
    /// </summary>
    [RootEntity, Serializable, CriteriaQuery, Label("OEE效率")]
    public class OEEfficiency : DataEntity
    {
        #region 车间Id ShopId
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间Id")]
        public static readonly Property<double> WorkShopIdProperty = P<OEEfficiency>.Register(e => e.WorkShopId);

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
        public static readonly Property<string> ShopNameProperty = P<OEEfficiency>.Register(e => e.ShopName);

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
        public static readonly Property<double> LineIdProperty = P<OEEfficiency>.Register(e => e.LineId);

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
        public static readonly Property<string> LineNameProperty = P<OEEfficiency>.Register(e => e.LineName);

        /// <summary>
        /// 产线名称
        /// </summary>
        public string LineName
        {
            get { return this.GetProperty(LineNameProperty); }
            set { this.SetProperty(LineNameProperty, value); }
        }
        #endregion


        #region 效率 Efficiency
        /// <summary>
        /// 效率
        /// </summary>
        [Label("效率")]
        public static readonly Property<double> EfficiencyProperty = P<OEEfficiency>.Register(e => e.Efficiency);

        /// <summary>
        /// 效率
        /// </summary>
        public double Efficiency
        {
            get { return this.GetProperty(EfficiencyProperty); }
            set { this.SetProperty(EfficiencyProperty, value); }
        }
        #endregion

        #region 统计日期 Date
        /// <summary>
        /// 统计日期
        /// </summary>
        [Label("统计日期")]
        public static readonly Property<DateTime> DateProperty = P<OEEfficiency>.Register(e => e.Date);

        /// <summary>
        /// 统计日期
        /// </summary>
        public DateTime Date
        {
            get { return this.GetProperty(DateProperty); }
            set { this.SetProperty(DateProperty, value); }
        }
        #endregion
    }

    internal class OEEfficiencyConfig : EntityConfig<OEEfficiency>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("OEE_EFF").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
