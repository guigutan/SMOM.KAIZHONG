using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ProductIntfc.InspLogs.Configs
{
    /// <summary>
    /// 成品报检是否传QMS配置项
    /// </summary>
    [RootEntity, Serializable]
    [Label("成品报检是否传QMS配置")]
    public class InspLogCallConfigValue : ConfigValue
    {
        #region 是否启用QMS接口 IsCall
        /// <summary>
        /// 是否启用QMS接口
        /// </summary>
        [Label("是否启用QMS接口")]
        public static readonly Property<bool> IsCallProperty = P<InspLogCallConfigValue>.Register(e => e.IsCall);

        /// <summary>
        /// 是否启用QMS接口
        /// </summary>
        public bool IsCall
        {
            get { return this.GetProperty(IsCallProperty); }
            set { this.SetProperty(IsCallProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns></returns>
        public override string Display()
        {
            return IsCall ? "是".L10N() : "否".L10N();
        }
    }
}
