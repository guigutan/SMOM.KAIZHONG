using SIE.MES.LoadItemRecords;
using SIE.MetaModel.View;

namespace SIE.Web.MES.LoadItemsRecords
{
    public class LoadItemsRecordViewConfig : WebViewConfig<LoadItemsRecord>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(LoadItemsRecord));
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseClientOrder();
            View.DisableEditing();
            View.UseCommands(WebCommandNames.ExportXlsAll, WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection);
            using (View.OrderProperties())
            {
                View.Property(p => p.FactoryName).HasLabel("工厂");
                View.Property(p => p.Resource).HasLabel("资源");
                View.Property(p => p.Station).HasLabel("工位");
                View.Property(p => p.ItemCode);
                View.Property(p => p.ItemName);
                View.Property(p => p.WorkOrder);
                View.Property(p => p.SourceCode);

                View.Property(p => p.SourceType);
                View.Property(p => p.OpareteType);
                View.Property(p => p.UnloadItemType).UseListSetting(p => p.HelpInfo = "操作类型=下料，才显示下料类型数据");
                View.Property(p => p.LoadDownQty);
                View.Property(p => p.OparetorName);
                View.Property(p => p.OpareteTime);
                View.Property(p => p.ItemExtPropName);
                View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
