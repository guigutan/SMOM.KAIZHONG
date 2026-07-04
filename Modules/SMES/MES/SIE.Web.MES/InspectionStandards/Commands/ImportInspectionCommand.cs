using SIE.Common.Import;
using SIE.MES.InspectionStandards;
using SIE.Web.Command;
using SIE.Web.Common.Import.Commands;
using System.Collections.Generic;

namespace SIE.Web.EMS.InventoryTasks.Commands
{
    /// <summary>
    /// 盘点任务设备清单-导入
    /// </summary>
    [JsCommand("SIE.Web.EMS.InventoryTasks.Commands.ImportInspectionCommand")]
    public class ImportInspectionCommand : ImportExcelCommand
    {
        /// <summary>
        /// 数据保存
        /// </summary>
        /// <param name="batch">数据</param>
        protected override void OnSave(IList<RowData> batch)
        {
            importResult.MessageList.AddRange(RT.Service.Resolve<ModelInspectionItemController>().ImportInspection(batch));
        }
    }
}
