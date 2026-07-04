using System;

namespace SIE.EventMessages.EMS.Fixtures
{
    /// <summary>
    /// 工治具验收信息
    /// </summary>
    [Serializable]
    public class FixtureAcceptanceInfo
    {

        /// <summary>
        /// Id
        /// </summary>
        public double Id { get; set; }
       /// <summary>
       /// 工治具编码Id
       /// </summary>
        public double FixtureEncodeId { get; set; }

        /// <summary>
        /// 待验收合格数
        /// </summary>
        public int PassQty { get; set; }
    }
}
