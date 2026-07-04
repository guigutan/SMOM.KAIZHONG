using SIE.ERPInterface.Common.ERPJobCloseRules;
using SIE.MetaModel.View;
using SIE.Web.ERPInterface.ERPJobCloseRules.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.ERPInterface.ERPJobCloseRules
{
    /// <summary>
    /// 交易期关闭日视图配置
    /// </summary>
    public class ErpJobCloseRuleViewConfig : WebViewConfig<ErpJobCloseRule>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.ReplaceCommands(WebCommandNames.Save, typeof(ErpJobCloseRuleSaveCommand).FullName);
            using(View.OrderProperties())
            {
                View.Property(p => p.During).ShowInList(width: 120);
                View.Property(p => p.StartTime).UseDateTimeEditor(p => p.Format = "Y/m/d H:i").ShowInList(width: 150);
                View.Property(p => p.EndTime).UseDateTimeEditor(p => p.Format = "Y/m/d H:i").ShowInList(width: 150);
            }
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using(View.OrderProperties())
            {
                View.Property(p => p.During).Show();
            }
        }
    }
}
