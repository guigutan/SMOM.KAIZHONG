using SIE.AbnormalInfo.AbnormalMonitors;
using SIE.Web.Command;
using System;

namespace SIE.Web.AbnormalInfo.AbnormalMonitors.Commands
{
	/// <summary>
	/// 来料检验填写报告命令
	/// </summary>
	[JsCommand("SIE.Web.AbnormalInfo.AbnormalMonitors.Commands.WritingRuleCommand")]
    public class WritingRuleCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">SIE.MetaModel.View.Block.EntityType</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var bill = args.Data.ToJsonObject<AbnormalDecisionRule>();
            if (null == bill)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(bill)));
            }

            //var iqcBill = RT.Service.Resolve<IqcBillController>().WritingReport(bill.Id);
            return bill;
        }
    }
}
