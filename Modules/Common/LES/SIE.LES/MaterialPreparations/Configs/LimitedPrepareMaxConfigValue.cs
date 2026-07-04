using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.LES.MaterialPreparations.Configs
{
    /// <summary>
    /// 推式备料是否限制最高存量
    /// </summary>
    [RootEntity, Serializable]
    [Label("推式备料是否限制最高存量")]
    public class LimitedPrepareMaxConfigValue : ConfigValue
    {
        #region 是否限制 IsLimited
        /// <summary>
        /// 是否限制
        /// </summary>
        [Label("是否限制")]
        public static readonly Property<bool> IsLimitedProperty = P<LimitedPrepareMaxConfigValue>.Register(e => e.IsLimited);

        /// <summary>
        /// 是否限制
        /// </summary>
        public bool IsLimited
        {
            get { return this.GetProperty(IsLimitedProperty); }
            set { this.SetProperty(IsLimitedProperty, value); }
        }
        #endregion


        /// <summary>
        /// 显示值
        /// </summary>
        /// <returns></returns>
        public override string Display()
        {
            return IsLimited ? "限制".L10N() : "不限制".L10N();
        }
    }
}
