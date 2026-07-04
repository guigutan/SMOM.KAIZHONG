using SIE.Common.Prints;
using SIE.DataTrace.TraceMainDatas;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Rbac.Users;
using SIE.Rbac.Users.Configs;
using SIE.Web.Command;
using SIE.Web.Common.Prints;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.DataTrace.TraceMainDatas.Commands
{
    /// <summary>
    /// 打印二维码命令
    /// </summary>
    public class ViewMutiDocumenttCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            //1.0获取预览电子批
            List<double> docIds = Newtonsoft.Json.JsonConvert.DeserializeObject<List<double>>(args.Data);
            if (!docIds.Any()) return null;
            // 1.获取打印模板
            var printTemplates = RT.Service.Resolve<TraceMainDataController>().GetBillPrintTemplates(docIds[0],true);
            var prints = new List<TraceDataPrintTempViewmodel>();
            printTemplates.ForEach(print =>
            {
                prints.Add(GeneralTemplate(print));
            });
            return prints;
        }

        /// <summary>
        /// 生成打印模板
        /// </summary>
        /// <param name="printBind"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        protected TraceDataPrintTempViewmodel GeneralTemplate(TraceDataPrintDataViewmodel printBind) 
        {
            var template = printBind.Template;
            var report = ReportFactory.Current.GetReportByExtension(template.Type);

            //3.获取打印实体对像
            var printableType = Type.GetType(template.EntityType);
            if (printableType == null)
                throw new ValidationException("不存在实体类型[{0}]".L10nFormat(template.EntityType));
            //4.创建实体打印对像 如果清楚实体打印对像自己NEW 一个出来也行
            var printable = Activator.CreateInstance(printableType) as IPrintable;
            if (printable == null)
                throw new ValidationException("创建实体类型[{0}]失败！".L10nFormat(template.EntityType));
            //template.Type = ".sied"返回的是路径,template.Type = ".siedev"返回是流
            var reportPrintProcess = report.PrintProcess(printable, template.Id, template.Content, () =>
            {
                return printBind.ListData as IEnumerable<object>;
            });
            return new TraceDataPrintTempViewmodel { path = reportPrintProcess, Content = template.Content, Type = template.Type };

        }
    }
}
