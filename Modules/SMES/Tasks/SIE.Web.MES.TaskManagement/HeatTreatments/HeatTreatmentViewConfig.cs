using SIE.MES.TaskManagement.HeatTreatments;
using SIE.MetaModel.View;
using SIE.Web.MES.TaskManagement.HeatTreatments.Commands;

namespace SIE.Web.MES.TaskManagement.HeatTreatments
{
    /// <summary>
    /// 
    /// </summary>
    public class HeatTreatmentViewConfig : WebViewConfig<HeatTreatment>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.UseCommands(typeof(HeatTreatmentReportCommand).FullName, WebCommandNames.ExportXls);
            using (View.OrderProperties())
            {
                View.Property(p => p.Barcode).ShowInList(150).Readonly();
                View.Property(p => p.ProductCode).ShowInList(180);
                View.Property(p => p.ShortDescription).ShowInList(150).Show();
                View.Property(p => p.OperationType).Show();
                View.Property(p => p.EnableTime1).ShowInList(180);
                View.Property(p => p.DevId).Show();
                View.Property(p => p.Count00).Show();
                View.Property(p => p.SvTime).Show();
                View.Property(p => p.SvTimeMs).Show();
                View.Property(p => p.DevName).Show();
                View.Property(p => p.ErrNum).Show();
                View.Property(p => p.RunPro).Show();
                View.Property(p => p.State).Show();
                View.Property(p => p.Rec).Show();
                View.Property(p => p.SvId).Show();
                View.Property(p => p.TmpH).Show();
                View.Property(p => p.TmpL).Show();
                View.Property(p => p.Tmp).Show();
                View.Property(p => p.Tmp1).Show();
                View.Property(p => p.Tmp2).Show();
                View.Property(p => p.Tmp3).Show();
                View.Property(p => p.Tmp4).Show();
                View.Property(p => p.RunTime).Show();
                View.Property(p => p.RunId).Show();
                View.Property(p => p.Layer00).Show();
                View.Property(p => p.Card00).Show();
                View.Property(p => p.Type00).Show();
                View.Property(p => p.TmpH1).Show();
                View.Property(p => p.TmpH2).Show();
                View.Property(p => p.Ch1).Show();
                View.Property(p => p.Ch2).Show();
                View.Property(p => p.Ch3).Show();
                View.Property(p => p.Ch4).Show();
                View.Property(p => p.Flag).Show();
                View.Property(p => p.EnableTime).ShowInList(180);
                View.Property(p => p.PlanNum).Show();
                View.Property(p => p.ProductNum).Show();
                View.Property(p => p.WorkId).Show();
                View.Property(p => p.Factory).Show();
                View.Property(p => p.IsReported).Show();
                View.Property(p => p.ReportQty).Show();
                View.Property(p => p.Remark).Show();
            }
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Barcode).Show();
                View.Property(p => p.ProductCode).Show();
                View.Property(p => p.ShortDescription).Show();
                View.Property(p => p.OperationType).UseEnumEditor().Show();
                View.Property(p => p.EnableTime1).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All).Show();
                View.Property(p => p.DevId).Show();
                //View.Property(p => p.Count00).Show();
                //View.Property(p => p.SvTime).Show();
                //View.Property(p => p.SvTimeMs).Show();
                View.Property(p => p.DevName).Show();
                View.Property(p => p.IsReported).Show().UseCheckDropDownEditor();
            }
        }
    }
}
