using System.Collections.Generic;

namespace SIE.EventMessages.MES.AgvTasks
{
    /// <summary>
    /// 载具接口相关
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultICarriedInterface))]
    public interface ICarried
    {
        /// <summary>
        /// 获取空载具
        /// </summary>
        /// <param name="storageAreaId">货区Id</param>
        /// <param name="carriedModel">载具类型</param>
        /// <returns></returns>
        List<string> GetEmptyCarrieds(double storageAreaId, string carriedModel);
    }

    /// <summary>
    /// 默认实现
    /// </summary>
    public class DefaultICarriedInterface : ICarried
    {
        /// <summary>
        /// 获取空载具
        /// </summary>
        /// <returns></returns>
        public List<string> GetEmptyCarrieds(double storageAreaId, string carriedModel)
        {
            return new List<string>();
        }
    }
}
