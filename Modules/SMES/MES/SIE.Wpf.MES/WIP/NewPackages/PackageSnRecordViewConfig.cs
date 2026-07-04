using SIE.MES.WIP.NewPackages;
using SIE.Wpf.Common.ViewBehaviors;
using SIE.Wpf.MES.WIP.NewPackages.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.WIP.NewPackages
{
    public class PackageSnRecordViewConfig : WPFViewConfig<PackageSnRecord>
    {
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(NewPackageViewModel));
            View.AddBehavior(typeof(MultipleRowViewBehavior));
            View.ClearCommands();
            View.UseCommands(typeof(NewPackingCommand));
            using (View.OrderProperties())
            {
                View.Property(p => p.Sn).Show();
                View.Property(p => p.PackageUnit.Name).HasLabel("包装单位").Show();
                View.Property(p => p.WorkOrder.No).HasLabel("工单号").Show();
                View.Property(p => p.Product.Code).HasLabel("产品").Show();
                View.Property(p => p.WoSn).ShowInList(gridWidth: 800);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);

            }
        }
    }
}
