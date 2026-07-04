using SIE.Common.Algorithm;
using System;

namespace SIE.Core.Algorithms
{
    [Algorithm("库存组织编码段", typeof(CodeAlgorithmConfig), AlgorithmType.Common)]
    [RootEntity, Serializable]
    internal class InvOrgSegmentAlgorithm : EntityCodeAlgorithm
    {
        public override string GetCode()
        {
            return RT.InvOrg.ToString();
        }
    }
}
