using SIE.Domain;
using SIE.MES.BatchWIP.Products;
using SIE.MES.RoutingSettings;
using SIE.MES.WIP.Products;
using SIE.Tech.Processs;
using SIE.Web.Items._Extentions_;

namespace SIE.Web.MES.ProductRoutings
{
    /// <summary>
    /// 产品生产BOM视图配置
    /// </summary>
    internal class BomViewModelViewConfig : WebViewConfig<ProductBomViewModel>
    {
        /// <summary>
        /// 配置列表
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(WipProductRouting), typeof(BatchWipProductRouting));
            View.InlineEdit();
            View.UseCommands("SIE.Web.MES.ProductRoutings.AddBomCommand", "SIE.Web.MES.ProductRoutings.EditBomCommand", "SIE.Web.MES.ProductRoutings.DeleteBomCommand");

            View.Property(p => p.Item)
                .UsePagingLookUpEditor((c, e) =>
            {
                var keyValues = new System.Collections.Generic.Dictionary<string, string>();
                keyValues.Add(nameof(e.ItemName), nameof(e.Item.Name));
                keyValues.Add(nameof(e.IsAllowEdit), nameof(e.Item.EnableExtendProperty));
                c.DicLinkField = keyValues;
            }).ShowInList(width: 150);
            View.Property(p => p.ItemName).HasLabel("物料名称").ShowInList(width: 250);
            View.Property(p => p.Qty).UseItemUnitEditor().ShowInList(width: 100);
            View.Property(p => p.ItemExtPropName).UseItemExtPropRecordsFieldEditor(p =>
            {
                p.IsAllRequired = true;
                p.DbField = "ItemExtProp";
            }).Readonly(p => !p.IsAllowEdit).HasLabel("物料扩展属性").HasOrderNo(11);

            View.Property(p => p.ItemExtPropName).ShowInList(width: 100);

            View.Property(p => p.IsBuckleMaterial).Show(ShowInWhere.All).Readonly();

            View.Property(p => p.WorkStep).UsePagingLookUpEditor((c, e) =>
            {
                var keyValues = new System.Collections.Generic.Dictionary<string, string>();
                keyValues.Add(nameof(e.WorkStepName), nameof(e.WorkStep.Name));
                c.DicLinkField = keyValues;
            }).UseDataSource((t, p, s) =>
            {
                ProductBomViewModel bom = t as ProductBomViewModel;
                if (bom == null || bom.ProcessId == null)
                {
                    return new EntityList<WorkStep>();
                }
                return RT.Service.Resolve<ProcessController>().GetWorkSteps((double)bom.ProcessId);
            })
            .ShowInList(width: 50);

            View.Property(p => p.Alter).ShowInList(width: 150);
            View.Property(p => p.AlternativeGroup).ShowInList(width: 150);
        }
    }
}