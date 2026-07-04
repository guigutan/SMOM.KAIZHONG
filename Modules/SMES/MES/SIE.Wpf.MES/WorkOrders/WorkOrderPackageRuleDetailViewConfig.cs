using SIE.Common.Prints;
using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.Packages.Packages;
using SIE.Wpf.MES.WorkOrders.Commands;
using System.Linq;

namespace SIE.Wpf.MES.WorkOrders
{
    /// <summary>
    /// 工单包装规则 视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class WorkOrderPackageRuleDetailViewConfig : WPFViewConfig<WorkOrderPackageRuleDetail>
    {
        #region 只读控制,主单位不能编辑 IsReadOnly
        /// <summary>
        /// 只读控制
        /// </summary> 
        public static readonly Property<bool> IsReadOnlyProperty = P<WorkOrderPackageRuleDetail>.RegisterExtensionReadOnly("IsReadOnly", typeof(WorkOrderPackageRuleDetailViewConfig),
            GetIsReadOnly, WorkOrderPackageRuleDetail.PackageUnitIdProperty);

        /// <summary>
        /// 只读控制
        /// </summary>
        public static bool GetIsReadOnly(WorkOrderPackageRuleDetail me)
        {
            if (me.PackageUnit == null)
                return false;
            return me.PackageUnit.IsMasterUnit;
        }
        #endregion

        #region 产品数只读控制 IsQtyReadOnly
        /// <summary>
        /// 产品数只读控制
        /// </summary>
        public static readonly Property<bool> IsQtyReadOnlyProperty = P<WorkOrderPackageRuleDetail>.RegisterExtensionReadOnly("IsQtyReadOnly", typeof(WorkOrderPackageRuleDetailViewConfig),
            GetIsQtyReadOnly, WorkOrderPackageRuleDetail.PackageUnitIdProperty);

        /// <summary>
        /// 产品数只读控制
        /// </summary>
        public static bool GetIsQtyReadOnly(WorkOrderPackageRuleDetail me)
        {
            if (me.PackageUnit == null)
                return true;
            if (me.PackageUnit.IsMasterUnit/* && ItemExtBatchRule.GetBatchRule(me.WorkOrder.Product)?.RetrospectType == RetrospectType.Batch)*/)
                return false;
            return true;
        }
        #endregion

        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(WorkOrderViewConfig.ReadonlyView);
            if (ViewGroup == WorkOrderViewConfig.ReadonlyView)
            {
                ReadOnlyView();
            }
            else
            {
                View.InlineEdit();
                View.UseDefaultBehaviors();
                View.ClearCommands();
                View.UseCommands(typeof(SelectPackageRuleCommand));
                View.UseChildrenAsHorizontal(true);
                View.UseLayoutSize(-2.5, -2.5);
                using (View.OrderProperties())
                {
                    View.Property(p => p.PackageUnit).UsePagingLookUpEditor(p => p.DisplayMember = PackingUnit.NameProperty.Name).Show(ShowInWhere.All).Readonly(IsReadOnlyProperty);
                    View.Property(p => p.Description).Show(ShowInWhere.All);
                    View.Property(p => p.LevelQty).Show(ShowInWhere.All).UseSpinEditor(p => { p.Decimals = 0; p.MinValue = 1; }).Readonly(IsReadOnlyProperty);
                    View.Property(p => p.Qty).UseSpinEditor(p => { p.Decimals = 0; p.MinValue = 1; }).Show(ShowInWhere.All).Readonly(IsQtyReadOnlyProperty);
                    View.Property(p => p.IsPackage).Show(ShowInWhere.All);
                    View.Property(p => p.IsOutStockLabel).Show(ShowInWhere.All);
                    View.Property(p => p.IsInStockLabel).Show(ShowInWhere.All);
                    View.Property(p => p.Weight).Show(ShowInWhere.All).UseListSetting(p => p.ListGridWidth = 50);
                    View.Property(p => p.Height).Show(ShowInWhere.All).UseListSetting(p => p.ListGridWidth = 50);
                    View.Property(p => p.Volume).Show(ShowInWhere.All).UseListSetting(p => p.ListGridWidth = 50);
                    View.Property(p => p.Length).Show(ShowInWhere.All).UseListSetting(p => p.ListGridWidth = 50);
                    View.Property(p => p.Width).Show(ShowInWhere.All).UseListSetting(p => p.ListGridWidth = 50);
                    View.Property(p => p.NumberRule).Show(ShowInWhere.All);
                    View.Property(p => p.IsPrint).Show(ShowInWhere.All);
                    View.Property(p => p.PrintTemplate).UseDataSource((o, p, e) =>
                    {
                        var rule = o as WorkOrderPackageRuleDetail;
                        var templates = new EntityList<PrintTemplate>();
                        if (rule != null && rule.NumberRule != null)
                            rule.NumberRule.TemplateList.ForEach(a => templates.Add(a.Template));
                        return templates;
                    }).Show();
                    View.ChildrenProperty(p => p.WorkOrderProcessPackingUnitList).Show(ChildShowInWhere.All).UseViewGroup(WorkOrderProcessPackingUnitViewConfig.ListView);
                }
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
                View.Property(p => p.PackageUnit).UsePagingLookUpEditor(p => p.DisplayMember = PackingUnit.NameProperty.Name).Show(ShowInWhere.All);
                View.Property(p => p.Description).Show(ShowInWhere.All);
                View.Property(p => p.Qty).UseSpinEditor(p => { p.Decimals = 0; p.MinValue = 1; }).Show(ShowInWhere.All);
                View.Property(p => p.LevelQty).UseSpinEditor(p => { p.Decimals = 0; p.MinValue = 1; }).Show(ShowInWhere.All);
                View.Property(p => p.IsPackage).Show(ShowInWhere.All);
                View.Property(p => p.IsOutStockLabel).Show(ShowInWhere.All);
                View.Property(p => p.IsInStockLabel).Show(ShowInWhere.All);
                View.Property(p => p.Weight).Show(ShowInWhere.All);
                View.Property(p => p.Height).Show(ShowInWhere.All);
                View.Property(p => p.Volume).Show(ShowInWhere.All);
                View.Property(p => p.Length).Show(ShowInWhere.All);
                View.Property(p => p.Width).Show(ShowInWhere.All);
                View.Property(p => p.NumberRule).Show(ShowInWhere.All);
                View.Property(p => p.IsPrint).Show(ShowInWhere.All);
                View.Property(p => p.PrintTemplate).UseDataSource((o, p, e) =>
                {
                    var rule = o as WorkOrderPackageRuleDetail;
                    var templates = new EntityList<PrintTemplate>();
                    if (rule != null && rule.NumberRule != null)
                        rule.NumberRule.TemplateList.ForEach(a => templates.Add(a.Template));
                    return templates;
                }).Show();
                View.ChildrenProperty(p => p.WorkOrderProcessPackingUnitList).Show(ChildShowInWhere.All).UseViewGroup(WorkOrderViewConfig.ReadonlyView);
            }
        }
    }
}