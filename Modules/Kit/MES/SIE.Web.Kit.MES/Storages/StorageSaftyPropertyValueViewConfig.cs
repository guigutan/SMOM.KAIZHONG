using SIE.Kit.MES.Storages;

namespace SIE.Web.Kit.MES.Storages
{
    /// <summary>
    /// 物料库存属性值-界面
    /// </summary>
    internal class StorageSaftyPropertyValueViewConfig : WebViewConfig<StorageSaftyPropertyValue>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseGridSelectionModel();
            View.Property(p => p.Definition).Readonly();
            View.Property(p => p.Value).Readonly();
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
        }
    }
}
