using SapNwRfc;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.ERPInterface.Sap.Datas
{
    /// <summary>
    /// 参数
    /// </summary>
    [Serializable]
    public class SapParameterBase<T> where T : class
    {
        /// <summary>
        /// IT_DATA
        /// </summary>
        [SapName("IT_DATA")]
        public T[] IT_DATA { get; set; }
    }
}
