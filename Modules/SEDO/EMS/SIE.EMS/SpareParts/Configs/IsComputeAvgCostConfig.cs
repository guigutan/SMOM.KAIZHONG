using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.SpareParts.Configs
{
    /// <summary>
    /// 配置
    /// </summary>
    [System.ComponentModel.DisplayName("是否计算备件平均成本配置")]
    [System.ComponentModel.Description("用于配置是否计算备件平均成本")]
    public class IsComputeAvgCostConfig : ModuleConfig<IsComputeAvgCostConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly IsComputeAvgCostConfigValue defaultValue = new IsComputeAvgCostConfigValue { IsComputeAvgCost = false };

        /// <summary>
        /// 默认值
        /// </summary>
        public override IsComputeAvgCostConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 配置实体
    /// </summary>
    [RootEntity, Serializable]
    [Label("是否计算备件平均成本")]
    public class IsComputeAvgCostConfigValue : ConfigValue
    {
        #region 入库单是否计算备件平均成本 IsComputeAvgCost
        /// <summary>
        /// 入库单是否计算备件平均成本
        /// </summary>
        [Label("计算备件平均成本")]
        public static readonly Property<bool> IsComputeAvgCostProperty = P<IsComputeAvgCostConfigValue>.Register(e => e.IsComputeAvgCost);

        /// <summary>
        /// 入库单是否计算备件平均成本
        /// </summary>
        public bool IsComputeAvgCost
        {
            get { return this.GetProperty(IsComputeAvgCostProperty); }
            set { this.SetProperty(IsComputeAvgCostProperty, value); }
        }
        #endregion

        /// <summary>
        /// 把值显示出来
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            string display = IsComputeAvgCost ? "是".L10N() : "否".L10N();
            return display;
        }
    }
}
