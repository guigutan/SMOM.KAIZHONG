using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.RedCardManagment.RedCards.Config
{
    /// <summary>
    /// 红牌任务，处理方式
    /// </summary>
    [RootEntity, Serializable]
    [Label("红牌任务扩展配置")]
    public class RecardTaskExtConfigValue : ConfigValue
    {
        /// <summary>
        /// 禁用红牌管理
        /// </summary>
        [Label("解锁物料和产品")]
        public static readonly Property<bool> IsDisabledProperty = P<RecardTaskExtConfigValue>.Register(e => e.IsDisabled);

        /// <summary>
        /// 禁用红牌管理
        /// </summary>
        public bool IsDisabled
        {
            get { return this.GetProperty(IsDisabledProperty); }
            set { this.SetProperty(IsDisabledProperty, value); }
        }

        /// <summary>
        /// 禁用红牌管理
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            return IsDisabled ? "禁用".L10N() : "启用".L10N();
        }
    }
}
