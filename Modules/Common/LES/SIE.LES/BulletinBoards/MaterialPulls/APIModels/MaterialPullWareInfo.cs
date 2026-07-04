using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.LES.BulletinBoards.MaterialPulls.APIModels
{
    /// <summary>
    /// 物料拉动(仓库)API数据实体
    /// </summary>
    [Serializable]
    public class MaterialPullWareInfo
    {
        /// <summary>
        /// 计划单号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public double? ResourceId { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 发货数(已发数量)
        /// </summary>
        public decimal DeliveryQty { get; set; }

        /// <summary>
        /// 需求数
        /// </summary>
        public decimal RequireQty { get; set; }

        /// <summary>
        /// 拣货数Sum
        /// </summary>
        public decimal PickingQty { get; set; }

        /// <summary>
        /// 剩余数(RequireQty - DeliveryQty)
        /// </summary>
        public decimal Residue { get; set; }

        /// <summary>
        /// 未建单数
        /// </summary>
        public decimal NoCreateQty { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// 需求时间
        /// </summary>
        public DateTime DemandTime { get; set; }
    }
}
