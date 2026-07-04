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
    /// 培训记录导入
    /// </summary>
    [JsCommand("SIE.Web.MES.TeamManagement.SikllAuthentications.Commands.TrainingRecordImportCommand")]
    public class TrainingRecordImportCommand : ImportCommandBase
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
        /// <returns>type</returns>
        protected override Type GetImportHandleType()
        {
            return typeof(ImportTrainingRecordWebHandle);
        }

        /// <summary>
        /// 获取导入模板数据
        /// </summary>
        /// <returns>模板数据</returns>
        protected override List<string> GetImportTempleData()
        {
            return new List<string> { "1001", "张三", "男", "在职", "2019年6月1日 18:00:00", "2019年6月1日 20:00:00", "2", "完成" };
        }
    }
}