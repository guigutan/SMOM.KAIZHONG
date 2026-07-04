using SIE.EMS.Purchases.EquipmentInbounds;
using SIE.Web.Data;

namespace SIE.Web.EMS.Purchases.EquipmentInbounds
{
    /// <summary>
    /// 设备入库查询器
    /// </summary>
    public class EquipmentInboundDataQueryer : DataQueryer
    {
        /// <summary>
        /// 批量选择库位
        /// </summary>
        /// <param name="locationId">库位id</param>
        /// <param name="inboundId">入库id</param>
        public void SelectLocation(double locationId, double inboundId)
        {
            RT.Service.Resolve<EquipmentInboundController>().SelectLocation(locationId, inboundId);
        }
    }
}
