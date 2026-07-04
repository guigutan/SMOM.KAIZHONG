using SIE.Common;
using SIE.Services;
using System;
using System.Collections.Generic;

namespace SIE.EventMessages.Inspection
{
    /// <summary>
    /// 首检报检接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultToStorageBarcode))]
    public interface IToStorageBarcode
    {
        /// <summary>
        /// 成品入库
        /// </summary>
        /// <param name="toStorageEvent">成品入库事件</param>
        void ToStorageBarcode(ToStorageBarcodeEvent toStorageEvent);

        /// <summary>
        /// 检查包装条码是否已经入库
        /// </summary>
        /// <param name="barcode">包装条码</param>
        /// <param name="isBatch">是否批次</param>
        bool IsExistsPakStorageDetail(string barcode, bool isBatch);

        /// <summary>
        /// 检查单个产品条码是否已经入库
        /// </summary>
        /// <param name="barcode">单个产品条码</param>
        bool IsExistsStorageDetailByBarcode(string barcode);

        /// <summary>
        /// 移除入库数据
        /// </summary>
        /// <param name="batchPackRelationId">包装关系Id</param>
        /// <param name="isBatch">是否批次</param>       
        void MoveStorageDetailByPackcode(double batchPackRelationId, bool isBatch);

        /// <summary>
        /// 加入入库数据
        /// </summary>
        /// <param name="batchPackRelationId">包装关系Id</param>
        /// <param name="isBatch">是否批次</param>
        /// <param name="ProductBarcode">产品条码</param>
        void JoinStorageDetailByPackcode(double batchPackRelationId, bool isBatch, string ProductBarcode);

        /// <summary>
        /// 删除单体入库条码明细
        /// </summary>
        /// <param name="productBarcode">单体产品条码</param>
        void DeleteToStoreDetailByCode(string productBarcode);

        /// <summary>
        /// 获取工单入库仓库
        /// </summary>
        /// <returns></returns>
        double GetWoWarehouse();
    }

    /// <summary>
    /// 首检报检接口默认实现
    /// </summary>
    public class DefaultToStorageBarcode : IToStorageBarcode
    {
        /// <summary>
        /// 成品入库
        /// </summary>
        /// <param name="toStorageEvent">成品入库事件</param>
        public void ToStorageBarcode(ToStorageBarcodeEvent toStorageEvent) 
        {
            // 成品入库
        }

        /// <summary>
        /// 检查包装条码是否已经入库
        /// </summary>
        /// <param name="barcode">包装条码</param>
        /// <param name="isBatch">是否批次</param>
        public bool IsExistsPakStorageDetail(string barcode, bool isBatch)
        {
            return false;
        }

        /// <summary>
        /// 检查单个产品条码是否已经入库
        /// </summary>
        /// <param name="barcode">单个产品条码</param>
        public bool IsExistsStorageDetailByBarcode(string barcode)
        {
            return false;
        }

        /// <summary>
        /// 移除入库数据
        /// </summary>
        /// <param name="batchPackRelationId">包装关系Id</param>
        /// <param name="isBatch">是否批次</param>       
        public void MoveStorageDetailByPackcode(double batchPackRelationId, bool isBatch)
        {
            // 移除入库数据
        }

        /// <summary>
        /// 加入入库数据
        /// </summary>
        /// <param name="batchPackRelationId">包装关系Id</param>
        /// <param name="isBatch">是否批次</param>
        /// <param name="ProductBarcode">产品条码</param>
        public void JoinStorageDetailByPackcode(double batchPackRelationId, bool isBatch, string ProductBarcode) 
        {
            // 加入入库数据
        }

        /// <summary>
        /// 删除单体入库条码明细
        /// </summary>
        /// <param name="productBarcode">单体产品条码</param>
        public void DeleteToStoreDetailByCode(string productBarcode) 
        {
            // 删除单体入库条码明细
        }

        /// <summary>
        /// 获取工单入库仓库
        /// </summary>
        /// <returns></returns>
        public double GetWoWarehouse()
        {
            return 0;
        }
    }

    /// <summary>
    /// 成品入库事件
    /// </summary>
    [Serializable]
    public class ToStorageBarcodeEvent
    {
        /// <summary>
        /// 工单ID
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 入库条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime CollectionDate { get; set; }

        /// <summary>
        /// 条码数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 批次条码
        /// </summary>
        public string BatchBarcode { get; set; }

        /// <summary>
        /// 任务单ID
        /// </summary>
        public double? DispatchTaskId { get; set; }

        /// <summary>
        /// 报工记录ID
        /// </summary>
        public double? ReportRecordId { get; set; }

        /// <summary>
        /// 检验结果
        /// </summary>
        public int InspectionResult { get; set; }

        /// <summary>
        /// 检验状态
        /// </summary>
        public int InspectionStatus { get; set; }

        /// <summary>
        /// 处理方式
        /// </summary>
        public string ProcessMode { get; set; }

        /// <summary>
        /// 缺陷代码ID集合
        /// </summary>
        public List<double> DefectIds { get; set; }
    }
}