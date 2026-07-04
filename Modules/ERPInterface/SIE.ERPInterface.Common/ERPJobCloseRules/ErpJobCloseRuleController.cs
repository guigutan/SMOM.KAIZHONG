using SIE.Common.Schdules;
using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Common.ERPJobCloseRules
{
    /// <summary>
    /// Erp控制器
    /// </summary>
    public class ErpJobCloseRuleController : DomainController
    {
        /// <summary>
        /// 保存前校验
        /// </summary>
        /// <param name="entityList">关闭事务</param>
        public virtual void ValidateBeforeSave(EntityList<ErpJobCloseRule> entityList)
        {
            if (entityList.Any(p => p.During.IsNullOrEmpty()))
            {
                throw new ValidationException("期间不能为空".L10N());
            }
            if (entityList.Any(p => p.StartTime == null))
            {
                throw new ValidationException("交易期关闭开始时间不能为空".L10N());
            }
            if (entityList.Any(p => p.EndTime == null))
            {
                throw new ValidationException("交易期关闭结束时间不能为空".L10N());
            }
            if (entityList.Any(p => p.StartTime > p.EndTime))
            {
                throw new ValidationException("交易期关闭开始时间不能小于结束时间".L10N());
            }
        }

        /// <summary>
        /// 判断是否在关闭期
        /// </summary>
        public virtual bool ValidateInCloseTime()
        {
            var now = RF.Find<JobConfig>().GetDbTime();
            var i = Query<ErpJobCloseRule>().Where(p => p.StartTime <= now).Where(p => p.EndTime >= now).Count() > 0;
            return i;
        }
    }
}
