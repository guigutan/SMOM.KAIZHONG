using SIE.MES.TaskManagement.FeedingRecords;
using SIE.Web.MES.TaskManagement.FeedingRecords.Commands;

namespace SIE.Web.MES.TaskManagement.FeedingRecords
{
    public class FeedingAreaItemViewConfig : WebViewConfig<FeedingAreaItem>
    {
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(FeedingAreaItemSelectCommand).FullName, typeof(FeedingAreaItemDelCommand).FullName, typeof(FeedingAreaItemImportCommand).FullName, "SIE.Web.Common.Import.Commands.DownloadTemplateCommand");

            using (View.OrderProperties())
            {
                View.Property(p => p.ItemCode).ShowInList(150);
                View.Property(p => p.ItemName).ShowInList(150);
            }
        }

        protected override void ConfigImportView()
        {
            using (View.OrderProperties())
            {
                View.PropertyRef(p => p.Item.Code).ShowInList(150).HasLabel("物料编码");
            }
        }

    }
}
