using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.MES.BatchWIP
{
    /// <summary>
    /// 生产批次查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("生产批次查询实体")]
    public class WipBatchCriteria : Criteria
    {
        #region 工单编号 WorkOrderNo
        /// <summary>
        /// 工单编号
        /// </summary>
        [Label("工单编号")]
        public static readonly Property<string> WorkOrderNoProperty = P<WipBatchCriteria>.Register(e => e.WorkOrderNo);

        /// <summary>
        /// 工单编号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
            set { this.SetProperty(WorkOrderNoProperty, value); }
        }
        #endregion

        #region 批次号 BatchNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> BatchNoProperty = P<WipBatchCriteria>.Register(e => e.BatchNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
            set { this.SetProperty(BatchNoProperty, value); }
        }
        #endregion 

        #region 资源ID ResourceId
        /// <summary>
        /// 资源ID
        /// </summary>
        [Label("资源ID")]
        public static readonly Property<double> ResourceIdProperty = P<WipBatchCriteria>.Register(e => e.ResourceId);

        /// <summary>
        /// 资源ID
        /// </summary>
        public double ResourceId
        {
            get { return this.GetProperty(ResourceIdProperty); }
            set { this.SetProperty(ResourceIdProperty, value); }
        }
        #endregion

        #region 工序 ProcessId
        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序ID")]
        public static readonly Property<double> ProcessIdProperty = P<WipBatchCriteria>.Register(e => e.ProcessId);

        /// <summary>
        /// 工序
        /// </summary>
        public double ProcessId
        {
            get { return this.GetProperty(ProcessIdProperty); }
            set { this.SetProperty(ProcessIdProperty, value); }
        }
        #endregion

        /// <summary>
        /// 获取生产批次列表
        /// 根据资源过滤工单，
        /// 根据工序判断工单工序在该工序是不是首工序，非首工序过滤
        /// 资源——》工单——》工单工序(是否首工序) 《——工序 
        /// </summary>
        /// <returns>生产批次列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<BatchManageController>().GetWipBatches(this);
        }
    }
}