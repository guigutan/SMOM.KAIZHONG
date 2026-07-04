using Newtonsoft.Json;
using SIE.Core.Equipments;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.EquipAccounts;
using SIE.Items;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Datas;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.MES.Checker;
using SIE.MES.ItemEquipAccount;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EquipAccount = SIE.Equipments.EquipAccounts.EquipAccount;

namespace SIE.ERPInterface.Smom.Download.KaiZhong
{
    public class DownloadEquipAccountItemController : DomainController
    {

        /// <summary>
        /// 保存分类到业务表
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public virtual ApiCommonRes SaveEquipAccountItemFactory(List<EquipAccountItemData> datas)
        {
            //返回ERP结果
            ApiCommonRes apiResult = new ApiCommonRes() { DataCount = datas.Count };
            List<EquipAccountItemData> list = new List<EquipAccountItemData>();
            var dataJson = JsonConvert.SerializeObject(datas);
            var logController = AppRuntime.Service.Resolve<InfDataLogController>();
            int failCount = 0;

            //记录日志
            var erpDataInfLog = logController.SaveErpDataInfLog(InfType.EquipAccountItem, dataJson, DateTime.Now, CallDirection.NcToMom, CallResult.UnSave, datas.Count);
            try
            {
                if (datas != null || datas.Count > 0)
                {
                    //模具
                    var equipAccountCodes = datas.Select(p => p.EquipAccountCode).Distinct().ToList();
                    var eqList = RT.Service.Resolve<EquipAccountController>().GetEquipAccountsByCode(equipAccountCodes);

                    //产品
                    var itemCodes = datas.Select(p => p.ItemCode).Distinct().ToList();
                    var items = RT.Service.Resolve<ItemController>().GetItemDataList(itemCodes, new EagerLoadOptions().LoadWithViewProperty());

                    //工序
                    var processCodes = datas.Select(p => p.ProcessCode).Distinct().ToList();
                    var processes = RT.Service.Resolve<ProcessController>().GetProcessesList(processCodes);

                    foreach (var data in datas)
                    {
                        try
                        {
                            OuterSystemRetVO result = new OuterSystemRetVO();
                            Valid(data, ref result);
                            if (!result.errorMsg.IsNullOrEmpty())
                                throw new ValidationException(result.errorMsg);

                            var eq = eqList.FirstOrDefault(p => p.Code == data.EquipAccountCode);
                            if (eq == null)
                                throw new ValidationException("模具{0}不存在".L10nFormat(data.EquipAccountCode));

                            var item = items.FirstOrDefault(p => p.Code == data.ItemCode);
                            if (item == null)
                            {
                                throw new ValidationException("产品{0}不存在".L10nFormat(data.ItemCode));
                            }

                            Process process = null;
                            if (!data.ProcessCode.IsNullOrEmpty())
                            {
                                process = processes.FirstOrDefault(p => p.Code == data.ProcessCode);
                                if (process == null)
                                {
                                    throw new ValidationException("工序{0}不存在".L10nFormat(data.ProcessCode));
                                }
                            }

                            SaveEquipAccountItem(data, eq, item, process);
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
                    logController.UpadateLogData<EquipAccountItemData>(erpDataInfLog, list, apiResult);
                }
                else
                {
                    apiResult.ErrorList.Add("同步数据不能为空!".L10N());
                }
            }
            catch (Exception ex)
            {
                apiResult.ErrorList.Clear();
                apiResult.FailCount = datas.Count;
                apiResult.ErrorObjList.Clear();
                apiResult.ErrorObjList.AddRange(datas);
                apiResult.ErrorList.Add(ex.Message);
                logController.UpadateLogData<EquipAccountItemData>(erpDataInfLog, null, apiResult, ex.Message, 1);

            }
            return apiResult;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="data"></param>
        public virtual void SaveEquipAccountItem(EquipAccountItemData data, EquipAccount equipAccount,Item item,Process process)
        {
            try
            {
                EquipAccountItem equipAccountItem = new EquipAccountItem();

                equipAccountItem.PersistenceStatus = PersistenceStatus.New;
                equipAccountItem.EquipAccountId = equipAccount.Id;
                equipAccountItem.ItemId = item.Id;
                if (process != null)
                    equipAccountItem.ProcessId = process.Id;
                equipAccountItem.UniqueCode = data.UniqueCode;

                RF.Save(equipAccountItem);
            }
            catch (Exception ex)
            {
                if (!ex.GetBaseException().Message.Contains("数据已存在"))
                    throw new ValidationException(ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// 校验
        /// </summary>
        /// <param name="data"></param>
        /// <param name="result"></param>
        public virtual void Valid(EquipAccountItemData data, ref OuterSystemRetVO result)
        {
            if (data.EquipAccountCode.IsNullOrEmpty())
            {
                var msg = "模具编码不能为空";
                result.success = false;
                result.errorMsg = result.errorMsg.IsNullOrEmpty() ? msg : result.errorMsg + ";" + msg;
                result.errorList.Add(data);
                return;
            }
            if (data.ItemCode.IsNullOrEmpty())
            {
                var msg = "产品编码不能为空";
                result.success = false;
                result.errorMsg = result.errorMsg.IsNullOrEmpty() ? msg : result.errorMsg + ";" + msg;
                result.errorList.Add(data);
                return;
            }
        }
    }
}
