using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.Models.WIP.Entity
{
    /// <summary>
    /// 工单BOM
    /// </summary>
    public class WorkOrderBom
    {
        /// <summary>
        /// 
        /// </summary>
        public double Id{ get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkOrderBom()
        {
            IsByBill = false;
            IsRecoilItem = false;
            IsVritualItem = false;
        }

        /// <summary>
        /// 需求量
        /// </summary>
        public decimal RequireQty { get; set; }

        /// <summary>
        /// 单位耗用量
        /// </summary>
        public decimal SingleQty { get; set; }

        
        /// <summary>
        /// 是否反冲物料
        /// </summary>
        public bool IsRecoilItem { get; set; }

        /// <summary>
        /// 是否虚拟物料
        /// </summary>
        public bool IsVritualItem { get; set; }

        /// <summary>
        /// 是否按单标识
        /// </summary>
        public bool IsByBill { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item { get; set; }
        /// <summary>
        /// 工单与工单BOM关系Id
        /// </summary>
        public double WorkOrderId { get; set; }
        /// <summary>
        /// 工单与工单BOM关系
        /// </summary>
        public WorkOrder WorkOrder { get; set; }
        
        ///
        /// 扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 扩展属性值
        /// </summary>
        public string ItemExtPropName { get; set; }

       
        /// <summary>
        /// 是替代料
        /// </summary>
        public bool IsAlternative { get; set; }

        /// <summary>
        /// 是否允许编辑物料扩展属性
        /// </summary>
        public bool IsAllowEdit { get; set; }

        /// <summary>
        /// Erp主键
        /// </summary>
        public string ErpKey { get; set; }

    }
}
