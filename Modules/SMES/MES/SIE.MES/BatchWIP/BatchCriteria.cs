using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.MES.BatchWIP
{
    /// <summary>
    /// 批次产品工艺路线查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("批次产品工艺路线查询实体")]
    public class BatchCriteria : Criteria
    {
        #region 工单编号 WorkOrderNo
        /// <summary>
        /// 工单编号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<BatchCriteria>.Register(e => e.WorkOrderNo);

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
        [Label("子批次")]
        public static readonly Property<string> BatchNoProperty = P<BatchCriteria>.Register(e => e.BatchNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
            set { this.SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 生产批次 WipBatchNo
        /// <summary>
        /// 生产批次
        /// </summary>
        [Label("生产批次")]
        public static readonly Property<string> WipBatchNoProperty = P<BatchCriteria>.Register(e => e.WipBatchNo);

        /// <summary>
        /// 生产批次
        /// </summary>
        public string WipBatchNo
        {
            get { return this.GetProperty(WipBatchNoProperty); }
            set { this.SetProperty(WipBatchNoProperty, value); }
        }
        #endregion 

        /// <summary>
        /// 获取批次列表 
        /// </summary>
        /// <returns>生产批次列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<BatchManageController>().CriteriaGetBatches(this);
        }
    }
}