using SIE.Common.Import;
using SIE.Equipments.EquipAccounts;
using SIE.MES.ItemEquipAccount;
using SIE.Web.Common.Import.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ItemEquipAccount.Commands
{
    /// <summary>
    /// 模具组修改按钮
    /// </summary>
    public class EquipAccountItemImportUniqueCodeCommand: ImportExcelCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="batch"></param>
        protected override void OnSave(IList<RowData> batch)
        {
            var list = batch.Select(p => p.Entity as EquipAccountItem).ToList();
            var eqCodes = list.Where(p => !p.EquipAccountCode.IsNullOrEmpty()).Select(p => p.EquipAccountCode).Distinct().ToList();
            var eqList = RT.Service.Resolve<EquipAccountController>().GetEquipAccountsByCode(eqCodes);
            var eqIdList = eqList.Select(p => p.Id).Distinct().ToList();

            var itemCodes = list.Select(p => p.Item?.Code).Distinct().ToList();
            var processCodes = list.Select(p => p.Process?.Code).Distinct().ToList();
            var equipAccountItems = RT.Service.Resolve<EquipAccountItemController>().GetEquipAccountItemsByItemCodesEquipCodesProcessCodes(itemCodes, eqCodes, processCodes);

            var dels = new List<RowData>();
            foreach (var p in batch)
            {
                var entity = p.Entity as EquipAccountItem;
                var eq = eqList.FirstOrDefault(x => x.Code == entity.EquipAccountCode);

                var eais = equipAccountItems.Where(f => f.ProcessCode == entity.Process?.Code && f.ItemCode == entity.Item?.Code && f.EquipAccountCode == entity.EquipAccountCode && f.UniqueCode == entity.UniqueCode).ToList();
                if (eais.Count > 1)
                {
                    importResult.MessageList.Add(new ImportMessageResult() { Message = "存在多条相同工序[{0}]相同物料[{1}]相同模具组[{2}]相同模具{3},无法识别更新哪一条数据!".L10nFormat(entity.Process?.Code, entity.Item?.Code, entity.UniqueCode, entity.EquipAccountCode), MsgType = ImportMessageType.SaveFail, RowNum = p.RowIndex + 1 });
                    dels.Add(p);
                    continue;
                }
                var equipAccountItem = eais.FirstOrDefault();
                if (equipAccountItem != null)
                {
                    dels.Add(p);
                    continue;
                }
                else
                {
                    eais = equipAccountItems.Where(f => f.ItemCode == entity.Item?.Code && f.EquipAccountCode == entity.EquipAccountCode && f.ProcessCode == entity.Process?.Code).ToList();

                    if (eais.Count > 1)
                    {
                        importResult.MessageList.Add(new ImportMessageResult() { Message = "存在多条相同物料[{0}]相同模具[{1}]相同工序[{2}],无法识别更新哪一条数据!".L10nFormat(entity.Item?.Code, entity.EquipAccountCode, entity.Process?.Code), MsgType = ImportMessageType.SaveFail, RowNum = p.RowIndex + 1 });
                        dels.Add(p);
                        continue;
                    }
                    equipAccountItem = eais.FirstOrDefault();
                }

                if (equipAccountItem != null)
                {
                    entity.PersistenceStatus = Domain.PersistenceStatus.Modified;
                    entity.Id = equipAccountItem.Id;
                }
                entity.EquipAccount = eq;

                if (entity.EquipAccount == null)
                {
                    importResult.MessageList.Add(new ImportMessageResult() { Message = "设备[{0}]没有在设备台账里面!".L10nFormat(entity.EquipAccountCode), MsgType = ImportMessageType.SaveFail, RowNum = p.RowIndex + 1 });
                    dels.Add(p);
                }
                if (entity.EquipAccount != null && entity.EquipAccount.EquipModelTypeName != "模具设备")
                {
                    importResult.MessageList.Add(new ImportMessageResult() { Message = "设备[{0}]类型不是模具设备!".L10nFormat(entity.EquipAccountCode), MsgType = ImportMessageType.SaveFail, RowNum = p.RowIndex + 1 });
                    dels.Add(p);
                }
            }
            batch = batch.Where(p => !dels.Contains(p)).ToList();
            base.OnSave(batch);
        }
    }
}
