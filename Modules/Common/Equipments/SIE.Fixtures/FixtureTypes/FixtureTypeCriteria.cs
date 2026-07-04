using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Fixtures.FixtureTypes
{
    /// <summary>
    /// 工治具型号查询条件
    /// </summary>
    [QueryEntity, Serializable]
	[Label("工治具编码查询实体")]
	public class FixtureTypeCriteria: Criteria
    {
		#region 编码 Code
		/// <summary>
		/// 编码
		/// </summary>
		[Label("编码")]
		public static readonly Property<string> CodeProperty = P<FixtureTypeCriteria>.Register(e => e.Code);

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
		public static readonly Property<string> NameProperty = P<FixtureTypeCriteria>.Register(e => e.Name);

		/// <summary>
		/// 名称
		/// </summary>
		public string Name
		{
			get { return GetProperty(NameProperty); }
			set { SetProperty(NameProperty, value); }
		}
		#endregion

		/// <summary>
		/// 获取数据
		/// </summary>
		/// <returns></returns>
		protected override EntityList Fetch()
		{
			return RT.Service.Resolve<CoreFixtureController>().GetFixtureTypeList(this);
		}
	}
}
