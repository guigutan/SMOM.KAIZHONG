using SIE.MES.TaskManagement.FeedingRecords;
using SIE.MetaModel.View;
using SIE.Web.MES.TaskManagement.FeedingRecords.Commands;

namespace SIE.Web.MES.TaskManagement.FeedingRecords
{
    public class FeedingAreaLineViewConfig : WebViewConfig<FeedingAreaReource>
    {
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(FeedingAreaResourceSelectCommand).FullName, typeof(FeedingAreaResourceDelCommand).FullName, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll, typeof(FeedingAreaReourceImportCommand).FullName, "SIE.Web.Common.Import.Commands.DownloadTemplateCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.ResourceCode).ShowInList(150);
                View.Property(p => p.ResourceName).ShowInList(150);
            }
        }

        protected override void ConfigImportView()
        {
            using (View.OrderProperties())
            {
                View.PropertyRef(p => p.Resource.Code).ShowInList(150).HasLabel("产线编码");
            }
        }

    }
}
