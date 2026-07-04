using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EventMessages.WMS.Shipment
{
    /// <summary>
    /// 创建发运单数据
    /// </summary>
    [Services.Service(FallbackType = typeof(DefalitICreateSoInterface))]
    public interface ICreateSo
    {
        /// <summary>
        /// LES创建发运单
        /// </summary>
        /// <param name="mesMoveCreateSoDatas"></param>
        void CreateShippingOrderByLes(MesMoveCreateSoData mesMoveCreateSoDatas);
    }

    /// <summary>
    /// 获取发运单数据
    /// </summary>
    class DefalitICreateSoInterface : ICreateSo
    {
        public void CreateShippingOrderByLes(MesMoveCreateSoData mesMoveCreateSoDatas)
        {            
            ////默认实现
        }
    }
}
