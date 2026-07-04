using SIE.EventMessages.MES.Inspection.Models;
using SIE.Services;
using System.Collections.Generic;

namespace SIE.EventMessages.MES.Inspection
{
    /// <summary>
    /// 条码报检接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultInspBarcode))]
    public interface IInspBarcode
    {
        /// <summary>
        /// 获取报检条码
        /// </summary>
        /// <param name="queryInfo">报检条码查询信息</param>
        /// <returns>报检条码</returns>
        IList<InspBarcodeInfo> GetInspBarcodes(BarcodeQueryInfo queryInfo);

        /// <summary>
        /// 条码报检
        /// </summary>
        /// <param name="inspBarcodeIds">报检条码ID列表</param>
        /// <param name="inspType">报检类型 0成品 1首件 2抽检</param>
        void BarcodeInsp(List<double> inspBarcodeIds, int inspType);
    }

    /// <summary>
    /// 条码报检接口默认实现
    /// </summary>
    public class DefaultInspBarcode : IInspBarcode
    {
        /// <summary>
        /// 获取报检条码
        /// </summary>
        /// <param name="queryInfo">报检条码查询信息</param>
        /// <returns>报检条码</returns>
        public IList<InspBarcodeInfo> GetInspBarcodes(BarcodeQueryInfo queryInfo)
        {
            return new List<InspBarcodeInfo>();
        }

        /// <summary>
        /// 条码报检
        /// </summary>
        /// <param name="inspBarcodeIds">报检条码ID列表</param>
        /// <param name="inspType">报检类型 0成品 1首件 2抽检</param>
        public void BarcodeInsp(List<double> inspBarcodeIds, int inspType)
        {
            // 条码报检
        }
    }
}