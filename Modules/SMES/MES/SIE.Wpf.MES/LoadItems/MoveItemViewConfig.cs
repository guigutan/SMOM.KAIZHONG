using SIE.MES.LoadItems;
using SIE.Wpf.MES.BatchWIP.Assemblys;
using SIE.Wpf.MES.LoadItems.Commands;
using SIE.Wpf.MES.WIP.Assemblys;
using SIE.Wpf.MES.WIP.Repairs;

namespace SIE.Wpf.MES.LoadItems
{
    /// <summary>
    /// 工位挪料视图配置
    /// </summary>
    internal class MoveItemViewConfig : WPFViewConfig<MoveItem>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(AssemblyViewModel), typeof(RepairViewModel), typeof(BatchAssemblyViewModel));
            View.UseCommands(typeof(RefreshMoveItemCommand));
            using (View.OrderProperties())
            {
                View.Property(p => p.ItemCode).HasLabel("物料编码").Show(ShowInWhere.All);
                View.Property(p => p.ItemName).HasLabel("物料名称").Show(ShowInWhere.All);
                View.Property(p => p.SourceCode).HasLabel("标签号").Show(ShowInWhere.All);
                View.Property(p => p.SourceType).HasLabel("来源类型").Show(ShowInWhere.All);
                View.Property(p => p.ToResource).HasLabel("配送资源").Show(ShowInWhere.All);
                View.Property(p => p.ToProcess).HasLabel("配送工序").Show(ShowInWhere.All);
                View.Property(p => p.ToStation).HasLabel("配送工位").Show(ShowInWhere.All);
                View.Property(p => p.Qty).HasLabel("数量").Show(ShowInWhere.All);
                View.Property(p => p.CreateDate).HasLabel("配送时间").UseListSetting(e => e.ListGridWidth = 150).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.PropertyValueList).HasLabel("属性值").Show(ChildShowInWhere.Hide);
            }
        }
    }
}