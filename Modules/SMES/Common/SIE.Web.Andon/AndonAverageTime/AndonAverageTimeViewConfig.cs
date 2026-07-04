using SIE.Andon.AndonAverageTime;
using SIE.Andon.AndonStatisticsReports;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Andon.AndonAverageTime
{
    /// <summary>
    /// 
    /// </summary>
    public class AndonAverageTimeViewConfig : WebViewConfig<SIE.Andon.AndonAverageTime.AndonAverageTimeViewModel>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            View.UseClientOrder();            
            View.Property(p => p.ByUserName).Readonly().ShowInList(100);
            View.Property(p => p.ResponseDurationAverage).Readonly().ShowInList(150);
            View.Property(p => p.HandleDurationAverage).Readonly().ShowInList(150);
            View.Property(p => p.CheckDurationAverage).Readonly().ShowInList(150); 

        }
    }
}