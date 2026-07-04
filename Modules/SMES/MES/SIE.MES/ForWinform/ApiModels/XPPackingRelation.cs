using SIE.Domain;
using SIE.Packages;
using SIE.Packages.ItemLabels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ForWinform.ApiModels
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
        public Packages.LogisticState State { get; set; }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="packingRelation"></param>
        public XPPackingRelation(PackingRelation packingRelation, EntityList<ItemLabel> itemLabels)
        {
            this.Id = packingRelation.Id;
            this.PackageNo = packingRelation.PackageNo;
            this.ParentNo = packingRelation.ParentNo;
            this.PackedQty = packingRelation.PackedQty;
            this.FullPackedQty = packingRelation.FullPackedQty;
            this.ItemQty = packingRelation.ItemQty;
            this.PackingBy = packingRelation.PackingBy;
            this.PackedDate = packingRelation.PackedDate;
            this.PackageUnitId = packingRelation.PackageUnitId;
            this.RootId = packingRelation.RootId;
            this.State = packingRelation.State;
            this.ProcessId = packingRelation.ProcessId;
            this.StationId = packingRelation.StationId;
            this.IsProcessFinish = packingRelation.IsProcessFinish;
            this.WorkOrderId = packingRelation.WorkOrderId;
            this.PackageUnitName = packingRelation.PackageUnitName;
            this.IsPacked = packingRelation.IsPacked;
            this.IsFullPackage = packingRelation.IsFullPackage;
            this.Customer = packingRelation.Customer;
            this.ItemCode = packingRelation.ItemCode;
            this.ItemName = packingRelation.ItemName;
            this.EquipCode = packingRelation.EquipCode;
            this.MessageId = packingRelation.MessageId;

            if (itemLabels != null)
            {
                foreach (var il in itemLabels)
                {
                    if (il.RelationId.HasValue && il.RelationId.Value == this.Id)
                        this.ListItemLabel.Add(new XPItemLabel(il));
                }
            }
        }
    }
}
