using SIE.MES.TeamManagement.OnLoans;

namespace SIE.Wpf.MES.TeamManagement.OnLoans
{
    /// <summary>
    /// 借调单视图配置
    /// </summary>
    internal class StorageAreaViewConfig : WPFViewConfig<WorkGroupOnLoan>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultBehaviors();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.Property(p => p.No).Readonly();
            View.Property(p => p.InitiateDate).Readonly();
            View.Property(p => p.Initiator).Readonly();
            View.Property(p => p.DemandQty).Readonly();
            View.Property(p => p.GroupOut).Readonly();
            View.Property(p => p.GroupIn).Readonly();
            View.Property(p => p.DemandQty).Readonly();
            View.Property(p => p.BeginDate).Readonly();
            View.Property(p => p.EndDate).Readonly();
            View.Property(p => p.State).Readonly();
            View.Property(p => p.Approver).Readonly();
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.ChildrenProperty(p => p.DetailList).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.EmployeeList);
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.No);
            View.Property(p => p.Initiator);
            View.Property(p => p.State);
        }
    }
}