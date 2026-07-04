using SIE.Common.Prints;
using SIE.Domain;
using SIE.Packages;
using SIE.Wpf.Packages.ViewBehaviors;
using System.Linq;

namespace SIE.Wpf.Packages
{
    /// <summary>
    /// 物料包装规则明细主视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class ItemPackageRuleDetailViewConfig : WPFViewConfig<ItemPackageRuleDetail>
    {
        #region 只读控制,主单位不能编辑 IsReadOnly
        /// <summary>
        /// 只读控制,主单位不能编辑
        /// </summary> 
        public static readonly Property<bool> IsReadOnlyProperty = P<ItemPackageRuleDetail>.RegisterExtensionReadOnly("IsReadOnly", typeof(ItemPackageRuleDetailViewConfig),
            GetIsReadOnly, ItemPackageRuleDetail.PackageUnitIdProperty);

        /// <summary>
        /// 只读控制,主单位不能编辑
        /// </summary>
        public static bool GetIsReadOnly(ItemPackageRuleDetail me)
        {
            if (me.PackageUnit == null)
                return false;
            return me.PackageUnit.IsMasterUnit;
        }
        #endregion

        #region 只读控制,主单位产品数可编辑 IsReadOnly
        /// <summary>
        /// 只读控制,主单位不能编辑
        /// </summary> 
        public static readonly Property<bool> IsQtyReadOnlyProperty = P<ItemPackageRuleDetail>.RegisterExtensionReadOnly("IsQtyReadOnly", typeof(ItemPackageRuleDetailViewConfig),
            GetIsQtyReadOnly, ItemPackageRuleDetail.PackageUnitIdProperty);

        /// <summary>
        /// 只读控制,主单位不能编辑
        /// </summary>
        public static bool GetIsQtyReadOnly(ItemPackageRuleDetail me)
        {
            if (me.PackageUnit == null)
                return true;
            return !me.PackageUnit.IsMasterUnit;
        }
        #endregion

        /// <summary>
        /// 包装规则明细主信息视图ViewGroup
        /// </summary>
        public const string ItemPackRuleDtlMasterView = "ItemPackRuleDtlMasterView";

        /// <summary>
        /// 包装规则明细附加信息视图ViewGroup
        /// </summary>
        public const string ItemPackRuleDtlAttachView = "ItemPackRuleDtlAttachView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(ItemPackRuleDtlMasterView, ItemPackRuleDtlAttachView);
            View.UseDefaultBehaviors();
            View.AddBehavior(typeof(ItemPackageDetailChangeBehavior));
            if (ViewGroup == ItemPackRuleDtlMasterView)
                ItemPackRuleDtlMasterConfigView();
            else if (ViewGroup == ItemPackRuleDtlAttachView)
                ItemPackRuleDtlAttachConfigView();
        }

        /// <summary>
        /// 物料包装规则明细附加视图
        /// </summary>
        private void ItemPackRuleDtlAttachConfigView()
        {
            View.InlineEdit();
            View.ClearCommands();
            View.UseCommands(WPFCommandNames.ListEdit);
            using (View.OrderProperties())
            {
                View.Property(p => p.PackageUnit).UsePagingLookUpEditor().Show().Readonly(IsReadOnlyProperty);
                View.Property(p => p.PackageUnit.Name).Readonly(true).Show().HasLabel("单位名称");
                View.Property(p => p.Length).UseEditor(WPFEditorNames.Spin).Show().UseListSetting(w => w.ListGridWidth = 70);
                View.Property(p => p.Width).UseEditor(WPFEditorNames.Text).Show().UseListSetting(w => w.ListGridWidth = 70);
                View.Property(p => p.Height).Show().UseListSetting(w => w.ListGridWidth = 70);
                View.Property(p => p.Volume).Show().UseListSetting(w => w.ListGridWidth = 90);
                View.Property(p => p.Weight).Show().UseListSetting(w => w.ListGridWidth = 90);
            }
        }

        /// <summary>
        /// 物料包装规则明细主视图
        /// </summary>
        private void ItemPackRuleDtlMasterConfigView()
        {
            View.InlineEdit();
            View.UseCommands(WPFCommandNames.ListAdd, WPFCommandNames.ListEdit, WPFCommandNames.ListDelete);
            using (View.OrderProperties())
            {
                View.Property(p => p.PackageUnitId).UsePagingLookUpEditor().Show().Readonly(IsReadOnlyProperty);
                View.Property(p => p.PackageUnitName).Readonly(true).Show().HasLabel("单位名称");
                View.Property(p => p.LevelQty).UseSpinEditor(p => { p.MinValue = 1; p.Decimals = 0; }).Show().Readonly(IsReadOnlyProperty);
                View.Property(p => p.Qty).Show().Readonly(IsQtyReadOnlyProperty);
                View.Property(p => p.Description).Show();
                View.Property(p => p.IsMinPacking).Show().UseListSetting(w => w.ListGridWidth = 70);
                View.Property(p => p.NumberRule).UsePagingLookUpEditor().Show();
                View.Property(p => p.IsPrint).Show();
                View.Property(p => p.PrintTemplate).UseDataSource((e, p, d) =>
                {
                    var rule = e as ItemPackageRuleDetail;
                    var templates = new EntityList<PrintTemplate>();
                    if (rule != null && rule.NumberRule != null)
                        rule.NumberRule.TemplateList.ForEach(a => templates.Add(a.Template));
                    return templates;
                }).Show();
                View.Property(p => p.IsPackage).UseCheckEditor().Show().UseListSetting(w => w.ListGridWidth = 70);
                View.Property(p => p.IsOutStockLabel).Show().UseListSetting(w => w.ListGridWidth = 90);
                View.Property(p => p.IsInStockLabel).Show().UseListSetting(w => w.ListGridWidth = 90);
                View.Property(p => p.IsSequence).Show().UseListSetting(w => w.ListGridWidth = 90);
            }
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.PackageUnit);
                View.Property(p => p.PackageUnit.Name).HasLabel("单位名称");
                View.Property(p => p.LevelQty);
                View.Property(p => p.Qty);
                View.Property(p => p.Description);
            }
        }
    }
}