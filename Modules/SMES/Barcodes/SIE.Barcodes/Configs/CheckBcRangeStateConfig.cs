using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Barcodes.Configs
{
    [System.ComponentModel.DisplayName("产品上线是否校验条码领用")]
    [System.ComponentModel.Description("产品上线是否校验条码领用")]
    public  class CheckBcRangeStateConfig :  GlobalConfig<CheckBcRangeStateConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        private readonly CheckBcRangeStateConfigValue defaultValue = new CheckBcRangeStateConfigValue { IsCheck=false};

        /// <summary>
        /// 默认值
        /// </summary>
        public override CheckBcRangeStateConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
    [RootEntity, Serializable]
    [Label("是否启用上线时校验条码领用")]
    public class CheckBcRangeStateConfigValue : ConfigValue
    {

        #region 是否校验 IsCheck
        /// <summary>
        /// 是否校验
        /// </summary>
        [Label("是否校验")]
        public static readonly Property<bool> IsCheckProperty = P<CheckBcRangeStateConfigValue>.Register(e => e.IsCheck);

        /// <summary>
        /// 是否校验
        /// </summary>
        public bool IsCheck
        {
            get { return this.GetProperty(IsCheckProperty); }
            set { this.SetProperty(IsCheckProperty, value); }
        }
        #endregion


        public override string Display()
        {
            return IsCheck ? "是".L10N() : "否".L10N();
        }
    }

}
