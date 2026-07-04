using SIE.AbnormalInfo.AbnormalMonitors.Configs;
using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.AbnormalMonitors
{
	/// <summary>
	/// 异常清单
	/// </summary>
	[RootEntity, Serializable]
	[EntityWithConfig(typeof(NoConfig), "异常清单编码配置项", "异常清单编码配置规则")]
	[EntityWithConfig(typeof(AbmMonitorInventoryConfig))]
	[CriteriaQuery]
	[Label("异常清单")]
	public partial class AbnormalMonitorInventory : AbnormalMonitorTaskBase
	{
		

	}

	/// <summary>
	/// 异常监控任务 实体配置
	/// </summary>
	internal class AbnormalMonitorInventoryConfig : EntityConfig<AbnormalMonitorInventory>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("ABM_MONITOR_INVENTORY").MapAllProperties();
			Meta.Property(AbnormalMonitorInventory.ProblemDescriptionProperty).ColumnMeta.HasLength(2000);
			Meta.Property(AbnormalMonitorInventory.ProblemConditionProperty).ColumnMeta.HasLength(2000);
			Meta.EnablePhantoms();
		}
	}
}