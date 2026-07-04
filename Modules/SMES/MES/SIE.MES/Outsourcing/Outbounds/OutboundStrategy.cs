using SIE.Core.Items;
using SIE.Domain;
using SIE.MES.WIP.Products;
using SIE.MES.WorkOrders;
using System.Collections.Generic;

namespace SIE.MES.Outsourcing.Outbounds
{
    /// <summary>
    /// 委外出库策略
    /// </summary>
    public interface OutboundStrategy
    {
        /// <summary>
        /// 物料管理方式
        /// </summary>
        public RetrospectType RetrospectType { get; }

        /// <summary>
        /// 添加委外出库的产品
        /// </summary>
        /// <param name="outsourcingRequest">委外需求单</param>
        int AddOutboundProducts(OutsourcingRequest outsourcingRequest);

        /// <summary>
        /// 获取在制品出库-API
        /// </summary>
        /// <param name="SnOrLot"></param>
        /// <param name="outsourcingRequest"></param>
        /// <returns></returns>
        EntityList<ProcessingOutbound> GetProcessingOutbounds(string SnOrLot, OutsourcingRequest outsourcingRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outsourcingRequest"></param>
        /// <param name="outbounds"></param>
        void DeleteOutbounds(OutsourcingRequest outsourcingRequest, EntityList<ProcessingOutbound> outbounds);

        /// <summary>
        /// 提交出库
        /// </summary>
        /// <param name="outsourcingRequest"></param>
        /// <param name="processingOutbounds"></param>
        void SubmitOutbounds(OutsourcingRequest outsourcingRequest,EntityList<ProcessingOutbound> processingOutbounds);
        /// <summary>
        /// 提交API出库记录
        /// </summary>
        /// <param name="outsourcingRequest"></param>
        /// <param name="processingOutbounds"></param>
        /// <returns></returns>
        bool SaveOutboundProduct(OutsourcingRequest outsourcingRequest, EntityList<ProcessingOutbound> processingOutbounds);
    }
}
