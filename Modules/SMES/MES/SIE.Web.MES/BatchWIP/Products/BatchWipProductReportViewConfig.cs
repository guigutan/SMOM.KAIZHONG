using SIE.MES.BatchWIP.Products;
using SIE.MES.BatchWIP.Products.ViewModels.BatchWipProductReport;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.BatchWIP.Products
{
    /// <summary>
    /// 工位库龄查询
    /// </summary>
    public class BatchWipProductReportViewConfig : WebViewConfig<BatchWipProductReport>
    {
        protected override void ConfigView()
        {
            base.ConfigView();
            CheckConfigView();
        }


        //<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection);
            View.Property(p => p.StationName).Readonly().Show();
            View.Property(p => p.ProcessName).Readonly().Show();
            View.Property(p => p.BatchNo).Readonly().Show();
            View.Property(p => p.BatchQty).Readonly().Show();
            View.Property(p => p.WorkOrderNo).Readonly().Show();
            View.Property(p => p.ProductCode).Readonly().Show();
            View.Property(p => p.ProductName).Readonly().Show();
            View.Property(p => p.SluggishStorageAgeDays).Readonly().Show();
        }

        /// <summary>
        /// 配置视图
        /// </summary>
        protected void CheckConfigView()
        {
            View.DisableEditing();
            View.Property(p => p.StationName).Readonly().Show();
            View.Property(p => p.ProcessName).Readonly().Show();
            View.Property(p => p.BatchNo).Readonly().Show();
            View.Property(p => p.BatchQty).Readonly().Show();
            View.Property(p => p.WorkOrderNo).Readonly().Show();
            View.Property(p => p.ProductCode).Readonly().Show();
            View.Property(p => p.ProductName).Readonly().Show();
            View.Property(p => p.SluggishStorageAgeDays).Readonly().Show();
        }
    }
}
