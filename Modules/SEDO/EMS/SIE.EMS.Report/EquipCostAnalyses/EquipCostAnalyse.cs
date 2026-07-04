using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Report.EquipCostAnalyses
{
    /// <summary>
    /// 设备成本分析
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(EquipCostAnalyseCriteria))]
    [Label("设备成本分析")]
    public class EquipCostAnalyse : Entity<double>
    {

    }
}
