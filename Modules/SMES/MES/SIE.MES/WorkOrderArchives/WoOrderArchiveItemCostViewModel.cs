using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.WorkOrderArchives
{
    /// <summary>
    /// 工单制造档案物料耗用
    /// </summary>
    [RootEntity, Serializable]
    [Label("工单制造档案物料耗用")]
    public class WoOrderArchiveItemCostViewModel : ViewModel
    {
        #region 工序 ProcessName
        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static readonly Property<string> ProcessNameProperty = P<WoOrderArchiveItemCostViewModel>.Register(e => e.ProcessName);

        /// <summary>
        /// 工序
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
            set { this.SetProperty(ProcessNameProperty, value); }
        }
        #endregion

        #region 工步 WorkStep
        /// <summary>
        /// 工步
        /// </summary>
        [Label("工步")]
        public static readonly Property<string> WorkStepProperty = P<WoOrderArchiveItemCostViewModel>.Register(e => e.WorkStep);

        /// <summary>
        /// 工步
        /// </summary>
        public string WorkStep
        {
            get { return this.GetProperty(WorkStepProperty); }
            set { this.SetProperty(WorkStepProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<WoOrderArchiveItemCostViewModel>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<WoOrderArchiveItemCostViewModel>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 物料拓展属性 ItemExPro
        /// <summary>
        /// 物料拓展属性
        /// </summary>
        [Label("物料拓展属性")]
        public static readonly Property<string> ItemExProProperty = P<WoOrderArchiveItemCostViewModel>.Register(e => e.ItemExPro);

        /// <summary>
        /// 物料拓展属性
        /// </summary>
        public string ItemExPro
        {
            get { return this.GetProperty(ItemExProProperty); }
            set { this.SetProperty(ItemExProProperty, value); }
        }
        #endregion

        #region 单位耗用量 SingleQty
        /// <summary>
        /// 单位耗用量
        /// </summary>
        [Label("单位耗用量")]
        public static readonly Property<decimal> SingleQtyProperty = P<WoOrderArchiveItemCostViewModel>.Register(e => e.SingleQty);

        /// <summary>
        /// 单位耗用量
        /// </summary>
        public decimal SingleQty
        {
            get { return this.GetProperty(SingleQtyProperty); }
            set { this.SetProperty(SingleQtyProperty, value); }
        }
        #endregion

        #region 需求数 RequireQty
        /// <summary>
        /// 需求数
        /// </summary>
        [Label("需求数")]
        public static readonly Property<decimal> RequireQtyProperty = P<WoOrderArchiveItemCostViewModel>.Register(e => e.RequireQty);

        /// <summary>
        /// 需求数
        /// </summary>
        public decimal RequireQty
        {
            get { return this.GetProperty(RequireQtyProperty); }
            set { this.SetProperty(RequireQtyProperty, value); }
        }
        #endregion

        #region 总耗用量 TotalQty
        /// <summary>
        /// 总耗用量
        /// </summary>
        [Label("总耗用量")]
        public static readonly Property<decimal> TotalQtyProperty = P<WoOrderArchiveItemCostViewModel>.Register(e => e.TotalQty);

        /// <summary>
        /// 总耗用量
        /// </summary>
        public decimal TotalQty
        {
            get { return this.GetProperty(TotalQtyProperty); }
            set { this.SetProperty(TotalQtyProperty, value); }
        }
        #endregion

    }
}
