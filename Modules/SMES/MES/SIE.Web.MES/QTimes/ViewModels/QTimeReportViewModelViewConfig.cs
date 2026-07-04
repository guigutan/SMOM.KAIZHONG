using SIE.MES.QTimes.ViewModels;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.QTimes.ViewModels
{
    /// <summary>
    /// QT超时报表视图配置
    /// </summary>
    public class QTimeReportViewModelViewConfig : WebViewConfig<QTimeReportViewModel>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            //View.WithoutPaging();
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.Qtime).Readonly().ShowInList(width:150);
                View.Property(p => p.QTStandard).Readonly().ShowInList(width:150);
                View.Property(p => p.Barcode).Readonly().ShowInList(width:150);
                View.Property(p => p.WipResource).Readonly().ShowInList(width:150);
                View.Property(p => p.ProductCode).Readonly().ShowInList(width:150);
                View.Property(p => p.ProductName).Readonly().ShowInList(width:150);
                View.Property(p => p.BarcodeQty).Readonly().ShowInList(width:150);
                View.Property(p => p.StartProcess).Readonly().ShowInList(width:150);
                View.Property(p => p.StartState).Readonly().ShowInList(width:150);
                View.Property(p => p.StartCollectTime).Readonly().ShowInList(width:150);
                View.Property(p => p.EndProcess).Readonly().ShowInList(width:150);
                View.Property(p => p.EndState).Readonly().ShowInList(width:150);
                View.Property(p => p.EndCollectTime).Readonly().ShowInList(width:150);
                View.Property(p => p.QueryTime).Readonly().ShowInList(width: 150);
                View.Property(p => p.IsOverTime).Readonly().ShowInList(width: 150);
            }
        }
    }
}
