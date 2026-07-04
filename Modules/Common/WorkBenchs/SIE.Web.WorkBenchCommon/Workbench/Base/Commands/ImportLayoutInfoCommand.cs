using SIE.Common.ImportHelper;
using SIE.Web.Common.Import.Commands;
using SIE.WorkBenchCommon.Workbench.Base;
using System;
using System.Data;

namespace SIE.Web.WorkBenchCommon.Workbench.Base.Commands
{
    /// <summary>
    /// 导入命令
    /// </summary>
    public class ImportLayoutInfoCommand : ImportCommandBase
    {
        /// <summary>
        /// 导入命令
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
        protected override Type GetImportHandleType()
        {
            return typeof(ImportLayoutInfoHandle);
        }
    }
}
