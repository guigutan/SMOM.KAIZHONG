using SIE.Barcodes;
using SIE.Barcodes.Printables;
using SIE.Barcodes.WipBatchs;
using SIE.Common.Prints;
using SIE.Domain.Validation;
using SIE.Wpf.Barcodes.WipBatchs.ViewModels;
using SIE.Wpf.Command;
using SIE.Wpf.Common.Prints;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Wpf.Barcodes.WipBatchs.Commands
{
    /// <summary>
    /// 子批次信息补打
    /// </summary>
    [Command(ImageName = "Printer", Label = "补打", ToolTip = "补打", GroupType = CommandGroupType.Edit)]
    public class ReprintSubWipBatchCommand : ListViewCommand
    {
        /// <summary>
        /// 是否能执行
        /// </summary>
        /// <param name="view">视图</param>
        /// <returns>选择条码数量大于0返回true，否则返回false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.SelectedEntities.Count > 0;
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="view">视图</param>
        public override void Execute(ListLogicalView view)
        {
            var subWipBatchs = view.SelectedEntities.OfType<SubWipBatch>();
            if (!subWipBatchs.Any())
                return;
            var subWipBatch = View.Current as SubWipBatch;
            var model = new BatchReprintViewModel();
            model.IsSubBatch = true;
            var template = RT.Service.Resolve<BarcodeController>().GetPrintTemplateByWo(subWipBatch.WorkOrderId);
            if (template?.EntityType == typeof(WipBatchPrintable).GetQualifiedName())
                model.Template = template;
            DetailsUITemplate tmpl = new DetailsUITemplate(model.GetType());
            ControlResult ui = tmpl.CreateUI();
            ui.MainView.Data = model;
            CRT.Workbench.ShowDialog(ui, w =>
            {
                w.Title = "生产子批次补打".L10N();
                w.Width = 480;
                w.Height = 280;
                w.Closing += (s, e) =>
                {
                    if (w.Result == 0)
                    {
                        try
                        {
                            ReprintSubWipBatch(model, subWipBatchs.Select(p => p.Id).ToList());
                            view.Parent.Parent.DataLoader.ReloadDataAsync();
                        }
                        catch (Exception ex)
                        {
                            CRT.MessageService.ShowException(ex);
                            e.Cancel = true;
                        }
                    }
                };
            });
        }

        /// <summary>
        /// 打印保存
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="subWipBatchIds">子批次ID列表</param>
        private void ReprintSubWipBatch(BatchReprintViewModel model, List<double> subWipBatchIds)
        {
            if (model.Template == null)
                throw new ValidationException("打印模板不能为空".L10N());
            if (model.Template.EntityType != typeof(WipBatchPrintable).GetQualifiedName())
                throw new ValidationException("打印模板的类型必须是生产子批次".L10N());
            var subWipBatchs = RT.Service.Resolve<WipBatchController>().ReprintSubWipBatch(subWipBatchIds);
            var filePath = RT.Service.Resolve<PrintsController>().DownloadPrintTemplate(model.TemplateId);
            var printable = new WipBatchPrintable();
            var report = ReportFactory.Current.GetReportByExtension(model.Template.Type);
            report.Print(printable, filePath, model.Printer, () =>
            {
                return subWipBatchs;
            }, () =>
            { CRT.MessageService.ShowMessage("打印成功".L10N(), "操作提示".L10N()); }, 1);
        }
    }
}
