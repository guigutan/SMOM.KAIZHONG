using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Packages.Boxs.Configs
{
    ///// <summary>
    ///// 配送周转箱类型
    ///// </summary>
    //[System.ComponentModel.DisplayName("配送周转箱类型")]
    //[System.ComponentModel.Description("配送周转箱类型")]
    //public class DistributionTurnoverBoxTypeConfig : GlobalConfig<DistributionTurnoverBoxTypeConfigValue>
    //{
    //    /// <summary>
    //    /// 周转箱类型配置默认值
    //    /// </summary>
    //    readonly DistributionTurnoverBoxTypeConfigValue defaultValue = new DistributionTurnoverBoxTypeConfigValue { BoxType = string.Empty };

    //    /// <summary>
    //    /// 获取默认值
    //    /// </summary>
    //    public override DistributionTurnoverBoxTypeConfigValue DefaultValue
    //    {
    //        get { return defaultValue; }
    //    }
    //}

    ///// <summary>
    ///// 周转箱类型配置值
    ///// </summary>
    //[RootEntity, Serializable]
    //[Label("配送周转箱类型")]

    //public class DistributionTurnoverBoxTypeConfigValue : ConfigValue
    //{
    //    /// <summary>
    //    /// 周转箱类型
    //    /// </summary>
    //    [Label("配送周转箱类型")]
    //    public static readonly Property<string> BoxTypeProperty = P<DistributionTurnoverBoxTypeConfigValue>.Register(e => e.BoxType);

    //    /// <summary>
    //    /// 周转箱类型
    //    /// </summary>
    //    public string BoxType
    //    {
    //        get { return this.GetProperty(BoxTypeProperty); }
    //        set { this.SetProperty(BoxTypeProperty, value); }
    //    }

    //    /// <summary>
    //    /// 显示配置值
    //    /// </summary>
    //    /// <returns>配置值</returns>
    //    public override string Display()
    //    {
    //        return BoxType;
    //    }
    //}

    ///// <summary>
    ///// 实体配置
    ///// </summary>
    //class DistributionTurnoverBoxTypeConfigValueConfig : EntityConfig<DistributionTurnoverBoxTypeConfigValue>
    //{
    //    /// <summary>
    //    /// 添加验证规则
    //    /// </summary>
    //    /// <param name="rules">验证规则声明器</param>
    //    protected override void AddValidations(IValidationDeclarer rules)
    //    {
    //        rules.AddRule(DistributionTurnoverBoxTypeConfigValue.BoxTypeProperty, new RequiredRule());
    //    }
    //}
}
