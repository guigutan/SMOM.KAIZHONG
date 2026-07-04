using SIE.ObjectModel;

namespace SIE.Equipments.Enums
{
    /// <summary>
    /// 设备履历类型
    /// </summary>
    public enum ResumeType
    {
        /// <summary>
        /// 报修
        /// </summary>
        [Label("报修")]
        CallRepair = 0,
        /// <summary>
        /// 维修
        /// </summary>
        [Label("维修")]
        Repair = 1,
        /// <summary>
        /// 保养
        /// </summary>
        [Label("保养")]
        Maintain = 2,
        /// <summary>
        /// 变更
        /// </summary>
        [Label("变更")]
        Changed = 3,
        /// <summary>
        /// 点检
        /// </summary>
        [Label("点检")]
        Checked = 4,

        /// <summary>
        /// 立卡
        /// </summary>
        [Label("立卡")]
        Card = 5,

        /// <summary>
        /// 转固
        /// </summary>
        [Label("转固")]
        ToFixedAssets = 6,

        /// <summary>
        /// 验收
        /// </summary>
        [Label("验收")]
        Acceptance = 7,

        /// <summary>
        /// 定检
        /// </summary>
        [Label("定检")]
        Inspection = 8,
        ///// <summary>
        ///// 校准
        ///// </summary>
        //[Label("校准")]
        //Calibration = 9,
        /// <summary>
        /// 润滑
        /// </summary>
        [Label("润滑")]
        Lubrication = 10,

        /// <summary>
        /// 资产调拨
        /// </summary>
        [Label("资产调拨")]
        AssetTranstfers = 11,

        /// <summary>
        /// 特种设备定检
        /// </summary>
        [Label("特种设备定检")]
        RegularInspection = 12,

        /// <summary>
        /// 计量设备定检
        /// </summary>
        [Label("计量设备定检")]
        Calibration = 13,

        /// <summary>
        /// 闲置
        /// </summary>
        [Label("闲置")]
        Idle = 14,

        /// <summary>
        /// 封存
        /// </summary>
        [Label("封存")]
        Archive = 15,

        /// <summary>
        /// 闲置启用
        /// </summary>
        [Label("闲置启用")]
        IdleEnabled = 16,

        /// <summary>
        /// 封存启用
        /// </summary>
        [Label("封存启用")]
        ArchiveEnabled = 17,

        /// <summary>
        /// 计划维修
        /// </summary>
        [Label("计划维修")]
        PlanRepair = 18,

        /// <summary>
        /// 领用发放
        /// </summary>
        [Label("领用发放")]
        RequisitionIssue = 19,

        /// <summary>
        /// 借用发放
        /// </summary>
        [Label("借用发放")]
        LendingIssue = 20,

        /// <summary>
        /// 资产归还
        /// </summary>
        [Label("资产归还")]
        AssetReturn = 21,

        /// <summary>
        /// 报废
        /// </summary>
        [Label("报废")]
        AssetScrap = 22,
        /// <summary>
        /// 设备立卡
        /// </summary>
        [Label("设备立卡")]
        EquipmentCard = 23,
        /// <summary>
        /// 处置
        /// </summary>
        [Label("处置")]
        AssetDisposal = 24,

        /// <summary>
        /// 设备借还
        /// </summary>
        [Label("设备借还")]
        LendReturn = 25,
    }
}