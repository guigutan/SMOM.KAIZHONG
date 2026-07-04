using SIE.Domain;
using SIE.EventMessages;
using SIE.Kit.APS.EngineerPlans.Settings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Kit.APS.EngineerPlan.Settings
{
    /// <summary>
    /// 客户优先级控制器
    /// </summary>
    public class CustLevelSettingController : DomainController, ICustomerCreated
    {
        /// <summary>
        /// 获取客户优先级数据
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns></returns>
        public virtual EntityList<CustLevelSetting> GetCustLevelSettingList(CustLevelSettingCriteria criteria)
        {
            var query = Query<CustLevelSetting>();

            if (criteria.FactoryId > 0)
            {
                query = query.Where(p => p.FactoryId == criteria.FactoryId);
            }

            if (criteria.CustomerId > 0)
            {
                query = query.Where(p => p.CustomerId == criteria.CustomerId);
            }
            if (criteria.CustLevelId > 0)
            {
                query = query.Where(p => p.CustLevelId == criteria.CustLevelId);
            }
            if (criteria.Remerk.IsNotEmpty())
            {
                query = query.Where(p => p.Remerk.Contains(criteria.Remerk));
            }
            query.OrderBy(criteria.OrderInfoList);
            return query.ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        ///获取客户优先级数据
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<CustLevelSetting> GetCustLevelSettingList()
        {
            return Query<CustLevelSetting>().ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 同步创建 客户优先级
        /// </summary>
        /// <param name="CustomerId"></param>
        public virtual void CreateCustLevelSetting(double CustomerId)
        {
            if (DB.Query<CustLevel>().Count() == 0)
                return;

            var lSet = Query<CustLevelSetting>().Where(o => o.CustomerId == CustomerId).FirstOrDefault();
            if (lSet != null)
                return;

            lSet = new CustLevelSetting();
            lSet.CustomerId = CustomerId;
            lSet.CustLevel = Query<CustLevel>().ToList().Last();

            RF.Save(lSet);
        }
    }
}
