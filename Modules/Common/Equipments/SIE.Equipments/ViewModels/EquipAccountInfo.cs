using SIE.Domain;
using SIE.Equipments.EquipAccountLocations;
using SIE.Equipments.EquipAccounts;
using System;

namespace SIE.Equipments.ViewModels
{
    /// <summary>
    /// 设备台账信息
    /// </summary>
    [Serializable]
    public class EquipAccountInfo
    {
        /// <summary>
        /// 设备台账编码
        /// </summary>
        public string AccountCode { get; set; }

        /// <summary>
        /// 设备台账名称
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 型号编码
        /// </summary>
        public string ModelCode { get; set; }

        /// <summary>
        /// 行业属性
        /// </summary>
        public string IndustryCategory { get; set; }

        /// <summary>
        /// 校验类别
        /// </summary>
        public string CheckCategory { get; set; }

        #region EIS

        /// <summary>
        /// 轨道类型
        /// </summary>
        public int? RailType { get; set; }

        /// <summary>
        /// 虚拟设备
        /// </summary>
        public int VirtualDevice { get; set; }

        /// <summary>
        /// 是否Feeder绑定
        /// </summary>
        public int FeederBinding { get; set; }

        /// <summary>
        /// 启用站位防错
        /// </summary>
        public int FeederLocFailSafe { get; set; }

        /// <summary>
        /// 启用Feeder防错
        /// </summary>
        public int FeederBarcodeFailSafe { get; set; }

        /// <summary>
        /// 禁用
        /// </summary>
        public int IsDisabled { get; set; }

        /// <summary>
        /// 老化方式
        /// </summary>
        public int? AgingType { get; set; }

        /// <summary>
        /// 产品生产模式
        /// </summary>
        public int? ProductionType { get; set; }

        #endregion

        #region PCB

        /// <summary>
        /// 平均节拍
        /// </summary>
        public decimal AverageBeat { get; set; }

        /// <summary>
        /// 标准产能
        /// </summary>
        public decimal StandardCapacity { get; set; }

        /// <summary>
        /// 产能单位
        /// </summary>
        public string CapacityUnit { get; set; }

        #endregion

        #region 子列表
        /// <summary>
        /// 设备台账位置列表
        /// </summary>
        public EntityList<EquipAccountLocation> LocationList { get; set; } = new EntityList<EquipAccountLocation>();

        /// <summary>
        /// 设备物联列表
        /// </summary>
        public EntityList<EquipAccountPhysicalUnion> PhysicalUnionList { get; set; } = new EntityList<EquipAccountPhysicalUnion>();

        #endregion
    }
}
