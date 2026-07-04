using SIE.RedCardManagment.RedCards;

namespace SIE.Web.RedCardManagment.RedCards
{
	/// <summary>
	/// 红牌管理视图配置
	/// </summary>
	internal class ProductRetroactiveCriteriaViewConfig : WebViewConfig<ProductRetroactiveCriteria>
    {

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AddBehavior("SIE.Web.RedCardManagment.RedCards.Behaviors.ProductRetroactiveCriteriaBehavior");
            using (View.OrderProperties())
            {
                View.Property(p => p.SN);
                View.Property(p => p.ItemBatch).HasLabel("物料批次").Show(ShowInWhere.All);
                View.Property(p => p.ProductSn).Show(ShowInWhere.All);
                View.Property(p => p.ProductId).HasLabel("产品编码").Show(ShowInWhere.All);
                View.Property(p => p.WorkNo).Show(ShowInWhere.All);
                View.Property(p => p.Status).Show(ShowInWhere.All);
                View.Property(p => p.ApplicantId).HasLabel("执行人").Show(ShowInWhere.All);
                View.Property(p => p.ApplyTime).Show(ShowInWhere.All).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
            }
        }


    }
}