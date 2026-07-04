using SIE.Domain;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.Equipments.Models;
using SIE.Equipments.EquipAccounts;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts
{
    /// <summary>
    /// 计量设备台账
    /// </summary>
    [RootEntity, Serializable]
    [Label("计量设备台账")]
    [CompiledPropertyDeclarer]
    public static class MeteringEquipmentAccountExtension
    {
        #region EntityList<EquipParam> MeteringEquipParamList (仪器参数)
        /// <summary>
        /// 仪器参数 扩展属性。
        /// </summary>
        public static readonly ListProperty<EntityList<MeteringEquipParam>> MeteringEquipParamListProperty =
            P<MeteringEquipmentAccount>.RegisterExtensionList<EntityList<MeteringEquipParam>>("MeteringEquipParamList", typeof(MeteringEquipmentAccountExtension));

        /// <summary>
        /// 获取 仪器参数 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        public static EntityList<MeteringEquipParam> GetMeteringEquipParamList(this MeteringEquipmentAccount me)
        {
            return me.GetProperty(MeteringEquipParamListProperty);
        }
        #endregion

        #region EntityList<EquipAccountLubricationProject> MeteringLubricationProjectList (润滑项目列表)
        /// <summary>
        /// 润滑项目列表 扩展属性。
        /// </summary>
        public static readonly ListProperty<EntityList<MeteringEquipAccountLubricationProject>> MeteringLubricationProjectListProperty =
            P<MeteringEquipmentAccount>.RegisterExtensionList<EntityList<MeteringEquipAccountLubricationProject>>("MeteringLubricationProjectList", typeof(MeteringEquipmentAccountExtension));

        /// <summary>
        /// 获取 润滑项目列表 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        public static EntityList<MeteringEquipAccountLubricationProject> GetMeteringLubricationProjectList(this MeteringEquipmentAccount me)
        {
            return me.GetProperty(MeteringLubricationProjectListProperty);
        }
        #endregion

        #region EntityList<EquipModelTechParameter> MeteringTechParameterList (技术参数列表)
        /// <summary>
        /// 技术参数列表 扩展属性。
        /// </summary>
        public static readonly ListProperty<EntityList<EquipModelTechParameter>> MeteringTechParameterListProperty =
            P<MeteringEquipmentAccount>.RegisterExtensionList<EntityList<EquipModelTechParameter>>("MeteringTechParameterList", typeof(MeteringEquipmentAccountExtension));

        /// <summary>
        /// 获取 技术参数列表 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        public static EntityList<EquipModelTechParameter> GetMeteringTechParameterList(this MeteringEquipmentAccount me)
        {
            return me.GetProperty(MeteringTechParameterListProperty);
        }
        #endregion

        #region EntityList<EquipAccountCheckProject> MeteringCheckProjectList (点检项目列表)
        /// <summary>
        /// 点检项目列表 扩展属性。
        /// </summary>
        public static readonly ListProperty<EntityList<MeteringEquipAccountCheckProject>> MeteringCheckProjectListProperty =
            P<MeteringEquipmentAccount>.RegisterExtensionList<EntityList<MeteringEquipAccountCheckProject>>("MeteringCheckProjectList", typeof(MeteringEquipmentAccountExtension));

        /// <summary>
        /// 获取 点检项目列表 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        public static EntityList<MeteringEquipAccountCheckProject> GetMeteringCheckProjectList(this MeteringEquipmentAccount me)
        {
            return me.GetProperty(MeteringCheckProjectListProperty);
        }
        #endregion

        #region EntityList<EquipAccountMaintainProject> MeteringMaintainProjectList (保养项目列表)
        /// <summary>
        /// 保养项目列表 扩展属性。
        /// </summary>
        public static readonly ListProperty<EntityList<MeteringEquipAccountMaintainProject>> MeteringMaintainProjectListProperty =
            P<MeteringEquipmentAccount>.RegisterExtensionList<EntityList<MeteringEquipAccountMaintainProject>>("MeteringMaintainProjectList", typeof(MeteringEquipmentAccountExtension));

        /// <summary>
        /// 获取 保养项目列表 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        public static EntityList<MeteringEquipAccountMaintainProject> GetMeteringMaintainProjectList(this MeteringEquipmentAccount me)
        {
            return me.GetProperty(MeteringMaintainProjectListProperty);
        }
        #endregion

        #region EntityList<EquipAccountRepairStandard> MeteringEquipAccountRepairStandardList (维修定标)
        /// <summary>
        /// 维修定标 扩展属性。
        /// </summary>
        public static readonly ListProperty<EntityList<MeteringEquipAccountRepairStandard>> MeteringEquipAccountRepairStandardListProperty =
            P<MeteringEquipmentAccount>.RegisterExtensionList<EntityList<MeteringEquipAccountRepairStandard>>("MeteringEquipAccountRepairStandardList", typeof(MeteringEquipmentAccountExtension));

        /// <summary>
        /// 获取 维修定标 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        public static EntityList<MeteringEquipAccountRepairStandard> GetMeteringEquipAccountRepairStandardList(this MeteringEquipmentAccount me)
        {
            return me.GetProperty(MeteringEquipAccountRepairStandardListProperty);
        }
        #endregion

        #region MeteringSpPartRecord (备件记录)

        /// <summary>
        /// 
        /// </summary>
        public static readonly IRefIdProperty MeteringSpPartRecordIdProperty = P<MeteringEquipmentAccount>.RegisterExtensionRefId<double>("MeteringSpPartRecordId", typeof(MeteringEquipmentAccountExtension), ReferenceType.Normal);

        /// <summary>
        /// 
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSpPartRecord> MeteringSpPartRecordProperty = P<MeteringEquipmentAccount>.RegisterExtensionRef<EquipAccountSpPartRecord>("MeteringSpPartRecord", typeof(MeteringEquipmentAccountExtension), MeteringSpPartRecordIdProperty);

        /// <summary>
        /// 获取MeteringSpPartRecordId
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double? GetMeteringSpPartRecordId(this MeteringEquipmentAccount obj)
        {
            return (double?)obj.GetRefNullableId(MeteringSpPartRecordIdProperty);
        }

        /// <summary>
        /// 获取MeteringSpPartRecord
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static EquipAccountSpPartRecord GetMeteringSpPartRecord(this MeteringEquipmentAccount obj)
        {
            return obj.GetRefEntity(MeteringSpPartRecordProperty);
        }

        /// <summary>
        /// 设置MeteringSpPartRecordId
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void SetMeteringSpPartRecordId(this MeteringEquipmentAccount obj, double? value)
        {
            obj.SetRefNullableId(MeteringSpPartRecordIdProperty, value);
        }

        /// <summary>
        /// 设置MeteringSpPartRecord
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void SetMeteringSpPartRecord(this MeteringEquipmentAccount obj, EquipAccountSpPartRecord value)
        {
            obj.SetRefEntity(MeteringSpPartRecordProperty, value);
        }
        #endregion

        #region MeteringCheckMgt (设备台账点检管理)
        /// <summary>
        /// 
        /// </summary>
        public static readonly IRefIdProperty MeteringCheckMgtIdProperty = P<MeteringEquipmentAccount>.RegisterExtensionRefId<double>("MeteringCheckMgtId", typeof(MeteringEquipmentAccountExtension), ReferenceType.Normal);

        /// <summary>
        /// 
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountCheckMgt> MeteringCheckMgtProperty = P<MeteringEquipmentAccount>.RegisterExtensionRef<EquipAccountCheckMgt>("MeteringCheckMgt", typeof(MeteringEquipmentAccountExtension), MeteringCheckMgtIdProperty);

        /// <summary>
        /// 获取MeteringCheckMgtId
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double? GetMeteringCheckMgtId(this MeteringEquipmentAccount obj)
        {
            return (double?)obj.GetRefNullableId(MeteringCheckMgtIdProperty);
        }

        /// <summary>
        /// 获取MeteringCheckMgt
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static EquipAccountCheckMgt GetMeteringCheckMgt(this MeteringEquipmentAccount obj)
        {
            return obj.GetRefEntity(MeteringCheckMgtProperty);
        }

        /// <summary>
        /// 设置MeteringCheckMgtId
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void SetMeteringCheckMgtId(this MeteringEquipmentAccount obj, double? value)
        {
            obj.SetRefNullableId(MeteringCheckMgtIdProperty, value);
        }

        /// <summary>
        /// 设置MeteringCheckMgt
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void SetMeteringCheckMgt(this MeteringEquipmentAccount obj, EquipAccountCheckMgt value)
        {
            obj.SetRefEntity(MeteringCheckMgtProperty, value);
        }
        #endregion

        #region MeteringTpmMgt (设备台账TPM管理)
        /// <summary>
        /// 
        /// </summary>
        public static readonly IRefIdProperty MeteringTpmMgtIdProperty = P<MeteringEquipmentAccount>.RegisterExtensionRefId<double>("MeteringTpmMgtId", typeof(MeteringEquipmentAccountExtension), ReferenceType.Normal);

        /// <summary>
        /// 
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountTpmMgt> MeteringTpmMgtProperty = P<MeteringEquipmentAccount>.RegisterExtensionRef<EquipAccountTpmMgt>("MeteringTpmMgt", typeof(MeteringEquipmentAccountExtension), MeteringTpmMgtIdProperty);

        /// <summary>
        /// 获取MeteringTpmMgtId
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double? GetMeteringTpmMgtId(this MeteringEquipmentAccount obj)
        {
            return (double?)obj.GetRefNullableId(MeteringTpmMgtIdProperty);
        }

        /// <summary>
        /// 获取MeteringTpmMgt
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static EquipAccountTpmMgt GetMeteringTpmMgt(this MeteringEquipmentAccount obj)
        {
            return obj.GetRefEntity(MeteringTpmMgtProperty);
        }

        /// <summary>
        /// 设置MeteringTpmMgtId
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void SetMeteringTpmMgtId(this MeteringEquipmentAccount obj, double? value)
        {
            obj.SetRefNullableId(MeteringTpmMgtIdProperty, value);
        }

        /// <summary>
        /// 设置MeteringTpmMgt
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void SetMeteringTpmMgt(this MeteringEquipmentAccount obj, EquipAccountTpmMgt value)
        {
            obj.SetRefEntity(MeteringTpmMgtProperty, value);
        }
        #endregion

        #region int? MeteringRepairStateDontMap (维修单状态)
        /// <summary>
        /// 维修单状态 扩展属性。
        /// </summary>
        public static readonly Property<int?> MeteringRepairStateDontMapProperty =
            P<MeteringEquipmentAccount>.RegisterExtension<int?>("MeteringRepairStateDontMap", typeof(MeteringEquipmentAccountExtension));

        /// <summary>
        /// 获取 维修单状态 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        public static int? GetMeteringRepairStateDontMap(this MeteringEquipmentAccount me)
        {
            return me.GetProperty(MeteringRepairStateDontMapProperty);
        }

        /// <summary>
        /// 设置 维修单状态 属性的值。
        /// </summary>
        /// <param name="me">要设置扩展属性值的对象。</param>
        /// <param name="value">设置的值。</param>
        public static void SetMeteringRepairStateDontMap(this MeteringEquipmentAccount me, int? value)
        {
            me.SetProperty(MeteringRepairStateDontMapProperty, value);
        }
        #endregion

        #region int? MeteringResumeStateDontMap (设备履历状态)
        /// <summary>
        /// 设备履历状态 扩展属性。
        /// </summary>
        public static readonly Property<int?> MeteringResumeStateDontMapProperty =
            P<MeteringEquipmentAccount>.RegisterExtension<int?>("MeteringResumeStateDontMap", typeof(MeteringEquipmentAccountExtension));

        /// <summary>
        /// 获取 设备履历状态 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        public static int? GetMeteringResumeStateDontMap(this MeteringEquipmentAccount me)
        {
            return me.GetProperty(MeteringResumeStateDontMapProperty);
        }

        /// <summary>
        /// 设置 设备履历状态 属性的值。
        /// </summary>
        /// <param name="me">要设置扩展属性值的对象。</param>
        /// <param name="value">设置的值。</param>
        public static void SetMeteringResumeStateDontMap(this MeteringEquipmentAccount me, int? value)
        {
            me.SetProperty(MeteringResumeStateDontMapProperty, value);
        }
        #endregion
    }

    /// <summary>
    /// 设备型号维护 实体配置
    /// </summary>
    internal class MeteringEquipmentAccountExtensionConfig : EntityConfig<MeteringEquipmentAccount>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_ACCOUNT").MapAllProperties();
            Meta.Property(MeteringEquipmentAccount.CodeProperty).ColumnMeta.HasIndex();
            Meta.Property(MeteringEquipmentAccount.FactoryIdProperty).ColumnMeta.HasIndex();
            Meta.Property(MeteringEquipmentAccount.ManageDepartmentIdProperty).ColumnMeta.HasIndex();
            Meta.Property(MeteringEquipmentAccount.UseDepartmentIdProperty).ColumnMeta.HasIndex();
            Meta.Property(MeteringEquipmentAccount.EquipModelIdProperty).ColumnMeta.HasIndex();
            Meta.Property(MeteringEquipmentAccount.ScrapTypeProperty).DontMapColumn();
            Meta.Property(MeteringEquipmentAccount.ReasonProperty).DontMapColumn();
            Meta.Property(MeteringEquipmentAccount.IsCalibrationProperty).DontMapColumn();

            Meta.Property(MeteringEquipmentAccountExtension.MeteringRepairStateDontMapProperty).DontMapColumn();
            Meta.Property(MeteringEquipmentAccountExtension.MeteringResumeStateDontMapProperty).DontMapColumn();
            Meta.Property(MeteringEquipmentAccountExtension.MeteringTpmMgtIdProperty).DontMapColumn();
            Meta.Property(MeteringEquipmentAccountExtension.MeteringCheckMgtIdProperty).DontMapColumn();
            Meta.Property(MeteringEquipmentAccountExtension.MeteringSpPartRecordIdProperty).DontMapColumn();
            Meta.SupportTree();
            Meta.EnablePhantoms();
        }
    }
}
