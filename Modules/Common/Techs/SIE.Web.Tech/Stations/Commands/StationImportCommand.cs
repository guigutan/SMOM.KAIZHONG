using SIE.Common.ImportHelper;
using SIE.Tech.Stations.ImportStations;
using SIE.Web.Command;
using SIE.Web.Common.Import.Commands;
using System;
using System.Data;


namespace SIE.Web.Tech.Stations.Commands
{
    /// <summary>
    /// 工位导入
    /// </summary>
    [JsCommand("SIE.Web.Tech.Stations.Commands.StationImportCommand")]
    class StationImportCommand : ImportCommandBase
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
        /// <returns>工位物料导入处理类</returns>
        protected override Type GetImportHandleType()
        {
            return typeof(ImportStationHandle);
        }
    }
}
