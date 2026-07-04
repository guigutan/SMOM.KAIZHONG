using SIE.CrossPlatform.Collect.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.CrossPlatform.Collect.Models.Enums
{
    public enum ProcessType
    {
        /// <summary>
        /// 检验
        /// </summary>
        [Label("检验")]
        Pqc = 0,

        ///// <summary>
        ///// 终检
        ///// </summary>
        //[Label("终检")]
        //Fqc = 5,

        /// <summary>
        /// 维修
        /// </summary>
        [Label("维修")]
        Fix = 10,

        /// <summary>
        /// 返工
        /// </summary>
        [Label("返工")]
        Rework = 13,

        /// <summary>
        /// 装配
        /// </summary>
        [Label("装配")]
        Assembly = 15,

        /// <summary>
        /// 包装
        /// </summary>
        [Label("包装")]
        Packing = 20,

        /// <summary>
        /// 批次装配
        /// </summary>
        [Label("批次装配")]
        BatchAssembly = 25,

        /// <summary>
        /// 批次检验
        /// </summary>
        [Label("批次检验")]
        BatchPqc = 30,

        /// <summary>
        /// 批次维修
        /// </summary>
        [Label("批次维修")]
        BatchFix = 35,

        /// <summary>
        /// 批次包装
        /// </summary>
        [Label("批次包装")]
        BatchPacking = 40,

        /// <summary>
        /// 老化
        /// </summary>
        [Label("老化")]
        Ageing = 22



    }
}
