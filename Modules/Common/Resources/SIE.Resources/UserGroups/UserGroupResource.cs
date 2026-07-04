using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Rbac.Users;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Resources.UserGroups
{
    /// <summary>
    /// 用户组与资源
    /// </summary>
    [RootEntity, Serializable]
    [Label("用户组与资源")]
    public class UserGroupResource : DataEntity
    {
        #region 资源 Resource
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<UserGroupResource>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<UserGroupResource>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary> 
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 用户组 UserGroup
        /// <summary>
        /// 用户组Id
        /// </summary>
        [Label("用户组")]
        public static readonly IRefIdProperty UserGroupIdProperty =
            P<UserGroupResource>.RegisterRefId(e => e.UserGroupId, ReferenceType.Normal);

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
            P<UserGroupResource>.RegisterRef(e => e.UserGroup, UserGroupIdProperty);

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

        #region 资源编码 ResourceCode
        /// <summary>
        /// 资源编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> ResourceCodeProperty = P<UserGroupResource>.RegisterView(e => e.ResourceCode, p => p.Resource.Code);

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode
        {
            get { return this.GetProperty(ResourceCodeProperty); }
        }
        #endregion 

        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> ResourceNameProperty = P<UserGroupResource>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion

        #endregion
    }

    internal class UserGroupResourceConfig : EntityConfig<UserGroupResource>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("USER_GROUP_RESOURCE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
