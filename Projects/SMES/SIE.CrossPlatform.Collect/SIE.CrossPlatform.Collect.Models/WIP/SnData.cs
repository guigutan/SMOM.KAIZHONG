using SIE.CrossPlatform.Collect.Models.Attributes;
using SIE.CrossPlatform.Collect.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.CrossPlatform.Collect.Models.WIP
{
    /// <summary>
    /// 拼板绑定条码数据
    /// </summary>
    [Serializable]
    public class SnData
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SnData()
        {
            //默认条码类型为SN(产品条码)
            BarcodeType = BarcodeType.SN;
        }

        /// <summary>
        /// 待绑定
        /// </summary>
        public bool ToBinding { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 子产品工单
        /// </summary>
        public double? ChildWorkOrderId { get; set; }

        /// <summary>
        /// 子产品物料ID
        /// </summary>
        public double ChildProductId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProducName { get; set; }

        /// <summary>
        /// 拼板码
        /// </summary>
        public string PanelCode { get; set; }

        /// <summary>
        /// 条码类型
        /// </summary>
        public BarcodeType BarcodeType { get; set; }

        /// <summary>
        /// 板号
        /// </summary>
        public int BoardNo { get; set; }
    }
    /// <summary>
    /// SN绑定方式
    /// </summary>
    public enum BindingMode
    {
        /// <summary>
        /// 自动
        /// </summary>
        [Label("自动")]
        Auto,

        /// <summary>
        /// 手动
        /// </summary>
        [Label("手动")]
        Manual
    }
}
