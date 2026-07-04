using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.MES.Traces
{
    /// <summary>
    /// 过程追溯信息接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultTrace))]
    public interface ITrace
    {
        /// <summary>
        /// 过程追溯-关联产品追溯
        /// </summary>
        /// <param name="criteria">查询条件</param>
        /// <returns></returns>
        TraceInfoForProduct GetTraceInfoForProduct(TraceInfoForProductCriteria criteria);

        /// <summary>
        /// 过程追溯-采集记录追溯
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        TraceInfoForProcssKeyItem GetTraceInfoForProcssKeyItem(TraceInfoForProcssKeyItemCriteria criteria);

        /// <summary>
        /// 过程追溯-产品检验记录追溯
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        TraceInfoForProductInspect GetTraceInfoForProductInspect(TraceInfoForProductInspectCriteria criteria);

        /// <summary>
        /// 过程追溯-产品缺陷记录追溯
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        TraceInfoForProductDefect GetTraceInfoForProductDefect(TraceInfoForProductDefectCriteria criteria);

        /// <summary>
        /// 过程追溯-产品维修记录追溯
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        TraceInfoForProductRepair GetTraceInfoForProductRepair(TraceInfoForProductRepairCriteria criteria);

        /// <summary>
        /// 获取产品报检信息
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        List<ProductInspectInfo> GetProductInspectInfo(ProductInspectInfoCriteria criteria);

        /// <summary>
        /// 获取包装信息
        /// </summary>
        /// <param name="criteriaInfo">查询条件</param>
        /// <returns></returns>
        List<PackageInfo> GetPackageInfos(PackageInfoCriteria criteriaInfo);

        /// <summary>
        /// 获取Mes产品列表信息-反向追溯
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        MesProductInfo GetMesProductInfo(MesProductInfoCriteria criteria);

        /// <summary>
        /// 获取Mes工序采集信息-反向追溯
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        MesProcessCollectInfo GetMesProcessCollectInfo(MesProcessCollectInfoCriteria criteria);

        /// <summary>
        /// 获取Mes关键件采集信息-反向追溯
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        MesProcessCollectKeyItemInfo GetMesProcessCollectKeyItemInfo(MesProcessCollectKeyItemInfoCriteria criteria);

    }

    /// <summary>
    /// 过程追溯信息接口的默认实现
    /// </summary>算
    class DefaultTrace : ITrace
    {
        public List<ProductInspectInfo> GetProductInspectInfo(ProductInspectInfoCriteria criteria)
        {
            return new List<ProductInspectInfo>();
        }

        public TraceInfoForProcssKeyItem GetTraceInfoForProcssKeyItem(TraceInfoForProcssKeyItemCriteria criteria)
        {
            return new TraceInfoForProcssKeyItem();
        }

        /// <summary>
        /// 过程追溯-关联产品追溯
        /// </summary>
        /// <param name="criteria">查询条件</param>
        /// <returns></returns>
        public TraceInfoForProduct GetTraceInfoForProduct(TraceInfoForProductCriteria criteria)
        {
            return new TraceInfoForProduct();
        }

        public TraceInfoForProductDefect GetTraceInfoForProductDefect(TraceInfoForProductDefectCriteria criteria)
        {
            return new TraceInfoForProductDefect();
        }

        public TraceInfoForProductInspect GetTraceInfoForProductInspect(TraceInfoForProductInspectCriteria criteria)
        {
            return new TraceInfoForProductInspect();
        }

        public TraceInfoForProductRepair GetTraceInfoForProductRepair(TraceInfoForProductRepairCriteria criteria)
        {
            return new TraceInfoForProductRepair();
        }


        public List<PackageInfo> GetPackageInfos(PackageInfoCriteria criteriaInfo)
        {
            return new List<PackageInfo>();
        }

        public MesProductInfo GetMesProductInfo(MesProductInfoCriteria criteria)
        {
            return new MesProductInfo();
        }

        public MesProcessCollectInfo GetMesProcessCollectInfo(MesProcessCollectInfoCriteria criteria)
        {
            return new MesProcessCollectInfo();
        }

        public MesProcessCollectKeyItemInfo GetMesProcessCollectKeyItemInfo(MesProcessCollectKeyItemInfoCriteria criteria)
        {
            return new MesProcessCollectKeyItemInfo();
        }
    }
}
