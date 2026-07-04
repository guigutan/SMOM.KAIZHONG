using SIE.Andon.AndonStatisticsReports;
using SIE.Web.Andon.AndonStatisticsReports.Commands;
using SIE.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Andon.AndonStatisticsReports
{
    /// <summary>
    /// 安灯统计视图配置
    /// </summary>
    public class AndonStatisticsViewModelViewConfig : WebViewConfig<AndonStatisticsViewModel>
    {
        private readonly string andonTimeHelpInfoTips = "安灯管理中，验收时间-触发时间".L10N();

        private readonly string andonStopLineHelpInfoTips = "停线信息管理中,停机发生时间-停线结束时间(停线结束时间为空时，取当前时间)".L10N();

        private readonly string triggerAccuracyHelpInfoTips = "操作过安灯名称变更的单据/安灯单据总数".L10N();
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(AndonStatisticsViewModel));
            View.UseCommand(typeof(ExportMixCommand).FullName);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigListView()
        {


            View.DisableEditing();
            View.WithoutPaging();
            View.UseClientOrder();
            View.Property(p => p.AndonClass).ShowInList(100);
            View.Property(p => p.AndonType).ShowInList(100);
            View.Property(p => p.AndonName).ShowInList(100);
            View.Property(p => p.Factory).ShowInList(100);
            View.Property(p => p.WorkShop).ShowInList(100);
            View.Property(p => p.WipResource).ShowInList(100);
            View.Property(p => p.Department).ShowInList(100);
            View.Property(p => p.EquipmentName).ShowInList(100);

            View.Property(p => p.Trigger).ShowInList(100);
            View.Property(p => p.Product).ShowInList(100);
            View.Property(p => p.AndonNum).ShowInList(100);
            View.Property(p => p.AndonStopLine).ShowInList(100).UseListSetting(p => p.HelpInfo = andonStopLineHelpInfoTips);
            View.Property(p => p.AndonStopNum).ShowInList(100);
            View.Property(p => p.AndonTime).ShowInList(100).UseListSetting(p=>p.HelpInfo= andonTimeHelpInfoTips);
            View.Property(p => p.TriggerAccuracy).ShowInList(120).UseListSetting(p => p.HelpInfo = triggerAccuracyHelpInfoTips);
        }
    }
}
