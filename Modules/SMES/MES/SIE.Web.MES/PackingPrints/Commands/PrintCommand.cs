using SIE.Barcodes;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.Barcodes;
using SIE.MES.PackingPrints;
using SIE.MES.PackingPrints.ViewModels;
using SIE.Web.Command;
using SIE.Web.Common.Prints;
using System;


namespace SIE.Web.MES.PackingPrints.Commands
{
    /// <summary>
    /// 打印命令
    /// </summary>
    [JsCommand("SIE.Web.MES.PackingPrints.Commands.PrintCommand")]
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
            var model = args.Data.ToJsonObject<PackingBarcodePrintViewModel>();
            ValidatePrint(model);
            var info = new PackingPrinterInfo(model.WorkOrderId, model.NumberRuleId.Value,Convert.ToDouble(model.PackageRuleDetailId), model.TemplateId.Value, model.PrintQty, model.PageCount);
            var result = RT.Service.Resolve<PackingBarcodeController>().PrintPackingBarcodes(info); // 先保存创建的条码
            if (result.Item1.Length > 0)
                throw new ValidationException(result.Item1);
            else 
            {
                return PrintPackingBarcodes(model, result.Item2);
            }  
        }

        /// <summary>
        /// 打印包装号
        /// </summary>
        /// <param name="entity">包装号打印视图模型</param>
        /// <param name="packingBarcodes">生成包装号列表</param>
        private object PrintPackingBarcodes(PackingBarcodePrintViewModel entity, EntityList<PackingBarcode> packingBarcodes)
        {
            var template = entity.Template;
            var report = ReportFactory.Current.GetReportByExtension(template.Type);
            var printable = new PackingPrintable();
            return report.PrintProcess(printable, template.Id, template.Content, () =>
            {
                return packingBarcodes;
            });
        }

        /// <summary>
        /// 验证打印信息
        /// </summary>
        /// <param name="vm">包装号打印视图模型</param>
        private void ValidatePrint(PackingBarcodePrintViewModel vm)
        {
            var curTemplate = RF.GetById<PrintTemplate>(vm.TemplateId);
            if (curTemplate == null)
                throw new EntityNotFoundException(typeof(PrintTemplate), vm.TemplateId);
            if (curTemplate.EntityType != typeof(PackingPrintable).GetQualifiedName())
                throw new ValidationException("打印模板错误，请配置【包装号】类型的模板！".L10N());
            var broken = vm.Validate();
            if (broken.Count > 0)
                throw new ValidationException(broken.ToString());
        }
    }
}
