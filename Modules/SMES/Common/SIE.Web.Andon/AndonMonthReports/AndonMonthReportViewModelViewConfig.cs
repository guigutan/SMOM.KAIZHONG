using SIE.Andon.AndonMonthReports;
using SIE.Web.Andon.AndonStatisticsReports.Commands;
using System;

namespace SIE.Web.Andon.AndonMonthReports
{
    /// <summary>
    /// 安灯月度报表
    /// </summary>
    public class AndonMonthReportViewModelViewConfig : WebViewConfig<AndonMonthReportViewModel>
    {
        private readonly string andonTimeHelpInfoTips="安灯管理中，验收时间-触发时间".L10N();

        private readonly string andonStopLineHelpInfoTips = "停线信息管理中,停机发生时间-停线结束时间(停线结束时间为空时，取当前时间)".L10N();

        private readonly string triggerAccuracyHelpInfoTips = "操作过安灯名称变更的单据/安灯单据总数".L10N();
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(AndonMonthReportViewModel));
            View.UseCommand(typeof(ExportMixCommand).FullName);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.DraggableForTree();
                View.WithoutPaging();
                View.UseClientOrder();
                View.Property(p => p.SummaryDimension).Readonly();
                View.Property(p => p.GroupName).Readonly().HasLabel("分组层级");
                using (View.DeclareBand("1月".L10N()))
                {
                    View.Property(p => p.AndonNum1).Readonly();
                    View.Property(p => p.AndonTime1).Readonly().UseListSetting(p=>p.HelpInfo= andonTimeHelpInfoTips);
                    View.Property(p => p.AndonStopNum1).Readonly();
                    View.Property(p => p.AndonStopLine1).Readonly().UseListSetting(p=>p.HelpInfo= andonStopLineHelpInfoTips);
                    //View.Property(p => p.TriggerAccuracy1).Readonly().UseListSetting(p=>p.HelpInfo= triggerAccuracyHelpInfoTips);
                }
                using (View.DeclareBand("2月".L10N()))
                {
                    View.Property(p => p.AndonNum2).Readonly();
                    View.Property(p => p.AndonTime2).Readonly().UseListSetting(p => p.HelpInfo = andonTimeHelpInfoTips);
                    View.Property(p => p.AndonStopNum2).Readonly();
                    View.Property(p => p.AndonStopLine2).Readonly().UseListSetting(p => p.HelpInfo = andonStopLineHelpInfoTips);
                    //View.Property(p => p.TriggerAccuracy2).Readonly().UseListSetting(p => p.HelpInfo =triggerAccuracyHelpInfoTips);
                }
                using (View.DeclareBand("3月".L10N()))
                {
                    View.Property(p => p.AndonNum3).Readonly();
                    View.Property(p => p.AndonTime3).Readonly().UseListSetting(p => p.HelpInfo = andonTimeHelpInfoTips);
                    View.Property(p => p.AndonStopNum3).Readonly();
                    View.Property(p => p.AndonStopLine3).Readonly().UseListSetting(p => p.HelpInfo = andonStopLineHelpInfoTips);
                    //View.Property(p => p.TriggerAccuracy3).Readonly().UseListSetting(p => p.HelpInfo =triggerAccuracyHelpInfoTips);
                }
                using (View.DeclareBand("4月".L10N()))
                {
                    View.Property(p => p.AndonNum4).Readonly();
                    View.Property(p => p.AndonTime4).Readonly().UseListSetting(p => p.HelpInfo = andonTimeHelpInfoTips);
                    View.Property(p => p.AndonStopNum4).Readonly();
                    View.Property(p => p.AndonStopLine4).Readonly().UseListSetting(p => p.HelpInfo = andonStopLineHelpInfoTips);
                    //View.Property(p => p.TriggerAccuracy4).Readonly().UseListSetting(p => p.HelpInfo =triggerAccuracyHelpInfoTips);
                }
                using (View.DeclareBand("5月".L10N()))
                {
                    View.Property(p => p.AndonNum5).Readonly();
                    View.Property(p => p.AndonTime5).Readonly().UseListSetting(p => p.HelpInfo = andonTimeHelpInfoTips);
                    View.Property(p => p.AndonStopNum5).Readonly();
                    View.Property(p => p.AndonStopLine5).Readonly().UseListSetting(p => p.HelpInfo = andonStopLineHelpInfoTips);
                    //View.Property(p => p.TriggerAccuracy5).Readonly().UseListSetting(p => p.HelpInfo =triggerAccuracyHelpInfoTips);
                }
                using (View.DeclareBand("6月".L10N()))
                {
                    View.Property(p => p.AndonNum6).Readonly();
                    View.Property(p => p.AndonTime6).Readonly().UseListSetting(p => p.HelpInfo = andonTimeHelpInfoTips);
                    View.Property(p => p.AndonStopNum6).Readonly();
                    View.Property(p => p.AndonStopLine6).Readonly().UseListSetting(p => p.HelpInfo = andonStopLineHelpInfoTips);
                    //View.Property(p => p.TriggerAccuracy6).Readonly().UseListSetting(p => p.HelpInfo =triggerAccuracyHelpInfoTips);
                }
                
                NextYear();
            }

        }
        /// <summary>
        /// 配置下半年
        /// </summary>
        private void NextYear()
        {
            using (View.DeclareBand("7月".L10N()))
            {
                View.Property(p => p.AndonNum7).Readonly();
                View.Property(p => p.AndonTime7).Readonly().UseListSetting(p => p.HelpInfo = andonTimeHelpInfoTips);
                View.Property(p => p.AndonStopNum7).Readonly();
                View.Property(p => p.AndonStopLine7).Readonly().UseListSetting(p => p.HelpInfo = andonStopLineHelpInfoTips);
                //View.Property(p => p.TriggerAccuracy7).Readonly().UseListSetting(p => p.HelpInfo =triggerAccuracyHelpInfoTips);
            }
            using (View.DeclareBand("8月".L10N()))
            {
                View.Property(p => p.AndonNum8).Readonly();
                View.Property(p => p.AndonTime8).Readonly().UseListSetting(p => p.HelpInfo = andonTimeHelpInfoTips);
                View.Property(p => p.AndonStopNum8).Readonly();
                View.Property(p => p.AndonStopLine8).Readonly().UseListSetting(p => p.HelpInfo = andonStopLineHelpInfoTips);
                //View.Property(p => p.TriggerAccuracy8).Readonly().UseListSetting(p => p.HelpInfo =triggerAccuracyHelpInfoTips);
            }
            using (View.DeclareBand("9月".L10N()))
            {
                View.Property(p => p.AndonNum9).Readonly();
                View.Property(p => p.AndonTime9).Readonly().UseListSetting(p => p.HelpInfo = andonTimeHelpInfoTips);
                View.Property(p => p.AndonStopNum9).Readonly();
                View.Property(p => p.AndonStopLine9).Readonly().UseListSetting(p => p.HelpInfo = andonStopLineHelpInfoTips);
                //View.Property(p => p.TriggerAccuracy9).Readonly().UseListSetting(p => p.HelpInfo =triggerAccuracyHelpInfoTips);
            }
            using (View.DeclareBand("10月".L10N()))
            {
                View.Property(p => p.AndonNum10).Readonly();
                View.Property(p => p.AndonTime10).Readonly();
                View.Property(p => p.AndonStopNum10).Readonly().UseListSetting(p => p.HelpInfo = andonTimeHelpInfoTips);
                View.Property(p => p.AndonStopLine10).Readonly();
                //View.Property(p => p.TriggerAccuracy10).Readonly().UseListSetting(p => p.HelpInfo =triggerAccuracyHelpInfoTips);
            }
            using (View.DeclareBand("11月".L10N()))
            {
                View.Property(p => p.AndonNum11).Readonly();
                View.Property(p => p.AndonTime11).Readonly().UseListSetting(p => p.HelpInfo = andonTimeHelpInfoTips);
                View.Property(p => p.AndonStopNum11).Readonly();
                View.Property(p => p.AndonStopLine11).Readonly().UseListSetting(p => p.HelpInfo = andonStopLineHelpInfoTips);
                //View.Property(p => p.TriggerAccuracy11).Readonly().UseListSetting(p => p.HelpInfo =triggerAccuracyHelpInfoTips);
            }
            using (View.DeclareBand("12月".L10N()))
            {
                View.Property(p => p.AndonNum12).Readonly();
                View.Property(p => p.AndonTime12).Readonly().UseListSetting(p => p.HelpInfo = andonTimeHelpInfoTips);
                View.Property(p => p.AndonStopNum12).Readonly();
                View.Property(p => p.AndonStopLine12).Readonly().UseListSetting(p => p.HelpInfo = andonStopLineHelpInfoTips);
                //View.Property(p => p.TriggerAccuracy12).Readonly().UseListSetting(p => p.HelpInfo =triggerAccuracyHelpInfoTips);
            }
        }
    }
}
