using SIE.Domain;
using System;

namespace SIE.EMS.Equipments.Models
{
    /// <summary>
    /// 设备型号维修项目的控制器
    /// </summary>
    public class EquipModelRepairProjectController : DomainController
    {
        /// <summary>
        /// 设备型号维修项目查询
        /// </summary>
        /// <param name="equipModelRepairProjectCriteria"></param>
        /// <returns></returns>        
        public virtual EntityList GetEquipModelRepairProjectsByCriteria(EquipModelRepairProjectCriteria equipModelRepairProjectCriteria)
        {
            var query = Query<EquipModelRepairProject>();

            if (!equipModelRepairProjectCriteria.ProjectName.IsNullOrEmpty())
            {
                query.Where(x => x.ProjectDetail.Name.Contains(equipModelRepairProjectCriteria.ProjectName));
            }

            if (!equipModelRepairProjectCriteria.Part.IsNullOrEmpty())
            {
                query.Where(x => x.Part.Contains(equipModelRepairProjectCriteria.Part));
            }

            if (equipModelRepairProjectCriteria.EquipModelId.HasValue)
            {
                query.Where(x => x.EquipModelId == equipModelRepairProjectCriteria.EquipModelId);
            }

            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}
