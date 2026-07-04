using SIE.Common.Import;
using SIE.Equipments.EquipmentCards;
using SIE.Web.Common.Import.Commands;
using System.Collections.Generic;

namespace SIE.Web.Equipments.EquipmentCards.Commands
{
    /// <summary>
    /// 导入设备立卡
    /// </summary>
    public class EquipmentCardImportCommand : ImportExcelCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="batch"></param>
        protected override void OnSave(IList<RowData> batch)
        {
            if (batch == null)
            {
                return;
            }
            var ImpMesResultList = RT.Service.Resolve<EquipmentCardController>().ImportOnSave(batch);
            importResult.MessageList.AddRange(ImpMesResultList);
        }
    }
}