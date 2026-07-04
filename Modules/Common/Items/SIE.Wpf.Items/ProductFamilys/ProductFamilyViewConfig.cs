using SIE.Domain;
using SIE.Items;
using SIE.Wpf.Items.Commands;
using System.Collections.Generic;

namespace SIE.Wpf.Items
{
    /// <summary>
    /// 产品族分类 视图配置
    /// </summary>
    internal class ProductFamilyViewConfig : WPFViewConfig<ProductFamily>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit().UseDefaultCommands().UseCommands(typeof(ProductFamilyCommand));
            View.UseDefaultBehaviors();
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Category);
            View.Property(p => p.Category.Code).HasLabel("族分类编码");
            View.AttachChildrenProperty(typeof(ProductModel), w =>
            {
                var args = w as ChildPagingDataArgs;
                var productFamily = args.Parent as ProductFamily;
                var productModelList = new EntityList<ProductModel>();
                if (productFamily != null)
                {
                    var ctl = RT.Service.Resolve<ItemController>();
                    productModelList = ctl.GetProductModels(productFamily.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);
                }

                return productModelList;
            }, ProductModelViewConfig.FamilyCategoryView).HasLabel("产品机型");
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Category);
        }

        /// <summary>
        /// 选择视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }
    }
}