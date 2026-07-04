using SIE.Domain;
using SIE.EMS.EarlierStage.Enums;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.EarlierStage.Projects
{
    /// <summary>
    /// 变更内容
    /// </summary>
    [ChildEntity, Serializable]
    [Label("变更内容")]
    public partial class ProjectChangeContent : DataEntity
    {
        #region 项目变更 ProjectChange
        /// <summary>
        /// 项目变更Id
        /// </summary>
        [Label("项目变更")]
        public static readonly IRefIdProperty ProjectChangeIdProperty =
            P<ProjectChangeContent>.RegisterRefId(e => e.ProjectChangeId, ReferenceType.Parent);

        /// <summary>
        /// 项目变更Id
        /// </summary>
        public double ProjectChangeId
        {
            get { return (double)this.GetRefId(ProjectChangeIdProperty); }
            set { this.SetRefId(ProjectChangeIdProperty, value); }
        }

        /// <summary>
        /// 项目变更
        /// </summary>
        public static readonly RefEntityProperty<ProjectChange> ProjectChangeProperty =
            P<ProjectChangeContent>.RegisterRef(e => e.ProjectChange, ProjectChangeIdProperty);

        /// <summary>
        /// 项目变更
        /// </summary>
        public ProjectChange ProjectChange
        {
            get { return this.GetRefEntity(ProjectChangeProperty); }
            set { this.SetRefEntity(ProjectChangeProperty, value); }
        }
        #endregion

        #region 变更操作 ChangeOperate
        /// <summary>
        /// 变更操作
        /// </summary>
        [Label("变更操作")]
        public static readonly Property<ChangeOperate> ChangeOperateProperty = P<ProjectChangeContent>.Register(e => e.ChangeOperate);

        /// <summary>
        /// 变更操作
        /// </summary>
        public ChangeOperate ChangeOperate
        {
            get { return this.GetProperty(ChangeOperateProperty); }
            set { this.SetProperty(ChangeOperateProperty, value); }
        }
        #endregion

        #region 变更内容 ChangeType
        /// <summary>
        /// 变更内容
        /// </summary>
        [Label("变更内容")]
        public static readonly Property<ChangeType> ChangeTypeProperty = P<ProjectChangeContent>.Register(e => e.ChangeType);

        /// <summary>
        /// 变更内容
        /// </summary>
        public ChangeType ChangeType
        {
            get { return this.GetProperty(ChangeTypeProperty); }
            set { this.SetProperty(ChangeTypeProperty, value); }
        }
        #endregion

        #region 变更说明 ChangeExplain
        /// <summary>
        /// 变更说明
        /// </summary>
        [Label("变更说明")]
        [MaxLength(2000)]
        public static readonly Property<string> ChangeExplainProperty = P<ProjectChangeContent>.Register(e => e.ChangeExplain);

        /// <summary>
        /// 变更说明
        /// </summary>
        public string ChangeExplain
        {
            get { return this.GetProperty(ChangeExplainProperty); }
            set { this.SetProperty(ChangeExplainProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 变更单号 ProjectChangeNo
        /// <summary>
        /// 变更单号
        /// </summary>
        [Label("变更单号")]
        public static readonly Property<string> ProjectChangeNoProperty = P<ProjectChangeContent>.RegisterView(e => e.ProjectChangeNo, p => p.ProjectChange.No);

        /// <summary>
        /// 变更单号
        /// </summary>
        public string ProjectChangeNo
        {
            get { return this.GetProperty(ProjectChangeNoProperty); }
        }
        #endregion

        #region 状态 ApprovalStatus
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<ProjectChangeContent>.RegisterView(e => e.ApprovalStatus, p => p.ProjectChange.ApprovalStatus);

        /// <summary>
        /// 状态
        /// </summary>
        public ApprovalStatus ApprovalStatus
        {
            get { return this.GetProperty(ApprovalStatusProperty); }
        }
        #endregion

        #region 审核时间 ApprovalTime
        /// <summary>
        /// 审核时间
        /// </summary>
        [Label("审核时间")]
        public static readonly Property<DateTime?> ApprovalTimeProperty = P<ProjectChangeContent>.RegisterView(e => e.ApprovalTime, p => p.ProjectChange.ApprovalTime);

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? ApprovalTime
        {
            get { return this.GetProperty(ApprovalTimeProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 变更内容 实体配置
    /// </summary>
    internal class ProjectChangeContentConfig : EntityConfig<ProjectChangeContent>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EARL_PRO_CHA_CON").MapAllProperties();
            Meta.Property(ProjectChangeContent.ChangeExplainProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}
