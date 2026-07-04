using SIE.ERPInterface.Common.Datas;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.ERPInterface.Sap.Datas.ErpInfoDatas.Suppliers
{
    /// <summary>
    /// 供应商退货
    /// </summary>
    [Serializable]
    public class SupplierReturnUploadData:SapItemParamBase
    {
        /// <summary>
        /// WMS行号
        /// </summary>
        public string BILL_DTL_NO { get; set; }

        /// <summary>
        /// 采购凭证(发运单明细，采购退货单传的单号)
        /// </summary>
        public string EBELN { get; set; }

        /// <summary>
        /// 采购订单项目号(发运单明细，采购退货单传的行号)
        /// </summary>
        public string EBELP { get; set; }

        /// <summary>
        /// 工厂(库存组织编码)
        /// </summary>
        public string WERKS { get; set; }

        /// <summary>
        /// 库存地点(发货仓库编码)
        /// </summary>
        public string LGORT { get; set; }

        /// <summary>
        /// 基本单位数量（事务交易数量(主)）
        /// </summary>
        public decimal ERFMG { get; set; }

        /// <summary>
        /// 主单位
        /// </summary>
        public string ERFME { get; set; }

        /// <summary>
        /// 采购单位（发运单明细，辅助单位编码）
        /// </summary>
        public string TARME { get; set; }

        /// <summary>
        /// 移动类型
        /// </summary>
        public string BWART { get; set; }

        /// <summary>
        /// 暂收凭证号
        /// </summary>
        public string LFBNR { get; set; }

        /// <summary>
        /// 暂收凭证行号
        /// </summary>
        public string LFPOS { get; set; }
    }

    /// <summary>
    /// 销售出库上传数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class SapOrderParamSupplierReturn<T> : SapOrderParamBase<T>
    {
        /// <summary>
        /// WMS单行号
        /// </summary>
        public string BILL_NO { get; set; }
    }
}
