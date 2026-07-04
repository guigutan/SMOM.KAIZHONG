using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.FMS
{
    /// <summary>
    /// 文件用户组
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [DisplayMember(nameof(Code))]
    [Label("文件用户组")]
    public partial class FileUserGroup : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<FileUserGroup>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<FileUserGroup>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 是否文件管理员 IsAdmin
        /// <summary>
        /// 是否管理员
        /// </summary>
        [Label("文件管理员")]
        public static readonly Property<bool> IsAdminProperty = P<FileUserGroup>.Register(e => e.IsAdmin);

        /// <summary>
        /// 文件管理员
        /// </summary>
        public bool IsAdmin
        {
            get { return this.GetProperty(IsAdminProperty); }
            set { this.SetProperty(IsAdminProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 文件用户组 实体配置
    /// </summary>
    internal class FileUserGroupConfig : EntityConfig<FileUserGroup>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("FMS_USER_GROUP").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}