using SIE.MES.Workbench.AlertLights;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Workbench.AndonAbnormals
{
    /// <summary>
	/// 按灯管控
	/// </summary>
	[RootEntity, Serializable]
    ////[CriteriaQuery]
    [ConditionQueryType(typeof(AndonAbnormalCriteria))]
    [Label("安灯异常")]
    public partial class AndonAbnormal : AlertLight
    {
    }

    /// <summary>
    /// 按灯管控实体配置
    /// </summary>
    internal class AndonControlConfig : EntityConfig<AndonAbnormal>
    {
        /// <summary>
        /// 安灯异常实体配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WB_ALERT_LIGHT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
