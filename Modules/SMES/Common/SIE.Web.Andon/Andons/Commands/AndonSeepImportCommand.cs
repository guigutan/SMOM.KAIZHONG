using SIE.Andon.Andons;
using SIE.Common.Import;
using SIE.MES.BlueLable;
using SIE.Web.Common.Import.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Andon.Andons.Commands
{
    /// <summary>
    /// 导入
    /// </summary>
    public class AndonSeepImportCommand : ImportExcelCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="batch"></param>
        protected override void OnSave(IList<RowData> batch)
        {
            var andons = RT.Service.Resolve<AndonController>().GetAndons();
            batch.ForEach(p =>
            {
                var entity = p.Entity as AndonSesp;
                entity.AndonLevel = entity.AndonLevel.ToString();
                entity.EmployeeId = entity.EmployeeId;
                entity.AndonUpholdId = entity.AndonUpholdId;
                entity.Andon = andons.FirstOrDefault(x => x.AndonCode == entity.Andon.AndonCode);
            });
            base.OnSave(batch);
        }
    }
}
