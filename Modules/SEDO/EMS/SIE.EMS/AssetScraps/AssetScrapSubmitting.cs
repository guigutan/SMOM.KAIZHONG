using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SIE.EMS.AssetScraps
{
    /// <summary>
    /// 资产报废单保存前事件
    /// </summary>
    [System.ComponentModel.DisplayName("资产报废单保存前事件")]
    [System.ComponentModel.Description("资产报废单保存前校验设备台账或工治具ID是否存在于未完结的单据中")]
    public class AssetScrapSubmitting : OnSubmitting<AssetScrap>
    {
        /// <summary>
        /// 资产报废单保存前事件
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        protected override void Invoke(AssetScrap entity, EntitySubmittingEventArgs e)
        {
            if (e == null || entity == null)
            {
                return;
            }
            if (e.Action == SubmitAction.Insert || e.Action == SubmitAction.Update)
            {
                if (entity.AssetObject == Enums.AssetObject.Equipment && entity.AssetScrapEquipmentList.Count>0) 
                {
                    if (entity.AssetScrapEquipmentList.GroupBy(p=>p.EquipAccountId).Where(p=>p.Count()>1).Select(p=> p.Key).Any()) 
                    {
                        throw new ValidationException("同一个【设备编码】只能存在于一个未完结的单据中,请检查".L10N());
                    }

                    if (RT.Service.Resolve<AssetScrapController>().VerifyIsExistNotAuditAssetScrapEquip(entity.AssetScrapEquipmentList))
                    {
                        throw new ValidationException("同一个【设备编码】只能存在于一个未完结的单据中,请检查".L10N());
                    }
                }

                var fixtureIDAccountList = entity.AssetScrapFixtureList.Where(p => p.FixtureAccountId != null).ToList();
                if (entity.AssetObject == Enums.AssetObject.Fixture && fixtureIDAccountList.Count > 0)
                {
                    if (fixtureIDAccountList.GroupBy(p => p.FixtureAccountId).Where(p => p.Count() > 1).Select(p => p.Key).Any())
                    {
                        throw new ValidationException("同一个工治具【序列号】只能存在于一个未完结的单据中,请检查".L10N());
                    }

                    if (RT.Service.Resolve<AssetScrapController>().VerifyIsExistNotAuditAssetScrapFixture(fixtureIDAccountList))
                    {
                        throw new ValidationException("同一个工治具【序列号】只能存在于一个未完结的单据中,请检查".L10N());
                    }
                }
            }
        }
    }
}
