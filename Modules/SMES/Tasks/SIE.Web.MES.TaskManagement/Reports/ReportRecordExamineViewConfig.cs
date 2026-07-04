using SIE.Domain;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.FeedingRecords;
using SIE.MES.TaskManagement.Reports;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Web.MES.TaskManagement.Reports.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.Reports
{
    /// <summary>
    /// 
    /// </summary>
    public class ReportRecordExamineViewConfig : WebViewConfig<ReportRecordExamine>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.UseCommands(WebCommandNames.ExportXls, /*WebCommandNames.ExportXlsAll,*/ typeof(ReportExportByMonthCommand).FullName);
            View.UseClientOrder();
            //View.UseCommands(typeof(ReportExamineConfirmCommand).FullName/*, typeof(ReportExamineRevokeCommand).FullName*/);
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Readonly().ShowInList(150);
                View.Property(p => p.Wo).Readonly().ShowInList(150);
                View.Property(p => p.DispatchQty).Readonly().Show();

                View.Property(p => p.ReportQty).Readonly().Show();
                View.Property(p => p.RecordOkQty).Readonly().Show();
                View.Property(p => p.RecordNgQty).Readonly().Show();
                View.Property(p => p.ReworkQty).Readonly().Show();
                View.Property(p => p.SuspectQty).Readonly().Show();
                View.Property(p => p.IsRework).Readonly().Show();

                //View.Property(p => p.ReportQty).UseDisplayEditor(p => p.ColumnXType = "ReportDispatchTaskDisplay").Readonly().ShowInList(320).HasLabel("任务进度");

                View.Property(p => p.Product).Readonly().ShowInList(150).HasLabel("产品编码");
                View.Property(p => p.ProductName).Readonly().ShowInList(150).HasLabel("产品名称");
                View.Property(p => p.ShortDescription).Readonly().ShowInList(150);
                View.Property(p => p.ProcessCode).Readonly().ShowInList(120).HasLabel("工序编码");
                View.Property(p => p.Process).Readonly().ShowInList(120);
                View.Property(p => p.Resource).Readonly().Show();
                View.Property(p => p.ResourceName).Readonly().Show();
                View.Property(p => p.WorkShopCode).Readonly().Show();
                View.Property(p => p.WorkShopId).Readonly().Show();
                View.Property(p => p.Vornr).Readonly().Show();
                View.Property(p => p.Steus).Readonly().Show();
                View.Property(p => p.Zcode).Readonly().Show();
                View.Property(p => p.BatchNo).Readonly().ShowInList(150);
                View.Property(p => p.WipBatchNos).Readonly().ShowInList(150);
                View.Property(p => p.Lichas).Readonly().ShowInList(150);
                View.Property(p => p.Principal).Readonly().Show();
                //View.Property(p => p.InspectionStatus).Readonly().Show();
                //View.Property(p => p.InspectionResult).Readonly().Show();
                //View.Property(p => p.ProcessMode).Readonly().Show();
                View.Property(p => p.ExamineState).Readonly().Show();
                View.Property(p => p.Station).Readonly().Show();
                View.Property(p => p.WorkGroup).Readonly().Show();
                View.Property(p => p.Shift).Readonly().Show();
                View.Property(p => p.ReportTime).Readonly().ShowInList(150);
                View.Property(p => p.SourceType).Show();
                View.Property(p => p.UploadFlag).Readonly().Show();
                View.Property(p => p.UploadResult).Readonly().ShowInList(200);
                View.Property(p => p.Remark).Readonly().ShowInList(200);

                View.AddBehavior(BehaviorNames.ReportRecordExamineBehavior);
                View.AttachChildrenProperty(typeof(DeductionRecord), (e) =>
                {
                    var entity = e.Parent as ReportRecordExamine;
                    if (entity == null)
                    {
                        return new EntityList<DeductionRecord>();
                    }
                    return RT.Service.Resolve<FeedingRecordController>().GetDeductionRecordsByReportId(entity.Id);
                }, ListView).Show(ChildShowInWhere.List).HasLabel("扣料记录").OrderNo = 10;
                View.AttachChildrenProperty(typeof(ReportWipBatch), (e) =>
                {
                    var entity = e.Parent as ReportRecordExamine;
                    if (entity == null)
                    {
                        return new EntityList<ReportWipBatch>();
                    }
                    return RT.Service.Resolve<ReportController>().GetReportWipBatchsByReportId(entity.Id);
                }, ListView).Show(ChildShowInWhere.List).HasLabel("标签号").OrderNo = 20;
                View.AttachChildrenProperty(typeof(DeductionRecordEditLog), (e) =>
                {
                    var entity = e.Parent as ReportRecordExamine;
                    if (entity == null)
                        return new EntityList<DeductionRecordEditLog>();
                    var args = e as ChildPagingDataWithParentEntityArgs;

                    return RT.Service.Resolve<FeedingRecordController>().GetDeductionRecordEditLogsByReportId(entity.Id, args.PagingInfo);
                }).OrderNo = 30;
            }
        }
    }
}
