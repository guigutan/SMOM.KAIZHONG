using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Kit.APS.EngineerPlans.HelpClass
{
    /// <summary>
    /// 工程计划(同步使用)
    /// </summary>
    [Serializable]
    public class EngineerPlanInfo
    {
        /// <summary>
        /// 工程计划ID
        /// </summary>
        public double Id { get; set; }
        /// <summary>
        /// 销售订单明细ID
        /// </summary>
        public double OrderDetailId { get; set; }
        /// <summary>
        /// 工厂
        /// </summary>
        public double FactoryId { get; set; }

        /// <summary>
        /// 产品类型
        /// </summary>
        public string ProductType { get; set; }

        /// <summary>
        /// 销售订单明细行号
        /// </summary>
        public string LineNo { get; set; }

        public string ItemRevision { get; set; }

        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 物料
        /// </summary>
        public double ItemId { get; set; }
        /// <summary>
        /// 客户
        /// </summary>
        public double CustomerId { get; set; }
        /// <summary>
        /// 是否新单
        /// </summary>
        public bool IsNew { get; set; }
        /// <summary>
        /// 外部ECN
        /// </summary>
        public bool ExternalEcn { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public double UnitId { get; set; }
        /// <summary>
        /// 总面积M2
        /// </summary>
        public decimal Area { get; set; }
        /// <summary>
        /// 下单日期
        /// </summary>
        public DateTime? CustomerPoDate { get; set; }

        /// <summary>
        /// 登记日期
        /// </summary>
        public DateTime RegisterDateTime { get; set; }

        /// <summary>
        /// 客户交期
        /// </summary>
        public DateTime RequireDelivery { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public string OrderType { get; set; }
        /// <summary>
        /// 快板类型
        /// </summary>
        public string AllegroType { get; set; }
        /// <summary>
        /// 应用领域
        /// </summary>
        public string AppArea { get; set; }
    }
}
