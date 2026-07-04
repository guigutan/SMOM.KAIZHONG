using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.Checks.Plans.ViewModels
{
    /// <summary>
    /// 点检计划ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [Label("点检计划")]
    public class AddCheckPlanViewModel : ViewModel
    {

        #region 设备编码 EquipAccount
        /// <summary>
        /// 设备编码Id
        /// </summary>
        [Label("设备编码")]
        public static readonly IRefIdProperty EquipAccountIdProperty = P<AddCheckPlanViewModel>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备编码Id
        /// </summary>
        public double EquipAccountId
        {
            get { return (double)GetRefId(EquipAccountIdProperty); }
            set { SetRefId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备编码
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty = P<AddCheckPlanViewModel>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备编码
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return GetRefEntity(EquipAccountProperty); }
            set { SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 机台号 MachineNo
        /// <summary>
        /// 机台号
        /// </summary>
        [Label("机台号")]
        public static readonly Property<string> MachineNoProperty = P<AddCheckPlanViewModel>.RegisterView(e => e.MachineNo, p => p.EquipAccount.Name);

        /// <summary>
        /// 机台号
        /// </summary>
        public string MachineNo
        {
            get { return GetProperty(MachineNoProperty); }
            set { SetProperty(MachineNoProperty, value); }
        }
        #endregion

        #region 周期类型 CheckCycleType
        /// <summary>
        /// 周期类型
        /// </summary>
        [Label("周期类型")]
        public static readonly Property<CheckCycleType> CheckCycleTypeProperty = P<AddCheckPlanViewModel>.Register(e => e.CheckCycleType);

        /// <summary>
        /// 周期类型
        /// </summary>
        public CheckCycleType CheckCycleType
        {
            get { return GetProperty(CheckCycleTypeProperty); }
            set { SetProperty(CheckCycleTypeProperty, value); }
        }
        #endregion

        #region 计划开始日期 BeginDate
        /// <summary>
        /// 计划开始日期
        /// </summary>
        [Label("计划开始日期")]
        public static readonly Property<DateTime> BeginDateProperty = P<AddCheckPlanViewModel>.Register(e => e.BeginDate);

        /// <summary>
        /// 计划开始日期
        /// </summary>
        public DateTime BeginDate
        {
            get { return GetProperty(BeginDateProperty); }
            set { SetProperty(BeginDateProperty, value); }
        }
        #endregion

        #region 计划结束日期 EndDate
        /// <summary>
        /// 计划结束日期
        /// </summary>
        [Label("计划结束日期")]
        public static readonly Property<DateTime> EndDateProperty = P<AddCheckPlanViewModel>.Register(e => e.EndDate);

        /// <summary>
        /// 计划结束日期
        /// </summary>
        public DateTime EndDate
        {
            get { return GetProperty(EndDateProperty); }
            set { SetProperty(EndDateProperty, value); }
        }
        #endregion

        #region 设备点检类型 EquipCheckType
        /// <summary>
        /// 设备点检类型
        /// </summary>
        [Label("设备点检类型")]
        public static readonly Property<EquipCheckType> EquipCheckTypeProperty = P<AddCheckPlanViewModel>.Register(e => e.EquipCheckType);

        /// <summary>
        /// 设备点检类型
        /// </summary>
        public EquipCheckType EquipCheckType
        {
            get { return this.GetProperty(EquipCheckTypeProperty); }
            set { this.SetProperty(EquipCheckTypeProperty, value); }
        }
        #endregion

        #region 点检时长(min) CheckTime
        /// <summary>
        /// 点检时长(min)
        /// </summary>
        [Label("点检时长(min)")]
        public static readonly Property<int?> CheckTimeProperty = P<AddCheckPlanViewModel>.Register(e => e.CheckTime);

        /// <summary>
        /// 点检时长(min)
        /// </summary>
        public int? CheckTime
        {
            get { return this.GetProperty(CheckTimeProperty); }
            set { this.SetProperty(CheckTimeProperty, value); }
        }
        #endregion

        #region 产线 Resource
        /// <summary>
        /// 产线Id
        /// </summary>
        [Label("产线")]
        public static readonly IRefIdProperty ResourceIdProperty = P<AddCheckPlanViewModel>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)GetRefNullableId(ResourceIdProperty); }
            set { SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 产线
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> ResourceProperty = P<AddCheckPlanViewModel>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public Enterprise Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 产线 Resource
        ///// <summary>
        ///// 产线Id
        ///// </summary>
        //[Label("产线编码")]
        //public static readonly IRefIdProperty ResourceIdProperty = P<AddCheckPlanViewModel>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        ///// <summary>
        ///// 产线Id
        ///// </summary>
        //public double? ResourceId
        //{
        //    get { return (double?)GetRefNullableId(ResourceIdProperty); }
        //    set { SetRefNullableId(ResourceIdProperty, value); }
        //}

        ///// <summary>
        ///// 产线
        ///// </summary>
        //public static readonly RefEntityProperty<WipResource> ResourceProperty = P<AddCheckPlanViewModel>.RegisterRef(e => e.Resource, ResourceIdProperty);

        ///// <summary>
        ///// 产线
        ///// </summary>
        //public WipResource Resource
        //{
        //    get { return GetRefEntity(ResourceProperty); }
        //    set { SetRefEntity(ResourceProperty, value); }
        //}
        #endregion

        #region 产线名称 ResourceName
        /// <summary>
        /// 产线名称
        /// </summary>
        [Label("产线名称")]
        public static readonly Property<string> ResourceNameProperty = P<AddCheckPlanViewModel>.Register(e => e.ResourceName);

        /// <summary>
        /// 产线名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
            set { this.SetProperty(ResourceNameProperty, value); }
        }
        #endregion

    }
}
