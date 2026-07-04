using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.EMS.Purchases.EquipmentSetups
{
    /// <summary>
    /// 安装调试附件
    /// </summary>
    [ChildEntity, Serializable]
    [Label("安装调试附件")]
    public partial class SetupAttachment : DataEntity
    {
        #region 附件 EquipmentSetup
        /// <summary>
        /// 附件Id
        /// </summary>
        public static readonly IRefIdProperty EquipmentSetupIdProperty = P<SetupAttachment>.RegisterRefId(e => e.EquipmentSetupId, ReferenceType.Parent);

        /// <summary>
        /// 附件Id
        /// </summary>
        public double EquipmentSetupId
        {
            get { return (double)GetRefId(EquipmentSetupIdProperty); }
            set { SetRefId(EquipmentSetupIdProperty, value); }
        }

        /// <summary>
        /// 附件
        /// </summary>
        public static readonly RefEntityProperty<EquipmentSetup> EquipmentSetupProperty = P<SetupAttachment>.RegisterRef(e => e.EquipmentSetup, EquipmentSetupIdProperty);

        /// <summary>
        /// 附件
        /// </summary>
        public EquipmentSetup EquipmentSetup
        {
            get { return GetRefEntity(EquipmentSetupProperty); }
            set { SetRefEntity(EquipmentSetupProperty, value); }
        }
        #endregion

        #region 文件名称 FileName
        /// <summary>
        /// 文件名称
        /// </summary>
        [Label("文件名称")]
        public static readonly Property<string> FileNameProperty = P<SetupAttachment>.Register(e => e.FileName);

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName
        {
            get { return GetProperty(FileNameProperty); }
            set { SetProperty(FileNameProperty, value); }
        }
        #endregion

        #region 文件路径 FilePath
        /// <summary>
        /// 文件路径
        /// </summary>
        [Label("文件路径")]
        [MaxLength(300)]
        public static readonly Property<string> FilePathProperty = P<SetupAttachment>.Register(e => e.FilePath);

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath
        {
            get { return GetProperty(FilePathProperty); }
            set { SetProperty(FilePathProperty, value); }
        }
        #endregion

        #region 文件扩展名 FileExtesion
        /// <summary>
        /// 文件扩展名
        /// </summary>
        [Label("文件扩展名")]
        public static readonly Property<string> FileExtesionProperty = P<SetupAttachment>.Register(e => e.FileExtesion);

        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string FileExtesion
        {
            get { return GetProperty(FileExtesionProperty); }
            set { SetProperty(FileExtesionProperty, value); }
        }
        #endregion

        #region 文件大小 FileSize
        /// <summary>
        /// 文件大小
        /// </summary>
        [Label("文件大小")]
        public static readonly Property<string> FileSizeProperty = P<SetupAttachment>.Register(e => e.FileSize);

        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileSize
        {
            get { return GetProperty(FileSizeProperty); }
            set { SetProperty(FileSizeProperty, value); }
        }
        #endregion

        #region 上传时间 UploadDate
        /// <summary>
        /// 上传时间
        /// </summary>
        [Label("上传时间")]
        public static readonly Property<DateTime> UploadDateProperty = P<SetupAttachment>.Register(e => e.UploadDate);

        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime UploadDate
        {
            get { return GetProperty(UploadDateProperty); }
            set { SetProperty(UploadDateProperty, value); }
        }
        #endregion

        #region 上传人 Uploader
        /// <summary>
        /// 上传人Id
        /// </summary>
        [Label("上传人")]
        public static readonly IRefIdProperty UploaderIdProperty = P<SetupAttachment>.RegisterRefId(e => e.UploaderId, ReferenceType.Normal);

        /// <summary>
        /// 上传人Id
        /// </summary>
        public double UploaderId
        {
            get { return (double)GetRefId(UploaderIdProperty); }
            set { SetRefId(UploaderIdProperty, value); }
        }

        /// <summary>
        /// 上传人
        /// </summary>
        public static readonly RefEntityProperty<Employee> UploaderProperty = P<SetupAttachment>.RegisterRef(e => e.Uploader, UploaderIdProperty);

        /// <summary>
        /// 上传人
        /// </summary>
        public Employee Uploader
        {
            get { return GetRefEntity(UploaderProperty); }
            set { SetRefEntity(UploaderProperty, value); }
        }
        #endregion

        #region 设备 EquipAccount
        /// <summary>
        /// 设备Id
        /// </summary>
        [Label("设备")]
        public static readonly IRefIdProperty EquipAccountIdProperty = P<SetupAttachment>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备Id
        /// </summary>
        public double? EquipAccountId
        {
            get { return (double?)GetRefNullableId(EquipAccountIdProperty); }
            set { SetRefNullableId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty = P<SetupAttachment>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return GetRefEntity(EquipAccountProperty); }
            set { SetRefEntity(EquipAccountProperty, value); }
        }
        #endregion

        #region 安装调试工作计划 EquipmentSetupPlan
        /// <summary>
        /// 安装调试工作计划Id
        /// </summary>
        public static readonly IRefIdProperty EquipmentSetupPlanIdProperty = P<SetupAttachment>.RegisterRefId(e => e.EquipmentSetupPlanId, ReferenceType.Normal);

        /// <summary>
        /// 安装调试工作计划Id
        /// </summary>
        public double? EquipmentSetupPlanId
        {
            get { return (double?)GetRefNullableId(EquipmentSetupPlanIdProperty); }
            set { SetRefNullableId(EquipmentSetupPlanIdProperty, value); }
        }

        /// <summary>
        /// 安装调试工作计划
        /// </summary>
        public static readonly RefEntityProperty<EquipmentSetupPlan> EquipmentSetupPlanProperty = P<SetupAttachment>.RegisterRef(e => e.EquipmentSetupPlan, EquipmentSetupPlanIdProperty);

        /// <summary>
        /// 安装调试工作计划
        /// </summary>
        public EquipmentSetupPlan EquipmentSetupPlan
        {
            get { return GetRefEntity(EquipmentSetupPlanProperty); }
            set { SetRefEntity(EquipmentSetupPlanProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 设备名称 EquipAccountName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipAccountNameProperty = P<SetupAttachment>.RegisterView(e => e.EquipAccountName, p => p.EquipAccount.Name);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipAccountName
        {
            get { return this.GetProperty(EquipAccountNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 安装调试附件 实体配置
    /// </summary>
    internal class SetupAttachmentConfig : EntityConfig<SetupAttachment>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_SETUP_ATCH").MapAllProperties();
            Meta.Property(SetupAttachment.FilePathProperty).ColumnMeta.HasLength(1200);
            Meta.EnablePhantoms();
        }
    }
}