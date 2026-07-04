using SIE.Common.Configs;
using SIE.Domain;
using SIE.MES.WIP.Reworks;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.WIP.Configs
{
    /// <summary>
    /// 置换后原关键件处理方式配置项的值配置
    /// </summary>
    [RootEntity, Serializable]
    [Label("置换后原关键件处理方式配置项的值")]
    public class KeyComponentsReplacementConfigValue : ConfigValue
    {
        #region 置换后原关键件处理方式 HandleMethod
        /// <summary>
        /// 置换后处理关键件处理方式
        /// </summary>
        [Label("换料后原关键件处理方式")]
        public static readonly Property<ReplaceItemHandleMethod> HandleMethodProperty
            = P<KeyComponentsReplacementConfigValue>.Register(e => e.HandleMethod);

        /// <summary>
        /// 置换后原关键件处理方式
        /// </summary>
        public ReplaceItemHandleMethod HandleMethod
        {
            get { return this.GetProperty(HandleMethodProperty); }
            set { this.SetProperty(HandleMethodProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示
        /// </summary>        
        public override string Display()
        {
            return HandleMethod.ToLabel().L10N();
        }
    }
}
