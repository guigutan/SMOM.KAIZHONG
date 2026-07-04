using SIE.Common.Schdules;
using System;

namespace SIE.LES.Job.PrepareItems
{
    /// <summary>
    /// 拉式备料调度Job
    /// </summary>
    [Job("拉式备料调度", typeof(JobParameter))]
    public class PrepareItemPullJob : JobBase
    {
        /// <summary>
        /// 拉式备料调度
        /// </summary>
        /// <param name="param">调度参数</param>
        protected override void ExecuteJob(object param)
        {
            AddLog("当前组织[{0}]，当前身份[{1}]".L10nFormat(RT.InvOrg, RT.IdentityId));

            try
            {
                var prepareItemPullResult = RT.Service.Resolve<PrepareItemController>().PullJobCreatePreparation();

                AddLog("【备料模式维护-拉式】累计匹配（{0}）笔（仓库+物料）的数据，".L10nFormat(prepareItemPullResult.DataCount) +
                    "其中满足触发方式的共有（{0}）笔数据，".L10nFormat(prepareItemPullResult.FitDataCount) +
                    "合计生成（{0}）个备料需求，".L10nFormat(prepareItemPullResult.PrepareDataCount) +
                    "共生成（{0}）个备料单。 !".L10nFormat(prepareItemPullResult.StockOrderCount));
            }
            catch (Exception exMsg)
            {
                AddLog("拉式备料执行失败，错误信息: {0}".L10nFormat(exMsg.GetExceptionMessage()));
            }
        }
    }
}
