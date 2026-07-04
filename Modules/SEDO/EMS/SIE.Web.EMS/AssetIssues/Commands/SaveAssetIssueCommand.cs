using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.AssetIssues;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.AssetIssues.Commands
{
    /// <summary>
    /// 保存命令
    /// </summary>
    public class SaveAssetIssueCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存操作
        /// </summary>
        /// <param name="entity">发放单实体</param>
        protected override void DoSave(Entity entity)
        {
            var assetIssue = entity as AssetIssue;

            if (assetIssue == null) 
            {
                throw new ValidationException("保存数据异常，请重新尝试！".L10N());
            }

            if (assetIssue.AssetRequisition.AssetObject == SIE.EMS.Enums.AssetObject.Equipment) 
            {
                RT.Service.Resolve<AssetIssueController>().SaveAssetIssueEquipment(assetIssue);
            }

            if (assetIssue.AssetRequisition.AssetObject == SIE.EMS.Enums.AssetObject.Fixture)
            {
                RT.Service.Resolve<AssetIssueController>().SaveAssetIssueFixture(assetIssue);
            }
        }
    }
}
