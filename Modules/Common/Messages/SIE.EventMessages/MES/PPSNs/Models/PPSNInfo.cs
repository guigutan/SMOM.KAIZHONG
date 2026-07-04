using System;
using System.Collections.Generic;

namespace SIE.EventMessages.MES.PPSNs
{
    /// <summary>
    /// PPSN信息
    /// </summary>
    [Serializable]
    public class PPSNInfo
    {
        /// <summary>
        /// PPID
        /// </summary>
        public string PPSN { get; set; }

        /// <summary>
        /// 产品SN
        /// </summary>
        public string ProductSN { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public double ProductId { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public double Operator { get; set; }

        /// <summary>
        /// 绑定时间
        /// </summary>
        public DateTime BindingDate { get; set; }

        /// <summary>
        /// 拼板码
        /// </summary>
        public string CombinedCode { get; set; }
    }

   public class PPSNInfoComparer : IEqualityComparer<PPSNInfo>
    {
        public bool Equals(PPSNInfo x, PPSNInfo y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;
            return x.PPSN == y.PPSN && x.ProductSN == y.ProductSN && x.ProductId == y.ProductId && x.WorkOrderNo==y.WorkOrderNo;
        }

        public int GetHashCode(PPSNInfo obj)
        {
            int hash = 17;
            hash = hash * 23 + obj.PPSN.GetHashCode();
            hash = hash * 23 + obj.ProductSN.GetHashCode();
            hash = hash * 23 + obj.ProductId.GetHashCode();
            hash = hash * 23 + obj.WorkOrderNo.GetHashCode();
            return hash;
        }
    }
}