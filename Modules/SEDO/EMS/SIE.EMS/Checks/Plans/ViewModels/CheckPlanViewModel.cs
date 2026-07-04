using SIE.Common.Configs;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.EMS.Checks.Configs;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Checks.Plans.ViewModels
{
    /// <summary>
    /// 点检计划ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [EntityWithConfig(typeof(CheckPlanNoConfig))]
    [EntityWithConfig(typeof(CheckPlanTypeConfig))]
    [EntityWithConfig(typeof(CheckAlertTimeConfig))]
    //[EntityWithConfig(typeof(CheckPlanScoreConfig))]
    [EntityWithConfig(typeof(CheckChildProjectConfig))]
    [EntityWithConfig(typeof(CheckConfirmDepartConfig))]
    [ConditionQueryType(typeof(CheckPlanCriteria))]
    [Label("点检计划")]
    public class CheckPlanViewModel : ViewModel
    {
        #region 年/月 YearAndMonth
        /// <summary>
        /// 年/月
        /// </summary>
        [Label("年/月")]
        public static readonly Property<DateTime?> YearAndMonthProperty = P<CheckPlanViewModel>.Register(e => e.YearAndMonth);

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
        public static readonly Property<double> EquipAccountIdProperty = P<CheckPlanViewModel>.Register(e => e.EquipAccountId);

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
        public static readonly Property<string> EquipAccountCodeProperty = P<CheckPlanViewModel>.Register(e => e.EquipAccountCode);

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
        public static readonly Property<string> EquipAccountNameProperty = P<CheckPlanViewModel>.Register(e => e.EquipAccountName);

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
        public static readonly Property<string> EquipModelNameProperty = P<CheckPlanViewModel>.Register(e => e.EquipModelName);

        /// <summary>
        /// 设备型号
        /// </summary>
        public string EquipModelName
        {
            get { return this.GetProperty(EquipModelNameProperty); }
            set { this.SetProperty(EquipModelNameProperty, value); }
        }
        #endregion

        #region 设备类型 EquipTypeName
        /// <summary>
        /// 设备类型
        /// </summary>
        [Label("设备类型")]
        public static readonly Property<string> EquipTypeNameProperty = P<CheckPlanViewModel>.Register(e => e.EquipTypeName);

        /// <summary>
        /// 设备类型
        /// </summary>
        public string EquipTypeName
        {
            get { return this.GetProperty(EquipTypeNameProperty); }
            set { this.SetProperty(EquipTypeNameProperty, value); }
        }
        #endregion

        #region 车间 WorkShopName
        /// <summary>
        /// 车间
        /// </summary>
        [Label("车间")]
        public static readonly Property<string> WorkShopNameProperty = P<CheckPlanViewModel>.Register(e => e.WorkShopName);

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
        public static readonly Property<string> ResourceNameProperty = P<CheckPlanViewModel>.Register(e => e.ResourceName);

        /// <summary>
        /// 产线
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
            set { this.SetProperty(ResourceNameProperty, value); }
        }
        #endregion

        #region 工序 ProcessName
        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static readonly Property<string> ProcessNameProperty = P<CheckPlanViewModel>.Register(e => e.ProcessName);

        /// <summary>
        /// 工序
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
            set { this.SetProperty(ProcessNameProperty, value); }
        }
        #endregion

        #region 管理状态 UseState 
        /// <summary>
        /// 管理状态
        /// </summary>
        [Label("管理状态")]
        public static readonly Property<AccountUseState?> UseStateProperty = P<CheckPlanViewModel>.Register(e => e.UseState);

        /// <summary>
        /// 管理状态（原使用状态）
        /// </summary>
        public AccountUseState? UseState
        {
            get { return GetProperty(UseStateProperty); }
            set { SetProperty(UseStateProperty, value); }
        }
        #endregion


        #region 数据 DataJsonString
        /// <summary>
        /// 数据
        /// </summary>
        [Label("数据")]
        public static readonly Property<string> DataJsonStringProperty = P<CheckPlanViewModel>.Register(e => e.DataJsonString);

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
