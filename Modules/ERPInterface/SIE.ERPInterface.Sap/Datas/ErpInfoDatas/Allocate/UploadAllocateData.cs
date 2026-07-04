using SIE.ERPInterface.Common.Datas;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.ERPInterface.Sap.Datas.ErpInfoDatas.Allocate
{
    /// <summary>
    /// 库存调拨事务上传
    /// </summary>
    [Serializable]
    public class UploadAllocateData : SapItemParamBase
    {
        /// <summary>
        /// WMS行号
        /// </summary>
        public string BILL_DTL_NO { get; set; }

        /// <summary>
        /// 工厂(库存组织)
        /// </summary>
        public string WERKS { get; set; }

        /// <summary>
        /// 移动类型
        /// </summary>
        public string BWART { get; set; }

        /// <summary>
        /// 发货仓库编码
        /// </summary>
        public string LGORT { get; set; }

        /// <summary>
        /// 目标仓库编码
        /// </summary>
        public string UMLGO { get; set; }

        /// <summary>
        /// 收货物料
        /// </summary>
        public string UMMAT { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal ERFMG { get; set; }

        /// <summary>
        /// 基本单位
        /// </summary>
        public string ERFME { get; set; }

        /// <summary>
        /// 发出批次
        /// </summary>
        public string CHARG { get; set; }

        /// <summary>
        /// 接受批次
        /// </summary>
        public string UMCHA { get; set; }

        /// <summary>
        /// 特殊库存 批次属性06有值时，取Q；
        /// 批次属性08 、09都有值时，取E；
        /// 批次属性06 、08 、09都为空时，取空值；
        /// </summary>
        public string SOBKZ { get; set; }

        /// <summary>
        /// 发出WBS项目号(取批次属性06)
        /// </summary>
        public string MAT_PSPNR { get; set; }

        /// <summary>
        /// 接收WBS项目号(取批次属性06)
        /// </summary>
        public string UMMAT_PSPNR { get; set; }

        /// <summary>
        /// 发出销售订单号 特殊库存=E时必填;(批次属性08)
        /// </summary>
        public string MAT_KDAUF { get; set; }

        /// <summary>
        /// 发出销售订单行号  批次属性09
        /// </summary>
        public string MAT_KDPOS { get; set; }

        /// <summary>
        /// 接收销售订单号 批次属性08
        /// </summary>
        public string UMMAT_KDAUF { get; set; }

        /// <summary>
        /// 接收销售订单行号 批次属性09
        /// </summary>
        public string UMMAT_KDPOS { get; set; }

        /// <summary>
        /// 整备销售订单 批次属性11
        /// </summary>
        public string KDAUF { get; set; }

        /// <summary>
        /// 整备销售订单行号 批次属性12
        /// </summary>
        public string KDPOS { get; set; }
    }

    /// <summary>
    /// 采购入库上传数据
    /// </summary>
    [Serializable]
    public class SapAllocateUploadData<T> : SapOrderParamBase<T>
    {
        /// <summary>
        /// WMS单号
        /// </summary>
        public string BILL_NO { get; set; }

        /// <summary>
        /// 整备调拨单号
        /// </summary>
        public string DBDH { get; set; }
    }
}
