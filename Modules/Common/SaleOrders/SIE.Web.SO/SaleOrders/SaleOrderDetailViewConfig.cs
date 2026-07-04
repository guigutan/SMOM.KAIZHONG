using SIE.SO.SaleOrders;
using SIE.Resources.Enterprises;
using SIE.Web.Common;
using SIE.Web.Items._Extentions_;
using SIE.Web.SO.SaleOrders.Commands;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.SO.SaleOrders
{
    internal class SaleOrderDetailViewConfig : WebViewConfig<SaleOrderDetail>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.SO.SaleOrders.Behaviors.DetailPropertyChangedBehavior");
            View.UseChildrenAsHorizontal().UseLayoutSize(-8, -2);
            View.UseCommands(
               typeof(SaleOrderDetailAddCommand).FullName,
               "SIE.Web.SO.SaleOrders.Commands.SaleOrderDetailEditCommand",
               typeof(SaleOrderDetailDeleteCommand).FullName);

            using (View.OrderProperties())
            {
                View.Property(p => p.LineNo).Readonly(p => p.LineState != LineState.NEW);
                //View.Property(p => p.Version).UseListSetting(e => { e.HelpInfo = "请输入大写字母与两位整数的版本组合"; }).Readonly(p => p.LineState != LineState.NEW);
                View.Property(p => p.ItemId).HasLabel("生产型号").UseSalesOrderDetailItemEditor().UsePagingLookUpEditor((m, e) =>
                {
                    m.ReloadDataOnPopping = true;
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(e.ItemName), nameof(e.Item.Name));
                    dic.Add(nameof(e.ItemEnableExtendProperty), nameof(e.Item.EnableExtendProperty));
                    m.DicLinkField = dic;
                }).Readonly(p => p.LineState != LineState.NEW);

                View.Property(p => p.ItemName).HasLabel("物料名称").Readonly();
                View.Property(p => p.ItemExtPropName).UseItemExtPropRecordsFieldEditor(p =>
                {
                    p.IsAllRequired = true;
                    p.DbField = "ItemRevision";
                }).Readonly(p => !p.ItemEnableExtendProperty);

                //View.Property(p => p.EnterpriseCode).HasLabel("库存组织编码").Readonly();
                //View.Property(p => p.EnterpriseId).HasLabel("库存组织").UseDataSource((e, p, r) =>
                //{
                   // var enterprises = RT.Service.Resolve<EnterpriseController>().GetEnterprises(EnterpriseType.Plant, p, r);
                //    enterprises.ForEach(enterprise => { enterprise.TreePId = null; });
                //    return enterprises;
                //}).UsePagingLookUpEditor((m, e) =>
                //{
                //    Dictionary<string, string> dic = new Dictionary<string, string>();
                //    dic.Add(nameof(e.EnterpriseCode), nameof(e.Enterprise.Code));
                //    m.DicLinkField = dic;
                //    m.DisplayField = nameof(Enterprise.Name);
                //}).Readonly(p => p.LineState != LineState.NEW);
                View.Property(p => p.IndustryType).UseListSetting(e => { e.HelpInfo = "行业类型快码类型(INDUSTRY_TYPE)"; })
                        .UseCatalogEditor(e => e.CatalogType = SaleOrderDetail.INDUSTRYTYPE).Show(ShowInWhere.List).Readonly(p => p.LineState != LineState.NEW);
                View.Property(p => p.OrderType).UseListSetting(e => { e.HelpInfo = "订单类型快码类型(ORDER_TYPE)"; })
                        .UseCatalogEditor(e => e.CatalogType = SaleOrderDetail.ORDERTYPE).Show(ShowInWhere.List).Readonly(p => p.LineState != LineState.NEW);
                View.Property(p => p.ProductType).UseListSetting(e => { e.HelpInfo = "产品类型快码类型(PRODUCT_TYPE)"; })
                        .UseCatalogEditor(e => e.CatalogType = SaleOrderDetail.PRODUCTTYPE).Show(ShowInWhere.List).Readonly(p => p.LineState != LineState.NEW);
                //View.Property(p => p.ProductLevel).UseListSetting(e => { e.HelpInfo = "产品等级快码类型(PRODUCT_LEVEL)"; })
                //        .UseCatalogEditor(e => e.CatalogType = SaleOrderDetail.PRODUCTLEVEL).Show(ShowInWhere.List).Readonly(p => p.LineState != LineState.NEW);
                View.Property(p => p.SpecialProcessStr).Readonly();
                View.Property(p => p.IsNew).UseCheckDropDownEditor(p => p.AllowBlank = true).Readonly(p => p.LineState != LineState.NEW);
                View.Property(p => p.LineState).Readonly();
                View.Property(p => p.Qty).UseSpinEditor(p =>
                {
                    p.MinValue = 0;
                    p.AllowDecimals = false;
                }).Readonly(p => p.LineState != LineState.NEW && p.LineState != LineState.CONFIRMED);
                View.Property(p => p.Unit).HasLabel("单位").Readonly(p => p.LineState != LineState.NEW);
                View.Property(p => p.TargetOrderCode).Readonly();
                View.Property(p => p.MiDateTime).UseDateEditor().Readonly(p => p.LineState != LineState.NEW);
                View.Property(p => p.Area).UseSpinEditor(p =>
                {
                    p.MinValue = 0;
                }).HasLabel("行面积").Readonly(p => p.LineState != LineState.NEW);
                //View.Property(p => p.OrderArea).UseSpinEditor(p =>
                //{
                //    p.MinValue = 0;
                //}).HasLabel("行面积").Readonly(p => p.LineState != LineState.NEW);
                View.Property(p => p.SingleArea).Readonly();
                View.Property(p => p.PlateSize).Readonly(p => p.LineState != LineState.NEW);
                View.Property(p => p.MaterialPnl).UseSpinEditor(p =>
                {
                    p.MinValue = 0;
                }).Readonly(p => p.LineState != LineState.NEW);
                //View.Property(p => p.SetPnl).UseSpinEditor(p =>
                //{
                //    p.MinValue = 0;
                //}).Readonly(p => p.LineState != LineState.NEW);
                //View.Property(p => p.PcsPnl).UseSpinEditor(p =>
                //{
                //    p.MinValue = 0;
                //}).Readonly(p => p.LineState != LineState.NEW);
                View.Property(p => p.RequireDelivery).UseDateEditor().Readonly(p => p.LineState != LineState.NEW && p.LineState != LineState.CONFIRMED);
                View.Property(p => p.PromiseDelivery).UseDateEditor().Readonly();
                View.Property(p => p.IsHangUp).UseCheckDropDownEditor(p => p.AllowBlank = true).Readonly(p => p.LineState != LineState.NEW);
                #region  SGC
                View.Property(p => p.AppArea).HasLabel("应用领域");
                View.Property(p => p.ExternalEcn).UseCheckDropDownEditor(p => p.AllowBlank = true); ;
                View.Property(p => p.IsNpi).UseCheckDropDownEditor(p => p.AllowBlank = true); 
                View.Property(p => p.IsSamplePlate);
                View.Property(p => p.IsRdPlate);
                View.Property(p => p.AllegroType);
                View.Property(p => p.UndeliveredQuantity);
                View.Property(p => p.Remark);
                View.Property(p => p.MendMIState);
                View.Property(p => p.FinishMIState);
                View.Property(p => p.FeedState);
                View.Property(p => p.Coating);
                View.Property(p => p.BigPlateQty);
                View.Property(p => p.LaminationQty);
                View.Property(p => p.ItemSetTime).Readonly();
                #endregion
                View.ChildrenProperty(p => p.SpecialProcessList).Show(ChildShowInWhere.Hide);
            }
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.SaleOrderId).HasLabel("销售订单编码").UseSalesOrderEditor();
            View.Property(p => p.LineNo);
            View.Property(p => p.Version);
            View.Property(p => p.ItemId).HasLabel("物料编码").UseSalesOrderDetailItemEditor().UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add(nameof(e.ItemName), nameof(e.Item.Name));
                m.DicLinkField = dic;
            });
            View.Property(p => p.ItemName).HasLabel("物料名称");
            View.Property(p => p.EnterpriseCode).HasLabel("库存组织编码");
            View.Property(p => p.EnterpriseId).HasLabel("库存组织").UseDataSource((e, p, r) =>
            {
                var enterprises = RT.Service.Resolve<EnterpriseController>().GetEnterprises(EnterpriseType.Plant, p, r);
                enterprises.ForEach(enterprise => { enterprise.TreePId = null; });
                return enterprises;
            });
            View.Property(p => p.IndustryType).UseListSetting(e => { e.HelpInfo = "行业类型快码类型(INDUSTRY_TYPE)"; })
                    .UseCatalogEditor(e => e.CatalogType = SaleOrderDetail.INDUSTRYTYPE).Show(ShowInWhere.List);
            View.Property(p => p.OrderType).UseListSetting(e => { e.HelpInfo = "订单类型快码类型(ORDER_TYPE)"; })
                    .UseCatalogEditor(e => e.CatalogType = SaleOrderDetail.ORDERTYPE).Show(ShowInWhere.List);
            View.Property(p => p.ProductType).UseListSetting(e => { e.HelpInfo = "产品类型快码类型(PRODUCT_TYPE)"; })
                    .UseCatalogEditor(e => e.CatalogType = SaleOrderDetail.PRODUCTTYPE).Show(ShowInWhere.List);
            View.Property(p => p.ProductLevel).UseListSetting(e => { e.HelpInfo = "产品等级快码类型(PRODUCT_LEVEL)"; })
                    .UseCatalogEditor(e => e.CatalogType = SaleOrderDetail.PRODUCTLEVEL).Show(ShowInWhere.List);
            View.Property(p => p.SpecialProcessStr);
            View.Property(p => p.IsNew);
            View.Property(p => p.LineState);
        }
    }
}
