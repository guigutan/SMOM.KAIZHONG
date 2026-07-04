using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.FMS
{
    /// <summary>
    /// 文件操作日志
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember(nameof(Operation))]
    [Label("文件操作日志")]
    public partial class FileLog : DataEntity
    {
        #region 操作 Operation
        /// <summary>
        /// 操作
        /// </summary>
        [Label("操作")]
        public static readonly Property<string> OperationProperty = P<FileLog>.Register(e => e.Operation);

        /// <summary>
        /// 操作
        /// </summary>
        public string Operation
        {
            get { return GetProperty(OperationProperty); }
            set { SetProperty(OperationProperty, value); }
        }
        #endregion

        #region 文件 FileManage
        /// <summary>
        /// 文件Id
        /// </summary>
        [Label("文件")]
        public static readonly IRefIdProperty FileManageIdProperty = P<FileLog>.RegisterRefId(e => e.FileManageId, ReferenceType.Normal);

        /// <summary>
        /// 文件Id
        /// </summary>
        public double FileManageId
        {
            get { return (double)GetRefId(FileManageIdProperty); }
            set { SetRefId(FileManageIdProperty, value); }
        }

        /// <summary>
        /// 文件
        /// </summary>
        public static readonly RefEntityProperty<FileManage> FileManageProperty = P<FileLog>.RegisterRef(e => e.FileManage, FileManageIdProperty);

        /// <summary>
        /// 文件
        /// </summary>
        public FileManage FileManage
        {
            get { return GetRefEntity(FileManageProperty); }
            set { SetRefEntity(FileManageProperty, value); }
        }
        #endregion

        #region 注册视图属性
        #region 文件版本 FileVersion  
        /// <summary>
        /// 文件版本
        /// </summary>
        [Label("文件版本")]
        public static readonly Property<string> FileVersionProperty = P<FileLog>.RegisterView(e => e.FileVersion, p => p.FileManage.Version);

        /// <summary>
        /// 文件版本
        /// </summary>
        public string FileVersion
        {
            get { return this.GetProperty(FileVersionProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 文件操作日志 实体配置
    /// </summary>
    internal class FileLogConfig : EntityConfig<FileLog>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("FMS_FILE_LOG").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}