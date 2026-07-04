using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.Web.Command;
using SIE.Web.Common.Prints;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.MES.TaskManagement.Dispatchs.Commands
{
    /// <summary>
    /// 任务单打印
    /// </summary>
    public class PrintDispatchTaskCommand : ViewCommand<PrintDatas>
    {
        /// <summary>
        /// 执行打印
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>打印</returns>
        protected override object Excute(PrintDatas args, string scope)
        {
            var ctl = RT.Service.Resolve<DispatchController>();
            var billTemplateId = args.TaskTemplateId;
            List<double> taskIdList = args.TaskIdList.ToList();
            var tasks = ctl.GetDispatchTasks(taskIdList);
            if (tasks.Count <= 0)
                throw new ValidationException("未选择打印标签数据".L10N());

            // 1.获取打印模板
            PrintTemplate template = RF.GetById<PrintTemplate>(billTemplateId);

            //2.根据类型获取报表处理对像
            var report = ReportFactory.Current.GetReportByExtension(template.Type);

            //4.创建实体打印对像 如果清楚实体打印对像自己NEW 一个出来也行
            var printable = new DispatchTaskBillPrintable();

            //5.调用打印处理函数返回打印模板BASE64字符串到前台，用于传输到打印预览页面
            return report.PrintProcess(printable, template.Id, template.Content, () =>
             {
                 List<DispatchTask> printData = new List<DispatchTask>();
                 printData.AddRange(tasks);

                 return printData;
             });
        }
    }

    /// <summary>
    /// 打印信息
    /// </summary>
    public class PrintDatas
    {
        /// <summary>
        /// 任务单Id列表
        /// </summary>
        public double[] TaskIdList { get; set; }

        /// <summary>
        /// 模板Id
        /// </summary>
        public double TaskTemplateId { get; set; }
    }
}
