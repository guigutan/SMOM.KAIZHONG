using SIE.Common.Import;
using SIE.Domain.Validation;
using SIE.Equipments.EquipModels;
using SIE.Fixtures.Models;
using SIE.Web.Command;
using SIE.Web.Common.Import.Commands;
using System;

namespace SIE.Web.Fixtures.Models.Commands
{
    /// <summary>
    /// 导入
    /// </summary>
    [JsCommand("SIE.Web.Fixtures.Models.Commands.FixtureModelEquipImportCommand")]
    public class FixtureModelEquipImportCommand : ImportExcelCommand
    {
        /// <summary>
        /// 重写导入
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cache"></param>
        protected override void OnRowDataRead(RowData data, CacheData cache)
        {
            base.OnRowDataRead(data, cache);

            var fixtureModelEquipDetail = data.Entity as FixtureModelEquipDetail;
            if (fixtureModelEquipDetail.FixtureModel == null)
            {
                throw new ValidationException("第{0}行的工治具型号不能为空".L10nFormat(data.RowIndex+1));
            }
            if (fixtureModelEquipDetail.EquipModelCode.IsNullOrEmpty())
            {
                throw new ValidationException("第{0}行的设备型号不能为空".L10nFormat(data.RowIndex));
            }
            var equipModel = RT.Service.Resolve<EquipModelController>().GetEquipModelByCode(fixtureModelEquipDetail.EquipModelCode);
            if (equipModel == null)
            {
                throw new ValidationException("第{0}行的设备型号在系统不存在".L10nFormat(data.RowIndex+1));
            }
            fixtureModelEquipDetail.EquipModelId = equipModel.Id;
        }
    }

}
