using SIE; 
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Fixtures.Enums
{
	/// <summary>
	/// 行业属性
	/// </summary>
	public enum IndustryProperties
	{
		/// <summary>
		/// 通用
		/// </summary>
		[Label("通用")]
		General = 10,
		/// <summary>
		/// 电子
		/// </summary>
		[Label("电子")]
		Electronic = 20,
	}
}