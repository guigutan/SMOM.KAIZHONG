using SIE.Web.Command;
using System;
using SIE.Dock.DockQueues;
using SIE.Dock.DockQueues.Service;
using System.Linq;
using SIE.Dock.Datas;
using SIE.Dock.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.Web.Common.Prints;
using SIE.Dock.Printables;
using SIE.Warehouses;

namespace SIE.Web.Dock.DockQueues.Commands
{
    /// <summary>
    /// 现场取号命令
    /// </summary>
    public class AddSceneDockQueueCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var dockQueue = args.Data.ToJsonObject<DockQueue>();
            dockQueue.No = RT.Service.Resolve<DockQueueService>().GetDockQueueNo();
            dockQueue.TakeNoWay = TakeNoWay.Scene;

            return dockQueue;
        }
    }

    /// <summary>
    /// 预约取号命令
    /// </summary>
    public class AddAppointDockQueueCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var dockQueue = args.Data.ToJsonObject<DockQueue>();
            dockQueue.No = RT.Service.Resolve<DockQueueService>().GetDockQueueNo();
            dockQueue.TakeNoWay = TakeNoWay.Appoint;
            return dockQueue;
        }
    }

    /// <summary>
    /// 取消
    /// </summary>
    public class CancelDockQueueCommand : ViewCommand<DockAppointData>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(DockAppointData args, string scope)
        {
            RT.Service.Resolve<DockQueueService>().CancelDockQueueData(args.BillIds.ToList(), args.ReasonDesc);
            return true;
        }
    }

    /// <summary>
    /// 分配月台命令
    /// </summary>
    public class AssignDockCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var assignDock = args.Data.ToJsonObject<AssignDockViewModel>();
            RT.Service.Resolve<DockQueueService>().AssignDockData(assignDock.DockQueueId, assignDock.DockMaintainId.Value, assignDock.IsAtOnceAssign);
            return true;
        }
    }

    /// <summary>
    /// 推迟命令
    /// </summary>
    public class DelayDockQueueCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>true</returns>
        protected override object Excute(double[] args, string scope)
        {
            RT.Service.Resolve<DockQueueService>().DelayQueue(args.ToList());
            return true;
        }
    }

    /// <summary>
    /// 签到命令
    /// </summary>
    public class CheckInQueueCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>true</returns>
        protected override object Excute(double[] args, string scope)
        {
            RT.Service.Resolve<DockQueueService>().CheckInQueue(args.ToList());
            return true;
        }
    }

    /// <summary>
    /// 签出命令
    /// </summary>
    public class CheckOutQueueCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>true</returns>
        protected override object Excute(double[] args, string scope)
        {
            RT.Service.Resolve<DockQueueService>().CheckOutQueue(args.ToList());
            return true;
        }
    }

    /// <summary>
    /// 升级命令
    /// </summary>
    public class UpDockQueueCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>true</returns>
        protected override object Excute(double[] args, string scope)
        {
            RT.Service.Resolve<DockQueueService>().UpDockQueueData(args.FirstOrDefault());
            return true;
        }
    }

    /// <summary>
    /// 降级命令
    /// </summary>
    public class DownDockQueueCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>true</returns>
        protected override object Excute(double[] args, string scope)
        {
            RT.Service.Resolve<DockQueueService>().DownDockQueueData(args.FirstOrDefault());
            return true;
        }
    }

    /// <summary>
    /// 打印命令
    /// </summary>
    public class PrintDockQueueCommand : ViewCommand<DockAppointData>
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
            var dockQueues = RT.Service.Resolve<DockQueueService>().GetDockQueueList(billIds);
            if (!dockQueues.Any())
            {
                throw new ValidationException("未选择月台排队打印数据".L10N());
            }

            // 1.获取打印模板
            PrintTemplate template = RF.GetById<PrintTemplate>(printTemplateId);

            //2.根据类型获取报表处理对像
            var report = ReportFactory.Current.GetReportByExtension(template.Type);

            //4.创建实体打印对像 如果清楚实体打印对像自己NEW 一个出来也行
            var printable = new DockQueuePrintable();
            var printData = new PrintDataCommon();
            printData.Type = template.Type;

            //5.调用打印处理函数返回打印模板BASE64字符串到前台，用于传输到打印预览页面
            printData.Url = report.PrintProcess(printable, template.Id, template.Content, () =>
            {
                List<DockQueue> printData = new List<DockQueue>();
                printData.AddRange(dockQueues);
                return printData;
            });

            return printData;
        }
    }
}
