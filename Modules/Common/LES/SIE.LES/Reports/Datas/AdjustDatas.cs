using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.LES.Reports.Datas
{
    /// <summary>
    /// 数量更新参数
    /// </summary>
    [Serializable]
    public class AdjustQtyParams
    {

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId { get; set; }

        /// <summary>
        /// 物料
        /// </summary>
        public double ItemId { get; set; }
        /// <summary>
        /// 物料扩展属性名称
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }


        /// <summary>
        /// 物料数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public double? WorkOrderId { get; set; }
        /// <summary>
        /// 车间
        /// </summary>
        public double? WorkShopId { get; set; }
        /// <summary>
        /// 资源
        /// </summary>
        public double? ResourceId { get; set; }


    }
}
