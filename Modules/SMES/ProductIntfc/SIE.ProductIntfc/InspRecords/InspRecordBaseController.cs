using SIE.Domain;
using SIE.EventMessages.Inspection;
using SIE.ProductIntfc.InspLogs;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ProductIntfc.InspRecords
{
    /// <summary>
    /// 报检记录控制器-用于扩展继承重写
    /// </summary>
    public class InspRecordBaseController : DomainController
    {
        /// <summary>
        /// 设置报检条码（用于扩展使用）
        /// </summary>
        /// <param name="barcode">报检条码</param>
        /// <param name="inspEvent">成品报检事件</param>
        public virtual void SetInspBarcode(InspBarcode barcode, ProductInspEvent inspEvent)
        {
        }

        /// <summary>
        /// 获取报检条码列表（用于扩展使用）
        /// </summary>
        /// <param name="inspBarcodes">报检条码列表</param>
        /// <returns>报检条码列表</returns>
        public virtual List<InspBarcode> GetInspBarcodeList(EntityList<InspBarcode> inspBarcodes)
        {
            return inspBarcodes.ToList();
        }

        /// <summary>
        /// 设置报检条码日志明细（用于扩展使用）
        /// </summary>
        /// <param name="barcodeLog">报检条码日志明细</param>
        /// <param name="e">报检条码</param>
        public virtual void SetInspBarcodeLog(InspBarcodeLog barcodeLog, InspBarcode e)
        {
        }

        /// <summary>
        /// 更新首检单信息（用于扩展使用）
        /// </summary>
        /// <param name="inspLog">报检日志</param>
        public virtual void UpdateFirstAuditResultExt(InspLog inspLog)
        {
        }

        /// <summary>
        /// 设置首件报检参数（用于扩展使用）
        /// </summary>
        /// <param name="inspLogId">报检日志id</param>
        /// <param name="firstInspBillEvent">首件报检参数</param>
        public virtual void SetFirstInspBillEvent(double inspLogId, EventMessages.Inspection.FirstInspBillEvent firstInspBillEvent)
        {
        }
    }
}
