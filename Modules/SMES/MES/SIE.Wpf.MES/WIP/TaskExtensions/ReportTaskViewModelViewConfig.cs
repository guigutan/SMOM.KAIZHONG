using SIE.MES.WIP.TaskExtensions;
using SIE.Wpf.MES.BatchWIP.Assemblys;
using SIE.Wpf.MES.BatchWIP.Inspects;
using SIE.Wpf.MES.BatchWIP.Moves;
using SIE.Wpf.MES.BatchWIP.Packings;
using SIE.Wpf.MES.WIP.Assemblys;
using SIE.Wpf.MES.WIP.Inspects;
using SIE.Wpf.MES.WIP.Moves;
using SIE.Wpf.MES.WIP.Packings;

namespace SIE.Wpf.MES.WIP.TaskExtensions
{
    internal class ReportTaskViewModelViewConfig : WPFViewConfig<ReportTaskViewModel>
    {
        private static string CollectView = "CollectionView";

        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(AssemblyViewModel), typeof(MoveViewModel), typeof(InspectViewModel), typeof(PackingViewModel));
            View.AssignAuthorize(typeof(BatchAssemblyViewModel), typeof(BatchMoveViewModel), typeof(BatchInspectViewModel), typeof(BatchPackingViewModel));
            View.DeclareExtendViewGroup(CollectView);
            if (ViewGroup == CollectView)
            {
                ConfigCollectView();
            }
        }

        void ConfigCollectView()
        {
            View.UseCommands(typeof(RefreshTaskCommand));
            using (View.OrderProperties())
            {
                View.Property(p => p.No).ShowInList();
                View.Property(p => p.DispatchQty).ShowInList();
                View.Property(p => p.WorkOrderNo).ShowInList();
                View.Property(p => p.ProductCode).ShowInList();
                View.Property(p => p.Priority).ShowInList();
                View.Property(p => p.ProcessName).ShowInList();
                View.Property(p => p.ResourceName).ShowInList();
                View.Property(p => p.OkQty).ShowInList(80);
                View.Property(p => p.NgQty).ShowInList(80);
                View.Property(p => p.ToReportQty).ShowInList(80);
                View.Property(p => p.TaskStatus).ShowInList();
                View.Property(p => p.BeginTime).ShowInList();
                View.Property(p => p.EndTime).ShowInList();
                View.Property(p => p.CreateDate).ShowInList();
                View.Property(p => p.PlanBeginTime).ShowInList();
                View.Property(p => p.PlanEndTime).ShowInList();
            }
        }
    }
}