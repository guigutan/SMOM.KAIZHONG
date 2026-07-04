using SIE.Domain;
using SIE.MES.WIP.Reworks;

namespace SIE.MES.WorkOrders.Reworks
{
    /// <summary>
    /// 返工条码获取接口
    /// </summary>
    [Services.Service(FallbackType = typeof(ReworkBarcode))]
    public interface IReworkBarcode
    {
        /// <summary>
        /// 获取返工关联条码列表
        /// </summary>
        /// <param name="criteria">返工关联条码查询实体</param>
        /// <returns>关联条码列表</returns>
        EntityList GetUnionBarcodeViews(UnionBarcodeViewCriteria criteria);
    }

    /// <summary>
    /// 返工条码，没有报检功能时返修条码获取类
    /// </summary>
    public class ReworkBarcode : IReworkBarcode
    {
        /// <summary>
        /// 获取返工关联条码列表
        /// </summary>
        /// <param name="criteria">返工关联条码查询实体</param>
        /// <returns>关联条码列表</returns>
        public virtual EntityList GetUnionBarcodeViews(UnionBarcodeViewCriteria criteria)
        {
            return RT.Service.Resolve<ReworkController>().GetUnionBarcodeViews(criteria);
        }
    }
}