using SIE.Domain;
using SIE.EMS.Equipments.Models;
using SIE.Equipments.EquipModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.SpareParts.SparePartTypes
{
    /// <summary>
    /// 备件类型控制器
    /// </summary>
    public partial class SparePartTypeController : DomainController
    {
        /// <summary>
        /// 通过设备类型获取设备型号
        /// </summary>
        /// <param name="equipTypeId">设备类型Id</param>
        /// <param name="keyword">下拉列表过滤条件</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>设备型号列表</returns>
        public virtual EntityList<EquipModel> GetEquipModels(double? equipTypeId, string keyword, PagingInfo pagingInfo)
        {
            var query = Query<EquipModel>();
            if (equipTypeId != null && equipTypeId != 0)
                query = query.Where(p => p.EquipTypeId == equipTypeId);
            if (!keyword.IsNullOrEmpty())
                query = query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));

            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}
