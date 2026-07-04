using SIE.Domain;
using SIE.Items;
using SIE.ManagedProperty;
using System.Collections.Generic;

namespace SIE.Web.Items
{
    /// <summary>
    /// 产品族视图配置
    /// </summary>
    [CompiledPropertyDeclarer]
    internal class ProductFamilyViewConfig : WebViewConfig<ProductFamily>
    {
        /// <summary>
        /// 产品族分类命令
        /// </summary>
        const string ProductFamilyCommand = "SIE.Web.Items.ProductFamilys.Commands.ProductFamilyCommand";

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit().UseDefaultCommands().UseCommands(ProductFamilyCommand);
            View.Property(p => p.Code).ShowInList(150).Readonly(p => p.PersistenceStatus != PersistenceStatus.New)
                .UseListSetting(e => { e.HelpInfo = "新增状态可编辑"; });
            View.Property(p => p.Name).ShowInList(150);
            View.Property(p => p.Category).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.CategoryCode), nameof(e.Category.Code));
                m.DicLinkField = keyValues;
            }).ShowInList(150);
            View.Property(p => p.CategoryCode).HasLabel("族分类维护*").ShowInList(150);
            View.Property(p => p.CreateByName);
            View.Property(p => p.CreateDate).ShowInList(150);
            View.Property(p => p.UpdateByName);
            View.Property(p => p.UpdateDate).ShowInList(150);
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
            }, ProductModelViewConfig.ModelWithFamilyView).HasLabel("产品机型");
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
