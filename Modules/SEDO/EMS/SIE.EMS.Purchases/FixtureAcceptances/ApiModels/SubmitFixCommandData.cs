using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EMS.Purchases.FixtureAcceptances.ApiModels
{
    /// <summary>
    /// 提交数据
    /// </summary>
    [Serializable]
    public class SubmitFixCommandData
    {
        /// <summary>
        /// 验收单
        /// </summary>
        public EntityList<FixtureAcceptance> Acceptances { get; set; }

        /// <summary>
        /// 序列号明细
        /// </summary>
        public EntityList<FixtureAcceptanceSn> FixtureAcceptanceSns { get; set; }

        /// <summary>
        /// 验收项目
        /// </summary>
        public EntityList<FixtureAcceptanceItem> FixtureAcceptanceItems { get; set; }
    }
}
