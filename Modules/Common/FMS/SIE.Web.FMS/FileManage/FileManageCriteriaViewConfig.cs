using SIE.FMS;
using SIE.MetaModel.View;

namespace SIE.Web.FMS
{
    /// <summary>
    /// 文件管理查询 视图配置
    /// </summary>
    internal class FileManageCriteriaViewConfig : WebViewConfig<FileManageCriteria>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("文件管理查询");
            View.UseDefaultCommands();
            View.ReplaceCommands(WebCommandNames.ExecuteQuery, "SIE.Web.FMS.FileManage.Commands.FileManageQuery");
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.All);
                View.Property(p => p.KeyWord).HasLabel("文件名").Show(ShowInWhere.All);
                View.Property(p => p.FileState).HasLabel("状态").Show(ShowInWhere.All);              
                View.Property(p => p.CreateDate).HasLabel("上传时间").UseDateRangeEditor(p => { p.DateRangeType = ObjectModel.DateRangeType.LastMonth; }).Show(ShowInWhere.All);                
            }
        }
    }
}
