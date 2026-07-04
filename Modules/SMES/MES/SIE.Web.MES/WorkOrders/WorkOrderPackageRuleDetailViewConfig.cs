using SIE.Common.Prints;
using SIE.Domain;
using SIE.MES.WorkOrders;
using System.Linq;

namespace SIE.Web.MES.WorkOrders
{
    /// <summary>
    /// 工单包装规则 视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class WorkOrderPackageRuleDetailViewConfig : WebViewConfig<WorkOrderPackageRuleDetail>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(WorkOrder));
            View.DeclareExtendViewGroup(WorkOrderViewConfig.ReadonlyView);
            if (ViewGroup == WorkOrderViewConfig.ReadonlyView)
            {
                ReadOnlyView();
                return;
            }
            View.InlineEdit();
            View.ClearCommands();
            View.UseCommands("SIE.Web.MES.WorkOrders.SelectPackageRuleCommand");
            View.UseChildrenAsHorizontal(true);
            View.UseLayoutSize(-2.5, -2.5);
            using (View.OrderProperties())
            {
                View.Property(p => p.PackageUnitName).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Description).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.LevelQty).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Qty).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.IsPackage).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.IsOutStockLabel).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.IsInStockLabel).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Weight).ShowInList(50).Readonly();
                View.Property(p => p.Height).ShowInList(50).Readonly();
                View.Property(p => p.Volume).ShowInList(50).Readonly();
                View.Property(p => p.Length).ShowInList(50).Readonly();
                View.Property(p => p.Width).ShowInList(50).Readonly();
                View.Property(p => p.NumberRule).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.IsPrint).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.PrintTemplate).UseDataSource((o, p, e) =>
                {
                    var rule = o as WorkOrderPackageRuleDetail;
                    var templates = new EntityList<PrintTemplate>();
                    if (rule != null && rule.NumberRule != null)
                        rule.NumberRule.TemplateList.ForEach(a => templates.Add(a.Template));
                    return templates;
                }).Show().Readonly();
                View.ChildrenProperty(p => p.WorkOrderProcessPackingUnitList).Show(ChildShowInWhere.All).UseViewGroup(WorkOrderProcessPackingUnitViewConfig.ListView);
            }
        }

        /// <summary>
        /// 只读视图配置
        /// </summary>
        void ReadOnlyView()
        {
            View.FormEdit();
            View.ClearCommands(false);
            View.UseChildrenAsHorizontal(true);
            View.UseLayoutSize(-2.5, -2.5);
            using (View.OrderProperties())
            {
                View.Property(p => p.PackageUnitName).Show(ShowInWhere.All);
                View.Property(p => p.Description).Show(ShowInWhere.All);
                View.Property(p => p.LevelQty).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Qty).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.IsPackage).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.IsOutStockLabel).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.IsInStockLabel).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Weight).ShowInList(50).Readonly();
                View.Property(p => p.Height).ShowInList(50).Readonly();
                View.Property(p => p.Volume).ShowInList(50).Readonly();
                View.Property(p => p.Length).ShowInList(50).Readonly();
                View.Property(p => p.Width).ShowInList(50).Readonly();
                View.Property(p => p.NumberRuleId).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.IsPrint).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.PrintTemplateId).ShowInList().Readonly();
                View.ChildrenProperty(p => p.WorkOrderProcessPackingUnitList).Show(ChildShowInWhere.All).UseViewGroup(WorkOrderViewConfig.ReadonlyView);
            }
        }
    }
}