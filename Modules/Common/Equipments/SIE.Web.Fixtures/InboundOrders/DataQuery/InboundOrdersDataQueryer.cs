using SIE.Fixtures.InboundOrders;
using SIE.Warehouses;
using SIE.Web.Data;

namespace SIE.Web.Fixtures.InboundOrders.DataQuery
{
    /// <summary>
    /// 工治具台账查询器
    /// </summary>
    public class InboundOrdersDataQueryer : DataQueryer
    {
      
        /// <summary>
        /// 获取默认库位
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public StorageLocation GetDefaultLocation(double Id)
        {
            return RT.Service.Resolve<InboundOrderController>().GetDefaultLocation(Id);
        }
    }
}
