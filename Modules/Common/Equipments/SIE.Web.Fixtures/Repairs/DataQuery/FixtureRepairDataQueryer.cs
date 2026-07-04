using SIE.Fixtures;
using SIE.Fixtures.Repairs.ViewModels;
using SIE.Web.Data;

namespace SIE.Web.Fixtures.Repairs.DataQuery
{
    /// <summary>
    /// 获取工治具报修信息查询器
    /// </summary>
    public class FixtureRepairDataQueryer : DataQueryer
    {

        /// <summary>
        /// 通过工治具台帐Id获取工治具台帐信息
        /// </summary>
        /// <param name="fixtureAccountId">工治具台帐Id</param>
        /// <returns>工治具台帐信息</returns>
        public FixtureRepairDetailInfo GetFixtureRepairDetailInfo(double fixtureAccountId)
        {
            return RT.Service.Resolve<CoreFixtureController>().GetFixtureRepairDetailInfo(fixtureAccountId);
        }
    }
}
