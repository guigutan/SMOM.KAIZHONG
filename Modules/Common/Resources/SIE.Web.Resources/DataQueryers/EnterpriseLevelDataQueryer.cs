using SIE.Domain;
using SIE.Resources.Enterprises;

namespace SIE.Web.Resources.DataQueryers
{
    /// <summary>
    /// 数据查询者
    /// </summary>
    public class EnterpriseLevelDataQueryer : Data.DataQueryer
    {
        /// <summary>
        /// 获取企业层级
        /// </summary>
        /// <param name="levelId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public EntityList GetEnterpriseLevel(double levelId, int pageIndex, int pageSize, string keyword)
        {
            var pagingInfo = new PagingInfo(pageIndex, pageSize, true);
            var enterpriseLevelList = new EntityList<EnterpriseLevel>();
            if (levelId > 0)
            {
                enterpriseLevelList = RT.Service.Resolve<EnterpriseController>()
                    .GetEnterpriseLevelsByParentId(pagingInfo, keyword, levelId);
            }

            if (enterpriseLevelList == null || enterpriseLevelList.Count <= 0)
                return new EntityList<EnterpriseLevel>();
            for (var i = 0; i < enterpriseLevelList.Count; i++)
            {
                enterpriseLevelList[i].TreePId = null;
            }

            return enterpriseLevelList;
        }
    }

}