using SIE.Domain;
using SIE.MES.TaskManagement.SuspectProductLabels;
using SIE.MES.TaskManagement.SuspectProductLabels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.SuspectProductLabels.ViewModels
{
    public class ProcessingViewModelViewConfig : WebViewConfig<ProcessingViewModel>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(SuspectProductLabel));
            if (ViewGroup == "ProcessingView")
                ConfigProcessingView();
        }

        private void ConfigProcessingView()
        {
            using (View.OrderProperties())
            {
                View.HasDetailColumnsCount(4);
                View.Property(p => p.SuspectProductLabel).Show().Readonly();
                View.Property(p => p.ItemDesc).Show().Readonly();
                View.Property(p => p.Qty).Show().Readonly();
                View.Property(p => p.GoodQty).Show();
                View.AttachChildrenProperty(typeof(ProcessingDefectDtlViewModel), (e) =>
                {
                    return new EntityList<ProcessingDefectDtlViewModel>();
                }).HasLabel("报废/返工品处理").UseViewGroup("ListView").Show(ChildShowInWhere.All);
            }
        }
    }
}
