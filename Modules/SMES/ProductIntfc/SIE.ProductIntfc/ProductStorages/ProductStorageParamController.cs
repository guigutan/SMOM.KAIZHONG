using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ProductIntfc.ProductStorages
{
    public class ProductStorageParamController : DomainController
    {
        public virtual EntityList<ProductStorageParam> GetProductStorageParams(ProductStorageParamCriteria criteria) { 
            var q= Query<ProductStorageParam>();
            if (criteria.ItemId.HasValue) {
                q.Where(p => p.ItemId == criteria.ItemId);
            }
            if (criteria.ItemType.HasValue) {
                q.Where(p => p.Item.Type == criteria.ItemType);
            }
            return q.ToList(criteria.PagingInfo,new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}
