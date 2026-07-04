using SIE.MES.WIP.Products;

namespace SIE.Wpf.MES.WIP.Products
{
    /// <summary>
    /// 产品缺陷记录视图配置
    /// </summary>
    internal class WipProductDefectViewConfig : WPFViewConfig<WipProductDefect>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(WipProductVersion), typeof(WipProductRouting));
            View.ClearCommands();
            View.Property(p => p.Process);
            View.Property(p => p.DefectCode);
            View.Property(p => p.DefectDesc);
            View.Property(p => p.InspItemName).HasLabel("检验项描述");
            View.Property(p => p.Location);
            View.Property(p => p.EmployeeName);
            View.Property(p => p.FixedDate).ShowInList(gridWidth: 150);
            View.Property(p => p.Remark);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.ChildrenProperty(p => p.ResponsibilityList).HasLabel("缺陷责任");
            View.ChildrenProperty(p => p.MeasureList).HasLabel("维修措施");
        }
    }
}