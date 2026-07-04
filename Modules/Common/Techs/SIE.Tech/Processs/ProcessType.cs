using SIE.ObjectModel;
using System.ComponentModel;

namespace SIE.Tech.Processs
{
    /// <summary>
    /// 工序类型
    /// </summary>
    public enum ProcessType
    {
        /// <summary>
        /// 检验
        /// </summary>
        [Label("检验")]
        [Category("Single")]
        Pqc = 0,

        ///// <summary>
        ///// 终检
        ///// </summary>
        //[Label("终检")]
        //Fqc = 5,

        /// <summary>
        /// 维修
        /// </summary>
        [Category("Single")]
        [Label("维修")]
        Fix = 10,

        /// <summary>
        /// 返工
        /// </summary>
        [Category("Single")]
        [Label("返工")]
        Rework = 13,

        /// <summary>
        /// 装配
        /// </summary>
        [Category("Single")]
        [Label("装配")]
        Assembly = 15,

        /// <summary>
        /// 包装
        /// </summary>
        [Category("Single")]
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
        [Category("Single")]
        [Label("老化")]
        Ageing = 22
       


    }

    /// <summary>
    /// 工序交接类型
    /// </summary>
    public enum TransferType
    {

        /// <summary>
        /// 转入转出
        /// </summary>\
        [Label("转入转出")]
        TransferInOut = 0,
        /// <summary>
        /// 转入
        /// </summary>
        [Label("转入")] 
        TransferIn =1,
        /// <summary>
        /// 转出
        /// </summary>
        [Label("转出")] 
        TransferOut = 2,

    }
}