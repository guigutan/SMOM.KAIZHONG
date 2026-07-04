using SIE.Common.Import;
using SIE.EMS.Purchases.SparePartReceives;
using SIE.Web.Command;
using SIE.Web.Common.Import.Commands;
using System.Collections.Generic;

namespace SIE.Web.EMS.Purchases.SparePartReceives.Commands
{
    /// <summary>
    /// 备件明细-导入
    /// </summary>
    [JsCommand("SIE.Web.EMS.Purchases.SparePartReceives.Commands.ImportLotSnCommand")]
    public class ImportLotSnCommand : ImportExcelCommand
    {
        /// <summary>
        /// 数据保存
        /// </summary>
        /// <param name="batch">数据</param>
        protected override void OnSave(IList<RowData> batch)
        {
            importResult.MessageList.AddRange(RT.Service.Resolve<SparePartReceiveController>().ImportOnSave(batch));
        }
    }
}
