using SIE.MES.TaskManagement.SuspectProductLabels;
using SIE.MetaModel.View;

namespace SIE.Web.MES.SuspectProductLabels
{
    /// <summary>
    /// 可疑品标签 视图配置
    /// </summary>
    internal class SuspectProductLabelViewConfig : WebViewConfig<SuspectProductLabel>
    {

        protected override void ConfigListView()
        {
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.UseCommands("SIE.Web.MES.TaskManagement.SuspectProductLabels.Commands.SuspectLabelProcessingCommand", WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
                View.Property(p => p.BatchNo).ShowInList(150).UseDisplayEditor(p => p.ColumnXType = "SuspectQtyColorChange");
                View.Property(p => p.ProcessBatchNo).ShowInList(150).Readonly();
                View.Property(p => p.Qty);
                View.Property(p => p.ProcessId);
                View.Property(p => p.HandleState).Show().Readonly();
                //View.Property(p => p.ProcessBatchNo);
                View.Property(p => p.ProductShortDescription).Readonly().Show();
                View.Property(p => p.Bismt).Show().Readonly();
                View.Property(p => p.WorkOrderId).ShowInList(150);
                View.Property(p => p.DispatchTask).ShowInList(150);
                View.Property(p => p.WorkShopName).ShowInList(150);
                View.Property(p => p.WipResourceId).ShowInList(150);
                View.Property(p => p.WipResourceName).ShowInList(150);
                View.Property(p => p.ProductCode).ShowInList(150);
                View.Property(p => p.ProductName).ShowInList(150);
                View.Property(p => p.GoodQty);
                View.Property(p => p.ScrapQty);
                View.Property(p => p.RepairQty);
                View.Property(p => p.LabelType).Readonly();
                View.ChildrenProperty(p => p.DetailList);
                View.ChildrenProperty(p => p.AttachmentList);
            }
        }
    }
}
