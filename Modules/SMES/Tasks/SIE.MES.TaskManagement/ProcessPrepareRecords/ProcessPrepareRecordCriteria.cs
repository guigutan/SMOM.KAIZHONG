using SIE.Domain;
using SIE.MES.PrepareProducts.Enums;
using SIE.MES.ProcessPrepareRecords;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.ProcessPrepareRecords
{
    /// <summary>
    /// 工序产前准备记录查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("工序产前准备记录查询实体")]
    public class ProcessPrepareRecordCriteria : SIE.MES.ProcessPrepareRecords.ProcessPrepareRecordCriteria
    {
        #region 派工任务执行对象 TaskPerformer
        /// <summary>
        /// 派工任务执行对象
        /// </summary>
        [Label("派工任务执行对象")]
        public static readonly Property<string> TaskPerformerProperty = P<ProcessPrepareRecordCriteria>.Register(e => e.TaskPerformer);

        /// <summary>
        /// 派工任务执行对象
        /// </summary>
        public string TaskPerformer
        {
            get { return this.GetProperty(TaskPerformerProperty); }
            set { this.SetProperty(TaskPerformerProperty, value); }
        }
        #endregion


        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ProcessPrepareRecordsController>().QueryPrepareRecordList(this);
        }
    }
}
