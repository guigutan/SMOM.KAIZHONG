using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Core.QmsStaticConst
{
    /// <summary>
    /// t值
    /// </summary>
    [ChildEntity, Serializable]
	//[CriteriaQuery]
	[Label("t值")]
	public partial class StaticConstT : StaticConstValueBase
	{
		#region 阿尔法 Alpha
		/// <summary>
		/// 阿尔法
		/// </summary>
		[Label("α")]
		public static readonly Property<double> AlphaProperty = P<StaticConstT>.Register(e => e.Alpha);

		/// <summary>
		/// 阿尔法
		/// </summary>
		public double Alpha
		{
			get { return GetProperty(AlphaProperty); }
			set { SetProperty(AlphaProperty, value); }
		}
		#endregion

		#region n SampleQty
		/// <summary>
		/// n
		/// </summary>
		[Label("n")]
		public static readonly Property<int> SampleQtyProperty = P<StaticConstT>.Register(e => e.SampleQty);

		/// <summary>
		/// n
		/// </summary>
		public int SampleQty
		{
			get { return GetProperty(SampleQtyProperty); }
			set { SetProperty(SampleQtyProperty, value); }
		}
		#endregion

		#region 数值 Value
		/// <summary>
		/// 数值
		/// </summary>
		[Label("数值")]
		public static readonly Property<double> ValueProperty = P<StaticConstT>.Register(e => e.Value);

		/// <summary>
		/// 数值
		/// </summary>
		public double Value
		{
			get { return GetProperty(ValueProperty); }
			set { SetProperty(ValueProperty, value); }
		}
		#endregion

		#region t值集合 MsaConst
		/// <summary>
		/// t值集合Id
		/// </summary>
		public static readonly IRefIdProperty MsaConstIdProperty = P<StaticConstT>.RegisterRefId(e => e.MsaConstId, ReferenceType.Parent);

		/// <summary>
		/// t值集合Id
		/// </summary>
		public double MsaConstId
		{
			get { return (double)GetRefId(MsaConstIdProperty); }
			set { SetRefId(MsaConstIdProperty, value); }
		}

		/// <summary>
		/// t值集合
		/// </summary>
		public static readonly RefEntityProperty<StaticConst> MsaConstProperty = P<StaticConstT>.RegisterRef(e => e.MsaConst, MsaConstIdProperty);

		/// <summary>
		/// t值集合
		/// </summary>
		public StaticConst MsaConst
		{
			get { return GetRefEntity(MsaConstProperty); }
			set { SetRefEntity(MsaConstProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// t值 实体配置
	/// </summary>
	internal class MsaConstTConfig : EntityConfig<StaticConstT>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("MSA_CONST_T").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}