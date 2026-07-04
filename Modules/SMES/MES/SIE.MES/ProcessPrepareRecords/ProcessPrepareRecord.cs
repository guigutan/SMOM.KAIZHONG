using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Items;
using SIE.MES.PrepareProducts;
using SIE.MES.PrepareProducts.Enums;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrder = SIE.MES.WorkOrders.WorkOrder;

namespace SIE.MES.ProcessPrepareRecords
{
    /// <summary>
    /// 工序产前准备记录
    /// </summary>
    [RootEntity, Serializable]
    [Label("工序产前准备记录")]
    public class ProcessPrepareRecord : DataEntity
    {
        #region 产前准备状态 PrepareState
        /// <summary>
        /// 产前准备状态
        /// </summary>
        [Label("产前准备状态")]
        public static readonly Property<PrepareRecordState> PrepareStateProperty = P<ProcessPrepareRecord>.Register(e => e.PrepareState);

        /// <summary>
        /// 产前准备状态
        /// </summary>
        public PrepareRecordState PrepareState
        {
            get { return this.GetProperty(PrepareStateProperty); }
            set { this.SetProperty(PrepareStateProperty, value); }
        }
        #endregion

        #region 产前准备记录明细 PrepareRecordDetail
        /// <summary>
        /// 产前准备记录子表
        /// </summary>
        [Label("产前准备记录明细")]
        public static readonly ListProperty<EntityList<ProcessPrepareRecordDetail>> PrepareRecordDetailProperty = P<ProcessPrepareRecord>.RegisterList(e => e.PrepareRecordDetail);

        /// <summary>
        /// 产前准备记录子表
        /// </summary>
        public EntityList<ProcessPrepareRecordDetail> PrepareRecordDetail
        {
            get { return this.GetLazyList(PrepareRecordDetailProperty); }
        }
        #endregion

    }

    internal class ProcessPrepareRecordConfig : EntityConfig<ProcessPrepareRecord>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("PROCESS_PREPARE_RECORD").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
