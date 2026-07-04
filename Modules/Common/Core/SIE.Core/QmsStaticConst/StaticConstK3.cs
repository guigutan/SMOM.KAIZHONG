using SIE;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Core.QmsStaticConst
{
	/// <summary>
	/// K3
	/// </summary>
	[ChildEntity, Serializable]
	//[CriteriaQuery]
	[Label("K3")]
	public partial class StaticConstK3 :StaticConstValueBase
	{
		#region 样本数量 SampleQty
		/// <summary>
		/// 样本数量
		/// </summary>
		[Label("样本数量")]
		[Required]
		public static readonly Property<int> SampleQtyProperty = P<StaticConstK3>.Register(e => e.SampleQty);

		/// <summary>
		/// 样本数量
		/// </summary>
		public int SampleQty
		{
			get { return GetProperty(SampleQtyProperty); }
			set { SetProperty(SampleQtyProperty, value); }
		}
		#endregion

		#region K3 Value
		/// <summary>
		/// K3
		/// </summary>
		[Label("K3")]
		[Required]
		public static readonly Property<double?> ValueProperty = P<StaticConstK3>.Register(e => e.Value);

		/// <summary>
		/// K3
		/// </summary>
		public double? Value
		{
			get { return GetProperty(ValueProperty); }
			set { SetProperty(ValueProperty, value); }
		}
		#endregion

		#region K3集合 MsaConst
		/// <summary>
		/// K3集合Id
		/// </summary>
		public static readonly IRefIdProperty MsaConstIdProperty = P<StaticConstK3>.RegisterRefId(e => e.MsaConstId, ReferenceType.Parent);

		/// <summary>
		/// K3集合Id
		/// </summary>
		public double MsaConstId
		{
			get { return (double)GetRefId(MsaConstIdProperty); }
			set { SetRefId(MsaConstIdProperty, value); }
		}

		/// <summary>
		/// K3集合
		/// </summary>
		public static readonly RefEntityProperty<StaticConst> MsaConstProperty = P<StaticConstK3>.RegisterRef(e => e.MsaConst, MsaConstIdProperty);

		/// <summary>
		/// K3集合
		/// </summary>
		public StaticConst MsaConst
		{
			get { return GetRefEntity(MsaConstProperty); }
			set { SetRefEntity(MsaConstProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// K3 实体配置
	/// </summary>
	internal class MsaConstK3Config : EntityConfig<StaticConstK3>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("MSA_CONST_K3").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}