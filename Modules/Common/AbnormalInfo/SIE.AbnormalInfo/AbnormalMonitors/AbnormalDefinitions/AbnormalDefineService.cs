using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.NumberRules;
using SIE.Common.Schdules;
using SIE.Core.Common.Service;
using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using SIE.Schedule;
using SIE.Threading;

namespace SIE.AbnormalInfo.AbnormalMonitors
{
    /// <summary>
    /// 
    /// </summary>
    public class AbnormalDefineService : DomainService
    {
        private readonly AbnormalDefineDao _abnormalDefineDao;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abnormalDefineDao"></param>
        public AbnormalDefineService(AbnormalDefineDao abnormalDefineDao)
        {
            _abnormalDefineDao = abnormalDefineDao;
        }

        /// <summary>
        /// 获取异常定义列表
        /// </summary>
        /// <param name="criteria">异常定义查询实体</param>
        /// <returns>异常定义实体列表</returns>
        public virtual EntityList<AbnormalDefine> GetAbnormalDefines(AbnormalDefineCriteria criteria)
        {
            return _abnormalDefineDao.GetAbnormalDefines(criteria);
        }

        /// <summary>
        /// 生成异常定义编码
        /// </summary>
        /// <returns>异常定义编码</returns>
        public virtual string GenerateCode()
        {
            var config = ConfigService.GetConfig<NoConfigValue>(new NoConfig(), typeof(AbnormalDefine));
            if (config == null || config.BacodeRule == null)
                throw new ValidationException("未找到异常定义编码生成规则,请检查规则配置".L10N());
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.BacodeRule.Id, 1).FirstOrDefault();
        }

        /// <summary>
        /// 禁用调度
        /// </summary>
        /// <param name="selectIds"></param>
        public virtual bool Disable(List<double> selectIds)
        {
            var definitions = _abnormalDefineDao.FindMany(c => selectIds.Contains(c.Id));
            if (definitions.IsNotEmpty())
            {
                var jobConfigIds = definitions.Select(c => c.JobConfigId).Cast<object>().ToArray();
                var entityType = typeof(JobConfig);
                var repo = RF.Find(entityType);
                var entityList = repo.GetByIdList(jobConfigIds)
                    .OfType<JobConfig>()
                    .Where(p => p.State == Domain.State.Enable)
                    .ToList();
                RT.Service.Resolve<JobController>().Disable(entityList);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 启用调度
        /// </summary>
        /// <param name="selectIds"></param>
        public virtual bool Enable(List<double> selectIds)
        {
            var definitions = _abnormalDefineDao.FindMany(c => selectIds.Contains(c.Id));
            if (definitions.IsNotEmpty())
            {
                var jobConfigIds = definitions.Select(c => c.JobConfigId).Cast<object>().ToArray();
                var entityType = typeof(JobConfig);
                var repo = RF.Find(entityType);
                var entityList = repo.GetByIdList(jobConfigIds)
                    .OfType<JobConfig>()
                    .Where(p => p.State == Domain.State.Disable)
                    .ToList();
                RT.Service.Resolve<JobController>().Enable(entityList);
                return true;
            }
            return false;
        }


        /// <summary>
        /// 运行调度
        /// </summary>
        /// <param name="defineId"></param>
        public virtual bool Run(double defineId)
        {
            var definition = _abnormalDefineDao.GetById(defineId);
            if (definition != null)
            {
                if (!definition.JobConfigId.HasValue)
                    throw new ValidationException("选择的异常定义未绑定调度任务".L10N());

                AbnormalDefinitionJobParameter jobParameter = new AbnormalDefinitionJobParameter();
                //jobParameter.DefineId = defineId;
                definition.JobConfig.Parameter = Newtonsoft.Json.JsonConvert.SerializeObject(jobParameter);
                AsyncHelper.InvokeSafe(() => RT.Service.Resolve<JobController>().Run(definition.JobConfig));
      
            }
            return true;
        }
    }
}
