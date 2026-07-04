using SIE.ObjectModel;
using System.ComponentModel;

namespace SIE.Core.Inspections
{
    /// <summary>
    /// 检验类型
    /// </summary>
    public enum InspectionType
    {
        #region IQC
        /// <summary>
        /// 供应商检验
        /// </summary>
        [Category("IQC")]
        [Label("供应商检验")]
        SupplierInsp = 0,

        /// <summary>
        /// 驻厂检验
        /// </summary>
        [Category("IQC")]
        [Label("驻厂检验")]
        FactoryInsp = 1,

        /// <summary>
        /// 来料检验
        /// </summary>
        [Category("IQC")]
        [Label("来料检验")]
        IncomingInsp = 2,

        /// <summary>
        /// 来料检验
        /// </summary>
        [Category("IQC")]
        [Label("来料挑选")]
        IncomingPick = 3,
        #endregion

        #region OQC
        /// <summary>
        /// 成品检验
        /// </summary>
        [Category("OQC")]
        [Label("成品检验")]
        ShippingInsp = 30,

        /// <summary>
        /// 出货检验
        /// </summary>
        [Category("OQC")]
        [Label("出货检验")]
        OobInsp = 31,
        #endregion

        #region PQC
        /// <summary>
        /// 首检
        /// </summary>
        [Category("PQC")]
        [Label("首检")]
        FirstInsp = 20,

        /// <summary>
        /// 巡检
        /// </summary>
        [Category("PQC")]
        [Label("巡检")]
        PatrolInsp = 21,

        /// <summary>
        /// 抽检
        /// </summary>
        [Category("PQC")]
        [Label("抽检")]
        SamplingInsp = 22,

        /// <summary>
        /// 联机检验
        /// </summary>
        [Category("MES")]
        [Label("联机检验")]
        OnlineInsp = 23,
        #endregion

        #region Recheck
        /// <summary>
        /// 超期复检
        /// </summary>
        [Category("Recheck")]
        [Label("超期复检")]
        RecheckInsp = 40,
        #endregion

        #region PCB
        /// <summary>
        /// 通用
        /// </summary>
        [Category("PCB")]
        [Label("通用")]
        PcbCommonInsp = 100,
        /// <summary>
        /// 首检
        /// </summary>
        [Category("PCB_PQC")]
        [Label("首检")]
        PcbFirstInsp = 101,

        /// <summary>
        /// 工序抽检
        /// </summary>
        [Category("PCB_PQC")]
        [Label("工序抽检")]
        PcbSampleInsp = 102,

        /// <summary>
        /// 成品抽检
        /// </summary>
        [Category("PCB_PQC")]
        [Label("FQA")]
        PcbFqa = 108,

        /// <summary>
        /// 物理实验室
        /// </summary>
        [Category("PCB")]
        [Label("物理实验室")]
        PcbPhysicalLabInsp = 103,

        /// <summary>
        /// 化学实验室
        /// </summary>
        [Category("PCB")]
        [Label("化学实验室")]
        PcbChemicalLabInsp = 104,

        /// <summary>
        /// FQC
        /// </summary>
        [Category("PCB_PQC")]
        [Label("FQC")]
        PcbFqc = 105,

        /// <summary>
        /// AOI
        /// </summary>
        [Category("PCB")]
        [Label("AOI")]
        PcbAoi = 106,

        /// <summary>
        /// ET
        /// </summary>
        [Category("PCB")]
        [Label("ET")]
        PcbEt = 107,

        /// <summary>
        /// Qtime
        /// </summary>
        [Category("PCB")]
        [Label("QTIME超时")]
        Qtime = 109,
        #endregion
    }
}
