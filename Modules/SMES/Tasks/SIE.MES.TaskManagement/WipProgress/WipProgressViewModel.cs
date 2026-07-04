using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TaskManagement.WipProgress
{
    /// <summary>
    /// 在制品查询
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(WipProgressViewModelCriteria))]
    [Label("在制品查询")]
    public class WipProgressViewModel : ViewModel
    {
        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<WipProgressViewModel>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 工序号 ProcessSeq
        /// <summary>
        /// 工序号
        /// </summary>
        [Label("工序号")]
        public static readonly Property<string> ProcessSeqProperty = P<WipProgressViewModel>.Register(e => e.ProcessSeq);

        /// <summary>
        /// 工序号
        /// </summary>
        public string ProcessSeq
        {
            get { return this.GetProperty(ProcessSeqProperty); }
            set { this.SetProperty(ProcessSeqProperty, value); }
        }
        #endregion

        #region 工序Id ProcessId
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序Id")]
        public static readonly Property<double> ProcessIdProperty = P<WipProgressViewModel>.Register(e => e.ProcessId);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId
        {
            get { return this.GetProperty(ProcessIdProperty); }
            set { this.SetProperty(ProcessIdProperty, value); }
        }
        #endregion

        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<WipProgressViewModel>.Register(e => e.ProcessCode);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
            set { this.SetProperty(ProcessCodeProperty, value); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<WipProgressViewModel>.Register(e => e.ProcessName);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
            set { this.SetProperty(ProcessNameProperty, value); }
        }
        #endregion

        #region 是否首工序 IsStartProcess
        /// <summary>
        /// 是否首工序
        /// </summary>
        [Label("是否首工序")]
        public static readonly Property<bool> IsStartProcessProperty = P<WipProgressViewModel>.Register(e => e.IsStartProcess);

        /// <summary>
        /// 是否首工序
        /// </summary>
        public bool IsStartProcess
        {
            get { return this.GetProperty(IsStartProcessProperty); }
            set { this.SetProperty(IsStartProcessProperty, value); }
        }
        #endregion

        #region 旧物料号 OldItem
        /// <summary>
        /// 旧物料号
        /// </summary>
        [Label("旧物料号")]
        public static readonly Property<string> OldItemProperty = P<WipProgressViewModel>.Register(e => e.OldItem);

        /// <summary>
        /// 旧物料号
        /// </summary>
        public string OldItem
        {
            get { return this.GetProperty(OldItemProperty); }
            set { this.SetProperty(OldItemProperty, value); }
        }
        #endregion

        #region 父级旧物料号 ParentOldItem
        /// <summary>
        /// 父级旧物料号
        /// </summary>
        [Label("父级旧物料号")]
        public static readonly Property<string> ParentOldItemProperty = P<WipProgressViewModel>.Register(e => e.ParentOldItem);

        /// <summary>
        /// 父级旧物料号
        /// </summary>
        public string ParentOldItem
        {
            get { return this.GetProperty(ParentOldItemProperty); }
            set { this.SetProperty(ParentOldItemProperty, value); }
        }
        #endregion

        #region 工序状态 ProcessStatus
        /// <summary>
        /// 工序状态
        /// </summary>
        [Label("工序状态")]
        public static readonly Property<string> ProcessStatusProperty = P<WipProgressViewModel>.Register(e => e.ProcessStatus);

        /// <summary>
        /// 工序状态
        /// </summary>
        public string ProcessStatus
        {
            get { return this.GetProperty(ProcessStatusProperty); }
            set { this.SetProperty(ProcessStatusProperty, value); }
        }
        #endregion

        #region 批次标签 BatchNo
        /// <summary>
        /// 批次标签
        /// </summary>
        [Label("批次标签")]
        public static readonly Property<string> BatchNoProperty = P<WipProgressViewModel>.Register(e => e.BatchNo);

        /// <summary>
        /// 批次标签
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
            set { this.SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 工序数量 PlanQty
        /// <summary>
        /// 工序数量
        /// </summary>
        [Label("工序数量")]
        public static readonly Property<decimal> PlanQtyProperty = P<WipProgressViewModel>.Register(e => e.PlanQty);

        /// <summary>
        /// 工序数量
        /// </summary>
        public decimal PlanQty
        {
            get { return this.GetProperty(PlanQtyProperty); }
            set { this.SetProperty(PlanQtyProperty, value); }
        }
        #endregion

        #region 合格数量 OkQty
        /// <summary>
        /// 合格数量
        /// </summary>
        [Label("合格数量")]
        public static readonly Property<decimal> OkQtyProperty = P<WipProgressViewModel>.Register(e => e.OkQty);

        /// <summary>
        /// 合格数量
        /// </summary>
        public decimal OkQty
        {
            get { return this.GetProperty(OkQtyProperty); }
            set { this.SetProperty(OkQtyProperty, value); }
        }
        #endregion

        #region 工序完工数量 FinishQty
        /// <summary>
        /// 工序完工数量
        /// </summary>
        [Label("工序完工数量")]
        public static readonly Property<decimal> FinishQtyProperty = P<WipProgressViewModel>.Register(e => e.FinishQty);

        /// <summary>
        /// 工序完工数量
        /// </summary>
        public decimal FinishQty
        {
            get { return this.GetProperty(FinishQtyProperty); }
            set { this.SetProperty(FinishQtyProperty, value); }
        }
        #endregion

        #region 上一工序完工数量 PreFinishQty
        /// <summary>
        /// 上一工序完工数量
        /// </summary>
        [Label("上一工序完工数量")]
        public static readonly Property<decimal> PreFinishQtyProperty = P<WipProgressViewModel>.Register(e => e.PreFinishQty);

        /// <summary>
        /// 上一工序完工数量
        /// </summary>
        public decimal PreFinishQty
        {
            get { return this.GetProperty(PreFinishQtyProperty); }
            set { this.SetProperty(PreFinishQtyProperty, value); }
        }
        #endregion

        #region 上一工序合格数量 PreOkQty
        /// <summary>
        /// 上一工序合格数量
        /// </summary>
        [Label("上一工序合格数量")]
        public static readonly Property<decimal> PreOkQtyProperty = P<WipProgressViewModel>.Register(e => e.PreOkQty);

        /// <summary>
        /// 上一工序合格数量
        /// </summary>
        public decimal PreOkQty
        {
            get { return this.GetProperty(PreOkQtyProperty); }
            set { this.SetProperty(PreOkQtyProperty, value); }
        }
        #endregion

        #region 前置工序编码 PreProcessCode
        /// <summary>
        /// 前置工序编码
        /// </summary>
        [Label("前置工序编码")]
        public static readonly Property<string> PreProcessCodeProperty = P<WipProgressViewModel>.Register(e => e.PreProcessCode);

        /// <summary>
        /// 前置工序编码
        /// </summary>
        public string PreProcessCode
        {
            get { return this.GetProperty(PreProcessCodeProperty); }
            set { this.SetProperty(PreProcessCodeProperty, value); }
        }
        #endregion

        #region 在制数量 InProcessQty
        /// <summary>
        /// 在制数量
        /// </summary>
        [Label("在制数量")]
        public static readonly Property<decimal> InProcessQtyProperty = P<WipProgressViewModel>.RegisterReadOnly(
            e => e.InProcessQty, e => e.GetInProcessQty(), FinishQtyProperty, PreFinishQtyProperty);
        /// <summary>
        /// 在制数量
        /// </summary>

        public decimal InProcessQty
        {
            get { return this.GetProperty(InProcessQtyProperty); }
        }
        private decimal GetInProcessQty()
        {
            var qty = 0m;
            if(IsStartProcess)
                qty = PlanQty - FinishQty;
            else
                qty = PreOkQty - FinishQty;
            return qty<=0?0:qty;
        }
        #endregion



    }

}