using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Logging;
using SIE.MES.WorkOrders;
using SIE.Packages;
using SIE.Packages.Printables;
using SIE.Threading;
using SIE.Wpf.Command;
using SIE.Wpf.Common.Prints;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.WIP.Packings.Commands
{
    /// <summary>
    /// 打印包装条码命令
    /// </summary>
    [Command(ImageName = "Print", Label = "打印包装条码", ToolTip = "打印包装条码", GroupType = CommandGroupType.Edit, Gestures = "Ctrl+F2")]
    public class PrintBarcodeCommand : ListViewCommand
    {
        readonly ILog logger = LogManager.GetLogger("wip");
        /// <summary>
        /// 未打印抛出异常
        /// </summary>
        public const string NotPrintThrowException = "NotPrintThrowException";

        /// <summary>
        /// 允许叠加
        /// </summary>
        public const string EnableMultiple = "EnableMultiple";

        /// <summary>
        /// 包装ViewModel
        /// </summary>
        private PackingViewModel ViewModel => View.Parent.Current as PackingViewModel;

        /// <summary>
        /// 是否可执行
        /// 1、单个包装条码打印：包装号不为空且不支持多选，选中的包装个数为1且打印机不为空
        /// 2、多个包装条码打印：包装号都不为空，支持多选，选中的包装个数大于等于1且打印机不为空
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var pkg = view.SelectedEntities.LastOrDefault() as PackingRelation;
            var result = ((pkg != null && !pkg.PackageNo.IsNullOrEmpty() && (bool?)view[EnableMultiple] != true && view.SelectedEntities.Count == 1) || ((bool?)view[EnableMultiple] == true && view.SelectedEntities.Count >= 1 && !view.SelectedEntities.OfType<PackingRelation>().Any(f => f.PackageNo.IsNullOrEmpty())))
                && ViewModel.Printer.IsNotEmpty();
            return result;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            using (PerformenceWatcher.Start(logger, "PrintBarcodeCommand"))
            {
                var current = view.SelectedEntities.LastOrDefault() as PackingRelation;
                var selectPkgs = view.SelectedEntities.OfType<PackingRelation>().OrderBy(f => f.ItemQty).ToArray();
                var wo = RF.GetById<WorkOrder>(current.WorkOrderId);
                if (wo == null)
                {
                    throw new ValidationException("找不到此包装[{0}]对应的工单信息".L10nFormat(current.PackageNo));
                }
                foreach (PackingRelation pkg in selectPkgs)
                {
                    var rule = wo.PackageRuleDetailList.FirstOrDefault(f => f.PackageUnitId == pkg.PackageUnit.Id);
                    if (rule == null)
                    {
                        throw new ValidationException("工单[{0}]不存在[{1}]对应的包装规则".L10nFormat(wo.No, pkg.PackageUnit.Name));
                    }
                    if (!rule.IsPrint && view.GetPropertyOrDefault(NotPrintThrowException, true))
                    {
                        throw new ValidationException("包装不需要打印,如果需要打印,请前往对应工单进行配置".L10N());
                    }
                    else if (!rule.IsPrint && !view.GetPropertyOrDefault(NotPrintThrowException, true))
                    {
                        continue;
                    }
                    else
                    {
                        //
                    }

                    if (rule.PrintTemplate == null)
                    {
                        throw new ValidationException("打印包装[{0}]失败,获取模板时.无法确定打印模板".L10nFormat(pkg.PackageNo));
                    }

                    var filePath = RT.Service.Resolve<PrintsController>().DownloadPrintTemplate(rule.PrintTemplateId.Value);
                    var report = ReportFactory.Current.GetReportByExtension(rule.PrintTemplate.Type);
#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
                    DoPrint(report, filePath, ViewModel.Printer, pkg);
#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
                    RT.Service.Resolve<PackingRelationController>().UpdateRelationState(pkg.Id, LogisticState.Printed);
                    var printedRelation = view.Data.OfType<PackingRelation>().FirstOrDefault(p => p.PackageNo == pkg.PackageNo);
                    if (printedRelation != null)
                    {
                        printedRelation.State = LogisticState.Printed;
                    }
                }
            }
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="report"></param>
        /// <param name="filePath"></param>
        /// <param name="printer"></param>
        /// <param name="pkg"></param>
        /// <returns></returns>
        private async Task DoPrint(IReport report, string filePath, string printer, PackingRelation pkg)
        {
            await Task.Run(new Action(() =>
              {
                  using (PerformenceWatcher.Start(logger, "DoPrint"))
                  {
                      PackingRelationPrintable printable = new PackingRelationPrintable();
                      report.Print(printable, filePath, printer, () =>
                      {
                          return new PackingRelation[] { pkg };
                      }, () =>
                      {
                      });
                  }
              }).WithCurrentThreadContext()).ConfigureAwait(true);
        }
    }
}
