using System;
using System.Collections.Generic;

namespace SIE.EventMessages
{
    /// <summary>
    /// 获取ASN数据
    /// </summary>
    [Services.Service(FallbackType = typeof(DefalitIReceiptInterface))]
    public interface IReceipt
    {
        /// <summary>
        /// 获取ASN数据给发货计划齐套分析
        /// </summary>
        /// <param name="itemIds">物料</param>
        /// <param name="warehouseIds">仓库</param>
        /// <param name="lastDeliveryDate">最后交货日期</param>
        /// <param name="storeCode">项目号</param>
        /// <param name="projectNo">任务号</param>
        /// <param name="taskNo">任务号</param>
        /// <returns>发货计划分析库存数据</returns>
        List<AnalysOnhandData> GetReceiptAnalysOnhandDatas(List<double> itemIds, List<double> warehouseIds, DateTime? lastDeliveryDate, string storeCode, string projectNo, string taskNo);

        /// <summary>
        /// 获取ASN关联了工单的明细
        /// </summary>
        /// <param name="wos">工单</param>
        /// <returns>ASN关联工单的明细</returns>
        List<AsnWoData> GetAsnWoDatas(List<string> wos);

        /// <summary>
        /// 验证ASN单号状态
        /// </summary>
        /// <param name="asnDtlIds">ASN明细状态</param>
        void CheckAsnState(List<double> asnDtlIds);

        /// <summary>
        /// 处理ASN单的退货数
        /// </summary>
        /// <param name="soReurnDatas">供应商退货单数据</param>
        /// <param name="type">0-发货 1-取消发货 2-关闭</param>
        void HandleAsnReturnQty(Dictionary<double, decimal> dicDelivery);

        /// <summary>
        /// 创建转仓入库单
        /// </summary>
        /// <param name="paramDatas">参数数据</param>       

        List<AllotAsnData> CreateOtherInAsnData(List<OtherInAsnDtlParamData> paramDatas);


        /// <summary>
        /// 创建委外退料入库单
        /// </summary>
        /// <param name="paramDatas">参数数据</param>       

        List<AllotAsnData> CreateOutRetAsnData(List<OutRetAsnParam> paramDatas);

        /// <summary>
        /// 关闭ASN单
        /// </summary>
        /// <param name="asnDtlIds">ASN明细</param>
        /// <param name="reason">关闭原因</param>
        void CloseAsnDetails(List<double> asnDtlIds, string reason);

        /// <summary>
        /// 关闭ASN单（根据不同的业务扩展type的值）
        /// </summary>
        /// <param name="asnIds">ASN</param>
        /// <param name="reason">关闭原因</param>
        /// <param name="type">类型 0 供应商退货关闭委外退料单</param>
        /// <remarks>根据不同的业务扩展type的值</remarks>
        void CloseAsns(List<double> asnIds, string reason, int type);

        /// <summary>
        /// 获取ASN单数据
        /// </summary>
        /// <param name="supplierId">供应商ID</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>ASN单数据</returns>
        List<AsnData> GetAsnDatas(double supplierId, string keyword, PagingInfo pagingInfo);

        /// <summary>
        /// 获取ASN单收货明细数据
        /// </summary>
        /// <param name="asnNo">ASN单号</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>收货明细数据</returns>
        List<AsnDetailData> GetAsnDetailDatas(string asnNo, string keyword, PagingInfo pagingInfo);

        /// <summary>
        /// 获取ASN单收货明细数据
        /// </summary>
        /// <param name="supplierId">供应商ID</param>
        /// <param name="itemCode">物料编码</param>
        /// <param name="itemName">物料名称</param>
        /// <param name="poNo">采购订单号</param>
        /// <param name="asnNo">ASN单号</param>
        /// <param name="lotNo">批次号</param>
        /// <param name="pagingInfo">分页信息</param>
        List<AsnDetailData> GetAsnDtlBySupplierIds(double supplierId, string itemCode, string itemName, string poNo, string asnNo, string lotNo, PagingInfo pagingInfo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asnNos"></param>
        /// <returns></returns>
        List<AsnData> GetAsnDataByAsnNos(List<string> asnNos);

        /// <summary>
        /// 获取离线单据物料
        /// </summary>
        /// <param name="whId">仓库</param>
        /// <returns></returns>
        List<OutLineAsnDtlDataSimple> GetOutlineOrderItems(double whId);

        /// <summary>
        /// 根据ASN明细获取采购订单数量
        /// </summary>
        /// <param name="asnDtlId">收货明细Id</param>
        /// <param name="isMatFail">是否来料不良</param>
        /// <returns></returns>
        PoDtlData GetPoDtlData(double asnDtlId, bool isMatFail);

        /// <summary>
        /// 根据ASN明细获取明细数据
        /// </summary>
        /// <param name="asnDtlIds">收货明细Id</param>
        /// <param name="isMatFail">是否来料退</param>
        /// <returns></returns>
        List<AsnDtlData> GetAsnDtlData(List<double> asnDtlIds, bool isMatFail);

    }

    /// <summary>
    /// 获取ASN数据
    /// </summary>
    class DefalitIReceiptInterface : IReceipt
    {
        /// <summary>
        /// 获取ASN关联了工单的明细
        /// </summary>
        /// <param name="wos">工单</param>
        /// <returns>ASN关联工单的明细</returns>
        public List<AsnWoData> GetAsnWoDatas(List<string> wos)
        {
            return new List<AsnWoData>();
        }

        /// <summary>
        /// 获取ASN数据给发货计划齐套分析
        /// </summary>
        /// <param name="itemIds">物料</param>
        /// <param name="warehouseIds">仓库</param>
        /// <param name="lastDeliveryDate">最后交货日期</param>
        /// <param name="storeCode">货主</param>
        /// <param name="projectNo">项目号</param>
        /// <param name="taskNo">任务号</param>
        /// <returns>发货计划分析库存数据</returns>
        public List<AnalysOnhandData> GetReceiptAnalysOnhandDatas(List<double> itemIds, List<double> warehouseIds, DateTime? lastDeliveryDate, string storeCode, string projectNo, string taskNo)
        {
            return new List<AnalysOnhandData>();
        }

        /// <summary>
        /// 验证ASN单号状态
        /// </summary>
        /// <param name="asnDtlIds">ASN明细ID</param>
        public void CheckAsnState(List<double> asnDtlIds)
        {
            //
        }

        /// <summary>
        /// 处理ASN单的退货数
        /// </summary>
        /// <param name="soReurnDatas">供应商退货单数据</param>
        /// <param name="type">0-发货 1-取消发货 2-关闭</param>
        public void HandleAsnReturnQty(Dictionary<double, decimal> dicDelivery)
        {
            //
        }

        /// <summary>
        /// 创建其他入库单
        /// </summary>
        /// <param name="paramDatas">参数数据</param>        
        public List<AllotAsnData> CreateOtherInAsnData(List<OtherInAsnDtlParamData> paramDatas)
        {
            return new List<AllotAsnData>();
        }

        /// <summary>
        /// 关闭ASN单
        /// </summary>
        /// <param name="asnDtlIds">ASN明细</param>
        /// <param name="reason">关闭原因</param>
        public void CloseAsnDetails(List<double> asnDtlIds, string reason)
        {
            //
        }

        /// <summary>
        /// 获取ASN单数据
        /// </summary>
        /// <param name="supplierId">供应商ID</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>ASN单数据</returns>
        public List<AsnData> GetAsnDatas(double supplierId, string keyword, PagingInfo pagingInfo)
        {
            return new List<AsnData>();
        }

        /// <summary>
        /// 获取ASN单收货明细数据
        /// </summary>
        /// <param name="asnNo">ASN单号</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>收货明细数据</returns>
        public List<AsnDetailData> GetAsnDetailDatas(string asnNo, string keyword, PagingInfo pagingInfo)
        {
            return new List<AsnDetailData>();
        }

        /// <summary>
        /// 获取ASN单收货明细数据
        /// </summary>
        /// <param name="supplierId">供应商ID</param>
        /// <param name="itemCode">物料编码</param>
        /// <param name="itemName">物料名称</param>
        /// <param name="poNo">采购订单号</param>
        /// <param name="asnNo">ASN单号</param>
        /// <param name="lotNo">批次号</param>
        /// <param name="pagingInfo">分页信息</param>
        public List<AsnDetailData> GetAsnDtlBySupplierIds(double supplierId, string itemCode, string itemName, string poNo, string asnNo, string lotNo, PagingInfo pagingInfo)
        {
            return new List<AsnDetailData>();
        }

        /// <summary>
        /// 获取ASN单数据
        /// </summary>
        /// <param name="asnNos">ASN单号集合</param>
        /// <returns>ASN单数据</returns>
        public List<AsnData> GetAsnDataByAsnNos(List<string> asnNos)
        {
            return new List<AsnData>();
        }

        /// <summary>
        /// 获取离线单据物料
        /// </summary>
        /// <param name="whId">仓库</param>
        /// <returns></returns>
        public List<OutLineAsnDtlDataSimple> GetOutlineOrderItems(double whId)
        {
            return new List<OutLineAsnDtlDataSimple>();
        }

        /// <summary>
        /// 获取采购订单明细数据
        /// </summary>
        /// <param name="asnDtlId"></param>
        /// <param name="isMatFail"></param>
        /// <returns></returns>        
        public PoDtlData GetPoDtlData(double asnDtlId, bool isMatFail)
        {
            return new PoDtlData();
        }

        /// <summary>
        /// 根据ASN明细获取采购订单数量
        /// </summary>
        /// <param name="asnDtlIds">收货明细Id</param>
        /// <param name="isMatFail">是否来料退</param>
        /// <returns></returns>
        public List<AsnDtlData> GetAsnDtlData(List<double> asnDtlIds, bool isMatFail)
        {
            return new List<AsnDtlData>();
        }

        /// <summary>
        /// 创建委外退料单
        /// </summary>
        /// <param name="paramDatas"></param>
        /// <returns></returns>    
        public List<AllotAsnData> CreateOutRetAsnData(List<OutRetAsnParam> paramDatas)
        {
            return new List<AllotAsnData>();
        }

        /// <summary>
        /// 发运单ASN单
        /// </summary>
        /// <param name="asnIds"></param>
        /// <param name="reason"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void CloseAsns(List<double> asnIds, string reason, int type)
        {
            ////无
        }
    }
}


