using SIE.EventMessages.Receipt;
using SIE.Services;

namespace SIE.EventMessages.MES.ProductStorage
{
    /// <summary>
    /// 产品库存接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultProductStorage))]
    public interface IProductStorage
    {
        /// <summary>
        /// WMS单号回传
        /// </summary>
        /// <param name="wmsAsn"></param>
        void UpdateFromWMSAsn(RemoteAsnNo wmsAsn);
    }

    /// <summary>
    /// 产品
    /// </summary>
    public class DefaultProductStorage : IProductStorage
    {
        /// <summary>
        /// WMS单号回传ASN
        /// </summary>
        /// <param name="wmsAsn"></param>
        public void UpdateFromWMSAsn(RemoteAsnNo wmsAsn)
        {
            // Method intentionally left empty.
        }
    }
}
