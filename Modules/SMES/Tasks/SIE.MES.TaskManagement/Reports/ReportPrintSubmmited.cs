using SIE.Domain;
using SIE.Items;

namespace SIE.MES.TaskManagement.Reports
{
    /// <summary>
    /// 物料保存前需要保存物料批次规则
    /// </summary>
    [System.ComponentModel.DisplayName("产品族保存后需要保存打印设置")]
    [System.ComponentModel.Description("产品族保存后需要保存打印设置")]
    public class ReportPrintSubmmited : OnSubmitted<ProductFamily>
    {
        protected override void Invoke(ProductFamily entity, EntitySubmittedEventArgs e)
        {
            var reportCt = RT.Service.Resolve<ReportController>();
            if (e.Action == SubmitAction.Insert || e.Action == SubmitAction.Update)
            {
                ReportPrintConfig reportPrintConfig = entity.GetProperty(FamilyExtReportPrint.FamilyPrintRuleProperty);
                if (reportPrintConfig != null)
                {
                    ReportPrintConfig orgReportPrintConfig = null;
                    orgReportPrintConfig = reportCt.GetReportPrintConfig(entity.Id);
                    if (orgReportPrintConfig != null)
                    {
                        orgReportPrintConfig.TemplateId = reportPrintConfig.TemplateId;
                        orgReportPrintConfig.PersistenceStatus = PersistenceStatus.Modified;
                    }
                    else
                    {
                        orgReportPrintConfig = new ReportPrintConfig();
                        orgReportPrintConfig.TemplateId = reportPrintConfig.TemplateId;
                        orgReportPrintConfig.ProductFamilyId = entity.Id;
                        orgReportPrintConfig.PersistenceStatus = PersistenceStatus.New;
                    }
                    RF.Save(orgReportPrintConfig);

                    reportPrintConfig.MarkSaved();
                }
            }
            else if (e.Action == SubmitAction.Delete)
            {
                var reportPrint = reportCt.GetReportPrintConfig(entity.Id);
                if (reportPrint != null)
                {
                    reportPrint.PersistenceStatus = PersistenceStatus.Deleted;
                    RF.Save(reportPrint);
                }
            }
        }
    }
}
