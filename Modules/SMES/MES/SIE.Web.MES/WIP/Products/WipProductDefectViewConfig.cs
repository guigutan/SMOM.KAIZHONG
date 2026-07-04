using SIE.MES.WIP.Products;

namespace SIE.Web.MES.WIP.Products
{
    /// <summary>
    /// 产品缺陷记录视图配置
    /// </summary>
    internal class WipProductDefectViewConfig : WebViewConfig<WipProductDefect>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(WipProductVersion), typeof(WipProductRouting));
            View.ClearCommands();
            View.Property(p => p.ProcessName).Readonly();
            View.Property(p => p.DefectCode).Readonly();
            View.Property(p => p.DefectDesc).Readonly();
            View.Property(p => p.InspItemName).Readonly().HasLabel("检验项描述");
            View.Property(p => p.BoardNo).Show(ShowInWhere.Hide).Readonly();
            View.Property(p => p.Sn).Readonly();
            View.Property(p => p.Location).Readonly();
            View.Property(p => p.EmployeeName).Readonly();
            View.Property(p => p.IsMisjudgment).Readonly();
            View.Property(p => p.FixedDate).Readonly().ShowInList(150);
            View.Property(p => p.Remark).Readonly();
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.ChildrenProperty(p => p.ResponsibilityList).HasLabel("缺陷责任");
            View.ChildrenProperty(p => p.MeasureList).HasLabel("维修措施");
        }
    }
}