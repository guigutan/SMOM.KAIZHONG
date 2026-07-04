using SIE.Items;
using System;
using System.Collections.Generic;

namespace SIE.MES.WIP.LoadMateriales.ApiModels
{
    /// <summary>
    /// 装配明细对象
    /// </summary>
    [Serializable]
    public class AssemblyDetailViewModel
    {
        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get;
            set;
        }

        /// <summary>
        /// 物料
        /// </summary>
        public string ItemCode
        {
            get;
            set;
        }

        /// <summary>
        /// 需求数量
        /// </summary>
        public decimal DemandQty
        {
            get;
            set;
        }
        /// <summary>
        /// 已扫数量
        /// </summary>
        public decimal Qty
        {
            get;
            set;
        }

        /// <summary>
        /// 物料标签
        /// </summary>
        public string ItemLabel
        {
            get;
            set;
        }
        /// <summary>
        /// 物料标签剩余数量
        /// </summary>
        public decimal RemainQty
        {
            get;
            set;
        }
        /// <summary>
        /// 替代料
        /// </summary>
        public List<AltItemViewModel> AltItemList
        {
            get;
            set;
        }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get;
            set;
        }
        /// <summary>
        /// 替代组合分组
        /// </summary>
        public string AlterGroup
        {
            get;
            set;
        }
    }
}
