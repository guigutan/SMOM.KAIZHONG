using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using System;

namespace SIE.Kit.MES.CallMaterials
{
    /// <summary>
    /// 工单叫料设置
    /// </summary>
    [RootEntity, Serializable]
    [Label("工单叫料设置")]
    public partial class CallMaterialSetting : DataEntity
    {
        #region 自动叫料 IsAuto
        /// <summary>
        /// 自动叫料
        /// </summary>
        [Label("自动叫料")]
        public static readonly Property<bool> IsAutoProperty = P<CallMaterialSetting>.Register(e => e.IsAuto);

        /// <summary>
        /// 自动叫料
        /// </summary>
        public bool IsAuto
        {
            get { return GetProperty(IsAutoProperty); }
            set { SetProperty(IsAutoProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<CallMaterialSetting>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 当前资源Id
        /// </summary>
        public double ResourceId
        {
            get { return (double)GetRefId(ResourceIdProperty); }
            set { SetRefId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<CallMaterialSetting>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 工单叫料设置 实体配置
    /// </summary>
    internal class CallMaterialSettingConfig : EntityConfig<CallMaterialSetting>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_CALL_SET").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}