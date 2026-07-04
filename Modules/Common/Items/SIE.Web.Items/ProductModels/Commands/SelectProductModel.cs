using SIE.Items;
using SIE.Web.Command;
using System.Collections.Generic;

namespace SIE.Web.Items.ProductModels.Commands
{
    /// <summary>
    /// 选择产品机型
    /// </summary>
    public class SelectProductModel : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<SelectProductModelViewArgs>();
            var ctl = RT.Service.Resolve<ItemController>();
            var criteria = new ProductModelCriteria();
            ctl.AddRangeProductModel(data.ProductModelList, criteria, data.ProductsFamilyId);
            return true;
        }
    }

    /// <summary>
    /// 参数
    /// </summary>
    public class SelectProductModelViewArgs
    {
        /// <summary>
        /// 产品族Id
        /// </summary>
        /// <value>
        /// The products family identifier.
        /// </value>
        public double ProductsFamilyId { get; set; }
        /// <summary>
        /// 产品机型列表
        /// </summary>
        /// <value>
        /// The product model list.
        /// </value>
        public List<ProductModel> ProductModelList { get; set; }
    }
}
