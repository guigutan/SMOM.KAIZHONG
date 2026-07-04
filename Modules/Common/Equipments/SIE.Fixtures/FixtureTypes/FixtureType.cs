using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Fixtures.FixtureTypes
{
    /// <summary>
    /// 工治具类型
    /// </summary>
    [RootEntity, Serializable]
	[ConditionQueryType(typeof(FixtureTypeCriteria))]
	[DisplayMember(nameof(Code))]
	[Label("工治具类型")]
	public partial class FixtureType : DataEntity
	{
		#region 编码 Code
		/// <summary>
		/// 编码
		/// </summary>
		[Label("编码")]
		[Required]
		[NotDuplicate]
		public static readonly Property<string> CodeProperty = P<FixtureType>.Register(e => e.Code);

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
		[Label("名称")]
		[Required]
		[NotDuplicate]
		public static readonly Property<string> NameProperty = P<FixtureType>.Register(e => e.Name);

		/// <summary>
		/// 名称
		/// </summary>
		public string Name
		{
			get { return GetProperty(NameProperty); }
			set { SetProperty(NameProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 工治具类型 实体配置
	/// </summary>
	internal class FixtureTypeConfig : EntityConfig<FixtureType>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_FIXTURE_TYPE").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}