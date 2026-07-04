using SIE.Core.Common.Service;
using SIE.Domain;
using SIE.MES.Validitys.Daos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Validitys.Service
{
    /// <summary>
    /// 有效期标准维护Service
    /// </summary>
    public class ValidityService : DomainService
    {
        private readonly ValidityDao _validityDao;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="validityDao"></param>
        public ValidityService(ValidityDao validityDao)
        {
            _validityDao = validityDao;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList QueryValidityStandards(ValidityStandardCriteria criteria)
        {
            return _validityDao.QueryValidityStandards(criteria);
        }

        /// <summary>
        /// 根据物料Ids获取有效期标准规则
        /// </summary>
        /// <param name="itemIds">物料ids</param>
        /// <param name="ids">数据ids</param>
        /// <returns></returns>
        public virtual EntityList<ValidityStandard> GetValidityStandardByItemIds(List<double> itemIds, List<double> ids)
        {
            return _validityDao.GetValidityStandardByItemIds(itemIds, ids);
        }

        /// <summary>
        /// 根据物料id和扩展属性获取当前时间的有效期
        /// </summary>
        /// <param name="itemId">物料id</param>
        /// <param name="itemExtProp">物料扩展属性</param>
        /// <returns></returns>
        public virtual ValidityStandard GetValidityStandard(double itemId, string itemExtProp)
        {
            return _validityDao.GetValidityStandard(itemId, itemExtProp);
        }
    }
}
