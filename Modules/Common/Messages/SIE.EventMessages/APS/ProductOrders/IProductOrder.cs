using System.Collections.Generic;

namespace SIE.EventMessages.APS.ProductOrders
{
    /// <summary>
    /// 生产订单编号接口
    /// </summary>
    [Services.Service(FallbackType = typeof(EmptyProductOrder))]
    public interface IProductOrder
    {
        /// <summary>
        /// 获取生产订单编号
        /// </summary>
        /// <returns>生产订单编号List</returns>
        IReadOnlyList<string> GetProductOrderNos(int qty);

        /// <summary>
        /// 上传生产建议到生产订单，并返回提示框信息
        /// </summary>
        /// <param name="adviceIds">生产建议ID List</param>
        /// <returns>提示信息</returns>
        string UploadAdvicesToOrders(List<double> adviceIds);

        /// <summary>
        /// 获取要清除的销售订单编号（已完成且客户交期过去一个月）
        /// </summary>
        /// <returns>销售订单编号</returns>
        List<string> GetSaleOrdersToClear();

        /// <summary>
        ///  获取要清除的生产订单编号（已完成且客户交期过去一个月）
        /// </summary>
        /// <returns>生产订单编号</returns>
        List<string> GetProOrderCodesToClear();
    }

    /// <summary>
    /// 生产订单接口默认实现
    /// </summary>
    public class EmptyProductOrder : IProductOrder
    {
        /// <summary>
        /// 获取生产订单编号
        /// </summary>
        /// <returns>生产订单编号List</returns>
        public IReadOnlyList<string> GetProductOrderNos(int qty)
        {
            return new List<string>();
        }

        /// <summary>
        /// 上传失败
        /// </summary>
        public string UploadAdvicesToOrders(List<double> adviceIds)
        {
            return "上传失败 没有生产订单模块";
        }

        /// <summary>
        /// 无需求管理模块
        /// </summary>
        public List<string> GetSaleOrdersToClear()
        {
            return new List<string>();
        }

        /// <summary>
        /// 无生产订单模块
        /// </summary>
        public List<string> GetProOrderCodesToClear()
        {
            return new List<string>();
        }
    }
}
