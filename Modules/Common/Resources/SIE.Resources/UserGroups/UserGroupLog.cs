using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Resources.UserGroups
{
    /// <summary>
    /// 用户组操作日志
    /// </summary>
    [RootEntity, Serializable]
    [Label("用户组操作日志")]
    [CriteriaQuery]
    public class UserGroupLog : DataEntity
    {
        #region 用户组 UserGroup
        /// <summary>
        /// 用户组
        /// </summary>
        [Label("用户组")]
        public static readonly Property<string> UserGroupProperty = P<UserGroupLog>.Register(e => e.UserGroup);

        /// <summary>
        /// 用户组
        /// </summary>
        public string UserGroup
        {
            get { return this.GetProperty(UserGroupProperty); }
            set { this.SetProperty(UserGroupProperty, value); }
        }
        #endregion

        #region 用户组Id UserGroupId
        /// <summary>
        /// 用户组Id
        /// </summary>
        [Label("用户组Id")]
        public static readonly Property<double> UserGroupIdProperty = P<UserGroupLog>.Register(e => e.UserGroupId);

        /// <summary>
        /// 用户组Id
        /// </summary>
        public double UserGroupId
        {
            get { return this.GetProperty(UserGroupIdProperty); }
            set { this.SetProperty(UserGroupIdProperty, value); }
        }
        #endregion

        #region 类型 Type
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<UserGroupLogType> TypeProperty = P<UserGroupLog>.Register(e => e.Type);

        /// <summary>
        /// 类型
        /// </summary>
        public UserGroupLogType Type
        {
            get { return this.GetProperty(TypeProperty); }
            set { this.SetProperty(TypeProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<UserGroupLogState> StateProperty = P<UserGroupLog>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public UserGroupLogState State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 操作数据 OperateData
        /// <summary>
        /// 操作数据
        /// </summary>
        [Label("操作数据")]
        public static readonly Property<string> OperateDataProperty = P<UserGroupLog>.Register(e => e.OperateData);

        /// <summary>
        /// 操作数据
        /// </summary>
        public string OperateData
        {
            get { return this.GetProperty(OperateDataProperty); }
            set { this.SetProperty(OperateDataProperty, value); }
        }
        #endregion

    }

    internal class UserGroupLogConfig : EntityConfig<UserGroupLog>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("USER_GROUP_LOG").MapAllProperties();
            Meta.EnableInvOrg();
            Meta.EnablePhantoms();
        }
    }

    /// <summary>
    /// 用户组操作日志类型
    /// </summary>
    public enum UserGroupLogType
    {
        /// <summary>
        /// 用户
        /// </summary>
        [Label("用户")]
        User = 1,

        /// <summary>
        /// 角色
        /// </summary>
        [Label("角色")]
        Role = 2,

        /// <summary>
        /// 资源
        /// </summary>
        [Label("资源")]
        Resource = 3,

        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        Factory = 4,

        /// <summary>
        /// 库存组织
        /// </summary>
        [Label("库存组织")]
        InvOrg = 5,

        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        Process = 6
    }

    /// <summary>
    /// 用户组操作日志状态
    /// </summary>
    public enum UserGroupLogState
    {
        /// <summary>
        /// 添加
        /// </summary>
        [Label("添加")]
        Add = 1,

        /// <summary>
        /// 删除
        /// </summary>
        [Label("删除")]
        Delete = 2
    }
}
