using SIE.Common.ImportHelper;
using SIE.Domain.Validation;
using SIE.TurnoverTools.TurnoverTools.ImportHandle;
using SIE.Web.Command;
using SIE.Web.Common.Import.Commands;
using System;
using System.Data;

namespace SIE.Web.Elec.MES.TurnoverTools.Commands
{
    /// <summary>
    /// 周转工具导入
    /// </summary>
    [JsCommand("SIE.Web.Elec.MES.TurnoverTools.Commands.TurnoverToolImportCommand")]
    public class TurnoverToolImportCommand : ImportCommandBase
    {
        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="importViewArgs">导入视图参数</param>
        /// <param name="scope">使用范围</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ImportViewArgs importViewArgs, string scope)
        {
            if (importViewArgs == null)
            {
                throw new ValidationException("参数：{0}为空".L10nFormat(nameof(importViewArgs)));
            }

            if (importViewArgs.BehaviorName == "Download")
            {
                return DownloadTemplate();
            }
            else
            {
                return ImportData(importViewArgs);
            }
        }

        /// <summary>
        /// 直接从服务器上下载模板
        /// </summary>
        /// <returns></returns>
        public virtual object DownloadTemplate()
        {
            const string templateFileName = "周转工具导入模板.xlsx";
            const string fileRelativePath = "Templates/" + templateFileName;
            return new
            {
                FilePath = fileRelativePath,
                FileName = templateFileName
            };
        }

        /// <summary>
        /// 导入完成
        /// </summary>
        /// <returns>ImportCompleted</returns>
        protected override ImportCompleted GetImportCompleted()
        {
            return (DataRow[] drSuccess, DataRow[] drFailed) =>
            {
            };
        }

        /// <summary>
        /// 获取导入处理类型
        /// </summary>
        /// <returns>Type</returns>
        protected override Type GetImportHandleType()
        {
            return typeof(ImportTurnoverToolHandle);
        }
    }
}
