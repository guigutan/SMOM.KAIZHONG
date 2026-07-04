using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ProductIntfc.FirstInsps
{
    /// <summary>
    /// 首件规则
    /// </summary>
    [RootEntity, Serializable]
    [Label("首件报价规则")]
    public partial class FirstInspRule : DataEntity
    {
        #region 报检参数 Parameter
        /// <summary>
        /// 报检参数
        /// </summary>
        [Label("报检参数")]
        public static readonly Property<FirstInspParam> ParameterProperty = P<FirstInspRule>.Register(e => e.Parameter);

        /// <summary>
        /// 报检参数
        /// </summary>
        public FirstInspParam Parameter
        {
            get { return this.GetProperty(ParameterProperty); }
            set { this.SetProperty(ParameterProperty, value); }
        }
        #endregion

        #region 是否选择 IsSelect
        /// <summary>
        /// 是否选择
        /// </summary>
        [Label("是否选择")]
        public static readonly Property<bool> IsSelectProperty = P<FirstInspRule>.Register(e => e.IsSelect);

        /// <summary>
        /// 是否选择
        /// </summary>
        public bool IsSelect
        {
            get { return this.GetProperty(IsSelectProperty); }
            set { this.SetProperty(IsSelectProperty, value); }
        }
        #endregion  
    }

    /// <summary>
    /// 首检规则实体配置
    /// </summary>
    internal class FirstInspRuleEntityConfig : EntityConfig<FirstInspRule>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("FIRST_INSP_RULE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}