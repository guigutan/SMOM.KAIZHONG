using SIE.Common.ImportHelper;
using SIE.MES.TeamManagement.SikllAuthentications.Import;
using SIE.Web.Command;
using SIE.Web.Common.Import.Commands;
using System;
using System.Collections.Generic;
using System.Data;

namespace SIE.Web.MES.TeamManagement.SikllAuthentications.Commands
{
    /// <summary>
    /// 考试结果导入
    /// </summary>
    [JsCommand("SIE.Web.MES.TeamManagement.SikllAuthentications.Commands.ExamResultImportCommand")]
    public class ExamResultImportCommand : ImportCommandBase
    {
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
        /// <returns>类型</returns>
        protected override Type GetImportHandleType()
        {
            return typeof(ImportExamResultWebHandle);
        }

        /// <summary>
        /// 获取导入模板数据
        /// </summary>
        /// <returns>模板数据</returns>
        protected override List<string> GetImportTempleData()
        {
            return new List<string> { "1001", "张三", "男", "在职", "60", "2019年6月1日 20:00:00", "及格" };
        }
    }
}