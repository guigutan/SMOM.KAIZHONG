using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Packages.ItemLabels.Configs
{
    /// <summary>
    /// 物料标签配置值
    /// </summary>
    [System.ComponentModel.DisplayName("物料标签配置值")]
    [System.ComponentModel.Description("物料标签配置值")]
    public class ItemLabelConfig: ModuleConfig<ItemLabelConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly ItemLabelConfigValue defaultValue = new ItemLabelConfigValue { IsValidFactory = false };

        /// <summary>
        /// 默认值 
        /// </summary>
        public override ItemLabelConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 物料标签号配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("物料标签配置值")]
    public class ItemLabelConfigValue : ConfigValue
    {
        #region 上料是否校验标签工厂与工序BOM发料工厂 IsValidFactory
        /// <summary>
        /// 上料是否校验标签工厂与工序BOM发料工厂
        /// </summary>
        [Label("上料是否校验标签工厂与工序BOM发料工厂")]
        public static readonly Property<bool?> IsValidFactoryProperty = P<ItemLabelConfigValue>.Register(e => e.IsValidFactory);

        /// <summary>
        /// 上料是否校验标签工厂与工序BOM发料工厂
        /// </summary>
        public bool? IsValidFactory
        {
            get { return this.GetProperty(IsValidFactoryProperty); }
            set { this.SetProperty(IsValidFactoryProperty, value); }
        }
        #endregion

    }
}
