using SIE.Domain;

namespace SIE.MES.TeamManagement.RatedItems
{
    /// <summary>
    /// 评分项目控制器
    /// </summary>
    public class RatedItemController : DomainController
    {
        /// <summary>
        /// 获取评分项目分类集合
        /// </summary>
        /// <returns>评分项目分类集合</returns>
        public virtual EntityList<RatedItemCategory> GetRatedItemCategories()
        {
            var ratedItemCategorys = Query<RatedItemCategory>().ToList();
            return ratedItemCategorys;
        }

        /// <summary>
        /// 获取评分项目集合
        /// </summary>
        /// <param name="isSystem">是否系统评分项目</param>
        /// <returns>评分项目集合</returns>
        public virtual EntityList<RatedItem> GetRatedItems(bool? isSystem = null)
        {
            var ratedItems = Query<RatedItem>().Where(x => x.State == State.Enable);
            if (isSystem.HasValue)
            {
                ratedItems = ratedItems.Where(p => p.IsSystem == isSystem);
            }

            return ratedItems.ToList();
        }
    }
}