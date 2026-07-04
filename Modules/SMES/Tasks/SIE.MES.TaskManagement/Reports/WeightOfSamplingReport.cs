using DocumentFormat.OpenXml.Drawing.Charts;
using SIE.Domain;
using SIE.Domain.Query;
using SIE.Items;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Reports
{
    /// <summary>
    /// 取样净重详情表
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(WeightOfSamplingReportCriteria))]
    [Label("取样净重详情表")]
    public class WeightOfSamplingReport : DataEntity
    {
        #region 任务单 DispatchTask
        /// <summary>
        /// 任务单Id
        /// </summary>
        [Label("任务单")]
        public static readonly IRefIdProperty DispatchTaskIdProperty =
            P<WeightOfSamplingReport>.RegisterRefId(e => e.DispatchTaskId, ReferenceType.Normal);

        /// <summary>
        /// 任务单Id
        /// </summary>
        public double DispatchTaskId
        {
            get { return (double)this.GetRefId(DispatchTaskIdProperty); }
            set { this.SetRefId(DispatchTaskIdProperty, value); }
        }

        /// <summary>
        /// 任务单
        /// </summary>
        public static readonly RefEntityProperty<DispatchTask> DispatchTaskProperty =
            P<WeightOfSamplingReport>.RegisterRef(e => e.DispatchTask, DispatchTaskIdProperty);

        /// <summary>
        /// 任务单
        /// </summary>
        public DispatchTask DispatchTask
        {
            get { return this.GetRefEntity(DispatchTaskProperty); }
            set { this.SetRefEntity(DispatchTaskProperty, value); }
        }
        #endregion

        #region 工序BOM WorkOrderProcessBom
        /// <summary>
        /// 工序BOMId
        /// </summary>
        [Label("工序BOM")]
        public static readonly IRefIdProperty WorkOrderProcessBomIdProperty =
            P<WeightOfSamplingReport>.RegisterRefId(e => e.WorkOrderProcessBomId, ReferenceType.Normal);

        /// <summary>
        /// 工序BOMId
        /// </summary>
        public double? WorkOrderProcessBomId
        {
            get { return (double?)this.GetRefId(WorkOrderProcessBomIdProperty); }
            set { this.SetRefId(WorkOrderProcessBomIdProperty, value); }
        }

        /// <summary>
        /// 工序BOM
        /// </summary>
        public static readonly RefEntityProperty<WorkOrderProcessBom> WorkOrderProcessBomProperty =
            P<WeightOfSamplingReport>.RegisterRef(e => e.WorkOrderProcessBom, WorkOrderProcessBomIdProperty);

        /// <summary>
        /// 工序BOM
        /// </summary>
        public WorkOrderProcessBom WorkOrderProcessBom
        {
            get { return this.GetRefEntity(WorkOrderProcessBomProperty); }
            set { this.SetRefEntity(WorkOrderProcessBomProperty, value); }
        }
        #endregion

        #region 取样净重 Weight
        /// <summary>
        /// 取样净重
        /// </summary>
        [Label("取样净重")]
        public static readonly Property<decimal?> WeightProperty = P<WeightOfSamplingReport>.Register(e => e.Weight);

        /// <summary>
        /// 取样净重
        /// </summary>
        public decimal? Weight
        {
            get { return this.GetProperty(WeightProperty); }
            set { this.SetProperty(WeightProperty, value); }
        }
        #endregion

        #region 原取样净重 OldWeight
        /// <summary>
        /// 原取样净重
        /// </summary>
        [Label("原取样净重")]
        public static readonly Property<decimal?> OldWeightProperty = P<WeightOfSamplingReport>.Register(e => e.OldWeight);

        /// <summary>
        /// 原取样净重
        /// </summary>
        public decimal? OldWeight
        {
            get { return this.GetProperty(OldWeightProperty); }
            set { this.SetProperty(OldWeightProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<WeightOfSamplingReport>.RegisterView(e => e.WorkOrderNo, p => p.DispatchTask.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<WeightOfSamplingReport>.RegisterView(e => e.ProductCode, p => p.DispatchTask.Product.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<WeightOfSamplingReport>.RegisterView(e => e.ProductName, p => p.DispatchTask.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion

        #region 单位 Unit
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitProperty = P<WeightOfSamplingReport>.RegisterView(e => e.Unit, p => p.DispatchTask.Product.Unit.Code);

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit
        {
            get { return this.GetProperty(UnitProperty); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static readonly Property<string> ProcessProperty = P<WeightOfSamplingReport>.RegisterView(e => e.Process, p => p.WorkOrderProcessBom.Process.Code);

        /// <summary>
        /// 工序
        /// </summary>
        public string Process
        {
            get { return this.GetProperty(ProcessProperty); }
        }
        #endregion

        #region 计划数量 PlanQty
        /// <summary>
        /// 计划数量
        /// </summary>
        [Label("计划数量")]
        public static readonly Property<decimal> PlanQtyProperty = P<WeightOfSamplingReport>.RegisterView(e => e.PlanQty, p => p.DispatchTask.WorkOrder.PlanQty);

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanQty
        {
            get { return this.GetProperty(PlanQtyProperty); }
        }
        #endregion

        #endregion

    }

    internal class WeightOfSamplingReportConfig : EntityConfig<WeightOfSamplingReport>
    {
        protected override void ConfigMeta()
        {

            Meta.MapTable("WEIGHT_OF_SAMP_REPORT").MapAllProperties();
            Meta.IsTreeEntity = false;
            Meta.DisablePhantoms();
        }
    }
}
