using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.WorkFlow.Base.FlowInstances;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.DataTrace.TraceMainDatas
{
    /// <summary>
    /// 追溯节点-数据模板
    /// </summary>
    [Serializable]
    public class TraceDataPrintDataViewmodel
    {
        /// <summary>
        /// 打印模板
        /// </summary>
        public PrintTemplate? Template { get; set; }

        /// <summary>
        /// 表单数据
        /// </summary>
        //public Entity? FormData { get; set; }

        /// <summary>
        /// 列表数据
        /// </summary>
        public EntityList? ListData { get; set; }
    }

    [Serializable]
    public class TraceFileViewmodel
    {
        /// <summary>
        /// 追溯主数据
        /// </summary>
        public double TraceMainId { get; set; }

        /// <summary>
        /// 追溯主数据单号
        /// </summary>
        public string TraceMainNo { get; set; }

        /// <summary>
        /// 库存组织
        /// </summary>
        public int? InvOrg { get; set; } 
    }
}
