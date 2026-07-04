using SIE.Domain;
using SIE.ProductIntfc.InspLogs;

namespace SIE.ProductIntfc.InspSettings
{
    /// <summary>
    /// 报检参数设置控制器
    /// </summary>
    public class InspParameterController : DomainController
    {
        /// <summary>
        /// 获取产品报检参数
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="inspType">报检类型</param>
        /// <returns>报检参数</returns>
        public virtual InspParameter GetInspParameter(double productId, InspType inspType)
        {
            return Query<InspParameter>().Where(p => p.ProductId == productId && p.InspType == inspType).FirstOrDefault();
        }

        /// <summary>
        /// 获取产品报检参数
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="processId">工序ID</param>
        /// <param name="inspType">报检类型</param>
        /// <returns>报检参数</returns>
        public virtual InspParameter GetInspParameter(double productId, double processId, InspType inspType)
        {
            return Query<InspParameter>().Where(p => p.ProductId == productId && p.InspProcessId == processId && p.InspType == inspType).FirstOrDefault();
        }

        /// <summary>
        /// 获取产品报检参数
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="processType">工序类型</param>
        /// <param name="inspType">报检类型</param>
        /// <returns>报检参数</returns>
        public virtual InspParameter GetInspParameter(double productId, InspProcess processType, InspType inspType)
        {
            return Query<InspParameter>().Where(p => p.ProductId == productId && p.ProcessType == processType && p.InspType == inspType).FirstOrDefault();
        }

        /// <summary>
        /// 获取报检参数
        /// </summary>
        /// <param name="inspLog">报检日志</param>
        /// <returns>报检参数</returns>
        public virtual InspParameter GetInspParameter(InspLog inspLog)
        {
            if(inspLog == null)
            {
                return null;
            }
            var parameter = GetInspParameter(inspLog.WorkOrder.ProductId, inspLog.InspType);
            if (parameter != null)
            {
                return parameter;
            }
            return GetGeneralInspParameter(inspLog.InspType);
        }

        /// <summary>
        /// 获取通用产品报检参数
        /// </summary>
        /// <param name="inspType">报检类型</param>
        /// <returns>通用产品报检参数</returns>
        public virtual InspParameter GetGeneralInspParameter(InspType inspType)
        {
            return Query<InspParameter>().Where(p => p.ProductId == null && p.InspType == inspType).FirstOrDefault();
        }

        /// <summary>
        /// 是否存在通用的报检参数
        /// </summary>
        /// <param name="id">报检Id</param>
        /// <param name="inspType">报检类型</param>
        /// <returns>是否存在通用的报检参数</returns>
        public virtual bool ExistGeneralParam(double id, InspType inspType)
        {
            return Query<InspParameter>().Where(p => p.Id != id && p.ProductId == null && p.InspType == inspType).Count() > 0;
        }

        /// <summary>
        /// 产品是否存在其他报检参数
        /// </summary>
        /// <param name="inspType">报检类型</param>
        /// <param name="productId">产品id</param>
        /// <param name="id">报检参数id</param>
        /// <returns>是否存在</returns>
        public virtual bool ExistOtherParam(InspType inspType, double productId, double id)
        {
            return Query<InspParameter>().Where(p => p.ProductId == productId && p.InspType == inspType && p.Id != id).Count() > 0;
        }

        /// <summary>
        /// 产品是否存在首件非自定义工序
        /// </summary>
        /// <param name="productId">产品id</param>
        /// <param name="id">报检参数id</param>
        /// <returns>是否存在</returns>
        public virtual bool ExistFirstParam(double productId, double id)
        {
            return Query<InspParameter>().Where(p => p.ProductId == productId && p.InspType == InspType.FirstProduct && p.Id != id && p.ProcessType != InspProcess.Custom)
                .Count() > 0;
        }

        /// <summary>
        /// 是否存在相同【产品编码+报检类型+工序类型+报检工序】的报检参数
        /// </summary>
        /// <param name="inspParameter">报检参数</param>
        /// <returns>是否存在</returns>
        public virtual bool ExistSameInspParameter(InspParameter inspParameter)
        {
            return Query<InspParameter>().Where(p => p.Id != inspParameter.Id && p.ProductId == inspParameter.ProductId 
            && p.InspType == inspParameter.InspType && p.ProcessType == inspParameter.ProcessType && p.InspProcessId == inspParameter.InspProcessId).Count() > 0;
        }
    }
}