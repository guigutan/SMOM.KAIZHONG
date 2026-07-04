using SIE.XPCJ.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.Models.WIP.Entity
{
    [Serializable]
    public class Item
    {
        /// <summary>
        /// 
        /// </summary>
        public double Id { get; set; }


        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public State State { get; set; }


        /// <summary>
        /// 类型
        /// </summary>
        public ItemType Type { get; set; }

        
        /// <summary>
        /// 物料消耗类型
        /// </summary>
        public ConsumeMode ConsumeMode { get; set; }

        /// <summary>
        /// 来源类型
        /// </summary>
        public ItemSourceType? ItemSourceType { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        public SourceType SourceType { get; set; }

        /// <summary>
        /// 物料标签类型
        /// </summary>
        public ItemLabelType ItemLabelType { get; set; }

        /// <summary>
        /// 单位Id
        /// </summary>
        public double? UnitId { get; set; }


        /// <summary>
        /// 单位编码
        /// </summary>
        public string UnitCode { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// 单位精度
        /// </summary>
        public int? UnitPrecision { get; set; }
    }

    public enum ItemLabelType
    {
        /// <summary>
        /// 物料标签
        /// </summary>
        [Label("物料标签")]
        ItemLabel,

        /// <summary>
        /// 物料批次
        /// </summary>
        [Label("物料批次")]
        ItemBatch,

        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        ItemCode,
    }

    public enum State
    {
        //
        // 摘要:
        //     可用
        [Label("可用")]
        Enable = 1,
        //
        // 摘要:
        //     禁用
        [Label("禁用")]
        Disable = 0
    }
    public enum ItemType
    {
        /// <summary>
        /// 成品
        /// </summary>
        [Category("CadManage")]
        [Label("成品")]
        Product = 0,

        /// <summary>
        /// 原材料
        /// </summary>
        [Label("原材料")]
        Material = 1,

        /// <summary>
        /// 半成品
        /// </summary>
        [Category("CadManage")]
        [Label("半成品")]
        SemiFinished = 2,

        /// <summary>
        /// 备件
        /// </summary>
        [Category("CadManage")]
        [Label("备件")]
        SparePart = 3,

        /// <summary>
        /// 其他
        /// </summary>
        [Label("其他")]
        Other = 9,

    }

    /// <summary>
    /// 物料消耗类型
    /// </summary>
    public enum ConsumeMode
    {
        /// <summary>
        /// 拉式物料
        /// </summary>
        [Label("拉式物料")]
        [Category("PullPush")]
        Pull = 0,

        /// <summary>
        /// 推式物料
        /// </summary>
        [Label("推式物料")]
        [Category("PullPush")]
        Push = 1,

        /// <summary>
        /// 储备物料
        /// </summary>
        [Label("储备物料")]
        Reserve = 2
    }
    public enum ItemSourceType
    {
        /// <summary>
        /// 外购
        /// </summary>
        [Label("外购")]
        Outsourcing,

        /// <summary>
        /// 自制
        /// </summary>
        [Label("自制")]
        SelfMade,

        /// <summary>
        /// 外协
        /// </summary>
        [Label("外协")]
        OutMade,
    }
    public enum SourceType
    {
        //
        // 摘要:
        //     自建
        [Label("自建")]
        Internal,
        //
        // 摘要:
        //     外部
        [Label("外部")]
        External
    }
}
