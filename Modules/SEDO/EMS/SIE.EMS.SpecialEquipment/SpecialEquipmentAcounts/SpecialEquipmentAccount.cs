using SIE.Common.Configs;
using SIE.DataAuth;
using SIE.Domain;
using SIE.EMS.SpecialEquipment.SpecialEquipmentAcounts.Tab;
using SIE.Equipments.Configs;
using SIE.Equipments.DataAuth;
using SIE.Equipments.DeviceControls.Configs;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.EMS.SpecialEquipment.SpecialEquipmentAcounts
{
    /// <summary>
    /// 特种设备台账
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember(nameof(Code))]
    [EntityWithConfig(typeof(AccountNoConfig))]
    [EntityWithConfig(typeof(SmdcUrlConfig))]
    [ConditionQueryType(typeof(SpecialEquipmentAccountCriteria))]
    [EquipAccountAuth(nameof(EquipModelId), nameof(UseDepartmentId), true)]
    [EntityDataAuth(typeof(EmployeeEnterprise), nameof(FactoryId), true)]
    [Label("特种设备台账")]
    public partial class SpecialEquipmentAccount : EquipAccount
    {
        #region 设备定检规程列表 EquipAccountRegularInspectionList
        /// <summary>
        /// 设备定检规程列表
        /// </summary>
        public static readonly ListProperty<EntityList<EquipAccountRegularInspection>> EquipAccountRegularInspectionListProperty = P<SpecialEquipmentAccount>.RegisterList(e => e.EquipAccountRegularInspectionList, new ListPropertyMeta()
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => (e as SpecialEquipmentAccount).LoadEquipAccountRegularInspectionList()
        });
        private EntityList<EquipAccountRegularInspection> LoadEquipAccountRegularInspectionList()
        {
            return new EntityList<EquipAccountRegularInspection>();
        }       /// <summary>
                /// 设备定检规程列表
                /// </summary>
        public EntityList<EquipAccountRegularInspection> EquipAccountRegularInspectionList
        {
            get { return this.GetLazyList(EquipAccountRegularInspectionListProperty); }
        }
        #endregion

        #region  基础子标签New新集合

        #region 缸槽列表 PcbSlotList
        /// <summary>
        /// 缸槽列表
        /// </summary>
        public static new readonly ListProperty<EntityList<SpecialEquipAccountSlot>> PcbSlotListProperty = P<SpecialEquipmentAccount>.RegisterList(e => e.PcbSlotList);
        /// <summary>
        /// 缸槽列表
        /// </summary>
        public new EntityList<SpecialEquipAccountSlot> PcbSlotList
        {
            get { return this.GetLazyList(PcbSlotListProperty); }
        }
        #endregion

        #region 设备履历列表 ResumeList
        /// <summary>
        /// 设备履历列表
        /// </summary>
        [Label("设备履历列表")]
        public static new readonly ListProperty<EntityList<SpecialEquipAccountResume>> ResumeListProperty
            = P<SpecialEquipmentAccount>.RegisterList(e => e.ResumeList);

        /// <summary>
        /// 设备履历列表
        /// </summary>
        public new EntityList<SpecialEquipAccountResume> ResumeList
        {
            get { return this.GetLazyList(ResumeListProperty); }
        }
        #endregion

        #region 工序列表 ProcessList
        /// <summary>
        /// 工序列表
        /// </summary>
        [Label("工序列表")]
        public static new readonly ListProperty<EntityList<SpecialEquipAccountProcess>> ProcessListProperty = P<SpecialEquipmentAccount>.RegisterList(e => e.ProcessList);

        /// <summary>
        /// 工序列表
        /// </summary>
        public new EntityList<SpecialEquipAccountProcess> ProcessList
        {
            get { return this.GetLazyList(ProcessListProperty); }
        }
        #endregion

        #region 设备位置列表 EquipAccountLocationList
        /// <summary>
        /// 设备位置列表
        /// </summary>
        [Label("设备位置列表")]
        public static new readonly ListProperty<EntityList<SpecialEquipAccountLocation>> EquipAccountLocationListProperty
            = P<SpecialEquipmentAccount>.RegisterList(e => e.EquipAccountLocationList);

        /// <summary>
        /// 设备位置列表
        /// </summary>
        public new EntityList<SpecialEquipAccountLocation> EquipAccountLocationList
        {
            get { return this.GetLazyList(EquipAccountLocationListProperty); }
        }
        #endregion

        #region 设备物联列表 EquipAccountPhysicalUnionList
        /// <summary>
        /// 设备物联列表
        /// </summary>
        [Label("设备物联列表")]
        public static new readonly ListProperty<EntityList<SpecialEquipAccountPhysicalUnion>> EquipAccountPhysicalUnionListProperty = P<SpecialEquipmentAccount>.RegisterList(e => e.EquipAccountPhysicalUnionList);

        /// <summary>
        /// 设备物联列表
        /// </summary>
        public new EntityList<SpecialEquipAccountPhysicalUnion> EquipAccountPhysicalUnionList
        {
            get { return this.GetLazyList(EquipAccountPhysicalUnionListProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 特种设备台账 实体配置
    /// </summary>
    internal class SpecialEquipmentAccountConfig : EntityConfig<SpecialEquipmentAccount>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_ACCOUNT").MapAllProperties();
            Meta.Property(SpecialEquipmentAccount.CodeProperty).ColumnMeta.HasIndex();
            Meta.Property(SpecialEquipmentAccount.ScrapTypeProperty).DontMapColumn();
            Meta.Property(SpecialEquipmentAccount.ReasonProperty).DontMapColumn();
            Meta.Property(SpecialEquipmentAccount.IsCalibrationProperty).DontMapColumn();
            Meta.SupportTree();
            Meta.EnablePhantoms();
        }
    }
}