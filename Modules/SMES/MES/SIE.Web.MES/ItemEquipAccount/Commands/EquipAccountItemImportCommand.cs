using SIE.Common.Import;
using SIE.Common.ImportHelper;
using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.MES.ItemEquipAccount;
using SIE.Web.Common.Import.Commands;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SIE.Web.MES.ItemEquipAccount.Commands
{
    /// <summary>
    /// 导入
    /// </summary>
    public class EquipAccountItemImportCommand : ImportExcelCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="batch"></param>
        protected override void OnSave(IList<RowData> batch)
        {
            var list = batch.Select(p => p.Entity as EquipAccountItem).ToList();
            var eqCodes = list.Select(p => p.EquipAccountCode).Distinct().ToList();
            var eqList = RT.Service.Resolve<EquipAccountController>().GetEquipAccountsByCode(eqCodes);
            var dels = new List<RowData>();
            foreach (var p in batch)
            {
                var entity = p.Entity as EquipAccountItem;
                entity.EquipAccount = eqList.FirstOrDefault(x => x.Code == entity.EquipAccountCode);
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
                //entity.State = Domain.State.Enable;
            }
            batch = batch.Where(p => !dels.Contains(p)).ToList();
            base.OnSave(batch);
        }
    }

}
