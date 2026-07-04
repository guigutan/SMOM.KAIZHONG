using SIE.TurnoverTools.TurnoverTools.ImportHandle;
using SIE.Web.Command;
using System;

namespace SIE.Web.Elec.MES.TurnoverTools.Commands
{
    /// <summary>
    /// 周转工具型号维护导入
    /// </summary>
    [JsCommand("SIE.Web.Elec.MES.TurnoverTools.Commands.TurnoverToolModelImportCommand")]
    public class TurnoverToolModelImportCommand : TurnoverToolImportCommand
    {
        /// <summary>
        /// 直接从服务器上下载模板
        /// </summary>
        /// <returns>模板信息</returns>
        public override object DownloadTemplate()
        {
            const string templateFileName = "周转工具型号维护导入模板.xlsx";
            const string fileRelativePath = "Templates/" + templateFileName;
            return new
            {
                FilePath = fileRelativePath,
                FileName = templateFileName
            };
        }

        /// <summary>
        /// 获取导入处理类型
        /// </summary>
        /// <returns>Type</returns>
        protected override Type GetImportHandleType()
        {
            return typeof(ImportTurnoverToolModelHandle);
        }
    }
}
