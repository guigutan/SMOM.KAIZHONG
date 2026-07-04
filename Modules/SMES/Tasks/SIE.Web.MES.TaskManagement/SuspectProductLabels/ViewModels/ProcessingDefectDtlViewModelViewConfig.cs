using SIE.MES.TaskManagement.SuspectProductLabels;
using SIE.MES.TaskManagement.SuspectProductLabels.ViewModels;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.SuspectProductLabels.ViewModels
{
    public class ProcessingDefectDtlViewModelViewConfig : WebViewConfig<ProcessingDefectDtlViewModel>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(SuspectProductLabel), typeof(ProcessingViewModel));
        }

        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.UseCommands(WebCommandNames.Add, WebCommandNames.Delete);
                View.Property(p => p.Qty).Show();
                View.Property(p => p.DefectId).Show();
                View.Property(p => p.SuspectJudgeResult).Show();
            }
        }
    }
}
