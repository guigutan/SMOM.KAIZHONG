using SIE.EventMessages.MES.PPSNs;
using SIE.MES.WIP.Products;
using System.Collections.Generic;

namespace SIE.MES.Wip.Products
{
	/// <summary>
	/// PPID查询接口实现
	/// </summary>
	public class PPSNQuery : IPPSNQuery
    {
        /// <summary>
        /// 获取PPID集合
        /// </summary>
        /// <param name="queryInfo">PPID查询信息</param>
        /// <returns>PPID集合</returns>
        public List<PPSNInfo> GetPPSNInfos(PPSNQueryInfo queryInfo)
        {
            return RT.Service.Resolve<WipProductVersionController>().GetPPSNInfos(queryInfo);
        }
    }
}