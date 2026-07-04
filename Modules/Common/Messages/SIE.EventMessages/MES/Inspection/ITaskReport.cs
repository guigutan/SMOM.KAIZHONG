using DocumentFormat.OpenXml.Office2010.ExcelAc;
using SIE.EventMessages.Inspection;
using SIE.Services;
using System;
using System.Collections.Generic;

namespace SIE.EventMessages.MES.Inspection
{
    /// <summary>
    /// 任务报工接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultTaskReport))]
    public interface ITaskReport
    {
        /// <summary>
        /// 校验工序上工序是否已经报工完
        /// </summary>
        /// <param name="wipBatchId"></param>
        /// <param name="curProcessId"></param>
        void ValidationLastProcessReport(double wipBatchId, double curProcessId);

        /// <summary>
        /// 校验工序是否报工完成
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        void ValidationSeqTask(List<double> wipBatchIds);

        /// <summary>
        /// 成品报检入库（任务单）
        /// </summary>
        /// <param name="toStorageEvent">成品入库事件</param>
        void ToStorageTaskBarcode(ToStorageBarcodeEvent toStorageEvent);

        /// <summary>
        /// 检验不合格更新报工记录数量（任务单）
        /// </summary>
        /// <param name="toStorageEvent">成品入库事件</param>
        void ToUpdateTaskReportQty(ToStorageBarcodeEvent toStorageEvent);

        /// <summary>
        /// 检验合格更新报工记录信息（任务单）
        /// </summary>
        /// <param name="toStorageEvent">成品入库事件</param>
        void ToUpdateTaskReportState(ToStorageBarcodeEvent toStorageEvent);
    }

    /// <summary>
    /// 任务报工接口默认实现
    /// </summary>
    public class DefaultTaskReport : ITaskReport
    {

        /// <summary>
        /// 校验工序上工序是否已经报工完
        /// </summary>
        /// <param name="wipBatchId"></param>
        /// <param name="curProcessId"></param>
        public void ValidationLastProcessReport(double wipBatchId, double curProcessId)
        { 
        
        }

        /// <summary>
        /// 校验工序是否报工完成
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public void ValidationSeqTask(List<double> wipBatchIds)
        {

        }

        /// <summary>
        /// 成品报检入库（任务单）
        /// </summary>
        /// <param name="toStorageEvent">成品入库事件</param>
        public void ToStorageTaskBarcode(ToStorageBarcodeEvent toStorageEvent)
        {
            // 成品报检入库（任务单）
        }

        /// <summary>
        /// 检验不合格更新报工记录数量（任务单）
        /// </summary>
        /// <param name="toStorageEvent">成品入库事件</param>
        public void ToUpdateTaskReportQty(ToStorageBarcodeEvent toStorageEvent)
        {
            // 检验不合格更新报工记录数量（任务单）
        }

        /// <summary>
        /// 检验合格更新报工记录信息（任务单）
        /// </summary>
        /// <param name="toStorageEvent"></param>
        public void ToUpdateTaskReportState(ToStorageBarcodeEvent toStorageEvent)
        {

        }
    }
}
