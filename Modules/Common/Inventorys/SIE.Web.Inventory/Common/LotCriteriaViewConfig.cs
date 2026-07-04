using SIE.Inventory.Commom;
using SIE.Items;

namespace SIE.Web.Inventory.Common
{
    /// <summary>
    /// 批次信息查询视图
    /// </summary>
    internal class LotCriteriaViewConfig : WebViewConfig<LotCriteria>
    {
        /// <summary>
        /// 配置扩展明细视图
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Item).UsePagingLookUpEditor(p =>
                {
                    p.SearchFieldList.Add(Item.CodeProperty.Name);
                    p.SearchFieldList.Add(Item.NameProperty.Name);
                    p.SearchFieldList.Add(Item.SpecificationModelProperty.Name);
                }).Show(ShowInWhere.All).HasLabel("物料编码");
                View.Property(p => p.Code).Show(ShowInWhere.All).HasLabel("批号");
                View.Property(p => p.LotAtt01).UseDateTimeEditor().Show(ShowInWhere.All).HasLabel("生产日期");
                View.Property(p => p.LotAtt02).UseDateTimeEditor().Show(ShowInWhere.All).HasLabel("最大有效期");
                View.Property(p => p.LotAtt03).UseDateTimeEditor().Show(ShowInWhere.All).HasLabel("收货日期");
                View.Property(p => p.LotAtt04).Show(ShowInWhere.All).HasLabel("生产批次");
                View.Property(p => p.AsnNo).Show(ShowInWhere.All).HasLabel("来源单号");
            }
        }
    }
}
