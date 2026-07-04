using SIE.Equipments.DeviceIOTParas.Criterias;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Equipments.DeviceIOTParas.Details
{
    /// <summary>
    /// 物联参数
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(PhysicalUnionSelCriteria))]
    [Label("物联参数")]
    public class PhysicalUnionSel : PhysicalUnion
    {

    }
}
