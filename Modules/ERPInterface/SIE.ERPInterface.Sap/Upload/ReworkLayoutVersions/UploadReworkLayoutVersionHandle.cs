using DotLiquid.Util;
using Newtonsoft.Json;
using SIE.Domain;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Sap.Datas.ErpInfoDatas.Deduction;
using SIE.ERPInterface.Sap.Datas.ErpInfoDatas.ReworkLayoutVersions;
using SIE.MES.ReworkLayoutVersions;
using SIE.MES.TaskManagement.FeedingRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Sap.Upload.ReworkLayoutVersions
{
    public class UploadReworkLayoutVersionHandle : IUploadDataHandler<KzReworkLayoutVersionUploadData>
    {
        public Dictionary<string, List<UploadTransaction>> Grouped(EntityList<UploadTransaction> uploadTransactions)
        {
            var dic = new Dictionary<string, List<UploadTransaction>>();
            //一条数据一条数据上传(应讨论结果)
            dic = uploadTransactions.GroupBy(p => p.Id).ToDictionary(p => p.Key.ToString(), p => p.ToList());
            return dic;
        }

        public string SetParam(EntityList<UploadTransaction> uploadTransactions)
        {
            var cur = DateTime.Now;
            var rst = new SapUploadParam<KzReworkLayoutVersionUploadData>();

            var recordIds = uploadTransactions.Select(p => p.BillId.Value).Distinct().ToList();
            var records = RT.Service.Resolve<ReworkLayoutVersionController>().GetReworkInfoRecordsByIds(recordIds);

            uploadTransactions
                //.GroupBy(f => new { f.WoNo, f.ProcessCode })   
                .ForEach(p =>
                {
                    var record = records.FirstOrDefault(f => f.Id == p.BillId.Value);
                    KzReworkLayoutVersionUploadData sapOrderParam = new KzReworkLayoutVersionUploadData();
                    sapOrderParam.ZJSBS = ((int)record.State).ToString();
                    sapOrderParam.WERKS = p.WERKS;
                    sapOrderParam.MATNR = p.ItemCode;
                    sapOrderParam.GAMNG = p.Quantity;
                    sapOrderParam.DAUAT = "KZ05";
                    sapOrderParam.GSTRP = DateTime.Now.ToString("yyyyMMdd");//record.BeginDateTime?.ToString("yyyyMMdd");
                    sapOrderParam.GLTRP = DateTime.Now.ToString("yyyyMMdd");//record.EndDateTime?.ToString("yyyyMMdd");
                    sapOrderParam.VERID = p.Version;
                    sapOrderParam.ABLAD = p.Zuid;
                    sapOrderParam.WEMPF = p.Department;
                    rst.ITEMS.Add(sapOrderParam);
                });
            return JsonConvert.SerializeObject(rst);
        }

        public SapUploadParam<KzReworkLayoutVersionUploadData> SetUploadData(List<UploadTransaction> uploadTransactions)
        {
            throw new NotImplementedException();
        }

        public ProcessResult Uploaded(SapResult sapResult, EntityList<UploadTransaction> uploadTransactions, string str)
        {
            var param = JsonConvert.DeserializeObject<SapResponseParam<KzReworkLayoutVersionResultData>>(sapResult.ResponseStr);
            ProcessResult processResult = new ProcessResult();

            foreach (var result in param.Return)
            {
                var ids = uploadTransactions.Select(p => p.Id).Distinct().ToList();
                if (result != null && result.ZFKBS == "S")
                {
                    DB.Update<UploadTransaction>()
                        .Set(p => p.State, Common.Enums.ProcessState.Processed)
                        .Set(p => p.ValidateMessage, result.ZFKBS)
                        .Set(p => p.ProcessMessage, result.ZFKXX)
                        .Where(p => ids.Contains(p.Id))
                        .Execute();

                    var recordIds = uploadTransactions.Select(p => p.BillId.Value).Distinct().ToList();
                    DB.Update<ReworkInfoRecord>()
                        .Set(p => p.ProductOrder, result.AUFNR)
                        .Set(p => p.Identification, result.ZFKBS)
                        .Set(p => p.Msg, result.ZFKXX)
                        .Where(p => p.UniqueCode == result.ABLAD).Execute();
                    processResult.AddSuccessMsg("{0}上传成功".L10nFormat(result.ABLAD));
                }
                else
                {
                    //先更新失败的，再更新重试的
                    DB.Update<UploadTransaction>()
                        .Set(p => p.State, Common.Enums.ProcessState.Failed)
                        .Set(p => p.ValidateMessage, result.ZFKBS)
                        .Set(p => p.ProcessMessage, result.ZFKXX)
                        .Set(p => p.UploadCount, p => p.UploadCount + 1)
                        .Where(p => ids.Contains(p.Id) && p.UploadCount >= 5)
                        .Execute();
                    DB.Update<UploadTransaction>()
                        .Set(p => p.State, Common.Enums.ProcessState.Retry)
                        .Set(p => p.ValidateMessage, result.ZFKBS)
                        .Set(p => p.ProcessMessage, result.ZFKXX)
                        .Set(p => p.UploadCount, p => p.UploadCount + 1)
                        .Where(p => ids.Contains(p.Id) && p.UploadCount < 5)
                        .Execute();

                    processResult.AddSuccessMsg("上传失败".L10N());
                    sapResult.IsSuccess = false;
                }
            }
            return processResult;
        }
    }
}
