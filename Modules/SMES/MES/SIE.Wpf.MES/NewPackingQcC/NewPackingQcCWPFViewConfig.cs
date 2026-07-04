using SIE.MES.PackingQC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.NewPackingQcC
{
    public class NewPackingQcCWPFViewConfig : WPFViewConfig<PackingQc>
    {
        protected override void ConfigListView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.BlueLabel).Readonly().ShowInList();
                View.Property(p => p.PackIdent).Readonly().ShowInList();
                View.Property(p => p.Confirm).Readonly().ShowInList();
                View.Property(p => p.Item).Readonly().ShowInList();
                View.Property(p => p.ItemName).Readonly().ShowInList();
                View.Property(p => p.CreateByName).Readonly().ShowInList();
                View.Property(p => p.UpdateDate).Readonly().ShowInList();
                View.ChildrenProperty(p => p.PackingDetailList).HasLabel("装箱明细");
                View.ChildrenProperty(p => p.DocList).HasLabel("附件");
            }
        }
    }
}
