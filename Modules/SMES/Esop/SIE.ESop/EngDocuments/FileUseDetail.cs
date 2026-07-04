using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ESop.EngDocuments
{
    /// <summary>
    /// 工程文件使用类型
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("工程文件使用类型")]
    public class FileUseDetail : DataEntity
    {
        #region 使用类型 UseType
        /// <summary>
        /// 使用类型
        /// </summary>
        [Label("使用类型")]
        public static readonly Property<string> UseTypeProperty = P<FileUseDetail>.Register(e => e.UseType);

        /// <summary>
        /// 使用类型
        /// </summary>
        public string UseType
        {
            get { return this.GetProperty(UseTypeProperty); }
            set { this.SetProperty(UseTypeProperty, value); }
        }
        #endregion

        #region 文件夹 File
        /// <summary>
        /// 文件夹
        /// </summary>
        [Label("文件夹")]
        public static readonly Property<string> FileProperty = P<FileUseDetail>.Register(e => e.File);

        /// <summary>
        /// 文件夹
        /// </summary>
        public string File
        {
            get { return this.GetProperty(FileProperty); }
            set { this.SetProperty(FileProperty, value); }
        }
        #endregion

        #region 文件夹Id FolderId
        /// <summary>
        /// 文件夹Id
        /// </summary>
        [Label("文件夹Id")]
        public static readonly Property<double?> FolderIdProperty = P<FileUseDetail>.Register(e => e.FolderId);

        /// <summary>
        /// 文件夹Id
        /// </summary>
        public double? FolderId
        {
            get { return this.GetProperty(FolderIdProperty); }
            set { this.SetProperty(FolderIdProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 工程文件使用明细配置项
    /// </summary>
    public class FileUseDetailConfig : EntityConfig<FileUseDetail>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("FILE_USE_DETAIL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
