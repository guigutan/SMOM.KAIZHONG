using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.FMS
{
    /// <summary>
    /// 文件
    /// </summary>
    [RootEntity, Serializable]
    [Label("文件管理")]
    [DisplayMember(nameof(Code))]
    [ConditionQueryType(typeof(FileManageCriteria))]
    public class FileManage : DataEntity
    {
        #region 文件编号 Code
        /// <summary>
        /// 文件编号
        /// </summary>
        [Label("文件编号")]
        public static readonly Property<string> CodeProperty = P<FileManage>.Register(e => e.Code);

        /// <summary>
        /// 文件编号
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 状态 FileState
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<FileState> FileStateProperty = P<FileManage>.Register(e => e.FileState);

        /// <summary>
        /// 状态
        /// </summary>
        public FileState FileState
        {
            get { return this.GetProperty(FileStateProperty); }
            set { this.SetProperty(FileStateProperty, value); }
        }
        #endregion

        #region 大小 Size
        /// <summary>
        /// 大小
        /// </summary>
        [Label("大小")]
        public static readonly Property<string> SizeProperty = P<FileManage>.Register(e => e.Size);

        /// <summary>
        /// 大小
        /// </summary>
        public string Size
        {
            get { return this.GetProperty(SizeProperty); }
            set { this.SetProperty(SizeProperty, value); }
        }
        #endregion

        #region 版本 Version
        /// <summary>
        /// 版本
        /// </summary>
        [Label("版本")]
        public static readonly Property<string> VersionProperty = P<FileManage>.Register(e => e.Version);

        /// <summary>
        /// 版本
        /// </summary>
        public string Version
        {
            get { return this.GetProperty(VersionProperty); }
            set { this.SetProperty(VersionProperty, value); }
        }
        #endregion

        #region 文件名 Name
        /// <summary>
        /// 文件名
        /// </summary>
        [Label("文件名")]
        public static readonly Property<string> NameProperty = P<FileManage>.Register(e => e.Name);

        /// <summary>
        /// 文件名
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        #region 上传路径 Path
        /// <summary>
        /// 上传路径
        /// </summary>
        [Label("上传路径")]
        [MaxLength(200)]
        public static readonly Property<string> PathProperty = P<FileManage>.Register(e => e.Path);

        /// <summary>
        /// 上传路径
        /// </summary>
        public string Path
        {
            get { return this.GetProperty(PathProperty); }
            set { this.SetProperty(PathProperty, value); }
        }
        #endregion

        #region 文件夹 Folder
        /// <summary>
        /// 文件夹Id
        /// </summary>
        [Label("文件夹")]
        public static readonly IRefIdProperty FolderIdProperty =
            P<FileManage>.RegisterRefId(e => e.FolderId, ReferenceType.Normal);

        /// <summary>
        /// 文件夹Id
        /// </summary>
        public double? FolderId
        {
            get { return (double?)this.GetRefNullableId(FolderIdProperty); }
            set { this.SetRefNullableId(FolderIdProperty, value); }
        }

        /// <summary>
        /// 文件夹
        /// </summary>
        public static readonly RefEntityProperty<Folder> FolderProperty =
            P<FileManage>.RegisterRef(e => e.Folder, FolderIdProperty);

        /// <summary>
        /// 文件夹
        /// </summary>
        public Folder Folder
        {
            get { return this.GetRefEntity(FolderProperty); }
            set { this.SetRefEntity(FolderProperty, value); }
        }
        #endregion

        #region 文件扩展名 FileExtesion
        /// <summary>
        /// 文件扩展名
        /// </summary>
        [Label("文件扩展名")]
        public static readonly Property<string> FileExtesionProperty = P<FileManage>.Register(e => e.FileExtesion);

        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string FileExtesion
        {
            get { return GetProperty(FileExtesionProperty); }
            set { SetProperty(FileExtesionProperty, value); }
        }
        #endregion

        #region 在服务器上的文件名 ServerFileName
        /// <summary>
        /// 在服务器上的文件名
        /// </summary>
        [MaxLength(200)]
        public static readonly Property<string> ServerFileNameProperty = P<FileManage>.Register(e => e.ServerFileName);

        /// <summary>
        /// 在服务器上的文件名
        /// </summary>
        public string ServerFileName
        {
            get { return this.GetProperty(ServerFileNameProperty); }
            set { this.SetProperty(ServerFileNameProperty, value); }
        }
        #endregion

        #region 原状态 PreFileState
        /// <summary>
        /// 原状态
        /// </summary>
        [Label("原状态")]
        public static readonly Property<FileState?> PreFileStateProperty = P<FileManage>.Register(e => e.PreFileState);

        /// <summary>
        /// 原状态
        /// </summary>
        public FileState? PreFileState
        {
            get { return this.GetProperty(PreFileStateProperty); }
            set { this.SetProperty(PreFileStateProperty, value); }
        }
        #endregion

        #region 流程发起人 FlowCreateBy
        /// <summary>
        /// 流程发起人
        /// </summary>
        [Label("流程发起人")]
        public static readonly Property<double?> FlowCreateByProperty = P<FileManage>.Register(e => e.FlowCreateBy);

        /// <summary>
        /// 流程发起人
        /// </summary>
        public double? FlowCreateBy
        {
            get { return this.GetProperty(FlowCreateByProperty); }
            set { this.SetProperty(FlowCreateByProperty, value); }
        }
        #endregion

        #region 是否历史版本 IsHistory
        /// <summary>
        /// 是否历史版本
        /// </summary>
        [Label("是否历史版本")]
        public static readonly Property<bool> IsHistoryProperty = P<FileManage>.Register(e => e.IsHistory);

        /// <summary>
        /// 是否历史版本
        /// </summary>
        public bool IsHistory
        {
            get { return this.GetProperty(IsHistoryProperty); }
            set { this.SetProperty(IsHistoryProperty, value); }
        }
        #endregion

        #region 版本前缀 VersionPrefix
        /// <summary>
        /// 版本前缀
        /// </summary>
        [Label("版本前缀")]
        public static readonly Property<string> VersionPrefixProperty = P<FileManage>.Register(e => e.VersionPrefix);

        /// <summary>
        /// 版本前缀
        /// </summary>
        public string VersionPrefix
        {
            get { return this.GetProperty(VersionPrefixProperty); }
            set { this.SetProperty(VersionPrefixProperty, value); }
        }
        #endregion

        #region 文件审批流列表 FileAuditList
        /// <summary>
        /// 文件审批流列表
        /// </summary>
        [Label("文件审批流列表")]
        public static readonly ListProperty<EntityList<FileAudit>> FileAuditListProperty = P<FileManage>.RegisterList(e => e.FileAuditList);

        /// <summary>
        /// 文件审批流列表
        /// </summary>
        public EntityList<FileAudit> FileAuditList
        {
            get { return this.GetLazyList(FileAuditListProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 文件管理 实体配置
    /// </summary>
    internal class FileManageConfig : EntityConfig<FileManage>
    {
        /// <summary>
        /// 子类重写此方法，并完成对 Meta 属性的配置。     
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("FILE_MANAGE").MapAllProperties();
            Meta.Property(FileManage.PathProperty).MapColumn().HasLength(400);
            Meta.Property(FileManage.ServerFileNameProperty).MapColumn().HasLength(400);
            Meta.EnablePhantoms();
        }
    }
}
