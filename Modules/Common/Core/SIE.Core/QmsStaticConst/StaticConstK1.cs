using SIE;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Core.QmsStaticConst
{
	/// <summary>
	/// K1
	/// </summary>
	[ChildEntity, Serializable]
	//[CriteriaQuery]
	[Label("K1")]
	public partial class StaticConstK1 : StaticConstValueBase
	{
		#region 试验次数 TestQty
		/// <summary>
		/// 试验次数
		/// </summary>
		[Required]
		[Label("试验次数")]
		public static readonly Property<int> TestQtyProperty = P<StaticConstK1>.Register(e => e.TestQty);

		/// <summary>
		/// 试验次数
		/// </summary>
		public int TestQty
		{
			get { return GetProperty(TestQtyProperty); }
			set { SetProperty(TestQtyProperty, value); }
		}
		#endregion

		#region K1 Value
		/// <summary>
		/// K1
		/// </summary>
		[Label("K1")]
		[Required]
		public static readonly Property<double?> ValueProperty = P<StaticConstK1>.Register(e => e.Value);

		/// <summary>
		/// K1
		/// </summary>
		public double? Value
		{
			get { return GetProperty(ValueProperty); }
			set { SetProperty(ValueProperty, value); }
		}
		#endregion

		#region K1集合 MsaConst
		/// <summary>
		/// K1集合Id
		/// </summary>
		public static readonly IRefIdProperty MsaConstIdProperty = P<StaticConstK1>.RegisterRefId(e => e.MsaConstId, ReferenceType.Parent);

		/// <summary>
		/// K1集合Id
		/// </summary>
		public double MsaConstId
		{
			get { return (double)GetRefId(MsaConstIdProperty); }
			set { SetRefId(MsaConstIdProperty, value); }
		}

		/// <summary>
		/// K1集合
		/// </summary>
		public static readonly RefEntityProperty<StaticConst> MsaConstProperty = P<StaticConstK1>.RegisterRef(e => e.MsaConst, MsaConstIdProperty);

		/// <summary>
		/// K1集合
		/// </summary>
		public StaticConst MsaConst
		{
			get { return GetRefEntity(MsaConstProperty); }
			set { SetRefEntity(MsaConstProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// K1 实体配置
	/// </summary>
	internal class MsaConstK1Config : EntityConfig<StaticConstK1>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("MSA_CONST_K1").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}