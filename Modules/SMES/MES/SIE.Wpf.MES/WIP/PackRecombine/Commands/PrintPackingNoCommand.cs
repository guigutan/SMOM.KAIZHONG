using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.WorkOrders;
using SIE.Packages;
using SIE.Packages.Printables;
using SIE.Wpf.Command;
using SIE.Wpf.Common.Prints;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Wpf.MES.WIP.PackRecombine.Commands
{
    /// <summary>
    /// 打印包装条码命令
    /// </summary>
    [Command(ImageName = "Print", Label = "打印包装条码", ToolTip = "打印包装条码", GroupType = CommandGroupType.Edit, Gestures = "Ctrl+F2")]
    public class PrintPackingNoCommand : ListViewCommand
    {
        /// <summary>
        /// 判断命令能否执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>能执行返回true，否则返回false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            bool isSearchMode = false;
            if (view != null)
            {
                var parent = View.Relations.Find("mainView")?.Current as PackRecombineBaseViewModel;
                if (parent != null && parent.PackScanMode == SIE.MES.WIP.PackRecombine.ScanMode.Search)
                {
                    isSearchMode = true;
                }
            }
            return view != null && view.SelectedEntities.OfType<PackingRelation>().Any() && !isSearchMode;
        }

        /// <summary>
        /// 命令执行逻辑
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <exception cref="ValidationException">未找到工单信息，未找到工单包装规则，未找到标签模板</exception> 
        public override void Execute(ListLogicalView view)
        {
            var current = view.SelectedEntities.LastOrDefault() as PackingRelation;
            var selectPkgs = view.SelectedEntities.OfType<PackingRelation>().OrderBy(f => f.ItemQty).ToArray();
            string printer = GetPrinter();
            Dictionary<double, WorkOrder> dicWo = new Dictionary<double, WorkOrder>();
            foreach (PackingRelation pkg in selectPkgs)
            {
                WorkOrder workOrder;
                if (!dicWo.TryGetValue(current.WorkOrderId, out workOrder))
                {
                    workOrder = RF.GetById<WorkOrder>(current.WorkOrderId);
                    if (workOrder == null)
                        throw new ValidationException("找不到此包装[{0}]对应的工单信息".L10nFormat(current.PackageNo));
                    dicWo.Add(current.WorkOrderId, workOrder);
                }
                var rule = workOrder.PackageRuleDetailList.FirstOrDefault(f => f.PackageUnitId == pkg.PackageUnit.Id);
                if (rule == null)
                    throw new ValidationException("工单[{0}]不存在[{1}]对应的包装规则".L10nFormat(workOrder.No, pkg.PackageUnit.Name));
                if (rule.NumberRuleId == null)
                    throw new ValidationException("打印包装[{0}]失败,获取模板时.无法确定对应工单[{1}]包装规则".L10nFormat(pkg.PackageNo, workOrder.No));
                if (rule.PrintTemplate == null)
                    throw new ValidationException("打印包装[{0}]失败,获取模板时.无法确定打印模板".L10nFormat(pkg.PackageNo));
                var filePath = RT.Service.Resolve<PrintsController>().DownloadPrintTemplate(rule.PrintTemplateId.Value);
                var printable = new PackingRelationPrintable();
                var report = ReportFactory.Current.GetReportByExtension(rule.PrintTemplate.Type);
                report.Print(printable, filePath, printer, () =>
                {
                    return new PackingRelation[] { pkg };
                }, () =>
                {
                });
            }
            view.RefreshControl();
        }

        /// <summary>
        /// 获取打印机名称
        /// </summary>
        /// <returns>打印机名称</returns>
        /// <exception cref="ValidationException">未配置打印机</exception>
        string GetPrinter()
        {
            var printer = SIE.Common.Properties.Settings.Default?.PrinterName;

            if (printer.IsNullOrEmpty())
            {
                throw new ValidationException("未配置打印机，请在【打印机配置】进行配置".L10N());
            }

            return printer;
        }
    }
}