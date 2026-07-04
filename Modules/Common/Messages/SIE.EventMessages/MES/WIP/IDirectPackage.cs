using SIE.Services;
using System;

namespace SIE.EventMessages.MES.WIP
{
    /// <summary>
    /// 直接包装采集接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultIDirectPackage))]
    public interface IDirectPackage
    {
        /// <summary>
        /// 判断条码是否已入库
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        bool BarcodeIsDetail(string barcode, double? workOrderId);
    }

    /// <summary>
    /// 默认实体
    /// </summary>
    public class DefaultIDirectPackage : IDirectPackage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        public bool BarcodeIsDetail(string barcode, double? workOrderId)
        {
            return false;
        }
    }
}
