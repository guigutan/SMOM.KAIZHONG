using Newtonsoft.Json;
using SIE.Domain;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Logs;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.ERPInterface.Logs.DataQueryer
{
    /// <summary>
    /// 接口下载异常查询器
    /// </summary>
    public partial class DownloadExcDataQueryer : Data.DataQueryer
    {
        /// <summary>
        /// 获取接口下载异常记录
        /// </summary>
        /// <param name="criteria">接口异常下载 查询实体</param>
        /// <returns>接口下载异常记录</returns>
        public List<DownloadExcViewModel> GetDownloadExcs(DownloadExcViewModelCriteria criteria)
        {
            var model = RT.Service.Resolve<DownloadExcController>().GetDownloadExcs(criteria);
            var results = model.DownloadExcList.ToList<DownloadExcViewModel>();

            return results;
        }

        /// <summary>
        /// 更新事务上传记录
        /// </summary>
        /// <param name="ebsUploadLogId">事务ID</param>
        /// <param name="requestStr">修改的报文</param>
        /// <returns></returns>
        public object UpdateEbsUploadLog(double ebsUploadLogId, string requestStr)
        {
            return RT.Service.Resolve<ErpUploadLogController>().UpdateEbsUploadLog(ebsUploadLogId, requestStr);
        }

        /// <summary>
        /// 事务上传记录报文转换
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public object ChangeResponseData(double id)
        {
            var dtl = RF.GetById<DownloadJobTimeDetail>(id);
            try
            {
                var rconfig = SIE.Common.Configs.ConfigService.GetConfig(new SIE.Core.Configs.InterfaceSourceConfig());
                if (rconfig.InterfaceSourceType == SIE.Core.Configs.InterfaceSourceType.EBS)
                {
                    EbsUploadResponse ebsResponse = JsonConvert.DeserializeObject<EbsUploadResponse>(dtl.ResponseStr);
                    if (ebsResponse.OutputParameters.X_RETURN_STATUS == "S")
                    {
                        var returnDatas = JsonConvert.DeserializeObject<List<EbsReturnData>>(ebsResponse.OutputParameters.RETURN_DATA);
                        if (returnDatas != null && returnDatas.Count > 0)
                        {
                            return returnDatas;
                        }
                    }
                }
            }
            catch
            {
                ////不需要报错
            }
            return new List<EbsReturnData>();
        }

        /// <summary>
        /// 获取接口任务时间戳明细
        /// </summary>
        /// <param name="id">接口任务时间戳ID</param>
        /// <param name="requestStr">接口任务时间戳明细请求报文</param>
        /// <returns></returns>
        public EntityList<DownloadJobTimeDetail> GetRequestStr(double id, string requestStr)
        {
            return RT.Service.Resolve<DownloadExcController>().GetDownloadJobTimeDetails(id, requestStr);
        }

        /// <summary>
        /// 获取请求报文
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetUploadRequestStr(double id)
        {
            return RT.Service.Resolve<ErpUploadLogController>().GetRequestStr(id);
        }
    }
}
