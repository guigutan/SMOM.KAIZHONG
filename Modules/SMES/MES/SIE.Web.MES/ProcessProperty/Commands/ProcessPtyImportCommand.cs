using SIE.Common.Import;
using SIE.Common.ImportHelper;
using SIE.Equipments.EquipTypes;
using SIE.MES.LineAndon;
using SIE.MES.ProcessProperty;
using SIE.Resources.Enterprises;
using SIE.Resources.WorkCenters;
using SIE.Web.Common.Import.Commands;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ProcessProperty.Commands
{

    /// <summary>
    /// 维护工序属性导入
    /// </summary>
    public class ProcessPtyImportCommand : ImportExcelCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="batch"></param>
        protected override void OnSave(IList<RowData> batch)
        {
           
            var processData = RT.Service.Resolve<ProcessPtyController>().GetProcess();
            batch.ForEach(p =>
            {
                var entity = p.Entity as ProcessPty;
                entity.Process = processData.FirstOrDefault(x => x.Code == entity.ProcessCode);
            });
            base.OnSave(batch);
        }
    }
}
