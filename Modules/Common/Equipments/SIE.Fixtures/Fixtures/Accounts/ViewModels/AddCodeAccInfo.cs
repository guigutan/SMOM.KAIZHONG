using System;

namespace SIE.Fixtures.Fixtures.Accounts.ViewModels
{
    /// <summary>
    /// 编码类工治具台账信息
    /// </summary>
    [Serializable]
    public class AddCodeAccInfo
    {
        /// <summary>
        /// 状态
        /// </summary>
        public FixtureAccountState State { get; set; }

        /// <summary>
        /// 工治具台账
        /// </summary>
        public FixtureAccount Account { get; set; }
    }
}
