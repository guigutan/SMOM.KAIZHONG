using SIE.Domain;
using SIE.Items;
using SIE.MES.PrepareProducts;
using SIE.MES.PrepareProducts.Services;
using SIE.MetaModel.View;
using SIE.Web.MES.PrepareProducts.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.MES.PrepareProducts
{
    /// <summary>
    /// 产品产前准备设置视图配置
    /// </summary>
    public class PrepareProductViewConfig : WebViewConfig<PrepareProduct>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, "SIE.Web.MES.PrepareProducts.Commands.PrepareProductEditCommand", WebCommandNames.Delete, typeof(PrepareProductSaveCommand).FullName);
            View.UseCommands(typeof(PrepareProductImportCommand).FullName,typeof(PrepareProductExportCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.Product).UseDataSource((e, p, k) =>
                {
                    var prepareProduct = e as PrepareProduct;
                    if (prepareProduct != null)
                    {
                        return RT.Service.Resolve<PrepareProductService>().EditGetProduct(p, k);
                    }
                    return new EntityList<Item>();
                }).Readonly(p => p.ProductFamilyId != 0 && p.ProductFamilyId != null).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ProductName), nameof(e.Product.Name));
                    m.DicLinkField = keyValues;
                }).HasLabel("产品编码").ShowInList(width: 150);
                View.Property(p => p.ProductName).Readonly().ShowInList(width: 150);
                View.Property(p => p.ProductFamily).Readonly(p => p.ProductId != 0 && p.ProductId != null).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ProductFamilyName), nameof(e.ProductFamily.Name));
                    m.DicLinkField = keyValues;
                }).HasLabel("产品族编码").ShowInList(width: 150);
                View.Property(p => p.ProductFamilyName).Readonly().ShowInList(width: 150);
                //View.AttachChildrenProperty(typeof(PrepareProductDetail), (w) =>
                //{
                //    var args = w as ChildPagingDataArgs;
                //    var preProduct = args.Parent.CastTo<PrepareProduct>();
                //    if (preProduct == null)
                //    {
                //        return new EntityList<PrepareProductDetail>();
                //    }
                //    return RT.Service.Resolve<PrepareProductService>().GetPrepareProductDetailList(preProduct.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);
                //}, PrepareProductDetailViewConfig.AttachChildViewGroup).HasLabel("产前项目准备");
                View.ChildrenProperty(p => p.PrepareProjectDetailList).HasLabel("产前准备项目").Show(ChildShowInWhere.All);
            }
        }

        /// <summary>
        /// 导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.PropertyRef(p => p.Product.Code).BeforeImportRequireFunc("产品编码").HasLabel("产品编码");
            View.PropertyRef(p => p.ProductFamily.Code).BeforeImportRequireFunc("产品族编码").HasLabel("产品族编码");
        }
    }
}
