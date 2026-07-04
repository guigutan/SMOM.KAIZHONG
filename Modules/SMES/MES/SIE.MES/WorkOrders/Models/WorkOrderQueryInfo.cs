using SIE.Core.ApiModels;
using System;
using System.Collections.Generic;

namespace SIE.MES.WorkOrders.Models
{
    /// <summary>
    /// 工单查询信息
    /// </summary>
    [Serializable]
    public class WorkOrderQueryInfo : PagingKeywordQueryInfo
    {
        /// <summary>
        /// 产线ID
        /// </summary>
        public double ResourceId { get; set; }
        /// <summary>
        /// 工单状态
        /// </summary>
        public List<int> StateList { get; set; }
    }


    /// <summary>
    /// 工单基本信息
    /// </summary>
    [Serializable]
    public class WorkOrderBaseData
    {
        /// <summary>
        /// Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId { get; set; }

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanQty { get; set; }
    }
}