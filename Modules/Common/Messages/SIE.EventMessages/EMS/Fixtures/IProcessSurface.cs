using SIE.Services;

namespace SIE.EventMessages.EMS.Fixtures
{
    /// <summary>
    /// 工单工艺面关联接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultProcessSurface))]
    public interface IProcessSurface
    {
        /// <summary>
        /// 根据工单Id获取工单的工艺面
        /// </summary>
        /// <param name="woId">工单Id</param>
        /// <returns>工单的工艺面</returns>
        int? GetProcessSurface(double woId);
    }

    /// <summary>
    /// 工单工艺面关联默认实现
    /// </summary>
    public class DefaultProcessSurface : IProcessSurface
    {
        /// <summary>
        /// 根据工单Id获取工单的工艺面
        /// </summary>
        /// <param name="woId">工单Id</param>
        /// <returns>工单的工艺面</returns>
        public int? GetProcessSurface(double woId)
        {
            return null;
        }
    }
}
