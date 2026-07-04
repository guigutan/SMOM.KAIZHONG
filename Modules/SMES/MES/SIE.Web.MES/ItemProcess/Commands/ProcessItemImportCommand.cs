using SIE.Common.Import;
using SIE.Common.ImportHelper;
using SIE.Core.Common.Controllers;
using SIE.MES.ItemProcess;
using SIE.Web.Common.Import.Commands;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SIE.Web.MES.ItemProcess.Commands
{
    /// <summary>
    /// 导入
    /// </summary>
    public class ProcessItemImportCommand : ImportExcelCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="batch"></param>
        protected override void OnSave(IList<RowData> batch)
        {
            batch.ForEach(p =>
            {
                var entity = p.Entity as ProcessItem;
                entity.State = Domain.State.Enable;
            });
            base.OnSave(batch);
        }
    }

}
