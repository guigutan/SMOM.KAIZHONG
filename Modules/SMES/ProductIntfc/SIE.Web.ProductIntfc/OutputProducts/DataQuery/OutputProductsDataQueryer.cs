using SIE.MES.WorkOrders;
using SIE.ProductIntfc.OutputProducts;
using SIE.Warehouses;
using SIE.Web.Data;
using System.Collections.Generic;

namespace SIE.Web.ProductIntfc.OutputProducts.DataQuery
{
    public class OutputProductsDataQueryer : DataQueryer
    {

        /// <summary>
        /// 获取单据默认仓库
        /// </summary>
        /// <param name="outPutType"></param>
        /// <returns></returns>
        public Warehouse GetBillWh(OutputListType outPutType)
        {
            return RT.Service.Resolve<OutputProductController>().GetBillWh(outPutType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="woIds"></param>
        /// <returns></returns>
        public object GetOutputProductRecordViewModels(List<double> woIds)
        {
            return RT.Service.Resolve<OutputProductController>().GetOutputProductRecordViewModels(woIds);
        }
    }
}
