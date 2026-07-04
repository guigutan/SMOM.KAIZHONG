using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.FMS
{
    /// <summary>
    /// 文件权限
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember(nameof(Permissions))]
    [Label("文件权限")]
    public partial class UserGroupPermission : DataEntity
    {
        #region 权限集合（位运算） Permissions
        /// <summary>
        /// 权限集合（位运算）
        /// </summary>
        [Label("权限集合（位运算）")]
        public static readonly Property<PermissionType> PermissionsProperty = P<UserGroupPermission>.Register(e => e.Permissions);

        /// <summary>
        /// 权限集合（位运算）
        /// </summary>
        public PermissionType Permissions
        {
            get { return GetProperty(PermissionsProperty); }
            set { SetProperty(PermissionsProperty, value); }
        }
        #endregion

        #region 文件用户组 FileUserGroup
        /// <summary>
        /// 文件用户组Id
        /// </summary>
        [Label("文件用户组")]
        public static readonly IRefIdProperty FileUserGroupIdProperty = P<UserGroupPermission>.RegisterRefId(e => e.FileUserGroupId, ReferenceType.Normal);

        /// <summary>
        /// 文件用户组Id
        /// </summary>
        public double FileUserGroupId
        {
            get { return (double)GetRefId(FileUserGroupIdProperty); }
            set { SetRefId(FileUserGroupIdProperty, value); }
        }

        /// <summary>
        /// 文件用户组
        /// </summary>
        public static readonly RefEntityProperty<FileUserGroup> FileUserGroupProperty = P<UserGroupPermission>.RegisterRef(e => e.FileUserGroup, FileUserGroupIdProperty);

        /// <summary>
        /// 文件用户组
        /// </summary>
        public FileUserGroup FileUserGroup
        {
            get { return GetRefEntity(FileUserGroupProperty); }
            set { SetRefEntity(FileUserGroupProperty, value); }
        }
        #endregion

        #region 文件夹 Folder
        /// <summary>
        /// 文件夹Id
        /// </summary>
        [Label("文件夹")]
        public static readonly IRefIdProperty FolderIdProperty = P<UserGroupPermission>.RegisterRefId(e => e.FolderId, ReferenceType.Normal);

        /// <summary>
        /// 文件夹Id
        /// </summary>
        public double? FolderId
        {
            get { return (double?)GetRefNullableId(FolderIdProperty); }
            set { SetRefNullableId(FolderIdProperty, value); }
        }

        /// <summary>
        /// 文件夹
        /// </summary>
        public static readonly RefEntityProperty<Folder> FolderProperty = P<UserGroupPermission>.RegisterRef(e => e.Folder, FolderIdProperty);

        /// <summary>
        /// 文件夹
        /// </summary>
        public Folder Folder
        {
            get { return GetRefEntity(FolderProperty); }
            set { SetRefEntity(FolderProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 文件权限 实体配置
    /// </summary>
    internal class UserGroupPermissionConfig : EntityConfig<UserGroupPermission>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("FMS_PERMISSION").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}