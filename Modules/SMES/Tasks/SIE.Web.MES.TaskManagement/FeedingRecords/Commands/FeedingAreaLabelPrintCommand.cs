using SIE.Barcodes.Barcodes.ViewModels;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.MES.DataBarcode;
using SIE.MES.TaskManagement.FeedingRecords;
using SIE.Packages.ItemLabels;
using SIE.Web.Command;
using SIE.Web.Common.Prints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.FeedingRecords.Commands
{
    /// <summary>
    /// 打印
    /// </summary>
    public class FeedingAreaLabelPrintCommand : ListViewCommand
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
                var entity = args.Data.ToJsonObject<BatchReprintDataModel>();
                var template = RF.GetById<PrintTemplate>(entity.TemplateId);
                Check.NotNull(template, nameof(PrintTemplate));

                //if (template.EntityType != typeof(WipBatchPrintable).GetQualifiedName())
                //{
                //    throw new ValidationException("打印模板的类型必须是生产批次".L10N());
                //}

                rstPrint.Type = template.Type;

                var wipBatchs = RT.Service.Resolve<FeedingAreaController>().GetFeedingAreaPrintDatas(entity.Ids);
                var report = ReportFactory.Current.GetReportByExtension(template.Type);

                var printable = new FeedingAreaPrintable();

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
        public class BatchReprintDataModel
        {
            /// <summary>
            /// 选中条码Id列表
            /// </summary>
            public List<double> Ids { get; set; }

            /// <summary>
            /// 打印模板Id
            /// </summary>
            public double TemplateId { get; set; }
        }
    }
}
