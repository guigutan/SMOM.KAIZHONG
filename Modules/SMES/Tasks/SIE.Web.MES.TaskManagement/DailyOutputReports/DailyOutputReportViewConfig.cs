using DocumentFormat.OpenXml.Wordprocessing;
using SIE.Domain;
using SIE.MES.TaskManagement.DailyOutputReports;
using SIE.MetaModel.View;
using System;

namespace SIE.Web.MES.TaskManagement.DailyOutputReports
{
    /// <summary>
    /// 视图配置
    /// </summary>
    internal class DailyOutputReportViewConfig : WebViewConfig<DailyOutputReport>
    {
        protected override void ConfigListView()
        {
            View.WithoutPaging();
            View.DisableEditing().UseClientOrder();

            //View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            View.UseCommands("SIE.Web.MES.TaskManagement.DailyOutputReports.Commands.DailyOutputReportExportCommand", "SIE.Web.MES.TaskManagement.DailyOutputReports.Commands.DailyOutputReportExportAllCommand");

            using (View.OrderProperties())
            {
                View.Property(p => p.Date).ShowInList().Readonly().UseDateEditor();
                View.Property(p => p.Division).ShowInList().Readonly();
                View.Property(p => p.Department).ShowInList().Readonly();
                View.Property(p => p.Factory).ShowInList().Readonly();
                View.Property(p => p.MrpController).ShowInList().Readonly();
                View.Property(p => p.ProcessCode).ShowInList(150).Readonly();
                //View.Property(p => p.ProcessName).ShowInList(150).Readonly();
                View.Property(p => p.ResourceCode).ShowInList(150).Readonly();
                View.Property(p => p.ResourceName).ShowInList(150).Readonly();
                View.Property(p => p.ProductCode).ShowInList(150).Readonly();
                View.Property(p => p.ProductName).ShowInList(150).Readonly();
                View.Property(p => p.ShortDescription).ShowInList(150).Readonly();
                View.Property(p => p.ParentOldItem).ShowInList(150).Readonly();
                View.Property(p => p.Capacity).ShowInList().Readonly();
                View.Property(p => p.TaskQty).ShowInList().Readonly();
                View.Property(p => p.ReportedQty).ShowInList().Readonly();
                View.Property(p => p.AchievementRate).ShowInList().Readonly();
                //View.Property(p => p.DiffQty).ShowInList().Readonly()
                //    .UseDisplayEditor(p => p.ColumnXType = "DiffQtyColorChange")
                //    .UseListSetting(e => { e.HelpInfo = "差异数量=报工数量-排程数量"; });

                using (View.DeclareBand("报工数量".L10N()))
                {
                    //View.Property(p => p.DayShiftTaskQty).ShowInList().Readonly();
                    View.Property(p => p.DayShiftReportedQty).ShowInList().Readonly().HasLabel("白班");
                    //View.Property(p => p.DayShiftDiffQty).ShowInList().Readonly()
                    //    .UseDisplayEditor(p => p.ColumnXType = "DiffQtyColorChange")
                    //    .UseListSetting(e => { e.HelpInfo = "差异数量=报工数量-排程数量"; });
                
                    //View.Property(p => p.NightShiftTaskQty).ShowInList().Readonly();
                    View.Property(p => p.NightShiftReportedQty).ShowInList().Readonly().HasLabel("夜班");
                    //View.Property(p => p.NightShiftDiffQty).ShowInList().Readonly()
                    //    .UseDisplayEditor(p => p.ColumnXType = "DiffQtyColorChange")
                    //    .UseListSetting(e => { e.HelpInfo = "差异数量=报工数量-排程数量"; });
                }
                using (View.DeclareBand("白班小时产出".L10N()))
                {
                    View.Property(p => p.OuputQty_08to10).ShowInList().Readonly();
                    View.Property(p => p.OuputQty_10to12).ShowInList().Readonly();
                    View.Property(p => p.OuputQty_12to15).ShowInList().Readonly();
                    View.Property(p => p.OuputQty_15to17).ShowInList().Readonly();
                    View.Property(p => p.OuputQty_17to20).ShowInList().Readonly();
                }
                using (View.DeclareBand("夜班小时产出".L10N()))
                {
                    View.Property(p => p.OuputQty_20to22).ShowInList().Readonly();
                    View.Property(p => p.OuputQty_22to00).ShowInList().Readonly();
                    View.Property(p => p.OuputQty_00to03).ShowInList().Readonly();
                    View.Property(p => p.OuputQty_03to05).ShowInList().Readonly();
                    View.Property(p => p.OuputQty_05to08).ShowInList().Readonly();
                }
                View.Property(p => p.ReasonCategory).ShowInList(150).Readonly();
                View.Property(p => p.ReasonAnalysis).ShowInList(150).Readonly();

            }
        }
    }
}
