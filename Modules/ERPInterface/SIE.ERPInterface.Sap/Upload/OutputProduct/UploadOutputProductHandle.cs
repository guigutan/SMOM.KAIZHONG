using Newtonsoft.Json;
using SIE.Domain;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Sap.Datas.ErpInfoDatas.Deduction;
using SIE.MES.TaskManagement.FeedingRecords;
using SIE.ProductIntfc.OutputProducts;
using SIE.Rbac.InvOrgs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Sap.Upload.OutputProduct
{
    public class UploadOutputProductHandle : IUploadDataHandler<KzDeductionUploadData>
    {

        /// <summary>
        /// 分组
        /// </summary>
        /// <param name="uploadTransactions"></param>
        public Dictionary<string,List<UploadTransaction>> Grouped(EntityList<UploadTransaction> uploadTransactions)
        {
            var dic = new Dictionary<string, List<UploadTransaction>>();
            dic.Add("", uploadTransactions.ToList());
            return dic;
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string SetParam(EntityList<UploadTransaction> uploadTransactions)
        {
            var cur = DateTime.Now;
            var rst = new SapUploadParam<KzDeductionUploadData>();
            var invOrg = DB.Query<InvOrg>().Where(a => a.Code == RT.InvOrg).FirstOrDefault();
            uploadTransactions
                //.GroupBy(f => new { f.WoNo, f.ProcessCode })   
                .ForEach(p =>
                {
                    KzDeductionUploadData sapOrderParam = new KzDeductionUploadData();
                    sapOrderParam.AUFNR = p.WoNo;
                    sapOrderParam.PLANT = p.WERKS;
                    sapOrderParam.BUDAT = cur.ToString("yyyyMMdd");
                    sapOrderParam.MATNR = p.ItemCode;
                    sapOrderParam.MEINS = p.UnitName;
                    sapOrderParam.MENGE = p.Quantity.ToString();
                    sapOrderParam.CHARG = p.LotCode;
                    sapOrderParam.LGORT = p.ToLocationCode;
                    sapOrderParam.BWART = "531";
                    sapOrderParam.GRUND = "";
                    rst.ITEMS.Add(sapOrderParam);
                });
            return JsonConvert.SerializeObject(rst);
        }

        public ProcessResult Uploaded(SapResult sapResult, EntityList<UploadTransaction> uploadTransactions, string key)
        {
            var param = JsonConvert.DeserializeObject<SapResponseParam<KzDeductionResultData>>(sapResult.ResponseStr);
            ProcessResult processResult = new ProcessResult();
            foreach (var r in param.Return)
            {
                if (r.ZFKBS == "S")
                {
                    var uts = uploadTransactions.Where(p => p.WoNo == r.AUFNR).ToList();
                    var ids = uts.Select(p => p.Id).Distinct().ToList();
                    DB.Update<UploadTransaction>()
                        .Set(p => p.State, Common.Enums.ProcessState.Processed)
                        .Set(p => p.ValidateMessage, r.ZFKXX)
                        .Set(p => p.ProcessMessage, r.ZFKXX)
                        .Set(p => p.Mblnr, r.MBLNR)
                        .Set(p => p.Mjahr, r.MJAHR)
                        .Where(p => ids.Contains(p.Id))
                        .Execute();
                    foreach (var ut in uts)
                    {
                        DB.Update<OutputProductRecord>().Set(p => p.UploadResult, r.ZFKXX).Set(p => p.Mblnr, r.MBLNR).Set(p => p.Mjahr, r.MJAHR).Where(p => p.Id == ut.BillId).Execute();
                    }
                    processResult.AddSuccessMsg("工单{0}上传成功".L10nFormat(r.AUFNR));
                }
                else
                {
                    var uts = uploadTransactions.Where(p => p.WoNo == r.AUFNR).ToList();
                    var ids = uts.Select(p => p.Id).Distinct().ToList();

                    sapResult.IsSuccess = false;
                    //先更新失败的，再更新重试的
                    DB.Update<UploadTransaction>()
                        .Set(p => p.State, Common.Enums.ProcessState.Failed)
                        .Set(p => p.ValidateMessage, r.ZFKXX)
                        .Set(p => p.ProcessMessage, r.ZFKXX)
                        .Set(p => p.Mblnr, r.MBLNR)
                        .Set(p => p.Mjahr, r.MJAHR)
                        .Set(p => p.UploadCount, p => p.UploadCount + 1)
                        .Where(p => ids.Contains(p.Id) && p.UploadCount >= 5)
                        .Execute();
                    DB.Update<UploadTransaction>()
                        .Set(p => p.State, Common.Enums.ProcessState.Retry)
                        .Set(p => p.ValidateMessage, r.ZFKXX)
                        .Set(p => p.ProcessMessage, r.ZFKXX)
                        .Set(p => p.Mblnr, r.MBLNR)
                        .Set(p => p.Mjahr, r.MJAHR)
                        .Set(p => p.UploadCount, p => p.UploadCount + 1)
                        .Where(p => ids.Contains(p.Id) && p.UploadCount < 5)
                        .Execute();

                    foreach (var ut in uts)
                    {
                        DB.Update<OutputProductRecord>().Set(p => p.UploadResult, r.ZFKXX).Set(p => p.Mblnr, r.MBLNR).Set(p => p.Mjahr, r.MJAHR).Where(p => p.Id == ut.BillId).Execute();
                    }
                    processResult.AddFailMsg("工单{0}上传失败:{1};".L10nFormat(r.AUFNR, r.ZFKXX));
                }

            }
            return processResult;
        }

        public SapUploadParam<KzDeductionUploadData> SetUploadData(List<UploadTransaction> uploadTransactions)
        {
            var cur = DateTime.Now;
            var rst = new SapUploadParam<KzDeductionUploadData>();
            //var invOrg = DB.Query<InvOrg>().Where(a => a.Code == RT.InvOrg).FirstOrDefault();
            uploadTransactions
                //.GroupBy(f => new { f.WoNo, f.ProcessCode })    //按工单工序合并上传
                .ForEach(p =>
                {
                    KzDeductionUploadData sapOrderParam = new KzDeductionUploadData();
                    sapOrderParam.AUFNR = p.WoNo;
                    sapOrderParam.PLANT = p.WERKS;
                    sapOrderParam.BUDAT = cur.ToString("yyyyMMdd");
                    sapOrderParam.MATNR = p.ItemCode;
                    sapOrderParam.MEINS = p.UnitName;
                    sapOrderParam.MENGE = p.Quantity.ToString();
                    sapOrderParam.CHARG = p.LotCode;
                    sapOrderParam.LGORT = p.ToLocationCode;
                    sapOrderParam.BWART = "531";
                    sapOrderParam.GRUND = "";
                    rst.ITEMS.Add(sapOrderParam);
                });
            return rst;
        }

    }
}
