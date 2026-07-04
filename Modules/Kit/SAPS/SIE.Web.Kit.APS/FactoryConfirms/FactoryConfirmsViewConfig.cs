using SIE.Kit.APS.FactoryConfirms;
using SIE.SO.SaleOrders;
using SIE.Resources.Enterprises;
using SIE.Web.Common;
using SIE.Web.Items._Extentions_;
using SIE.Web.Kit.APS.FactoryConfirms.Commands;
using System.Collections.Generic;
using System.Linq;
using SIE.Web.Kit.APS;

namespace SIE.Web.Pcb.APS.FactoryConfirms
{
    /// <summary>
    /// 厂别确认
    /// </summary>
    public class FactoryConfirmsViewConfig : WebViewConfig<FactoryConfirmsViewModel>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit();
        }

        /// <summary>
        ///  配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(BranchFactoryProgrammeCommand).FullName, typeof(FactoryConfirmsSaveCommand).FullName, typeof(GenerateOrderReviewCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.SalesOrderCode).HasLabel("销售订单编号").FixColumn().Readonly();
                View.Property(p => p.LineNo).HasLabel("行号").FixColumn().Readonly();
                View.Property(p => p.ItemId).HasLabel("物料编码").FixColumn().Readonly();
                View.Property(p => p.ItemName).HasLabel("物料名称").Readonly();
                View.Property(p => p.ItemExtPropName).UseItemExtPropRecordsFieldEditor(p =>
                {
                    p.IsAllRequired = true;
                    p.DbField = "ItemRevision";
                }).Readonly(p => !p.ItemEnableExtendProperty);
                View.Property(p => p.CustomerId).HasLabel("客户编号").UseSalesOrderCustomerEditor()
                    .UsePagingLookUpEditor((m, e) =>
                    {
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add(nameof(e.CustomerName), nameof(e.Customer.ShortName));
                        m.DicLinkField = dic;
                    }).Readonly();
                View.Property(p => p.CustomerName).HasLabel("客户名称").Readonly().Readonly();
                View.Property(p => p.IndustryType).UseCatalogEditor(e => e.CatalogType = SaleOrderDetail.INDUSTRYTYPE).Show(ShowInWhere.List).Readonly();
                View.Property(p => p.OrderType).UseCatalogEditor(e => e.CatalogType = SaleOrderDetail.ORDERTYPE).Show(ShowInWhere.List).Readonly();
                View.Property(p => p.ProductType).UseCatalogEditor(e => e.CatalogType = SaleOrderDetail.PRODUCTTYPE).Show(ShowInWhere.List).Readonly();
                View.Property(p => p.SpecialProcessStr).HasLabel("特殊工艺").Readonly();
                View.Property(p => p.IsNew).UseCheckDropDownEditor().HasLabel("是否新单").Readonly();
                View.Property(p => p.LineState).HasLabel("行状态").Readonly();
                View.Property(p => p.Qty).HasLabel("数量").Readonly();
                View.Property(p => p.UnitName).HasLabel("单位").Readonly();
                View.Property(p => p.EnterpriseId).HasLabel("工厂编码").UseDataSource((e, p, r) =>
                {
                    var enterprises = RT.Service.Resolve<EnterpriseController>().GetEnterprises(EnterpriseType.Plant, p, r);
                    enterprises.ForEach(enterprise => { enterprise.TreePId = null; });
                    return enterprises;
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(e.EnterpriseName), nameof(e.Enterprise.Name));
                    m.DicLinkField = dic;
                    m.DisplayField = nameof(Enterprise.Code);
                }).Readonly(p => p.LineState != LineState.NEW);
                View.Property(p => p.EnterpriseName).HasLabel("工厂名称").Readonly();
                View.Property(p => p.MiDateTime).HasLabel("MI完成时间").Readonly();
                View.Property(p => p.Area).HasLabel("总面积M2").Readonly();
                View.Property(p => p.PlateSize).HasLabel("大板尺寸").Readonly();
                View.Property(p => p.MaterialPnl).HasLabel("开料PNL数").Readonly();
                View.Property(p => p.SetPnl).HasLabel("SET/PNL数").Readonly();
                View.Property(p => p.PcsPnl).HasLabel("PCS/PNL数").Readonly();
                View.Property(p => p.RequireDelivery).HasLabel("客户交期").Readonly();
                View.Property(p => p.PromiseDelivery).HasLabel("承诺交期").Readonly();
                View.Property(p => p.ProductLevel).UseListSetting().HasLabel("产品等级").UseCatalogEditor(e => e.CatalogType = SaleOrderDetail.PRODUCTLEVEL).Show(ShowInWhere.List).Readonly();
                View.Property(p => p.IsHangUp).UseCheckDropDownEditor().Readonly();
            }
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.SalesOrderCode);
            View.Property(p => p.ItemCode);
            View.Property(p => p.ItemName);
            View.Property(p => p.IndustryType).UseCatalogEditor(e => e.CatalogType = SaleOrderDetail.INDUSTRYTYPE).Show(ShowInWhere.List);
            View.Property(p => p.OrderType).UseCatalogEditor(e => e.CatalogType = SaleOrderDetail.ORDERTYPE).Show(ShowInWhere.List);
            View.Property(p => p.ProductType).UseCatalogEditor(e => e.CatalogType = SaleOrderDetail.PRODUCTTYPE).Show(ShowInWhere.List);
            View.Property(p => p.IsNew).UseCheckDropDownEditor().HasLabel("是否新单");
            View.Property(p => p.LineState).HasLabel("行状态").DefaultValue((int)LineState.NEW);
            View.Property(p => p.EnterpriseId).HasLabel("库存组织编码").UseDataSource((e, p, k) =>
            {
                return RT.Service.Resolve<EnterpriseController>().GetEnterprises(EnterpriseType.Plant, p, k);
            });
            View.Property(p => p.EnterpriseName).HasLabel("库存组织名称");
            View.Property(p => p.RequireDelivery).UseDateRangeEditor(p =>
            {
                p.DateRangeType = ObjectModel.DateRangeType.Month;
            });
        }
    }
}
