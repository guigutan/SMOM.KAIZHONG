using SIE.Common.Import;
using SIE.MES.TaskManagement.FeedingRecords;
using SIE.Web.Common.Import.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.FeedingRecords.Commands
{
    public class FeedingAreaItemImportCommand: ImportExcelCommand
    {
        public double parentId { get; set; }

        protected override object Excute(ImportViewArgs importViewArgs, string scope)
        {
            parentId = importViewArgs.SelectedParentId;
            return base.Excute(importViewArgs, scope);
        }

        protected override void OnSave(IList<RowData> batch)
        {
            foreach (var b in batch)
            {
                var entity = b.Entity as FeedingAreaItem;
                entity.FeedingAreaId = parentId;
            }
            base.OnSave(batch);
        }
    }
}
