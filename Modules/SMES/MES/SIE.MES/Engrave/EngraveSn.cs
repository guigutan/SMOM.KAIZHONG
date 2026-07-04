using SIE.Domain;
using SIE.MES.WIP.Pressure;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Engrave
{
    /// <summary>
    /// 刻码SN
    /// </summary>
    [RootEntity, Serializable]
    [Label("刻码SN")]
    public partial class EngraveSn :DataEntity
    {
        public EngraveSn()
        { 
        }

        #region 耐压测试批次 EngraveLabel
        /// <summary>
        /// 耐压测试批次Id
        /// </summary>
        [Label("耐压测试批次")]
        public static readonly IRefIdProperty EngraveLabelIdProperty =
            P<EngraveSn>.RegisterRefId(e => e.EngraveLabelId, ReferenceType.Parent);

        /// <summary>
        /// 耐压测试批次Id
        /// </summary>
        public double EngraveLabelId
        {
            get { return (double)this.GetRefId(EngraveLabelIdProperty); }
            set { this.SetRefId(EngraveLabelIdProperty, value); }
        }

        /// <summary>
        /// 耐压测试批次
        /// </summary>
        public static readonly RefEntityProperty<EngraveLabel> EngraveLabelProperty =
            P<EngraveSn>.RegisterRef(e => e.EngraveLabel, EngraveLabelIdProperty);

        /// <summary>
        /// 耐压测试批次
        /// </summary>
        public EngraveLabel EngraveLabel
        {
            get { return this.GetRefEntity(EngraveLabelProperty); }
            set { this.SetRefEntity(EngraveLabelProperty, value); }
        }
        #endregion

        #region SN Sn
        /// <summary>
        /// SN
        /// </summary>
        [Label("SN")]
        public static readonly Property<string> SnProperty = P<EngraveSn>.Register(e => e.Sn);

        /// <summary>
        /// SN
        /// </summary>
        public string Sn
        {
            get { return this.GetProperty(SnProperty); }
            set { this.SetProperty(SnProperty, value); }
        }
        #endregion

        #region 工序标签 BatchNo
        /// <summary>
        /// 工序标签
        /// </summary>
        [Label("工序标签")]
        public static readonly Property<string> BatchNoProperty = P<EngraveSn>.RegisterView(e => e.BatchNo, p => p.EngraveLabel.BatchNo);

        /// <summary>
        /// 工序标签
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
        }

        #endregion

        #region 工单Id WorkOrderId
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单Id")]
        public static readonly Property<double> WorkOrderIdProperty = P<EngraveSn>.RegisterView(e => e.WorkOrderId, p => p.EngraveLabel.WorkOrderId);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return this.GetProperty(WorkOrderIdProperty); }
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<EngraveSn>.RegisterView(e => e.WorkOrderNo, p => p.EngraveLabel.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion
    }
    internal class EngraveSnEntityConfig : EntityConfig<EngraveSn>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("ENGRAVE_LABEL_SN").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
