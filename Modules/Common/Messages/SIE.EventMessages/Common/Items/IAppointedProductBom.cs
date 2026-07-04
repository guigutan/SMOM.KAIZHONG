using System.Collections.Generic;

namespace SIE.EventMessages.Common.Items
{
    /// <summary>
    /// 指定产品BOM接口-需求管理
    /// </summary>
    [Services.Service(FallbackType = typeof(EmptyAppointedDemandManagement))]
    public interface IAppointedDemandManagement
    {
        /// <summary>
        /// 是否存在指定BOM为传入id的需求管理
        /// </summary>
        /// <param name="bomId">bom的ID</param>
        /// <returns>是否存在</returns>
        bool ExsitAppointedProductBomDemandManagement(double bomId);

        /// <summary>
        /// 更新需求管理的状态(上传状态）
        /// </summary>
        /// <param name="productOrderCodes"></param>
        void CallBackDemandState(List<string> productOrderCodes);
    }


    /// <summary>
    ///指定产品BOM接口默认实现
    /// </summary>
    public class EmptyAppointedDemandManagement : IAppointedDemandManagement
    {
        /// <summary>
        /// 是否存在指定BOM为传入id的需求管理
        /// </summary>
        /// <param name="bomId">bom的ID</param>
        /// <returns>是否存在</returns>
        public bool ExsitAppointedProductBomDemandManagement(double bomId)
        {
            return false;
        }

        /// <summary>
        /// 更新需求管理的状态
        /// </summary>
        public void CallBackDemandState(List<string> productOrderCodes)
        {

        }
    }


    /// <summary>
    /// 指定产品BOM接口-生产订单
    /// </summary>
    [Services.Service(FallbackType = typeof(EmptyAppointedProductOrder))]
    public interface IAppointedProductOrder
    {
        /// <summary>
        /// 是否存在指定BOM为传入id的生产订单
        /// </summary>
        /// <param name="bomId">bom的ID</param>
        /// <returns>是否存在</returns>
        bool ExsitAppointedProductBomProductOrder(double bomId);
    }


    /// <summary>
    ///指定产品BOM接口默认实现
    /// </summary>
    public class EmptyAppointedProductOrder : IAppointedProductOrder
    {
        /// <summary>
        /// 是否存在指定BOM为传入id的生产订单
        /// </summary>
        /// <param name="bomId">bom的ID</param>
        /// <returns>是否存在</returns>
        public bool ExsitAppointedProductBomProductOrder(double bomId)
        {
            return false;
        }
    }
}
