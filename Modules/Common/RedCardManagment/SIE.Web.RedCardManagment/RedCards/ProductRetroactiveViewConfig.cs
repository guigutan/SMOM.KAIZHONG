using SIE.RedCardManagment.RedCards;
using SIE.Web.RedCardManagment.RedCards.Commands;

namespace SIE.Web.RedCardManagment.RedCards
{
	/// <summary>
	/// 红牌管理视图配置(关联产品)
	/// </summary>
	internal class ProductRetroactiveViewConfig : WebViewConfig<ProductRetroactive>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(RedCard));
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseClientOrder();
            View.UseGridSelectionModel();
            View.UseCommands(typeof(ProductEnableRedCardCommand).FullName,typeof(ProductDisableRedCardCommand).FullName);
            View.AddBehavior("SIE.Web.RedCardManagment.RedCards.Behaviors.CreateProductDisableBehavior");
            View.Property(p => p.SN).Readonly().ShowInList(160);
            View.Property(p => p.ItemBatch).HasLabel("物料批次").Readonly();
            View.Property(p => p.ProductSn).Readonly();
            View.Property(p => p.ProductId).HasLabel("产品编码").Readonly(); 
            View.Property(p => p.WorkNo).Readonly();
            View.Property(p => p.Status).Readonly();
            View.Property(p => p.ApplicantId).HasLabel("执行人").Readonly();
            View.Property(p => p.ApplyTime).Readonly().ShowInList(160);
        }

    }
}