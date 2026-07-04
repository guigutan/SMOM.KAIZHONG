using Newtonsoft.Json;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Datas;
using SIE.Rbac.InvOrgs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Enums;

namespace SIE.ERPInterface.Ebs.Connection
{
    /// <summary>
    /// Ebs帮助类，调用最新使用的rest服务，
    /// </summary>
    public static class EbsHelper
    {
        #region 下载
        /// <summary>
        /// 获取EBS配置
        /// </summary>
        /// <param name="isOrg">区分库存组织</param>
        /// <returns></returns>
        public static EbsParameter GetEbsParameter(bool isOrg = true)
        {
            var ebsPara = new EbsParameter();
            ebsPara.DownloadUri = RT.Config.Get<string>("ERP.Download.Uri");
            ebsPara.UploadUri = RT.Config.Get<string>("ERP.Upload.Uri");
            ebsPara.Authorization = RT.Config.Get<string>("ERP.Authorization");
            ebsPara.Language = RT.Config.Get<string>("ERP.Language");
            if (ebsPara.DownloadUri.IsNullOrEmpty())
                throw new ValidationException("ERP.Download.Uri没有配置值".L10N());
            if (RT.InvOrg == null)
                throw new ValidationException("RT.InvOrg没有值".L10N());
            if (isOrg)
            {
                var invOrg = DB.Query<InvOrg>().Where(a => a.Code == RT.InvOrg).FirstOrDefault();
                ebsPara.InvOrgId = invOrg.ExternalId;
            }

            return ebsPara;
        }

        /// <summary>
        /// POST ERP接口
        /// </summary>
        /// <param name="para"></param>      
        /// <returns></returns>
        public static EbsResult<T> ExecuteEbsBase<T>(EbsParameter para)
        {
            //PARA_DATA_TYPE可填的值如下：
            //"PARA_DATA_TYPE": "N"--数字
            //"PARA_DATA_TYPE": "C"--字符串
            //"PARA_DATA_TYPE": "D"--日期（不带时分秒）
            //"PARA_DATA_TYPE": "DT"--日期（带时分秒）

            var result = new EbsResult<T>();

            if (para.DownParameter.ParaStr.IsNullOrEmpty())
            {
                para.DownParameter.ParaStr = @"{
                                        ""PARA_DATA_TYPE"": ""DT"",
                                        ""PARA_NAME"": ""Last_Update_Date"",
                                        ""PARA_OPERATOR"": "">="",
                                        ""PARA_VALUE"": ""{0}""
                                    }".FormatArgs(para.DownParameter.LastUpdateDate.ToString("yyyy-MM-dd HH:mm:ss"));

                if (para.InvOrgId.IsNotEmpty())
                {

                    para.DownParameter.ParaStr += @", {
                                        ""PARA_DATA_TYPE"": ""N"",
                                        ""PARA_NAME"": ""Organization_Id"",
                                        ""PARA_OPERATOR"": ""="",
                                        ""PARA_VALUE"": ""{0}""
                                    }".FormatArgs(para.InvOrgId);
                }
            }

            string requestBody = @"{
                ""Main_Input"": {
                    ""InputParameters"": {
                        ""PN_PAGE_SIZE"": {0},
                        ""PN_PAGE"": {1},
                        ""PN_GROUP_ID"": ""{4}"",
                        ""PV_BUSINESS_TYPE_CODE"": ""{2}"",
                        ""PV_PARAMETER"": {
                            ""PARA"": {
                                ""PARAD"": [
                                     {3}
                                ]
                            }
                        }
                    },
                    ""RESTHeader"": {
                        ""Org_Id"": ""81""
                    }
                }
            }".FormatArgs(para.DownParameter.PageSize, para.DownParameter.PageNum, para.InterfaceCode, para.DownParameter.ParaStr,
             para.DownParameter.PN_GROUP_ID);

            byte[] requestBodyBytes = Encoding.UTF8.GetBytes(requestBody);
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(para.DownloadUri);
                request.Method = "POST";
                request.ContentType = "application/json;charset=utf-8";
                request.Timeout = 1200000;//设置超时时间(批量处理，数据量大时，时间可能会很长)
                request.Headers.Add("Authorization", para.Authorization);

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(requestBodyBytes, 0, requestBodyBytes.Length);
                }
                result.RequestDate = DateTime.Now;  //记录请求时间
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                        result.ResponseDate = DateTime.Now; //记录接收时间
                        string responseString = reader.ReadToEnd();
                        result.ResponseStr = responseString;
                        result.RequestStr = requestBody;
                        reader.Close();
                        response.Close();
                        // 处理响应数据
                        EbsResponseCheck ebsResponsecheck = JsonConvert.DeserializeObject<EbsResponseCheck>(responseString);
                        if (ebsResponsecheck.OutputParameters.XV_RET_STATUS == "S")
                        {
                            EbsResponse ebsResponse = JsonConvert.DeserializeObject<EbsResponse>(responseString);
                            result.XV_RECORD_TOTAL = ebsResponse.OutputParameters.XV_RECORD_TOTAL;
                            result.XV_GROUP_ID = ebsResponse.OutputParameters.XV_GROUP_ID;
                            result.XV_RET_STATUS = ebsResponse.OutputParameters.XV_RET_STATUS;
                            result.XV_RET_MESSAGE = "";
                            result.XV_RESULT = JsonConvert.DeserializeObject<List<T>>(ebsResponse.OutputParameters.XV_RESULT);
                        }
                        else
                        {
                            result.XV_RET_STATUS = ebsResponsecheck.OutputParameters.XV_RET_STATUS;
                            result.XV_RET_MESSAGE = ebsResponsecheck.OutputParameters.XV_RET_MESSAGE.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ValidationException("ERP接口程序错误：{0}".L10nFormat(ex.GetBaseException().Message));
            }

            return result;
        }

        /// <summary>
        /// 获取ERP数据
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="ebsPara">Ebs报文参数</param>
        /// <returns>Ebs返回报文</returns>
        public static EbsResult<T> ExecuteEbs<T>(EbsParameter ebsPara)
        {
            var soapResult = ExecuteEbsBase<T>(ebsPara);
            //当数据总量超过分页大小，需要按一页一页取数
            if (soapResult.XV_RECORD_TOTAL > ebsPara.DownParameter.PageSize)
            {
                var tail = soapResult.XV_RECORD_TOTAL % ebsPara.DownParameter.PageSize > 0 ? 1 : 0;
                var pageCount = soapResult.XV_RECORD_TOTAL / ebsPara.DownParameter.PageSize + tail;
                for (int i = 2; i <= pageCount; i++)//第一页已经下载过，从第二页开始
                {
                    ebsPara.DownParameter.PageNum = i;
                    var datas = ExecuteEbsBase<T>(ebsPara);
                    soapResult.XV_RESULT.AddRange(datas.XV_RESULT);
                }
            }
            return soapResult;
        }
        #endregion

        #region 上传          
        /// <summary>
        /// POST ERP接口
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public static Tuple<ProcessResult, List<ErpUploadLog>> UploadExecuteEbsBase(EbsParameter para)
        {
            ProcessResult result = new ProcessResult();
            List<ErpUploadLog> ebsUploadLogs = new List<ErpUploadLog>();
            if (para.UploadStr.IsNullOrEmpty())
                return new Tuple<ProcessResult, List<ErpUploadLog>>(result, ebsUploadLogs);

            //上传报文构建
            string requestBody = @"{
  ""Main_Input"": {
    ""RESTHeader"": {
                ""Org_Id"": ""141"",
                ""Responsibility"": """",
                ""RespApplication"": """",
                ""SecurityGroup"": """",
                ""NLSLanguage"": ""{2}""
    },
    ""InputParameters"": {
                ""P_TRANSFER_DATA"": {
                    ""ROOT"": {
                        ""DATAROW"": [{1}]
        }
                },
      ""P_SYSTEM_CODE"": ""WMS"",
      ""P_INTERFACE_CODE"": ""{0}""
    }
        }
    }".FormatArgs(para.InterfaceCode, para.UploadStr, para.Language);
            //记录到任务时间戳明细
            var jobTimeDetail = new DownloadJobTimeDetail();
            jobTimeDetail.GenerateId();
            jobTimeDetail.RequestStr = requestBody;

            byte[] requestBodyBytes = Encoding.UTF8.GetBytes(requestBody);
            try
            {
                DateTime beginTime = DateTime.Now;
                jobTimeDetail.RequestDate = beginTime;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(para.UploadUri);
                request.Method = "POST";
                request.ContentType = "application/json;charset=utf-8";
                request.Timeout = 1200000;//设置超时时间(批量处理，数据量大时，时间可能会很长)
                request.Headers.Add("Authorization", para.Authorization);

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(requestBodyBytes, 0, requestBodyBytes.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);

                        string responseString = reader.ReadToEnd();
                        reader.Close();
                        response.Close();
                        // 处理响应数据
                        //记录报文到任务时间戳明细
                        DateTime responseDate = DateTime.Now;
                        jobTimeDetail.ResponseStr = responseString;
                        jobTimeDetail.ResponseDate = responseDate;
                        //反序列化响应数据
                        EbsUploadResponse ebsResponse = JsonConvert.DeserializeObject<EbsUploadResponse>(responseString);
                        if (ebsResponse.OutputParameters.X_RETURN_STATUS == "S")
                        {
                            jobTimeDetail.State = RequestState.Success;
                            var returnDatas = JsonConvert.DeserializeObject<List<EbsReturnData>>(ebsResponse.OutputParameters.RETURN_DATA);
                            returnDatas.ForEach(p =>
                            {
                                ebsUploadLogs.Add(SetEbsUploadLogData(p, result));
                            });
                        }
                        else
                        {
                            result.HeadMsg = "调用接口成功，ERP返回状态[{0}失败]".FormatArgs(ebsResponse.OutputParameters.X_RETURN_STATUS);
                            result.FailMsg.Add("调用接口成功，ERP返回状态[{0}失败]".FormatArgs(ebsResponse.OutputParameters.X_RETURN_STATUS));
                            jobTimeDetail.State = RequestState.Failed;
                            jobTimeDetail.ResponseMessage = result.HeadMsg;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                jobTimeDetail.State = RequestState.Failed;
                jobTimeDetail.ResponseMessage = "ERP接口程序错误：{0}".FormatArgs(ex.GetBaseException().Message);
            }
            SaveJobTime(para, jobTimeDetail);
            return new Tuple<ProcessResult, List<ErpUploadLog>>(result, ebsUploadLogs);
        }

        /// <summary>
        /// 设置上传记录的数据
        /// </summary>
        /// <param name="returnData">返回的数据</param>      
        /// <param name="result">处理结果</param>
        private static ErpUploadLog SetEbsUploadLogData(EbsReturnData returnData, ProcessResult result)
        {
            ErpUploadLog ebsUploadLog = new ErpUploadLog()
            {
                LineNo = returnData.SCUX_SOURCE_LINE_NUM,
                OrderNo = returnData.SCUX_SOURCE_NUM,
            };
            if (double.TryParse(returnData.SCUX_SOURCE_LOT_NUM, out double tranId))
                ebsUploadLog.TransactionId = tranId;//ERP返回正确的事务交易Id，来源是我们传过去
            string processStatus = returnData.PROCESS_STATUS.ToUpper();           
            const string completedStatus = "COMPLETED"; //成功
            const string pendingStatus = "PENDING"; //处理中

            switch (processStatus)
            {
                case completedStatus://成功
                    ebsUploadLog.IsSuccess = true;
                    ebsUploadLog.State = ProcessState.Processed;
                    string responseMessage = ebsUploadLog.TransactionId > 0
                        ? string.Empty
                        : $"ERP处理成功，但是返回的事务ID转换成数字失败，返回的值{returnData.SCUX_SOURCE_LOT_NUM}";
                    ebsUploadLog.ResponseMessage = responseMessage;
                    result.SuccessMsg.Add("Success");
                    break;
                case pendingStatus://处理中
                    ebsUploadLog.State = ProcessState.Processing;
                    ebsUploadLog.ResponseMessage = "ERP返回PENDING(正在处理)，需要重传";
                    result.FailMsg.Add("ERP返回处理失败单号{0}行号{1}交易Id{2}".FormatArgs(returnData.SCUX_SOURCE_NUM, returnData.SCUX_SOURCE_LINE_NUM, returnData.SCUX_SOURCE_LOT_NUM));
                    break;
                default://失败
                    ebsUploadLog.State = ProcessState.Retry;
                    string responseMessage2 = $"{returnData.PROCESS_MESSAGE}，返回的事务ID：{returnData.SCUX_SOURCE_LOT_NUM}";
                    ebsUploadLog.ResponseMessage = responseMessage2;
                    result.FailMsg.Add("ERP返回处理失败单号{0}行号{1}交易Id{2}".FormatArgs(returnData.SCUX_SOURCE_NUM, returnData.SCUX_SOURCE_LINE_NUM, returnData.SCUX_SOURCE_LOT_NUM));
                    break;
            }

            return ebsUploadLog;
        }

        /// <summary>
        /// 保存接口任务时间戳
        /// </summary>
        /// <param name="para">EBS报文参数</param>
        /// <param name="jobTimeDetail">接口任务时间戳明细</param>
        private static void SaveJobTime(EbsParameter para, DownloadJobTimeDetail jobTimeDetail)
        {
            var jobTime = RT.Service.Resolve<DownloadInfBaseController>().GetDownloadJobTime(para.UploadJobType);

            if (jobTime == null) jobTime = new DownloadJobTime() { JobType = para.UploadJobType, JobCate = JobCate.Upload };
            jobTime.LastDownloadDate = DateTime.Now;
            jobTimeDetail.ErpBatchId = jobTimeDetail.Id;
            jobTimeDetail.IsManual = false;
            jobTime.DetailList.Add(jobTimeDetail);
            RF.Save(jobTime);
        }

        #endregion


    }
}
