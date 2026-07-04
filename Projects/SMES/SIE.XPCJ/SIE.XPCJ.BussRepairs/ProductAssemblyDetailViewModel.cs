using SIE.XPCJ.Common.Exceptions;
using SIE.XPCJ.Common.Extensions;
using SIE.XPCJ.Models;
using SIE.XPCJ.Models.Enums;
using SIE.XPCJ.Models.WIP;
using SIE.XPCJ.WIP.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.BussRepairs
{
    [Serializable]
    public class ProductAssemblyDetailViewModel
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 当前工作单元
        /// </summary>
        public Workcell Workcell { get; set; }

        public string TreePId { get; set; }

        /// <summary>
        /// 原标签
        /// </summary>
        public string SourceCode
        {
            get; set;
        }

        /// <summary>
        /// 关键件
        /// </summary>
        public WipProductProcessKeyItem KeyItem
        {
            get; set;
        }

        /// <summary>
        /// 待换料条码
        /// </summary>
        public string Barcode
        {
            get; set;
        }

        /// <summary>
        /// 换料数量
        /// </summary>
        public decimal ChangeQty
        {
            get; set;
        }

        /// <summary> 
        /// 换料列表
        /// </summary>
        public List<ChangeItemViewModel> ChangeItemViewModelList { get; set; } = new List<ChangeItemViewModel>();
        /// <summary>
        /// 是否换料
        /// </summary>
        public bool IsChangeSn
        {
            get; set;
        }

        /// <summary>
        /// 换料后标签
        /// </summary>
        public string ChangeBarcode
        {
            get; set;
        }

        /// <summary>
        /// 工单ID
        /// </summary>
        public double? WorkOrderId
        {
            get; set;
        }


        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get; set;
        }

        /// <summary>
        /// 总换料数量
        /// </summary>
        public decimal TotalChangeQty
        {
            get; set;
        }

        /// <summary>
        /// 置换后处理
        /// </summary>
        public ChangeItemHandleMethod HandleMethod
        {
            get; set;
        }
        /// <summary>
        /// 物料标签
        /// </summary>
        public string KeyItemItemCode
        {
            get; set;
        }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string KeyItemItemName
        {
            get; set;
        }



        /// <summary>
        /// 工序
        /// </summary>
        public string ProcessName
        {
            get; set;
        }

    }

    [Serializable]
    public class ChangeItemViewModel
    {
        /// <summary>
        /// 数据ID 用于删除
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 换料条码
        /// </summary>
        public string ChangeSn
        {
            get;
            set;
        }


        /// <summary>
        /// 换料数量
        /// </summary>
        public decimal ChangeQty
        {
            get;
            set;
        }
        /// <summary>
        /// 是否已上料
        /// </summary>
        public bool IsLoadItem
        {
            get;
            set;
        }

        /// <summary>
        /// 上料条码信息
        /// </summary>
        public LoadItemBarcodeInfo LoadItemBarcodeInfo
        {
            get;
            set;
        }
    }
}
