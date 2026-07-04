using SIE.Common.Import;
using SIE.MES.ItemLine;
using SIE.Web.Common.Import.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ItemLine.Commands
{
    public class ProductLineImportProcessCommand : ImportExcelCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="batch"></param>
        protected override void OnSave(IList<RowData> batch)
        {
            var list = batch.Select(p => p.Entity as ProductLine).ToList();

            var itemCodes = list.Select(p => p.Item?.Code).Distinct().ToList();
            var wipResourceCodes = list.Select(p => p.WipResource?.Code).Distinct().ToList();

            var productLines = RT.Service.Resolve<ProductLineController>().GetProductLinesByWipResourceCodesItemCodes(wipResourceCodes, itemCodes);

            var dels = new List<RowData>();
            foreach (var p in batch)
            {
                var entity = p.Entity as ProductLine;
                if (productLines.Where(x => x.ProcessCode == entity.Process?.Code && x.ItemCode == entity.Item?.Code && x.WipResourceId == entity.WipResource?.Id).Count() > 1)
                {
                    importResult.MessageList.Add(new ImportMessageResult() { Message = "存在多条相同工序[{0}]相同产线/机台[{1}]相同物料[{2}],无法识别更新哪一条数据!".L10nFormat(entity.Process?.Code, entity.WipResource?.Code, entity.Item?.Code), MsgType = ImportMessageType.SaveFail, RowNum = p.RowIndex + 1 });
                    dels.Add(p);
                    continue;
                }
                //有一摸一样的数据就直接跳过
                if (productLines.Any(x => x.ProcessCode == entity.Process?.Code && x.ItemCode == entity.Item?.Code && x.WipResourceId == entity.WipResource?.Id))
                {
                    dels.Add(p);
                    continue;
                }
                var pls = productLines.Where(x => x.ItemCode == entity.Item?.Code && x.WipResourceId == entity.WipResource?.Id).ToList();
                if (pls.Count > 1)
                {
                    importResult.MessageList.Add(new ImportMessageResult() { Message = "存在多条相同物料[{0}]相同产线/机台[{1}],无法识别更新哪一条数据!".L10nFormat(entity.Item?.Code, entity.WipResource?.Code), MsgType = ImportMessageType.SaveFail, RowNum = p.RowIndex + 1 });
                    dels.Add(p);
                    continue;
                }
                var productLine = pls.FirstOrDefault();
                if (productLine != null)
                {
                    entity.Id = productLine.Id;
                    entity.PersistenceStatus = Domain.PersistenceStatus.Modified;
                }
            }
            batch = batch.Where(p => !dels.Contains(p)).ToList();
            base.OnSave(batch);
        }
    }
}
