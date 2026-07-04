using SIE.Andon.Andons;
using SIE.Common.Import;
using SIE.Web.Common.Import.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Andon.Andons.Commands
{
    public class AndonGroupDtlImportCommand : ImportExcelCommand
    {
        /// <summary>
        /// 父Id
        /// </summary>
        public double parentId { get; set; }

        protected override object Excute(ImportViewArgs importViewArgs, string scope)
        {
            parentId = importViewArgs.SelectedParentId;
            return base.Excute(importViewArgs, scope);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="batch"></param>
        protected override void OnSave(IList<RowData> batch)
        {
            //batch.ForEach(p =>
            //{
            //    var entity = p.Entity as AndonGroupDetail;
            //    entity.AndonGroupId = parentId;//andons.FirstOrDefault(x => x.AndonCode == entity.Andon.AndonCode);
            //});
            base.OnSave(batch);
        }
    }
}
