using SIE.Services;
using System.Collections.Generic;

namespace SIE.EventMessages.EMS.Fixtures
{
    /// <summary>
    /// 工治具验收接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultUpdateFixtureDemand))]
    public interface IFixtureAcceptance
    {

        /// <summary>
        /// 根据工治具编码ID集合获取待验收的合格数信息集合
        /// </summary>
        /// <param name="fixtureEncodeIds"></param>
        /// <returns></returns>
       List<FixtureAcceptanceInfo> GetTobeAcceptanceInfos( List<double> fixtureEncodeIds);
    }

    /// <summary>
    /// 新工单工治具需求清单默认实现
    /// </summary>
    public class DefaultFixtureAcceptance : IFixtureAcceptance
    {
        /// <summary>
        /// 根据工治具编码ID集合获取待验收的合格数信息集合
        /// </summary>
        /// <param name="fixtureEncodeIds"></param>
        /// <returns></returns>
        public List<FixtureAcceptanceInfo> GetTobeAcceptanceInfos(List<double> fixtureEncodeIds)
        {
            return new List<FixtureAcceptanceInfo>();
        }
    }
}
