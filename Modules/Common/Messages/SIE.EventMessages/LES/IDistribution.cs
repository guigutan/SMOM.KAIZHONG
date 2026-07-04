using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EventMessages.LES
{
    /// <summary>
    /// 配送管理接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultIDistribution))]
    public interface IDistribution
    {
        /// <summary>
        /// AGV更新配送单状态
        /// </summary>
        /// <param name="lpn"></param>
        void AgvFinishUpdateDistribution(string lpn);

        /// <summary>
        /// 获取集货库位
        /// </summary>
        /// <param name="billNo">备料单号</param>
        /// <param name="lineNo">备料单行号</param>       
        /// <returns>集货库位</returns>
        double? GetDisSettingLocation(string billNo, string lineNo);

        /// <summary>
        /// 发运单创建配送单
        /// </summary>
        /// <param name="distributionDatas">配送单数据</param>
        bool CreateDistribution(List<DistributionData> distributionDatas);

        /// <summary>
        /// 取消发货
        /// </summary>
        /// <param name="soDtlIds">发运单明细</param>       
        void CancelShipping(List<double> soDtlIds);
    }

    /// <summary>
    /// 发运单发货更新备料单信息
    /// </summary>
    class DefaultIDistribution : IDistribution
    {
        /// <summary>
        /// AGV更新配送单状态
        /// </summary>
        /// <param name="lpn"></param>
        public void AgvFinishUpdateDistribution(string lpn)
        {
            //无
        }

        /// <summary>
        /// 取消发货
        /// </summary>
        /// <param name="soDtlIds">发运单明细</param>    
        public void CancelShipping(List<double> soDtlIds)
        {
            //无
        }

        /// <summary>
        /// 发运单创建配送单
        /// </summary>
        /// <param name="distributionDatas">配送单数据</param>
        public bool CreateDistribution(List<DistributionData> distributionDatas)
        {
            return false;
        }

        /// <summary>
        /// 获取集货库位
        /// </summary>
        /// <param name="billNo">备料单号</param>
        /// <param name="lineNo">备料单行号</param>       
        /// <returns>集货库位</returns>
        public double? GetDisSettingLocation(string billNo, string lineNo)
        {
            return null;
        }
    }
}
