using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.ErpCommon.Datas
{
    /// <summary>
    /// 委外需求单收货创建报工事务上传
    /// </summary>
    [Serializable]
    public class ProcessingInStockReportTranData
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime TransactionDate { get; set; }

        public double? WoId { get; set; }

        public double? ItemId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string OrdKey { get; set; }

        public string WoNo { get; set; }

        public decimal Quantity { get; set; }

        public string WorkCenter { get; set; }

        public string WERKS { get; set; }

        public string Vornr { get; set; }

        public string ProcessCode { get; set; }

        public decimal? NgQty { get; set; }

        public decimal? OkQty { get; set; }

        public decimal? ReworkQty { get; set; }

        public decimal? SuspectQty { get; set; }

        public double? BillLineId { get; set; }

        public double? BillId { get; set; }

        public string BillNo { get; set; }
    }
}
