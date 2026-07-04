using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Common.Datas.SmomOrder
{
    /// <summary>
    /// Ebs工单信息
    /// </summary>
    [Serializable]
    public class EbsWorkOrderData : EbsOrderBaseData
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        public double? CustomerId { get; set; }

        /// <summary>
        /// 客户编码
        /// </summary>
        public string CustomerCode { get; set; }

        /// <summary>
        /// 客户订单号
        /// </summary>
        public string CustomerOrderNo { get; set; }

        /// <summary>
        /// 销售订单号
        /// </summary>
        public string SaleOrderNo { get; set; }

        /// <summary>
        /// 工单类型 0量产、1试产、2返工、3校验
        /// </summary>
        public int WorkOrderType { get; set; }

        /// <summary>
        /// 工厂编码
        /// </summary>
        public string FactoryCode { get; set; }

        /// <summary>
        /// 车间编码
        /// </summary>
        public string WorkshopCode { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// Erp工单状态（1未释放 3已释放 4完成 7已取消 12已关闭）
        /// </summary>
        public int ErpState { get; set; }

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanQty { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        public decimal OrderQty { get; set; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanBeginDate { get; set; }

        /// <summary>
        /// 计划完成时间
        /// </summary>
        public DateTime PlanEndDate { get; set; }


        /// <summary>
        /// 工单BOM列表
        /// </summary>
        public List<EbsWorkOrderBomData> BomList { get; } = new List<EbsWorkOrderBomData>();
    }
}
