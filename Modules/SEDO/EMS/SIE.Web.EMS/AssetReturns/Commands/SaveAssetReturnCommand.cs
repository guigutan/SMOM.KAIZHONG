using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.AssetReturns;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.AssetReturns.Commands
{
    /// <summary>
    /// 保存命令
    /// </summary>
    public class SaveAssetReturnCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存操作
        /// </summary>
        /// <param name="entity">归还单实体</param>
        protected override void DoSave(Entity entity)
        {
            var assetReturn = entity as AssetReturn;

            if (assetReturn == null)
            {
                throw new ValidationException("保存数据异常，请重新尝试！".L10N());
            }

            if (assetReturn.AssetRequisition.AssetObject == SIE.EMS.Enums.AssetObject.Equipment)
            {
                RT.Service.Resolve<AssetReturnController>().SaveAssetReturnEquipment(assetReturn);
            }

            if (assetReturn.AssetRequisition.AssetObject == SIE.EMS.Enums.AssetObject.Fixture)
            {
                RT.Service.Resolve<AssetReturnController>().SaveAssetReturnFixture(assetReturn);
            }
        }
    }
}
