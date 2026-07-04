using SIE.Domain;
using SIE.ObjectModel;

namespace SIE.Resources.Enterprises
{
    /// <summary>
    /// 组织层级、组织模型、企业层级、企业模型查询提供者
    /// </summary>
    public class CriteriaProvider : ICriteriaQueryProvider
    {
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="query">标准查询实体</param>
        /// <returns>实体列表</returns>
        public EntityList GetList(CriteriaQuery query)
        {
            if (query == null)
            {
                return new EntityList<Entity>();
            }
            if (query.EntityType == typeof(Enterprise))
            {
                return RT.Service.Resolve<EnterpriseController>().GetEnterprises(query);
            }
            else if (query.EntityType == typeof(Resource))
            {
                return RT.Service.Resolve<EnterpriseController>().GetResources(query);
            }
            else if (query.EntityType == typeof(EnterpriseLevel))
            {
                return RT.Service.Resolve<EnterpriseController>().GetEnterpriseLevels(query);
            }
            else
            {
                return new EntityList<Entity>();
            }
        }
    }
}
