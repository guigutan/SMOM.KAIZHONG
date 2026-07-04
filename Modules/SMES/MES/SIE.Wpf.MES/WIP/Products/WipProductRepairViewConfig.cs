using SIE.MES.WIP.Products;

namespace SIE.Wpf.MES.WIP.Products
{
    /// <summary>
    /// 产品维修记录视图配置
    /// </summary>
    internal class WipProductRepairViewConfig : WPFViewConfig<WipProductRepair>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(WipProductVersion), typeof(WipProductRouting));
            View.ClearCommands();
            View.Property(p => p.RepaireTime).ShowInList(gridWidth: 150);
            View.Property(p => p.RepaireByName);
            View.Property(p => p.StationName);
            View.Property(p => p.ProcessName);
            View.Property(p => p.ResourceName);
            View.Property(p => p.ShiftName);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.ChildrenProperty(p => p.WipProductRepairDefectList).Show(ChildShowInWhere.List).Readonly();
        }
    }
}