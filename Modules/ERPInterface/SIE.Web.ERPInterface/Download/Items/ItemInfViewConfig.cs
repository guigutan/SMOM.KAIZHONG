using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.MetaModel.View;

namespace SIE.Web.ERPInterface.Download.Items
{
    /// <summary>
    /// 物料中间表视图配置
    /// </summary>
    internal class ItemInfViewConfig : WebViewConfig<ItemInf>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Description);
            View.Property(p => p.DrawingNo);
            View.Property(p => p.Version);
            View.Property(p => p.BaseModel);
            View.Property(p => p.Person);
            View.Property(p => p.MrpPerson);
            View.Property(p => p.PurchasingAgent);
            View.Property(p => p.UpperWeight);
            View.Property(p => p.LowerWeight);
            View.Property(p => p.MinPackingQty);
            View.Property(p => p.SpecificationModel);
            View.Property(p => p.EnglishDescription);
            View.Property(p => p.ShortDescription);
            View.Property(p => p.Length);
            View.Property(p => p.Width);
            View.Property(p => p.Height);
            View.Property(p => p.Volume);
            View.Property(p => p.Weight);
            View.Property(p => p.PurchaseLeadTime);
            View.Property(p => p.Precision);
            View.Property(p => p.GoodsBarcode);
            View.Property(p => p.ItemCategoryCode);
            View.Property(p => p.Unit);
            View.Property(p => p.ConsumeMode);
            View.Property(p => p.ItemType);
            View.Property(p => p.ItemSourceType);
        }

        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name);
                View.Property(p => p.PurchaseLeadTime);
                View.Property(p => p.ItemCategoryCode);
                View.Property(p => p.ConsumeMode);
                View.Property(p => p.ItemType);
                View.Property(p => p.ItemSourceType);
            }
        }
    }
}