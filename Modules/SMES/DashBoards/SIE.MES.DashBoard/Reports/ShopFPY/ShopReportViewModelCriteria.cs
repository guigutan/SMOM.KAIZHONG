using SIE.Domain;
using SIE.MES.DashBoard.TeamManagement;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;

namespace SIE.MES.DashBoard.Reports.ShopFPY
{
    /// <summary>
    /// 车间直通率报表查询实体
    /// </summary>
    [QueryEntity, Serializable]
    public class ShopReportViewModelCriteria : Criteria
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ShopReportViewModelCriteria()
        {
            CollectDate = new DateRange();
        }

        #region 车间 Shop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty ShopIdProperty =
            P<ShopReportViewModelCriteria>.RegisterRefId(e => e.ShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? ShopId
        {
            get { return (double?)this.GetRefNullableId(ShopIdProperty); }
            set { this.SetRefNullableId(ShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> ShopProperty =
            P<ShopReportViewModelCriteria>.RegisterRef(e => e.Shop, ShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise Shop
        {
            get { return this.GetRefEntity(ShopProperty); }
            set { this.SetRefEntity(ShopProperty, value); }
        }
        #endregion

        #region 日期 CollectDate
        /// <summary>
        /// 日期
        /// </summary>
        [Label("日期")]
        public static readonly Property<DateRange> CollectDateProperty = P<ShopReportViewModelCriteria>.Register(e => e.CollectDate);

        /// <summary>
        /// 日期
        /// </summary>
        public DateRange CollectDate
        {
            get { return this.GetProperty(CollectDateProperty); }
            set { this.SetProperty(CollectDateProperty, value); }
        }
        #endregion

        #region 类型 DateType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        [Required]
        public static readonly Property<DateType> DateTypeProperty = P<ShopReportViewModelCriteria>.Register(e => e.DateType);

        /// <summary>
        /// 类型
        /// </summary>
        public DateType DateType
        {
            get { return this.GetProperty(DateTypeProperty); }
            set { this.SetProperty(DateTypeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            var modelList = new EntityList<ShopReportViewModel>();
            EntityList<Statistics.Fpy.ProcessFpyStatistics> shopFpyStatistics;
            System.Collections.Generic.Dictionary<double, string> resNameDics;
            System.Collections.Generic.Dictionary<double, Enterprise> shopDics;
            modelList.Add(RT.Service.Resolve<ShopReportViewModelController>().GetShopReportViewModel(this, out shopFpyStatistics, out resNameDics, out shopDics));
            return modelList;
        }
    }
}
