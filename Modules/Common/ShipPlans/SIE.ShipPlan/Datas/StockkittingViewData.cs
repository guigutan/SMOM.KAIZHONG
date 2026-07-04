using SIE.Domain;
using SIE.ShipPlan.ViewModels;
using System;
using System.Collections.Generic;

namespace SIE.ShipPlan.Datas
{
    /// <summary>
    /// 备料计划/发货计划返回数据
    /// </summary>
    [Serializable]
    public class StockkittingViewData
    {
        /// <summary>
        /// 备料计划/发货计划主表数据
        /// </summary>
        public EntityList<DeliveryPlan> stockPlans { get; set; } = new EntityList<DeliveryPlan>();

        /// <summary>
        /// 备料计划/发货计划需求数据
        /// </summary>
        public EntityList<StockPlanAssignViewModel> stockPlanAssigns { get; set; } = new EntityList<StockPlanAssignViewModel>();
    }

    /// <summary>
    /// 齐套数据
    /// </summary>
    public class KittingData
    {
        /// <summary>
        /// 发运订单
        /// </summary>
        public List<double> stockIds { get; set; }

        /// <summary>
        /// 考虑采购
        /// </summary>
        public bool BuyOnLoad { get; set; }

        /// <summary>
        /// 考虑在制
        /// </summary>
        public bool MakeOnLoad { get; set; }

    }
}
