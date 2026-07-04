using SIE.Common.Prints;
using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Packages;
using SIE.Web.Packages.Packages.Commands;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Packages.Packages
{
    /// <summary>
    /// 物料包装规则明细主视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class ItemPackageRuleDetailViewConfig : WebViewConfig<ItemPackageRuleDetail>
    {
        /// <summary>
        /// 包装规则明细主信息视图ViewGroup
        /// </summary>
        public const string ItemPackRuleDtlMasterView = "ItemPackRuleDtlMasterView";

        /// <summary>
        /// 包装规则明细附加信息视图ViewGroup
        /// </summary>
        public const string ItemPackRuleDtlAttachView = "ItemPackRuleDtlAttachView";
        private const string COLUMNX_TYPE = "itempackageruledetailcheckeditor";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(ItemPackRuleDtlMasterView, ItemPackRuleDtlAttachView);
            if (ViewGroup == ItemPackRuleDtlMasterView)
            {
                ItemPackRuleDtlMasterConfigView();
            }
            else if (ViewGroup == ItemPackRuleDtlAttachView)
            {
                ItemPackRuleDtlAttachConfigView();
            }
            else
            { 
                //
            }
        }

        /// <summary>
        /// 物料包装规则明细附加视图
        /// </summary>
        private void ItemPackRuleDtlAttachConfigView()
        {
            View.InlineEdit();
            View.ClearCommands();
            View.UseCommands(WebCommandNames.Edit);
            using (View.OrderProperties())
            {
                View.Property(p => p.PackageUnit).UsePagingLookUpEditor((m, e) =>
                {
                    var keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.PackageUnitName), nameof(e.PackageUnit.Name));
                    keyValues.Add(nameof(e.PackageUnitType), nameof(e.PackageUnit.PackageUnitType));
                    m.DicLinkField = keyValues;
                }).Show().Readonly(p => p.IsMasterUnit);
                View.Property(p => p.PackageUnitName).Readonly(true).Show().HasLabel("单位名称");
                View.Property(p => p.PackageUnitType).Show().Readonly();
                View.Property(p => p.Length).ShowInList(width: 70);
                View.Property(p => p.Width).ShowInList(width: 70);
                View.Property(p => p.Height).ShowInList(width: 70);
                View.Property(p => p.Volume).ShowInList(width: 90);
                View.Property(p => p.Weight).ShowInList(width: 90);
            }
        }

        /// <summary>
        /// 物料包装规则明细主视图
        /// </summary>
        private void ItemPackRuleDtlMasterConfigView()
        {
            View.ClearCommands();
            View.InlineEdit();
            View.UseCommands(typeof(AddItemPkgRuleDtlCommand).FullName, typeof(EditItemPkgRuleDtlCommand).FullName);
            View.UseCommands(typeof(DeleteItemPkgRuleDtlCommand).FullName);
            View.UseCommands(typeof(MoveBottomItemPkgRuleDtlCommand).FullName);
            View.UseCommands(typeof(MoveTopItemPkgRuleDtlCommand).FullName);
            View.UseCommands(typeof(MoveDownItemPkgRuleDtlCommand).FullName);
            View.UseCommands(typeof(MoveUpItemPkgRuleDtlCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(SIE.Common.Sort.SortExtension.INDEX_Property).Show(ShowInWhere.Hide);
                View.Property(p => p.PackageUnit).UsePagingLookUpEditor((m, e) =>
                {
                    var keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.PackageUnitName), nameof(e.PackageUnit.Name));
                    keyValues.Add(nameof(e.PackageUnitType), nameof(e.PackageUnit.PackageUnitType));
                    m.DicLinkField = keyValues;
                }).Show().Readonly(p => p.IsMasterUnit)
                .UseListSetting(e => { e.HelpInfo = "包装单位主单位不可编辑"; });
                View.Property(p => p.PackageUnitName).Readonly(true).Show().HasLabel("单位名称");
                View.Property(p => p.PackageUnitType).Show().Readonly();
                View.Property(p => p.LevelQty).Show().Readonly(p => p.IsMasterUnit)
                    .UseListSetting(e => { e.HelpInfo = "包装单位主单位不可编辑"; });
                View.Property(p => p.Qty).Show().Readonly(p => !p.IsMasterUnit)
                    .UseListSetting(e => { e.HelpInfo = "包装单位主单位可编辑"; });
                View.Property(p => p.Description).Show();
                View.Property(p => p.IsMinPacking).UseCheckEditor(p => p.ColumnXType = COLUMNX_TYPE).ShowInList(width: 70);
                View.Property(p => p.NumberRuleId).Show();
                View.Property(p => p.IsPrint).Show();
                View.Property(p => p.PrintTemplate).UseDataSource((e, p, d) =>
                {
                    var rule = e as ItemPackageRuleDetail;
                    var templates = new EntityList<PrintTemplate>();
                    if (rule != null && rule.NumberRule != null)
                        rule.NumberRule.TemplateList.ForEach(a => templates.Add(a.Template));
                    return templates;
                }).UseListSetting(e => { e.HelpInfo = "显示编码规则模板"; }).Show();
                View.Property(p => p.IsPackage).UseCheckEditor().ShowInList(width: 70);
                View.Property(p => p.IsOutStockLabel).UseCheckEditor(p => p.ColumnXType = COLUMNX_TYPE).ShowInList(width: 90);
                View.Property(p => p.IsInStockLabel).UseCheckEditor(p => p.ColumnXType = COLUMNX_TYPE).ShowInList(width: 90);
                View.Property(p => p.IsSequence).Readonly(p => p.ItemPackageRuleItemType != Items.ItemType.Material)
                    .UseCheckEditor(p => p.ColumnXType = COLUMNX_TYPE).ShowInList(width: 90).UseListSetting(e => { e.HelpInfo = "只有原材料才可以修改"; });
            }
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.PackageUnitId);
                View.Property(p => p.PackageUnitName).HasLabel("单位名称");
                View.Property(p => p.PackageUnitType);
                View.Property(p => p.LevelQty);
                View.Property(p => p.Qty);
                View.Property(p => p.Description);
            }
        }
    }
}
