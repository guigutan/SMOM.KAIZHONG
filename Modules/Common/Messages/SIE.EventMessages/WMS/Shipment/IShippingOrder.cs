using NPOI.POIFS.Storage;
using SIE.EventMessages.WMS.Shipment;
using System.Collections.Generic;

namespace SIE.EventMessages.Shipment
{
    /// <summary>
    /// 获取发运单数据
    /// </summary>
    [Services.Service(FallbackType = typeof(DefalitIShippingOrderInterface))]
    public interface IShippingOrder
    {
        /// <summary>
        /// 获取发运单数据
        /// </summary>
        /// <param name="keywork">查询关键字</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="type">类型1 生产退料 2 委外退料 3销售退货</param>
        /// <returns>发运单数据</returns>
        List<SoData> GetShippingOrderDatas(string keywork, PagingInfo pagingInfo, int type);

        /// <summary>
        /// 获取发运单明细行数据
        /// </summary>
        /// <param name="getSoDetailParams">获取发运单明细数据参数</param>      
        /// <returns>发运单明细行数据</returns>
        List<SoDetailData> GetShippingOrderDetailDatas(GetSoDetailParams getSoDetailParams);

        /// <summary>
        /// 获取发运单明细数据
        /// </summary>
        /// <param name="itemIds">物料id</param>     
        /// <param name="ShippingOrderNos">发运单号</param>
        /// <returns>明细数据</returns>
        List<SoDetailData> GetSoDetailDatas(List<string> ShippingOrderNos, List<double> itemIds);

        /// <summary>
        /// 获取发运单明细数据
        /// </summary>
        /// <param name="orderNos">相关单号集合</param>
        /// <param name="orderLineNos">相关方法集合</param>
        /// <returns>明细数据</returns>
        bool GetIsHasOrderNoData(List<string> orderNos, List<string> orderLineNos);

        /// <summary>
        /// 分配发运单
        /// </summary>
        /// <param name="soIds">发运单</param>
        /// <param name="soDtlIds">发运单明细Id</param>
        /// <param name="onhandIds">库存Id</param>
        /// <param name="snNos">ASN序列号</param>     
        void AssignShippingOrderFromAsn(List<double> soIds, List<double> soDtlIds, List<double> onhandIds, List<string> snNos);

        /// <summary>
        /// 确认分配数据
        /// </summary>
        /// <param name="soIds">发运单集合</param>
        /// <param name="soDtlIds">发运单明细Id集合</param>       
        void ConfirmAssignShippingOrderDetail(List<double> soIds, List<double> soDtlIds);

        /// <summary>
        /// 拣货发运单明细
        /// </summary>
        /// <param name="shippingOrderDetailIdList">发运单明细Id</param>
        /// <returns>返回错误消息</returns>
        void PickingShippingOrderDetail(List<double> shippingOrderDetailIdList);

        /// <summary>
        /// 拣货发运单
        /// </summary>
        /// <param name="soIds">发运单Id</param>
        /// <returns>返回错误消息</returns>
        void PickingShippingOrders(List<double> soIds);

        /// <summary>
        /// 删除的发运单分配明细
        /// </summary>
        /// <param name="soDetails">发运单明细</param>
        void DeleteAssignFromSoDetailId(List<double> soDetails);

        /// <summary>
        /// 发货计划创建发运订单
        /// </summary>
        /// <param name="soParams">发运订单参数集合</param>
        List<double> CreateSoByDeliveryPlan(List<SoParam> soParams);

        /// <summary>
        /// 审核发运单数据
        /// </summary>
        /// <param name="soIds">发运单ID集合</param>
        void AuditShippingOrderData(List<double> soIds);

        /// <summary>
        /// ASN明细创建供应商退货发运单
        /// </summary>
        /// <param name="returnSoParam">退货发运订单参数集合</param>
        List<double> CreateReturnSoByAsn(ReturnSoParam returnSoParam);

        /// <summary>
        /// 根据相关单号行号查询发运单
        /// </summary>
        /// <param name="orderNo">相关单号</param>
        /// <param name="orderLineNos">相关行号</param>
        /// <returns>发运单</returns>
        bool CheckHasSoByOrderNo(string orderNo, List<string> orderLineNos);

        /// <summary>
        /// 获取发运单数据
        /// </summary>
        /// <param name="soNos">发运单号集合</param>
        /// <returns>发运单数据</returns>
        List<SoData> GetSoDatas(List<string> soNos);

        /// <summary>
        /// 保存虚拟出库信息
        /// </summary>
        /// <param name="outSoParam">入库信息</param>
        string SaveQuickDeliveryData(OutSoParam outSoParam);

        /// <summary>
        /// 更新发运单已接收字段
        /// </summary>
        /// <param name="updateSos"></param>
        void UpdateSoRecive(List<UpdateSoReciveData> updateSos);

        /// <summary>
        /// 获取离线单据物料
        /// </summary>
        /// <param name="whId">仓库</param>
        /// <returns></returns>
        List<double> GetOutlineOrderItems(double whId);

        /// <summary>
        /// 创建耗料单
        /// </summary>
        /// <param name="outUseBillDatas">耗料单数据</param>
        List<ReturnUseBillData> CreateOutUseShipmentBill(List<OutUseBillData> outUseBillDatas);

        /// <summary>
        /// 取消耗料单
        /// </summary>
        /// <param name="soNos"></param>
        void CancelShippingOrder(List<string> soNos);
        
    }

    /// <summary>
    /// 获取发运单数据
    /// </summary>
    class DefalitIShippingOrderInterface : IShippingOrder
    {
        /// <summary>
        /// 获取发运单数据
        /// </summary>
        /// <param name="keywork">查询关键字</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="type">类型1 生产退料 2 委外退料 3销售退货</param>
        /// <returns>发运单数据</returns>
        public List<SoData> GetShippingOrderDatas(string keywork, PagingInfo pagingInfo, int type)
        {
            return new List<SoData>();
        }

        /// <summary>
        /// 获取发运单明细行数据
        /// </summary>
        /// <param name="getSoDetailParams">获取发运单明细数据参数</param>      
        /// <returns>发运单明细行数据</returns>
        public List<SoDetailData> GetShippingOrderDetailDatas(GetSoDetailParams getSoDetailParams)
        {
            return new List<SoDetailData>();
        }

        /// <summary>
        /// 获取发运单明细数据
        /// </summary>
        /// <param name="itemIds">物料id</param>      
        /// <param name="ShippingOrderNos">发运单号</param>
        /// <returns>明细数据</returns>
        public List<SoDetailData> GetSoDetailDatas(List<string> ShippingOrderNos, List<double> itemIds)
        {
            return new List<SoDetailData>();
        }

        /// <summary>
        /// 获取发运单明细数据
        /// </summary>
        /// <param name="orderNos">相关单号集合</param>
        /// <param name="orderLineNos">相关方法集合</param>
        /// <returns>明细数据</returns>
        public bool GetIsHasOrderNoData(List<string> orderNos, List<string> orderLineNos)
        {
            return false;
        }

        /// <summary>
        /// 分配发运单指定库存
        /// </summary>
        /// <param name="soIds">发运单</param>
        /// <param name="soDtlIds">发运单明细Id</param>
        /// <param name="onhandIds">库存Id</param>
        /// <param name="snNos">ASN序列号</param>      
        public void AssignShippingOrderFromAsn(List<double> soIds, List<double> soDtlIds, List<double> onhandIds, List<string> snNos)
        {
            //无
        }

        /// <summary>
        /// 确认分配数据
        /// </summary>
        /// <param name="soIds">发运单集合</param>
        /// <param name="soDtlIds">发运单明细Id集合</param>    
        public void ConfirmAssignShippingOrderDetail(List<double> soIds, List<double> soDtlIds)
        {
            //无
        }

        /// <summary>
        /// 拣货发运单明细
        /// </summary>
        /// <param name="shippingOrderDetailIdList">发运单明细Id</param>
        /// <returns>返回错误消息</returns>
        public void PickingShippingOrderDetail(List<double> shippingOrderDetailIdList)
        {
            //无
        }

        /// <summary>
        /// 删除的发运单分配明细
        /// </summary>
        /// <param name="soDetails">发运单明细</param>
        public void DeleteAssignFromSoDetailId(List<double> soDetails)
        {
            //无
        }

        /// <summary>
        /// 根据发货计划创建发运单
        /// </summary>
        /// <param name="soParams"></param>
        public List<double> CreateSoByDeliveryPlan(List<SoParam> soParams)
        {
            return new List<double>();
        }

        /// <summary>
        /// 审核发运单数据
        /// </summary>
        /// <param name="soIds">发运单ID集合</param>
        public void AuditShippingOrderData(List<double> soIds)
        {
            //无
        }

        /// <summary>
        /// ASN明细创建供应商退货发运单
        /// </summary>
        /// <param name="returnSoParam">退货发运订单参数集合</param>
        public List<double> CreateReturnSoByAsn(ReturnSoParam returnSoParam)
        {
            return new List<double>();
        }

        /// <summary>
        /// 根据相关单号行号查询发运单
        /// </summary>
        /// <param name="orderNo">相关单号</param>
        /// <param name="orderLineNos">相关行号</param>
        /// <returns>发运单</returns>
        public bool CheckHasSoByOrderNo(string orderNo, List<string> orderLineNos)
        {
            return false;
        }

        /// <summary>
        /// 获取发运单数据
        /// </summary>
        /// <param name="soNos">发运单号集合</param>
        /// <returns>发运单数据</returns>
        public List<SoData> GetSoDatas(List<string> soNos)
        {
            return new List<SoData>();
        }

        /// <summary>
        /// 保存虚拟出库信息
        /// </summary>
        /// <param name="outSoParam">入库信息</param>
        public string SaveQuickDeliveryData(OutSoParam outSoParam)
        {
            return string.Empty;
        }

        public void UpdateSoRecive(List<UpdateSoReciveData> updateSos)
        {
            //无
        }

        public void PickingShippingOrders(List<double> soIds)
        {
            //无
        }

        public List<ReturnUseBillData> CreateOutUseShipmentBill(List<OutUseBillData> outUseBillDatas)
        {
            throw new System.NotImplementedException();
        }

        public List<double> GetOutlineOrderItems(double whId)
        {
            throw new System.NotImplementedException();
        }

        public void CancelShippingOrder(List<string> soNos)
        {

        }
    }
}
