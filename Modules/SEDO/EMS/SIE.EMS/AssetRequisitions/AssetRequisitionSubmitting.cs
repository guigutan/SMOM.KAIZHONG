using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SIE.EMS.Equipments;

namespace SIE.EMS.AssetRequisitions
{
    /// <summary>
    /// 资产领用单保存前事件
    /// </summary>
    [System.ComponentModel.DisplayName("资产领用单保存前事件")]
    [System.ComponentModel.Description("资产领用单保存前校验设备编码是否重复，或存在于其他领用单中")]
    public class AssetRequisitionSubmitting : OnSubmitting<AssetRequisition>
    {
        /// <summary>
        /// 资产领用单保存前事件
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Invoke(AssetRequisition entity, EntitySubmittingEventArgs e)
        {
            if (e == null || entity == null)
            {
                return;
            }
            if ((e.Action == SubmitAction.Insert || e.Action == SubmitAction.Update) && entity.AssetRequisitionEquipmentList.Any())
            {
                var assetEquipmentList = entity.AssetRequisitionEquipmentList.Where(p => p.EquipAccountId != null).ToList();
                var equipAccountIdList = assetEquipmentList.Select(p => p.EquipAccountId).Distinct().ToList();

                if (assetEquipmentList.Count != equipAccountIdList.Count)
                {
                    throw new ValidationException("设备清单中的【设备编码】须唯一".L10N());
                }

                var otherEquipmentList = equipAccountIdList.SplitContains(tempEquipAccountIds =>
                {
                    return DB.Query<AssetRequisitionEquipment>().Where(p => tempEquipAccountIds.Contains(p.EquipAccountId) && p.AssetRequisition.ApprovalStatus == SIE.Equipments.Enums.ApprovalStatus.PendingReview && p.AssetRequisitionId != entity.Id).ToList();
                });

                if (otherEquipmentList.Any())
                {
                    throw new ValidationException("设备清单中的【设备编码】已存在于其他待审核领用单的设备清单中".L10N());
                }
            }
        }
    }
}
