using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.XPCJ.Models.WIP
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
    }
}
