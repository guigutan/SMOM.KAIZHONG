using SIE.Domain;
using SIE.Items;
using SIE.Web.Data;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Items.ProductModels
{
    /// <summary>
    /// 产品机型查询器
    /// </summary>
    public class ProductModelDataQuery : DataQueryer
    {
        /// <summary>
        /// 获取产品机型
        /// </summary>
        /// <returns>产品机型</returns>
        public EntityList<ProductModel> GetProductModels()
        {
            var productModelsList = RT.Service.Resolve<ItemController>().GetProductModels();
            return productModelsList;
        }

        /// <summary>
        /// 设置产品机型
        /// </summary>
        /// <param name="productModel">productModel</param>
        /// <returns>true</returns>
        public bool SetProductModel(List<SelectProductModel> productModel)
        {
            var ctl = RT.Service.Resolve<ItemController>();
            if (productModel.Count > 0)
            {
                var productModelList = ctl.GetProductModels(productModel.Select(p => p.ProductModelId).ToList()).ToList();
                ctl.SetProductModelFamily(productModelList, productModel.Select(p => p.ProductFamilyId).FirstOrDefault());
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>
    /// 参数
    /// </summary>
    public class SelectProductModel
    {
        /// <summary>
        /// 产品族Id
        /// </summary>
        /// <value>
        /// The products family identifier.
        /// </value>
        public double ProductFamilyId { get; set; }

        /// <summary>
        /// 产品机型Id
        /// </summary>
        /// <value>
        /// The product model list.
        /// </value>
        public double ProductModelId { get; set; }
    }
}
