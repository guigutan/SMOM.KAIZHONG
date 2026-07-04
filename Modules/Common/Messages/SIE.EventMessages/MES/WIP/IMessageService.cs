using SIE.Services;

namespace SIE.EventMessages.MES.WIP
{
    /// <summary>
    /// 消息服务接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultMessageService))]
    public interface IMessageService
    {
        /// <summary>
        /// 重传失败消息
        /// </summary>
        /// <param name="msgIds">消息Ids</param>
        /// <returns>成功条数</returns>
        int ReSubmitErrorMessage(double[] msgIds);
    }

    /// <summary>
    /// 默认实现叫料接口
    /// </summary>
    public class DefaultMessageService : IMessageService
    {
        /// <summary>
        /// 重传失败消息
        /// </summary>
        /// <param name="msgIds">消息Ids</param>
        /// <returns>成功条数</returns>
        public int ReSubmitErrorMessage(double[] msgIds)
        {
            return 0;
        }
    }
}
