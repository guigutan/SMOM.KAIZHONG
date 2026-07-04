using SIE.Common.Attachments;
using SIE.Domain;
using SIE.EMS.Checks.Plans;
using SIE.EMS.Enums;
using SIE.EMS.Tpms;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using System;

namespace SIE.EMS.Checks.Confirmations
{
    /// <summary>
    /// 点检确认
    /// </summary>
    [ChildEntity, Serializable]
    [Label("点检确认")]
    public class CheckConfirmation : Attachment<CheckPlan>
    {
        #region 确认部门 ConfirmDept
        /// <summary>
        /// 确认部门Id
        /// </summary>
        [Label("确认部门")]
        public static readonly IRefIdProperty ConfirmDeptIdProperty = P<CheckConfirmation>.RegisterRefId(e => e.ConfirmDeptId, ReferenceType.Normal);

        /// <summary>
        /// 确认部门Id
        /// </summary>
        public double? ConfirmDeptId
        {
            get { return (double?)GetRefNullableId(ConfirmDeptIdProperty); }
            set { SetRefNullableId(ConfirmDeptIdProperty, value); }
        }

        /// <summary>
        /// 确认部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> ConfirmDeptProperty = P<CheckConfirmation>.RegisterRef(e => e.ConfirmDept, ConfirmDeptIdProperty);

        /// <summary>
        /// 确认部门
        /// </summary>
        public Enterprise ConfirmDept
        {
            get { return GetRefEntity(ConfirmDeptProperty); }
            set { SetRefEntity(ConfirmDeptProperty, value); }
        }
        #endregion

        #region 确认人 Confirmor
        /// <summary>
        /// 确认人Id
        /// </summary>
        [Label("确认人")]
        public static readonly IRefIdProperty ConfirmorIdProperty = P<CheckConfirmation>.RegisterRefId(e => e.ConfirmorId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Employee> ConfirmorProperty = P<CheckConfirmation>.RegisterRef(e => e.Confirmor, ConfirmorIdProperty);

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
        public static readonly Property<ConfirmResult?> ConfirmResultProperty = P<CheckConfirmation>.Register(e => e.ConfirmResult);

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
        public static readonly Property<string> ConfirmNoteProperty = P<CheckConfirmation>.Register(e => e.ConfirmNote);

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
        public static readonly Property<DateTime?> ConfirmDateProperty = P<CheckConfirmation>.Register(e => e.ConfirmDate);

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
        public static readonly IRefIdProperty TpmScoreProjectIdProperty = P<CheckConfirmation>.RegisterRefId(e => e.TpmScoreProjectId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<TpmWeekInspectScore> TpmScoreProjectProperty = P<CheckConfirmation>.RegisterRef(e => e.TpmScoreProject, TpmScoreProjectIdProperty);

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
        public static readonly Property<Score> ScoreProperty = P<CheckConfirmation>.Register(e => e.Score);

        /// <summary>
        /// 评分
        /// </summary>
        public Score Score
        {
            get { return this.GetProperty(ScoreProperty); }
            set { this.SetProperty(ScoreProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 评分项名称 ProjectName
        /// <summary>
        /// 评分项名称
        /// </summary>
        [Label("评分项名称")]
        public static readonly Property<string> ProjectNameProperty = P<CheckConfirmation>.RegisterView(e => e.ProjectName, p => p.TpmScoreProject.ProjectName);

        /// <summary>
        /// 评分项名称
        /// </summary>
        public string ProjectName
        {
            get { return GetProperty(ProjectNameProperty); }
        }
        #endregion

        #region 是否拍照 IsPhoto
        /// <summary>
        /// 是否拍照
        /// </summary>
        [Label("是否拍照")]
        public static readonly Property<bool> IsPhotoProperty = P<CheckConfirmation>.RegisterView(e => e.IsPhoto, p => p.TpmScoreProject.IsPhoto);

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
    ///  仓库
    /// </summary>
    [DataProvider(typeof(EmsEntityDataProvider))]
    public partial class CheckConfirmationRepository : AttachmentRepository<CheckConfirmation>
    {
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    internal class CheckConfirmationConfig : EntityConfig<CheckConfirmation>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_CHK_CONFM").MapAllProperties();            
            Meta.Property(CheckConfirmation.ConfirmNoteProperty).ColumnMeta.HasLength(800);
            Meta.Property(CheckConfirmation.FileNameProperty).ColumnMeta.HasLength(4000);
            Meta.Property(CheckConfirmation.FilePathProperty).ColumnMeta.HasLength(4000);
            Meta.Property(CheckConfirmation.ContentProperty).DontMapColumn();
            Meta.DisablePhantoms();//不启用幽灵属性
        }
    }
}
