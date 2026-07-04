using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.PrepareProducts.ApiModels
{
    [Serializable]
    public class PrepareProductsFilter
    {
        /// <summary>
        /// 计划开始时间
        /// </summary>
        public int? PlanBeginTime { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo { get; set; }

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double? FactoryId { get; set; }

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkShopId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public double? ResourceId { get; set; }

        /// <summary>
        /// 产前准备状态
        /// </summary>
        public int? State { get; set; }

        /// <summary>
        /// 工单状态
        /// </summary>
        public int? WoState { get; set; }
    }
}
