using SIE.Domain;
using SIE.Domain.Query;
using SIE.Fixtures.Models;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Fixtures.Fixtures.Accounts
{
    /// <summary>
	/// 工治具台账（编码管理）
	/// </summary>
	[RootEntity, Serializable]
    [ConditionQueryType(typeof(FixtureCodeAccountCriteria))]
    [Label("工治具台账（编码管理）")]
    public partial class FixtureCodeAccount : FixtureAccount
    {
    }

    /// <summary>
    /// 工治具台账（编码管理） 实体配置
    /// </summary>
    internal class FixtureCodeAccountConfig : EntityConfig<FixtureCodeAccount>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Func<IQuery> view = () => DB.Query<FixtureAccount>()
                .Exists<FixtureEncode>((x, y) =>
                    y.Join<FixtureModel>((c, d) => c.FixtureModelId == d.Id && d.ManageMode == ManageMode.Code)
                    .Where(p => p.Id == x.FixtureEncodeId)).ToQuery();
            Meta.Property(FixtureCodeAccount.ScrapTypeProperty).DontMapColumn();
            Meta.Property(FixtureCodeAccount.ReasonProperty).DontMapColumn();
            Meta.MapView(view).MapAllProperties();
        }
    }
}
