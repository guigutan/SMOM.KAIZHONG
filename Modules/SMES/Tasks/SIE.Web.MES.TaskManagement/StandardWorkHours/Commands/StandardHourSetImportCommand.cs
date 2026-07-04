using SIE.Common.ImportHelper;
using SIE.MES.TaskManagement.StandardWorkHours.ImportHandles;
using SIE.Web.Common.Import.Commands;
using System;
using System.Data;

namespace SIE.Web.MES.TaskManagement.StandardWorkHours.Commands
{
    /// <summary>
    /// 产品标准工时维护导入命令
    /// </summary>
    public class StandardHourSetImportCommand : ImportCommandBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override ImportCompleted GetImportCompleted()
        {
            return (DataRow[] drSuccess, DataRow[] drFailed) =>
            {
            };
        }

        /// <summary>
        /// 获取导入处理类型
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override Type GetImportHandleType()
        {
            return typeof(StandardHourSetImportHandle);
        }

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="importViewArgs">导入视图参数</param>
        /// <param name="scope">使用范围</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ImportViewArgs importViewArgs, string scope)
        {
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
            const string templateFileName = "产品标准工时维护导入模板.xlsx";
            const string fileRelativePath = "Templates/" + templateFileName;

            return new
            {
                FilePath = fileRelativePath,
                FileName = templateFileName
            };
        }
    }
}
