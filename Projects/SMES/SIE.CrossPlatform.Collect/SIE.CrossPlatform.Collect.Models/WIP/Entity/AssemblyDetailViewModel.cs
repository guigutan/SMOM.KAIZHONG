using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.CrossPlatform.Collect.Models.WIP.Entity
{
    [Serializable]
    public class AssemblyDetailViewModel
    {
        public AssemblyDetailViewModel()
        {

            AltItemList = new List<AltItemViewModel>();
        }
        public double Id { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get;
            set;
        }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get;
            set;
        }

        /// <summary>
        /// 物料编码
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
        /// 物料扩展属性
        /// </summary>
        public string ItemExtPropName
        {
            get;
            set;
        }

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

        /// <summary>
        /// 替代料
        /// </summary>
        public List<AltItemViewModel> AltItemList
        {
            get;
            set;
        }
    }
}
