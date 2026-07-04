using SIE.Items;
using SIE.ProductIntfc.ProductStorages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.ProductIntfc.ProductStorages
{
    public class ProductStorageParamCriteriaViewConfig : WebViewConfig<ProductStorageParamCriteria>
    {
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Item).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<ItemController>().GetItems(keyword, pagingInfo);
            }).HasLabel("产品编码");
            View.Property(p => p.ItemType).UseEnumEditor().HasLabel("入库类型");
        }
    }
}
