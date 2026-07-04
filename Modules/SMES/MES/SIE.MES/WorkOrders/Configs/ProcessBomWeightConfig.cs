using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.WorkOrders.Configs
{
    /// <summary>
    /// 工序BOM取样净重范围配置项
    /// </summary>
    [System.ComponentModel.DisplayName("工序BOM取样净重范围配置项")]
    [System.ComponentModel.Description("工序BOM取样净重范围配置项")]
    public class ProcessBomWeightConfig: ModuleConfig<ProcessBomWeightConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly ProcessBomWeightConfigValue defaultValue = new ProcessBomWeightConfigValue
        {
            Scope = null
        };

        /// <summary>
        /// 默认值 
        /// </summary>
        public override ProcessBomWeightConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 工序BOM取样净重范围配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("工序BOM取样净重范围配置值")]
    public class ProcessBomWeightConfigValue : ConfigValue
    {
        #region 范围(%) Scope
        /// <summary>
        /// 范围(%)
        /// </summary>
        [Label("范围(%)")]
        public static readonly Property<decimal?> ScopeProperty = P<ProcessBomWeightConfigValue>.Register(e => e.Scope);

        /// <summary>
        /// 范围(%)
        /// </summary>
        public decimal? Scope
        {
            get { return this.GetProperty(ScopeProperty); }
            set { this.SetProperty(ScopeProperty, value); }
        }
        #endregion

    }
}
