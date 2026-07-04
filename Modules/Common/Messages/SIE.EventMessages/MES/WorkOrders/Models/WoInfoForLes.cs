using System;
using System.Collections.Generic;

namespace SIE.EventMessages.MES.WorkOrders.Models
{
    /// <summary>
    /// 工单信息（提供给LES备料计算）
    /// </summary>
    [Serializable]
    public class WoInfoForLes
    {
        /// <summary>
        /// 工厂ID
        /// </summary>
        public double? FactoryId { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName{ get; set; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanBeginDate { get; set; }

        /// <summary>
        /// 车间ID
        /// </summary>
        public double? WorkShopId { get; set; }

        /// <summary>
        /// 生产资源Id
        /// </summary>
        public double? ResourceId { get; set; }

        private List<WoBomInfoForLes> woBomInfos;

        /// <summary>
        /// 工单BOM信息
        /// </summary>
        public List<WoBomInfoForLes> WoBomInfos
        {
            get
            {
                if (woBomInfos == null)
                {
                    woBomInfos = new List<WoBomInfoForLes>();
                }

                return woBomInfos;
            }
            set { woBomInfos = value; }
        }
    }
}
