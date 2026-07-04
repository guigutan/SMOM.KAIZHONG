using SIE.Domain;
using SIE.Inventory.Strategy;
using SIE.Security;
using SIE.Web.Command;
using System.Linq;

namespace SIE.Web.Inventory.Strategy.Commands
{
    #region 上架规则
    /// <summary>
    /// 添加命令
    /// </summary>
    public class AddOnShelvesRuleCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<OnShelvesRule>();
            var code = RT.Service.Resolve<RuleController>().GetOnShelvesRuleCode();
            data.Code = code;
            data.State = State.Enable;
            return data;
        }
    }

    /// <summary>
    /// 删除命令
    /// </summary>
    public class DeleteOnShelvesRuleCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<OnShelvesRule>();
            var code = RT.Service.Resolve<RuleController>().GetOnShelvesRuleCode();
            data.Code = code;
            data.State = State.Enable;
            return data;
        }
    }

    #region 设置默认命令
    /// <summary>
    /// 设置默认命令
    /// </summary>
    public class SetIsDefaultOnShelvesRuleCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(double[] args, string scope)
        {
            RT.Service.Resolve<RuleController>().SetIsDefaultOnShelvesRuleData(args.FirstOrDefault());
            return true;
        }
    }
    #endregion
    #endregion

    #region 上架规则明细
    /// <summary>
    /// 添加命令
    /// </summary>
    public class AddOnShelvesRuleDetailCommand : ViewCommand
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

    /// <summary>
    /// 编辑命令
    /// </summary>
    public class EditOnShelvesRuleDetailCommand : ViewCommand
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

    /// <summary>
    /// 查看命令
    /// </summary>
    public class ViewOnShelvesRuleDetailCommand : ViewCommand
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

    /// <summary>
    /// 验证上架规则明细
    /// </summary>
    [AllowAnonymous]
    public class ValidOnShelvesRuleDtlCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<OnShelvesRuleDetail>();
            RT.Service.Resolve<RuleController>().ValidOnShelvesRule(data);
            return true;
        }
    }
    #endregion
}
