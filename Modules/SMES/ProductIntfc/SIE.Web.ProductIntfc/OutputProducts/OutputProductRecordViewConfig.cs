using SIE.MetaModel.View;
using SIE.ProductIntfc.OutputProducts;
using SIE.Web.Items._Extentions_;

namespace SIE.Web.ProductIntfc.OutputProducts
{
    /// <summary>
    /// 入库单
    /// </summary>
    public class OutputProductRecordViewConfig : WebViewConfig<OutputProductRecord>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.UseCommands(WebCommandNames.ExportXls);
            using (View.OrderProperties())
            {
                
                View.Property(p => p.OutPutType).Readonly().UseEnumEditor().ShowInList(150);
                View.Property(p => p.ItemCode).Readonly().ShowInList(150);
                View.Property(p => p.ItemName).Readonly().ShowInList(150);
                View.Property(p => p.ShortDescription).Readonly().ShowInList(150);
                //View.Property(p => p.ItemExtPropName).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.Qty).UseItemUnitEditor().Show(ShowInWhere.All);
                View.Property(p => p.SubmitQty).HasLabel("总量").Readonly().Show(ShowInWhere.All);

                View.Property(p => p.UploadFlag).Readonly().Show();
                View.Property(p => p.UploadResult).Readonly().ShowInList(200);
                View.Property(p => p.Mblnr).Readonly().ShowInList(200);
                View.Property(p => p.Mjahr).Readonly().ShowInList(200);

            }
        }
    }
}
