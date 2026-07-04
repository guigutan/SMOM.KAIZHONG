using SIE.Domain;
using SIE.Items;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.Items.ProductModels.Commands
{
    /// <summary>
    /// 删除
    /// </summary>
    [JsCommand("SIE.Web.Items.ProductModels.Commands.ProductModelDeleteCommand")]
    public class ProductModelDeleteCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            if (null == args.SelectedIds || args.SelectedIds.Length == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(args.SelectedIds)));
            }
            var ctl = RT.Service.Resolve<ItemController>();
            var selectedItems = new List<ProductModel>();
            foreach (var item in args.SelectedIds)
            {
                var productModel = RF.GetById<ProductModel>(item);
                selectedItems.Add(productModel);
            }
            var criteria = new ProductModelCriteria();
            ctl.RemoveProductModel(selectedItems, criteria);
            return true;
        }
    }
}
