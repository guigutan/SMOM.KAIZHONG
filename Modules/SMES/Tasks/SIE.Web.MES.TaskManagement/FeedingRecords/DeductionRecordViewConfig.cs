using SIE.MES.TaskManagement.FeedingRecords;
using SIE.MetaModel.View;
using SIE.Web.MES.TaskManagement.FeedingRecords.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.FeedingRecords
{
    /// <summary>
    /// 
    /// </summary>
    public class DeductionRecordViewConfig : WebViewConfig<DeductionRecord>
    {
        protected override void ConfigView()
        {
            if (ViewGroup == "EditView")
                ConfigEditView();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.ExportXls);
            View.UseCommands(typeof(DeductionRecordEditCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrderNo).ShowInList(150).Readonly();
                View.Property(p => p.TaskNo).ShowInList(150).Readonly();
                //View.Property(p => p.Label).ShowInList(150).Readonly().HasLabel("物料标签");
                View.Property(p => p.FeedingItemLabel).ShowInList(150).Readonly();
                View.Property(p => p.ItemLabelLot).Show().Readonly();
                View.Property(p => p.ResourceName).Show().HasLabel("机台号").Readonly();
                View.Property(p => p.ProductCode).Show().Readonly();
                View.Property(p => p.ProductName).Show().Readonly();
                View.Property(p => p.BatchNo).Show().Readonly();
                View.Property(p => p.Licha).Show().Readonly();
                View.Property(p => p.ProcessCode).Show().Readonly();
                View.Property(p => p.ProcessName).Show().Readonly();

                View.Property(p => p.ItemCode).ShowInList(150).Readonly();
                View.Property(p => p.ItemName).ShowInList(200).Readonly();
                View.Property(p => p.ShortDescription).ShowInList(150).HasLabel("扣料物料旧料号").Readonly();
                View.Property(p => p.DeductedQty).Show().Readonly();
                View.Property(p => p.EditQty).Show().Readonly();
                View.Property(p => p.UploadFlag).Readonly().Show();
                View.Property(p => p.UploadResult).Readonly().ShowInList(200);
                View.Property(p => p.Mblnr).Readonly().ShowInList(200);
                View.Property(p => p.Mjahr).Readonly().ShowInList(200);
            }
        }

        public void ConfigEditView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.EditQty).Show();
                View.Property(p => p.DeductedQty).Show().Readonly();

                View.Property(p => p.WorkOrderNo).ShowInList(150).Readonly();
                View.Property(p => p.TaskNo).ShowInList(150).Readonly();
                //View.Property(p => p.Label).ShowInList(150).Readonly().HasLabel("物料标签");
                View.Property(p => p.FeedingItemLabel).ShowInList(150).Readonly();
                View.Property(p => p.ItemLabelLot).Show().Readonly();
                View.Property(p => p.ResourceName).Show().HasLabel("机台号").Readonly();
                View.Property(p => p.ProductCode).Show().Readonly();
                View.Property(p => p.ProductName).Show().Readonly();
                View.Property(p => p.BatchNo).Show().Readonly();
                View.Property(p => p.ProcessCode).Show().Readonly();
                View.Property(p => p.ProcessName).Show().Readonly();

                View.Property(p => p.ItemCode).ShowInList(150).Readonly();
                View.Property(p => p.ItemName).ShowInList(200).Readonly();
                View.Property(p => p.ShortDescription).ShowInList(150).HasLabel("扣料物料旧料号").Readonly();
                View.Property(p => p.UploadFlag).Readonly().Show();
                View.Property(p => p.UploadResult).Readonly().ShowInList(200);
                View.Property(p => p.Mblnr).Readonly().ShowInList(200);
                View.Property(p => p.Mjahr).Readonly().ShowInList(200);
            }
        }
    }
}
