using SIE.XPCJ.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.Models.WIP.Entity
{
    [Serializable]
   public  class Barcode
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        public double Id
        {
            get; set;
        }
        /// <summary>
        /// 条码号
        /// </summary>
        public string Sn
        {
            get;set;
        }

        /// <summary>
        /// 是否报废
        /// </summary>
        public bool IsScraped
        {
            get; set;
        }
        
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get; set;
        }
        /// <summary>
        /// 满箱数量
        /// </summary>
        public decimal BoxesQty
        {
            get; set;
        }
       
        /// <summary>
        /// 是否尾数
        /// </summary>
        public bool IsMantissa
        {
            get; set;
        }
        /// <summary>
        /// 工单Id
        /// </summary>
        public  double? WorkOrderId { get; set; }

        /// <summary>
        /// 是否挂起
        /// </summary>
        public bool IsPending
        {
            get; set;
        }

        
        /// <summary>
        /// 打印日期
        /// </summary>
        public DateTime? PrintDate
        {
            get; set;
        }
        /// <summary>
        /// 打印次数
        /// </summary>
        public int PrintTimes
        {
            get; set;
        }

        /// <summary>
        /// 打印状态
        /// </summary>
        public BarcodeState PrintedState
        {
            get; set;
        }
       
        /// <summary>
        ///  条码范围Id
        /// </summary>
        public double? RangeId
        {
            get; set;
        }


        /// <summary>
        /// 打印员Id
        /// </summary>
        public double? PrintById
        {
            get; set;
        }

        /// <summary>
        /// 开始条码
        /// </summary>
        public string StartSn
        {
            get; set;
        }
        
        /// <summary>
        /// 结束条码
        /// </summary>
        public string EndSn
        {
            get; set;
        }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WONo
        {
            get; set;
        }
        /// <summary>
        /// 打印人
        /// </summary>
        public string PrinterName
        {
            get; set;
        }
        
        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get; set;
        }
        
        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductName
        {
            get; set;
        }
    }

    public enum BarcodeState
    {
        /// <summary>
        /// 未打印
        /// </summary>
        [Label("未打印")]
        Notprint = 0,

        /// <summary>
        /// 已打印
        /// </summary>
        [Label("已打印")]
        Printed = 1,
    }
}
