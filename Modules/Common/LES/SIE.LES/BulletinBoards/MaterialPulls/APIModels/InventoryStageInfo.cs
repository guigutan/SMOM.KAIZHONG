using System;

namespace SIE.LES.BulletinBoards.MaterialPulls.APIModels
{
    /// <summary>
    /// 水位看板信息
    /// </summary>
    [Serializable]
    public class InventoryStageInfo
    {
        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProName { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// 安全水位
        /// </summary>
        public decimal? LowestStage { get; set; }

        /// <summary>
        /// 现有库存
        /// </summary>
        public decimal NowStage { get; set; }

        /// <summary>
        /// 工单剩余需求
        /// </summary>
        public decimal WoNeed { get; set; }

        /// <summary>
        /// 警戒水位
        /// </summary>
        public decimal? HightestStage { get; set; }

        /// <summary>
        /// 剩余耗用时间
        /// </summary>
        public decimal LastCostTime { get; set; }
    }

    /// <summary>
    /// 物料单位
    /// </summary>
    [Serializable]
    public class ItemUnitInfo
    {
        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName { get; set; }
    }

    /// <summary>
    /// 工单剩余需求和bom每小时耗用量
    /// </summary>
    [Serializable]
    public class RequireUseInfo
    {
        /// <summary>
        /// 工单剩余需求Z
        /// </summary>
        public decimal WoNeed { get; set; }

        /// <summary>
        /// bom每小时耗用量B
        /// </summary>
        public decimal BomUseHour { get; set; }

        /// <summary>
        /// Z/B
        /// </summary>
        public decimal Remainder { get; set; }
    }
}
