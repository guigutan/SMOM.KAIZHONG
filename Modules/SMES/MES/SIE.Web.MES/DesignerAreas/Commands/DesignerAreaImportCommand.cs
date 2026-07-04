using SIE.Common.Import;
using SIE.MES.DesignerAreas;
using SIE.Web.Common.Import.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.DesignerAreas.Commands
{
    /// <summary>
    /// 导入
    /// </summary>
    public class DesignerAreaImportCommand : ImportExcelCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="batch"></param>
        protected override void OnSave(IList<RowData> batch)
        {
            //var Items = RT.Service.Resolve<DesignerAreaController>().GetItems();
            //batch.ForEach(p => {
            //    var entity = p.Entity as DesignerArea;
            //    entity.Item=Items.FirstOrDefault(x=>x.Code==entity.AreaCode);
            //});

            base.OnSave(batch);

        }
    }
}
