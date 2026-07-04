using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SIE.Domain.Validation;
using SIE.Web.Command;
using SIE.Web.Common.Export;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace SIE.Web.AbnormalInfo.AbnormalMonitors.Commands
{
    /// <summary>
    /// 异常任务导出
    /// </summary>
    public class ExportCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            //获取数据。因为涉及到多态转换，所以使用JObject的方式，逐个转换
            var json = JsonConvert.DeserializeObject<JObject>(args.Data);
            Dictionary<string, string> data = json?.ToObject<Dictionary<string, string>>();
            ExportCommandExcelHelper excel = new ExportCommandExcelHelper();
            string Code = json.Value<string>("Code");
            using (MemoryStream ms = excel.ExportData(data))
            {
                byte[] byteArr = ms.ToArray();
                ms.Close();

                return new
                {
                    FileName = $"[{Code}]异常任务导出.xls",
                    FileContent = FileStreamHelper.ExcelToBase64(byteArr, FileType.xls)
                };
            }
        }
    }
}
