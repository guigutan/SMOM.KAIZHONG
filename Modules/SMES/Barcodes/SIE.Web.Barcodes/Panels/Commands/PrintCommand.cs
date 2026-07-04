using SIE.Barcodes;
using SIE.Barcodes.Barcodes.ViewModels;
using SIE.Barcodes.Panels;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Web.Barcodes.Panels.ViewModels;
using SIE.Web.Command;
using SIE.Web.Common.Prints;
using System;

namespace SIE.Web.Barcodes.Panels.Commands
{
    /// <summary>
    /// 拼板码打印命令
    /// </summary>
    [JsCommand("SIE.Web.Barcodes.Panels.Commands.PrintCommand")]
    public class PrintCommand : ListViewCommand
    {
        /// <summary>
        /// 命令执行
        /// </summary>
        /// <param name="args">实体参数</param>
        /// <param name="scope">scope</param>
        /// <returns>返回结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var rstPrint = new RstBarcodePrint();
            var model = args.Data.ToJsonObject<PanelPrintViewModel>();
            ValidatePrint(model);
            var info = new PrinterInfo(model.WorkOrderId, model.NumberRuleId.Value, model.TemplateId.Value, model.PrintQty, 1, model.PrintedQty);
            var result = RT.Service.Resolve<PanelController>().PrintPanels(info); // 先保存创建的条码
            if (result.Item1.Length > 0)
                throw new ValidationException(result.Item1);
            else
            {
                rstPrint.Type = model.Template?.Type;
                rstPrint.Url = PrintPanels(model, result.Item2);
                return rstPrint;
            }
        }

        /// <summary>
        /// 打印拼板码
        /// </summary>
        /// <param name="entity">拼板码打印视图模型</param>
        /// <param name="panels">生成拼板码列表</param>
        private string PrintPanels(PanelPrintViewModel entity, EntityList<Panel> panels)
        {
            var template = entity.Template;
            var report = ReportFactory.Current.GetReportByExtension(template.Type);
            var printable = new PanelPrintable();
            return report.PrintProcess(printable, template.Id, template.Content, () =>
            {
                return panels;
            });
        }

        /// <summary>
        /// 验证打印信息
        /// </summary>
        /// <param name="vm">拼板码打印视图模型</param>
        private void ValidatePrint(PanelPrintViewModel vm)
        {
            var curTemplate = RF.GetById<PrintTemplate>(vm.TemplateId);
            if (curTemplate == null)
                throw new EntityNotFoundException(typeof(PrintTemplate), vm.TemplateId);
            if (curTemplate.EntityType != typeof(PanelPrintable).GetQualifiedName())
                throw new ValidationException("打印模板错误，请配置【拼板码】类型的模板！".L10N());
            var broken = vm.Validate();
            if (broken.Count > 0)
                throw new ValidationException(broken.ToString());
        }
    }
}
