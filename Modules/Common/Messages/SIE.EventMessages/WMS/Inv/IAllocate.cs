using System.Collections.Generic;

namespace SIE.EventMessages
{

    /// <summary>
    /// 库存调拨接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefalitIAllocateInterface))]
    public interface IAllocate
    {
        /// <summary>
        /// 创建库存调拨单
        /// </summary>
        /// <param name="allotParams">库存调拨参数集合</param>
        /// <returns>调拨单据号集合</returns>
        List<double> CreateInvAllocateByShipPlan(List<InvAllotParam> allotParams);

        /// <summary>
        /// 审核库存调拨单
        /// </summary>
        /// <param name="invAllocatIds">库存调拨ID集合</param>
        void AuditInvAllocateData(List<double> invAllocatIds);
    }

    /// <summary>
    /// 默认实现
    /// </summary>
    public class DefalitIAllocateInterface : IAllocate
    {
        /// <summary>
        /// 创建库存调拨单
        /// </summary>
        /// <param name="allotParams">库存调拨参数集合</param>
        /// <returns>调拨单据号集合</returns>
        public List<double> CreateInvAllocateByShipPlan(List<InvAllotParam> allotParams)
        {
            return new List<double>();
        }

        /// <summary>
        /// 审核库存调拨单
        /// </summary>
        /// <param name="invAllocatIds">库存调拨ID集合</param>
        public void AuditInvAllocateData(List<double> invAllocatIds)
        {
            //
        }
    }
}
