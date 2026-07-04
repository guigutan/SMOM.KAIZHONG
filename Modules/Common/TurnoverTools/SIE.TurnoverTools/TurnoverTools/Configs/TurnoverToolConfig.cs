using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.ComponentModel;

namespace SIE.TurnoverTools.TurnoverTools.Configs
{
    /// <summary>
    /// 启用混工单模式
    /// </summary>
    [DisplayName("周转工具：启用混工单模式")]
    [Description("启用代表同产品不同工单可以绑定同一个周转工具；禁用代表不同工单产品不能绑定同一个周转工具")]
    [ConfigForEntity(typeof(TurnoverTool))]
    public class TurnoverToolConfigs : ModuleConfig<TurnoverToolConfigsValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly TurnoverToolConfigsValue defaultValue = new TurnoverToolConfigsValue();

        /// <summary>
        /// 默认值
        /// </summary>
        public override TurnoverToolConfigsValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }


    /// <summary>
    /// 启用混工单模式
    /// </summary>
    [RootEntity, Serializable]
    [Label("启用混工单模式")]
    public class TurnoverToolConfigsValue : ConfigValue
    {

        #region 启用混工单模式 EquipTypeIds
        /// <summary>
        /// 启用混工单模式
        /// </summary>
        [Label("启用混工单模式")]
        public static readonly Property<YesNo> MixedWorkOrderModeProperty = P<TurnoverToolConfigsValue>.Register(e => e.MixedWorkOrderMode);

        /// <summary>
        /// 启用混工单模式
        /// </summary>
        public YesNo MixedWorkOrderMode
        {
            get { return GetProperty(MixedWorkOrderModeProperty); }
            set { SetProperty(MixedWorkOrderModeProperty, value); }
        }
        #endregion
        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>编码规则名称</returns>
        public override string Display()
        {
            return MixedWorkOrderMode == YesNo.Yes ? "启用".L10N() : "禁用".L10N();
        }
    }
}