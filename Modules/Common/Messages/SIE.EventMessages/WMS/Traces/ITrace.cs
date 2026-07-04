using System;
using System.Collections.Generic;

namespace SIE.EventMessages.WMS.Traces
{
    /// <summary>
    /// 查询物料追溯[TraceableItem]信息接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultTrace))]
    public interface ITrace
    {
        /// <summary>
        /// 库存物料信息列表
        /// </summary>
        /// <param name="criteria">查询条件</param>
        /// <returns></returns>
        WmsItemInfo GetWmsItemInfo(WmsItemCriteria criteria);

        /// <summary>
        /// 库存追溯-物料的库存信息列表
        /// </summary>
        /// <param name="criteria">查询条件</param>
        /// <returns></returns>
        WmsItemOnhandInfo GetWmsItemOnhandInfo(WmsItemOnhandCriteria criteria);

        /// <summary>
        /// 获取发运单相关追溯信息
        /// </summary>
        /// <param name="criteria">查询条件</param>
        /// <returns></returns>
        ShipmentTraceInfo GetWmsShippingTraceInfo(ShipmentTraceInfoCriteria criteria);

        /// <summary>
        /// 获取asn收货明细Id信息
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        List<double> GetAsnDetailIds(AsnDetailIdsCriteria criteria);

        /// <summary>
        /// 获取出货信息
        /// </summary>
        /// <param name="criteria">查询条件</param>
        /// <returns></returns>
        ShipmentInfo GetShipmentInfo(ShipmentInfoCriteria criteria);

        /// <summary>
        /// 获取包装信息的Wms信息
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        PackageWmsInfo GetPackageWmsInfo(PackageWmsInfoCriteria criteria);

        /// <summary>
        /// 获取Asn单信息
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        AsnInfo GetAsnInfo(AsnInfoCriteria criteria);
    }

    /// <summary>
    /// 查询TraceableItem信息接口的默认实现
    /// </summary>
    class DefaultTrace : ITrace
    {

        public PackageWmsInfo GetPackageWmsInfo(PackageWmsInfoCriteria criteriaInfo)
        {
            return new PackageWmsInfo();
        }

        public ShipmentInfo GetShipmentInfo(ShipmentInfoCriteria criteriaInfo)
        {
            return new ShipmentInfo();
        }

        public virtual List<double> GetAsnDetailIds(AsnDetailIdsCriteria criteriaInfo)
        {

            return new List<double>();
        }

        public WmsItemInfo GetWmsItemInfo(WmsItemCriteria criteria)
        {
            return new WmsItemInfo();
        }

        public WmsItemOnhandInfo GetWmsItemOnhandInfo(WmsItemOnhandCriteria criteria)
        {
            return new WmsItemOnhandInfo();
        }

        public AsnInfo GetAsnInfo(AsnInfoCriteria criteria)
        {
            return new AsnInfo();
        }

        public ShipmentTraceInfo GetWmsShippingTraceInfo(ShipmentTraceInfoCriteria criteria)
        {
            return new ShipmentTraceInfo();
        }
    }
}
