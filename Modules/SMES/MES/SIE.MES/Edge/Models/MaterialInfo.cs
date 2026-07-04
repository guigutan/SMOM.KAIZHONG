using SIE.MES.LoadItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Edge.Models
{
    /// <summary>
    /// 物料信息
    /// </summary>
    [Serializable]
    public class MaterialInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MaterialInfo()
        {
            PropertyValueList = new List<PropertyValue>();
        }

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 配送单号（配送上料物料对应的条码）
        /// 来源类型为配送时有值
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 条码来源类型
        /// </summary>
        public SingleLabels.LoadItemSourceType Type { get; set; }

        /// <summary>
        /// 条码来源Id
        /// </summary>
        public double SourceId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal RemainQty { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        public List<PropertyValue> PropertyValueList { get; set; }


        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ItemExtProp
        {

            get;
            set;

        }
        /// <summary>
        /// 扩展属性值
        /// </summary>
        public string ItemExtPropName
        {
            get;
            set;
        }
    }
}
