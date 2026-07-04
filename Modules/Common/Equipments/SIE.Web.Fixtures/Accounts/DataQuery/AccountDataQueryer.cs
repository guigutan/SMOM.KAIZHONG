using SIE.Fixtures;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.Fixtures.Accounts.ViewModels;
using SIE.Web.Data;

namespace SIE.Web.Fixtures.Accounts.DataQuery
{
    /// <summary>
    /// 工治具台账查询器
    /// </summary>
    public class AccountDataQueryer : DataQueryer
    {
        /// <summary>
        /// 根据工治具编码Id获取工治具类台账信息
        /// </summary>
        /// <param name="encodeId">工治具编码Id</param>
        /// <returns>工治具类台账信息</returns>
        public AddCodeAccInfo GetAddCodeAccInfo(double encodeId)
        {
            return RT.Service.Resolve<CoreFixtureController>().GetAddCodeAccInfo(encodeId);
        }

        /// <summary>
        /// 根据工治具ID配置项获取工治具ID
        /// </summary>
        /// <returns>工治具类台账信息</returns>
        public string GetFixtureIDAccountNo()
        {
            return RT.Service.Resolve<CoreFixtureController>().GetFixtureIDAccountNo();
        }
        

        /// <summary>
        /// 根据工治具编码Id获取治具状态（ID类）
        /// </summary>
        /// <param name="encodeId">工治具编码Id</param>
        /// <returns>治具状态</returns> 
        public FixtureAccountState GetIdAccountState(double encodeId)
        {
            return RT.Service.Resolve<CoreFixtureController>().GetIdAccountState(encodeId);
        }

        /// <summary>
        /// 根据Id编码获取工治具台账
        /// </summary>
        /// <param name="code">Id编码</param>
        /// <returns>工治具台账</returns>
        public FixtureAccount GetFixtureAccount(string code)
        {
            return RT.Service.Resolve<CoreFixtureController>().GetFixtureAccountByCodeOrRFID(code);
        }

        /// <summary>
        /// 该库位是否被其他工治具关联占用
        /// </summary>
        /// <param name="locationId">库位ID</param>
        /// <returns>已关联工治具ID编码</returns>
        public string IsExistAccountLocation(double locationId)
        {
            return RT.Service.Resolve<CoreFixtureController>().IsExistAccountLocation(locationId);
        }
    }
}
