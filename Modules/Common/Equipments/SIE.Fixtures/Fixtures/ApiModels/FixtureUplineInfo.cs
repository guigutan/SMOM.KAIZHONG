using System;

namespace SIE.Fixtures.Fixtures.ApiModels
{
    /// <summary>
    /// 工治具上线信息
    /// </summary>
    [Serializable]
    public class FixtureUplineInfo
    {
        /// <summary>
        /// 产线ID
        /// </summary>
        public double ResourceId { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public double WorkOrderId { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public double EquipId { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public double? ProcessId { get; set; }

        /// <summary>
        /// 工位ID
        /// </summary>
        public double? StationId { get; set; }

        /// <summary>
        /// 工治具条码
        /// </summary>
        public string FixtureEncodeCode { get; set; }
    }
}