using SIE;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.RedCardManagment.RedCards
{
	/// <summary>
	/// 物料SN追溯清单
	/// </summary>
	[RootEntity, Serializable]
	//[CriteriaQuery]
	[Label("物料SN追溯清单")]
	public partial class ItemSnRetroactive : RetroactiveBase
	{

		#region 序列号 SN
		/// <summary>
		/// 序列号
		/// </summary>
		[Label("物料SN")]
		public static readonly Property<string> SNProperty = P<ItemSnRetroactive>.Register(e => e.SN);

		/// <summary>
		/// 序列号
		/// </summary>
		public string SN
		{
			get { return GetProperty(SNProperty); }
			set { SetProperty(SNProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 物料SN追溯清单 实体配置
	/// </summary>
	internal class ItemSnRetroactiveConfig : EntityConfig<ItemSnRetroactive>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("QMS_RCD_ITEM_SN_REACT").MapAllProperties();
            Meta.Property(ItemSnRetroactive.WmsKeyProperty).ColumnMeta.HasLength(600);
            Meta.EnablePhantoms();
		}
	}
}