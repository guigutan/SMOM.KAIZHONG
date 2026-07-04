using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Rbac.Menus;
using SIE.Rbac.Roles;
using SIE.Rbac.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Common.HomeMenusConfigs
{

    /// <summary>
    /// 触摸屏首页设置
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(HomeMenusConfigCriteria))]
    [Label("触摸屏首页设置")]
    public partial class HomeMenusConfig : DataEntity
    {
        #region 角色 Role
        /// <summary>
        /// 角色
        /// </summary>
        [Label("角色")]
        public static readonly IRefIdProperty RoleIdProperty =
            P<HomeMenusConfig>.RegisterRefId(e => e.RoleId, ReferenceType.Normal);

        /// <summary>
        /// 角色
        /// </summary>
        public double? RoleId
        {
            get { return (double?)this.GetRefNullableId(RoleIdProperty); }
            set { this.SetRefNullableId(RoleIdProperty, value); }
        }

        /// <summary>
        /// 角色
        /// </summary>
        public static readonly RefEntityProperty<Role> RoleProperty =
            P<HomeMenusConfig>.RegisterRef(e => e.Role, RoleIdProperty);

        /// <summary>
        /// 角色
        /// </summary>
        public Role Role
        {
            get { return this.GetRefEntity(RoleProperty); }
            set { this.SetRefEntity(RoleProperty, value); }
        }
        #endregion

        #region 用户 User
        /// <summary>
        /// 用户Id
        /// </summary>
        [Label("用户")]
        public static readonly IRefIdProperty UserIdProperty =
            P<HomeMenusConfig>.RegisterRefId(e => e.UserId, ReferenceType.Normal);

        /// <summary>
        /// 用户Id
        /// </summary>
        public double? UserId
        {
            get { return (double?)this.GetRefNullableId(UserIdProperty); }
            set { this.SetRefNullableId(UserIdProperty, value); }
        }

        /// <summary>
        /// 用户
        /// </summary>
        public static readonly RefEntityProperty<User> UserProperty =
            P<HomeMenusConfig>.RegisterRef(e => e.User, UserIdProperty);

        /// <summary>
        /// 用户
        /// </summary>
        public User User
        {
            get { return this.GetRefEntity(UserProperty); }
            set { this.SetRefEntity(UserProperty, value); }
        }
        #endregion


        #region 菜单集合 EntityListType
        /// <summary>
        /// 菜单集合
        /// </summary>
        [Label("菜单")]
        public static readonly ListProperty<EntityList<HomeMenusConfigDetail>> HomeMenusConfigDetailListProperty = P<HomeMenusConfig>.RegisterList(e => e.HomeMenusConfigDetailList);

        /// <summary>
        /// 菜单集合
        /// </summary>
        public EntityList<HomeMenusConfigDetail> HomeMenusConfigDetailList
        {
            get { return this.GetLazyList(HomeMenusConfigDetailListProperty); }
        }
        #endregion



    }
    internal class HomeMenusConfigConfig : EntityConfig<HomeMenusConfig>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("HOME_MENU_CONFIG").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
