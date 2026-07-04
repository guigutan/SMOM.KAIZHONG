using System;

namespace SIE.Fixtures.Repairs.ViewModels
{
    /// <summary>
    /// 工治具报修信息
    /// </summary>
    [Serializable]
    public class AddFixtureRepairInfo
    {
        /// <summary>
        /// 工治具报修
        /// </summary>
        public FixtureRepair data { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string errMsg { get; set; }
    }
}
