using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Configs
{
    /// <summary>
    /// 接口失败次数维护
    /// </summary>
    [System.ComponentModel.DisplayName("接口失败次数维护")]
    [System.ComponentModel.Description("接口失败次数维护")]
    public class InfLogFacFailConfig : ModuleConfig<InfLogFacFailConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly InfLogFacFailConfigValue defaultValue = new InfLogFacFailConfigValue { FailCount = 5 };

        /// <summary>
        /// 默认值 
        /// </summary>
        public override InfLogFacFailConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }
    /// <summary>
    /// 接口失败次数维护
    /// </summary>
    [RootEntity, Serializable]
    [Label("接口失败次数维护")]
    public class InfLogFacFailConfigValue : ConfigValue
    {
        #region 失败次数上限 FailCount
        /// <summary>
        /// 失败次数上限
        /// </summary>
        [Label("失败次数上限")]
        public static readonly Property<int> FailCountProperty = P<InfLogFacFailConfigValue>.Register(e => e.FailCount);

        /// <summary>
        /// 失败次数上限
        /// </summary>
        public int FailCount
        {
            get { return this.GetProperty(FailCountProperty); }
            set { this.SetProperty(FailCountProperty, value); }
        }
        #endregion

        public override string Display()
        {
            return base.Display();
        }
    }
}
