using SIE.Common;
using SIE.Domain;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;

namespace SIE.MES.TaskManagement.Reports
{
    /// <summary>
    /// 报工转入标签
    /// </summary>
    [ChildEntity, Serializable]
    [Label("报工转入标签")]
    public partial class ReportTransferLabel : DataEntity
    {

        #region 派工任务 DispatchTask
        /// <summary>
        /// 派工任务Id
        /// </summary>
        public static readonly IRefIdProperty DispatchTaskIdProperty = P<ReportTransferLabel>.RegisterRefId(e => e.DispatchTaskId, ReferenceType.Parent);

        /// <summary>
        /// 派工任务Id
        /// </summary>
        public double DispatchTaskId
        {
            get { return (double)GetRefId(DispatchTaskIdProperty); }
            set { SetRefId(DispatchTaskIdProperty, value); }
        }

        /// <summary>
        /// 派工任务
        /// </summary>
        public static readonly RefEntityProperty<DispatchTask> DispatchTaskProperty = P<ReportTransferLabel>.RegisterRef(e => e.DispatchTask, DispatchTaskIdProperty);

        /// <summary>
        /// 派工任务
        /// </summary>
        public DispatchTask DispatchTask
        {
            get { return GetRefEntity(DispatchTaskProperty); }
            set { SetRefEntity(DispatchTaskProperty, value); }
        }
        #endregion

        #region 标签号 LabelNo
        /// <summary>
        /// 标签号
        /// </summary>
        [Label("标签号")]
        public static readonly Property<string> LabelNoProperty = P<ReportTransferLabel>.Register(e => e.LabelNo);

        /// <summary>
        /// 标签号
        /// </summary>
        public string LabelNo
        {
            get { return this.GetProperty(LabelNoProperty); }
            set { this.SetProperty(LabelNoProperty, value); }
        }
        #endregion


        #region 标签数量 Qty
        /// <summary>
        /// 标签数量
        /// </summary>
        [Label("标签数量")]
        public static readonly Property<decimal> QtyProperty = P<ReportTransferLabel>.Register(e => e.Qty);

        /// <summary>
        /// 标签数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion



        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<ReportTransferLabel>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 报工转入标签 实体配置
    /// </summary>
    internal class ReportTransferLabelConfig : EntityConfig<ReportTransferLabel>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TM_REPORT_TRANS_LABEL").MapAllProperties();
            Meta.Property(ReportTransferLabel.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}