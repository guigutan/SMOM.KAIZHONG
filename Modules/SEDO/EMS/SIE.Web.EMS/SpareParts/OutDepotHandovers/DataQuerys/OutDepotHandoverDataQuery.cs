using SIE.Domain;
using SIE.EMS.SpareParts.OutDepotHandovers;
using System.Collections.Generic;

namespace SIE.Web.EMS.SpareParts.OutDepotHandovers.DataQuerys
{
    /// <summary>
    /// 备件交接单查询器
    /// </summary>
    public class OutDepotHandoverDataQuery : Data.DataQueryer
    {
        /// <summary>
        /// 交接单明细扫描查询
        /// </summary>
        /// <param name="barcode">扫描条码</param>
        /// <returns>查询信息</returns>
        public virtual OutDepotHandoverQueryInfo OutDepotHandoverBarcodeQuery(string barcode)
        {
            return RT.Service.Resolve<OutDepotHandoverController>().OutDepotHandoverBarcodeQuery(barcode);
        }
        /// <summary>
        /// 获取接收明细
        /// </summary>
        /// <param name="handoverId">交接单Id</param>
        /// <param name="sparePartId">备件Id</param>
        /// <returns>接收明细列表</returns>
        public virtual EntityList<OutDepotHandoverDetail> GetOutDepotHandoverDetails(double? handoverId, double? sparePartId)
        {
            return RT.Service.Resolve<OutDepotHandoverController>().GetOutDepotHandoverDetails(handoverId, sparePartId, new List<OrderInfo>(), null);
        }
    }
}
