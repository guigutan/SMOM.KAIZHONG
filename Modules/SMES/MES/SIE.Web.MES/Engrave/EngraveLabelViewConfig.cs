using SIE.MES.Engrave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Engrave
{
    internal class EngraveLabelViewConfig :WebViewConfig<EngraveLabel>
    {
        protected override void ConfigListView()
        {
            View.Property(p => p.WorkOrder).ShowInList(150).Readonly();
            View.Property(p => p.BatchNo).ShowInList(150).Readonly();
            View.Property(p => p.Resource).ShowInList(150).Readonly();
            View.Property(p => p.Product).ShowInList(150).Readonly();
            View.Property(p => p.Qty).ShowInList(100).Readonly();
            View.ChildrenProperty(p => p.EngraveSnList).HasLabel("SN明细");
        }
    }
}
