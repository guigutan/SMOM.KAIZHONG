using SIE.MES.WorkOrders;

namespace SIE.Wpf.MES.WorkOrders
{
    /// <summary>
    /// 工单工序清单视图配置
    /// </summary>
    internal class WorkOrderRoutingProcessViewConfig : WPFViewConfig<WorkOrderRoutingProcess>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseChildrenAsHorizontal();
            using (View.OrderProperties())
            {
                View.Property(p => p.Name).Show(ShowInWhere.All);
                View.Property(p => p.ActivityId).Show(ShowInWhere.Hide);
                View.Property(p => p.ProcessType).Show(ShowInWhere.All);
                View.Property(p => p.Segment).Show(ShowInWhere.All);
                View.Property(p => p.Process).Show(ShowInWhere.All);
                View.Property(p => p.IsOptional).Show(ShowInWhere.All);
                View.Property(p => p.CreateSku).Show(ShowInWhere.All);
                View.Property(p => p.IsRepeat).Show(ShowInWhere.All);
                View.Property(p => p.IsBuckleMaterial).Show(ShowInWhere.All);
                View.Property(p => p.IsPassRate).Show(ShowInWhere.All);
                View.Property(p => p.IsBinding).Show(ShowInWhere.All);
                View.Property(p => p.IsUnBinding).Show(ShowInWhere.All);
                View.Property(p => p.StartProcess).Show(ShowInWhere.All);
                View.Property(p => p.NormalVictoryCode).Show(ShowInWhere.All);
                View.Property(p => p.RepairVictoryCode).Show(ShowInWhere.All);
                View.Property(p => p.IsStricter).Show(ShowInWhere.All);
                View.Property(p => p.Overtime).Show(ShowInWhere.All);
                View.Property(p => p.Sign).Show(ShowInWhere.All);
                View.Property(p => p.Index).Show(ShowInWhere.All);

                View.ChildrenProperty(p => p.ParameterList).HasLabel("工艺路线参数").Show(ChildShowInWhere.List);

                View.ChildrenProperty(p => p.BomConfigList).HasLabel("BOM设置").Show(ChildShowInWhere.List).IsVisible(false);
                View.ChildrenProperty(p => p.DefectList).HasLabel("工单工序清单与参数关系").Show(ChildShowInWhere.List).IsVisible(false);
                View.ChildrenProperty(p => p.CollectStepList).HasLabel("工单工序清单与参数关系").Show(ChildShowInWhere.List).IsVisible(false);
            }
        }
    }
}
