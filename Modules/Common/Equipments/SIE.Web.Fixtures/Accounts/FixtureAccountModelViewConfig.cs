using SIE.Fixtures.Fixtures.Accounts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Fixtures.Accounts
{
    /// <summary>
    /// 工治具台账-界面
    /// </summary>
    internal class FixtureAccountModelViewConfig : WebViewConfig<FixtureAccountModel>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AddBehavior("SIE.Web.Fixtures.Accounts.Scripts.FixtureAccountModelBehavior");
        }
    }
}
