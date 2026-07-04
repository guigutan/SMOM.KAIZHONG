using SIE.MES.WIP.Products;

namespace SIE.Web.MES.WIP.Products
{
    /// <summary>
    /// 产品维修记录视图配置
    /// </summary>
    internal class WipProductRepairViewConfig : WebViewConfig<WipProductRepair>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(WipProductVersion), typeof(WipProductRouting));
            View.ClearCommands();
            
            View.Property(p => p.RepairType).Readonly().ShowInList(150);
            View.Property(p => p.RepairStart).Readonly().ShowInList(150);
            View.Property(p => p.RepaireTime).Readonly().ShowInList(150);
            View.Property(p => p.RepaireByName).Readonly();
            View.Property(p => p.StationName).Readonly();
            View.Property(p => p.ProcessName).Readonly();
            View.Property(p => p.ResourceName).Readonly();
            View.Property(p => p.ShiftName).Readonly();
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.ChildrenProperty(p => p.WipProductRepairDefectList).Show(ChildShowInWhere.List).Readonly();
            View.ChildrenProperty(p => p.Attachments).Show(ChildShowInWhere.Hide);
        }
    }
}