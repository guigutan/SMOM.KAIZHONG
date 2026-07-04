using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.ShipPlan;
using System;

namespace SIE.LES.StockPlans
{
    /// <summary>
    /// 备料计划
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(StockPlanCriteria))]
    [Label("备料计划")]
    public class StockPlan : DeliveryPlan
    {
        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> ResourceNameProperty = P<StockPlan>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 备料计划 实体配置
    /// </summary>
    internal class StockPlanConfig : EntityConfig<StockPlan>
    {
        /// <summary>
        /// 增加验证逻辑
        /// </summary>
        /// <param name="rules">验证集合</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(new NotDuplicateRule()
            {
                Properties =
                {
                    StockPlan.NoProperty,
                    StockPlan.LineNoProperty,
                },
                MessageBuilder = o =>
                {
                    return "已经存在相同的计划明细行号".L10N();
                }
            }, new RuleMeta { Scope = EntityStatusScopes.Add | EntityStatusScopes.Update });
        }

        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("DELIVERY_PLAN").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}