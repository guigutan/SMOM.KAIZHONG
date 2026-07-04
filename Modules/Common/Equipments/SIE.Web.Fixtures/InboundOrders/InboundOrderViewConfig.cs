using SIE.Fixtures.InboundOrders;
using SIE.Web.Fixtures.InboundOrders.Commands;

namespace SIE.Web.Fixtures.InboundOrders
{
    /// <summary>
    /// 入库视图
    /// </summary>
    public class InboundOrderViewConfig : WebViewConfig<InboundOrder>
    {
        private const int cloumnWidth = 20;

        /// <summary>
        /// 编码明细视图
        /// </summary>
        private const string CodeDetailsView = "CodeDetailsView";
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(InboundOrderViewConfig.CodeDetailsView);
            if (ViewGroup == CodeDetailsView)
            {
                ConfigCodeDetailsView();
            }
        }

        /// <summary>
        /// 配置列表
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.AddBehavior("SIE.Web.Fixtures.InboundOrders.Scripts.InboundOrdersBehavior");
            View.UseCommand("SIE.Web.Fixtures.InboundOrders.Commands.InboundCommand");
            View.Property(p => p.No).ShowInList(cloumnWidth * 6).HasLabel("入库单号");
            View.Property(p => p.InboundType).ShowInList(cloumnWidth * 5);
            View.Property(p => p.ReceiptOrderNo).ShowInList(cloumnWidth * 6);
            View.Property(p => p.AcceptanceOrderNo).ShowInList(cloumnWidth * 6);
            View.Property(p => p.Qty).ShowInList(cloumnWidth * 4);
            View.Property(p => p.Warehouse).ShowInList(cloumnWidth * 8).HasLabel("仓库");
            View.Property(p => p.InboundStatus).ShowInList(cloumnWidth * 5);
            View.Property(p => p.MaintainTask).ShowInList(cloumnWidth * 6);
            View.Property(p => p.MaintainStatus).ShowInList(cloumnWidth * 6);
            View.Property(p => p.InboundDate).ShowInList(cloumnWidth * 6);
            View.Property(p => p.Proprietorship).ShowInList(cloumnWidth * 4);
            View.Property(p => p.FixtureEncode).ShowInList(cloumnWidth * 6).HasLabel("工治具编码");
            View.Property(p => p.ManageMode).ShowInList(cloumnWidth * 6).HasLabel("管理方式");
            View.Property(p => p.QualityState).ShowInList(cloumnWidth * 5);
            View.Property(p => p.FixtureType).ShowInList(cloumnWidth * 6).HasLabel("工治具类型");
            View.Property(p => p.Supplier).ShowInList(cloumnWidth * 5).HasLabel("供应商编码");
            View.Property(p => p.SupplierName).ShowInList(cloumnWidth * 6);

            View.Property(p => p.Customer).ShowInList(cloumnWidth * 5).HasLabel("客户编码");
            View.Property(p => p.CustomerName).ShowInList(cloumnWidth * 6);
            View.ChildrenProperty(p => p.InboundOrderFixtureIdAccountList).HasLabel("ID类入库明细");
            View.ChildrenProperty(p => p.InboundOrderFixtureCodeAccountList).HasLabel("编码类入库明细");

            View.ChildrenProperty(p => p.InboundOrderPurchaseList).HasLabel("采购订单");

        }


        /// <summary>
        /// 配置明细
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.DisableEditing();
            View.HasDetailColumnsCount(3);
            View.UseCommands(typeof(SaveInboundCommand).FullName,typeof(SumbitInboundCommand).FullName);
            View.Property(p => p.No).ShowInList(cloumnWidth * 6).HasLabel("入库单号");
            View.Property(p => p.InboundType).ShowInList(cloumnWidth * 5);
            View.Property(p => p.ReceiptOrderNo).ShowInList(cloumnWidth * 6);
            View.Property(p => p.AcceptanceOrderNo).ShowInList(cloumnWidth * 6);
            View.Property(p => p.Qty).ShowInList(cloumnWidth * 4);
            View.Property(p => p.Warehouse).ShowInList(cloumnWidth * 8).HasLabel("仓库");
            View.Property(p => p.InboundStatus).ShowInList(cloumnWidth * 5);
            View.Property(p => p.MaintainTask).ShowInList(cloumnWidth * 6);
            View.Property(p => p.MaintainStatus).ShowInList(cloumnWidth * 6);
            View.Property(p => p.InboundDate).ShowInList(cloumnWidth * 6);
            View.Property(p => p.Proprietorship).ShowInList(cloumnWidth * 4);
            View.Property(p => p.FixtureEncode).ShowInList(cloumnWidth * 6).HasLabel("工治具编码");
            View.Property(p => p.ManageMode).ShowInList(cloumnWidth * 6).HasLabel("管理方式");

            View.Property(p => p.FixtureType).ShowInList(cloumnWidth * 6).HasLabel("工治具类型");
            View.Property(p => p.Supplier).ShowInList(cloumnWidth * 5).HasLabel("供应商编码");
            View.Property(p => p.SupplierName).ShowInList(cloumnWidth * 6);
            View.Property(p => p.QualityState).ShowInList(cloumnWidth * 5);
            View.Property(p => p.Customer).ShowInList(cloumnWidth * 5).HasLabel("客户编码");
            View.Property(p => p.CustomerName).ShowInList(cloumnWidth * 6);
            View.Property(p => p.ScanedNum).ShowInList(cloumnWidth * 4).Show(ShowInWhere.All).Readonly();
            View.ChildrenProperty(p => p.InboundOrderFixtureIdAccountList).UseViewGroup(InboundOrderFixtureIdAccountViewConfig.IDLsitView).HasLabel("ID类入库明细");

            View.ChildrenProperty(p => p.InboundOrderFixtureCodeAccountList).HasLabel("编码类入库明细").Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.InboundOrderPurchaseList).HasLabel("采购订单").Show(ChildShowInWhere.Hide);
        }


        /// <summary>
        ///配置编码类明细视图
        /// </summary>
        protected void ConfigCodeDetailsView()
        {
            View.DisableEditing();
            View.HasDetailColumnsCount(3);
            View.UseCommands(typeof(SaveInboundCommand).FullName,typeof(SumbitInboundCommand).FullName);
            View.Property(p => p.No).ShowInList(cloumnWidth * 6).HasLabel("入库单号").Show( ShowInWhere.All);
            View.Property(p => p.InboundType).ShowInList(cloumnWidth * 5).Show(ShowInWhere.All);
            View.Property(p => p.ReceiptOrderNo).ShowInList(cloumnWidth * 6).Show(ShowInWhere.All);
            View.Property(p => p.AcceptanceOrderNo).ShowInList(cloumnWidth * 6).Show(ShowInWhere.All);
            View.Property(p => p.Qty).ShowInList(cloumnWidth * 4).Show(ShowInWhere.All);
            View.Property(p => p.Warehouse).ShowInList(cloumnWidth * 8).HasLabel("仓库").Show(ShowInWhere.All);
            View.Property(p => p.InboundStatus).ShowInList(cloumnWidth * 5).Show(ShowInWhere.All);
            View.Property(p => p.MaintainTask).ShowInList(cloumnWidth * 6).Show(ShowInWhere.All);
            View.Property(p => p.MaintainStatus).ShowInList(cloumnWidth * 6).Show(ShowInWhere.All);
            View.Property(p => p.InboundDate).ShowInList(cloumnWidth * 6).Show(ShowInWhere.All);
            View.Property(p => p.Proprietorship).ShowInList(cloumnWidth * 4).Show(ShowInWhere.All);
            View.Property(p => p.FixtureEncode).ShowInList(cloumnWidth * 6).HasLabel("工治具编码").Show(ShowInWhere.All);
            View.Property(p => p.ManageMode).ShowInList(cloumnWidth * 6).HasLabel("管理方式").Show(ShowInWhere.All);

            View.Property(p => p.FixtureType).ShowInList(cloumnWidth * 6).HasLabel("工治具类型").Show(ShowInWhere.All);
            View.Property(p => p.Supplier).ShowInList(cloumnWidth * 5).HasLabel("供应商编码").Show(ShowInWhere.All);
            View.Property(p => p.SupplierName).ShowInList(cloumnWidth * 6).Show(ShowInWhere.All);

            View.Property(p => p.Customer).ShowInList(cloumnWidth * 5).HasLabel("客户编码").Show(ShowInWhere.All);
            View.Property(p => p.CustomerName).ShowInList(cloumnWidth * 6).Show(ShowInWhere.All);
            View.Property(p => p.QualityState).ShowInList(cloumnWidth * 5).Show(ShowInWhere.All);
            View.Property(p => p.ScanedNum).ShowInList(cloumnWidth * 4).Show(ShowInWhere.All).Readonly();
            View.ChildrenProperty(p => p.InboundOrderFixtureIdAccountList).HasLabel("ID类入库明细").Show( ChildShowInWhere.Hide);

            View.ChildrenProperty(p => p.InboundOrderFixtureCodeAccountList).UseViewGroup(InboundOrderFixtureCodeAccountViewConfig.CodeLsitView).HasLabel("编码类入库明细").Show(ChildShowInWhere.All);
            View.ChildrenProperty(p => p.InboundOrderPurchaseList).HasLabel("采购订单").Show(ChildShowInWhere.Hide);

        }
    }
}
