using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Fixtures.FixtureTypes;
using SIE.Fixtures.Models;
using System;

namespace SIE.Fixtures
{
    public partial class CoreFixtureController
    {
        /// <summary>
        /// 获取工治具类型
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<SIE.Fixtures.FixtureTypes.FixtureType> GetFixtureTypeList(FixtureTypeCriteria criteria)
        {
            var query = Query<SIE.Fixtures.FixtureTypes.FixtureType>();
            if (criteria.Code.IsNotEmpty())
                query.Where(p => p.Code.Contains(criteria.Code));
            if (criteria.Name.IsNotEmpty())
                query.Where(p => p.Name.Contains(criteria.Name));

            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据名称或编码获取工治具类型列表
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<SIE.Fixtures.FixtureTypes.FixtureType> GetFixtureTypes(PagingInfo pagingInfo,string keyword)
        {
            var query = Query<SIE.Fixtures.FixtureTypes.FixtureType>();
            if (keyword.IsNotEmpty())
                query.Where(p => p.Code.Contains(keyword)|| p.Name.Contains(keyword));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
        /// <summary>
        /// 获取编码规则
        /// </summary>
        /// <returns></returns>

        public virtual string GetNo()
        {
            return RT.Service.Resolve<CommonController>().GetNo<FixtureEncode>("工治具编号");
        }
    }
}
