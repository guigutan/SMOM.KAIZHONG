using SIE.Common.Import;
using SIE.Web.Common.Import.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Tech.Processs.Commands
{
    public class ProcessEmployeeImportCommand : ImportExcelCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="batch"></param>
        protected override void OnSave(IList<RowData> batch)
        {

            base.OnSave(batch);
        }
    }
}
