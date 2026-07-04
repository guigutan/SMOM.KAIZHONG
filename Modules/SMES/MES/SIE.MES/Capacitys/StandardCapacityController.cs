using SIE.Domain;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Capacitys
{
    public partial class StandardCapacityController:DomainController
    {
        /// <summary>
        /// 根据物料ID获取标准产能维护
        /// </summary>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        public virtual EntityList<StandardCapacity> GetStandardCapacityByItemIds( List<double> itemIds)
        {
            var ids = itemIds.ConvertAll(p => (double?)p).ToList();

            return ids.SplitContains(itemId =>
            {
                var query = Query<StandardCapacity>().Where(p => itemId.Contains(p.ItemId))
                    .Exists<Process>((d, p) => p.Where(w => w.Id == d.ProcessId && w.Code == "精加工"));
                return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }
    }
}
