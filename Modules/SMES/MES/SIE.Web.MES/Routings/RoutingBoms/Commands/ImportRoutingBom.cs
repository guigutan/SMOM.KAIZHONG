using SIE.Common.ImportHelper;
using SIE.MES.Routings.RoutingBoms.ImportBoms;
using SIE.Web.Command;
using SIE.Web.Common.Import.Commands;
using System;
using System.Data;

namespace SIE.Web.MES.Routings.RoutingBoms.Commands
{
    /// <summary>
    /// 工艺路线版本（工序bom主表）导入
    /// </summary>
    [JsCommand("SIE.Web.MES.Routings.RoutingBoms.Commands.ImportRoutingBomCommand")]
    public class ImportRoutingBom : ImportCommandBase
    {
        /// <summary>
        /// 导入完成回调方法
        /// </summary>
        /// <returns>导入结果</returns>
        protected override ImportCompleted GetImportCompleted()
        {
            return (DataRow[] drSuccess, DataRow[] drFailed) =>
            {
            };
        }

        /// <summary>
        /// 获取导入处理类型
        /// </summary>
        /// <returns>导入处理类</returns>
        protected override Type GetImportHandleType()
        {
            return typeof(ImportRoutingBomHandle);
        }
    }
}
