using SIE.Domain;
using SIE.MES.SingleLabels;
using SIE.MES.WIP.Products;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ForWinform.ApiModels
{
    /// <summary>
    /// 产品生产关键件
    /// </summary>
    [Serializable]
    public class XPWipProductProcessKeyItem
    {
        /// <summary>
        /// 
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 用料数
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 工序过站记录Id
        /// </summary>
        public double ProcessId { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 来源类型
        /// </summary>
        public LoadItemSourceType SourceType { get; set; }

        /// <summary>
        /// 来源条码
        /// </summary>
        public string SourceCode { get; set; }

        /// <summary>
        /// 来源ID
        /// </summary>
        public double SourceId { get; set; }

        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 扩展属性值
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 单位用料数
        /// </summary>
        public decimal UnitQty { get; set; }

        /// <summary>
        /// 是否解绑
        /// </summary>
        public bool IsUnbound { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// 工位
        /// </summary>
        public string StationName { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 物料描述
        /// </summary>
        public string ItemDescription { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string ItemUnitName { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public double ResourceId { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 采集条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public XPWipProductProcessKeyItem()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wipProductProcessKeyItem"></param>
        public XPWipProductProcessKeyItem(WipProductProcessKeyItem wipProductProcessKeyItem)
        {
            this.Id = wipProductProcessKeyItem.Id;
            this.Qty = wipProductProcessKeyItem.Qty;
            this.ProcessId = wipProductProcessKeyItem.ProcessId;
            this.ItemId = wipProductProcessKeyItem.ItemId;
            this.SourceType = wipProductProcessKeyItem.SourceType;
            this.SourceCode = wipProductProcessKeyItem.SourceCode;
            this.SourceId = wipProductProcessKeyItem.SourceId;
            this.ItemExtProp = wipProductProcessKeyItem.ItemExtProp;
            this.ItemExtPropName = wipProductProcessKeyItem.ItemExtPropName;
            this.UnitQty = wipProductProcessKeyItem.UnitQty;
            this.IsUnbound = wipProductProcessKeyItem.IsUnbound;
            this.ProcessName = wipProductProcessKeyItem.ProcessName;
            this.StationName = wipProductProcessKeyItem.StationName;
            this.ItemCode = wipProductProcessKeyItem.ItemCode;
            this.ItemName = wipProductProcessKeyItem.ItemName;
            this.ItemDescription = wipProductProcessKeyItem.ItemDescription;
            this.ItemUnitName = wipProductProcessKeyItem.ItemUnitName;
            this.ResourceId = wipProductProcessKeyItem.ResourceId;
            this.ResourceName = wipProductProcessKeyItem.ResourceName;
            this.Barcode = wipProductProcessKeyItem.Barcode;
        }
    }
}
