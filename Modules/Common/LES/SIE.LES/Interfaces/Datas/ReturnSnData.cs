using System;
using System.Collections.Generic;

namespace SIE.LES.Interfaces.Datas
{
    /// <summary>
    /// MES退料数据
    /// </summary>
    [Serializable]
    public class ReturnSnData
    {
        /// <summary>
        /// 标签
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotCode { get; set; }

        /// <summary>
        /// 物料
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 退料数量/挪料数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 来源库位
        /// </summary>
        public double? SourceStorageLocationId { get; set; }

        /// <summary>
        /// 来源仓库
        /// </summary>
        public double? SourceWarehouseId { get; set; }

        /// <summary>
        /// 来源仓库编码
        /// </summary>
        public string SourceWarehouseCode { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public string WoNo { get; set; }

        /// <summary>
        /// 车间Id
        /// </summary>
        public double WorkShopId { get; set; }

        /// <summary>
        /// 是否不合格
        /// </summary>
        public bool IsFail { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 剩余数量(计算用，MES不需要管这个字段)
        /// </summary>
        public decimal LeftQty { get; set; }

        /// <summary>
        /// 是否序列号物料(计算用，MES不需要管这个字段)
        /// </summary>
        public bool IsSerialNumber { get; set; }

        /// <summary>
        /// 已经被使用过(计算用，MES不需要管这个字段)
        /// </summary>
        public bool IsUse { get; set; }
    }

    /// <summary>
    /// 工单挪料数据
    /// </summary>
    [Serializable]
    public class MoveSnData
    {
        /// <summary>
        /// 退料信息
        /// </summary>
        public List<ReturnSnData> ReturnSnDatas { get; set; } = new List<ReturnSnData>();

        /// <summary>
        /// 目标工单
        /// </summary>
        public double TargetWoId { get; set; }

        /// <summary>
        /// 目标库位（挪料转移库位的时候必填）
        /// </summary>
        public double? TargetStorageLocationId { get; set; }

        /// <summary>
        /// 目标仓库（挪料转移库位的时候必填）
        /// </summary>
        public double? TargetWarehouseId { get; set; }

        /// <summary>
        /// 目标仓库编码（挪料转移库位的时候必填）
        /// </summary>
        public string TargetWarehouseCode { get; set; }

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double FactoryId { get; set; }
    }
}
