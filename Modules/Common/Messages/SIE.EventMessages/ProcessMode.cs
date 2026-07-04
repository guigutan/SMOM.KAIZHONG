using SIE.ObjectModel;
using System;
using System.ComponentModel;

namespace SIE.EventMessages
{
    /// <summary>
    /// 处理方式
    /// </summary>
    [Serializable]
    public enum ProcessMode
    {
        #region IQC处理方式
        /// <summary>
        /// 特采
        /// </summary>
        [Category("IQC")]
        [Label("特采")]
        Concession,

        /// <summary>
        /// 挑选
        /// </summary>
        [Category("IQC")]
        [Label("挑选")]
        Pick,

        /// <summary>
        /// 报废
        /// </summary>
        [Category("IQC")]
        [Label("报废")]
        Scrap,

        /// <summary>
        /// 退货
        /// </summary>
        [Category("IQC")]
        [Label("退货")]
        ReturnToSupplier,
        #endregion

        #region PQC处理方式
        /// <summary>
        /// 停产整改
        /// </summary>
        [Category("PQC")]
        [Label("停产整改")]
        StopToCorrect,

        /// <summary>
        /// 挑选
        /// </summary>
        [Category("PQC")]
        [Label("边整改边生产")]
        CorrectAndProduct,

        ///// <summary>
        ///// 复检
        ///// </summary>
        //[Category("PQC")]
        //[Label("复检")]
        //ReCheck,

        ///// <summary>
        ///// 加抽
        ///// </summary>
        //[Category("PQC")]
        //[Label("加抽")]
        //AddCheck,

        ///// <summary>
        ///// 继续生产
        ///// </summary>
        //[Category("PQC")]
        //[Label("继续生产")]
        //AlleyOop,

        ///// <summary>
        ///// 调机
        ///// </summary>
        //[Category("PQC")]
        //[Label("调机")]
        //AdjustMachine,
        #endregion

        //#region Pcb化学实验室处理方式
        ///// <summary>
        ///// 按调整方案执行
        ///// </summary>
        //[Category("PCB")]
        //[Label("按调整方案执行")]
        //FixedExecution,

        //#endregion

        #region OQC处理方式
        /// <summary>
        /// 返工
        /// </summary>
        [Category("OQC")]
        [Label("返工")]
        Repair,

        /// <summary>
        /// 特采
        /// </summary>
        [Category("OQC")]
        [Label("特采")]
        OQCConcession,

        ///// <summary>
        ///// 挑选
        ///// </summary>
        //[Category("OQC")]
        //[Label("挑选")]
        //OQCPick,

        /// <summary>
        /// 报废
        /// </summary>
        [Category("OQC")]
        [Label("报废")]
        OQCScrap,
        #endregion


    }
}