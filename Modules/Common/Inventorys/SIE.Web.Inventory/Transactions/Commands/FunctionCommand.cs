using SIE.Domain;
using SIE.Inventory.Transactions;
using SIE.Web.Command;
using SIE.Web.Inventory.Transactions.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Inventory.Transactions.Commands
{
    #region 功能界面选择事务
    /// <summary>
    /// 功能界面选择事务
    /// </summary>
    [JsCommand("SIE.Web.Inventory.Transactions.Commands.LookupTransactionCommand")]
    public class LookupTransactionCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var meta = ClientEntities.Find(args.Type);
            var savedData = RF.Find(meta.EntityType).NewList();
            var transactionList = args.Data.ToJsonObject<List<FunctionTransaction>>();
            Check.NotNullOrEmpty(transactionList, nameof(transactionList));
            if (transactionList == null || transactionList.Count == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(transactionList)));
            }
            foreach (var item in transactionList)
            {
                var transaction = new FunctionTransaction();
                transaction.TransactionId = item.TransactionId;
                transaction.FunctionId = item.FunctionId;
                savedData.Add(transaction);
            }
            RF.Save(savedData);
            return true;
        }
    }
    #endregion

    #region 删除
    /// <summary>
    /// 修改命令
    /// </summary>
    public class DeleteFunTransactionCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(double[] args, string scope)
        {
            var trans = RT.Service.Resolve<TransactionController>().GetTransactionDatas(args.ToList());
            trans.ForEach(p => p.PersistenceStatus = PersistenceStatus.Deleted);
            RF.Save(trans);
            return true;
        }
    }
    #endregion

    #region 功能初始化
    /// <summary>
    /// 功能初始化
    /// </summary>
    [JsCommand("SIE.Web.Inventory.Transactions.Commands.InitFunctionCommand")]
    public class InitFunctionCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<List<FunctionModel>>();
            var codes = data.Select(p => p.Code).Distinct().ToList();
            RT.Service.Resolve<TransactionController>().InitFunction(codes);
            return true;
        }
    }
    #endregion

    #region 禁用IQC报检
    /// <summary>
    /// 禁用IQC报检
    /// </summary>
    [JsCommand("SIE.Web.Inventory.Transactions.Commands.DisableFunctionIsQcCommand")]
    public class DisableFunctionIsQcCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var ctl = RT.Service.Resolve<TransactionController>();
            if (null == args.SelectedIds || args.SelectedIds.Length == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(args.SelectedIds)));
            }

            ctl.DisableFunctionIsQc(args.SelectedIds.ToList());

            return true;
        }
    }
    #endregion

    #region 启用IQC报检
    /// <summary>
    /// 启用IQC报检
    /// </summary>
    [JsCommand("SIE.Web.Inventory.Transactions.Commands.EnableFunctionIsQcCommand")]
    public class EnableFunctionIsQcCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var ctl = RT.Service.Resolve<TransactionController>();
            if (null == args.SelectedIds || args.SelectedIds.Length == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(args.SelectedIds)));
            }

            ctl.EnableFunctionIsQc(args.SelectedIds.ToList());

            return true;
        }
    }
    #endregion

    #region 禁用OQC报检
    /// <summary>
    /// 禁用OQC报检
    /// </summary>
    [JsCommand("SIE.Web.Inventory.Transactions.Commands.DisableFunctionOqcCommand")]
    public class DisableFunctionOqcCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var ctl = RT.Service.Resolve<TransactionController>();
            if (null == args.SelectedIds || args.SelectedIds.Length == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(args.SelectedIds)));
            }

            ctl.DisableFunctionOqc(args.SelectedIds.ToList());

            return true;
        }
    }
    #endregion

    #region 启用OQC报检
    /// <summary>
    /// 启用OQC报检
    /// </summary>
    [JsCommand("SIE.Web.Inventory.Transactions.Commands.EnableFunctionOqcCommand")]
    public class EnableFunctionOqcCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var ctl = RT.Service.Resolve<TransactionController>();
            if (null == args.SelectedIds || args.SelectedIds.Length == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(args.SelectedIds)));
            }

            ctl.EnableFunctionOqc(args.SelectedIds.ToList());

            return true;
        }
    }
    #endregion

    #region 禁用允许超发
    /// <summary>
    /// 禁用允许超发
    /// </summary>
    [JsCommand("SIE.Web.Inventory.Transactions.Commands.DisableFunctionAllowOutCommand")]
    public class DisableFunctionAllowOutCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var ctl = RT.Service.Resolve<TransactionController>();
            if (null == args.SelectedIds || args.SelectedIds.Length == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(args.SelectedIds)));
            }

            ctl.DisableFunctionAllowOut(args.SelectedIds.ToList());

            return true;
        }
    }
    #endregion

    #region 启用允许超发
    /// <summary>
    /// 启用允许超发
    /// </summary>
    [JsCommand("SIE.Web.Inventory.Transactions.Commands.EnableFunctionAllowOutCommand")]
    public class EnableFunctionAllowOutCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var ctl = RT.Service.Resolve<TransactionController>();
            if (null == args.SelectedIds || args.SelectedIds.Length == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(args.SelectedIds)));
            }

            ctl.EnableFunctionAllowOut(args.SelectedIds.ToList());

            return true;
        }
    }
    #endregion

    #region 禁用拣货后即发货
    /// <summary>
    /// 禁用拣货后即发货
    /// </summary>
    [JsCommand("SIE.Web.Inventory.Transactions.Commands.DisableIsAutoDeliveryCommand")]
    public class DisableIsAutoDeliveryCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var ctl = RT.Service.Resolve<TransactionController>();
            if (null == args.SelectedIds || args.SelectedIds.Length == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(args.SelectedIds)));
            }

            ctl.DisableFunctionIsAutoDelivery(args.SelectedIds.ToList());

            return true;
        }
    }
    #endregion

    #region 启用拣货后即发货
    /// <summary>
    /// 启用拣货后即发货
    /// </summary>
    [JsCommand("SIE.Web.Inventory.Transactions.Commands.EnableIsAutoDeliveryCommand")]
    public class EnableIsAutoDeliveryCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var ctl = RT.Service.Resolve<TransactionController>();
            if (null == args.SelectedIds || args.SelectedIds.Length == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(args.SelectedIds)));
            }

            ctl.EnableFunctionIsAutoDelivery(args.SelectedIds.ToList());

            return true;
        }
    }
    #endregion

    #region 禁用越库后自动拣货
    /// <summary>
    /// 禁用越库后自动拣货
    /// </summary>
    [JsCommand("SIE.Web.Inventory.Transactions.Commands.DisableIsCrossPickCommand")]
    public class DisableIsCrossPickCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var ctl = RT.Service.Resolve<TransactionController>();
            if (null == args.SelectedIds || args.SelectedIds.Length == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(args.SelectedIds)));
            }

            ctl.DisableFunctionIsCrossPick(args.SelectedIds.ToList());

            return true;
        }
    }
    #endregion

    #region 启用越库后自动拣货
    /// <summary>
    /// 启用越库后自动拣货
    /// </summary>
    [JsCommand("SIE.Web.Inventory.Transactions.Commands.EnableIsCrossPickCommand")]
    public class EnableIsCrossPickCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var ctl = RT.Service.Resolve<TransactionController>();
            if (null == args.SelectedIds || args.SelectedIds.Length == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(args.SelectedIds)));
            }

            ctl.EnableFunctionIsCrossPick(args.SelectedIds.ToList());

            return true;
        }
    }
    #endregion

    #region 启用按包装分配
    /// <summary>
    /// 启用按包装分配
    /// </summary>
    public class EnableIsPackageCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var ctl = RT.Service.Resolve<TransactionController>();
            if (null == args.SelectedIds || args.SelectedIds.Length == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(args.SelectedIds)));
            }

            ctl.EnableIsPackage(args.SelectedIds.ToList());

            return true;
        }
    }
    #endregion

    #region 禁用按包装分配
    /// <summary>
    /// 禁用按包装分配
    /// </summary>
    public class DisableIsPackageCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var ctl = RT.Service.Resolve<TransactionController>();
            if (null == args.SelectedIds || args.SelectedIds.Length == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(args.SelectedIds)));
            }

            ctl.DisableIsPackage(args.SelectedIds.ToList());

            return true;
        }
    }
    #endregion

    #region 启用收货到不合格
    /// <summary>
    /// 启用收货到不合格
    /// </summary>
    [JsCommand("SIE.Web.Inventory.Transactions.Commands.EnableIsReceiveNgCommand")]
    public class EnableIsReceiveNgCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var ctl = RT.Service.Resolve<TransactionController>();
            if (null == args.SelectedIds || args.SelectedIds.Length == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(args.SelectedIds)));
            }

            ctl.EnableIsReceiveNg(args.SelectedIds.ToList());

            return true;
        }
    }
    #endregion

    #region 禁用收货到不合格
    /// <summary>
    /// 禁用收货到不合格
    /// </summary>
    [JsCommand("SIE.Web.Inventory.Transactions.Commands.DisableIsReceiveNgCommand")]
    public class DisableIsReceiveNgCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var ctl = RT.Service.Resolve<TransactionController>();
            if (null == args.SelectedIds || args.SelectedIds.Length == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(args.SelectedIds)));
            }

            ctl.DisableIsReceiveNg(args.SelectedIds.ToList());

            return true;
        }
    }
    #endregion

    #region 禁用按送货明细收货
    /// <summary>
    /// 禁用按送货明细收货
    /// </summary>
    [JsCommand("SIE.Web.Inventory.Transactions.Commands.DisableCollectByDeliveryCommand")]
    public class DisableCollectByDeliveryCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var ctl = RT.Service.Resolve<TransactionController>();
            if (null == args.SelectedIds || args.SelectedIds.Length == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(args.SelectedIds)));
            }

            ctl.DisableCollectByDelivery(args.SelectedIds.ToList());

            return true;
        }
    }
    #endregion

    #region 启用按送货明细收货
    /// <summary>
    /// 启用按送货明细收货
    /// </summary>
    [JsCommand("SIE.Web.Inventory.Transactions.Commands.EnableCollectByDeliveryCommand")]
    public class EnableCollectByDeliveryCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var ctl = RT.Service.Resolve<TransactionController>();
            if (null == args.SelectedIds || args.SelectedIds.Length == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(args.SelectedIds)));
            }

            ctl.EnableCollectByDelivery(args.SelectedIds.ToList());

            return true;
        }
    }
    #endregion

    #region 启用部分发货
    /// <summary>
    /// 启用部分发货
    /// </summary>
    [JsCommand("SIE.Web.Inventory.Transactions.Commands.EnableIsPartDeliveryCommand")]
    public class EnableIsPartDeliveryCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var ctl = RT.Service.Resolve<TransactionController>();
            if (null == args.SelectedIds || args.SelectedIds.Length == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(args.SelectedIds)));
            }

            ctl.EnableIsPartDelivery(args.SelectedIds.ToList());

            return true;
        }
    }
    #endregion

    #region 禁用部分发货
    /// <summary>
    /// 禁用部分发货
    /// </summary>
    [JsCommand("SIE.Web.Inventory.Transactions.Commands.DisableIsPartDeliveryCommand")]
    public class DisableIsPartDeliveryCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var ctl = RT.Service.Resolve<TransactionController>();
            if (null == args.SelectedIds || args.SelectedIds.Length == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(args.SelectedIds)));
            }

            ctl.DisableIsPartDelivery(args.SelectedIds.ToList());

            return true;
        }
    }
    #endregion

    #region 大类与员工关系
    /// <summary>
    /// 大类与员工关系
    /// </summary>
    public class FunctionEmployeeLookupCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var meta = ClientEntities.Find(args.Type);
            var savedData = RF.Find(meta.EntityType).NewList();
            var employeeList = args.Data.ToJsonObject<List<FunctionEmployee>>();
            Check.NotNullOrEmpty(employeeList, nameof(employeeList));
            if (employeeList == null || employeeList.Count == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(employeeList)));
            }
            foreach (var item in employeeList)
            {
                var employee = new FunctionEmployee();
                employee.EmployeeId = item.EmployeeId;
                employee.FunctionId = item.FunctionId;
                savedData.Add(employee);
            }
            RF.Save(savedData);
            return true;
        }
    }
    #endregion
}
