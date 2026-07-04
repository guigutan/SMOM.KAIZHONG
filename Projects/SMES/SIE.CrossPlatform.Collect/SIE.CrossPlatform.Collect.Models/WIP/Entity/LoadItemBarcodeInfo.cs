using SIE.CrossPlatform.Collect.Models.WIP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.WIP.Entity
{
    [Serializable]
    public class LoadItemBarcodeInfo
    {
        /// <summary>
        /// ID编码
        /// </summary>
        public double Id
        {
            get; set;
        }

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode
        {
            get; set;
        }
        /// <summary>
        /// 配送单号
        /// </summary>
        public string BillNo
        {
            get; set;
        }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get; set;
        }

        /// <summary>
        /// 标签
        /// </summary>
        public string Label
        {
            get; set;
        }

        /// <summary>
        /// 条码来源类型
        /// </summary>
        public LoadItemSourceType Type
        {
            get; set;
        }
        /// <summary>
        /// 条码来源Id
        /// </summary>
        public double SourceId
        {
            get; set;
        }

        /// <summary>
        /// 可用数量
        /// </summary>
        public decimal Qty
        {
            get; set;
        }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get; set;
        }
        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get; set;
        }


        /// <summary>
        /// 属性id
        /// </summary>
        public string ItemExtProp
        {
            get; set;
        }

        /// <summary>
        /// 属性名称
        /// </summary>
        public string ItemExtPropName
        {
            get; set;
        }

        /// <summary>
        /// 批次
        /// </summary>
        public string LotNo
        {
            get; set;
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public string WarehouseName
        {
            get; set;
        }

        /// <summary>
        /// 库位
        /// </summary>
        public string StorageLocationCode
        {
            get; set;
        }

        /// <summary>
        /// 当前在制工单
        /// </summary>
        public double WipWorkOrderId
        {
            get; set;
        }

        /// <summary>
        /// 在制工单号
        /// </summary>
        public string WipWorkOrderNo
        {
            get; set;
        }

        /// <summary>
        /// 是否序列号管理
        /// </summary>
        public bool? IsSerialNumber
        {
            get; set;
        }

        /// <summary>
        /// 物料消耗类型
        /// </summary>
        public ConsumeMode ConsumeMode
        {
            get; set;
        }
    }
}
