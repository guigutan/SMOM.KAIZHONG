using SIE.Domain;
using SIE.EMS.Equipments.Models;
using SIE.Equipments.EquipAccounts;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Equipments.Accounts
{
    /// <summary>
    /// 设备台账
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备台账")]
    [CompiledPropertyDeclarer]
    public static class EquipAccountExtension
    {
        #region EquipParam EquipParamList (仪器参数)
        /// <summary>
        /// 仪器参数 扩展属性。
        /// </summary>
        [Label("仪器参数")]
        public static readonly ListProperty<EntityList<EquipParam>> EquipParamListProperty =
           P<EquipAccount>.RegisterExtensionList<EntityList<EquipParam>>("EquipParamList", typeof(EquipAccountExtension));

        /// <summary>
        /// 获取 仪器参数 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        /// <returns>仪器参数</returns>
        public static EntityList<EquipParam> GetEquipParamList(Entity me)
        {
            return me.GetProperty(EquipParamListProperty);
        }

        /// <summary>
        /// 设置仪器参数 属性的值。
        /// </summary>
        /// <param name="me">要设置扩展属性值的对象。</param>
        /// <param name="value">值</param>
        public static void SetEquipParamList(Entity me, EntityList<EquipParam> value)
        {
            me.SetProperty(EquipParamListProperty, value);
        }
        #endregion

        #region ProjectDetail LubricationProjectList (润滑项目列表)
        /// <summary>
        /// 润滑项目列表 扩展属性。
        /// </summary>
        [Label("润滑项目列表")]
        public static readonly ListProperty<EntityList<EquipAccountLubricationProject>> LubricationProjectListProperty =
           P<EquipAccount>.RegisterExtensionList<EntityList<EquipAccountLubricationProject>>("LubricationProjectList", typeof(EquipAccountExtension));

        /// <summary>
        /// 获取 润滑项目列表 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        /// <returns>润滑项目列表</returns>
        public static EntityList<EquipAccountLubricationProject> GetLubricationProjectList(EquipAccount me)
        {
            return me.GetProperty(LubricationProjectListProperty);
        }

        /// <summary>
        /// 设置润滑项目列表 属性的值。
        /// </summary>
        /// <param name="me">要设置扩展属性值的对象。</param>
        /// <param name="value">值</param>
        public static void SetLubricationProjectList(EquipAccount me, EntityList<EquipAccountLubricationProject> value)
        {
            me.SetProperty(LubricationProjectListProperty, value);
        }
        #endregion

        #region TechParameter TechParameterList (技术参数列表)
        /// <summary>
        /// 技术参数列表 扩展属性。
        /// </summary>
        [Label("技术参数列表")]
        public static readonly ListProperty<EntityList<EquipModelTechParameter>> TechParameterListProperty =
           P<EquipAccount>.RegisterExtensionList<EntityList<EquipModelTechParameter>>("TechParameterList", typeof(EquipAccountExtension));

        /// <summary>
        /// 获取 技术参数列表 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        /// <returns>技术参数列表</returns>
        public static EntityList<EquipModelTechParameter> GetTechParameterListProperty(EquipAccount me)
        {
            return me.GetProperty(TechParameterListProperty);
        }

        /// <summary>
        /// 设置技术参数列表 属性的值。
        /// </summary>
        /// <param name="me">要设置扩展属性值的对象。</param>
        /// <param name="value">值</param>
        public static void SetTechParameterListProperty(EquipAccount me, EntityList<EquipModelLubricationProject> value)
        {
            me.SetProperty(TechParameterListProperty, value);
        }
        #endregion

        #region EquipAccountCheckProject CheckProjectList (点检项目列表)
        /// <summary>
        /// 点检项目列表 扩展属性。
        /// </summary>
        [Label("点检项目列表")]
        public static readonly ListProperty<EntityList<EquipAccountCheckProject>> CheckProjectListProperty =
           P<EquipAccount>.RegisterExtensionList<EntityList<EquipAccountCheckProject>>("CheckProjectList",
               typeof(EquipAccountExtension));

        /// <summary>
        /// 获取 点检项目列表 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        /// <returns>点检项目列表</returns>
        public static EntityList<EquipAccountCheckProject> GetCheckProjectList(EquipAccount me)
        {
            return me.GetProperty(CheckProjectListProperty);
        }

        /// <summary>
        /// 设置点检项目列表 属性的值。
        /// </summary>
        /// <param name="me">要设置扩展属性值的对象。</param>
        /// <param name="value">值</param>
        public static void SetCheckProjectList(EquipAccount me, EntityList<EquipAccountCheckProject> value)
        {
            me.SetProperty(CheckProjectListProperty, value);
        }
        #endregion

        #region EquipAccountMaintainProject MaintainProjectList (保养项目列表)
        /// <summary>
        /// 保养项目列表 扩展属性。
        /// </summary>
        [Label("保养项目列表")]
        public static readonly ListProperty<EntityList<EquipAccountMaintainProject>> MaintainProjectListProperty =
           P<EquipAccount>.RegisterExtensionList<EntityList<EquipAccountMaintainProject>>("MaintainProjectList",
               typeof(EquipAccountExtension));

        /// <summary>
        /// 获取 保养项目列表 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        /// <returns>保养项目列表</returns>
        public static EntityList<EquipAccountMaintainProject> GetMaintainProjectList(EquipAccount me)
        {
            return me.GetProperty(MaintainProjectListProperty);
        }

        /// <summary>
        /// 设置保养项目列表 属性的值。
        /// </summary>
        /// <param name="me">要设置扩展属性值的对象。</param>
        /// <param name="value">值</param>
        public static void SetMaintainProjectList(EquipAccount me, EntityList<EquipAccountMaintainProject> value)
        {
            me.SetProperty(MaintainProjectListProperty, value);
        }
        #endregion

        #region SpPartRecord (备件记录)
        /// <summary>
        /// 
        /// </summary>
        public static readonly IRefIdProperty SpPartRecordIdProperty
            = P<EquipAccount>.RegisterExtensionRefId<double>("SpPartRecordId",
                typeof(EquipAccountExtension), ReferenceType.Normal);
        /// <summary>
        /// 
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSpPartRecord> SpPartRecordProperty
            = P<EquipAccount>.RegisterExtensionRef<EquipAccountSpPartRecord>("SpPartRecord",
                typeof(EquipAccountExtension), SpPartRecordIdProperty);

        /// <summary>
        /// 获取SpPartRecordId
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double? GetSpPartRecordId(this EquipAccount obj)
        {
            return (double?)obj.GetRefNullableId(SpPartRecordIdProperty);
        }

        /// <summary>
        /// 获取SpPartRecord
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static EquipAccountSpPartRecord GetSpPartRecord(this EquipAccount obj)
        {
            return obj.GetRefEntity(SpPartRecordProperty);
        }

        /// <summary>
        /// 设置SpPartRecordId
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void SetSpPartRecordId(this EquipAccount obj, double? value)
        {
            obj.SetRefNullableId(SpPartRecordIdProperty, value);
        }

        /// <summary>
        /// 设置SpPartRecord
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void SetSpPartRecord(this EquipAccount obj, EquipAccountSpPartRecord value)
        {
            obj.SetRefEntity(SpPartRecordProperty, value);
        }
        #endregion

        #region CheckMgt (设备台账点检管理)
        /// <summary>
        /// 设备台账点检管理
        /// </summary>
        public static readonly IRefIdProperty CheckMgtIdProperty
            = P<EquipAccount>.RegisterExtensionRefId<double>("CheckMgtId",
                typeof(EquipAccountExtension), ReferenceType.Normal);

        /// <summary>
        /// 设备台账点检管理
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountCheckMgt> CheckMgtProperty
            = P<EquipAccount>.RegisterExtensionRef<EquipAccountCheckMgt>("CheckMgt",
                typeof(EquipAccountExtension), CheckMgtIdProperty);

        /// <summary>
        /// 获取CheckMgtId
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double? GetCheckMgtId(this EquipAccount obj)
        {
            return (double?)obj.GetRefNullableId(CheckMgtIdProperty);
        }

        /// <summary>
        /// 获取CheckMgt
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static EquipAccountCheckMgt GetCheckMgt(this EquipAccount obj)
        {
            return obj.GetRefEntity(CheckMgtProperty);
        }

        /// <summary>
        /// 设置CheckMgtId
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void SetCheckMgtId(this EquipAccount obj, double? value)
        {
            obj.SetRefNullableId(CheckMgtIdProperty, value);
        }

        /// <summary>
        /// 设置CheckMgt
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void SetCheckMgt(this EquipAccount obj, EquipAccountCheckMgt value)
        {
            obj.SetRefEntity(CheckMgtProperty, value);
        }
        #endregion

        #region TpmMgt (设备台账TPM管理)
        /// <summary>
        /// 设备台账TPM管理
        /// </summary>
        public static readonly IRefIdProperty TpmMgtIdProperty
            = P<EquipAccount>.RegisterExtensionRefId<double>("TpmMgtId",
                typeof(EquipAccountExtension), ReferenceType.Normal);

        /// <summary>
        /// 设备台账TPM管理
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountTpmMgt> TpmMgtProperty
            = P<EquipAccount>.RegisterExtensionRef<EquipAccountTpmMgt>("TpmMgt",
                typeof(EquipAccountExtension), TpmMgtIdProperty);

        /// <summary>
        /// 获取TpmMgtId
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double? GetTpmMgtId(this EquipAccount obj)
        {
            return (double?)obj.GetRefNullableId(TpmMgtIdProperty);
        }

        /// <summary>
        /// 获取TpmMgt
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static EquipAccountTpmMgt GetTpmMgt(this EquipAccount obj)
        {
            return obj.GetRefEntity(TpmMgtProperty);
        }

        /// <summary>
        /// 设置TpmMgtId
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void SetTpmMgtId(this EquipAccount obj, double? value)
        {
            obj.SetRefNullableId(TpmMgtIdProperty, value);
        }

        /// <summary>
        /// 设置TpmMgt
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void SetTpmMgt(this EquipAccount obj, EquipAccountTpmMgt value)
        {
            obj.SetRefEntity(TpmMgtProperty, value);
        }
        #endregion

        #region int? RepairStateDontMap (维修单状态)
        /// <summary>
        /// 维修单状态 扩展属性。
        /// </summary>
        public static readonly Property<int?> RepairStateDontMapProperty =
            P<EquipAccount>.RegisterExtension<int?>("RepairStateDontMap", typeof(EquipAccountExtension));

        /// <summary>
        /// 获取 维修单状态 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        public static int? GetRepairStateDontMap(this Entity me)
        {
            return me.GetProperty(RepairStateDontMapProperty);
        }

        /// <summary>
        /// 设置 维修单状态 属性的值。
        /// </summary>
        /// <param name="me">要设置扩展属性值的对象。</param>
        /// <param name="value">设置的值。</param>
        public static void SetRepairStateDontMap(this Entity me, int? value)
        {
            me.SetProperty(RepairStateDontMapProperty, value);
        }
        #endregion

        #region int? ResumeStateDontMap (设备履历状态)
        /// <summary>
        /// 设备履历状态 扩展属性。
        /// </summary>
        public static readonly Property<int?> ResumeStateDontMapProperty =
            P<EquipAccount>.RegisterExtension<int?>("ResumeStateDontMap", typeof(EquipAccountExtension));

        /// <summary>
        /// 获取 设备履历状态 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        public static int? GetResumeStateDontMap(this Entity me)
        {
            return me.GetProperty(ResumeStateDontMapProperty);
        }

        /// <summary>
        /// 设置 设备履历状态 属性的值。
        /// </summary>
        /// <param name="me">要设置扩展属性值的对象。</param>
        /// <param name="value">设置的值。</param>
        public static void SetResumeStateDontMap(this Entity me, int? value)
        {
            me.SetProperty(ResumeStateDontMapProperty, value);
        }
        #endregion

        #region EquipAccountRepairStandard EquipAccountRepairStandardList (维修定标)
        /// <summary>
        /// 维修定标 扩展属性。
        /// </summary>
        [Label("维修定标")]
        public static readonly ListProperty<EntityList<EquipAccountRepairStandard>> EquipAccountRepairStandardListProperty =
           P<EquipAccount>.RegisterExtensionList<EntityList<EquipAccountRepairStandard>>("EquipAccountRepairStandardList", typeof(EquipAccountExtension));

        /// <summary>
        /// 获取 维修定标 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        /// <returns>维修定标</returns>
        public static EntityList<EquipAccountRepairStandard> GetEquipAccountRepairStandardList(EquipAccount me)
        {
            return me.GetProperty(EquipAccountRepairStandardListProperty);
        }

        /// <summary>
        /// 设置维修定标 属性的值。
        /// </summary>
        /// <param name="me">要设置扩展属性值的对象。</param>
        /// <param name="value">值</param>
        public static void SetEquipAccountRepairStandardList(EquipAccount me, EntityList<EquipAccountRepairStandard> value)
        {
            me.SetProperty(EquipAccountRepairStandardListProperty, value);
        }
        #endregion
    }


    /// <summary>
    /// 设备型号维护 实体配置
    /// </summary>
    internal class EquipModelConfig : EntityConfig<EquipAccount>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_ACCOUNT").MapAllProperties();
            Meta.SupportTree();
            Meta.Property(EquipAccount.CodeProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
            Meta.Property(EquipAccountExtension.RepairStateDontMapProperty).DontMapColumn();
            Meta.Property(EquipAccountExtension.TpmMgtIdProperty).DontMapColumn();
            Meta.Property(EquipAccountExtension.CheckMgtIdProperty).DontMapColumn();
            Meta.Property(EquipAccountExtension.SpPartRecordIdProperty).DontMapColumn();
            Meta.Property(EquipAccount.ScrapTypeProperty).DontMapColumn();
            Meta.Property(EquipAccount.ReasonProperty).DontMapColumn();
            Meta.Property(EquipAccount.IsCalibrationProperty).DontMapColumn();
        }
    }
}