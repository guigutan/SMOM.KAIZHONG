using SIE.Common.Import;
using SIE.Items;
using SIE.Items.Items;
using SIE.Web.Common.Import.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Items.Items.Commands
{
    /// <summary>
    /// 导入
    /// </summary>
    public class UnValidFactoryItemImportCommand: ImportExcelCommand
    {
        protected override object Excute(ImportViewArgs importViewArgs, string scope)
        {
            return base.Excute(importViewArgs, scope);
        }

        protected override void OnSave(IList<RowData> batch)
        {
            //var entitys = batch.Select(p => p.Entity as UnValidFactoryItem).ToList();
            //var itemIds = entitys.Select(p => p.ItemId).Distinct().ToList();

            //var list = RT.Service.Resolve<ItemController>().GetUnValidFactoryItemsByItemIds(itemIds);

            //foreach (var data in batch)
            //{
            //    var entity = data.Entity as UnValidFactoryItem;
                
            //}
            base.OnSave(batch);
        }
    }
}
