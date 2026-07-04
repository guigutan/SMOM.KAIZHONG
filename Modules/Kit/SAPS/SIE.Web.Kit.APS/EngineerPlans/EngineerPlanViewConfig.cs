using SIE.Kit.APS.EngineerPlans;
using SIE.MetaModel.View;
using SIE.SO.SaleOrders;
using SIE.Web.Common;
using SIE.Web.Kit.APS.EngineerPlans.Commands;
using SIE.Web.Resources;

namespace SIE.Web.Kit.APS.EngineerPlans
{
    /// <summary>
    ///工程计划视图 
    /// </summary>
    public class EngineerPlanViewConfig : WebViewConfig<EngineerPlan>
    {
        /// <summary>
        /// 视图
        /// </summary>
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.UseCommands(typeof(GenerateEngineerPlanCommand).FullName, typeof(DoScheduleCommand).FullName, WebCommandNames.Edit, WebCommandNames.Save, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
                View.Property(p => p.FactoryId).UseFactoryEditor().Readonly();
                View.Property(p => p.PlanState).Show(ShowInWhere.All);
                View.Property(p => p.IsUrgent).Readonly(p => p.PlanState == SOMI_PlanState.Deleted || p.PlanState == SOMI_PlanState.Finish).Show(ShowInWhere.List);
                View.Property(p => p.Remark).Readonly(p => p.PlanState == SOMI_PlanState.Deleted || p.PlanState == SOMI_PlanState.Finish).Show(ShowInWhere.List);

                View.Property(p => p.SaleOrderNo).Readonly().Show(ShowInWhere.All).ShowInList(width: 160);
                View.Property(p => p.LineNo).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.ItemId).HasLabel("生产型号").Readonly().Show(ShowInWhere.All);
                View.Property(p => p.ItemName).HasLabel("物料名称").Readonly().Show(ShowInWhere.All).ShowInList(width: 160);

                View.Property(p => p.ItemExtPropName).HasLabel("版本").Readonly().Show(ShowInWhere.All);
                View.Property(p => p.CustomerCode).HasLabel("客户编码").Readonly().Show(ShowInWhere.All);
                View.Property(p => p.CustomerName).HasLabel("客户名称").Readonly().Show(ShowInWhere.All);
                View.Property(p => p.Hour).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.CustLevel).HasLabel("客户优先级").Readonly().Show(ShowInWhere.All);
                View.Property(p => p.ScheduleDay).UseDateEditor(p => p.Format = "Y/m/d").Readonly().Show(ShowInWhere.All);
                View.Property(p => p.SortDate).UseDateEditor(p => p.Format = "Y/m/d").Readonly().Show(ShowInWhere.All);
                //View.Property(p => p.DaySortIndex).Readonly().Show(ShowInWhere.All);
                //View.Property(p => p.PlanFinishDay).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.OrderClassify).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.ProductType)
                                .UseListSetting(e => { e.HelpInfo = "产品类型快码类型(PRODUCT_TYPE)"; })
                                .UseCatalogEditor(e => e.CatalogType = SaleOrderDetail.PRODUCTTYPE)
                                .Show(ShowInWhere.List)
                                .Readonly(true);

                View.Property(p => p.AllegroType).Readonly().Show(ShowInWhere.List);
                View.Property(p => p.AppArea).HasLabel("应用领域").Readonly().Show(ShowInWhere.List);
                View.Property(p => p.IsNew).UseCheckDropDownEditor().Readonly().Show(ShowInWhere.List);
                View.Property(p => p.ExternalEcn).UseCheckDropDownEditor().Readonly().Show(ShowInWhere.List);
                View.Property(p => p.Qty).Readonly().Show(ShowInWhere.List);
                View.Property(p => p.Unit).Readonly().Show(ShowInWhere.List);
                View.Property(p => p.Area).Readonly().Show(ShowInWhere.List);
                View.Property(p => p.CustomerPoDate).Readonly().Show(ShowInWhere.List);
                View.Property(p => p.RequireDelivery).Readonly().Show(ShowInWhere.List);
                View.Property(p => p.RegisterDateTime).Readonly().Show(ShowInWhere.List);
            }
        }

        /// <summary>
        /// 明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            using (View.OrderProperties())
            {
                View.UseCommands(WebCommandNames.FormSave);
                View.Property(p => p.SaleOrderNo).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.ItemName).HasLabel("物料名称").Readonly().Show(ShowInWhere.All);

                View.Property(p => p.Remark).Show(ShowInWhere.List);
                View.Property(p => p.PlanState).UseEnumEditor("AllowEdit").Show(ShowInWhere.All);
            }
        }
    }
}


