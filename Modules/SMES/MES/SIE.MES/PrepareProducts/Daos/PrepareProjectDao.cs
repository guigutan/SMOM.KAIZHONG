using SIE.Core.Common.Dao;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.PrepareProducts.Daos
{
    /// <summary>
    /// 产前准备项目维护Dao
    /// </summary>
    public class PrepareProjectDao : BaseDao<PrepareProject>
    {
        /// <summary>
        /// 添加时根据编码获取数据库中的编码
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
        public List<string> GetDataBasePreProByCode(List<string> codes)
        {
            var preProjectList = codes.SplitContains(temps =>
            {
                return Query().Where(p => temps.Contains(p.ProCode)).ToList();
            });
            return preProjectList.Select(p => p.ProCode).ToList();
        }

        /// <summary>
        /// 添加时根据编码获取数据库中的编码
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<string> GetDataBasePreProByCode(List<double> ids)
        {
            var preProjectList = ids.SplitContains(temps =>
            {
                return Query().Where(p => !temps.Contains(p.Id)).ToList();
            });
            return preProjectList.Select(p => p.ProCode).ToList();
        }

        /// <summary>
        /// 添加时根据名称获取数据库中的名称
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        public List<string> GetDataBasePreProByName(List<string> names)
        {
            var preProjectList = names.SplitContains(temps =>
            {
                return Query().Where(p => temps.Contains(p.ProName)).ToList();
            });
            return preProjectList.Select(p => p.ProName).ToList();
        }

        /// <summary>
        /// 添加时根据名称获取数据库中的名称
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<string> GetDataBasePreProByName(List<double> ids)
        {
            var preProjectList = ids.SplitContains(temps =>
            {
                return Query().Where(p => !temps.Contains(p.Id)).ToList();
            });
            return preProjectList.Select(p => p.ProName).ToList();
        }

        internal EntityList QueryPrepareProjectList(PrepareProjectCriteria prepareProjectCriteria)
        {
            var query = Query();
            if (prepareProjectCriteria.ProCode.IsNotEmpty())
            {
                query.Where(p => p.ProCode.Contains(prepareProjectCriteria.ProCode));
            }
            if (prepareProjectCriteria.ProName.IsNotEmpty())
            {
                query.Where(p => p.ProName.Contains(prepareProjectCriteria.ProName));
            }
            if (prepareProjectCriteria.ProType.HasValue)
            {
                query.Where(p => p.ProType == prepareProjectCriteria.ProType);
            }
            return query.OrderBy(prepareProjectCriteria.OrderInfoList).ToList(prepareProjectCriteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}
