using SIE.Common.Import;
using SIE.Common.ImportHelper;
using SIE.Core.Common.Controllers;
using SIE.MES.Andon;
using SIE.MES.LineAndon;
using SIE.Resources.Enterprises;
using SIE.Resources.WorkCenters;
using SIE.Web.Common.Import.Commands;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SIE.Web.MES.LineAndon.Commands
{
    /// <summary>
    /// 导入
    /// </summary>
    public class AndonLineImportCommand : ImportExcelCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="batch"></param>
        protected override void OnSave(IList<RowData> batch)
        {
            var list = batch.Select(p => p.Entity as AndonLine).ToList();
            var wcCodes = list.Select(p => p.WorkCenterCode).Distinct().ToList();
            var wcList = RT.Service.Resolve<WorkCenterController>().GetWorkCentersByCode(wcCodes);
            var factoryList = RT.Service.Resolve<EnterpriseController>().GetFactoriesList(null, null);
            var workShopList = RT.Service.Resolve<EnterpriseController>().GetWorkShops(null, null, null);
            var equipAccount = RT.Service.Resolve<AndonLineController>().GetEquipAccounts(null, null);

            var andonCodes = list.Select(p => p.AndonCode).Distinct().ToList();
            var andonUphold = RT.Service.Resolve<AndonUpholdController>().GetAndonUpholds(andonCodes);
            //找出现有的产线与安灯区域数据
            var machineCodes = list.Select(p => p.MachineCode).Distinct().ToList();
            var andonLines = RT.Service.Resolve<AndonLineController>().GetAndonLinesByMachineCodes(machineCodes);

            batch.ForEach(p =>
            {
                var entity = p.Entity as AndonLine;
                var andonLine = andonLines.FirstOrDefault(p => p.MachineCode == entity.MachineCode);
                if (andonLine != null)
                {
                    entity.PersistenceStatus = Domain.PersistenceStatus.Modified;
                    entity.Id = andonLine.Id;
                }
                entity.WorkCenter = wcList.FirstOrDefault(x => x.Code == entity.WorkCenterCode);
                entity.Factory = factoryList.FirstOrDefault(x => x.Code == entity.FactoryCode);
                entity.WorkShop = workShopList.FirstOrDefault(x => x.Code == entity.WorkShopCode);
                entity.Equipment = equipAccount.FirstOrDefault(x => x.Code == entity.EquipmentNo);
                entity.AndonUphold = andonUphold.FirstOrDefault(p => p.AndonCode == entity.AndonCode);
            });
            base.OnSave(batch);
        }
    }

}
