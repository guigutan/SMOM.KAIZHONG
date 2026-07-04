using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ProductAgingProcesss
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ProductAgingProcessController:DomainController
    {
        /// <summary>
        /// 获取产品老化工艺时间
        /// </summary>
        /// <param name="productIds"></param>
        /// <returns></returns>
        public virtual EntityList<ProductAgingProcess> GetProductAgingProcessesByProductId(List<double> productIds)
        {
            var itemIds = productIds.ConvertAll(p => (double?)p).ToList();
            return itemIds.SplitContains(ids =>
            {
                return Query<ProductAgingProcess>().Where(p => ids.Contains(p.ItemId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }
    }
}
