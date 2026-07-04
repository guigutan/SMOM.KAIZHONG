using DevExpress.CodeParser;
using SIE.MES.TaskManagement.PackingDetailReports;
using SIE.MetaModel.View;
using SIE.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.PackingDetailReports
{
    /// <summary>
    /// 视图配置
    /// </summary>
    public class PackingDetailReportViewConfig : WebViewConfig<PackingDetailReport>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.WithoutPaging();          
            View.UseCommand(WebCommandNames.ExportXls);
            using (View.OrderProperties())
            {

                //View.Property(p => p.OldBlueLabel).Readonly().ShowInList(width: 150);


                View.Property(p => p.WorkOrderNo).Readonly().ShowInList(width: 130);
                View.Property(p => p.ItemCode).Readonly().ShowInList(width: 120);
                View.Property(p => p.ItemName).Readonly().ShowInList(width: 120);
                View.Property(p => p.BlueLabel).Readonly().ShowInList(width: 120);
                View.Property(p => p.BlueLableNum).Readonly().ShowInList(width: 130);
                View.Property(p => p.BlueLablePackingNum).Readonly().ShowInList(width: 130);
                View.Property(p => p.UnboxedQty).Readonly().ShowInList(width: 130);
                View.Property(p => p.PackIdent).Readonly().ShowInList(width: 80);
                View.Property(p => p.ProductLabel).Readonly().ShowInList(width: 250);
                View.Property(p => p.TestValue).Readonly().ShowInList(width: 100);
                View.Property(p => p.Confirm).Readonly().ShowInList(width: 120);
                View.Property(p => p.CreateByName).Readonly().ShowInList(width: 80);
                View.Property(p => p.CreateDate).Readonly().ShowInList(width: 150);
                View.Property(p => p.UpdateByName).Readonly().ShowInList(width: 80);
                View.Property(p => p.UpdateDate).Readonly().ShowInList(width: 150);


                View.Property(p => p.ResourceCode).Readonly().ShowInList(width: 80);
                View.Property(p => p.ResourceName).Readonly().ShowInList(width: 80);

                View.Property(p => p.ReportsType).Readonly().ShowInList(width: 80);             
                View.Property(p => p.IsUploadSap).Readonly().ShowInList(width: 200);
                View.Property(p => p.UploadResult).Readonly().ShowInList(200);                  
                View.Property(p => p.LabelType).Readonly().ShowInList(width: 80);
                View.Property(p => p.BatchLabel).Readonly().ShowInList(width: 80);
                View.Property(p => p.PackingNum).Readonly().ShowInList(width: 80);



            }
        }
    }
}
