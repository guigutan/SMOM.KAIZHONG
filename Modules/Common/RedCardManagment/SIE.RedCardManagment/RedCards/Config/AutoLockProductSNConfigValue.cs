using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.RedCardManagment.RedCards.Config
{
    /// <summary>
    /// 自动锁定产品SN配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("自动锁定产品SN")]
    public class AutoLockProductSNConfigValue : ConfigValue
    {
        /// <summary>
        /// 自动锁定产品SN
        /// </summary>
        [Label("自动锁定产品SN")]
        public static readonly Property<bool> IsAutoProperty = P<AutoLockProductSNConfigValue>.Register(e => e.IsAuto);

        /// <summary>
        /// 自动锁定产品SN
        /// </summary>
        public bool IsAuto
        {
            get { return this.GetProperty(IsAutoProperty); }
            set { this.SetProperty(IsAutoProperty, value); }
        }

        /// <summary>
        /// 把值显示出来
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            return IsAuto?"True":"False";
        }
    }
}
