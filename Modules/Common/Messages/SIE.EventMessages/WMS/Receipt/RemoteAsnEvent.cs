using System;
using System.Collections.Generic;

namespace SIE.EventMessages.Receipt
{
    /// <summary>
    /// MES成品入库到WMS接口
    /// </summary>
    [Serializable]
    public class ProductToAsnEvent
    {
        /// <summary>
        /// WMS成品入库数据
        /// </summary>
        public List<RemoteAsnEvent> RemoteAsnEventList { get; set; }
    }

    /// <summary>
    /// WMS收货单主信息
    /// </summary>
    [Serializable]
    public class RemoteAsnEvent
    {
        /// <summary>
        /// 请求号(每次传输不能重复)
        /// </summary>
        public string RequireNo { get; set; }

        /// <summary>
        /// MES请求ID
        /// </summary>
        public double RequireId { get; set; }

        /// <summary>
        /// 订单类型
        /// 0：采购入库，10：成品入库，20:半成品入库,30:生产退料,40:销售退货,50:VMI入库,60:其他入库,70:销售出库,80:工单发料,90:其他出库,100：供应商退货，110：库存移动，120：库存调拨
        /// </summary>
        public int OrderType { get; set; }

        /// <summary>
        /// 0：普通，1：紧急
        /// </summary>
        public int PriorityType { get; set; }

        /// <summary>
        /// 交货日期
        /// </summary>
        public DateTime DeliveryDate { get; set; }

        /// <summary>
        /// 收货仓库
        /// </summary>
        public double? WarehouseId { get; set; }

        /// <summary>
        /// 生产部门
        /// 必填情况：成品入库、半成品入库、其他入库、生产退料
        /// </summary>
        public double? EnterpriseId { get; set; }

        /// <summary>
        /// 货主
        /// 必填情况：VMI入库
        /// </summary>
        public double? ShipperId { get; set; }

        /// <summary>
        /// 供应商
        /// 必填情况：采购入库、VMI入库
        /// </summary>
        public double? SupplierId { get; set; }

        /// <summary>
        /// 客户
        /// 必填情况：销售退货
        /// </summary>
        public double? CustomerId { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactsNumber { get; set; }

        /// <summary>
        /// 交接人
        /// </summary>
        public string Connecter { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否生成送货明细默认是false,如果是生成送货明细必须是true
        /// </summary>
        public bool IsGenerateAsnDeliveryDtl { get; set; }

        /// <summary>
        /// 明细信息
        /// </summary>
        public List<RemoteAsnDTLEvent> DetailList { get; set; }
    }

    /// <summary>
    /// WMS收货单明细信息
    /// </summary>
    [Serializable]
    public class RemoteAsnDTLEvent
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkNo { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 订货数
        /// 如果SN不为空，这个为SN数量，如果SN为空，lpn不为空，这个为lpn数量
        /// </summary>
        public decimal ExpectQty { get; set; }

        /// <summary>
        /// Set数量
        /// </summary>
        public int SetQty { get; set; }

        /// <summary>
        /// 叉板数
        /// </summary>
        public int XPlateQty { get; set; }

        /// <summary>
        /// LPN包装规则
        /// </summary>
        public double? LPNPackageRuleId { get; set; }

        /// <summary>
        /// LPN对应的包装规则层级
        /// </summary>
        public double? LPNPackageRuleDetailId { get; set; }

        /// <summary>
        /// lpn
        /// </summary>
        public string LPN { get; set; }

        ///// <summary>
        ///// 上级条码
        ///// </summary>
        //public string PackageNo { get; set; }

        /// <summary>
        /// 入库条码
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 是否不合格
        /// </summary>
        public  bool IsNg { get; set; }

        /// <summary>
        /// 项目号
        /// </summary>
        private string projectNo = string.Empty;

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo
        {
            get { return projectNo.IsNullOrEmpty() ? "*" : projectNo; }
            set { projectNo = value; }
        }

        /// <summary>
        /// 是否外包
        /// </summary>
        public bool IsOutPack { get; set; }

        /// <summary>
        /// 原批次号
        /// </summary>
        public string SourceLot { get; set; }

        private DateTime? lotAtt01;

        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime? LotAtt01 
        {
            get 
            {
                if (lotAtt01.HasValue)
                {
                    return lotAtt01.Value.Date;
                }
                else
                {
                    return null;
                }
            }
            set { lotAtt01 = value; }
        }

        /// <summary>
        /// 失效日期
        /// </summary>
        public DateTime? LotAtt02 { get; set; }

        /// <summary>
        /// 生产批次
        /// </summary>
        public string LotAtt04 { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>        
        public string ItemExtPropName { get; set; }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }
    }

    /// <summary>
    /// 回传ASN单号给MES
    /// </summary>
    [Serializable]
    public class RemoteAsnNo
    {
        /// <summary>
        /// AsnNoList
        /// </summary>
        public List<RemoteAsn> AsnNoList { get; set; }
    }

    /// <summary>
    /// Asn单号
    /// </summary>
    [Serializable]
    public class RemoteAsn
    {
        /// <summary>
        /// Asn单号
        /// </summary>
        public string AsnNo { get; set; }

        /// <summary>
        /// MES请求ID
        /// </summary>
        public double RequireId { get; set; }
    }

    /// <summary>
    /// 更新MES序列号数据
    /// </summary>
    [Serializable]
    public class UpdateMesSnInfo
    {
        /// <summary>
        /// MES入库单号
        /// </summary>
        public string RequireNo { get; set; }

        /// <summary>
        /// 入库ID
        /// </summary>
        public double? RequireId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// SN
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }
    }
}
