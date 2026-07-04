using SIE.Common.Platform;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Rbac.Menus;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Common.HomeMenusConfigs
{

    /// <summary>
    /// 菜单配置明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("首页菜单配置明细")]
    public partial class HomeMenusConfigDetail : DataEntity
    {

        #region 菜单配置 HomeMenusConfig
        /// <summary>
        /// 菜单配置Id
        /// </summary>
        [Label("菜单配置")]
        public static readonly IRefIdProperty HomeMenusConfigIdProperty =
            P<HomeMenusConfigDetail>.RegisterRefId(e => e.HomeMenusConfigId, ReferenceType.Parent);

        /// <summary>
        /// 菜单配置Id
        /// </summary>
        public double HomeMenusConfigId
        {
            get { return (double)this.GetRefId(HomeMenusConfigIdProperty); }
            set { this.SetRefId(HomeMenusConfigIdProperty, value); }
        }

        /// <summary>
        /// 菜单配置
        /// </summary>
        public static readonly RefEntityProperty<HomeMenusConfig> HomeMenusConfigProperty =
            P<HomeMenusConfigDetail>.RegisterRef(e => e.HomeMenusConfig, HomeMenusConfigIdProperty);

        /// <summary>
        /// 菜单配置
        /// </summary>
        public HomeMenusConfig HomeMenusConfig
        {
            get { return this.GetRefEntity(HomeMenusConfigProperty); }
            set { this.SetRefEntity(HomeMenusConfigProperty, value); }
        }
        #endregion


        #region 菜单 Menu
        /// <summary>
        /// 菜单Id
        /// </summary>
        [Label("菜单")]
        public static readonly IRefIdProperty MenuIdProperty =
            P<HomeMenusConfigDetail>.RegisterRefId(e => e.MenuId, ReferenceType.Normal);

        /// <summary>
        /// 菜单
        /// </summary>
        public double MenuId
        {
            get { return (double)this.GetRefId(MenuIdProperty); }
            set { this.SetRefId(MenuIdProperty, value); }
        }

        /// <summary>
        /// 菜单
        /// </summary>
        public static readonly RefEntityProperty<Menu> MenuProperty =
            P<HomeMenusConfigDetail>.RegisterRef(e => e.Menu, MenuIdProperty);

        /// <summary>
        /// 菜单
        /// </summary>
        public Menu Menu
        {
            get { return this.GetRefEntity(MenuProperty); }
            set { this.SetRefEntity(MenuProperty, value); }
        }
        #endregion


        #region 视图属性

        #region 菜单名称 MenuName
        /// <summary>
        /// 菜单名称
        /// </summary>
        [Label("菜单名称")]
        public static readonly Property<string> NameProperty = P<HomeMenusConfigDetail>.RegisterView(e => e.MenuName, p => p.Menu.Label);

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName
        {
            get { return this.GetProperty(NameProperty); }
        }
        #endregion


        #region 模块 MoudleKey
        /// <summary>
        /// 模块
        /// </summary>
        [Label("模块")]
        public static readonly Property<string> MoudleKeyProperty = P<HomeMenusConfigDetail>.RegisterView(e => e.MoudleKey, p => p.Menu.ModuleKey);

        /// <summary>
        /// 模块
        /// </summary>
        public string MoudleKey
        {
            get { return this.GetProperty(MoudleKeyProperty); }
        }
        #endregion
        #region 平台 Platform
        /// <summary>
        /// 平台
        /// </summary>
        [Label("平台")]
        public static readonly Property<Platform> PlatformProperty = P<HomeMenusConfigDetail>.RegisterView(e => e.Platform, p => p.Menu.Platform);

        /// <summary>
        /// 
        /// </summary>
        public Platform Platform
        {
            get { return this.GetProperty(PlatformProperty); }
        }
        #endregion


        #endregion

    }
    internal class HomeMenusConfigDetailConfig : EntityConfig<HomeMenusConfigDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("HOME_MENU_CONFIG_DTL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
