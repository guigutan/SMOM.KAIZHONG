using SIE.XPCJ.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.XPCJ.Models.WIP
{
    /// <summary>
    /// 包装关系
    /// </summary>
    [Serializable]
    public class XPPackingRelation
    {
        /// <summary>
        /// 
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 包装号
        /// </summary>
        public string PackageNo { get; set; }

        /// <summary>
        /// 父包装号
        /// </summary>
        public string ParentNo { get; set; }

        /// <summary>
        /// 已加入包装数
        /// </summary>
        public decimal PackedQty { get; set; }

        /// <summary>
        /// 满包装包装数
        /// </summary>
        public decimal FullPackedQty { get; set; }

        /// <summary>
        /// 物料数量
        /// </summary>
        public decimal ItemQty { get; set; }

        /// <summary>
        /// 包装人
        /// </summary>
        public double PackingBy { get; set; }

        /// <summary>
        /// 包装时间
        /// </summary>
        public DateTime PackedDate { get; set; }

        /// <summary>
        /// 包装单位ID
        /// </summary>
        public double PackageUnitId { get; set; }

        /// <summary>
        /// 根Id
        /// </summary>
        public double RootId { get; set; }

        /// <summary>
        /// 物流状态
        /// </summary>
        public LogisticState State { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public double ProcessId { get; set; }

        /// <summary>
        /// 工位ID
        /// </summary>
        public double StationId { get; set; }

        /// <summary>
        /// 工序是否完成
        /// </summary>
        public bool IsProcessFinish { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 包装单位
        /// </summary>
        public string PackageUnitName { get; set; }

        /// <summary>
        /// 是否已打包
        /// </summary> 
        public bool IsPacked { get; set; }

        /// <summary>
        /// 是否满包装
        /// </summary> 
        public bool IsFullPackage { get; set; }

        /// <summary>
        /// 客户
        /// </summary>
        public string Customer { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipCode { get; set; }

        /// <summary>
        /// 信息ID
        /// </summary>
        public string MessageId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<XPItemLabel> ListItemLabel { get; set; } = new List<XPItemLabel>();
    }
}
