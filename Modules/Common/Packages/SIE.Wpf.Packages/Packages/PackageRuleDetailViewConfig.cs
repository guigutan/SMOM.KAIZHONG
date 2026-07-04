using SIE.Common.Prints;
using SIE.Domain;
using SIE.Packages;
using SIE.Packages.Packages;
using SIE.Wpf.Common.Sort;
using SIE.Wpf.Packages.Packages.Commands;
using SIE.Wpf.Packages.Packages.ViewBehaviors;
using System.Linq;

namespace SIE.Wpf.Packages
{
    /// <summary>
    /// 包装规则明细视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class PackageRuleDetailViewConfig : WPFViewConfig<PackageRuleDetail>
    {
        /// <summary>
        /// 包装规则明细 - 主信息 ViewGroup名
        /// </summary>
        public const string DetailMainViewGroup = "PackageRuleDetailMaster";

        /// <summary>
        /// 包装规则明细 - 附加 ViewGroup名
        /// </summary>
        public const string DetailSubViewGroup = "PackageRuleDetailSub";

        #region 只读控制,主单位不能编辑 IsReadOnly
        /// <summary>
        /// 只读控制
        /// </summary> 
        public static readonly Property<bool> IsReadOnlyProperty = P<PackageRuleDetail>.RegisterExtensionReadOnly("IsReadOnly", typeof(PackageRuleDetailViewConfig),
            GetIsReadOnly, PackageRuleDetail.PackageUnitIdProperty);

        /// <summary>
        /// 只读控制
        /// </summary>
        /// <param name="me">包装规则明细</param>
        /// <returns>bool</returns>
        public static bool GetIsReadOnly(PackageRuleDetail me)
        {
            if (me.PackageUnit == null)
                return false;
            return me.PackageUnit.IsMasterUnit;
        }
        #endregion

        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            View.DeclareExtendViewGroup(DetailMainViewGroup, DetailSubViewGroup);
            View.UseDefaultBehaviors();
            View.AddBehavior(typeof(PackageRuleDetailChangeBehavior));
            if (ViewGroup == DetailMainViewGroup)
                DetailMainConfigView();
            else if (ViewGroup == DetailSubViewGroup)
                DetailSubConfigView();
        }

        /// <summary>
        /// 包装规则明细主信息视图
        /// </summary>
        private void DetailSubConfigView()
        {
            View.InlineEdit();
            View.DomainName("附加信息");
            View.ClearCommands().UseCommands(WPFCommandNames.ListEdit);
            using (View.OrderProperties())
            {
                View.Property(p => p.PackageUnit).UsePagingLookUpEditor(p => p.DisplayMember = PackingUnit.NameProperty.Name).ShowInList().HasLabel("单位名称").Readonly(IsReadOnlyProperty);
                View.Property(p => p.Length).UseSpinEditor(p => p.MinValue = 0).ShowInList();
                View.Property(p => p.Width).UseSpinEditor(p => p.MinValue = 0).ShowInList();
                View.Property(p => p.Height).UseSpinEditor(p => p.MinValue = 0).ShowInList();
                View.Property(p => p.Volume).UseSpinEditor(p => p.MinValue = 0).ShowInList();
                View.Property(p => p.Weight).UseSpinEditor(p => p.MinValue = 0).ShowInList();
            }
        }

        /// <summary>
        /// 包装规则明细附加信息视图
        /// </summary>
        private void DetailMainConfigView()
        {
            View.InlineEdit();
            View.DomainName("主信息");
            View.ReplaceCommands(WPFCommandNames.ListAdd, typeof(PackageRuleDetailAddCommand));
            View.UseCommands(WPFCommandNames.ListEdit); ////WPFCommandNames.ListAdd,
            View.ReplaceCommands(WPFCommandNames.ListDelete, typeof(PackageRuleDetailDeleteCommand));
            View.ReplaceCommands(typeof(MoveUpCommand), typeof(PackageRuleDetailMoveUpCommand));
            View.ReplaceCommands(typeof(MoveDownCommand), typeof(PackageRuleDetailMoveDownCommand));
            View.ReplaceCommands(typeof(MoveTopCommand), typeof(PackageRuleDetailMoveTopCommand));
            View.ReplaceCommands(typeof(MoveBottomCommand), typeof(PackageRuleDetailMoveBottomCommand));

            using (View.OrderProperties())
            {
                View.Property(p => p.PackageUnit).UsePackageRuleDetailEditor().ShowInList().Readonly(IsReadOnlyProperty);
                View.Property(p => p.PackageUnit.Name).ShowInList();
                View.Property(p => p.LevelQty).ShowInList().UseSpinEditor(p => { p.Decimals = 0; p.MinValue = 0; }).Readonly(IsReadOnlyProperty);
                View.Property(p => p.Qty).ShowInList().UseSpinEditor(p => { p.Decimals = 0; p.MinValue = 0; }).Readonly();
                View.Property(p => p.Description).ShowInList();
                View.Property(p => p.NumberRule).ShowInList();
                View.Property(p => p.IsPrint).ShowInList().Readonly(IsReadOnlyProperty);
                View.Property(p => p.PrintTemplate).UsePagingLookUpEditor(e => e.ReloadDataOnPopping = true).UseDataSource((e, p, d) =>
                {
                    var rule = e as PackageRuleDetail;
                    var templates = new EntityList<PrintTemplate>();
                    if (rule == null || rule.NumberRule == null)
                        return templates;
                    rule.NumberRule.TemplateList.Where(s => s.Template.State == State.Enable).ForEach(a => templates.Add(a.Template));
                    return templates;
                }).ShowInList();
                View.Property(p => p.IsPackage).ShowInList();
                View.Property(p => p.IsOutStockLabel).ShowInList();
                View.Property(p => p.IsInStockLabel).ShowInList();
                View.Property(p => p.IsSequence).ShowInList();
            }
        }

        /// <summary>
        /// 选择视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.PackageRuleId);
            View.Property(p => p.PackageRuleName);
        }
    }
}