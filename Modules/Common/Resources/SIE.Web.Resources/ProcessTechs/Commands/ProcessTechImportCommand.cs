using SIE.Common.ImportHelper;
using SIE.Resources.ProcessTechs.ImportProcessTech;
using SIE.Web.Command;
using SIE.Web.Common.Import.Commands;
using System;
using System.Collections.Generic;
using System.Data;

namespace SIE.Web.Resources.ProcessTechs.Commands
{
    /// <summary>
    /// 制程工艺导入
    /// </summary>
    [JsCommand("SIE.Web.Resources.ProcessTechs.Commands.ProcessTechImportCommand")]
    public class ProcessTechImportCommand : ImportCommandBase
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
        /// <returns>Type</returns>
        protected override Type GetImportHandleType()
        {
            return typeof(ImportProcessTechHandle);
        }

        /// <summary>
        /// 获取导入模板数据
        /// </summary>
        /// <returns>模板数据</returns>
        protected override List<string> GetImportTempleData()
        {
            return new List<string> { "ZCGY0001", "制程1", "FUEL", "工段1", "是", "800", "100", "1","1" };
        }
    }
}
