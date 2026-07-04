namespace SIE.EventMessages.Sap
{
    /// <summary>
    /// 报废上传至SAP接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefalitlIScrap))]
    public interface IScrap
    {
        /// <summary>
        /// 处理上传至SAP
        /// </summary>
        /// <param name="scrapParam">报废参数</param>
        void Handle(ScrapParam scrapParam);
    }

    /// <summary>
    /// 默认实现
    /// </summary>
    public class DefalitlIScrap : IScrap
    {
        /// <summary>
        /// 处理上传至SAP
        /// </summary>
        /// <param name="scrapParam">报废参数</param>
        public void Handle(ScrapParam scrapParam)
        {
            // 处理上传至SAP
        }
    }
}
