using SIE.XPCJ.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.Models.WIP.Entity
{
    [Serializable]
    public class UnloadItem
    {
        /// <summary>
        /// Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 来源条码
        /// </summary>
        public string SourceCode { get; set; }


        /// <summary>
        /// 来源ID
        /// </summary>
        public double SourceId
        { get; set; }

        /// <summary>
        /// 来源类型
        /// </summary>
        public LoadItemSourceType SourceType
        { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        { get; set; }




        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get; set;
        }

        /// <summary>
        /// 上料数量
        /// </summary>
        public decimal LoadItemQty
        {
            get; set;
        }

        /// <summary>
        /// 下料数量
        /// </summary>
        public decimal Qty
        {
            get; set;
        }

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal RemainderQty
        {
            get; set;
        }

        /// <summary>
        /// 班次Id
        /// </summary>
        public double ShiftId
        {
            get; set;
        }

        /// <summary>
        /// 不良列表
        /// </summary>
        public List<UnloadItemDefect> DefectList
        {
            get; set;
        }

        /// <summary>
        /// 下料接收状态
        /// </summary>
        public UnloadState State
        {
            get; set;
        }

        /// <summary>
        /// 产线Id
        /// </summary>
        public double ResourceId
        {
            get; set;
        }


        /// <summary>
        /// 产线
        /// </summary>
        public string ResourceName
        {
            get; set;
        }

        /// <summary>
        /// 是否不良下料
        /// </summary>
        public bool IsNg
        {
            get; set;
        }

        /// <summary>
        /// 工位Id
        /// </summary>
        public double StationId
        {
            get; set;
        }
        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get; set;
        }


        /// <summary>
        /// 扩展属性值
        /// </summary>
        public string ItemExtPropName
        {
            get; set;
        }
        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get; set;
        }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get; set;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate
        {
            get; set;
        }
    }
    /// <summary>
    /// 下料接收状态
    /// </summary>
    public enum UnloadState
    {
        /// <summary>
        /// 待确认
        /// </summary>
        [Label("待确认")]
        UnConfirm,

        /// <summary>
        /// 已确认
        /// </summary>
        [Label("已确认")]
        Confirmed,

        /// <summary>
        /// 重新上料
        /// </summary>
        [Label("重新上料")]
        ReloadItem,
    }
}
