using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Inventory.Strategy;
using SIE.Web.Command;
using SIE.Web.Common.Commands;
using System;
using System.Linq;

namespace SIE.Web.Inventory.Strategy.Commands
{
    #region 分配规则
    #region 添加命令
    /// <summary>
    /// 添加命令
    /// </summary>
    [JsCommand("SIE.Web.Inventory.Strategy.Commands.AddAssignRuleCommand")]
    public class AddAssignRuleCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<AssignRule>();
            var code = RT.Service.Resolve<RuleController>().GetAssignRuleCode();
            data.Code = code;
            data.State = State.Enable;
            return data;
        }
    }
    #endregion

    /// <summary>
    /// 修改命令
    /// </summary>
    [JsCommand("SIE.Web.Inventory.Strategy.Commands.EditAssignRuleCommand")]
    public class EditAssignRuleCommand : ViewCommand
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
    [JsCommand("SIE.Web.Inventory.Strategy.Commands.DeleteAssignRuleCommand")]
    public class DeleteAssignRuleCommand : DeleteCommand
    {
    }
    #endregion

    /// <summary>
    /// 禁用命令
    /// </summary>
    [JsCommand("SIE.Web.Inventory.Strategy.Commands.DisableAssignRuleCommand")]
    public class DisableAssignRuleCommand : DisableCommand
    {
    }

    #region 初始化
    /// <summary>
    /// 初始化
    /// </summary>
    public class InitAssignRuleCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            RT.Service.Resolve<RuleController>().InitAssignRule();

            return true;
        }
    }
    #endregion

    #region 设置默认命令
    /// <summary>
    /// 设置默认命令
    /// </summary>
    public class SetIsDefaultAssignRuleCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(double[] args, string scope)
        {
            RT.Service.Resolve<RuleController>().SetIsDefaultAssignRuleData(args.FirstOrDefault());
            return true;
        }
    }
    #endregion

    /// <summary>
    /// 保存命令
    /// </summary>
    public class SaveAssignRuleCommand : SaveCommand
    {
        /// <summary>
        /// 保存前事件
        /// </summary>
        /// <param name="data">实体列表</param>
        protected override void OnSaving(EntityList data)
        {
            var ctl = RT.Service.Resolve<RuleController>();
            EntityList<AssignRule> assignRuleList = data.CastTo<EntityList<AssignRule>>();
            assignRuleList.ForEach(p =>
            {
                if (p.PersistenceStatus != PersistenceStatus.New)
                {
                    var delIdList = p.AssignRuleDetailList.DeletedList.Select(f => f.GetId()).ToList();
                    var commitDtlIdList = p.AssignRuleDetailList.Select(c => c.Id).ToList();
                    var oldDtlList = ctl.GetAssignRuleDetails(p.Id).Where(c => !commitDtlIdList.Contains(c.Id) && !delIdList.Contains(p.Id)).ToList();
                    p.AssignRuleDetailList.AddRange(oldDtlList);
                    p.AssignRuleDetailList.Where(x => !commitDtlIdList.Contains(x.Id)).ForEach(x => x.PersistenceStatus = PersistenceStatus.Unchanged);
                }
                if (p.PersistenceStatus == PersistenceStatus.New || p.PersistenceStatus == PersistenceStatus.Modified)
                {
                    if (p.OnhandState != null && p.AssignRuleDetailList.Any(x => x.State != p.OnhandState))
                        throw new ValidationException("明细库存状态必须与分配规则库存状态限制保持一致!");
                }
            });
            base.OnSaving(assignRuleList);
        }
    }
    #endregion

    #region 分配规则明细
    #region 添加命令
    /// <summary>
    /// 添加命令
    /// </summary>
    public class AddAssignRuleDetailCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<AssignRuleDetail>();
            var assignRule = RF.GetById<AssignRule>(data.AssignRuleId);
            if (assignRule != null)
            {
                data.LineNo = assignRule.AssignRuleDetailList.Count == 0 ? 1 : assignRule.AssignRuleDetailList.Max(p =>
                {
                    return p.LineNo + 1;
                });
            }

            data.Sort1 = AssignSortType.TurnOver;
            return data;
        }
    }
    #endregion

    #region 修改命令
    /// <summary>
    /// 修改命令
    /// </summary>
    public class EditAssignRuleDetailCommand : ViewCommand
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
    public class DeleteAssignRuleDetailCommand : DeleteCommand
    {
    }
    #endregion

    #endregion
}
