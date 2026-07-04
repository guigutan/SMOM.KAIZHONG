using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.WipProgress
{
    /// <summary>
    /// 在制品标签查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("在制品标签查询实体")]
    public class WipProgressWipBatchCriteria : Criteria
    {
        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<WipProgressWipBatchCriteria>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 批次标签 BatchNo
        /// <summary>
        /// 批次标签
        /// </summary>
        [Label("批次标签")]
        public static readonly Property<string> BatchNoProperty = P<WipProgressWipBatchCriteria>.Register(e => e.BatchNo);

        /// <summary>
        /// 批次标签
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
            set { this.SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 工序Id ProcessId
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序Id")]
        public static readonly Property<double> ProcessIdProperty = P<WipProgressWipBatchCriteria>.Register(e => e.ProcessId);

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
        public static readonly Property<string> ProcessCodeProperty = P<WipProgressWipBatchCriteria>.Register(e => e.ProcessCode);

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
        public static readonly Property<string> ProcessNameProperty = P<WipProgressWipBatchCriteria>.Register(e => e.ProcessName);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
            set { this.SetProperty(ProcessNameProperty, value); }
        }
        #endregion

        #region 前置工序编码 PreProcessCode
        /// <summary>
        /// 前置工序编码
        /// </summary>
        [Label("前置工序编码")]
        public static readonly Property<string> PreProcessCodeProperty = P<WipProgressWipBatchCriteria>.Register(e => e.PreProcessCode);

        /// <summary>
        /// 前置工序编码
        /// </summary>
        public string PreProcessCode
        {
            get { return this.GetProperty(PreProcessCodeProperty); }
            set { this.SetProperty(PreProcessCodeProperty, value); }
        }
        #endregion


        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<WipProgressViewModelController>().GetWipProgressWipBatchs(this);
        }

    }
}
