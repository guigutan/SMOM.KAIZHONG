using SIE.Common.Prints;
using SIE.Dock.Datas;
using SIE.Dock.DockAppoints;
using SIE.Dock.DockAppoints.Service;
using SIE.Dock.DockQueues.Service;
using SIE.Dock.Printables;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Warehouses;
using SIE.Web.Command;
using SIE.Web.Common.Prints;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Dock.DockAppoints.Commands
{
    /// <summary>
    /// 添加命令
    /// </summary>
    public class AddDockAppointCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var dockAppoint = args.Data.ToJsonObject<DockAppoint>();
            dockAppoint.No = RT.Service.Resolve<DockAppointService>().GetDockAppointNo();
            dockAppoint.AppointDate = DateTime.Now;
            return dockAppoint;
        }
    }

    /// <summary>
    /// 修改命令
    /// </summary>
    public class EditDockAppointCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }

    /// <summary>
    /// 取消预约
    /// </summary>
    public class CancelAppointCommand : ViewCommand<DockAppointData>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(DockAppointData args, string scope)
        {
            RT.Service.Resolve<DockAppointService>().CancelDockAppointData(args.BillIds.ToList(), args.ReasonDesc);
            return true;
        }
    }

    /// <summary>
    /// 打印命令
    /// </summary>
    public class PrintDockAppointCommand : ViewCommand<DockAppointData>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">打印数据</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(DockAppointData args, string scope)
        {
            double printTemplateId = args.PrintTemplateId;
            List<double> billIds = args.BillIds.ToList();
            var dockAppoints = RT.Service.Resolve<DockAppointService>().GetDockAppointList(billIds);
            if (!dockAppoints.Any())
            {
                throw new ValidationException("未选择月台预约打印数据".L10N());
            }

            // 1.获取打印模板
            PrintTemplate template = RF.GetById<PrintTemplate>(printTemplateId);

            //2.根据类型获取报表处理对像
            var report = ReportFactory.Current.GetReportByExtension(template.Type);

            //4.创建实体打印对像 如果清楚实体打印对像自己NEW 一个出来也行
            var printable = new DockAppointPrintable();
            var printData = new PrintDataCommon();
            printData.Type = template.Type;
            //5.调用打印处理函数返回打印模板BASE64字符串到前台，用于传输到打印预览页面
            printData.Url = report.PrintProcess(printable, template.Id, template.Content, () =>
            {
                List<DockAppoint> printData = new List<DockAppoint>();
                printData.AddRange(dockAppoints);

                return printData;
            });

            return printData;
        }
    }
}
