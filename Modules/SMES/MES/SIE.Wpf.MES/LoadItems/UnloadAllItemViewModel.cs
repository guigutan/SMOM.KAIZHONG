using SIE.MES.LoadItems.ViewModels;
using SIE.Wpf.MES.BatchWIP.Assemblys;
using SIE.Wpf.MES.WIP.Assemblys;

namespace SIE.Wpf.MES.LoadItems
{
    /// <summary>
    /// 一键下料视图模型视图配置
    /// </summary>
    internal class UnloadAllItemViewModelConfig : WPFViewConfig<UnloadAllItemViewModel>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(AssemblyViewModel), typeof(WIP.Repairs.RepairViewModel), typeof(BatchAssemblyViewModel)
                , typeof(WIP.TemporaryRepairs.TemporaryRepairViewModel));
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultBehaviors();
            View.AddBehavior(typeof(UnloadAllItemBehavior));
            View.ClearCommands().UseCommands(WPFCommandNames.ListEdit);
            View.Property(p => p.IsLoadItem).UseListSetting(e => e.ListGridWidth = 50).UseCheckEditor().HasLabel("");
            View.Property(p => p.LoadItem.Item.Code).HasLabel("物料编码");
            View.Property(p => p.LoadItem.Item.Name).HasLabel("物料名称");
            View.Property(p => p.LoadItem.SourceCode).HasLabel("标签号");
            View.Property(p => p.LoadItem.SourceType).HasLabel("来源类型").UseEnumEditor();
            View.Property(p => p.LoadItem.LoadQty).HasLabel("上料数量");
            View.Property(p => p.LoadItem.Qty).HasLabel("剩余数量");            
            View.Property(p => p.LoadItem.CreateDate).UseListSetting(e => e.ListGridWidth = 120).HasLabel("上料时间");
        }
    }
}