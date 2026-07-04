using SIE.Domain;
using SIE.LES.StockPlans.Service;
using SIE.ObjectModel;
using SIE.ShipPlan;
using System;

namespace SIE.LES.StockPlans
{
    /// <summary>
    /// 备料计划查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("备料计划查询实体")]
    public class StockPlanCriteria: DeliveryPlanCriteria
    {
        #region 来源 SourceType
        /// <summary>
        /// 来源
        /// </summary>
        [Label("来源")]
        public static readonly Property<DeliverySourceType?> SourceTypeProperty = P<StockPlanCriteria>.Register(e => e.SourceType);

        /// <summary>
        /// 来源
        /// </summary>
        public DeliverySourceType? SourceType
        {
            get { return GetProperty(SourceTypeProperty); }
            set { SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询逻辑
        /// </summary>
        /// <returns>返回查询后的数据</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<StockPlanService>().GetStockPlans(this);
        }
    }
}
