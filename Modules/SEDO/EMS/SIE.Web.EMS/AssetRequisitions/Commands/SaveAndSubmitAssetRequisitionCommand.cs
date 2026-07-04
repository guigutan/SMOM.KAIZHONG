using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.AssetRequisitions;
using SIE.Web.Command;
using System;

namespace SIE.Web.EMS.AssetRequisitions.Commands
{
    /// <summary>
    /// 保存命令
    /// </summary>
    public class SaveAndSubmitAssetRequisitionCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存操作
        /// </summary>
        /// <param name="entity">发放单实体</param>
        protected override void DoSave(Entity entity)
        {
            var assetRequisition = entity as AssetRequisition;

            if (assetRequisition == null)
            {
                throw new ValidationException("保存数据异常，请重新尝试！".L10N());
            }

            RT.Service.Resolve<AssetRequisitionController>().SaveAndSumbitAssetRequisitions(assetRequisition);
        }
    }
}
