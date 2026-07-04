using SIE;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Core.QmsStaticConst
{
	/// <summary>
	/// d2*表
	/// </summary>
	[ChildEntity, Serializable]
	//[CriteriaQuery]
	[Label("d2*表")]
	public partial class StaticConstD2 : StaticConstValueBase
	{
		#region 子组容量 SampleQty
		/// <summary>
		/// 子组容量
		/// </summary>
		[Label("子组容量")]
		public static readonly Property<int?> SampleQtyProperty = P<StaticConstD2>.Register(e => e.SampleQty);

		/// <summary>
		/// 子组容量
		/// </summary>
		public int? SampleQty
		{
			get { return GetProperty(SampleQtyProperty); }
			set { SetProperty(SampleQtyProperty, value); }
		}
		#endregion

		#region 测量次数 TestQty
		/// <summary>
		/// 测量次数
		/// </summary>
		[Label("测量次数")]
		public static readonly Property<int> TestQtyProperty = P<StaticConstD2>.Register(e => e.TestQty);

		/// <summary>
		/// 测量次数
		/// </summary>
		public int TestQty
		{
			get { return GetProperty(TestQtyProperty); }
			set { SetProperty(TestQtyProperty, value); }
		}
		#endregion

		#region 数值 Value
		/// <summary>
		/// 数值
		/// </summary>
		[Label("数值")]
		public static readonly Property<double> ValueProperty = P<StaticConstD2>.Register(e => e.Value);

		/// <summary>
		/// 数值
		/// </summary>
		public double Value
		{
			get { return GetProperty(ValueProperty); }
			set { SetProperty(ValueProperty, value); }
		}
		#endregion

		#region d2*表类型 MsaConstD2Type
		/// <summary>
		/// d2*表类型
		/// </summary>
		[Label("d2*表类型")]
		public static readonly Property<StaticConstD2Type> MsaConstD2TypeProperty = P<StaticConstD2>.Register(e => e.MsaConstD2Type);

		/// <summary>
		/// d2*表类型
		/// </summary>
		public StaticConstD2Type MsaConstD2Type
		{
			get { return GetProperty(MsaConstD2TypeProperty); }
			set { SetProperty(MsaConstD2TypeProperty, value); }
		}
		#endregion

		#region d2表集合 MsaConst
		/// <summary>
		/// d2表集合Id
		/// </summary>
		public static readonly IRefIdProperty MsaConstIdProperty = P<StaticConstD2>.RegisterRefId(e => e.MsaConstId, ReferenceType.Parent);

		/// <summary>
		/// d2表集合Id
		/// </summary>
		public double MsaConstId
		{
			get { return (double)GetRefId(MsaConstIdProperty); }
			set { SetRefId(MsaConstIdProperty, value); }
		}

		/// <summary>
		/// d2表集合
		/// </summary>
		public static readonly RefEntityProperty<StaticConst> MsaConstProperty = P<StaticConstD2>.RegisterRef(e => e.MsaConst, MsaConstIdProperty);

		/// <summary>
		/// d2表集合
		/// </summary>
		public StaticConst MsaConst
		{
			get { return GetRefEntity(MsaConstProperty); }
			set { SetRefEntity(MsaConstProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// d2*表 实体配置
	/// </summary>
	internal class MsaConstD2Config : EntityConfig<StaticConstD2>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("MSA_CONST_D2").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}