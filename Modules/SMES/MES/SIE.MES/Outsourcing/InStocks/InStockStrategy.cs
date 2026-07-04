using SIE.Core.Items;
using SIE.Domain;
using SIE.MES.Outsourcing.Outbounds;
using System.Collections.Generic;

namespace SIE.MES.Outsourcing.InStocks
{
    /// <summary>
    /// 委外入库策略
    /// </summary>
    public interface InStockStrategy
    {
        /// <summary>
        /// 物料管理方式
        /// </summary>
        public RetrospectType RetrospectType { get; }

        /// <summary>
        /// 入库提交重新上线
        /// </summary>
        /// <param name="outsourcingRequest">工序委外需求单</param>
        /// <param name="inStocks">入库明细</param>
        void ResumeProduction(OutsourcingRequest outsourcingRequest, List<ProcessingInStock> inStocks);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="SnOrLot"></param>
        /// <param name="outsourcingRequest"></param>
        /// <returns></returns>
        EntityList<ProcessingInStock> GetProcessingInbounds(string SnOrLot, OutsourcingRequest outsourcingRequest);
    }
}
