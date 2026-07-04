using Newtonsoft.Json;
using SIE.Common;
using SIE.Domain;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Sap.Connection;
using SIE.ERPInterface.Sap.Datas.ErpInfoDatas.Deduction;
using SIE.ERPInterface.Sap.Datas.ErpInfoDatas.SaleReturn;
using SIE.MES.TaskManagement.FeedingRecords;
using SIE.Rbac.InvOrgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Sap.Upload.Deduction
{
    public class UploadDeductionHandle : IUploadDataHandler<KzDeductionUploadData>
    {

        /// <summary>
        /// 分组
        /// </summary>
        /// <param name="uploadTransactions"></param>
        public Dictionary<string, List<UploadTransaction>> Grouped(EntityList<UploadTransaction> uploadTransactions)
        {
            var dic = new Dictionary<string, List<UploadTransaction>>();
            //一条数据一条数据上传(应讨论结果)
            dic = uploadTransactions.GroupBy(p => new { p.WoId, p.ItemId, p.ProcessCode, p.LotCode, p.Vornr, p.ToLocationCode }).ToDictionary(p => p.Key.ToString(), p => p.ToList());
            //dic.Add("", uploadTransactions.ToList());
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

            var uploadTransaction = uploadTransactions.FirstOrDefault();
            KzDeductionUploadData sapOrderParam = new KzDeductionUploadData();
            sapOrderParam.AUFNR = uploadTransaction.WoNo;
            sapOrderParam.PLANT = uploadTransaction.WERKS;
            sapOrderParam.BUDAT = cur.ToString("yyyyMMdd");
            sapOrderParam.MATNR = uploadTransaction.ItemCode;
            sapOrderParam.MEINS = uploadTransaction.UnitName;
            sapOrderParam.MENGE = uploadTransactions.Sum(p => p.Quantity).ToString();
            sapOrderParam.CHARG = uploadTransaction.LotCode;
            sapOrderParam.LGORT = uploadTransaction.ToLocationCode;
            sapOrderParam.BWART = "261";
            sapOrderParam.GRUND = "";
            rst.ITEMS.Add(sapOrderParam);

            //uploadTransactions
            //    //.GroupBy(f => new { f.WoNo, f.ProcessCode })   
            //    .ForEach(p =>
            //    {
            //        KzDeductionUploadData sapOrderParam = new KzDeductionUploadData();
            //        sapOrderParam.AUFNR = p.WoNo;
            //        sapOrderParam.PLANT = p.WERKS;
            //        sapOrderParam.BUDAT = cur.ToString("yyyyMMdd");
            //        sapOrderParam.MATNR = p.ItemCode;
            //        sapOrderParam.MEINS = p.UnitName;
            //        sapOrderParam.MENGE = p.Quantity.ToString();
            //        sapOrderParam.CHARG = p.LotCode;
            //        sapOrderParam.LGORT = p.ToLocationCode;
            //        sapOrderParam.BWART = "261";
            //        sapOrderParam.GRUND = "";
            //        rst.ITEMS.Add(sapOrderParam);
            //    });
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
                        var sourIds = new List<double>();
                        if (!ut.SourceId.IsNullOrEmpty())
                            sourIds = ut.SourceId.Split(';').Select(p => Convert.ToDouble(p)).ToList();
                        //再加一个判断是因为SourceId是后续加的，前面的数据仍然是根据BillId来的，防止之前的数据中SourceId为空导致数据更新失败
                        if (sourIds.Count == 0 && ut.BillId != null)
                            sourIds.Add(ut.BillId.Value);
                        if (sourIds.Count > 0)
                            DB.Update<DeductionRecord>().Set(p => p.UploadResult, r.ZFKXX).Set(p => p.Mblnr, r.MBLNR).Set(p => p.Mjahr, r.MJAHR).Where(p => sourIds.Contains(p.Id)).Execute();
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
                        .Where(p => ids.Contains(p.Id) && p.UploadCount >= 5 && p.Quantity >= (decimal)0.001)
                        .Execute();
                    //先更新放弃的，再更新重试的
                    DB.Update<UploadTransaction>()
                        .Set(p => p.State, Common.Enums.ProcessState.Abandon)
                        .Set(p => p.ValidateMessage, r.ZFKXX)
                        .Set(p => p.ProcessMessage, r.ZFKXX)
                        .Set(p => p.Mblnr, r.MBLNR)
                        .Set(p => p.Mjahr, r.MJAHR)
                        .Set(p => p.UploadCount, p => p.UploadCount + 1)
                        .Where(p => ids.Contains(p.Id) && p.UploadCount >= 5 && p.Quantity < (decimal)0.001)
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
                        var sourIds = new List<double>();
                        if (!ut.SourceId.IsNullOrEmpty())
                            sourIds = ut.SourceId.Split(';').Select(p => Convert.ToDouble(p)).ToList();
                        //再加一个判断是因为SourceId是后续加的，前面的数据仍然是根据BillId来的，防止之前的数据中SourceId为空导致数据更新失败
                        if (sourIds.Count == 0 && ut.BillId != null)
                            sourIds.Add(ut.BillId.Value);
                        if (sourIds.Count > 0)
                            DB.Update<DeductionRecord>().Set(p => p.UploadResult, r.ZFKXX).Set(p => p.Mblnr, r.MBLNR).Set(p => p.Mjahr, r.MJAHR).Where(p => sourIds.Contains(p.Id)).Execute();
                    }
                    processResult.AddFailMsg("工单{0}上传失败:{1};".L10nFormat(r.AUFNR, r.ZFKXX));
                }

            }
            return processResult;
        }

        public SapUploadParam<KzDeductionUploadData> SetUploadData(List<UploadTransaction> uploadTransactions)
        {
            return new SapUploadParam<KzDeductionUploadData>();
        }

    }
}
