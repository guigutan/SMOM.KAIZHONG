using SIE;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using System;

namespace SIE.RedCardManagment.RedCards
{
	/// <summary>
	/// 物料批次追溯清单
	/// </summary>
	[RootEntity, Serializable]
	//[CriteriaQuery]
	[Label("物料批次追溯清单")]
	public partial class BatchRetroactive : RetroactiveBase
	{
	
	}

	/// <summary>
	/// 物料批次追溯清单 实体配置
	/// </summary>
	internal class BatchRetroactiveConfig : EntityConfig<BatchRetroactive>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("QMS_RCD_BT_REACT").MapAllProperties();
            Meta.Property(BatchRetroactive.WmsKeyProperty).ColumnMeta.HasLength(600);
            Meta.EnablePhantoms();
		}
	}
}