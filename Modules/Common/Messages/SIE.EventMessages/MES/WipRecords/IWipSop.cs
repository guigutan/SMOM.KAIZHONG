using System.Collections.Generic;

namespace SIE.EventMessages.MES.WipRecords
{
    /// <summary>
    /// 获取ESOP接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultWipSopInterface))]
    public interface IWipSop 
    {
        /// <summary>
        /// 获取文档列表
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <returns>文档列表</returns>
        List<WipSopInfo> GetDocumentListByWoId(double workOrderId);

        /// <summary>
        /// 获取文档列表
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <returns>文档列表</returns>
        List<WipSopInfo> GetDocumentListByItemId(double itemId);
    }

    /// <summary>
    /// 获取ESOP接口
    /// </summary>
    class DefaultWipSopInterface : IWipSop
    {
        /// <summary>
        /// 获取文档列表
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <returns>文档列表</returns>
        public List<WipSopInfo> GetDocumentListByWoId(double workOrderId)
        {
            return new List<WipSopInfo>();
        }

        /// <summary>
        /// 获取文档列表
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <returns>文档列表</returns>
        public List<WipSopInfo> GetDocumentListByItemId(double itemId)
        {
            return new List<WipSopInfo>();
        }
    }
}
