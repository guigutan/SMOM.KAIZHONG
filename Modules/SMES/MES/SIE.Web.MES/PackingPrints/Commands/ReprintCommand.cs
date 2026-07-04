using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.PackingPrints;
using SIE.Web.Command;
using SIE.Web.Common.Prints;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.MES.PackingPrints.Commands
{
    /// <summary>
    /// 打印命令
    /// </summary>
    [JsCommand("SIE.Web.MES.PackingPrints.Commands.ReprintCommand")]
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
            var entity = args.Data.ToJsonObject<ReprintDataModel>();
            var packingBarcodeList = RT.Service.Resolve<PackingBarcodeController>().GetPackingBarcodesByIds(entity.BarCodeIds.ToList());
            var template = RF.GetById<PrintTemplate>(entity.TemplateId);
            if (template == null)
                throw new ValidationException("打印模板不能为空".L10N());
            if (template.EntityType != typeof(PackingPrintable).GetQualifiedName())
                throw new ValidationException("打印模板错误，请配置【包装号】类型的模板！".L10N());
            RT.Service.Resolve<PackingBarcodeController>().ReprintPackingBarcode(packingBarcodeList, entity.Reason, entity.Times); 
            return PrintPackingBarcodes(template, packingBarcodeList);
        }

        /// <summary>
        /// 补打包装号
        /// </summary>
        /// <param name="template">模板</param>
        /// <param name="packingBarcodes">条码</param>
        /// <returns></returns>
        private object PrintPackingBarcodes(PrintTemplate template, EntityList<PackingBarcode> packingBarcodes)
        {
            var report = ReportFactory.Current.GetReportByExtension(template.Type);
            var printable = new PackingPrintable();
            return report.PrintProcess(printable, template.Id, template.Content, () =>
            {
                return packingBarcodes;
            });
        }

        /// <summary>
        /// 补打数据类
        /// </summary>
        public class ReprintDataModel
        {
            /// <summary>
            /// 选中条码Id列表
            /// </summary>
            public List<double> BarCodeIds { get; set; }

            /// <summary>
            /// 补打原因
            /// </summary>
            public string Reason { get; set; }

            /// <summary>
            /// 补打次数
            /// </summary>
            public int Times { get; set; }

            /// <summary>
            /// 补打模板Id
            /// </summary>
            public double TemplateId { get; set; }
        }
    }
}
