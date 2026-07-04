using SIE.Services;
using System.Collections.Generic;

namespace SIE.EventMessages.MES.PPSNs
{
	/// <summary>
	/// 产品SN追溯查询接口
	/// </summary>
	[Service(FallbackType = typeof(DefaultPPSNQuery))]
    public interface IPPSNQuery
    {
        /// <summary>
        /// 获取PPID集合
        /// </summary>
        /// <param name="queryInfo">PPID查询信息</param>
        /// <returns>PPID集合</returns>
        List<PPSNInfo> GetPPSNInfos(PPSNQueryInfo queryInfo);
    }

    /// <summary>
    /// 默认实现
    /// </summary>
    public class DefaultPPSNQuery : IPPSNQuery
    {
        /// <summary>
        /// 获取PPID集合
        /// </summary>
        /// <param name="queryInfo">PPID查询信息</param>
        /// <returns>PPID集合</returns>
        public List<PPSNInfo> GetPPSNInfos(PPSNQueryInfo queryInfo)
        {
            return new List<PPSNInfo>();
        }
    }
}