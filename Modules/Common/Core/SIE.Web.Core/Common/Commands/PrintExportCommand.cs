using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Web.Command;
using SIE.Web.Common.Prints;
using SIE.Web.Common.Prints.Commands;
using System;
using System.Linq;

namespace SIE.Web.Core.Common.Commands
{
    /// <summary>
    /// 导出报告命令
    /// </summary>
    public class PrintExportCommand : PrintPreviewCommand
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<PrintTemplate>();

            // 1.获取打印模板
            var template = AppRuntime.Service.Resolve<PrintsController>().GetPrintTemplate(Convert.ToDouble(data.Id)) as PrintTemplate;
            //if (template.Type != ".sied")
            //{
            //    throw new ValidationException("打印模板类型必须是[.sied]文件格式！".L10nFormat(template.EntityType));
            //}
            //2.根据类型获取报表处理对像
            var report = ReportFactory.Current.GetReportByExtension(template.Type);

            //3.获取打印实体对像
            var printableType = Type.GetType(template.EntityType);
            if (printableType == null)
                throw new ValidationException("不存在实体类型[{0}]".L10nFormat(template.EntityType));
            //4.创建实体打印对像 如果清楚实体打印对像自己NEW 一个出来也行
            var printable = Activator.CreateInstance(printableType) as IPrintable;
            if (printable == null)
                throw new ValidationException("创建实体类型[{0}]失败！".L10nFormat(template.EntityType));

            var datas = GetDatas(args);

            //5.调用打印处理函数返回打印模板BASE64字符串到前台，用于传输到打印预览页面
            return report.PrintProcess(printable, template.Id, template.Content, () =>
            {
                return datas.OfType<Entity>();
            });
        }
    }
}
