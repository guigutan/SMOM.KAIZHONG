using SIE.LES.MaterialReceives;
using SIE.MetaModel.View;

namespace SIE.Web.LES.MaterialReceives
{
    /// <summary>
    /// 物料接收视图配置
    /// </summary>
    public class MaterialReceiveRecordViewConfig : WebViewConfig<MaterialReceiveRecord>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseClientOrder();
            View.DisableEditing();
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.SoNo).ShowInList(width: 130);
                View.Property(p => p.SoLineNo).ShowInList(width: 100);
                View.Property(p => p.SourceNo).HasLabel("来源单号").ShowInList(width: 130);
                View.Property(p => p.State);
                View.Property(p => p.ReceiveType);
                //View.Property(p => p.Item).UsePagingLookUpEditor((m, e) =>
                //{
                //    Dictionary<string, string> dic = new Dictionary<string, string>();
                //    dic.Add(nameof(e.ItemName), nameof(e.Item.Name));
                //    m.DicLinkField = dic;
                //}).HasLabel("物料编码").ShowInList(width: 100);
                View.Property(p => p.ItemCode).ShowInList(width: 130);
                View.Property(p => p.ItemName).ShowInList(width: 200);
                View.Property(p => p.ItemExtPropName).ShowInList();
                View.Property(p => p.LabelNo).ShowInList(width: 130);
                View.Property(p => p.LotCode).ShowInList(width: 130);
                View.Property(p => p.ProjectNo).ShowInList(width: 100);
                View.Property(p => p.IssuedQty).ShowInList(width: 100);
                View.Property(p => p.ReceivedQty).ShowInList(width: 100);
                View.Property(p => p.RejectedQty).ShowInList(width: 100);
                View.Property(p => p.ReceiveWarehouse).ShowInList(width: 100);
                View.Property(p => p.WoNo).ShowInList();
                View.Property(p => p.WorkShopName).ShowInList();
                View.Property(p => p.ResourceName).HasLabel("生产资源").ShowInList();
                View.Property(p => p.ReceiveByName).ShowInList();
                View.Property(p => p.ReceiveTime).ShowInList(width: 150);
            }
        }

    }
}
