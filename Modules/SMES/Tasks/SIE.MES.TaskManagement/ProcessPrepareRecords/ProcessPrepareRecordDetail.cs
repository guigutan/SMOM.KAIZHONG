using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.ProcessPrepareRecords
{
    [ChildEntity,Serializable]
    public class ProcessPrepareRecordDetail : SIE.MES.ProcessPrepareRecords.ProcessPrepareRecordDetail
    {
        #region 产前准备记录 PrepareRecord
        /// <summary>
        /// 产前准备记录Id
        /// </summary>
        [Label("产前准备记录")]
        public static readonly IRefIdProperty PrepareRecordIdProperty =
            P<ProcessPrepareRecordDetail>.RegisterRefId(e => e.PrepareRecordId, ReferenceType.Parent);

        /// <summary>
        /// 产前准备记录Id
        /// </summary>
        public double PrepareRecordId
        {
            get { return (double)this.GetRefId(PrepareRecordIdProperty); }
            set { this.SetRefId(PrepareRecordIdProperty, value); }
        }

        /// <summary>
        /// 产前准备记录
        /// </summary>
        public static readonly RefEntityProperty<ProcessPrepareRecord> PrepareRecordProperty =
            P<ProcessPrepareRecordDetail>.RegisterRef(e => e.PrepareRecord, PrepareRecordIdProperty);

        /// <summary>
        /// 产前准备记录
        /// </summary>
        public ProcessPrepareRecord PrepareRecord
        {
            get { return this.GetRefEntity(PrepareRecordProperty); }
            set { this.SetRefEntity(PrepareRecordProperty, value); }
        }
        #endregion

    }
}
