using SIE.Barcodes;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Web.Barcodes.Utils;
using SIE.Web.Command;
using System.Collections.Generic;
using System.Linq;
using System;
using SIE.Barcodes.Printables;

namespace SIE.Web.Barcodes.Barcodes.Commands
{
    /// <summary>
    /// 条码补打
    /// </summary>
    [JsCommand("SIE.Web.Barcodes.ReprintCommand")]
    public class ReprintCommand : ListViewCommand
    {
        /// <summary>
        /// 补打命令执行方法
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>补打结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var entity = args.Data.ToJsonObject<ReprintDataModel>();
            string errMsg = string.Empty;
            PrintHelper helper = new PrintHelper();
            List<double> barcodeIds = entity.BarCodeIds.ToList();
            var barcodeList = RT.Service.Resolve<BarcodeController>().GetBarcodesByIds(barcodeIds);
            var template = RF.GetById<PrintTemplate>(entity.TemplateId);
            if (template.EntityType != typeof(BarcodePrintable).GetQualifiedName())
                throw new ValidationException("打印模板错误，请配置【条码】类型的模板！".L10N());
            return helper.PrintBarcodes(barcodeList, template, string.Empty, (short)entity.Times, () =>
            {
                errMsg = RT.Service.Resolve<BarcodeController>().ReprintBarcode(barcodeList, BarcodeLogType.Remedy, entity.Reason, entity.Times);
            });
        }
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
