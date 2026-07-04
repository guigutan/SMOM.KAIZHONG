using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.EMS.InspectionRules
{
    /// <summary>
    /// 检验规程
    /// </summary>
    public class InspectionRulePool
    {
        #region 属性
        /// <summary>
        ///  key:等级, value:限制值
        /// </summary>
        public List<InspectionRule> InspectionRuleList { get; set; }

        /// <summary>
        /// 检验规程控制器
        /// </summary>
        protected InspectionRuleController InspectionRuleCtrl { get; set; }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public InspectionRulePool()
        {
            InspectionRuleCtrl = RT.Service.Resolve<InspectionRuleController>();
        }

        /// <summary>
        /// 加载检验规程数据
        /// </summary>
        public virtual void Load()
        {
            InspectionRuleList = InspectionRuleCtrl.GetInspectionRuleList().ToList();
        }

        /// <summary>
        /// 获取检验规程
        /// </summary>
        /// <param name="ids">检验规程集合</param>
        /// <returns></returns>
        public virtual List<InspectionRule> GetInspectionRuleList(List<double> ids)
        {
            return InspectionRuleList;
        }
    }
}
