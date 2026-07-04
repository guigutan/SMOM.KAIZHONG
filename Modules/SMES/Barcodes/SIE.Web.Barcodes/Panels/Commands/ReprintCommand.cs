using SIE.Barcodes.Barcodes.ViewModels;
using SIE.Barcodes.Panels;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Web.Barcodes.Barcodes.Commands;
using SIE.Web.Command;
using SIE.Web.Common.Prints;
using System;
using System.Linq;

namespace SIE.Web.Barcodes.Panels.Commands
{
    /// <summary>
    /// 拼板码补打命令
    /// </summary>
    [JsCommand("SIE.Web.Barcodes.Panels.Commands.ReprintCommand")]
    public class ReprintCommand : ListViewCommand
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
            var entity = args.Data.ToJsonObject<ReprintDataModel>();
            var panelList = RT.Service.Resolve<PanelController>().GetPanelsByIds(entity.BarCodeIds.ToList());
            var template = RF.GetById<PrintTemplate>(entity.TemplateId);
            if (template == null)
                throw new ValidationException("打印模板不能为空".L10N());
            if (template.EntityType != typeof(PanelPrintable).GetQualifiedName())
                throw new ValidationException("打印模板错误，请配置【拼板码】类型的模板！".L10N());
            RT.Service.Resolve<PanelController>().ReprintPanel(panelList, entity.Reason, entity.Times);
            rstPrint.Type = template.Type;
            rstPrint.Url = PrintPanels(template, panelList);
            return rstPrint;
        }

        /// <summary>
        /// 补打拼板码
        /// </summary>
        private string PrintPanels(PrintTemplate template, EntityList<Panel> panels)
        {
            var report = ReportFactory.Current.GetReportByExtension(template.Type);
            var printable = new PanelPrintable();
            return report.PrintProcess(printable, template.Id, template.Content, () =>
            {
                return panels;
            });
        }
    }
}
