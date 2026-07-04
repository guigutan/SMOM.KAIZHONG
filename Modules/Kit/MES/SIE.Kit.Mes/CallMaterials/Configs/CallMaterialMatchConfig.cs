using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.ComponentModel;

namespace SIE.Kit.MES.CallMaterials.Configs
{
    /// <summary>
    /// 报检单号配置
    /// </summary>
    [DisplayName("工单匹配参数配置")]
    [Description("工单匹配参数配置")]
    public class CallMaterialMatchConfig : ModuleConfig<CallMaterialMatchConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly CallMaterialMatchConfigValue defaultValue = new CallMaterialMatchConfigValue();

        /// <summary>
        /// 默认值
        /// </summary>
        public override CallMaterialMatchConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }

    /// <summary>
    /// 叫料单配置值
    /// </summary>
    [RootEntity, Serializable]
    public class CallMaterialMatchConfigValue : ConfigValue
    {
        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<bool> MatchItemCodeProperty = P<CallMaterialMatchConfigValue>.Register(e => e.MatchItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public bool MatchItemCode
        {
            get { return this.GetProperty(MatchItemCodeProperty); }
            set { SetProperty(MatchItemCodeProperty, value); }
        }

        /// <summary>
        /// 扩展属性
        /// </summary>
        ////[Required]
        [Label("扩展属性")]
        public static readonly Property<bool> ExtentAttriProperty = P<CallMaterialMatchConfigValue>.Register(e => e.ExtentAttr);

        /// <summary>
        /// 扩展属性
        /// </summary>
        public bool ExtentAttr
        {
            get { return this.GetProperty(ExtentAttriProperty); }
            set { this.SetProperty(ExtentAttriProperty, value); }
        }
        #endregion

        /// <summary>
        /// 把配置值显示出来
        /// </summary>
        /// <returns>配置值</returns>
        public override string Display()
        {
            return base.Display();
        }
    }
}