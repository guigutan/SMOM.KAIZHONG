using DocumentFormat.OpenXml.Wordprocessing;
using SIE.MetaModel.View;
using SIE.ProductIntfc.InspRecords;
using SIE.ProductIntfc.OutputProducts;
using SIE.Web.ProductIntfc.OutputProducts.Commands;
using System.Linq;

namespace SIE.Web.ProductIntfc.OutputProducts
{
    /// <summary>
    /// 成品入库视图配置
    /// </summary>
    public class OutputProductViewConfig : WebViewConfig<OutputProduct>
    {
        /// <summary>
        /// 成品入库视图组
        /// </summary>
        public static string OutputProductView { get; } = "OutputProductViewConfig";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(OutputProductView);
            if (ViewGroup == OutputProductView)
                ConfigOutputProductView();
        }

        /// <summary>
        /// 配置成品入库视图
        /// </summary>
        private void ConfigOutputProductView()
        {
            View.UseGridSelectionModel();
            View.ClearCommands(true);
            View.UseCommands("SIE.Web.ProductIntfc.OutputProducts.Commands.OutputProductConfigCommand", WebCommandNames.ExportXls);
            View.UseCommands(typeof(OutputProductReceiveCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Readonly().HasLabel("工单号").ShowInList(150);
                View.Property(p => p.Product).Readonly().HasLabel("产品编码").ShowInList(150);
                View.Property(p => p.WorkOrderProductName).Readonly().HasLabel("产品名称").ShowInList(150);
                View.Property(p => p.ProductType).Readonly().UseEnumEditor().HasLabel("基本分类").ShowInList();
                View.Property(p => p.Type).Readonly().UseEnumEditor().HasLabel("工单类型").ShowInList();
                View.Property(p => p.PlanQty).Readonly().HasLabel("计划数量").ShowInList();
                View.Property(p => p.Factory).Readonly().ShowInList();
                View.Property(p => p.WorkShop).Readonly().HasLabel("车间").ShowInList();
                View.Property(p => p.Resource).Readonly().HasLabel("资源").ShowInList();
                View.Property(p => p.State).Readonly().ShowInList().HasOrderNo(9);
                View.Property(p => p.PlanBeginDate).Readonly().ShowInList().HasOrderNo(10);
            }

            View.EntityViewMeta.ChildrenProperties.ForEach(p =>
            {
                p.ChildShowInWhere = ChildShowInWhere.Hide;
            });

            //View.ChildrenProperty(p => p.OutputProductRecords).Show(ChildShowInWhere.All);
            View.AttachChildrenProperty(typeof(OutputProductRecord), (c) =>
            {
                var child = c as ChildPagingDataArgs;
                var parent = child.Parent as OutputProduct;
                return RT.Service.Resolve<OutputProductController>().GetOutputProductRecords(parent.Id, child.PagingInfo, child.SortInfo);
            }).HasLabel("副产品记录");
        }
    }
}
