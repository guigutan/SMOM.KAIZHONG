using System;

namespace SIE.ERPInterface.Sap.Datas.ErpInfoDatas.Deduction
{
    /// <summary>
    /// 蓝标上传SAP接口返回
    /// </summary>
    [Serializable]
    public class KzBlueLabelResultData
    {
        /// <summary>
        /// 
        /// </summary>
        public KzBlueLabelResult Return { get; set; }

    }
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class KzBlueLabelResult
    {

        /// <summary>
        /// 返回标识
        /// </summary>
        public string ZFKBS { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string ZFKXX { get; set; }

    }


    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class KzBlueLabelRequestData
    {

        /// <summary>
        /// 蓝标
        /// </summary>
        public string EXIDV { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public string WERKS { get; set; }
        /// <summary>
        /// 标识 Y/N
        /// </summary>
        public string ZPACK { get; set; }

    }
}
