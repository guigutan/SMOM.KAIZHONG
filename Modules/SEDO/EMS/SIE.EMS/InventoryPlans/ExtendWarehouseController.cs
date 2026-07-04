using SIE.Domain;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EMS.InventoryPlans
{
    public class ExtendWarehouseController : DomainController
    {
        /// <summary>
        /// 获取仓库
        /// </summary>
        /// <param name="criteria">仓库查询实体</param>
        /// <returns>仓库集合</returns>
        public virtual EntityList<SIE.EMS.InventoryPlans.Warehouse> GetWarehouseData(WarehouseCriteria criteria)
        {
            var query = Query<Warehouse>();
            if (criteria != null)
            {
                if (!string.IsNullOrEmpty(criteria.Code))
                    query.Where(p => p.Code.Contains(criteria.Code));
                if (!string.IsNullOrEmpty(criteria.Name))
                    query.Where(p => p.Name.Contains(criteria.Name));
                if (criteria.LibraryType.HasValue)
                    query.Where(p => p.LibraryType == criteria.LibraryType.Value);
                if (criteria.Category.IsNotEmpty())
                    query.Where(p => p.Category.Contains(criteria.Category));
                if (criteria.IsFrozen.HasValue)
                    query.Where(p => p.IsFrozen == criteria.IsFrozen.Value);
                if (criteria.State.HasValue)
                    query.Where(p => p.State == criteria.State.Value);
                if (criteria.IsLine.HasValue)
                {
                    query.Where(p => p.IsLineWarehouse == criteria.IsLine.Value);
                }
                if (criteria.IsEmployeeWarehouse)
                    query.Exists<WarehouseEmployee>((p, e) => e.Where(t => t.WarehouseId == p.Id && t.EmployeeId == RT.IdentityId));

                if (criteria.IsAutomated.HasValue)
                {
                    query.Exists<StorageArea>((p, s) => s.Where(t => t.WarehouseId == p.Id && t.IsAutomatedArea == criteria.IsAutomated.Value));
                }
            }

            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();
            query.OrderBy(criteria.OrderInfoList);
            return query.ToList(criteria.PagingInfo, elo);
        }
    }
}
