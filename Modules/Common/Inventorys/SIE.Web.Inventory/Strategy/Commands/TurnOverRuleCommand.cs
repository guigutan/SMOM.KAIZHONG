using SIE.Domain;
using SIE.Inventory.Strategy;
using SIE.Security;
using SIE.Web.Command;
using SIE.Web.Common.Commands;
using System.Linq;

namespace SIE.Web.Inventory.Strategy.Commands
{
    #region 周转规则
    #region 添加命令
    /// <summary>
    /// 添加命令
    /// </summary>
    [JsCommand("SIE.Web.Inventory.Strategy.Commands.AddTurnOverRuleCommand")]
    public class AddTurnOverRuleCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<TurnOverRule>();
            var code = RT.Service.Resolve<RuleController>().GetTurnOverRuleCode();
            data.Code = code;
            data.State = State.Enable;
            data.CanPickOverDue = true;
            return data;
        }
    }
    #endregion

    /// <summary>
    /// 修改命令
    /// </summary>
    [JsCommand("SIE.Web.Inventory.Strategy.Commands.EditTurnOverRuleCommand")]
    public class EditTurnOverRuleCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }

    #region 删除命令
    /// <summary>
    /// 删除命令
    /// </summary>
    [JsCommand("SIE.Web.Inventory.Strategy.Commands.DeleteTurnOverRuleCommand")]
    public class DeleteTurnOverRuleCommand : DeleteCommand
    {
    }
    #endregion

    /// <summary>
    /// 修改命令
    /// </summary>
    [JsCommand("SIE.Web.Inventory.Strategy.Commands.DisableTurnOverRuleCommand")]
    public class DisableTurnOverRuleCommand : DisableCommand
    {
    }

    #region 初始化
    /// <summary>
    /// 初始化
    /// </summary>
    public class InitTurnOverRuleCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            RT.Service.Resolve<RuleController>().InitTurnOverRule();

            return true;
        }
    }
    #endregion

    #region 设置默认命令
    /// <summary>
    /// 设置默认命令
    /// </summary>
    public class SetIsDefaultTurnOverRuleCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(double[] args, string scope)
        {
            RT.Service.Resolve<RuleController>().SetIsDefaultTurnOverRuleData(args.FirstOrDefault());
            return true;
        }
    }
    #endregion
    #endregion

    #region 周转规则明细
    #region 添加命令
    /// <summary>
    /// 添加命令
    /// </summary>
    public class AddTurnOverRuleDetailCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<TurnOverRuleDetail>();
            var turnOverRule = RF.GetById<TurnOverRule>(data.TurnOverRuleId);
            if (turnOverRule != null)
            {
                data.LineNo = turnOverRule.DetailList.Count == 0 ? 1 : turnOverRule.DetailList.Max(p =>
                {
                    return p.LineNo + 1;
                });
            }

            return data;
        }
    }
    #endregion

    /// <summary>
    /// 验证周转规则
    /// </summary>
    [AllowAnonymous]
    public class ValidTurnOverRuleDetailCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<TurnOverRuleDetail>();
            RT.Service.Resolve<RuleController>().ValidValidTurnOverRuleDetail(data);
            return true;
        }
    }

    #region 修改命令
    /// <summary>
    /// 修改命令
    /// </summary>
    public class EditTurnOverRuleDetailCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }
    #endregion

    #region 删除命令
    /// <summary>
    /// 删除命令
    /// </summary>
    public class DeleteTurnOverRuleDetailCommand : DeleteCommand
    {
    }
    #endregion

    #endregion
}
