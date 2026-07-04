using SIE.Common.Configs;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.EMS.Checks.Plans.ViewModels;
using SIE.EMS.Maintains.Configs;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Maintains.Plans.ViewModels
{
    /// <summary>
    /// 保养计划ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(MaintainPlanCriteria))]
    [EntityWithConfig(typeof(MaintainPlanNoConfig))]
    [EntityWithConfig(typeof(MaintainAlertTimeConfig))]
    //[EntityWithConfig(typeof(MaintainPlanScoreConfig))]
    [EntityWithConfig(typeof(MaintainChildProjectConfig))]
    [EntityWithConfig(typeof(MaintainWorkTimeConfig))]
    [EntityWithConfig(typeof(MaintainIntervalTimeConfig))]
    [EntityWithConfig(typeof(MaintainPrecisePlanConfig))]
    [EntityWithConfig(typeof(MaintainConfirmDepartConfig))]
    [Label("保养计划")]
    public class MaintainPlanViewModel : ViewModel
    {
        #region 年/月 YearAndMonth
        /// <summary>
        /// 年/月
        /// </summary>
        [Label("年/月")]
        public static readonly Property<DateTime?> YearAndMonthProperty = P<MaintainPlanViewModel>.Register(e => e.YearAndMonth);

        /// <summary>
        /// 年/月
        /// </summary>
        public DateTime? YearAndMonth
        {
            get { return this.GetProperty(YearAndMonthProperty); }
            set { this.SetProperty(YearAndMonthProperty, value); }
        }
        #endregion

        #region 设备Id EquipAccountId
        /// <summary>
        /// 设备Id
        /// </summary>
        [Label("设备Id")]
        public static readonly Property<double> EquipAccountIdProperty = P<MaintainPlanViewModel>.Register(e => e.EquipAccountId);

        /// <summary>
        /// 设备Id
        /// </summary>
        public double EquipAccountId
        {
            get { return this.GetProperty(EquipAccountIdProperty); }
            set { this.SetProperty(EquipAccountIdProperty, value); }
        }
        #endregion

        #region 设备编码 EquipAccountCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipAccountCodeProperty = P<MaintainPlanViewModel>.Register(e => e.EquipAccountCode);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipAccountCode
        {
            get { return this.GetProperty(EquipAccountCodeProperty); }
            set { this.SetProperty(EquipAccountCodeProperty, value); }
        }
        #endregion

        #region 设备名称 EquipAccountName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipAccountNameProperty = P<MaintainPlanViewModel>.Register(e => e.EquipAccountName);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipAccountName
        {
            get { return this.GetProperty(EquipAccountNameProperty); }
            set { this.SetProperty(EquipAccountNameProperty, value); }
        }
        #endregion

        #region 设备型号 EquipModelName
        /// <summary>
        /// 设备型号
        /// </summary>
        [Label("设备型号")]
        public static readonly Property<string> EquipModelNameProperty = P<MaintainPlanViewModel>.Register(e => e.EquipModelName);

        /// <summary>
        /// 设备型号
        /// </summary>
        public string EquipModelName
        {
            get { return this.GetProperty(EquipModelNameProperty); }
            set { this.SetProperty(EquipModelNameProperty, value); }
        }
        #endregion

        #region 设备类别 EquipTypeCategory
        /// <summary>
        /// 设备类别
        /// </summary>
        [Label("设备类别")]
        public static readonly Property<string> EquipTypeCategoryProperty = P<MaintainPlanViewModel>.Register(e => e.EquipTypeCategory);

        /// <summary>
        /// 设备类别
        /// </summary>
        public string EquipTypeCategory
        {
            get { return this.GetProperty(EquipTypeCategoryProperty); }
            set { this.SetProperty(EquipTypeCategoryProperty, value); }
        }
        #endregion

        #region 车间 WorkShopName
        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        public static readonly Property<string> WorkShopNameProperty = P<MaintainPlanViewModel>.Register(e => e.WorkShopName);

        /// <summary>
        /// 车间
        /// </summary>
        public string WorkShopName
        {
            get { return this.GetProperty(WorkShopNameProperty); }
            set { this.SetProperty(WorkShopNameProperty, value); }
        }
        #endregion

        #region 产线 ResourceName
        /// <summary>
        /// 产线
        /// </summary>
        [Label("产线")]
        public static readonly Property<string> ResourceNameProperty = P<MaintainPlanViewModel>.Register(e => e.ResourceName);

        /// <summary>
        /// 产线
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
            set { this.SetProperty(ResourceNameProperty, value); }
        }
        #endregion

        #region 管理状态 UseState
        /// <summary>
        /// 管理状态
        /// </summary>
        [Label("管理状态")]
        public static readonly Property<AccountUseState> UseStateProperty = P<MaintainPlanViewModel>.Register(e => e.UseState);

        /// <summary>
        /// 管理状态
        /// </summary>
        public AccountUseState UseState
        {
            get { return this.GetProperty(UseStateProperty); }
            set { this.SetProperty(UseStateProperty, value); }
        }
        #endregion


        #region 数据 DataJsonString
        /// <summary>
        /// 数据
        /// </summary>
        [Label("数据")]
        public static readonly Property<string> DataJsonStringProperty = P<MaintainPlanViewModel>.Register(e => e.DataJsonString);

        /// <summary>
        /// 数据
        /// </summary>
        public string DataJsonString
        {
            get { return this.GetProperty(DataJsonStringProperty); }
            set { this.SetProperty(DataJsonStringProperty, value); }
        }
        #endregion
    }
}
