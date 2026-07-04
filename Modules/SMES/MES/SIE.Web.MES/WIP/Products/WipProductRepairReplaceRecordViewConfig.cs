using SIE.MES.WIP.Products;

namespace SIE.Web.MES.WIP.Products
{
    /// <summary>
    /// 产品维修记录缺陷视图配置
    /// </summary>
    internal class WipProductRepairReplaceRecordViewConfig : WebViewConfig<WipProductRepairReplaceRecord>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(WipProductVersion), typeof(WipProductRouting));
            View.ClearCommands();

            View.Property(p => p.LabelNo).Readonly().ShowInList(150);
            View.Property(p => p.NewLabeNo).Readonly().ShowInList(150);
            View.Property(p => p.Qty).Readonly();
            View.Property(p => p.ItemCode).Readonly();
            View.Property(p => p.ItemName).Readonly();
            View.Property(p => p.CreateDate).Show(ShowInWhere.All);
            View.Property(p => p.CreateByName).Show(ShowInWhere.All);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.All);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.All);

        }
    }
}