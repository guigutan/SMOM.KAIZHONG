using SIE.Domain;
using SIE.ProductIntfc.ProductStorages;
using SIE.Web.Data;

namespace SIE.Web.ProductIntfc.ProductStorages.DataQuery
{
    public class StorageWorkOrderDataQueryer : DataQueryer
    {
        /// <summary>
        /// 获取待入库条码明细列表
        /// </summary>
        /// <param name="barcodeId">待入库条码Id</param>
        /// <returns>最大样本数</returns>
        public EntityList<ToStorageBarcodeDetail> GetBarcodeDetails(double barcodeId)
        {
            return RT.Service.Resolve<ProductStorageController>().GetToStoreBarcodeDetails(barcodeId);
        }
    }
}
