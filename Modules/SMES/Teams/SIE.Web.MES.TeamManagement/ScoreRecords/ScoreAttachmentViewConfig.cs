using SIE.MES.TeamManagement.ScoreRecords;

namespace SIE.Web.MES.TeamManagement.ScoreRecords
{
    /// <summary>
    /// 评分记录附件视图配置类
    /// </summary>
    internal class ScoreAttachmentViewConfig : WebViewConfig<ScoreAttachment>
    {
        /// <summary>
        /// 配置通用视图
        /// </summary>
        protected override void ConfigView()
        {
            // 配置通用视图
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.Property(p => p.FileName).Show(ShowInWhere.List);
            View.Property(p => p.FileExtesion).Show(ShowInWhere.List);
            ////View.Property(p => p.FileContent).UseImageEditor().Show(ShowInWhere.List);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}
