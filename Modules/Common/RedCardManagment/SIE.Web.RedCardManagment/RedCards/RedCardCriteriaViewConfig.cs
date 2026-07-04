using SIE.RedCardManagment.RedCards;

namespace SIE.Web.RedCardManagment.RedCards
{
    /// <summary>
    /// 红牌管理视图配置
    /// </summary>
    internal class RedCardCriteriaViewConfig : WebViewConfig<RedCardCriteria>
    {

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Show(ShowInWhere.All);
                View.Property(p => p.ItemName).Show(ShowInWhere.All);
                View.Property(p => p.Supplier).Show(ShowInWhere.All);
                View.Property(p => p.ItemSN).Show(ShowInWhere.All);
                View.Property(p => p.ItemBatch).HasLabel("物料批次").Show(ShowInWhere.All);
                View.Property(p => p.Status).Show(ShowInWhere.All);
                View.Property(p => p.AddWay).Show(ShowInWhere.All);
                View.Property(p => p.ApplyBillNo).Show(ShowInWhere.All);
                View.Property(p => p.CreateDate).Show(ShowInWhere.All).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.Week);
                View.Property(p => p.ApplyTime).Show(ShowInWhere.All).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
            }
        }


    }
}