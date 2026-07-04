using System;

namespace SIE.EventMessages.LES
{
    /// <summary>
    /// 创建发运单数据
    /// </summary>
    [Serializable]
    public class CreateShippingOrderData
    {
        /// <summary>
        /// 库存组织
        /// </summary>
        public int InvOrgId { get; set; }

        /// <summary>
        /// 来源(0-工单  1备料单需求)
        /// </summary>
        public int SourceType { get; set; }

        /// <summary>
        /// 1：工单发料；2：工单超发；3：车间备料；
        /// </summary>
        public int TransactionType { get; set; }

        /// <summary>
        /// 来源主键(工单BOM行的主键或备料需求单的主键)
        /// </summary>
        public string SourceKey { get; set; }

        /// <summary>
        /// 生产部门
        /// </summary>
        public string EnterpriseCode { get; set; }

        /// <summary>
        /// 是否工单超发
        /// </summary>
        public bool IsWoShipMore { get; set; }

        /// <summary>
        /// 货主编码
        /// </summary>
        public string StorerCode { get; set; } = "*";

        /// <summary>
        /// 备料单需求单号，写到So的来源单号，更新的时候会根据这个找对应的发运单
        /// </summary>
        public string RequireNo { get; set; }

        /// <summary>
        /// 备料单需求单行号，写到So明细的行号，更新的时候根据这个找对应行
        /// </summary>
        public string RequireLineNo { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 工单BOM行号（对接ERP使用)
        /// </summary>
        public string OrderLineNo { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 订货数(辅)
        /// </summary>
        public decimal SecondExceptQty { get; set; }

        /// <summary>
        /// 领料单位&辅助单位
        /// </summary>
        public string SecondUnitName { get; set; }

        /// <summary>
        /// 物料扩展属性值，正确格式如[颜色:红色;高度:1米]
        /// </summary>
        public string ItemExtPropName { get; set; }
       
        /// <summary>
        /// 交货日期
        /// </summary>
        public DateTime DeliveryDate { get; set; }

        /// <summary>
        /// 仓库（计算栏位，接口不需要赋值）
        /// </summary>
        public double WhId { get; set; }    
        
        /// <summary>
        /// 企业模型（计算栏位，接口不需要赋值）
        /// </summary>
        public double EnterpriseId { get; set; }

        /// <summary>
        /// 指定任务号
        /// </summary>
        public string AppointTaskNo { get; set; }

        /// <summary>
        /// 指定项目号
        /// </summary>
        public string AppointProjectNo { get; set; }

        /// <summary>
        /// 发货仓库编码
        /// </summary>
        public string ShippingWarehouseCode { get; set; }
    }

    /// <summary>
    /// 修改发运单明细数据
    /// </summary>
    [Serializable]
    public class UpdateShippingOrderData
    {
        /// <summary>
        /// 库存组织
        /// </summary>
        public int InvOrgId { get; set; }

        /// <summary>
        /// 发运订单号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 备料单需求单行号
        /// </summary>
        public string LineNo { get; set; }

        /// <summary>
        /// 订货数(辅)
        /// </summary>
        public decimal SecondExceptQty { get; set; }
    }


}
