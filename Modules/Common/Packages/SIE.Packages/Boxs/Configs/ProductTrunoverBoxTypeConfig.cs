using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Packages.Boxs.Configs
{
    /// <summary>
    /// 生产周转箱类型
    /// </summary>
    [System.ComponentModel.DisplayName("生产周转箱类型")]
    [System.ComponentModel.Description("生产周转箱类型")]
    public class ProductTrunoverBoxTypeConfig : GlobalConfig<ProductTrunoverBoxTypeConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        private readonly ProductTrunoverBoxTypeConfigValue defaultValue = new ProductTrunoverBoxTypeConfigValue { };

        /// <summary>
        /// 默认值
        /// </summary>
        public override ProductTrunoverBoxTypeConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// RFID类型配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("生产周转箱类型")]
    public class ProductTrunoverBoxTypeConfigValue : ConfigValue
    {
        /// <summary>
        /// 周转类型
        /// </summary>
        [Label("生产周转箱类型")]
        public static readonly Property<string> BoxTypeProperty = P<ProductTrunoverBoxTypeConfigValue>.Register(e => e.BoxType);

        /// <summary>
        /// 周转类型
        /// </summary>
        public string BoxType
        {
            get { return this.GetProperty(BoxTypeProperty); }
            set { this.SetProperty(BoxTypeProperty, value); }
        }

        /// <summary>
        /// 显示周转箱类型
        /// </summary>
        /// <returns>返回周转箱类型</returns>
        public override string Display()
        {
            return BoxType;
        }
    }

    /// <summary>
    /// RFID类型配置值 实体配置
    /// </summary>
    class ProductTrunoverBoxTypeConfigValueConfig : EntityConfig<ProductTrunoverBoxTypeConfigValue>
    {
        /// <summary>
        /// 增加实体的验证规则
        /// </summary>
        /// <param name="rules">验证规则集合</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(ProductTrunoverBoxTypeConfigValue.BoxTypeProperty, new RequiredRule());
        }
    }
}
