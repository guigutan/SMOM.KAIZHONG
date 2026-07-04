using SIE.Common.Configs;
using SIE.Domain;
using SIE.MES.WIP.Repairs;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WIP.Configs
{
    /// <summary>
    /// 换料后原关键件处理方式配置项的值配置
    /// </summary>
    [RootEntity, Serializable]
    [Label("换料后原关键件处理方式配置项的值")]
    public class ChangeItemHandleMethodConfigValue : ConfigValue
    {
        #region 换料后原关键件处理方式 HandleMethod
        /// <summary>
        /// 置换后处理(默认为报废,如果选择正常下料，要进行下料，即更新物料标签表加上换料数量）
        /// </summary>
        [Label("换料后原关键件处理方式")]
        public static readonly Property<ChangeItemHandleMethod> HandleMethodProperty
            = P<ChangeItemHandleMethodConfigValue>.Register(e => e.HandleMethod);

        /// <summary>
        /// 换料后原关键件处理方式
        /// </summary>
        public ChangeItemHandleMethod HandleMethod
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