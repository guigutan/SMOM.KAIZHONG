using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WorkOrders.Configs
{
    /// <summary>
    /// 打印设置配置
    /// </summary>
    [System.ComponentModel.DisplayName("是否必须配置打印设置")]
    [System.ComponentModel.Description("新建/复制/修改工单是否必须配置打印设置")]
    public class PrintTemplateConfig : ModuleConfig<PrintTemplateConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public override PrintTemplateConfigValue DefaultValue { get; } = new PrintTemplateConfigValue() { IsNeed = false };
    }

    /// <summary>
    /// 打印设置配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("打印设置")]
    public class PrintTemplateConfigValue : ConfigValue
    {
        #region 是否配置打印设置 IsNeed
        /// <summary>
        /// 打印设置
        /// </summary>
        [Label("是否配置")]
        public static readonly Property<bool> IsNeedProperty = P<PrintTemplateConfigValue>.Register(e => e.IsNeed);

        /// <summary>
        /// 打印设置
        /// </summary>
        public bool IsNeed
        {
            get { return this.GetProperty(IsNeedProperty); }
            set { this.SetProperty(IsNeedProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>打印设置</returns>
        public override string Display()
        {
            return IsNeed.ToString();
        }
    }
}
