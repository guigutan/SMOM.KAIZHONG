using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.WorkOrders;
using SIE.Packages;
using SIE.Packages.Printables;
using SIE.Wpf.Command;
using SIE.Wpf.Common.Prints;
using System;
using System.Linq;

namespace SIE.Wpf.MES.BatchWIP.Packings.Commands
{
    /// <summary>
    /// 打印包装条码命令
    /// </summary>
    [Command(ImageName = "Print", Label = "打印包装条码", ToolTip = "打印包装条码", GroupType = CommandGroupType.Edit, Gestures = "Ctrl+F2")]
    public class BatchPkgBarcodePrintCommand : ListViewCommand
    {
        ///// <summary>
        ///// 未打印抛出异常
        ///// </summary>
        //public const string NotPrintThrowException = "NotPrintThrowException";

        ///// <summary>
        ///// 允许叠加
        ///// </summary>
        //public const string EnableMultiple = "EnableMultiple";

        /// <summary>
        /// 包装ViewModel
        /// </summary>
        private BatchPackingViewModel ViewModel => View.Relations.Find("mainView").Current as BatchPackingViewModel;

        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var pkg = view.Current as BatchPackingRelation;
            var pkgs = view.SelectedEntities.OfType<BatchPackingRelation>().ToList();

            return pkgs.Count >= 1 &&
                !pkgs.Any(p => p.PackingBatch != pkg.PackingBatch && p.PackageUnitId != pkg.PackageUnitId && (p.State == LogisticState.UnPrinted || p.State == LogisticState.Printed));
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var pkg = view.Current as BatchPackingRelation;
            var wo = RF.GetById<WorkOrder>(pkg.WorkOrderId);

            var pkgs = view.SelectedEntities.OfType<BatchPackingRelation>().ToList();

            try
            {
                var rule = wo.PackageRuleDetailList.FirstOrDefault(f => f.PackageUnitId == pkg.PackageUnit.Id);
                if (rule == null)
                    throw new ValidationException("工单[{0}]不存在[{1}]对应的包装层级".L10nFormat(wo.No, pkg.PackageUnit.Name));
                if (!rule.IsPrint)
                    throw new ValidationException("工单[{0}]对应包装层级[{1}]不需要打印".L10nFormat(wo.No, pkg.PackageUnit.Name));
                if (rule.PrintTemplateId == null)
                    throw new ValidationException("工单[{0}]对应的[{1}]包装层级不存在打印模板".L10nFormat(wo.No, pkg.PackageUnit.Name));

                var filePath = RT.Service.Resolve<PrintsController>().DownloadPrintTemplate(rule.PrintTemplateId.Value);
                var printable = new BatchPackingRelationPrintable();
                var report = ReportFactory.Current.GetReportByExtension(rule.PrintTemplate.Type);
                report.Print(printable, filePath, ViewModel.Printer, () =>
                {
                    return pkgs;
                }, () =>
                {
                    pkgs.ForEach(p => p.State = LogisticState.Printed);
                    var relas = new EntityList<BatchPackingRelation>();
                    relas.AddRange(pkgs);
                    RF.Save(relas);
                });
            }
            catch (Exception exc)
            {
                ViewModel.ShowError(exc);
            }
            finally
            {

                ViewModel.ReloadPackingRelation();
                view.RefreshControl();
            }
        }
    }
}
