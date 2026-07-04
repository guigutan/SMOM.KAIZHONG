using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using SIE.Core;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.EquipAccounts;
using SIE.Items;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Datas;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.MES.ReworkLayoutVersions;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Item = SIE.Items.Item;

namespace SIE.ERPInterface.Smom.Download.KaiZhong
{
    public class DownloadReworkLayoutVersionController : DomainController
    {

        /// <summary>
        /// 保存分类到业务表
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public virtual ApiCommonRes SaveEquipAccountItemFactory(Rlvd data)
        {
            //返回ERP结果
            ApiCommonRes apiResult = new ApiCommonRes() { DataCount = data.Data1.Count };
            List<ReworkLayoutVersionData> list = new List<ReworkLayoutVersionData>();
            var dataJson = JsonConvert.SerializeObject(data);
            var logController = AppRuntime.Service.Resolve<InfDataLogController>();
            int failCount = 0;

            //记录日志
            var erpDataInfLog = logController.SaveErpDataInfLog(InfType.ReworkLayoutVersion, dataJson, DateTime.Now, CallDirection.NcToMom, CallResult.UnSave, data.Data1.Count);
            try
            {
                if (data != null || data.Data1.Count > 0)
                {
                    //产品
                    var itemCodes = data.Data1.Where(p => !p.MATNR.IsNullOrEmpty()).Select(p => p.MATNR).Distinct().ToList();
                    var items = RT.Service.Resolve<ItemController>().GetItemDataList(itemCodes, new EagerLoadOptions().LoadWithViewProperty());

                    //工序
                    var processCodes = data.Data1.SelectMany(p=>p.Data2).Where(p => !p.KTSCH.IsNullOrEmpty()).Select(p => p.KTSCH).Distinct().ToList();
                    var processes = RT.Service.Resolve<ProcessController>().GetProcessesList(processCodes);

                    EntityList<ReworkLayoutVersion> reworkLayoutVersions = RT.Service.Resolve<ReworkLayoutVersionController>().GetReworkLayoutVersionsByItemCodes(itemCodes);

                    foreach (var d in data.Data1)
                    {
                        try
                        {
                            OuterSystemRetVO result = new OuterSystemRetVO();
                            Valid(d, ref result);
                            if (!result.errorMsg.IsNullOrEmpty())
                                throw new ValidationException(result.errorMsg);

                            var reworkLayoutVersion = reworkLayoutVersions.FirstOrDefault(p => p.ItemCode == d.MATNR && p.Factory == d.WERKS && p.Version == d.VERID);

                            var item = items.FirstOrDefault(p => p.Code == d.MATNR);
                            if (item == null)
                            {
                                throw new ValidationException("产品{0}不存在".L10nFormat(d.MATNR));
                            }

                            SaveReworkLayoutVersion(d, reworkLayoutVersion, processes, item);

                            if (reworkLayoutVersions.All(p => p.Id != reworkLayoutVersion.Id))
                                reworkLayoutVersions.Add(reworkLayoutVersion);

                            apiResult.SuccessList.Add(data);
                        }
                        catch (Exception ex)
                        {
                            failCount++;
                            apiResult.ErrorList.Add(ex.GetBaseException().Message);
                            apiResult.ErrorObjList.Add(data);
                        }
                    }

                    apiResult.SuccessCount = list.Count;
                    apiResult.FailCount = failCount;
                    logController.UpadateLogData<Rlvd>(erpDataInfLog, new List<Rlvd>() { data }, apiResult);
                }
                else
                {
                    apiResult.ErrorList.Add("同步数据不能为空!".L10N());
                }
            }
            catch (Exception ex)
            {
                apiResult.ErrorList.Clear();
                apiResult.FailCount = data.Data1.Count;
                apiResult.ErrorObjList.Clear();
                apiResult.ErrorObjList.Add(data);
                apiResult.ErrorList.Add(ex.Message);
                logController.UpadateLogData<Rlvd>(erpDataInfLog, null, apiResult, ex.Message, 1);

            }
            return apiResult;
        }

        public virtual void SaveReworkLayoutVersion(ReworkLayoutVersionData data, ReworkLayoutVersion reworkLayoutVersion, EntityList<Process> processes,Item item)
        {
            using (var tran = DB.TransactionScope("MES")) {
                if (reworkLayoutVersion == null)
                {
                    reworkLayoutVersion = new ReworkLayoutVersion();
                    reworkLayoutVersion.PersistenceStatus = PersistenceStatus.New;
                }
                reworkLayoutVersion.Version = data.VERID;
                reworkLayoutVersion.Desc = data.TEXT1;
                reworkLayoutVersion.ItemId = item.Id;
                reworkLayoutVersion.Factory = data.WERKS;
                if (!data.GSTRP.IsNullOrEmpty() && DateTimeFormat.SafeConvertToDateTime(data.GSTRP, out var gstrp))
                    reworkLayoutVersion.BeginDateTime = gstrp;
                if (!data.GLTRP.IsNullOrEmpty() && DateTimeFormat.SafeConvertToDateTime(data.GLTRP, out var gltrp))
                    reworkLayoutVersion.EndDateTime = gltrp;
                reworkLayoutVersion.Type = data.PLNTY;
                if (!data.BDATU.IsNullOrEmpty() && DateTimeFormat.SafeConvertToDateTime(data.BDATU, out var bdatu))
                    reworkLayoutVersion.EffEndDateTime = bdatu;
                if (!data.ADATU.IsNullOrEmpty() && DateTimeFormat.SafeConvertToDateTime(data.ADATU, out var adatu))
                    reworkLayoutVersion.EffBeginDateTime = adatu;
                reworkLayoutVersion.Group = data.PLNNR;
                reworkLayoutVersion.Counter = data.ALNAL;

                //旧的删掉，重新用新的覆盖
                foreach (var item1 in reworkLayoutVersion.ReworkLayoutList)
                {
                    item1.PersistenceStatus = PersistenceStatus.Deleted;
                    RF.Save(item1);
                }
                reworkLayoutVersion.ReworkLayoutList.Clear();

                foreach (var layout in data.Data2)
                {
                    Process process = null;
                    if (!layout.KTSCH.IsNullOrEmpty())
                    {
                        process = processes.FirstOrDefault(p => p.Code == layout.KTSCH);
                        if (process == null)
                        {
                            throw new ValidationException("工序{0}不存在".L10nFormat(layout.KTSCH));
                        }
                    }

                    ReworkLayout reworkLayout = new ReworkLayout();
                    reworkLayout.Vornr = layout.VORNR;
                    reworkLayout.ProcessCode = layout.KTSCH;
                    reworkLayout.WorkCenterCode = layout.ARBPL;
                    reworkLayout.Steus = layout.STEUS;
                    reworkLayout.Factory = layout.WERKS_D;
                    reworkLayout.ProcessQty = layout.MGVRG;
                    reworkLayout.Zcode = layout.ZCODE;
                    reworkLayout.PersistenceStatus = PersistenceStatus.New;
                    reworkLayoutVersion.ReworkLayoutList.Add(reworkLayout);
                }

                RF.Save(reworkLayoutVersion);
                tran.Complete();
            }

        }


        /// <summary>
        /// 校验
        /// </summary>
        /// <param name="data"></param>
        /// <param name="result"></param>
        public virtual void Valid(ReworkLayoutVersionData data, ref OuterSystemRetVO result)
        {
            if (data.VERID.IsNullOrEmpty())
            {
                var msg = "生产版本不能为空";
                result.success = false;
                result.errorMsg = result.errorMsg.IsNullOrEmpty() ? msg : result.errorMsg + ";" + msg;
                result.errorList.Add(data);
                return;
            }
            if (data.TEXT1.IsNullOrEmpty())
            {
                var msg = "版本描述不能为空";
                result.success = false;
                result.errorMsg = result.errorMsg.IsNullOrEmpty() ? msg : result.errorMsg + ";" + msg;
                result.errorList.Add(data);
                return;
            }
            if (data.MATNR.IsNullOrEmpty())
            {
                var msg = "产品编码不能为空";
                result.success = false;
                result.errorMsg = result.errorMsg.IsNullOrEmpty() ? msg : result.errorMsg + ";" + msg;
                result.errorList.Add(data);
                return;
            }
            if (data.PLNTY.IsNullOrEmpty())
            {
                var msg = "任务清单类型不能为空";
                result.success = false;
                result.errorMsg = result.errorMsg.IsNullOrEmpty() ? msg : result.errorMsg + ";" + msg;
                result.errorList.Add(data);
                return;
            }
            if (data.PLNNR.IsNullOrEmpty())
            {
                var msg = "任务清单组不能为空";
                result.success = false;
                result.errorMsg = result.errorMsg.IsNullOrEmpty() ? msg : result.errorMsg + ";" + msg;
                result.errorList.Add(data);
                return;
            }
            if (data.ALNAL.IsNullOrEmpty())
            {
                var msg = "组计数器不能为空";
                result.success = false;
                result.errorMsg = result.errorMsg.IsNullOrEmpty() ? msg : result.errorMsg + ";" + msg;
                result.errorList.Add(data);
                return;
            }
            if (!data.GSTRP.IsNullOrEmpty())
            {
                if (!DateTimeFormat.SafeConvertToDateTime(data.GSTRP, out DateTime gstrp))
                {
                    var msg = "基本开始日期格式错误，转换失败";
                    result.success = false;
                    result.errorMsg = result.errorMsg.IsNullOrEmpty() ? msg : result.errorMsg + ";" + msg;
                    result.errorList.Add(data);
                    return;
                }
            }
            if (!data.GLTRP.IsNullOrEmpty())
            {
                if (!DateTimeFormat.SafeConvertToDateTime(data.GLTRP, out DateTime gltrp))
                {
                    var msg = "基本完成日期格式错误，转换失败";
                    result.success = false;
                    result.errorMsg = result.errorMsg.IsNullOrEmpty() ? msg : result.errorMsg + ";" + msg;
                    result.errorList.Add(data);
                    return;
                }
            }
            if (!data.BDATU.IsNullOrEmpty())
            {
                if (!DateTimeFormat.SafeConvertToDateTime(data.BDATU, out DateTime bdatu))
                {
                    var msg = "有效期截止日期格式错误，转换失败";
                    result.success = false;
                    result.errorMsg = result.errorMsg.IsNullOrEmpty() ? msg : result.errorMsg + ";" + msg;
                    result.errorList.Add(data);
                    return;
                }
            }
            if (!data.ADATU.IsNullOrEmpty())
            {
                if (!DateTimeFormat.SafeConvertToDateTime(data.ADATU, out DateTime adatu))
                {
                    var msg = "有效开始日期格式错误，转换失败";
                    result.success = false;
                    result.errorMsg = result.errorMsg.IsNullOrEmpty() ? msg : result.errorMsg + ";" + msg;
                    result.errorList.Add(data);
                    return;
                }
            }
        }

    }
}
