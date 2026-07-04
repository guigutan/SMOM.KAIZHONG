using SIE.Barcodes.Barcodes.ViewModels;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.Resources.Employees;
using SIE.Web.Command;
using SIE.Web.Common.Prints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SIE.Web.Resources.Employees.Commands
{
    /// <summary>
    /// 打印
    /// </summary>
    public class EmployeeLabelPrintCommand : ListViewCommand
    {
        /// <summary>
       /// <summary>
        /// 打印执行方法
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
                var entity = args.Data.ToJsonObject<EmployeeDataModel>();
                var template = RF.GetById<PrintTemplate>(entity.TemplateId);
                Check.NotNull(template, nameof(PrintTemplate));


                rstPrint.Type = template.Type;

                var wipBatchs = RT.Service.Resolve<EmployeeController>().GetEmployeeDatasPrintDatas(entity.Ids);
                var report = ReportFactory.Current.GetReportByExtension(template.Type);

                var printable = new EmployeePrintable();

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
        public class EmployeeDataModel
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
