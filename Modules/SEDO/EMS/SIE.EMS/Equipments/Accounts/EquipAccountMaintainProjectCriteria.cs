using SIE.Domain;
using SIE.EMS.MainenanceProjects;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Equipments.Accounts
{
    /// <summary>
    /// 设备台账保养项目查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("设备台账保养项目查询实体")]
    public partial class EquipAccountMaintainProjectCriteria : Criteria
    {
        #region 设备编码 EquipAccountCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipAccountCodeProperty = P<EquipAccountMaintainProjectCriteria>.Register(e => e.EquipAccountCode);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipAccountCode
        {
            get { return GetProperty(EquipAccountCodeProperty); }
            set { SetProperty(EquipAccountCodeProperty, value); }
        }
        #endregion

        #region 设备名称 EquipAccountName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipAccountNameProperty = P<EquipAccountMaintainProjectCriteria>.Register(e => e.EquipAccountName);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipAccountName
        {
            get { return GetProperty(EquipAccountNameProperty); }
            set { SetProperty(EquipAccountNameProperty, value); }
        }
        #endregion

        #region 项目名称 ProjectName
        /// <summary>
        /// 项目名称
        /// </summary>
        [Label("项目名称")]
        public static readonly Property<string> ProjectNameProperty = P<EquipAccountMaintainProjectCriteria>.Register(e => e.ProjectName);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName
        {
            get { return GetProperty(ProjectNameProperty); }
            set { SetProperty(ProjectNameProperty, value); }
        }
        #endregion

        #region 周期类型 CycleType
        /// <summary>
        /// 周期类型
        /// </summary>
        [Label("周期类型")]
        public static readonly Property<CycleType?> CycleTypeProperty = P<EquipAccountMaintainProjectCriteria>.Register(e => e.CycleType);

        /// <summary>
        /// 周期类型
        /// </summary>
        public CycleType? CycleType
        {
            get { return GetProperty(CycleTypeProperty); }
            set { SetProperty(CycleTypeProperty, value); }
        }
        #endregion

        #region 项目分类 Category
        /// <summary>
        /// 项目分类
        /// </summary>
        [Label("项目分类")]
        public static readonly Property<string> CategoryProperty = P<EquipAccountMaintainProjectCriteria>.Register(e => e.Category);

        /// <summary>
        /// 项目分类
        /// </summary>
        public string Category
        {
            get { return GetProperty(CategoryProperty); }
            set { SetProperty(CategoryProperty, value); }
        }
        #endregion
    }
}
