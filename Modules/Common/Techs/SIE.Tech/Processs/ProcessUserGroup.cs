using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Rbac.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Tech.Processs
{
    /// <summary>
    /// 用户组与工序
    /// </summary>
    [RootEntity, Serializable]
    [Label("用户组与工序")]
    public class ProcessUserGroup : DataEntity
    {
        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<ProcessUserGroup>.RegisterRefId(e => e.ProcessId, ReferenceType.Parent);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)GetRefNullableId(ProcessIdProperty); }
            set { SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<ProcessUserGroup>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 产品族小类

        /// <summary>
        /// 产品族小类
        /// </summary>
        [Label("产品族小类")]
        public static readonly Property<string> ProductFamilyCategoryProperty = P<ProcessUserGroup>.RegisterReadOnly(
            e => e.ProductFamilyCategory, e => e.GetProductFamilyCategory());

        /// <summary>
        /// 产品族小类
        /// </summary>
        public string ProductFamilyCategory
        {
            get { return this.GetProperty(ProductFamilyCategoryProperty); }
        }

        /// <summary>
        /// 获取产品族小类
        /// </summary>
        /// <returns>返回产品族小类</returns>
        private string GetProductFamilyCategory()
        {
            var process = Process;
            if (process != null)
                return process.ProductFamily?.Name;
            return string.Empty;
        }
        #endregion

        #region 用户组 UserGroup
        /// <summary>
        /// 用户组Id
        /// </summary>
        [Label("用户组")]
        public static readonly IRefIdProperty UserGroupIdProperty =
            P<ProcessUserGroup>.RegisterRefId(e => e.UserGroupId, ReferenceType.Normal);

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
            P<ProcessUserGroup>.RegisterRef(e => e.UserGroup, UserGroupIdProperty);

        /// <summary>
        /// 用户组
        /// </summary>
        public UserGroup UserGroup
        {
            get { return this.GetRefEntity(UserGroupProperty); }
            set { this.SetRefEntity(UserGroupProperty, value); }
        }
        #endregion
    }

    internal class ProcessUserGroupConfig : EntityConfig<ProcessUserGroup>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("PROCESS_USER_GROUP").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
