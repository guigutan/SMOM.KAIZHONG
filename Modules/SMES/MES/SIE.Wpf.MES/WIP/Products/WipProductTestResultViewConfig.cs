using SIE.MES.WIP.Products;

namespace SIE.Wpf.MES.WIP.Products
{
    /// <summary>
    /// 产品测试结果视图配置
    /// </summary>
    internal class WipProductTestResultViewConfig : WPFViewConfig<WipProductTestResult>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(WipProductVersion));
            View.ClearCommands();
            View.Property(p => p.Item).Readonly();
            View.Property(p => p.Result).Readonly();
            View.Property(p => p.CreateDate).HasLabel("测试时间").Readonly().ShowInList(gridWidth: 150);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
        }
    }
}
