using SIE.Common.Schdules;
using SIE.LES.PrepareItems.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.LES.Job.PrepareItems
{

    /// <summary>
    /// 推式备料调度Job
    /// </summary>
    [Job("推式备料调度", typeof(JobParameter))]
    public class PrepareItemPushJob : JobBase
    {
        /// <summary>
        /// 推式备料调度
        /// </summary>
        /// <param name="param">调度参数</param>
        protected override void ExecuteJob(object param)
        {
            AddLog("当前组织[{0}]，当前身份[{1}]".L10nFormat(RT.InvOrg, RT.IdentityId));

            try
            {
                var prepareItemPushResult = RT.Service.Resolve<PrepareItemController>().PushJobCreatePreparation();
                AddLog("推式调度：【备料模式维护-推式】累计匹配（{0}）（资源+物料+触发方式）的数据，".L10nFormat(prepareItemPushResult.DataCount) +
                    "共有（{0}）个发放的工单，".L10nFormat(prepareItemPushResult.WoCount) +
                    "共有（{0}）个（工单+物料+触发方式）满足触发条件，".L10nFormat(prepareItemPushResult.FitDataCount) +
                    "合计生成（{0}）个备料需求".L10nFormat(prepareItemPushResult.PrepareDataCount) +
                    "，共生成（{0}）个备料单。".L10nFormat(prepareItemPushResult.StockOrderCount));
            }
            catch (Exception exMsg)
            {
                AddLog("推式备料调度执行失败，错误信息: {0}".L10nFormat(exMsg.GetExceptionMessage()));
            }
        }
    }
}