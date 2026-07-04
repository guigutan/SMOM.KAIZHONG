using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EventMessages.WMS.Inventory
{
    /// <summary>
    /// 提交数据
    /// </summary>
    public class MesReturnItemData
    {
        /// <summary>
        /// 操作员工编号
        /// </summary>
        public string EmpCode { get; set; }

        /// <summary>
        /// 来源单号
        /// </summary>
        public string SourceBillNo { get; set; }

        /// <summary>
        /// 条码集合
        /// </summary>
        public List<ReturnLabelData> returnLabelList { get; set; } = new List<ReturnLabelData>();
    }

    /// <summary>
    /// 标签条码
    /// </summary>
    public class ReturnLabelData
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 库位
        /// </summary>
        public double? StorageLocationId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 生产部门ID
        /// </summary>
        public double? EnterprisesId { get; set; }

        /// <summary>
        /// 生产部门Code
        /// </summary>
        public string EnterprisesCode { get; set; }

        /// <summary>
        /// 备料单号
        /// </summary>
        public string StockOrderNo { get; set; }

        /// <summary>
        /// 备料单号
        /// </summary>
        public double StockOrderId { get; set; }

        /// <summary>
        /// 是否合格 true-不合格 false-合格
        /// </summary>
        public bool IsFail { get; set; }

        /// <summary>
        /// 备料需求明细ID
        /// </summary>
        public double DetailId { get; set; }

        /// <summary>
        /// 明了明细行号
        /// </summary>
        public string DetailLineNo { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public double WoId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo { get; set; }

        /// <summary>
        /// 是否序列号物料
        /// </summary>
        public bool IsSerialNumber { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotCode { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double? WarehouseId { get; set; }
    }
}
