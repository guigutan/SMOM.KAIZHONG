using SIE.Domain;
using SIE.AbnormalInfo.AbnormalInfos;
using SIE.Web.Data;

namespace SIE.Web.AbnormalInfo.AbnormalInfos.DataQuery
{
    /// <summary>
    /// 异常信息数据查询
    /// </summary>
    public class AbnormalInfoQueryer: DataQueryer
    {
        /// <summary>
        /// 推送升级设置数据列表
        /// </summary>
        /// <param name="AbnormalInfoCategoryId">异常信息分类id</param>
        /// <returns>推送升级设置列表</returns>
        public EntityList<SenderUpgradeSettings> ChangeSenderUpgrades(double AbnormalInfoCategoryId)
        {
            Check.NotNull(AbnormalInfoCategoryId, nameof(AbnormalInfoCategoryId));
            var SenderUpgrades = RT.Service.Resolve<AbnormalInfoController>().GetSenderUpgrades(AbnormalInfoCategoryId);
            return SenderUpgrades;
        }
    }
}
