using SIE.Barcodes.Barcodes.ViewModels;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.MES.DataBarcode;
using SIE.MES.ItemEquipAccount;
using SIE.Packages.ItemLabels;
using SIE.Web.Command;
using SIE.Web.Common.Prints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ItemEquipAccount.Commands
{
    /// <summary>
    /// 打印
    /// </summary>
    public class EquipAccountItemLabelPrintCommand : ListViewCommand
    {
        /// <summary>
        /// 打印命令执行方法
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>打印结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var rstPrint = new RstBarcodePrint();
            rstPrint.ErrMsg = string.Empty;

            try
            {
                var entity = args.Data.ToJsonObject<PrintDataModel>();
                var template = RF.GetById<PrintTemplate>(entity.TemplateId);
                Check.NotNull(template, nameof(PrintTemplate));

                rstPrint.Type = template.Type;

                var wipBatchs = RT.Service.Resolve<EquipAccountItemController>().GetEquipAccountItemPrintDatas(entity.Ids);
                var report = ReportFactory.Current.GetReportByExtension(template.Type);

                var printable = new EquipAccountItemPrintable();

                rstPrint.Url = report.PrintProcess(printable, template.Id, template.Content, () =>
                {
                    return wipBatchs;
                });
            }
            catch (Exception exc)
            {
                rstPrint.ErrMsg = exc.Message;
            }

            return rstPrint;
        }


        /// <summary>
        /// 打印数据类
        /// </summary>
        public class PrintDataModel
        {
            /// <summary>
            /// 选中条码Id列表
            /// </summary>
            public List<double> Ids { get; set; }

            /// <summary>
            /// 补打模板Id
            /// </summary>
            public double TemplateId { get; set; }
        }
    }
}
