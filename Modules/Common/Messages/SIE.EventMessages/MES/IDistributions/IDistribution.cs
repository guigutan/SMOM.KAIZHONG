using SIE.Services;

namespace SIE.EventMessages.IDistributions
{
    /// <summary>
    /// 标签载具关联接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultDistribution))]
    public interface IDistribution
    {
        /// <summary>
        /// 标签是否已经配送
        /// </summary>
        /// <param name="label">标签号</param>
        /// <returns>已配送返回true，否则返回false</returns>
        bool IsDistribution(string label);
    }

    /// <summary>
    /// 标签载具关联默认实现
    /// </summary>
    public class DefaultDistribution : IDistribution
    {
        /// <summary>
        /// 标签是否已经配送
        /// </summary>
        /// <param name="label">标签号</param>
        /// <returns>已配送返回true，否则返回false</returns>
        public bool IsDistribution(string label)
        {
            return true;
        }
    }
}