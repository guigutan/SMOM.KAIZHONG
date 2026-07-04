using SIE.LES.RetreatItemManage.MaterialReturns;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.LES.WorkOrderMaterialtransfes.ApiModel
{
   /// <summary>
   /// 物料标签扫描结果
   /// </summary>
    [Serializable]
    public class ScandResultInfos
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public double Id { get; set; }

       /// <summary>
       /// Sn
       /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WoId { get; set; }

        /// <summary>
        /// 物料标签工单关联Id
        /// </summary>
        public double? WoLabelRelationId { get; set; }


        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 生产资源
        /// </summary>
        public string Wipresoure { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public  decimal Qty { get; set; }

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal RemainingQty { get; set; }


        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo { get; set; }

       /// <summary>
       /// 退料类型
       /// </summary>

        public ReturnTypes ReturnType { get; set; }

        /// <summary>
        /// 退料类型显示值
        /// </summary>
        public string ReturnTypeDisplay { get; set; }

        /// <summary>
        /// 原因
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 退料描述
        /// </summary>

        public string Desc { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public string Factory { get; set; }

        /// <summary>
        /// 工厂Id
        /// </summary>
        public double? FactoryId { get; set; }

        /// <summary>
        ///资源
        /// </summary>
        public string Resoure { get; set; }

        /// <summary>
        /// 生产资源
        /// </summary>
        public double? ResoureId { get; set; }
        /// <summary>
        /// 仓库
        /// </summary>
        public string Warehouse { get; set; }

        /// <summary>
        /// 库位
        /// </summary>
        public string WarehouseLocation { get; set; }
        /// <summary>
        /// 是否已经扫描过（通过Id和工单号判断）
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// 精度
        /// </summary>
        public double unitPrecsion { get; set; }

        /// <summary>
        /// 进位
        /// </summary>
        public int carry { get; set; }
    }
}
