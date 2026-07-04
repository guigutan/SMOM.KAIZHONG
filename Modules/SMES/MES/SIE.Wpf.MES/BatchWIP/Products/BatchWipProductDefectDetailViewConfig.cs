using SIE.MES.BatchWIP.Products;

namespace SIE.WPF.MES.BatchWIP.Products
{
    /// <summary>
    /// 产品缺陷记录明细视图配置
    /// </summary>
    internal class BatchWipProductDefectDetailViewConfig : WPFViewConfig<BatchWipProductDefectDetail>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(BatchWipProductVersion));
            View.ClearCommands();
            View.Property(p => p.DefectCode).HasLabel("编码");
            View.Property(p => p.DefectDesc).HasLabel("描述");
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
        }
    }
}
