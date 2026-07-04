using SIE.Barcodes.WipBatchs;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TaskManagement.WipProgress
{
    /// <summary>
    /// 在制品标签
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(WipProgressWipBatchCriteria))]
    [Label("在制品标签")]
    public class WipProgressWipBatch : WipBatch
    {

    }

}