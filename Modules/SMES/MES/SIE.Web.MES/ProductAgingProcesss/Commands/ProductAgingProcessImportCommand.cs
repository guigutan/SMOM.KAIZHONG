using SIE.Common.Import;
using SIE.Items;
using SIE.MES.ProductAgingProcesss;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Web.Common.Import.Commands;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.MES.ProductAgingProcesss.Commands
{
    /// <summary>
    /// 导入
    /// </summary>
    public class ProductAgingProcessImportCommand : ImportExcelCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="batch"></param>
        protected override void OnSave(IList<RowData> batch)
        {
            var list = batch.Select(p => p.Entity as ProductAgingProcess).ToList();
            var inCodes = list.Select(p => p.ItemCode).Distinct().ToList();
            var pnCodes=list.Select(p=>p.ProcessCode).Distinct().ToList();
            var rnCodes=list.Select(p=>p.ResourceCode).Distinct().ToList();
            var itemList = RT.Service.Resolve<ItemController>().GetItemDrList(inCodes);
            var processList = RT.Service.Resolve<ProcessController>().GetProcessesList(pnCodes);
            var resourceList = RT.Service.Resolve <WipResourceController>().GetWipResourceByCodes(rnCodes);
            batch.ForEach(p =>
            {
                var entity = p.Entity as ProductAgingProcess;
                entity.Item=itemList.FirstOrDefault(x=>x.Code==entity.ItemCode);
                entity.Process = processList.FirstOrDefault(x => x.Code == entity.ProcessCode);
                entity.Resource = resourceList.FirstOrDefault(x => x.Code == entity.ResourceCode);
            });
            base.OnSave(batch);
        }
    }

}
