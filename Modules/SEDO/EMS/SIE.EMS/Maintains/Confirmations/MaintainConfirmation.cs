using SIE.Domain;
using SIE.EMS.Enums;
using SIE.EMS.Maintains.Plans;
using SIE.EMS.Tpms;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.Maintains.Confirmations
{
    /// <summary>
    /// 保养确认
    /// </summary>
    [ChildEntity, Serializable]
    [Label("保养确认")]
    public class MaintainConfirmation : DataEntity
    {
        #region 保养计划 MaintainPlan
        /// <summary>
        /// 保养计划Id
        /// </summary>
        [Label("保养计划")]
        public static readonly IRefIdProperty MaintainPlanIdProperty =
            P<MaintainConfirmation>.RegisterRefId(e => e.MaintainPlanId, ReferenceType.Parent);

        /// <summary>
        /// 保养计划Id
        /// </summary>
        public double MaintainPlanId
        {
            get { return (double)this.GetRefId(MaintainPlanIdProperty); }
            set { this.SetRefId(MaintainPlanIdProperty, value); }
        }

        /// <summary>
        /// 保养计划
        /// </summary>
        public static readonly RefEntityProperty<MaintainPlan> MaintainPlanProperty =
            P<MaintainConfirmation>.RegisterRef(e => e.MaintainPlan, MaintainPlanIdProperty);

        /// <summary>
        /// 保养计划
        /// </summary>
        public MaintainPlan MaintainPlan
        {
            get { return this.GetRefEntity(MaintainPlanProperty); }
            set { this.SetRefEntity(MaintainPlanProperty, value); }
        }
        #endregion

        #region 确认部门 ConfirmDept
        /// <summary>
        /// 确认部门Id
        /// </summary>
        [Label("确认部门")]
        public static readonly IRefIdProperty ConfirmDeptIdProperty =
            P<MaintainConfirmation>.RegisterRefId(e => e.ConfirmDeptId, ReferenceType.Normal);

        /// <summary>
        /// 确认部门Id
        /// </summary>
        public double ConfirmDeptId
        {
            get { return (double)this.GetRefId(ConfirmDeptIdProperty); }
            set { this.SetRefId(ConfirmDeptIdProperty, value); }
        }

        /// <summary>
        /// 确认部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> ConfirmDeptProperty =
            P<MaintainConfirmation>.RegisterRef(e => e.ConfirmDept, ConfirmDeptIdProperty);

        /// <summary>
        /// 确认部门
        /// </summary>
        public Enterprise ConfirmDept
        {
            get { return this.GetRefEntity(ConfirmDeptProperty); }
            set { this.SetRefEntity(ConfirmDeptProperty, value); }
        }
        #endregion

        #region 确认人 Confirmor
        /// <summary>
        /// 确认人Id
        /// </summary>
        [Label("确认人")]
        public static readonly IRefIdProperty ConfirmorIdProperty = P<MaintainConfirmation>.RegisterRefId(e => e.ConfirmorId, ReferenceType.Normal);

        /// <summary>
        /// 确认人Id
        /// </summary>
        public double? ConfirmorId
        {
            get { return (double?)GetRefNullableId(ConfirmorIdProperty); }
            set { SetRefNullableId(ConfirmorIdProperty, value); }
        }

        /// <summary>
        /// 确认人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ConfirmorProperty = P<MaintainConfirmation>.RegisterRef(e => e.Confirmor, ConfirmorIdProperty);

        /// <summary>
        /// 确认人
        /// </summary>
        public Employee Confirmor
        {
            get { return GetRefEntity(ConfirmorProperty); }
            set { SetRefEntity(ConfirmorProperty, value); }
        }
        #endregion

        #region 确认结果 ConfirmResult
        /// <summary>
        /// 确认结果
        /// </summary>
        [Label("确认结果")]
        public static readonly Property<ConfirmResult?> ConfirmResultProperty = P<MaintainConfirmation>.Register(e => e.ConfirmResult);

        /// <summary>
        /// 确认结果
        /// </summary>
        public ConfirmResult? ConfirmResult
        {
            get { return GetProperty(ConfirmResultProperty); }
            set { SetProperty(ConfirmResultProperty, value); }
        }
        #endregion

        #region 确认备注 ConfirmNote
        /// <summary>
        /// 确认备注
        /// </summary>
        [Label("确认备注")]
        public static readonly Property<string> ConfirmNoteProperty = P<MaintainConfirmation>.Register(e => e.ConfirmNote);

        /// <summary>
        /// 确认备注
        /// </summary>
        public string ConfirmNote
        {
            get { return GetProperty(ConfirmNoteProperty); }
            set { SetProperty(ConfirmNoteProperty, value); }
        }
        #endregion

        #region 确认日期 ConfirmDate
        /// <summary>
        /// 确认日期
        /// </summary>
        [Label("确认日期")]
        public static readonly Property<DateTime?> ConfirmDateProperty = P<MaintainConfirmation>.Register(e => e.ConfirmDate);

        /// <summary>
        /// 确认日期
        /// </summary>
        public DateTime? ConfirmDate
        {
            get { return GetProperty(ConfirmDateProperty); }
            set { SetProperty(ConfirmDateProperty, value); }
        }
        #endregion

        #region 评分项 TpmScoreProject
        /// <summary>
        /// 评分项Id
        /// </summary>
        [Label("评分项")]
        public static readonly IRefIdProperty TpmScoreProjectIdProperty = P<MaintainConfirmation>.RegisterRefId(e => e.TpmScoreProjectId, ReferenceType.Normal);

        /// <summary>
        /// 评分项Id
        /// </summary>
        public double? TpmScoreProjectId
        {
            get { return (double?)GetRefNullableId(TpmScoreProjectIdProperty); }
            set { SetRefNullableId(TpmScoreProjectIdProperty, value); }
        }

        /// <summary>
        /// 评分项
        /// </summary>
        public static readonly RefEntityProperty<TpmWeekInspectScore> TpmScoreProjectProperty = P<MaintainConfirmation>.RegisterRef(e => e.TpmScoreProject, TpmScoreProjectIdProperty);

        /// <summary>
        /// 评分项
        /// </summary>
        public TpmWeekInspectScore TpmScoreProject
        {
            get { return GetRefEntity(TpmScoreProjectProperty); }
            set { SetRefEntity(TpmScoreProjectProperty, value); }
        }
        #endregion

        #region 评分 Score
        /// <summary>
        /// 评分
        /// </summary>
        [Label("评分")]
        public static readonly Property<Score> ScoreProperty = P<MaintainConfirmation>.Register(e => e.Score);

        /// <summary>
        /// 评分
        /// </summary>
        public Score Score
        {
            get { return this.GetProperty(ScoreProperty); }
            set { this.SetProperty(ScoreProperty, value); }
        }
        #endregion

        #region 文件名称 FileName
        /// <summary>
        /// 文件名称
        /// </summary>
        [Label("文件名称")]
        [MaxLength(80)]
        public static readonly Property<string> FileNameProperty = P<MaintainConfirmation>.Register(e => e.FileName);

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
        [MaxLength(500)]
        public static readonly Property<string> FilePathProperty = P<MaintainConfirmation>.Register(e => e.FilePath);

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
        public static readonly Property<string> FileExtesionProperty = P<MaintainConfirmation>.Register(e => e.FileExtesion);

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
        public static readonly Property<string> FileSizeProperty = P<MaintainConfirmation>.Register(e => e.FileSize);

        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileSize
        {
            get { return GetProperty(FileSizeProperty); }
            set { SetProperty(FileSizeProperty, value); }
        }
        #endregion

        #region 内容 Content
        /// <summary>
        /// 内容
        /// </summary>
        [Label("内容")]
        public static readonly Property<byte[]> ContentProperty = P<MaintainConfirmation>.Register(e => e.Content);

        /// <summary>
        /// 内容
        /// </summary>
        public byte[] Content
        {
            get { return GetProperty(ContentProperty); }
            set { SetProperty(ContentProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 评分项名称 ProjectName
        /// <summary>
        /// 评分项名称
        /// </summary>
        [Label("评分项名称")]
        public static readonly Property<string> ProjectNameProperty = P<MaintainConfirmation>.RegisterView(e => e.ProjectName, p => p.TpmScoreProject.ProjectName);

        /// <summary>
        /// 评分项名称
        /// </summary>
        public string ProjectName
        {
            get { return GetProperty(ProjectNameProperty); }
        }
        #endregion

        #region 确认部门编码 ConfirmDeptCode
        /// <summary>
        /// 确认部门编码
        /// </summary>
        [Label("确认部门编码")]
        public static readonly Property<string> ConfirmDeptCodeProperty = P<MaintainConfirmation>.RegisterView(e => e.ConfirmDeptCode, p => p.ConfirmDept.Code);

        /// <summary>
        /// 确认部门编码
        /// </summary>
        public string ConfirmDeptCode
        {
            get { return this.GetProperty(ConfirmDeptCodeProperty); }
        }
        #endregion

        #region 确认部门名称 ConfirmDeptName
        /// <summary>
        /// 确认部门名称
        /// </summary>
        [Label("确认部门名称")]
        public static readonly Property<string> ConfirmDeptNameProperty = P<MaintainConfirmation>.RegisterView(e => e.ConfirmDeptName, p => p.ConfirmDept.Name);

        /// <summary>
        /// 确认部门名称
        /// </summary>
        public string ConfirmDeptName
        {
            get { return this.GetProperty(ConfirmDeptNameProperty); }
        }
        #endregion

        #region 是否拍照 IsPhoto
        /// <summary>
        /// 是否拍照
        /// </summary>
        [Label("是否拍照")]
        public static readonly Property<bool> IsPhotoProperty = P<MaintainConfirmation>.RegisterView(e => e.IsPhoto, p => p.TpmScoreProject.IsPhoto);

        /// <summary>
        /// 是否拍照
        /// </summary>
        public bool IsPhoto
        {
            get { return this.GetProperty(IsPhotoProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    internal class MaintainConfirmationConfig : EntityConfig<MaintainConfirmation>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_MAINTAIN_CONFM").MapAllProperties();
            Meta.Property(MaintainConfirmation.ConfirmNoteProperty).ColumnMeta.HasLength(800);
            Meta.Property(MaintainConfirmation.FileNameProperty).ColumnMeta.HasLength(4000);
            Meta.Property(MaintainConfirmation.FilePathProperty).ColumnMeta.HasLength(4000);
            Meta.Property(MaintainConfirmation.ContentProperty).DontMapColumn();

            //不启用幽灵属性
            Meta.DisablePhantoms();
        }
    }
}
