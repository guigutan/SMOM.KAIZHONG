using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.FMS
{
    /// <summary>
    /// 文件夹
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember(nameof(Name))]
    [Label("文件夹")]   
    public class Folder : DataEntity
    {
        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<Folder>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        #region 上一级文件夹 PreFolderId
        /// <summary>
        /// 上一级文件夹
        /// </summary>
        [Label("上一级文件夹")]
        public static readonly Property<double?> PreFolderIdProperty = P<Folder>.Register(e => e.PreFolderId);

        /// <summary>
        /// 上一级文件夹
        /// </summary>
        public double? PreFolderId
        {
            get { return this.GetProperty(PreFolderIdProperty); }
            set { this.SetProperty(PreFolderIdProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 文件夹 实体配置
    /// </summary>
    internal class FolderConfig : EntityConfig<Folder>
    {
        /// <summary>
        /// 子类重写此方法，并完成对 Meta 属性的配置。     
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("FOLDER").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
