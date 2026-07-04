using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Fixtures.Fixtures.Accounts.ViewModels
{
    /// <summary>
    /// 工治具台账
    /// </summary>
    [Serializable]
    public class FixtureIDAccountData
    {
        /// <summary>
        /// 工治具台账ID
        /// </summary>
        public double AccountId { get; set; }

        /// <summary>
        /// 工治具台账编码
        /// </summary>
        public string AccountCode { get; set; }

        /// <summary>
        /// 工治具台账名称
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 工治具编码
        /// </summary>
        public double FixtureId { get; set; }
    }
}
