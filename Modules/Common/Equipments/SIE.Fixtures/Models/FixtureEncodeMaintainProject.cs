using SIE;
using SIE.Defects.InspectionItems;
using SIE.Domain;
using SIE.Fixtures.Projects;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Fixtures.Models
{
	/// <summary>
	/// 工治具编码保养项目
	/// </summary>
	[ChildEntity, Serializable]
	//[CriteriaQuery]
	[Label("工治具编码保养项目")]
	public partial class FixtureEncodeMaintainProject : DataEntity
	{
		#region 入库保养 InStorageMaintain
		/// <summary>
		/// 入库保养
		/// </summary>
		[Label("入库保养")]
		public static readonly Property<bool> InStorageMaintainProperty = P<FixtureEncodeMaintainProject>.Register(e => e.InStorageMaintain);

		/// <summary>
		/// 入库保养
		/// </summary>
		public bool InStorageMaintain
		{
			get { return GetProperty(InStorageMaintainProperty); }
			set { SetProperty(InStorageMaintainProperty, value); }
		}
		#endregion

		#region 常规保养 CommonMaintain
		/// <summary>
		/// 常规保养
		/// </summary>
		[Label("常规保养")]
		public static readonly Property<bool> CommonMaintainProperty = P<FixtureEncodeMaintainProject>.Register(e => e.CommonMaintain);

		/// <summary>
		/// 常规保养
		/// </summary>
		public bool CommonMaintain
		{
			get { return GetProperty(CommonMaintainProperty); }
			set { SetProperty(CommonMaintainProperty, value); }
		}
		#endregion

		#region 上线定期保养 OnlineMaintain
		/// <summary>
		/// 上线定期保养
		/// </summary>
		[Label("上线定期保养")]
		public static readonly Property<bool> OnlineMaintainProperty = P<FixtureEncodeMaintainProject>.Register(e => e.OnlineMaintain);

		/// <summary>
		/// 上线定期保养
		/// </summary>
		public bool OnlineMaintain
		{
			get { return GetProperty(OnlineMaintainProperty); }
			set { SetProperty(OnlineMaintainProperty, value); }
		}
		#endregion

		#region 出库保养 ToStorageMaintain
		/// <summary>
		/// 出库保养
		/// </summary>
		[Label("出库保养")]
		public static readonly Property<bool> ToStorageMaintainProperty = P<FixtureEncodeMaintainProject>.Register(e => e.ToStorageMaintain);

		/// <summary>
		/// 出库保养
		/// </summary>
		public bool ToStorageMaintain
		{
			get { return GetProperty(ToStorageMaintainProperty); }
			set { SetProperty(ToStorageMaintainProperty, value); }
		}
		#endregion

		#region 验收项目 Acceptance
		/// <summary>
		/// 验收项目
		/// </summary>
		[Label("验收项目")]
		public static readonly Property<bool> AcceptanceProperty = P<FixtureEncodeMaintainProject>.Register(e => e.Acceptance);

		/// <summary>
		/// 验收项目
		/// </summary>
		public bool Acceptance
		{
			get { return GetProperty(AcceptanceProperty); }
			set { SetProperty(AcceptanceProperty, value); }
		}
		#endregion

		#region 检测合格最小值 MinValue
		/// <summary>
		/// 检测合格最小值
		/// </summary>
		[Label("检测合格最小值")]
		public static readonly Property<decimal?> MinValueProperty = P<FixtureEncodeMaintainProject>.Register(e => e.MinValue);

		/// <summary>
		/// 检测合格最小值
		/// </summary>
		public decimal? MinValue
		{
			get { return GetProperty(MinValueProperty); }
			set { SetProperty(MinValueProperty, value); }
		}
		#endregion

		#region 检测合格最大值 MaxValue
		/// <summary>
		/// 检测合格最大值
		/// </summary>
		[Label("检测合格最大值")]
		public static readonly Property<decimal?> MaxValueProperty = P<FixtureEncodeMaintainProject>.Register(e => e.MaxValue);

		/// <summary>
		/// 检测合格最大值
		/// </summary>
		public decimal? MaxValue
		{
			get { return GetProperty(MaxValueProperty); }
			set { SetProperty(MaxValueProperty, value); }
		}
		#endregion

		#region  MaintainProject
		/// <summary>
		/// Id
		/// </summary>
		public static readonly IRefIdProperty MaintainProjectIdProperty = P<FixtureEncodeMaintainProject>.RegisterRefId(e => e.MaintainProjectId, ReferenceType.Normal);

		/// <summary>
		/// Id
		/// </summary>
		public double MaintainProjectId
		{
			get { return (double)GetRefId(MaintainProjectIdProperty); }
			set { SetRefId(MaintainProjectIdProperty, value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public static readonly RefEntityProperty<MaintainProject> MaintainProjectProperty = P<FixtureEncodeMaintainProject>.RegisterRef(e => e.MaintainProject, MaintainProjectIdProperty);

		/// <summary>
		/// 
		/// </summary>
		public MaintainProject MaintainProject
		{
			get { return GetRefEntity(MaintainProjectProperty); }
			set { SetRefEntity(MaintainProjectProperty, value); }
		}
		#endregion

		#region 工治具编码 FixtureEncode
		/// <summary>
		/// 工治具编码Id
		/// </summary>
		public static readonly IRefIdProperty FixtureEncodeIdProperty = P<FixtureEncodeMaintainProject>.RegisterRefId(e => e.FixtureEncodeId, ReferenceType.Parent);

		/// <summary>
		/// 工治具编码Id
		/// </summary>
		public double FixtureEncodeId
		{
			get { return (double)GetRefId(FixtureEncodeIdProperty); }
			set { SetRefId(FixtureEncodeIdProperty, value); }
		}

		/// <summary>
		/// 工治具编码
		/// </summary>
		public static readonly RefEntityProperty<FixtureEncode> FixtureEncodeProperty = P<FixtureEncodeMaintainProject>.RegisterRef(e => e.FixtureEncode, FixtureEncodeIdProperty);

		/// <summary>
		/// 工治具编码
		/// </summary>
		public FixtureEncode FixtureEncode
		{
			get { return GetRefEntity(FixtureEncodeProperty); }
			set { SetRefEntity(FixtureEncodeProperty, value); }
		}
		#endregion


		#region 视图属性

		#region 保养项目名称 MaintainProjectName
		/// <summary>
		/// 保养项目名称
		/// </summary>
		[Label("保养项目名称")]
		public static readonly Property<string> MaintainProjectNameProperty = P<FixtureEncodeMaintainProject>.RegisterView(e => e.MaintainProjectName, p => p.MaintainProject.Name);

		/// <summary>
		/// 保养项目名称
		/// </summary>
		public string MaintainProjectName
		{
			get { return this.GetProperty(MaintainProjectNameProperty); }
		}
		#endregion

		#region 耗材 ProjectConsumable
		/// <summary>
		/// 耗材
		/// </summary>
		[Label("耗材")]
		public static readonly Property<string> ProjectConsumableProperty = P<FixtureEncodeMaintainProject>.RegisterView(e => e.ProjectConsumable, p => p.MaintainProject.Consumable);

		/// <summary>
		/// 耗材
		/// </summary>
		public string ProjectConsumable
		{
			get { return this.GetProperty(ProjectConsumableProperty); }
		}
		#endregion

		#region 方法 ProjectMethod
		/// <summary>
		/// 方法
		/// </summary>
		[Label("方法")]
		public static readonly Property<string> ProjectMethodProperty = P<FixtureEncodeMaintainProject>.RegisterView(e => e.ProjectMethod, p => p.MaintainProject.Method);

		/// <summary>
		/// 方法
		/// </summary>
		public string ProjectMethod
		{
			get { return this.GetProperty(ProjectMethodProperty); }
		}
		#endregion

		#region 工具 ProjectTool
		/// <summary>
		/// 工具
		/// </summary>
		[Label("工具")]
		public static readonly Property<string> ProjectToolProperty = P<FixtureEncodeMaintainProject>.RegisterView(e => e.ProjectTool, p => p.MaintainProject.Tool);

		/// <summary>
		/// 工具
		/// </summary>
		public string ProjectTool
		{
			get { return this.GetProperty(ProjectToolProperty); }
		}
		#endregion

		#endregion

		#region 检验标识 CheckTag	
		/// <summary>
		/// 检验标识
		/// </summary>
		[Label("检验标识")]
        public static readonly Property<CheckTag> CheckTagProperty = P<FixtureEncodeMaintainProject>.Register(e => e.CheckTag);

		/// <summary>
		/// 检验标识
		/// </summary>
		public CheckTag CheckTag
		{
            get { return this.GetProperty(CheckTagProperty); }
            set { this.SetProperty(CheckTagProperty, value); }
        }
        #endregion

    }

	/// <summary>
	/// 工治具编码保养项目 实体配置
	/// </summary>
	internal class FixtureEncodeMaintainProjectConfig : EntityConfig<FixtureEncodeMaintainProject>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_CODE_MAINTAIN_PRJ").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}