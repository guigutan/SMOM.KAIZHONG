using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.LES.Datas
{
    /// <summary>
    /// 上料挪料信息(公共库存移至目标工单)
    /// </summary>
    [Serializable]
    public class LoadItemMoveData
    {
        /// <summary>
        /// 库存组织
        /// </summary>
        public int InvOrgId { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料扩展属性Id
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode { get; set; }

        /// <summary>
        /// 目标工单Id
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 挪料数量
        /// </summary>
        public decimal MoveQty { get; set; }
    }
}
