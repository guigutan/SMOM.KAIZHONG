using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.EMS.EquipRepair.ExperienceDepots.Controllers
{
    /// <summary>
    /// 维修经验库控制器
    /// </summary>
    public class ExperienceDepotController : DomainController
    {
        /// <summary>
        /// 获取维修经验库
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<ExperienceDepot> GetExperienceDepotList(ExperienceDepotCriteria criteria)
        {
            var query = Query<ExperienceDepot>();
            if (criteria.EquipAccountCode.IsNotEmpty())
            {
                query.Where(p => p.EquipAccount.Code.Contains(criteria.EquipAccountCode));
            }
            if (criteria.EquipAccountName.IsNotEmpty())
            {
                query.Where(p => p.EquipAccount.Name.Contains(criteria.EquipAccountName));
            }
            if (criteria.EquipModelCode.IsNotEmpty())
            {
                query.Where(p => p.EquipModel.Code.Contains(criteria.EquipModelCode));
            }
            if (criteria.SparePartCode.IsNotEmpty())
            {
                query.Where(p => p.SparePart.SparePartCode.Contains(criteria.SparePartCode));
            }
            if (criteria.SparePartName.IsNotEmpty())
            {
                query.Where(p => p.SparePart.SparePartName.Contains(criteria.SparePartName));
            }
            if (criteria.RepairNo.IsNotEmpty())
            {
                query.Where(p => p.RepairNo.Contains(criteria.RepairNo));
            }
            if (criteria.FaultPhenomenonId.HasValue)
            {
                query.Where(p => p.FaultPhenomenonId == criteria.FaultPhenomenonId);
            }
            if (criteria.EquipLargeFaultId.HasValue)
            {
                query.Where(p => p.EquipLargeFaultId == criteria.EquipLargeFaultId);
            }
            if (criteria.FaultReson.IsNotEmpty())
            {
                query.Where(p => p.FaultReson.Contains(criteria.FaultReson));
            }
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取自动编码No
        /// </summary>
        /// <returns></returns>
        public virtual string GetCode()
        {
            #region 注释
            //业务逻辑代码，此处生成自动编号
            var config = ConfigService.GetConfig<NoConfigValue>(new NoConfig(), typeof(ExperienceDepot));
            if (config == null || config.BacodeRule == null)
                throw new ValidationException("未找到维修经验库编号生成规则,请检查规则配置".L10N());
            var code = RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.BacodeRule.Id, 1).FirstOrDefault();
            #endregion
            return code;
        }

    }
}
