using SIE.MetaModel.View;
using SIE.Recheck.Common.ItemRecheck;
using SIE.Web.Recheck.Common.ItemRecheck.Commands;

namespace SIE.Web.Recheck.Common.ItemRecheck
{
    /// <summary>
    /// 视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class ItemRecheckProgramViewConfig : WebViewConfig<ItemRecheckProgram>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseCommands(ItemRecheckCommands.ItemRecheckProgramAddCommand, WebCommandNames.Edit, ItemRecheckCommands.ItemRecheckProgramDeleteCommand, ItemRecheckCommands.ItemRecheckProgramSaveCommand, WebCommandNames.ExportXls);
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Readonly().ShowInList(width: 150);
                View.Property(p => p.Name).Readonly(p => p.CreateBy > 0);
                View.Property(p => p.Description);
                View.Property(p => p.MaxRecheckCount).Readonly();
                View.Property(p => p.State).Readonly();
            }
            View.ChildrenProperty(p => p.ItemRecheckProgramDetailList).HasLabel("复检保质期");
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name);
            }
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name);
            }
        }
    }
}
