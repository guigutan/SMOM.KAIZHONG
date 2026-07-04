using SIE.Domain;
using SIE.ERPInterface.Common.ERPJobCloseRules;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.ERPInterface.ERPJobCloseRules.Commands
{
    /// <summary>
    /// 交易期关闭日保存命令
    /// </summary>
    public class ErpJobCloseRuleSaveCommand : SaveCommand
    {
        /// <summary>
        /// 保存前交易
        /// </summary>
        /// <param name="data"></param>
        protected override void OnSaving(EntityList data)
        {
            var rule = data as EntityList<ErpJobCloseRule>;
            RT.Service.Resolve<ErpJobCloseRuleController>().ValidateBeforeSave(rule);
            base.OnSaving(data);
        }
    }
}
