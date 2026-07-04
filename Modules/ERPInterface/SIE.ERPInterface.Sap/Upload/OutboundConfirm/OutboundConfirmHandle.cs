using DotLiquid.Util;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Sap.Datas.ErpInfoDatas.Deduction;
using SIE.ERPInterface.Sap.Datas.ErpInfoDatas.OutboundConfirm;
using SIE.MES.Outsourcing;
using SIE.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Sap.Upload.OutboundConfirm
{
    public class OutboundConfirmHandle : IUploadDataHandler<KzDeductionUploadData>
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
            var cur = RF.Find<UploadTransaction>().GetDbTime();
            List<KzOutboundConfirmUploadData> datas = new List<KzOutboundConfirmUploadData>();
            var zuids = uploadTransactions.Select(p => p.Zuid).Distinct().ToList();
            var details = RT.Service.Resolve<OutsourcingController>().GetOutboundConfirmDetailsByZuid(zuids);

            var uploadTransaction = uploadTransactions.FirstOrDefault();
            var detail = details.FirstOrDefault(p => p.Zuid == uploadTransaction.Zuid);
            if (detail == null)
                throw new ValidationException("未找到对应的发货确认明细".L10N());
            KzOutboundConfirmUploadData data = new KzOutboundConfirmUploadData();
            var employee = RF.GetById<Employee>(detail.CreateBy);
            data.FLOWNO = detail.FlowNo;
            data.OUTER = employee?.Code;
            data.INITIATORFACTORY = detail.InitiatorFactory;
            data.OUTFACTORY = detail.OutFactory;
            data.STATE = detail.FlowNo.IsNullOrEmpty() ? "0" : "1";
            data.DATE = cur.ToString("yyyy-MM-dd");
            data.QTY = detail.Qty;
            data.ZUID = uploadTransaction.Zuid;
            return JsonConvert.SerializeObject(data);
            //foreach (var uploadTransaction in uploadTransactions)
            //{
            //    var detail = details.FirstOrDefault(p => p.Zuid == uploadTransaction.Zuid);
            //    if (detail == null)
            //        continue;
            //    KzOutboundConfirmUploadData data = new KzOutboundConfirmUploadData();
            //    var employee = RF.GetById<Employee>(detail.CreateBy);
            //    data.FLOWNO = detail.FlowNo;
            //    data.OUTER = employee?.Code;
            //    data.INITIATORFACTORY = detail.InitiatorFactory;
            //    data.OUTFACTORY = detail.OutFactory;
            //    data.STATE = detail.State.ToLabel();
            //    data.DATE = cur.ToString("yyyyMMddHHmmss");
            //    data.QTY = detail.Qty;
            //    data.ZUID = uploadTransaction.Zuid;
            //    datas.Add(data);
            //}

            //return JsonConvert.SerializeObject(datas);
        }

        public SapUploadParam<KzDeductionUploadData> SetUploadData(List<UploadTransaction> uploadTransactions)
        {
            throw new NotImplementedException();
        }

        public ProcessResult Uploaded(SapResult sapResult, EntityList<UploadTransaction> uploadTransactions, string str)
        {
            var param = JsonConvert.DeserializeObject<KzOutboundConfirmResultData>(sapResult.ResponseStr);
            ProcessResult processResult = new ProcessResult();
            var uploadTransaction = uploadTransactions.FirstOrDefault();

            if (param != null)
            {
                if (uploadTransaction != null)
                {
                    OutboundConfirmDetailState? State = null;
                    if (param.state == "S")
                    {
                        State = OutboundConfirmDetailState.Delivery;
                        uploadTransaction.State = Common.Enums.ProcessState.Processed;
                    }
                    else
                    {
                        uploadTransaction.State = Common.Enums.ProcessState.Failed;
                    }
                    uploadTransaction.ProcessMessage = param.message;
                    uploadTransaction.ValidateMessage = param.message;
                    if (!param.FLOWNO.IsNullOrEmpty())
                        uploadTransaction.BillNo = param.FLOWNO;
                    uploadTransaction.UploadCount = (uploadTransaction.UploadCount ?? 0) + 1;
                    if (uploadTransaction.UploadCount < 5 && uploadTransaction.State == Common.Enums.ProcessState.Failed)
                        uploadTransaction.State = Common.Enums.ProcessState.Retry;
                    RF.Save(uploadTransaction);

                    var update = DB.Update<OutboundConfirmDetail>().Set(p => p.OaMsg, param.message).Where(p => p.Id == uploadTransaction.BillId);

                    if (!param.FLOWNO.IsNullOrEmpty())
                        update.Set(p => p.FlowNo, param.FLOWNO);

                    if (State != null && State == OutboundConfirmDetailState.Delivery)
                        update.Set(p => p.State, State);
                    update.Execute();

                    processResult.AddSuccessMsg(param.message.L10N());
                }
            }
            else
            {
                uploadTransaction.State = Common.Enums.ProcessState.Failed;
                uploadTransaction.ProcessMessage = "接口返回格式不正确";
                uploadTransaction.ValidateMessage = "接口返回格式不正确";
                uploadTransaction.UploadCount = (uploadTransaction.UploadCount ?? 0) + 1;
                if (uploadTransaction.UploadCount < 5)
                    uploadTransaction.State = Common.Enums.ProcessState.Retry;

                RF.Save(uploadTransaction);
                DB.Update<OutboundConfirmDetail>().Set(p => p.OaMsg, "接口返回格式不正确").Where(p => p.Id == uploadTransaction.BillId).Execute();
                processResult.AddSuccessMsg("接口返回格式不正确".L10N());
            }
            return processResult;
        }
    }
}
