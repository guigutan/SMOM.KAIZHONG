using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Fixtures;
using SIE.Fixtures.FixtureDemands;
using SIE.Web.Command;
using System;

namespace SIE.Web.Fixtures.Demands.Commands
{
    /// <summary>
    /// 保存工治具治具需求清单
    /// </summary>
    [JsCommand("SIE.Web.Fixtures.Demands.Commands.SaveDemandCommand")]
    public class SaveDemandCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        protected override void DoSave(Entity entity)
        {
            var fixDemandInfo = entity as FixtureDemand;
            if (fixDemandInfo != null)
            {
                var msg = RT.Service.Resolve<ElecFixtureController>().SaveFixtureDemandInfo(fixDemandInfo);
                if (!msg.IsNullOrEmpty())
                    throw new ValidationException(msg.L10N());
                return;
            }
            throw new ValidationException("请检查数据".L10N());
        }

    }
}
