using SIE.Domain;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Logs;
using System.Collections.Generic;

namespace SIE.ERPInterface.Sap
{
    /// <summary>
    /// 设置上传数据的接口
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    public interface IUploadDataHandler<T>
    {
        /// <summary>
        /// 设置业务上传的数据
        /// </summary>
        /// <param name="uploadTransactions"></param>      
        /// <returns></returns>
        SapUploadParam<T> SetUploadData(List<UploadTransaction> uploadTransactions);

        /// <summary>
        /// 上传后事件
        /// </summary>
        /// <param name="response"></param>
        ProcessResult Uploaded(SapResult sapResult, EntityList<UploadTransaction> uploadTransactions, string str);

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <returns></returns>
        string SetParam(EntityList<UploadTransaction> uploadTransactions);

        /// <summary>
        /// 分组
        /// </summary>
        /// <param name="uploadTransactions"></param>
        Dictionary<string, List<UploadTransaction>> Grouped(EntityList<UploadTransaction> uploadTransactions);
    }
}
