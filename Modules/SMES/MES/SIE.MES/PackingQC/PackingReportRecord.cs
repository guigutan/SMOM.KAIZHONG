using SIE.Domain;
using SIE.MES.BatchGeneration;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.PackingQC
{
    /// <summary>
    /// 包装报工记录
    /// </summary>
    /// <summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(PackingReportRecordCriterial))]
    [Label("包装报工记录")]
    public class PackingReportRecord :DataEntity
    {
        #region 蓝标 BlueLabel
        /// <summary>
        /// 蓝标
        /// </summary>
        [Required]
        [Label("蓝标")]
        public static readonly Property<string> BlueLabelProperty = P<PackingReportRecord>.Register(e => e.BlueLabel);

        /// <summary>
        /// 蓝标
        /// </summary>
        public string BlueLabel
        {
            get { return this.GetProperty(BlueLabelProperty); }
            set { this.SetProperty(BlueLabelProperty, value); }
        }
        #endregion

        #region 包装报工类型 Report
        /// <summary>
        /// 包装报工类型
        /// </summary>
        [Label("包装报工类型")]
        public static readonly Property<ReportType?> ReportProperty = P<PackingReportRecord>.Register(e => e.Report);

        /// <summary>
        /// 包装报工类型
        /// </summary>
        public ReportType? Report
        {
            get { return this.GetProperty(ReportProperty); }
            set { this.SetProperty(ReportProperty, value); }
        }
        #endregion

        #region 返回消息 ReturnMessage
        /// <summary>
        /// 返回消息
        /// </summary>
        [Label("返回消息")]
        public static readonly Property<string> ReturnMessageProperty = P<PackingReportRecord>.Register(e => e.ReturnMessage);

        /// <summary>
        /// 返回消息
        /// </summary>
        public string ReturnMessage
        {
            get { return this.GetProperty(ReturnMessageProperty); }
            set { this.SetProperty(ReturnMessageProperty, value); }
        }
        #endregion

        #region 开始时间 BeginDate
        /// <summary>
        /// 开始时间
        /// </summary>
        [Label("开始时间")]
        public static readonly Property<DateTime?> BeginDateProperty = P<PackingReportRecord>.Register(e => e.BeginDate);

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginDate
        {
            get { return GetProperty(BeginDateProperty); }
            set { SetProperty(BeginDateProperty, value); }
        }
        #endregion

        #region 结束时间 EndDate
        /// <summary>
        /// 结束时间
        /// </summary>
        [Label("结束时间")]
        public static readonly Property<DateTime?> EndDateProperty = P<PackingReportRecord>.Register(e => e.EndDate);

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDate
        {
            get { return GetProperty(EndDateProperty); }
            set { SetProperty(EndDateProperty, value); }
        }
        #endregion
    }

    public class PackingReportRecordConfig : EntityConfig<PackingReportRecord>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("PACKING_REPORT_RECORD").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }

    /// <summary>
    /// QC确认
    /// </summary>
    public enum ReportType
    {
        /// <summary>
        /// 耐压SN报工
        /// </summary>
        [Label("耐压SN报工")]
        PressureSn = 0,

        /// <summary>
        /// 耐压SN批次
        /// </summary>
        [Label("耐压SN批次")]
        PressureSnBatch = 1,

        /// <summary>
        /// 物料标签
        /// </summary>
        [Label("物料标签")]
        ItemLabel = 2,

        /// <summary>
        /// PDA物料标签数量累加
        /// </summary>
        [Label("PDA物料标签数量累加")]
        PDAItemLabelSum = 3,

        /// <summary>
        /// PDA物料标签数量个数
        /// </summary>
        [Label("PDA物料标签数量个数")]
        PDAItemLabelNum = 4,

        /// <summary>
        /// PDA提交
        /// </summary>
        [Label("PDA提交")]
        PDASubmit = 5,

        /// <summary>
        /// 提交
        /// </summary>
        [Label("提交")]
        Submit = 6,

        /// <summary>
        /// QC
        /// </summary>
        [Label("QC确认")]
        QC = 7,

        /// <summary>
        /// 连接器包装
        /// </summary>
        [Label("连接器包装")]
        Connector = 8,
    }
}
