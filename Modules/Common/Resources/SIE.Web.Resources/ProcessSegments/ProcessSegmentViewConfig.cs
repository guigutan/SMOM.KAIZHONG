using SIE.Resources.ProcessSegments;

namespace SIE.Web.Resources.ProcessSegments
{
    /// <summary>
    /// 工段
    /// </summary>
    public class ProcessSegmentViewConfig : WebViewConfig<ProcessSegment>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseDefaultCommands().UseImportCommands();
            View.Property(p => p.Code).Readonly(p => p.PersistenceStatus != Domain.PersistenceStatus.New)
                .UseListSetting(e => { e.HelpInfo = "新增状态可编辑"; });
            View.Property(p => p.Name);
            View.Property(p => p.IsSplit);
            View.Property(p => p.ReceiveMaterialSortIndex);
            
            View.Property(p => p.CreateByName);
            View.Property(p => p.CreateDate).ShowInList(150);
            View.Property(p => p.UpdateByName);
            View.Property(p => p.UpdateDate).ShowInList(150);
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.IsSplit).Readonly();
        }

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.IsSplit);
        }
    }
}
