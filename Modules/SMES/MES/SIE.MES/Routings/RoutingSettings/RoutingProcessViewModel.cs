using SIE.Domain;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using SIE.Tech.Routings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Routings.RoutingSettings
{
    /// <summary>
    /// 产品工艺路线设置-产品工艺路线明细
    /// </summary>
    [RootEntity, Serializable]
    [Label("产品工艺路线明细")]
    public class RoutingProcessViewModel : ViewModel
    {
        #region 工序 RoutingProcess
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty RoutingProcessIdProperty =
            P<RoutingProcessViewModel>.RegisterRefId(e => e.RoutingProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double RoutingProcessId
        {
            get { return (double)this.GetRefId(RoutingProcessIdProperty); }
            set { this.SetRefId(RoutingProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<RoutingProcess> RoutingProcessProperty =
            P<RoutingProcessViewModel>.RegisterRef(e => e.RoutingProcess, RoutingProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public RoutingProcess RoutingProcess
        {
            get { return this.GetRefEntity(RoutingProcessProperty); }
            set { this.SetRefEntity(RoutingProcessProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 顺序号 Index
        /// <summary>
        /// 顺序号
        /// </summary>
        [Label("顺序号")]
        public static readonly Property<string> IndexProperty = P<RoutingProcessViewModel>.RegisterView(e => e.Index, p => p.RoutingProcess.Index);

        /// <summary>
        /// 顺序号
        /// </summary>
        public string Index
        {
            get { return this.GetProperty(IndexProperty); }
        }
        #endregion

        #region 工序 ProcessName
        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static readonly Property<string> ProcessNameProperty = P<RoutingProcessViewModel>.RegisterView(e => e.ProcessName, p => p.RoutingProcess.Name);

        /// <summary>
        /// 工序
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion

        #region 工序类型 ProcessType
        /// <summary>
        /// 工序类型
        /// </summary>
        [Label("工序类型")]
        public static readonly Property<ProcessType?> ProcessTypeProperty = P<RoutingProcessViewModel>.RegisterView(e => e.ProcessType, p => p.RoutingProcess.Process.Type);

        /// <summary>
        /// 工序类型
        /// </summary>
        public ProcessType? ProcessType
        {
            get { return this.GetProperty(ProcessTypeProperty); }
        }
        #endregion

        #region 工段名称 SegmentName
        /// <summary>
        /// 工段名称
        /// </summary>
        [Label("工段名称")]
        public static readonly Property<string> SegmentNameProperty = P<RoutingProcessViewModel>.RegisterView(e => e.SegmentName, p => p.RoutingProcess.Process.Segment.Name);

        /// <summary>
        /// 工段名称
        /// </summary>
        public string SegmentName
        {
            get { return this.GetProperty(SegmentNameProperty); }
        }
        #endregion

        #region 是否可选 IsOptional
        /// <summary>
        /// 是否可选
        /// </summary>
        [Label("是否可选")]
        public static readonly Property<bool> IsOptionalProperty = P<RoutingProcessViewModel>.RegisterView(e => e.IsOptional, p => p.RoutingProcess.IsOptional);

        /// <summary>
        /// 是否可选
        /// </summary>
        public bool IsOptional
        {
            get { return this.GetProperty(IsOptionalProperty); }
        }
        #endregion

        #region 是否委外 Outsourcing
        /// <summary>
        /// 是否委外
        /// </summary>
        [Label("是否委外")]
        public static readonly Property<bool> OutsourcingProperty = P<RoutingProcessViewModel>.RegisterView(e => e.Outsourcing, p => p.RoutingProcess.Outsourcing);

        /// <summary>
        /// 是否委外
        /// </summary>
        public bool Outsourcing
        {
            get { return this.GetProperty(OutsourcingProperty); }
        }
        #endregion

        #region 是否生成工序任务单 IsGenerateTask
        /// <summary>
        /// 是否生成工序任务单
        /// </summary>
        [Label("是否生成工序任务单")]
        public static readonly Property<bool> IsGenerateTaskProperty = P<RoutingProcessViewModel>.RegisterView(e => e.IsGenerateTask, p => p.RoutingProcess.IsGenerateTask);

        /// <summary>
        /// 是否生成工序任务单
        /// </summary>
        public bool IsGenerateTask
        {
            get { return this.GetProperty(IsGenerateTaskProperty); }
        }
        #endregion

        #region 是否需求任务清单 IsRequirementTask
        /// <summary>
        /// 是否需求任务清单
        /// </summary>
        [Label("是否需求任务清单")]
        public static readonly Property<bool> IsRequirementTaskProperty = P<RoutingProcessViewModel>.RegisterView(e => e.IsRequirementTask, p => p.RoutingProcess.IsRequirementTask);

        /// <summary>
        /// 是否需求任务清单
        /// </summary>
        public bool IsRequirementTask
        {
            get { return this.GetProperty(IsRequirementTaskProperty); }
        }
        #endregion
        
        #endregion
    }
}
