using SIE.EventMessages.LES.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.LES
{
    /// <summary>
    /// 外部更新备料信息接口
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultILesMaterialPrepare))]
    public interface ILesMaterialPrepare
    {
        /// <summary>
        /// WMS发料更新备料需求单明细发料数
        /// </summary>
        /// <param name="shippingUpdateDatas">更新信息</param>
        void ShippingUpdateMaterialPre(List<ShippingUpdateData> shippingUpdateDatas);

        /// <summary>
        /// WMS关闭发运订单回写备料需求单取消数并更新状态
        /// </summary>
        /// <param name="shippingCloseDatas"></param>
        void ShippingCloseMaterialPre(List<ShippingUpdateData> shippingCloseDatas);

        /// <summary>
        /// WSM创建发运订单更新来源单发运单号
        /// </summary>
        /// <param name="shippingUpdateSoNoDatas">更新信息</param>
        void ShippingUpdateSourceSo(List<ShippingUpdateSoNoData> shippingUpdateSoNoDatas);
    }

    /// <summary>
    /// 外部更新备料信息
    /// </summary>
    class DefaultILesMaterialPrepare: ILesMaterialPrepare
    {
        /// <summary>
        /// WMS发料更新备料需求单明细发料数
        /// </summary>
        /// <param name="shippingUpdateDatas">更新信息</param>
        public void ShippingUpdateMaterialPre(List<ShippingUpdateData> shippingUpdateDatas)
        {

        }

        /// <summary>
        /// WSM创建发运订单更新来源单发运单号
        /// </summary>
        /// <param name="shippingUpdateSoNoDatas">更新信息</param>
        public void ShippingUpdateSourceSo(List<ShippingUpdateSoNoData> shippingUpdateSoNoDatas)
        {

        }

        /// <summary>
        /// WMS关闭发运订单回写备料需求单取消数并更新状态
        /// </summary>
        /// <param name="shippingCloseDatas"></param>
        public void ShippingCloseMaterialPre(List<ShippingUpdateData> shippingCloseDatas)
        {

        }
    }
}
