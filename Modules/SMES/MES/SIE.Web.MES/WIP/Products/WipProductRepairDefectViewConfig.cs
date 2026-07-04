using SIE.MES.WIP.Products;

namespace SIE.Web.MES.WIP.Products
{
    /// <summary>
    /// 产品维修记录缺陷视图配置
    /// </summary>
    internal class WipProductRepairDefectViewConfig : WebViewConfig<WipProductRepairDefect>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(WipProductVersion), typeof(WipProductRouting));
            View.ClearCommands();
            
            View.Property(p => p.DefectCode).Readonly().ShowInList(150);
            View.Property(p => p.DefectDesc).Readonly().ShowInList(150);
            View.Property(p => p.DefectLocation).Readonly();
            View.Property(p => p.DefectRemark).Readonly();            
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            
        }
    }
}