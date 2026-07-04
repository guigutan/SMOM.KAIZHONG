using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Rbac.Users;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Resources.UserGroups
{
    /// <summary>
    /// 用户组与工厂关系
    /// </summary>
    [RootEntity, Serializable]
    [Label("用户组与工厂关系")]
    public class UserGroupEnterprise : DataEntity
    {
        #region 工厂 Enterprise
        /// <summary>
        /// 工厂Id
        /// </summary>
        [Label("工厂")]
        public static readonly IRefIdProperty EnterpriseIdProperty = P<UserGroupEnterprise>.RegisterRefId(e => e.EnterpriseId, ReferenceType.Normal);

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double EnterpriseId
        {
            get { return (double)GetRefId(EnterpriseIdProperty); }
            set { SetRefId(EnterpriseIdProperty, value); }
        }

        /// <summary>
        /// 工厂
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> EnterpriseProperty = P<UserGroupEnterprise>.RegisterRef(e => e.Enterprise, EnterpriseIdProperty);

        /// <summary>
        /// 工厂
        /// </summary>
        public Enterprise Enterprise
        {
            get { return GetRefEntity(EnterpriseProperty); }
            set { SetRefEntity(EnterpriseProperty, value); }
        }
        #endregion

        #region 用户组 UserGroup
        /// <summary>
        /// 用户组Id
        /// </summary>
        [Label("用户组")]
        public static readonly IRefIdProperty UserGroupIdProperty =
            P<UserGroupEnterprise>.RegisterRefId(e => e.UserGroupId, ReferenceType.Normal);

        /// <summary>
        /// 用户组Id
        /// </summary>
        public double UserGroupId
        {
            get { return (double)this.GetRefId(UserGroupIdProperty); }
            set { this.SetRefId(UserGroupIdProperty, value); }
        }

        /// <summary>
        /// 用户组
        /// </summary>
        public static readonly RefEntityProperty<UserGroup> UserGroupProperty =
            P<UserGroupEnterprise>.RegisterRef(e => e.UserGroup, UserGroupIdProperty);

        /// <summary>
        /// 用户组
        /// </summary>
        public UserGroup UserGroup
        {
            get { return this.GetRefEntity(UserGroupProperty); }
            set { this.SetRefEntity(UserGroupProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 工厂编码 EnterpriseCode
        /// <summary>
        /// 工厂编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> EnterpriseCodeProperty = P<UserGroupEnterprise>.RegisterView(e => e.EnterpriseCode, p => p.Enterprise.Code);

        /// <summary>
        /// 工厂编码
        /// </summary>
        public string EnterpriseCode
        {
            get { return this.GetProperty(EnterpriseCodeProperty); }
        }
        #endregion

        #region 资源名称 EnterpriseName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> EnterpriseNameProperty = P<UserGroupEnterprise>.RegisterView(e => e.EnterpriseName, p => p.Enterprise.Name);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string EnterpriseName
        {
            get { return this.GetProperty(EnterpriseNameProperty); }
        }
        #endregion

        #endregion
    }

    internal class UserGroupEnterpriseConfig : EntityConfig<UserGroupEnterprise>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("USER_GROUP_ENTERPRISE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
