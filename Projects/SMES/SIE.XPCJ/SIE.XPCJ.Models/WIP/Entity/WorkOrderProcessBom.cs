using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.Models.WIP.Entity
{
    [Serializable]
    public class WorkOrderProcessBom
    {
        /// <summary>
        /// Id
        /// </summary>
        public double Id { get; set; }

        public WorkOrderProcessBom()
        {
            this.SingleQty = 0m;
        }
        /// <summary>
        /// 单位Id
        /// </summary>
        public double? UnitId { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName { get; set; }
        /// <summary>
        /// 单位耗用量
        /// </summary>
        public decimal SingleQty { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process { get; set; }

        /// <summary>
        /// 工步Id
        /// </summary>
        public double? WorkStepId { get; set; }

        /// <summary>
        /// 工序序列
        /// </summary>
        public int Seq { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }
        /// <summary>
        /// 物料
        /// </summary>
        public Item Item { get; set; }


        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string ItemUnitName { get; set; }

        /// <summary>
        /// 工单工序清单工序名称
        /// </summary>
        public string RoutingProcessName { get; set; }

        /// <summary>
        /// 替代组合分组
        /// </summary>
        public string AlterGroup { get; set; }

        /// <summary>
        /// 替代组
        /// </summary>
        public string Alter { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string ItemSpecificationModel { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessNameView { get; set; }

        /// <summary>
        /// 工段名称
        /// </summary>
        public string SegmentName { get; set; }

        /// <summary>
        ///工步名称
        /// </summary>
        public string WorkStepName { get; set; }

        /// <summary>
        /// 工单工序清单Id
        /// </summary>
        public double? RoutingProcessId { get; set; }

        /// <summary>
        /// 工单与工序BOM关系Id
        /// </summary>
        public double WorkOrderId { get; set; }
        /// <summary>
        /// 工单与工序BOM关系
        /// </summary>
        public WorkOrder WorkOrder { get; set; }


        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanQty { get; set; }


        /// <summary>
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
    }
}
