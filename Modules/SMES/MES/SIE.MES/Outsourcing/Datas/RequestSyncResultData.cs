using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Outsourcing.Datas
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class RequestSyncResultData
    {
        public string errMsg { get; set; }
        /// <summary>
        /// 成功Id
        /// </summary>
        //public List<RequestSyncSuccessResultData> sucessData { get; set; } = new List<RequestSyncSuccessResultData>();

        // <summary>
        // 失败数据
        // </summary>
        //public List<RequestSyncFailResultData> failData { get; set; } = new List<RequestSyncFailResultData>();
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class RequestSyncSuccessResultData
    { 
        /// <summary>
        /// 
        /// </summary>
        public double Id { get; set; }


        /// <summary>
        /// 标识
        /// </summary>
        public string Sign { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class RequestSyncFailResultData
    { 
        /// <summary>
        /// 
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ErrMsg { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        public string Sign { get; set; }
    }
}
