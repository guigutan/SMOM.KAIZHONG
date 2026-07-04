using SIE.Common.Import;
using SIE.MES.Andon;
using SIE.MES.LineAndon;
using SIE.Resources.Enterprises;
using SIE.Resources.WorkCenters;
using SIE.Web.Common.Import.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.LineAndon.Commands
{
    public class LineAreaImportCommand: ImportExcelCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="batch"></param>
        protected override void OnSave(IList<RowData> batch)
        {
            var list = batch.Select(p => p.Entity as LineArea).ToList();
            var wcCodes = list.Select(p => p.WorkCenterCode).Distinct().ToList();
            var wcList = RT.Service.Resolve<WorkCenterController>().GetWorkCentersByCode(wcCodes);
            var factoryList = RT.Service.Resolve<EnterpriseController>().GetFactoriesList(null, null);
            var workShopList = RT.Service.Resolve<EnterpriseController>().GetWorkShops(null, null, null);
            var equipAccount = RT.Service.Resolve<AndonLineController>().GetEquipAccounts(null, null);

            var andonCodes = list.Select(p => p.AndonCode).Distinct().ToList();
            var andonUphold = RT.Service.Resolve<AndonUpholdController>().GetAndonUpholds(andonCodes);
            //找出现有的产线与安灯区域数据
            var machineCodes = list.Select(p => p.MachineCode).Distinct().ToList();
            var andonAreas = RT.Service.Resolve<AndonLineController>().GetLineAreasByMachineCodes(machineCodes);

            batch.ForEach(p =>
            {
                var entity = p.Entity as LineArea;
                var andonArea = andonAreas.FirstOrDefault(p => p.MachineCode == entity.MachineCode);
                if (andonArea != null)
                {
                    entity.PersistenceStatus = Domain.PersistenceStatus.Modified;
                    entity.Id = andonArea.Id;
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
