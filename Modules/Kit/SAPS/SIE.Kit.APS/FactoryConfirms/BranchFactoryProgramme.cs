using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Kit.APS.FactoryConfirms
{
    /// <summary>
    /// 分厂方案设置
    /// </summary>
    [RootEntity, Serializable]
	//[CriteriaQuery]
	[Label("分厂方案设置")]
	public partial class BranchFactoryProgramme : DataEntity
    {
		#region 编码 Code
		/// <summary>
		/// 编码
		/// </summary>
		[Required]
		[NotDuplicate]
		[Label("编码")]
		public static readonly Property<string> CodeProperty = P<BranchFactoryProgramme>.Register(e => e.Code);

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
		[Label("名称")]
		public static readonly Property<string> NameProperty = P<BranchFactoryProgramme>.Register(e => e.Name);

		/// <summary>
		/// 名称
		/// </summary>
		public string Name
		{
			get { return GetProperty(NameProperty); }
			set { SetProperty(NameProperty, value); }
		}
		#endregion

		#region 备注 Remark
		/// <summary>
		/// 备注
		/// </summary>
		[Label("备注")]
		public static readonly Property<string> RemarkProperty = P<BranchFactoryProgramme>.Register(e => e.Remark);

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark
		{
			get { return GetProperty(RemarkProperty); }
			set { SetProperty(RemarkProperty, value); }
		}
		#endregion

		#region 分厂方案设置明细列表 BranchFactiryProgrammeDetailList
		/// <summary>
		/// 分厂方案设置明细列表
		/// </summary>
		public static readonly ListProperty<EntityList<BranchFactoryProgrammeDetail>> BranchFactiryProgrammeDetailListProperty = P<BranchFactoryProgramme>.RegisterList(e => e.BranchFactiryProgrammeDetailList);
		/// <summary>
		/// 分厂方案设置明细列表
		/// </summary>
		public EntityList<BranchFactoryProgrammeDetail> BranchFactiryProgrammeDetailList
		{
			get { return this.GetLazyList(BranchFactiryProgrammeDetailListProperty); }
		}
		#endregion
	}

	/// <summary>
	/// 分厂方案设置 实体配置
	/// </summary>
	internal class BranchFactoryProgrammeConfig : EntityConfig<BranchFactoryProgramme>
	{
		/// <summary>
      	/// 配置元数据
    	 /// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("APS_BRANCH_FACTORY").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}