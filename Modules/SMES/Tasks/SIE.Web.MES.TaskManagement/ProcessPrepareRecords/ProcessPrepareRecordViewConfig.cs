using SIE.MES.TaskManagement.ProcessPrepareRecords;
using SIE.Web.MES.TaskManagement.ProcessPrepareRecords.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.ProcessPrepareRecords
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
            View.UseCommands("SIE.Web.MES.TaskManagement.ProcessPrepareRecords.Commands.ProcessPrepareRecordExecuteCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrderNo).ShowInList(width: 150);
                View.Property(p => p.DispatchTaskNo).ShowInList(width: 150);
                View.Property(p => p.TaskStatus).Show().Readonly();
                View.Property(p => p.TaskPerformer).ShowInList(width: 150).Readonly();
                View.Property(p => p.ProcessName).Show().Readonly();
                View.Property(p => p.PrepareState).ShowInList(width: 150);
                View.Property(p => p.ProductCode).ShowInList(width: 150);
                View.Property(p => p.ProductName).ShowInList(width: 150);
                View.Property(p => p.ItemExtPropName).ShowInList(width: 150);
                View.Property(p => p.ProductType).HasLabel("基本分类").ShowInList(width: 150);
                View.Property(p => p.State).ShowInList(width: 150);
                View.Property(p => p.Type).ShowInList(width: 150);
                View.Property(p => p.PlanQty).ShowInList(width: 150);
                View.Property(p => p.Factory).ShowInList(width: 150);
                View.Property(p => p.WorkShop).ShowInList(width: 150);
                View.Property(p => p.Resource).ShowInList(width: 150);
                View.Property(p => p.PlanBeginDate).ShowInList(width: 150);
                View.ChildrenProperty(p => p.PrepareRecordDetail).Show(ChildShowInWhere.All);
            }
        }


        protected void ExecuteView()
        {
            View.DisableEditing();
            View.UseCommand(typeof(ComfrimCommand).FullName);
            View.AddBehavior("SIE.Web.MES.ProcessPrepareRecords.Scripts.ProcessPrepareRecordBehavior");
            View.HasDetailColumnsCount(4);
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrderNo).ShowInList(width: 150);
                View.Property(p => p.PrepareState).Show();
                View.Property(p => p.ProductCode).Show();
                View.Property(p => p.ProductName).Show();
                View.Property(p => p.ItemExtPropName).Show();
                View.Property(p => p.ProductType).HasLabel("基本分类").Show();
                View.Property(p => p.State).Show();
                View.Property(p => p.PlanQty).Show();
                View.Property(p => p.Factory).Show();
                View.Property(p => p.WorkShop).Show();
                View.Property(p => p.Resource).Show();
                View.Property(p => p.PlanBeginDate).Show();
                View.ChildrenProperty(p => p.PrepareRecordDetail).Show(ChildShowInWhere.All).ViewGroup = "ExecuteViewStr";
            }
        }
    }
}
