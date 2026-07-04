using SIE.MES.ProcessPrepareRecords;
using SIE.Web.MES.PrepareProducts.Commands;
using SIE.Web.MES.PrepareProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ProcessPrepareRecords
{
    public class ProcessPrepareRecordViewConfig : WebViewConfig<ProcessPrepareRecord>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(ProcessPrepareRecord));
            base.ConfigView();
            if (ViewGroup == "ExecuteViewStr")
            {
                ExecuteView();
            }
        }

        protected override void ConfigListView()
        {
            View.DisableEditing();
            using (View.OrderProperties())
            {
            }
        }


        protected void ExecuteView()
        {
            View.DisableEditing();
            View.HasDetailColumnsCount(4);
            using (View.OrderProperties())
            {
                View.ChildrenProperty(p => p.PrepareRecordDetail).Show(ChildShowInWhere.All).LazyLoad(false).ViewGroup = "ExecuteViewStr";
            }
        }
    }
}
