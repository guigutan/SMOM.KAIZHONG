using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Inventory.Transactions;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Inventory.Transactions.Commands
{
    #region 删除
    /// <summary>
    /// 修改命令
    /// </summary>
    [JsCommand("SIE.Web.Inventory.Transactions.Commands.DeleteTransactionCommand")]
    public class DeleteTransactionCommand : ViewCommand
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

    #region 启用/禁止内部使用
    /// <summary>
    /// 启用/禁止内部使用
    /// </summary>
    [JsCommand("SIE.Web.Inventory.Transactions.Commands.SetTransactionInternalUseCommand")]
    public class SetTransactionInternalUseCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            List<double> idlist = args.SelectedIds.ToList(); //transactionId列表  
            foreach (var id in idlist)
            {
                try
                {
                    var trans = RF.GetById<Transaction>(id);
                    trans.IsInternalUse = !trans.IsInternalUse;
                    RF.Save(trans);
                    trans.MarkSaved();
                    trans.NotifyAllPropertiesChanged();
                }
                catch (Exception exc)
                {
                    throw new ValidationException(exc.ToString());
                }
            }

            return true;
        }
    }
    #endregion

    #region 启用/禁止整单上传
    /// <summary>
    /// 启用/禁止整单上传
    /// </summary>
    [JsCommand("SIE.Web.Inventory.Transactions.Commands.SetTransactionIsUploadCommand")]
    public class SetTransactionIsUploadCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            List<double> idlist = args.SelectedIds.ToList(); //transactionId列表  
            foreach (var id in idlist)
            {
                try
                {
                    var trans = RF.GetById<Transaction>(id);
                    trans.IsUpload = !trans.IsUpload;
                    RF.Save(trans);
                    trans.MarkSaved();
                    trans.NotifyAllPropertiesChanged();
                }
                catch (Exception exc)
                {
                    throw new ValidationException(exc.ToString());
                }
            }

            return true;
        }
    }
    #endregion

    #region 修改事务
    /// <summary>
    /// 修改事务
    /// </summary>
    class EditTransactionDetailCommand : ViewCommand
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
}
