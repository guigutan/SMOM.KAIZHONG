using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Equipments.EquipStatus
{
    public partial class EquipStatusController : DomainController
    {
        /// <summary>
        /// 根据设备编码获取设备状态
        /// </summary>
        /// <param name="equipCode"></param>
        /// <returns></returns>
        public virtual EquipStatus GetEquipStatusByEquipCode(string equipCode)
        {
            var entity = Query<EquipStatus>().Where(p => p.EquipAccount.Code == equipCode).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            return entity;
        }

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<EquipStatus> CriteriaEquipStatus(EquipStatusCriteria criteria)
        {
            var q = Query<EquipStatus>();

            if (!criteria.EquipAccountCode.IsNullOrEmpty())
                q.Where(p => p.EquipAccount.Code.Contains(criteria.EquipAccountCode));
            if (!criteria.EquipAccountName.IsNullOrEmpty())
                q.Where(p => p.EquipAccount.Name.Contains(criteria.EquipAccountName));
            if (criteria.Status != null)
                q.Where(p => p.Status == criteria.Status);
            if (criteria.CreateDate.BeginValue != null)
                q.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue.Value);
            if (criteria.CreateDate.EndValue != null)
                q.Where(p => p.CreateDate <= criteria.CreateDate.EndValue.Value);

            var list = q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }
    }
}
