using SIE.MES.BatchWIP.Products;

namespace SIE.Web.MES.BatchWIP.Products
{
    /// <summary>
    /// 产品缺陷责任视图配置
    /// </summary>
    internal class BatchWipDefectResponsibilityViewConfig : WebViewConfig<BatchWipDefectResponsibility>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(BatchWipProductVersion));
            View.ClearCommands();
            View.Property(p => p.ResponseCode).Readonly();
            View.Property(p => p.ResponseDesc).Readonly();
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
        }
    }
}
