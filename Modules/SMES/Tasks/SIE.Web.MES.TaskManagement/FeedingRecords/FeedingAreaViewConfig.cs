using SIE.MES.TaskManagement.FeedingRecords;
using SIE.MetaModel.View;
using SIE.Web.MES.TaskManagement.FeedingRecords.Commands;

namespace SIE.Web.MES.TaskManagement.FeedingRecords
{
    public class FeedingAreaViewConfig : WebViewConfig<FeedingArea>
    {
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.UseCommands(WebCommandNames.ExportXls);
            View.UseCommands(typeof(FeedingAreaLabelPrintCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).ShowInList(150);
                View.Property(p => p.Name).ShowInList(150);
                View.Property(p => p.Desc).ShowInList(150);
                View.Property(p => p.State).Show().Readonly();
            }

            View.ChildrenProperty(p => p.ResourceList).HasOrderNo(1);
            View.ChildrenProperty(p => p.ItemList).HasOrderNo(2);
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show();
                View.Property(p => p.Name).Show();
                View.Property(p => p.State).Show();
            }
        }
    }
}
