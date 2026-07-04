using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Items.KzItemCategorys
{
    public class KzItemCategorysController : DomainController
    {

        /// <summary>
        /// 根据物料Id获取物料与分类关系
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public virtual KzItemCategory GetKzItemCategorieByItemId(double itemId)
        {
            var first = Query<KzItemCategory>().Where(p => p.ItemId == itemId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            return first;
        }
    }
}
