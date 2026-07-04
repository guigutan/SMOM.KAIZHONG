using SIE.MES.TaskManagement.SuspectProductLabels;
using System.Diagnostics;

namespace SIE.Web.MES.SuspectProductLabels
{
    /// <summary>
    /// 可疑品标签查询 视图配置
    /// </summary>
    internal class SuspectProductLabelCriteriaViewConfig : WebViewConfig<SuspectProductLabelCriteria>
    {
        protected override void ConfigQueryView()
        {
            View.Property(p => p.BatchNo);
            View.Property(p => p.ProcessBatchNo);
            View.Property(p => p.DispatchTaskNo);
            //View.Property(p => p.ProcessBatchNo);
            View.Property(p => p.ProcessId).UsePagingLookUpEditor(p =>
            {
                p.SearchFieldList.Add(SIE.Tech.Processs.Process.CodeProperty.Name);
                p.SearchFieldList.Add(SIE.Tech.Processs.Process.NameProperty.Name);
            });
            View.Property(p => p.WorkOrderId);
            View.Property(p => p.WoState).UseEnumMutilEditor(x => x.EnumType = typeof(SIE.Core.WorkOrders.WorkOrderState));
            View.Property(p => p.ShowCloseWo);
            View.Property(p => p.ProductCode);
            View.Property(p => p.OldProductCode);
            View.Property(p => p.WorkShop);
            View.Property(p => p.DefectId);
            View.Property(p => p.SubBatchNo);
            View.Property(p => p.CreateById);
            View.Property(p => p.CreateDate).UseDateRangeEditor(c => c.DateRangeType = ObjectModel.DateRangeType.Week);
            View.Property(p => p.HandleById);
            View.Property(p => p.HandleDate).UseDateRangeEditor(c => c.DateRangeType = ObjectModel.DateRangeType.Week);
            View.Property(p => p.HandleState).Show();
        }
    }
}
