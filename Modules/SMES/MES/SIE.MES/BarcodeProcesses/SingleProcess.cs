using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.BarcodeProcesses
{
    /// <summary>
    /// 单体工序
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(SingleProcessCriteria))]
    [Label("单体工序")]
    public class SingleProcess : Process
    {
    }
}
