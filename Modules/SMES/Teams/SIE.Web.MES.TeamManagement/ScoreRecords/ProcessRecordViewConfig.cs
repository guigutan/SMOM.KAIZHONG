using SIE.MES.TeamManagement.ScoreRecords;

namespace SIE.Web.MES.TeamManagement.ScoreRecords
{
    /// <summary>
    /// 处理记录视图配置类
    /// </summary>
    internal class ProcessRecordViewConfig : WebViewConfig<ProcessRecord>
    {
        /// <summary>
        /// 配置通用视图
        /// </summary>
        protected override void ConfigView()
        {
            View.ClearCommands();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.OldValue);
                View.Property(p => p.NewValue);
                View.Property(p => p.OldRatedItemCode);
                View.Property(p => p.OldRatedItemName);
                View.Property(p => p.NewRatedItemCode);
                View.Property(p => p.NewRatedItemName);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
