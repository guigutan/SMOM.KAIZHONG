using SIE.LES.LesStockCounts;
using SIE.MetaModel.View;

namespace SIE.Web.LES.LesStockCounts
{
    /// <summary>
    /// 盘点单明细视图配置
    /// </summary>
    public class LesStockCountWorkOrderViewConfig : WebViewConfig<LesStockCountWorkOrder>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(LesStockCount));
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            View.Property(p => p.LineNo).ShowInList(width: 60).Readonly();
            View.Property(p => p.WorkOrderNo).ShowInList(width: 150).Readonly();
            View.Property(p => p.Qty).ShowInList(width: 60).Readonly();
            View.Property(p => p.ActualCountQty).ShowInList(width: 90)
                .Readonly();
            View.Property(p => p.DiffCountQty).ShowInList(width: 80).Readonly();
            View.WithoutPaging();
        }
    }
}
