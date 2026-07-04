using System.Collections.Generic;

namespace SIE.EventMessages.LES
{
    /// <summary>
    /// 发运单发货更新备料单信息
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultILesShippingOrder))]
    public interface ILesShippingOrder
    {
        /// <summary>
        /// 创建发运单信息
        /// </summary>
        /// <param name="createShippingOrderDatas"></param>
        void CreateShippingOrder(List<CreateShippingOrderData> createShippingOrderDatas);

        /// <summary>
        /// 更新发运单信息（来源是备料单需求的才能做更新）
        /// </summary>
        /// <param name="updateShippingOrderDatas"></param>
        void UpdateShippingOrder(List<UpdateShippingOrderData> updateShippingOrderDatas);

        /// <summary>
        /// 获取可接收的发运单
        /// </summary>
        /// <param name="shipDataParam"></param>
        /// <returns></returns>
        List<ShippingOrderData> GetShipData(QueryShipDataParam shipDataParam);
        /// <summary>
        /// 获取可接收的发运单明细
        /// </summary>
        /// <param name="shipDataParam"></param>
        /// <returns></returns>
        List<ShippingOrderAssignData> GetShipDetailData(QueryShipDetailDataParam shipDataParam);

        /// <summary>
        /// 获取可接收的发运单序列号
        /// </summary>
        /// <param name="shipDataParam"></param>
        /// <returns></returns>
        List<ShippingOrderAssignSnData> GetShipSnData(QueryShipDetailDataParambase shipDataParam);

        /// <summary>
        /// LES接收发运单数据
        /// </summary>
        void ReceiveShipData(List<ReceiveShipDataParam> receiveShipDataParam);

        /// <summary>
        /// 接收扫描标签
        /// </summary>
        /// <param name="invOrgId"></param>
        /// <param name="barcode"></param>
        /// <returns></returns>
        List<ReciveWmsLabelData> ReciveWmsLabels(int invOrgId, string barcode);

        /// <summary>
        /// 扫描识别标签
        /// </summary>
        /// <param name="invOrgId"></param>
        /// <param name="barcode"></param>
        /// <returns></returns>
        List<ScanLabelData> ScanLabel(int invOrgId, string barcode);        
    }

    /// <summary>
    /// 发运单发货更新备料单信息
    /// </summary>
    class DefaultILesShippingOrder : ILesShippingOrder
    {
       
        /// <summary>
        /// 创建发运单信息
        /// </summary>
        /// <param name="createShippingOrderDatas"></param>
        public void CreateShippingOrder(List<CreateShippingOrderData> createShippingOrderDatas)
        {            
        }

        /// <summary>
        /// 获取可接收的发运单明细
        /// </summary>
        /// <param name="shipDataParam"></param>
        /// <returns></returns>
        public List<ShippingOrderData> GetShipData(QueryShipDataParam shipDataParam)
        {
            return new List<ShippingOrderData>();
        }

        /// <summary>
        /// 获取可接收的发运单明细
        /// </summary>
        /// <param name="shipDataParam"></param>
        /// <returns></returns>
        public List<ShippingOrderAssignData> GetShipDetailData(QueryShipDetailDataParam shipDataParam)
        {
            return new List<ShippingOrderAssignData>();
        }

        /// <summary>
        /// 获取可接收的发运单序列号
        /// </summary>
        /// <param name="shipDataParam"></param>
        /// <returns></returns>
        public List<ShippingOrderAssignSnData> GetShipSnData(QueryShipDetailDataParambase shipDataParam)
        {
            return new List<ShippingOrderAssignSnData>();
        }

        /// <summary>
        /// LES接收发运单数据
        /// </summary>
        public void ReceiveShipData(List<ReceiveShipDataParam> receiveShipDataParam)
        {
           
        }

        /// <summary>
        /// 接收扫描标签
        /// </summary>
        /// <param name="invOrgId"></param>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public List<ReciveWmsLabelData> ReciveWmsLabels(int invOrgId, string barcode)
        {
            return new List<ReciveWmsLabelData>();
        }

        /// <summary>
        /// 扫描识别标签
        /// </summary>
        /// <param name="invOrgId"></param>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public List<ScanLabelData> ScanLabel(int invOrgId, string barcode)
        {
            return new List<ScanLabelData>();
        }
        
        /// <summary>
        /// 更新发运单信息（来源是备料单需求的才能做更新）
        /// </summary>
        public void UpdateShippingOrder(List<UpdateShippingOrderData> updateShippingOrderDatas)
        {            
        }
    }
}
