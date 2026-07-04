using System.Collections.Generic;

namespace SIE.EventMessages.MES.Barcodes
{
    /// <summary>
    /// 更新IQC报检接口信息
    /// </summary>
    [Services.Service(FallbackType = typeof(DefalitIBarcodeInterface))]
    public interface IBarcode
    {
        /// <summary>
        /// 获取条码信息
        /// </summary>
        /// <returns>更新成功true/失败false</returns>
        List<BarCodeInfoWithQty> GetBarcodeInfo(List<string> sns);
    }

    /// <summary>
    /// 默认实现
    /// </summary>
    class DefalitIBarcodeInterface : IBarcode
    {
        public List<BarCodeInfoWithQty> GetBarcodeInfo(List<string> sns)
        {
            return new List<BarCodeInfoWithQty>();
        }
    }
}
