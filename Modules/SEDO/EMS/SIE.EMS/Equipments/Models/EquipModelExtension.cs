using SIE.Domain;
using SIE.Equipments.EquipModels;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Equipments.Models
{
    /// <summary>
	/// 设备型号扩展
	/// </summary>
	[RootEntity, Serializable]
    [Label("设备型号扩展")]
    [CompiledPropertyDeclarer]
    public static class EquipModelExtension
    {
        #region EquipModelMaintainProject MaintainProjectList (保养项目列表)
        /// <summary>
        /// 保养项目列表 扩展属性。
        /// </summary>
        [Label("保养项目列表")]
        public static readonly ListProperty<EntityList<EquipModelMaintainProject>> MaintainProjectListProperty =
           P<EquipModel>.RegisterExtensionList<EntityList<EquipModelMaintainProject>>("MaintainProjectList", typeof(EquipModelExtension));

        /// <summary>
        /// 获取 保养项目列表 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        /// <returns>保养项目列表</returns>
        public static EntityList<EquipModelMaintainProject> GetMaintainProjectList(Entity me)
        {
            return me.GetProperty(MaintainProjectListProperty);
        }

        /// <summary>
        /// 设置保养项目列表 属性的值。
        /// </summary>
        /// <param name="me">要设置扩展属性值的对象。</param>
        /// <param name="value">值</param>
        public static void SetMaintainProjectList(Entity me, EntityList<EquipModelMaintainProject> value)
        {
            me.SetProperty(MaintainProjectListProperty, value);
        }
        #endregion

        #region EquipModelCheckProject CheckProjectList (点检项目列表)
        /// <summary>
        /// 点检项目列表 扩展属性。
        /// </summary>
        [Label("点检项目列表")]
        public static readonly ListProperty<EntityList<EquipModelCheckProject>> CheckProjectListProperty =
           P<EquipModel>.RegisterExtensionList<EntityList<EquipModelCheckProject>>("CheckProjectList", typeof(EquipModelExtension));

        /// <summary>
        /// 获取 点检项目列表 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        /// <returns>点检项目列表</returns>
        public static EntityList<EquipModelCheckProject> GetCheckProjectList(EquipModel me)
        {
            return me.GetProperty(CheckProjectListProperty);
        }

        /// <summary>
        /// 设置点检项目列表 属性的值。
        /// </summary>
        /// <param name="me">要设置扩展属性值的对象。</param>
        /// <param name="value">值</param>
        public static void SetCheckProjectList(EquipModel me, EntityList<EquipModelCheckProject> value)
        {
            me.SetProperty(CheckProjectListProperty, value);
        }
        #endregion

        #region EquipModelVerifyProject VerifyProjectList (校验项目列表)
        /// <summary>
        /// 校验项目列表 扩展属性。
        /// </summary>
        [Label("校验项目列表")]
        public static readonly ListProperty<EntityList<EquipModelVerifyProject>> VerifyProjectListProperty =
           P<EquipModel>.RegisterExtensionList<EntityList<EquipModelVerifyProject>>("VerifyProjectList", typeof(EquipModelExtension));

        /// <summary>
        /// 获取 校验项目列表 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        /// <returns>校验项目列表</returns>
        public static EntityList<EquipModelVerifyProject> GetVerifyProjectList(EquipModel me)
        {
            return me.GetProperty(VerifyProjectListProperty);
        }

        /// <summary>
        /// 设置校验项目列表 属性的值。
        /// </summary>
        /// <param name="me">要设置扩展属性值的对象。</param>
        /// <param name="value">值</param>
        public static void SetVerifyProjectList(EquipModel me, EntityList<EquipModelVerifyProject> value)
        {
            me.SetProperty(VerifyProjectListProperty, value);
        }
        #endregion

        #region ProjectDetail LubricationProjectList (润滑项目列表)
        /// <summary>
        /// 润滑项目列表 扩展属性。
        /// </summary>
        [Label("润滑项目列表")]
        public static readonly ListProperty<EntityList<EquipModelLubricationProject>> LubricationProjectListProperty =
           P<EquipModel>.RegisterExtensionList<EntityList<EquipModelLubricationProject>>("LubricationProjectList", typeof(EquipModelExtension));

        /// <summary>
        /// 获取 润滑项目列表 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        /// <returns>润滑项目列表</returns>
        public static EntityList<EquipModelLubricationProject> GetLubricationProjectList(EquipModel me)
        {
            return me.GetProperty(LubricationProjectListProperty);
        }

        /// <summary>
        /// 设置润滑项目列表 属性的值。
        /// </summary>
        /// <param name="me">要设置扩展属性值的对象。</param>
        /// <param name="value">值</param>
        public static void SetLubricationProjectList(EquipModel me, EntityList<EquipModelLubricationProject> value)
        {
            me.SetProperty(LubricationProjectListProperty, value);
        }
        #endregion

        #region EquipModelTechParameter EquipModelTechParameterList (技术参数列表)
        /// <summary>
        /// 技术参数列表 扩展属性。
        /// </summary>
        [Label("技术参数列表")]
        public static readonly ListProperty<EntityList<EquipModelTechParameter>> EquipModelTechParameterListProperty =
           P<EquipModel>.RegisterExtensionList<EntityList<EquipModelTechParameter>>("EquipModelTechParameterList", typeof(EquipModelExtension));

        /// <summary>
        /// 获取 技术参数列表 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        /// <returns>技术参数列表</returns>
        public static EntityList<EquipModelTechParameter> GetEquipModelTechParameterList(EquipModel me)
        {
            return me.GetProperty(EquipModelTechParameterListProperty);
        }

        /// <summary>
        /// 设置技术参数列表 属性的值。
        /// </summary>
        /// <param name="me">要设置扩展属性值的对象。</param>
        /// <param name="value">值</param>
        public static void SetEquipModelTechParameterList(EquipModel me, EntityList<EquipModelLubricationProject> value)
        {
            me.SetProperty(EquipModelTechParameterListProperty, value);
        }
        #endregion

        #region EquipModelRepairProject EquipModelRepairProjectList (维修项目列表)
        /// <summary>
        /// 维修项目列表 扩展属性。
        /// </summary>
        [Label("维修项目列表")]
        public static readonly ListProperty<EntityList<EquipModelRepairProject>> EquipModelRepairProjectListProperty =
           P<EquipModel>.RegisterExtensionList<EntityList<EquipModelRepairProject>>("EquipModelRepairProjectList", typeof(EquipModelExtension));

        /// <summary>
        /// 获取 维修项目列表 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        /// <returns>维修项目列表</returns>
        public static EntityList<EquipModelRepairProject> GetEquipModelRepairProjectList(Entity me)
        {
            return me.GetProperty(EquipModelRepairProjectListProperty);
        }

        /// <summary>
        /// 设置维修项目列表 属性的值。
        /// </summary>
        /// <param name="me">要设置扩展属性值的对象。</param>
        /// <param name="value">值</param>
        public static void SetEquipModelRepairProjectList(Entity me, EntityList<EquipModelRepairProject> value)
        {
            me.SetProperty(EquipModelRepairProjectListProperty, value);
        }
        #endregion
    }

    /// <summary>
    /// 设备型号维护 实体配置
    /// </summary>
    internal class EquipModelConfig : EntityConfig<EquipModel>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQUIP_MODEL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}