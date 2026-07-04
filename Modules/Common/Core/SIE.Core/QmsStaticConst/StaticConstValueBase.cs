using SIE;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Core.QmsStaticConst
{
	/// <summary>
	/// MSA参数基类
	/// </summary>
	[RootEntity, Serializable]
	//[CriteriaQuery]
	[Label("MSA参数基类")]
	public  partial class StaticConstValueBase : DataEntity
	{

		#region 是否预设数据 IsFixed
		/// <summary>
		/// 是否预设数据
		/// </summary>
		[Label("是否预设数据")]
		public static readonly Property<bool> IsFixedProperty = P<StaticConstValueBase>.Register(e => e.IsFixed);

		/// <summary>
		/// 是否预设数据
		/// </summary>
		public bool IsFixed
		{
			get { return GetProperty(IsFixedProperty); }
			set { SetProperty(IsFixedProperty, value); }
		}
		#endregion
	}
}