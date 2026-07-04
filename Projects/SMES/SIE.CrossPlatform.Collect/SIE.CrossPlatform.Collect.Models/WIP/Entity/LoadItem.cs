using SIE.CrossPlatform.Collect.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.CrossPlatform.Collect.Models.WIP.Entity
{
    [Serializable]
    public enum YesNo
    {
        //
        // 摘要:
        //     是
        [Label("是")]
        Yes = 1,
        //
        // 摘要:
        //     否
        [Label("否")]
        No = 0
    }

    [Serializable]
    public  class LoadItem
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public LoadItem()
        {
            this.LoadQty = 0m;
            this.Qty = 0m;
            this.UnloadQty = 0m;
            this.NgQty = 0m;

            //默认挪料上料为否
            this.IsMoveItem = YesNo.No;
        }
        #endregion

        /// <summary>
        /// 数据Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 来源条码 
        /// </summary>
        public string SourceCode { get; set; }

        /// <summary>
        /// 来源ID
        /// </summary>
        public double SourceId { get; set; }

        /// <summary>
        /// 上料数量
        /// </summary>
        public decimal LoadQty { get; set; }

        /// <summary>
        /// 可用数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 下料数量
        /// </summary>
        public decimal UnloadQty { get; set; }
        /// <summary>
        /// 不良数量
        /// </summary>
        public decimal NgQty { get; set; }

        /// <summary>
        /// 班次Id
        /// </summary>
        public double ShiftId { get; set; }

        /// <summary>
        /// 资源
        /// </summary>
        public double ResourceId { get; set; }
        /// <summary>
        /// 工位Id
        /// </summary>
        public double StationId { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }
        /// <summary>
        /// 物料
        /// </summary>
        public Item Item { get; set; }


        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 扩展属性值
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 上料来源类型
        /// </summary>
        public LoadItemSourceType SourceType { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder { get; set; }

        /// <summary>
        /// 挪料上料
        /// </summary>
        public YesNo IsMoveItem { get; set; }

        /// <summary>
        /// 替代组合分组
        /// </summary>
        public string AlterGroup { get; set; }
        /// <summary>
        /// 替代组
        /// </summary>
        public string Alter { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }


        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 物料消耗属性
        /// </summary>
        public ConsumeMode ItemConsumeMode { get; set; }

        /// <summary>
        /// 工位名称
        /// </summary>
        public string StationName { get; set; }


        /// <summary>
        /// 单位精度
        /// </summary>
        public int? UnitPrecision { get; set; }

        /// <summary>
        ///创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

    }

    public class LoadItemViewModel
    {
        public double Id { get; set; }
        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }


        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        ///上料时间
        /// </summary>
        public string CreateDate { get; set; }


        /// <summary>
        /// 上料来源类型
        /// </summary>
        public string SourceType { get; set; }


        /// <summary>
        /// 上料数量
        /// </summary>
        public decimal LoadQty { get; set; }

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 来源条码 
        /// </summary>
        public string SourceCode { get; set; }
    }
}
