using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Kit.APS.FactoryPlanQtys
{
    /// <summary>
    /// 工厂计划配置数配置池
    /// </summary>
    public class FactoryPlanQtyPool
    {
        #region 属性
        /// <summary>
        ///  key0:工厂Id,value:限制值
        /// </summary>
        public Dictionary<double, FactoryPlanQty> DicPatternAmount { get; set; }

        /// <summary>
        /// 工厂计划配置数控制器
        /// </summary>
        protected FactoryPlanQtyController FactoryPlanQtyCtrl { get; set; }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public FactoryPlanQtyPool()
        {
            FactoryPlanQtyCtrl = RT.Service.Resolve<FactoryPlanQtyController>();
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public virtual void Load()
        {
            EntityList<FactoryPlanQty> factoryplanqtyList = FactoryPlanQtyCtrl.GetFactoryPlanQtyList();
            DicPatternAmount = factoryplanqtyList.GroupBy(a => a.FactoryId ?? -1).ToDictionary(a => a.Key, a => a.FirstOrDefault());
        }

        /// <summary>
        /// 根据工厂获取限制值
        /// </summary>
        /// <param name="factoryId">工厂Id</param>
        /// <returns>返回限制值</returns>
        public virtual decimal GetWorkCeil(double factoryId)
        {
            FactoryPlanQty factoryplan = null;
            factoryId = GetValidateFacotryId(factoryId);
            if (DicPatternAmount.TryGetValue(factoryId, out factoryplan))
            {
                return factoryplan.WorkCeil;
            }
            return 0;
        }

        /// <summary>
        /// 获取优先的工厂Id，如果指定工厂、指定日期内有维护款数限制，则返回指定工厂。否则返回-1
        /// </summary>
        /// <param name="factoryId">工厂Id</param>
        /// <returns>返回有效工厂Id</returns>
        public virtual double GetValidateFacotryId(double factoryId)
        {
            if (factoryId != -1 && (!DicPatternAmount.ContainsKey(factoryId)))
                factoryId = -1;
            return factoryId;
        }
    }
}
