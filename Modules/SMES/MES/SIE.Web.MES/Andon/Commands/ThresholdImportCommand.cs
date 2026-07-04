using SIE.Common.Import;
using SIE.MES.Threshold;
using SIE.Web.Common.Import.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Andon.Commands
{
    public class ThresholdImportCommand: ImportExcelCommand
    {
        protected override void OnSave(IList<RowData> batch)
        {
            var list = batch.Select(p => p.Entity as SIE.MES.Threshold.Threshold).ToList();
            var processCodes = list.Select(p => p.Process.Code).Distinct().ToList();
            var itemCodes = list.Select(p => p.Item.Code).Distinct().ToList();
            var thresholds = RT.Service.Resolve<ThresholdController>().GetThresholdsByCodeProcess(itemCodes, processCodes);
            batch.ForEach(p =>
            {
                var entity = p.Entity as SIE.MES.Threshold.Threshold;
                var threshold = thresholds.FirstOrDefault(p => p.ProcessCode == entity.Process.Code && p.ItemCode == entity.Item.Code);
                if (threshold != null)
                {
                    entity.Id = threshold.Id;
                    entity.PersistenceStatus = Domain.PersistenceStatus.Modified;
                }
            });
            base.OnSave(batch);
        }
    }
}
