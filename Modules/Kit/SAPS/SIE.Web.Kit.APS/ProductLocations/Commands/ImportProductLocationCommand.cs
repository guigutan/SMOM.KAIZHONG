using SIE.Common.ImportHelper;
using SIE.Kit.APS.ProductLocations.ImportProductLocations;
using SIE.Web.Command;
using SIE.Web.Common.Import.Commands;
using System;
using System.Collections.Generic;
using System.Data;

namespace SIE.Web.Kit.APS.ProductLocations.Commands
{
    /// <summary>
    /// 产品定位导入
    /// </summary>
    [JsCommand("SIE.Web.Kit.APS.ProductLocations.Commands.ImportProductLocationCommand")]
    public class ImportProductLocationCommand : ImportCommandBase
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
            return typeof(ImportProductLocationHandle);
        }

        /// <summary>
        /// 获取导入模板数据
        /// </summary>
        /// <returns>模板数据</returns>
        protected override List<string> GetImportTempleData()
        {
            return new List<string> { "S201", "产品类型", "薄板", "1", "2", "" };
        }
    }
 
}
