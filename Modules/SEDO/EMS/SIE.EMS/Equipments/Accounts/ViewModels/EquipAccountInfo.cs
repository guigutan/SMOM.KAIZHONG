using SIE.Domain;
using SIE.Equipments.EquipAccountLocations;
using SIE.Equipments.EquipAccounts;
using System;

namespace SIE.EMS.Equipments.Accounts.ViewModels
{
    /// <summary>
    /// 设备台账信息
    /// </summary>
    [Serializable]
    public class EquipAccountInfo
    {
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

        /// <summary>
        /// 是否计量类型
        /// </summary>
        public bool IsCalibration { get; set; }

        #endregion

        #region 子列表

        /// <summary>
        /// 点检项目列表
        /// </summary>
        public EntityList<EquipAccountCheckProject> CheckPrjList { get; set; } = new EntityList<EquipAccountCheckProject>();

        /// <summary>
        /// 保养项目列表
        /// </summary>
        public EntityList<EquipAccountMaintainProject> MaintainPrjList { get; set; } = new EntityList<EquipAccountMaintainProject>();

        ///// <summary>
        ///// 单元组成列表
        ///// </summary>
        //public EntityList<EquipAccountUnit> UnitList { get; set; } = new EntityList<EquipAccountUnit>();

        ///// <summary>
        ///// 单元组成物料列表
        ///// </summary>
        //public EntityList<EquipAccountUnitItem> UnitItemList { get; set; } = new EntityList<EquipAccountUnitItem>();

        /// <summary>
        /// 设备台账位置列表
        /// </summary>
        public EntityList<EquipAccountLocation> LocationList { get; set; } = new EntityList<EquipAccountLocation>();

        /// <summary>
        /// 设备物联列表
        /// </summary>
        public EntityList<EquipAccountPhysicalUnion> PhysicalUnionList { get; set; } = new EntityList<EquipAccountPhysicalUnion>();

        /// <summary>
        /// 润滑项目列表
        /// </summary>
        public EntityList<EquipAccountLubricationProject> LubricationProjectList { get; set; } = new EntityList<EquipAccountLubricationProject>();

        /// <summary>
        /// 设备型号技术参数列表
        /// </summary>
        public EntityList<Models.EquipModelTechParameter> TechParameterList { get; set; } = new EntityList<Models.EquipModelTechParameter>();
        #endregion
    }
}
