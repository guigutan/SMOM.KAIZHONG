using SIE.Common.Import;
using SIE.MES.BlueLable;
using SIE.Web.Common.Import.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.BlueLable.Commands
{
    /// <summary>
    /// 导入
    /// </summary>
    public class BlueLableLevelImportCommand : ImportExcelCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="batch"></param>
        protected override void OnSave(IList<RowData> batch)
        {
            var Items = RT.Service.Resolve<BlueLableController>().GetItems();
            batch.ForEach(p => {
                var entity = p.Entity as BlueLableLevel;
                entity.Item=Items.FirstOrDefault(x=>x.Code==entity.ItemCode);
            });
            base.OnSave(batch);
        }
    }
}
