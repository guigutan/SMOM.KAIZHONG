using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Domain;
using SIE.Domain.Query;
using SIE.Domain.Validation;
using SIE.Fixtures.Models;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Fixtures.Fixtures.Accounts
{
    /// <summary>
	/// 工治具台账（ID管理）
	/// </summary>
	[RootEntity, Serializable]
    [ConditionQueryType(typeof(FixtureIDAccountCriteria))]
    [EntityWithConfig(typeof(NoConfig), "工治具ID配置项", "工治具ID生成规则")]
    [Label("工治具台账（ID管理）")]
    public partial class FixtureIDAccount : FixtureAccount
    {
    }

    /// <summary>
    /// 工治具台账（ID管理） 实体配置
    /// </summary>
    internal class FixtureIDAccountConfig : EntityConfig<FixtureIDAccount>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Func<IQuery> view = () => DB.Query<FixtureAccount>()
                .Exists<FixtureEncode>((x, y) =>
                    y.Join<FixtureModel>((c, d) => c.FixtureModelId == d.Id && d.ManageMode == ManageMode.Number)
                    .Where(p => p.Id == x.FixtureEncodeId)).ToQuery();
            Meta.MapView(view).MapAllProperties();
            Meta.Property(FixtureIDAccount.ScrapTypeProperty).DontMapColumn();
            Meta.Property(FixtureIDAccount.ReasonProperty).DontMapColumn();
        }

        /// <summary>
        /// 校验规则
        /// </summary>
        /// <param name="rules">规则</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(new HandlerRule()
            {
                Handler = (o, e) =>
                {
                    var para = o.CastTo<FixtureIDAccount>();

                    if (para.Rfid.IsNotEmpty()) 
                    {
                        var account = RT.Service.Resolve<CoreFixtureController>().GetFixtureAccountByRfid(para.Id, para.Rfid);

                        if (account != null)
                        {
                            e.BrokenDescription = "已存在RFID为【{0}】的工治具台账，请确认".L10nFormat(para.Rfid);
                        }
                    }
                }
            }, new RuleMeta() { Scope = EntityStatusScopes.Add | EntityStatusScopes.Update });
        }
    }
}
