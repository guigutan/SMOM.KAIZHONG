using SIE.Domain.Query;
using SIE.Modules;
using SIE.Packages;
using SIE.Packages.QrCodeParseRules;
using SIE.Common.InvOrg;
using SIE.Packages.Packages;

[assembly: Module(typeof(Module))]

namespace SIE.Packages
{
    /// <summary>
    /// Module
    /// </summary>
    class Module : DomainModule
    {
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="app">Application</param>
        public override void Initialize(IApp app)
        {
            RT.Service.Register<IInvOrgInit, UnitInitController>();
            PackageEntityDataProvider.Querying += PackageEntityDataProvider_Querying;
        }

        /// <summary>
        /// 查询实体事件
        /// </summary>
        /// <param name="sender">数据提供者</param>
        /// <param name="e">查询事件参数</param>
        private void PackageEntityDataProvider_Querying(object sender, Domain.QueryingEventArgs e)
        {
            var provider = sender as PackageEntityDataProvider;
            if (provider != null && provider.Repository.EntityType == typeof(QrCodeParseRuleDetail))
            {
                var order = QueryFactory.Instance.OrderBy(e.Args.Query.MainTable.FindColumn(QrCodeParseRuleDetail.IdProperty));

                //先清除排序，如果不清楚，在下拉框查询时，会生成重复ID排序，SQLSERVER会报错
                e.Args.Query.OrderBy.Clear();
                e.Args.Query.OrderBy.Add(order);
            }
        }
    }
}
