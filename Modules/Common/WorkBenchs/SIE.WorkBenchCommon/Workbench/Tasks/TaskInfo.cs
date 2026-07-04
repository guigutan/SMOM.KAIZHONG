using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using System;

namespace SIE.WorkBenchCommon.Workbench.Tasks
{
    /// <summary>
    /// 任务信息
    /// </summary>
    [RootEntity, Serializable]
	[CriteriaQuery]
	[Label("任务信息"),DisplayMember(nameof(Title))]
	public partial class TaskInfo : DataEntity
	{
		#region 标题 Title
		/// <summary>
		/// 标题
		/// </summary>
		[Label("标题")]
		public static readonly Property<string> TitleProperty = P<TaskInfo>.Register(e => e.Title);

		/// <summary>
		/// 标题
		/// </summary>
		public string Title
		{
			get { return GetProperty(TitleProperty); }
			set { SetProperty(TitleProperty, value); }
		}
		#endregion

		#region 内容 Content
		/// <summary>
		/// 内容
		/// </summary>
		[MaxLength(2000)]
		[Label("内容")]
		public static readonly Property<string> ContentProperty = P<TaskInfo>.Register(e => e.Content);

		/// <summary>
		/// 内容
		/// </summary>
		public string Content
		{
			get { return GetProperty(ContentProperty); }
			set { SetProperty(ContentProperty, value); }
		}
		#endregion

		#region 计划开始 PlanBegin
		/// <summary>
		/// 计划开始
		/// </summary>
		[Label("计划开始")]
		public static readonly Property<DateTime?> PlanBeginProperty = P<TaskInfo>.Register(e => e.PlanBegin);

		/// <summary>
		/// 计划开始
		/// </summary>
		public DateTime? PlanBegin
		{
			get { return GetProperty(PlanBeginProperty); }
			set { SetProperty(PlanBeginProperty, value); }
		}
		#endregion

		#region 计划结束 PlanEnd
		/// <summary>
		/// 计划结束
		/// </summary>
		[Label("计划结束")]
		public static readonly Property<DateTime?> PlanEndProperty = P<TaskInfo>.Register(e => e.PlanEnd);

		/// <summary>
		/// 计划结束
		/// </summary>
		public DateTime? PlanEnd
		{
			get { return GetProperty(PlanEndProperty); }
			set { SetProperty(PlanEndProperty, value); }
		}
		#endregion

		#region 实际开始 ActualStart
		/// <summary>
		/// 实际开始
		/// </summary>
		[Label("实际开始")]
		public static readonly Property<DateTime?> ActualStartProperty = P<TaskInfo>.Register(e => e.ActualStart);

		/// <summary>
		/// 实际开始
		/// </summary>
		public DateTime? ActualStart
		{
			get { return GetProperty(ActualStartProperty); }
			set { SetProperty(ActualStartProperty, value); }
		}
		#endregion

		#region 实际结束 ActualEnd
		/// <summary>
		/// 实际结束
		/// </summary>
		[Label("实际结束")]
		public static readonly Property<DateTime?> ActualEndProperty = P<TaskInfo>.Register(e => e.ActualEnd);

		/// <summary>
		/// 实际结束
		/// </summary>
		public DateTime? ActualEnd
		{
			get { return GetProperty(ActualEndProperty); }
			set { SetProperty(ActualEndProperty, value); }
		}
		#endregion

		#region 参数 Parameter
		/// <summary>
		/// 参数
		/// </summary>
		[MaxLength(2000)]
		[Label("参数")]
		public static readonly Property<string> ParameterProperty = P<TaskInfo>.Register(e => e.Parameter);

		/// <summary>
		/// 参数
		/// </summary>
		public string Parameter
		{
			get { return GetProperty(ParameterProperty); }
			set { SetProperty(ParameterProperty, value); }
		}
        #endregion

        #region 重要程度 Importance
        /// <summary>
        /// 重要程度
        /// </summary>
        [Label("重要程度")]
		public static readonly Property<TaskImportance> ImportanceProperty = P<TaskInfo>.Register(e => e.Importance);

        /// <summary>
        /// 重要程度
        /// </summary>
        public TaskImportance Importance
		{
			get { return GetProperty(ImportanceProperty); }
			set { SetProperty(ImportanceProperty, value); }
		}
		#endregion

		#region 任务状态 Status
		/// <summary>
		/// 任务状态
		/// </summary>
		[Label("任务状态")]
		public static readonly Property<TaskStatus> StatusProperty = P<TaskInfo>.Register(e => e.Status);

		/// <summary>
		/// 任务状态
		/// </summary>
		public TaskStatus Status
		{
			get { return GetProperty(StatusProperty); }
			set { SetProperty(StatusProperty, value); }
		}
		#endregion

		#region  Notifications
		/// <summary>
		/// 
		/// </summary>
		[Label("通知方式")]
		public static readonly Property<TaskNotifications> NotificationsProperty = P<TaskInfo>.Register(e => e.Notifications);

		/// <summary>
		/// 
		/// </summary>
		public TaskNotifications Notifications
		{
			get { return GetProperty(NotificationsProperty); }
			set { SetProperty(NotificationsProperty, value); }
		}
        #endregion

        #region  TaskType
        /// <summary>
        /// Id
        /// </summary>
        [Label("任务类型")]
        public static readonly IRefIdProperty TaskTypeIdProperty = P<TaskInfo>.RegisterRefId(e => e.TaskTypeId, ReferenceType.Normal);

		/// <summary>
		/// Id
		/// </summary>
		public double TaskTypeId
		{
			get { return (double)GetRefId(TaskTypeIdProperty); }
			set { SetRefId(TaskTypeIdProperty, value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public static readonly RefEntityProperty<TaskType> TaskTypeProperty = P<TaskInfo>.RegisterRef(e => e.TaskType, TaskTypeIdProperty);

		/// <summary>
		/// 
		/// </summary>
		public TaskType TaskType
		{
			get { return GetRefEntity(TaskTypeProperty); }
			set { SetRefEntity(TaskTypeProperty, value); }
		}
        #endregion

        #region 负责人 AssignTo
        /// <summary>
        /// 负责人
        /// </summary>
        [Label("负责人")]
        public static readonly IRefIdProperty AssignToIdProperty =
            P<TaskInfo>.RegisterRefId(e => e.AssignToId, ReferenceType.Normal);

        /// <summary>
        /// 负责人
        /// </summary>
        public double? AssignToId
        {
            get { return (double?)this.GetRefNullableId(AssignToIdProperty); }
            set { this.SetRefNullableId(AssignToIdProperty, value); }
        }

        /// <summary>
        /// 负责人
        /// </summary>
        public static readonly RefEntityProperty<Employee> AssignToProperty =
            P<TaskInfo>.RegisterRef(e => e.AssignTo, AssignToIdProperty);

        /// <summary>
        /// 负责人
        /// </summary>
        public Employee AssignTo
        {
            get { return this.GetRefEntity(AssignToProperty); }
            set { this.SetRefEntity(AssignToProperty, value); }
        }
        #endregion

        #region 负责人 AssignToName
        /// <summary>
        /// 负责人
        /// </summary>
        [Label("负责人")]
        public static readonly Property<string> AssignToNameProperty = P<TaskInfo>.RegisterView(e => e.AssignToName, p => p.AssignTo.Name);

        /// <summary>
        /// 负责人
        /// </summary>
        public string AssignToName
        {
            get { return this.GetProperty(AssignToNameProperty); }
        }
        #endregion

        #region 抄送人 CopyTo
        /// <summary>
        /// 抄送人Id
        /// </summary>
        [Label("抄送人")]
        public static readonly IRefIdProperty CopyToIdProperty = P<TaskInfo>.RegisterRefId(e => e.CopyToId, ReferenceType.Normal);

        /// <summary>
        /// 抄送人Id
        /// </summary>
        public double? CopyToId
        {
            get { return (double?)GetRefNullableId(CopyToIdProperty); }
            set { SetRefNullableId(CopyToIdProperty, value); }
        }

        /// <summary>
        /// 抄送人
        /// </summary>
        public static readonly RefEntityProperty<Employee> CopyToProperty = P<TaskInfo>.RegisterRef(e => e.CopyTo, CopyToIdProperty);

        /// <summary>
        /// 抄送人
        /// </summary>
        public Employee CopyTo
        {
            get { return GetRefEntity(CopyToProperty); }
            set { SetRefEntity(CopyToProperty, value); }
        }
        #endregion

        #region 类型图标 TypeIcon
        /// <summary>
        /// 类型图标
        /// </summary>
        [Label("类型图标")]
        public static readonly Property<string> TypeIconProperty = P<TaskInfo>.RegisterView(e => e.TypeIcon, p => p.TaskType.Icon);

        /// <summary>
        /// 类型图标
        /// </summary>
        public string TypeIcon
        {
            get { return this.GetProperty(TypeIconProperty); }
        }
        #endregion

        #region 类型名称 TypeName
        /// <summary>
        /// 类型名称
        /// </summary>
        [Label("类型名称")]
        public static readonly Property<string> TypeNameProperty = P<TaskInfo>.RegisterView(e => e.TypeName, p => p.TaskType.Name);

        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName
        {
            get { return this.GetProperty(TypeNameProperty); }
        }
        #endregion

        #region 图片 Pic
        /// <summary>
        /// 图片
        /// </summary>
        [Label("图片")]
        public static readonly Property<byte[]> PicProperty = P<TaskInfo>.Register(e => e.Pic);

        /// <summary>
        /// 图片
        /// </summary>
        public byte[] Pic
        {
            get { return this.GetProperty(PicProperty); }
            set { this.SetProperty(PicProperty, value); }
        }
        #endregion
    }

	/// <summary>
	/// 任务信息 实体配置
	/// </summary>
	internal class TaskInfoConfig : EntityConfig<TaskInfo>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("WB_TASK").MapAllProperties();
			Meta.Property(TaskInfo.ContentProperty).ColumnMeta.HasLength(4000);
			Meta.Property(TaskInfo.ParameterProperty).ColumnMeta.HasLength(4000);
			Meta.EnablePhantoms();
		}
	}
}