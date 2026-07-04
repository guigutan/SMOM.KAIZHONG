using SIE.Domain;
using SIE.Fixtures.InboundOrders;
using SIE.MetaModel.View;
using SIE.Warehouses;
using SIE.Web.Core.Common.Commands;

namespace SIE.Web.Fixtures.InboundOrders
{
    /// <summary>
    /// 编码类工治具视图配置
    /// </summary>
    public class InboundOrderFixtureCodeAccountViewConfig : WebViewConfig<InboundOrderFixtureCodeAccount>
    {
        /// <summary>
        /// 编码类视图
        /// </summary>
        public const string CodeLsitView = "CodeLsitView";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(InboundOrderFixtureCodeAccountViewConfig.CodeLsitView);
            if (ViewGroup == CodeLsitView)
            {
                ConfigCodeLsitView();
            }
        }

        /// <summary>
        /// 配置列表
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.Property(p => p.Qty);
            View.Property(p => p.StorageLocation).HasLabel("库位");

            View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 配置编码修改视图
        /// </summary>
        protected void ConfigCodeLsitView()
        {
            View.UseCommands(WebCommandNames.Add, typeof(ImmediateDeleteCommand).FullName);
            View.AddBehavior("SIE.Web.Fixtures.InboundOrders.Scripts.InboundOrdersChildListBehavior");
            View.Property(p => p.Qty).UseSpinEditor(p=> { p.DecimalPrecision = 0;p.MinValue = 1; }).Show();
            View.Property(p => p.StorageLocation).UseDataSource((e, pagingInfo, keyword) =>
            {

                var inboundOrderFixtureCodeAccount = e as InboundOrderFixtureCodeAccount;
                if (inboundOrderFixtureCodeAccount == null)
                {
                    return new EntityList<StorageLocation>();
                }
                return RT.Service.Resolve<InboundOrderController>().GetStorageLocationList(inboundOrderFixtureCodeAccount.InboundOrderId, pagingInfo, keyword);

            }).HasLabel("库位").Show();

            View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
        }
    }
}
