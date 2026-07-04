using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Equipments.Configs
{
    /// <summary>
    /// 配置
    /// </summary>
    [System.ComponentModel.DisplayName("是否生成备件交接单配置")]
    [System.ComponentModel.Description("用于配置是否生成备件交接单")]
    public class IsCreateHandoverBillConfig : GlobalConfig<IsCreateHandoverBillConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly IsCreateHandoverBillConfigValue defaultValue = new IsCreateHandoverBillConfigValue { IsCreateHandoverBill = false };

        /// <summary>
        /// 默认值
        /// </summary>
        public override IsCreateHandoverBillConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 配置实体
    /// </summary>
    [RootEntity, Serializable]
    [Label("是否生成备件交接单")]
    public class IsCreateHandoverBillConfigValue : ConfigValue
    {
        #region 出库单是否生成交接单 IsCreateHandoverBill
        /// <summary>
        /// 出库单是否生成交接单
        /// </summary>
        [Label("生成交接单")]
        public static readonly Property<bool> IsCreateHandoverBillProperty = P<IsCreateHandoverBillConfigValue>.Register(e => e.IsCreateHandoverBill);

        /// <summary>
        /// 出库单是否生成交接单
        /// </summary>
        public bool IsCreateHandoverBill
        {
            get { return this.GetProperty(IsCreateHandoverBillProperty); }
            set { this.SetProperty(IsCreateHandoverBillProperty, value); }
        }
        #endregion

        /// <summary>
        /// 把值显示出来
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            string display = IsCreateHandoverBill ? "是".L10N() : "否".L10N();
            return display;
        }
    }
}
