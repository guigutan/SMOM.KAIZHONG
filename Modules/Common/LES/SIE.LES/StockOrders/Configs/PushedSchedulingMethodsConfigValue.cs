using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Text;

namespace SIE.LES.StockOrders.Configs
{
    /// <summary>
    /// 推式调度多触发方式同时满足的处理方式
    /// </summary>
    [RootEntity, Serializable]
    [Label("推式调度多触发方式同时满足的处理方式")]
    public class PushedSchedulingMethodsConfigValue : ConfigValue
    {

        #region 处理方式 PushedSchedulingMethod
        /// <summary>
        /// 推式调度多触发方式处理方式
        /// </summary>
        [Label("处理方式")]
        public static readonly Property<PushedSchedulingMethods> PushedSchedulingMethodProperty = P<PushedSchedulingMethodsConfigValue>.Register(e => e.PushedSchedulingMethod);

        /// <summary>
        /// 处理方式
        /// </summary>
        public PushedSchedulingMethods PushedSchedulingMethod
        {
            get { return this.GetProperty(PushedSchedulingMethodProperty); }
            set { this.SetProperty(PushedSchedulingMethodProperty, value); }
        }
        #endregion


        /// <summary>
        /// 显示值
        /// </summary>
        /// <returns></returns>
        public override string Display()
        {
            return PushedSchedulingMethod.ToLabel().L10N();
        }
    }

    /// <summary>
    ///
    /// </summary>
    public enum PushedSchedulingMethods
    {
        /// <summary>
        /// 同时生成
        /// </summary>
        [Label("同时生成")]
        SimultaneousGeneration,

        /// <summary>
        /// 生成需求最大的单据
        /// </summary>
        [Label("生成需求最大的单据")]
        GenerateHighestDemand


    }
}
