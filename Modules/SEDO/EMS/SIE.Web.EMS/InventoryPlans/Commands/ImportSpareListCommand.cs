using SIE.Common.ImportHelper;
using SIE.EMS.InventoryPlans.ImportHandles;
using SIE.Web.Command;
using SIE.Web.Common.Import.Commands;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.EMS.InventoryPlans.Commands
{
    /// <summary>
    /// 导入备件清单列表
    /// </summary>
    [JsCommand("SIE.Web.EMS.InventoryPlans.Commands.ImportSpareListCommand")]
    public class ImportSpareListCommand : ImportCommandBase
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
            return typeof(ImportSpareListHandle);
        }

    }
}
