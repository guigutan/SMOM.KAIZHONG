using SIE.MES.TaskManagement.SchedulingInfs;
using SIE.Web.MES.TaskManagement.SchedulingInfs.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.SchedulingInfs
{
    public class SchedulingInfViewConfig : WebViewConfig<SchedulingInf>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(SchedulingInf));
            base.ConfigView();
        }

        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.MES.TaskManagement.SchedulingInfs.Scripts.SchedulingInfBehavior");
            View.UseCommands(typeof(SchedulingInfImportCommand).FullName, typeof(SchedulingInfValidCommand).FullName, typeof(SchedulingInfGenerateTaskCommand).FullName, typeof(SchedulingInfCancelCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.Factory).Show().Readonly();
                View.Property(p => p.WorkOrderNo).Show().Readonly();
                View.Property(p => p.Mrb).Show().Readonly();
                View.Property(p => p.WorkOrderUpdate).Show().Readonly();
                View.Property(p => p.ProcessCode).Show().Readonly();
                View.Property(p => p.ProcessName).Show().Readonly();
                View.Property(p => p.UpdateDate).Show().Readonly();
                View.Property(p => p.MachineCode).Show().Readonly().HasLabel("资源");
                View.Property(p => p.WorkCenterCode).Show().Readonly();
                View.Property(p => p.StandardCapacity).Show().Readonly();
                View.Property(p => p.ItemCode).Show().Readonly();
                View.Property(p => p.ShortDescription).Show().Readonly();
                View.Property(p => p.Unit).Show().Readonly();
                View.Property(p => p.InStorageDate).Show().Readonly();
                View.Property(p => p.BeginDate).Show().Readonly();
                View.Property(p => p.EndDate).Show().Readonly();
                View.Property(p => p.IsCheck).Show().Readonly();
                View.Property(p => p.CheckMsg).Show().Readonly();
                View.Property(p => p.IsSchedulingInfReturn).Show().Readonly();
                View.Property(p => p.SchedulingInfReturnReason).Show().Readonly();
                View.Property(p => p.IsCancel).Show().Readonly();
                View.Property(p => p.Cancer).Show().Readonly();
                View.Property(p => p.CancelTime).Show().Readonly();
                View.Property(p => p.CancelReason).Show().Readonly();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }



    }
}
