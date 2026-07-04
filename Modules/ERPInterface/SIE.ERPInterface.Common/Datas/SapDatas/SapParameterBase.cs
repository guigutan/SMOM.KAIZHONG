using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.ERPInterface.Common.Datas
{
    #region SAP输入参数

    [Serializable]
    public class SapResponseParam<T>
    {
        public List<T> Return { get; set; } = new List<T>();
    }

    /// <summary>
    /// 头部参数
    /// </summary>
    [Serializable]
    public class SapUploadParam<T>
    {
        ///// <summary>
        ///// 数据请求Key
        ///// </summary>
        //public string DATAKEY { get; set; }

        /// <summary>
        /// 明细数据
        /// </summary>
        public List<T> ITEMS { get; set; } = new List<T>();
    }

    /// <summary>
    /// 单据参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class SapOrderParamBase<T>
    {
        /// <summary>
        /// 凭证日期
        /// </summary>
        public int BLDAT { get; set; }

        /// <summary>
        /// 过账日期
        /// </summary>
        public int BUDAT { get; set; }

        /// <summary>
        /// 单据Key
        /// </summary>
        public string EXTDOCNO { get; set; }

        /// <summary>
        /// 明细列表
        /// </summary>

        public List<T> ITEM { get; set; } = new List<T>();
    }

    /// <summary>
    /// 明细
    /// </summary>
    [Serializable]
    public class SapItemParamBase
    {
        /// <summary>
        /// 明细KEY
        /// </summary>
        public string EXTDOC_ITEMNO { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MATNR { get; set; }
    }

    #endregion
}
