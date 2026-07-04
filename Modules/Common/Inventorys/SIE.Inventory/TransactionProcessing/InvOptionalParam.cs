using SIE.Inventory.Onhands;
using System;
using System.Collections.Generic;

namespace SIE.Inventory.TransactionProcessing
{
    /// <summary>
    /// 库存查询可选参数（如果为空，默认保存为空格）:货主、LPN、项目号、任务号
    /// </summary>
    public class InvOptionalParamBase
    {
        /// <summary>
        /// 货主
        /// </summary>
        private string storerCode = string.Empty;

        /// <summary>
        /// 货主
        /// </summary>
        public string StorerCode
        {
            get { return storerCode.IsNullOrEmpty() ? "*" : storerCode; }
            set { storerCode = value; }
        }

        /// <summary>
        /// LPN
        /// </summary>
        string lpn = string.Empty;

        /// <summary>
        /// LPN
        /// </summary>
        public string Lpn
        {
            get { return lpn.IsNullOrEmpty() ? "*" : lpn; }
            set { lpn = value; }
        }

        /// <summary>
        /// 项目号
        /// </summary>
        string projectNo = string.Empty;

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo
        {
            get { return projectNo.IsNullOrEmpty() ? "*" : projectNo; }
            set { projectNo = value; }
        }

        /// <summary>
        /// 任务号
        /// </summary>
        string taskNo = string.Empty;

        /// <summary>
        /// 任务号
        /// </summary>
        public string TaskNo
        {
            get { return taskNo.IsNullOrEmpty() ? "*" : taskNo; }
            set { taskNo = value; }
        }

        /// <summary>
        /// 库存状态
        /// </summary>
        public OnhandState State { get; set; }
    }

    /// <summary>
    /// 库存查询可选参数（如果为空，默认保存为空格）:货主、LPN、项目号、任务号
    /// </summary>
    public class InvOptionalParam : InvOptionalParamBase
    {
        /// <summary>
        /// 静态实例化
        /// </summary>
        public static readonly InvOptionalParam Empty = new InvOptionalParam();
       
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp { get; set; } = string.Empty;

        /// <summary>
        /// 物料扩展属性名称
        /// </summary>
        public string ItemExtPropName { get; set; } = string.Empty;

        /// <summary>
        /// 是否忽略扩展属性
        /// </summary>
        public bool IsIgnoreItemExtProp { get; set; }

        /// <summary>
        /// Lpn库存
        /// </summary>
        public List<LotLpnOnhand> LotLpnOnhands { get; set; }

        ///// <summary>
        ///// 库位库存
        ///// </summary>
        //public List<LocationOnhand> LocOnhands { get; set; }

        ///// <summary>
        ///// 批次库存
        ///// </summary>
        //public List<LotOnhand> LotOnhands { get; set; }

    }

    /// <summary>
    /// 分配库存性能优化
    /// </summary>
    public class InvOptionalParamForOp : InvOptionalParamBase
    {
        /// <summary>
        /// 分配库存的时候使用，不需要赋值
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 来源库位
        /// </summary>
        public double LocId { get; set; }

        /// <summary>
        /// 目标库位
        /// </summary>
        public double TarLocId { get; set; }
    }
}
