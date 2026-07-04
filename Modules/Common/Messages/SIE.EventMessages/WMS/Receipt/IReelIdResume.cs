using System.Collections.Generic;

namespace SIE.EventMessages
{
    /// <summary>
    /// 获取标签履历
    /// </summary>
    [Services.Service(FallbackType = typeof(DefalitIReelIdResumeInterface))]
    public interface IReelIdResume
    {
        /// <summary>
        /// 获取标签履历
        /// </summary>
        /// <param name="reelId">标签</param>
        /// <returns>标签履历数据</returns>
        List<ReelIdResumeData> GetReelIdResumeData(string reelId);        
    }

    /// <summary>
    /// 获取标签履历默认方法
    /// </summary>
    class DefalitIReelIdResumeInterface : IReelIdResume
    {
        /// <summary>
        /// 获取标签履历
        /// </summary>
        /// <param name="reelId">标签</param>
        /// <returns>标签履历数据</returns>
        public List<ReelIdResumeData> GetReelIdResumeData(string reelId)
        {
            return new List<ReelIdResumeData>();
        }
    }
}
