using SIE.EventMessages.WMS.Shipment;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EventMessages.WMS.Inventory
{
    /// <summary>
    /// MES工单退料更新库存接口
    /// </summary>
    public interface IMesReturnItem
    {
        /// <summary>
        ///  MES退料更新库存接口
        /// </summary>
        /// <param name="mesReturnItemData">退料数据</param>
        void UpdateInventoryByReturnItem(MesReturnItemData mesReturnItemData);

        /// <summary>
        /// MES挪料创建ASN单
        /// </summary>
        /// <param name="mesMoveCreateSoData"></param>
        /// <param name="isSendRightNow"></param>
        void CreateAsnByLes(MesMoveCreateSoData mesMoveCreateSoData, bool isSendRightNow);

        /// <summary>
        /// 退货创建ASN
        /// </summary>
        /// <param name="returnMaterialData">退料单集合</param>
        void ReturnMaterialCreateAsn(List<ReturnMaterialData> returnMaterialData);
    }

    /// <summary>
    ///  MES工单退料更新库存接口默认实现
    /// </summary>
    public class DefaultIMesReturnItem : IMesReturnItem
    {
        /// <summary>
        /// MES挪料创建ASN单
        /// </summary>
        /// <param name="mesMoveCreateSoData"></param>
        /// <param name="isSendRightNow"></param>
        public void CreateAsnByLes(MesMoveCreateSoData mesMoveCreateSoData, bool isSendRightNow)
        {
            ////
        }

        /// <summary>
        ///  MES退料更新库存接口默认实现
        /// </summary>
        /// <param name="mesReturnItemData">退料数据</param>
        public void UpdateInventoryByReturnItem(MesReturnItemData mesReturnItemData)
        {
            // Method intentionally left empty.
        }

        /// <summary>
        /// 退货创建ASN
        /// </summary>
        /// <param name="returnMaterialData"></param>
        public void ReturnMaterialCreateAsn(List<ReturnMaterialData> returnMaterialData)
        {
            // Method intentionally left empty.
        }
    }
}
