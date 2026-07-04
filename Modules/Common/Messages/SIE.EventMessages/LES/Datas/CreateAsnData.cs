using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.LES
{
    /// <summary>
    /// 创建退料ASN单
    /// </summary>
    [Serializable]
    public class CreateAsnData
    {
        /// <summary>
        /// 库存组织Id
        /// </summary>
        public int InvOrgId { get; set; }

        /// <summary>
        /// 生产部门Id
        /// </summary>
        public double EnterpriseId { get; set; }

        /// <summary>
        /// 单号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 是否原批次退
        /// </summary>
        public bool IsReturnLot { get; set; } = true;

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotCode { get; set; }

        /// <summary>
        /// 相关单号,工单号,可空
        /// </summary>
        public string OrderNo { get; set; }


        /// <summary>
        /// 相关单行号，工单bom行号,可空
        /// </summary>
        public string OrderLineNo { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 退料数(辅)
        /// </summary>
        public decimal SecondQty { get; set; }

        /// <summary>
        /// 辅助单位Id
        /// </summary>
        public double SecondUnitId { get; set; }

        /// <summary>
        /// 序列号
        /// </summary>
        public List<CreateAsnDataSn> CreateAsnDataSns { get; set; } = new List<CreateAsnDataSn>();
    }

    /// <summary>
    /// 序列号
    /// </summary>
    [Serializable]
    public class CreateAsnDataSn
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SN { get; set; }

        /// <summary>
        /// 退料数(辅)
        /// </summary>
        public decimal SecondQty { get; set; }
    }
}
