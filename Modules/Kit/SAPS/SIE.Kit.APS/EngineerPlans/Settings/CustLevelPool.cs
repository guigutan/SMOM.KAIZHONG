using SIE.Domain;
using SIE.Kit.APS.EngineerPlan.Settings;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Kit.APS.EngineerPlans.Settings
{
    /// <summary>
    /// 等级日排产上限
    /// </summary>
    public class CustLevelPool
    {
        #region 属性
        /// <summary>
        ///  key:等级, value:限制值
        /// </summary>
        public List<CustLevel> CustLevelList { get; set; }

        /// <summary>
        /// 等级日排产上限控制器
        /// </summary>
        protected CustLevelController CustLevelCtrl { get; set; }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public CustLevelPool()
        {
            CustLevelCtrl = RT.Service.Resolve<CustLevelController>();
        }

        /// <summary>
        /// 加载等级日排产上限数据
        /// </summary>
        public virtual void Load()
        {
            CustLevelList = CustLevelCtrl.GetCustLevelList().ToList();
        }

        /// <summary>
        /// 根据等级获取数据
        /// </summary>
        /// <returns>返回限制值</returns>
        public virtual List<CustLevel> GetCustLevelList()
        {
            return CustLevelList;
        }
    }
}
