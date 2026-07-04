using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
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
    /// 追溯主数据
    /// </summary>
    [Serializable]
    public class TraceDataPrintTempViewmodel
    {
        /// <summary>
        /// 路径
        /// </summary>
        public string path { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public byte[] Content { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
    }
}
