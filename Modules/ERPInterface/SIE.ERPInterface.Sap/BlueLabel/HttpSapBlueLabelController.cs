using Newtonsoft.Json;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Sap.Connection;
using SIE.ERPInterface.Sap.Controller;
using SIE.ERPInterface.Sap.Datas.ErpInfoDatas.Deduction;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.MES.BlueLable;
using SIE.MES.PackingQC;
using SIE.MES.WorkOrders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Sap.Upload.BlueLabel
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpSapBlueLabelController : SapUploadController
    {
        /// <summary>
        /// 蓝标装箱上传
        /// </summary>      
        public virtual (int, int, string) UploadBlueLabelToSap(IList<PackingQc> packingQcs)
        {

            var msg = string.Empty;
            var totalCount = packingQcs.Count;
            var successCount = 0;

            //接口名称,必改
            string interfaceName = "LanCheck";

            var invOrgs = RF.GetAll<Rbac.InvOrgs.InvOrg>();
            var invOrg = invOrgs.FirstOrDefault(p => p.Code == RT.InvOrg);
            var blueLabels = packingQcs.Select(p => p.BlueLabel).Distinct().ToList();

            var blueLables = RT.Service.Resolve<BlueLableController>().GetBlueLableDatas(blueLabels);
            var woNos = blueLables.Select(p => p.ProductionNo).Distinct().ToList();
            var wos = RT.Service.Resolve<WorkOrderController>().GetWorkOrders(woNos);
            //防止批量失败,单独上传
            foreach (var qc in packingQcs)
            {
                var list = new List<KzBlueLabelRequestData>();
                var blueLable = blueLables.FirstOrDefault(p => p.BlueLableBox == qc.BlueLabel);
                var wo = wos.FirstOrDefault(p => p.No == blueLable.ProductionNo);
                if (qc.PackingNum == 0)
                {
                    list.Add(new KzBlueLabelRequestData { EXIDV = qc.BlueLabel, WERKS = wo?.Factory?.Code, ZPACK = "N" }); //空箱状态,取消标识
                }
                else
                {
                    list.Add(new KzBlueLabelRequestData { EXIDV = qc.BlueLabel, WERKS = wo?.Factory?.Code, ZPACK = "Y" }); //正常装箱标识
                    if (qc.OldBlueLabel.IsNotEmpty())
                        list.Add(new KzBlueLabelRequestData { EXIDV = qc.OldBlueLabel, WERKS = wo?.Factory?.Code, ZPACK = "N" });  //换箱蓝标,取消标识
                }

                //构建上传数据结构
                var data = new
                {
                    ITEMS = list
                };
                //设置参数
                var json = JsonConvert.SerializeObject(data);

                var erpDataInfLog = RT.Service.Resolve<InfDataLogController>().SaveErpDataInfLog(InfType.BlueLabelToSap, json, DateTime.Now, CallDirection.MesToSap, CallResult.UnSave, 1);
                try
                {
                    SapResult sapResult = RT.Service.Resolve<HttpHelper>().InvokeSapAPI(interfaceName, json);
                    erpDataInfLog.ResponseContent = sapResult.ResponseStr;

                    if (sapResult.IsSuccess == false)
                        throw new ValidationException("接口调用失败:{0}".L10nFormat(sapResult.ResponseStr));
                    sapResult.InterfaceName = interfaceName;

                    if (sapResult.IsSuccess == true)
                        erpDataInfLog.CallResult = CallResult.Success;
                    else
                        erpDataInfLog.CallResult = CallResult.Fail;

                    //上传后处理
                    var result = JsonConvert.DeserializeObject<KzBlueLabelResultData>(sapResult.ResponseStr);
                    var isUpload = result.Return.ZFKBS == "S";
                    var uploadResult = result.Return.ZFKXX;
                    DB.Update<PackingQc>().Set(p => p.IsUploadSap, isUpload).Set(p => p.UploadResult, uploadResult).Where(p => p.Id == qc.Id).Execute();
                    erpDataInfLog.TipMsg = uploadResult;
                    if (isUpload)
                        successCount++;

                }
                catch (Exception exx)
                {
                    erpDataInfLog.ErrorMsg = exx.InnerException != null ? exx.InnerException.Message : exx.Message;
                    erpDataInfLog.CallResult = CallResult.Fail;
                }
                finally
                {
                    erpDataInfLog.EndDate = DateTime.Now;
                    erpDataInfLog.PersistenceStatus = PersistenceStatus.Modified;
                    RF.Save(erpDataInfLog);
                }
            }

            return (totalCount, successCount, msg);
        }
    }
}
