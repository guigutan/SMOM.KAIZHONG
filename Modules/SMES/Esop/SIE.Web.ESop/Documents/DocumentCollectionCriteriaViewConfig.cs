using SIE.ESop.Documents;

namespace SIE.Web.ESop.Documents
{
    /// <summary>
    /// 文档集查询实体视图配置
    /// </summary>
    internal class DocumentCollectionCriteriaViewConfig : WebViewConfig<DocumentCollectionCriteria>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.All);
                View.Property(p => p.Name).Show(ShowInWhere.All);
                View.Property(p => p.FileName).Show(ShowInWhere.All);
                View.Property(p => p.Item).Show(ShowInWhere.All);
                View.Property(p => p.WorkOrder).Show(ShowInWhere.All);
            }
        }
    }
}
