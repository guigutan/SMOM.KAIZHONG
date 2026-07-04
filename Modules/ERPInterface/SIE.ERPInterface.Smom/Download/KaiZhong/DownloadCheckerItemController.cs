using Newtonsoft.Json;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Datas;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.MES.Checker;
using SIE.MES.ItemChecker;
using SIE.Resources.Enterprises;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Smom.Download.KaiZhong
{
    public class DownloadCheckerItemController : DomainController
    {

        /// <summary>
        /// 保存分类到业务表
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public virtual ApiCommonRes SaveCheckerItemFactory(List<CheckerItemData> datas)
        {
            //返回ERP结果
            ApiCommonRes apiResult = new ApiCommonRes() { DataCount = datas.Count };
            List<CheckerItemData> list = new List<CheckerItemData>();
            var dataJson = JsonConvert.SerializeObject(datas);
            var logController = AppRuntime.Service.Resolve<InfDataLogController>();
            int failCount = 0;

            //记录日志
            var erpDataInfLog = logController.SaveErpDataInfLog(InfType.CheckerItem, dataJson, DateTime.Now, CallDirection.NcToMom, CallResult.UnSave, datas.Count);
            try
            {
                if (datas != null || datas.Count > 0)
                {
                    //检具
                    var checkerCodes = datas.Select(p => p.CheckerCode).Distinct().ToList();
                    var checkerUpholds = RT.Service.Resolve<CheckerUpholdController>().GetCheckerUpholdsByCodes(checkerCodes);

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

                            var checkerUphold = checkerUpholds.FirstOrDefault(p => p.CheckerCode == data.CheckerCode);
                            if (checkerUphold == null)
                            {
                                throw new ValidationException("检具维护{0}不存在".L10nFormat(data.CheckerCode));
                            }

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

                            SaveCheckerItem(data, checkerUphold, item, process);
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
                    logController.UpadateLogData<CheckerItemData>(erpDataInfLog, list, apiResult);
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
                logController.UpadateLogData<CheckerItemData>(erpDataInfLog, null, apiResult, ex.Message, 1);

            }
            return apiResult;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="data"></param>
        /// <param name="checkerUphold"></param>
        /// <param name="item"></param>
        /// <param name="process"></param>
        public virtual void SaveCheckerItem(CheckerItemData data, CheckerUphold checkerUphold, Item item, Process process)
        {
            try
            {
                CheckerItem checkerItem = new CheckerItem();

                checkerItem.PersistenceStatus = PersistenceStatus.New;
                checkerItem.CheckerUpholdId = checkerUphold.Id;
                checkerItem.ItemId = item.Id;
                if (process != null)
                    checkerItem.ProcessId = process.Id;
                checkerItem.DrawingNo = data.DrawingNo;

                RF.Save(checkerItem);
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
        public virtual void Valid(CheckerItemData data, ref OuterSystemRetVO result)
        {
            if (data.CheckerCode.IsNullOrEmpty())
            {
                var msg = "检具编码不能为空";
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
