using SIE.Common.Configs;
using SIE.Core.Items;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Items.Items.Configs
{
    /// <summary>
    /// 异常提示信息配置值
    /// </summary>
    [RootEntity, Serializable]
    public class RetrospectTypeConfigValue : ConfigValue
    {
        #region 追溯方式 BackTypes
        /// <summary>
        /// 追溯方式
        /// </summary>
        [Label("产品追溯方式")]
        public static readonly Property<RetrospectType> RetrospectTypeProperty = P<RetrospectTypeConfigValue>.Register(e => e.RetrospectType);

        /// <summary>
        /// 产品追溯方式
        /// </summary>
        public RetrospectType RetrospectType
        {
            get { return this.GetProperty(RetrospectTypeProperty); }
            set { this.SetProperty(RetrospectTypeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示名称
        /// </summary>
        /// <returns>异常提示信息</returns>
        public override string Display()
        {
            return RetrospectType.ToLabel().L10N();
        }
    }
}
