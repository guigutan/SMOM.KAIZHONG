using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.ComponentModel;

namespace SIE.MES.Configs
{
    /// <summary>
    /// 工序交接校验配置
    /// </summary>
    [DisplayName("工序交接校验配置")]
    [Description("用于配置是否进行工序交接校验（默认不校验）")]
    public class ProcessTransferCheckConfig : GlobalCategoryConfig<ResourceWarehouse, ProcessTransferCheckConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly ProcessTransferCheckConfigValue defaultValue = new ProcessTransferCheckConfigValue();

        /// <summary>
        /// 默认值
        /// </summary>
        public override ProcessTransferCheckConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }

    /// <summary>
    /// 工序交接校验配置值
    /// </summary>
    [RootEntity, Serializable]
    public class ProcessTransferCheckConfigValue : ConfigValue
    {
        #region 是否校验工序交接 IsCheck
        /// <summary>
        /// 是否校验工序交接
        /// </summary>
        [Label("校验工序交接")]
        public static readonly Property<bool> IsCheckProperty = P<ProcessTransferCheckConfigValue>.Register(e => e.IsCheck);

        /// <summary>
        /// 是否校验工序交接
        /// </summary>
        public bool IsCheck
        {
            get { return this.GetProperty(IsCheckProperty); }
            set { this.SetProperty(IsCheckProperty, value); }
        }
        #endregion

        /// <summary>
        /// 把配置值显示出来
        /// </summary>
        /// <returns>配置值</returns>
        public override string Display()
        {
            return IsCheck.ToString();
        }
    }
}