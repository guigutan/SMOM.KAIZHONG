using SIE.Domain;
using SIE.MES.OnOffDuty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.OrgLevels
{
    public partial class OrgLevelController : DomainController
    {
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="orgLevelCriteria"></param>
        /// <returns></returns>
        public virtual EntityList<OrgLevel> Fetch(OrgLevelCriteria orgLevelCriteria)
        {
            var q = Query<OrgLevel>();

            //组织架构ID
            if (!orgLevelCriteria.OrgCode.IsNullOrEmpty())
            {
                q.Where(p => p.OrgCode.Contains("%" + orgLevelCriteria.OrgCode + "%"));
            }


            //组织架构名称
            if (!orgLevelCriteria.OrgName.IsNullOrEmpty())
            {
                q.Where(p => p.OrgName.Contains("%" + orgLevelCriteria.OrgName + "%"));
            }

            //上级组织
            if (!orgLevelCriteria.ParentLevel.IsNullOrEmpty())
            {
                q.Where(p => p.ParentLevel.Contains("%" + orgLevelCriteria.ParentLevel + "%"));
            }

            //组织层级
            if (!orgLevelCriteria.TheLevel.IsNullOrEmpty())
            {
                q.Where(p => p.TheLevel.Contains("%" + orgLevelCriteria.TheLevel + "%"));
            }



            return q.OrderBy(orgLevelCriteria.OrderInfoList).ToList(orgLevelCriteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
                    
        }




        /// <summary>
        /// 通过orgcode列表获取人员组织架构列表
        /// </summary>
        /// <param name="orgcode">orgcode列表</param>
        /// <returns>人员组织架构列表</returns>
        public virtual EntityList<OrgLevel> GetOrgLevelList(List<string> orgcode)
        {
            return orgcode.SplitContains(pCodes =>
            {
                return Query<OrgLevel>().Where(p => pCodes.Contains(p.OrgCode)).ToList();
            });
        }






    }
}
