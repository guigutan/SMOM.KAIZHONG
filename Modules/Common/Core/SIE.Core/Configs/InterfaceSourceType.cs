using SIE.ObjectModel;

namespace SIE.Core.Configs
{
    public enum InterfaceSourceType
    {
        /// <summary>
        /// 来源EBS
        /// </summary>
        [Label("EBS")]
        EBS = 0,

        /// <summary>
        /// 来源SAP
        /// </summary>
        [Label("SAP")]
        SAP = 1,
    }
}
