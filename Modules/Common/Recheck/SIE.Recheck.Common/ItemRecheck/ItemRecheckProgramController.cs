using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Linq;

namespace SIE.Recheck.Common.ItemRecheck
{
    /// <summary>
    /// 物料复检方案控制器
    /// </summary>
    public class ItemRecheckProgramController : DomainController
    {
        /// <summary>
        /// 获取物料复检方案
        /// </summary>
        /// <returns>物料复检方案</returns>
        public virtual EntityList<ItemRecheckProgram> GetItemRecheckPrograms(PagingInfo pagingInfo, string keyWord)
        {
            var query = Query<ItemRecheckProgram>().Where(p => p.State == State.Enable);
            if (keyWord.IsNotEmpty())
                query.Where(p => keyWord.Contains(p.Code) || keyWord.Contains(p.Name));
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取物料复检方案编码
        /// </summary>
        /// <returns>物料复检方案编码</returns>
        public virtual string GetItemRecheckProgramCode()
        {
            var config = ConfigService.GetConfig(new NoConfig(), typeof(ItemRecheckProgram));
            if (config == null || config.BacodeRule == null)
                throw new ValidationException("未找到物料复检方案编码生成规则,请检查规则配置".L10N());
            return RT.Service.Resolve<NumberRuleController>()
                .GenerateSegment(config.BacodeRule.Id, 1)
                .FirstOrDefault();
        }
    }
}
