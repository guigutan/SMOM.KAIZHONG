using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Core.Common.Dao;
using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Linq;

namespace SIE.AbnormalInfo.AbnormalMonitors.Dao
{
    /// <summary>
    /// 异常判定规则 AbnormalDecisionRuleDao DAO
    /// </summary>
    public class AbnormalDecisionRuleDao : BaseDao<AbnormalDecisionRule>
    {
        
        /// <summary>
        /// 指标条件编码
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual string GetInitialzationCodes()
        {
            var config = ConfigService.GetConfig(new IndicatorNoConfig(), typeof(AbnormalDecisionRule));
            if (config == null || config.NumberRule == null)
                throw new ValidationException("未找到指标运算条件编码生成规则,请检查规则配置".L10N());
            return RT.Service.Resolve<NumberRuleController>()
                .GenerateSegment(config.NumberRuleId, 1)
                .FirstOrDefault();
        }

        /// <summary>
        /// 获取判异规则
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual AbnormalDecisionRule GetAbnormalDecisionRule(double id)
        {
            return RF.GetById<AbnormalDecisionRule>(id,new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="abnormalDecisionRule"></param>
        public virtual void Save(AbnormalDecisionRule abnormalDecisionRule)
        {
            RF.Save(abnormalDecisionRule);
        }

    }
}
