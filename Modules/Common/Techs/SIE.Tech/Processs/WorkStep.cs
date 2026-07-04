using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Tech.Processs
{
	/// <summary>
	/// 工步
	/// </summary>
	[ChildEntity, Serializable]
	//[CriteriaQuery]
	[Label("工步")]
	[DisplayMember(nameof(Name))]
	public partial class WorkStep : DataEntity
	{
		#region 编码 Code
		/// <summary>
		/// 编码
		/// </summary>
		[Required]
		[MaxLength(50)]
		[Label("编码")]
		public static readonly Property<string> CodeProperty = P<WorkStep>.Register(e => e.Code);

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
		[MaxLength(100)]
		[Label("名称")]
		public static readonly Property<string> NameProperty = P<WorkStep>.Register(e => e.Name);

		/// <summary>
		/// 名称
		/// </summary>
		public string Name
		{
			get { return GetProperty(NameProperty); }
			set { SetProperty(NameProperty, value); }
		}
		#endregion

		#region 顺序 SeqNumber
		/// <summary>
		/// 顺序
		/// </summary>
		[Required]
		[MinValue(1)]
		[Label("顺序")]
		public static readonly Property<int> SeqNumberProperty = P<WorkStep>.Register(e => e.SeqNumber);

		/// <summary>
		/// 顺序
		/// </summary>
		public int SeqNumber
		{
			get { return GetProperty(SeqNumberProperty); }
			set { SetProperty(SeqNumberProperty, value); }
		}
		#endregion

		#region 工序与工步关系 Process
		/// <summary>
		/// 工序与工步关系Id
		/// </summary>
		[Label("工序")]
		public static readonly IRefIdProperty ProcessIdProperty = P<WorkStep>.RegisterRefId(e => e.ProcessId, ReferenceType.Parent);

		/// <summary>
		/// 工序与工步关系Id
		/// </summary>
		public double ProcessId
		{
			get { return (double)GetRefId(ProcessIdProperty); }
			set { SetRefId(ProcessIdProperty, value); }
		}

		/// <summary>
		/// 工序与工步关系
		/// </summary>
		public static readonly RefEntityProperty<Process> ProcessProperty = P<WorkStep>.RegisterRef(e => e.Process, ProcessIdProperty);

		/// <summary>
		/// 工序与工步关系
		/// </summary>
		public Process Process
		{
			get { return GetRefEntity(ProcessProperty); }
			set { SetRefEntity(ProcessProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 工步 实体配置
	/// </summary>
	internal class WorkStepConfig : EntityConfig<WorkStep>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("TECH_WORK_STEP").MapAllProperties();
			Meta.Property(WorkStep.CodeProperty).MapColumn().HasLength(100).HasIndex(IndexTypeMeta.Indexed);
			Meta.Property(WorkStep.ProcessIdProperty).MapColumn().HasIndex(IndexTypeMeta.Indexed);
			Meta.Property(WorkStep.NameProperty).MapColumn().HasLength(200);
			Meta.EnablePhantoms();
		}
	}
}