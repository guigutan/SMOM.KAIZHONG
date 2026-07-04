using SIE.Domain;
using System;

namespace SIE.Fixtures.Repairs.ViewModels
{
    /// <summary>
    /// 工治具报修-添加
    /// </summary>
    [Serializable]
    public class FixtureRepairInfo
    {
        /// <summary>
        /// 工治具报修
        /// </summary>
        public FixtureRepair FixtureRepair { get; set; }

        /// <summary>
        /// 治具异常详情
        /// </summary>
        public EntityList<FixtureRepairDetail> FixtureRepairDetailList { get; set; }

        /// <summary>
        /// 维修记录
        /// </summary>
        public EntityList<FixtureRepairRecord> FixtureRepairRecordList { get; set; }
    }
}
