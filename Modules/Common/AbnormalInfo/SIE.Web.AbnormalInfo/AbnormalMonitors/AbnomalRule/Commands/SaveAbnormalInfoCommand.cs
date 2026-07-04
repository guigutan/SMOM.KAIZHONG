using SIE.Domain;
using SIE.Domain.Validation;
using SIE.AbnormalInfo.AbnormalInfos;
using SIE.Web.Command;
using System;
using SIE.AbnormalInfo.AbnormalMonitors;
using Newtonsoft.Json.Linq;
using SIE.Web.Json;
using SIE.AbnormalInfo.AbnormalMonitors.Service;

namespace SIE.Web.AbnormalInfo.AbnormalMonitors.AbnomalRule.Commands
{
    /// <summary>
    /// 确认异常 保存命令
    /// </summary>
    public class SaveAbnormalRuleCommand : FormSaveCommand
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            var entityMeta = ClientEntities.Find(args.Type);
            var jEntityList = JObject.Parse(args.Data);
            var repository = RepositoryFactory.Find(entityMeta.EntityType);
            EntityList deserializeData = EntityJsonConverter.JsonToEntityList(jEntityList, repository);
            Entity entity = (deserializeData.Count > 0) ? deserializeData[0] : null;
            if (entity != null)
            {
                base.DataList = deserializeData.GetRepository().NewList();
                base.DataList.Add(entity);
            }
            DoSave(entity);
            return entity;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        protected override void DoSave(Entity entity)
        {
            if (entity == null)
            {
                throw new ValidationException("没有数据可以提交。".L10N());
            }
            if (!(entity is AbnormalDecisionRule))
                throw new ValidationException("该数据不是异常信息数据格式。".L10N());

            var abnormal = entity as AbnormalDecisionRule;
            RT.Service.Resolve<AbnormalDecisionRuleService>().Save(abnormal);
        }
    }
}
