using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.xUnit.DIST.Distribution.Models
{
    [Serializable]
    public class DistributionInfo
    {
        public double WorkOrderId { get; set; }
        public double ResourceId { get; set; }
        public List<DistributionDetailInfo> DetailInfos { get; } = new List<DistributionDetailInfo>();
    }

    [Serializable]
    public class DistributionDetailInfo
    {
        public double ItemId { get; set; }
        public decimal  Qty { get; set; }

        public double UnitId { get; set; }
    }
}
