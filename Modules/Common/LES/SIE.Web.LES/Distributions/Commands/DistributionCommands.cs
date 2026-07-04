using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.LES.Distributions;
using SIE.LES.Distributions.Printables;
using SIE.Warehouses;
using SIE.Web.Command;
using SIE.Web.Common.Prints;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.LES.Distributions.Commands
{
    #region DistributionSettingDeleteCommand 删除命令
    /// <summary>
    /// 删除命令
    /// </summary>
    public class DistributionSettingDeleteCommand : ViewCommand
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

    #endregion

    /// <summary>
    /// 强制完成
    /// </summary>
    public class CancelDistributionCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行
        /// </summary>               
        protected override object Excute(double[] args, string scope)
        {            
            RT.Service.Resolve<DistributionController>().CancelDistribution(args.ToList());
            return true;
        }
    }

    /// <summary>
    /// 打印单据
    /// </summary>
    public class PrintBillDistributionCommand : ViewCommand<PrintDatas>
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">打印数据</param>
        /// <param name="scope">scope</param>
        /// <returns>object</returns>
        protected override object Excute(PrintDatas args, string scope)
        {

            var billTemplateId = args.BillTemplateId;
            List<double> billIds = args.BillIdList.ToList();
            var bills = RT.Service.Resolve<DistributionController>().GetDistributions(billIds, new EagerLoadOptions().LoadWithViewProperty());
            if (bills.Count <= 0)
            {
                throw new ValidationException("未选择打印标签数据".L10N());
            }

            // 1.获取打印模板
            PrintTemplate template = RF.GetById<PrintTemplate>(billTemplateId);

            //2.根据类型获取报表处理对像
            var report = ReportFactory.Current.GetReportByExtension(template.Type);

            //4.创建实体打印对像 如果清楚实体打印对像自己NEW 一个出来也行
            var printable = new DistributionBillPrintable();
            var printData = new PrintDataCommon();
            //5.调用打印处理函数返回打印模板BASE64字符串到前台，用于传输到打印预览页面
            printData.Url = report.PrintProcess(printable, template.Id, template.Content, () =>
            {
                List<Distribution> printData = new List<Distribution>();
                printData.AddRange(bills);

                return printData;
            });
            printData.Type = template.Type;
            return printData;
        }
    }

    /// <summary>
    /// 编辑
    /// </summary>
    public class EditDistributionSettingCommand : ViewCommand
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
}
