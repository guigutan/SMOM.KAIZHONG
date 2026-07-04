using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.IdleArchives.Configs
{
    /// <summary>
    /// 闲置封存-闲置最长期限配置项
    /// </summary>
    [System.ComponentModel.DisplayName("闲置最长期限配置项")]
    [System.ComponentModel.Description("闲置最长期限配置项,具体规则详细请在配置项中进行配置")]
    public class IdleArchivesMaximumTermConfig : ModuleConfig<IdleArchivesMaximumTermConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly IdleArchivesMaximumTermConfigValue defaultValue = new IdleArchivesMaximumTermConfigValue { MaximumTerm = 0 };

        /// <summary>
        /// 默认值
        /// </summary>
        public override IdleArchivesMaximumTermConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }

    /// <summary>
    /// 设备台账启用固定资产配置
    /// </summary>
    [RootEntity, Serializable]
    [Label("闲置封存配置项")]
    public class IdleArchivesMaximumTermConfigValue : ConfigValue
    {
        #region 闲置最长期限 MaximumTerm
        /// <summary>
        /// 闲置最长期限
        /// </summary>
        [Label("闲置最长期限（天）")]
        public static readonly Property<int> MaximumTermProperty = P<IdleArchivesMaximumTermConfigValue>.Register(e => e.MaximumTerm);

        /// <summary>
        /// 闲置最长期限
        /// </summary>
        public int MaximumTerm
        {
            get { return this.GetProperty(MaximumTermProperty); }
            set { this.SetProperty(MaximumTermProperty, value); }
        }
        #endregion


        /// <summary>
        /// 显示
        /// </summary>
        /// <returns></returns>
        public override string Display()
        {
            return "闲置最长期限（天）:{0}".L10nFormat(MaximumTerm);
        }
    }
}
