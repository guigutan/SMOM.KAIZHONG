using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using System;

namespace SIE.MES.DashBoard.Reports.FpySettings
{
    /// <summary>
    /// 产线直通率设置
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产线直通率设置")]
    public partial class LineFpySetting : FpySetting
    {
        #region 资源 Line
        /// <summary>
        /// 资源Id
        /// </summary>
        public static readonly IRefIdProperty ResourceIdProperty = P<LineFpySetting>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double ResourceId
        {
            get { return (double)GetRefId(ResourceIdProperty); }
            set { SetRefId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> LineProperty = P<LineFpySetting>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(LineProperty); }
            set { SetRefEntity(LineProperty, value); }
        }
        #endregion

        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> ResourceNameProperty = P<LineFpySetting>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion

        #region 资源直通率设置 ShopFpySetting
        /// <summary>
        /// 资源直通率设置Id
        /// </summary>
        public static readonly IRefIdProperty ShopFpySettingIdProperty = P<LineFpySetting>.RegisterRefId(e => e.ShopFpySettingId, ReferenceType.Parent);

        /// <summary>
        /// 资源直通率设置Id
        /// </summary>
        public double ShopFpySettingId
        {
            get { return (double)GetRefId(ShopFpySettingIdProperty); }
            set { SetRefId(ShopFpySettingIdProperty, value); }
        }

        /// <summary>
        /// 资源直通率设置
        /// </summary>
        public static readonly RefEntityProperty<ShopFpySetting> ShopFpySettingProperty = P<LineFpySetting>.RegisterRef(e => e.ShopFpySetting, ShopFpySettingIdProperty);

        /// <summary>
        /// 资源直通率设置
        /// </summary>
        public ShopFpySetting ShopFpySetting
        {
            get { return GetRefEntity(ShopFpySettingProperty); }
            set { SetRefEntity(ShopFpySettingProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 产线直通率设置 实体配置
    /// </summary>
    internal class LineFpySettingConfig : EntityConfig<LineFpySetting>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_LINE_FPY").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}