using SIE;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Core.QmsStaticConst
{
	/// <summary>
	/// K2
	/// </summary>
	[ChildEntity, Serializable]
	//[CriteriaQuery]
	[Label("K2")]
	public partial class StaticConstK2 : StaticConstValueBase
	{
		#region 评价人数 EvaluateQty
		/// <summary>
		/// 评价人数
		/// </summary>
		[Label("评价人数")]
		[Required]
		public static readonly Property<int> EvaluateQtyProperty = P<StaticConstK2>.Register(e => e.EvaluateQty);

		/// <summary>
		/// 评价人数
		/// </summary>
		public int EvaluateQty
		{
			get { return GetProperty(EvaluateQtyProperty); }
			set { SetProperty(EvaluateQtyProperty, value); }
		}
		#endregion

		#region K2 Value
		/// <summary>
		/// K2
		/// </summary>
		[Label("K2")]
		[Required]
		public static readonly Property<double?> ValueProperty = P<StaticConstK2>.Register(e => e.Value);

		/// <summary>
		/// K2
		/// </summary>
		public double? Value
		{
			get { return GetProperty(ValueProperty); }
			set { SetProperty(ValueProperty, value); }
		}
		#endregion

		#region K2集合 MsaConst
		/// <summary>
		/// K2集合Id
		/// </summary>
		public static readonly IRefIdProperty MsaConstIdProperty = P<StaticConstK2>.RegisterRefId(e => e.MsaConstId, ReferenceType.Parent);

		/// <summary>
		/// K2集合Id
		/// </summary>
		public double MsaConstId
		{
			get { return (double)GetRefId(MsaConstIdProperty); }
			set { SetRefId(MsaConstIdProperty, value); }
		}

		/// <summary>
		/// K2集合
		/// </summary>
		public static readonly RefEntityProperty<StaticConst> MsaConstProperty = P<StaticConstK2>.RegisterRef(e => e.MsaConst, MsaConstIdProperty);

		/// <summary>
		/// K2集合
		/// </summary>
		public StaticConst MsaConst
		{
			get { return GetRefEntity(MsaConstProperty); }
			set { SetRefEntity(MsaConstProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// K2 实体配置
	/// </summary>
	internal class MsaConstK2Config : EntityConfig<StaticConstK2>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("MSA_CONST_K2").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}