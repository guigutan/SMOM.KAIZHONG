using Newtonsoft.Json;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Datas;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.MES.Fixture;
using SIE.MES.ItemFixture;
using SIE.Resources.Enterprises;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Smom.Download.KaiZhong
{
    public class DownloadFixtureItemController : DomainController
    {

        /// <summary>
        /// 保存分类到业务表
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public virtual ApiCommonRes SaveFixtureItemFactory(List<FixtureItemData> datas)
        {
            //返回ERP结果
            ApiCommonRes apiResult = new ApiCommonRes() { DataCount = datas.Count };
            List<FixtureItemData> list = new List<FixtureItemData>();
            var dataJson = JsonConvert.SerializeObject(datas);
            var logController = AppRuntime.Service.Resolve<InfDataLogController>();
            int failCount = 0;

            //记录日志
            var erpDataInfLog = logController.SaveErpDataInfLog(InfType.FixtureItem, dataJson, DateTime.Now, CallDirection.NcToMom, CallResult.UnSave, datas.Count);
            try
            {
                if (datas != null || datas.Count > 0)
                {
                    //工装维护
                    var fixtureCodes = datas.Select(p => p.FixtureCode).Distinct().ToList();
                    var fixtureUpholds = RT.Service.Resolve<FixtureUpholdController>().GetFixtureUpholdsByCodes(fixtureCodes);

                    //物料
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

                            var fixtureUphold = fixtureUpholds.Where(p => p.FixtureCode == data.FixtureCode).FirstOrDefault();
                            if (fixtureUphold == null)
                                throw new ValidationException("工装维护{0}不存在".L10nFormat(data.FixtureCode));

                            var item = items.FirstOrDefault(p => p.Code == data.ItemCode);
                            if (item == null)
                                throw new ValidationException("物料{0}不存在".L10nFormat(data.ItemCode));

                            Process process = null;
                            if (!data.ProcessCode.IsNullOrEmpty())
                            {
                                process = processes.Where(p => p.Code == data.ProcessCode).FirstOrDefault();
                                if (process == null)
                                    throw new ValidationException("工序{0}不存在".L10nFormat(data.ProcessCode));
                            }

                            SaveFixtureItem(data, fixtureUphold, item, process);
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
                    logController.UpadateLogData<FixtureItemData>(erpDataInfLog, list, apiResult);
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
                logController.UpadateLogData<FixtureItemData>(erpDataInfLog, null, apiResult, ex.Message, 1);

            }
            return apiResult;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fixtureUphold"></param>
        /// <param name="item"></param>
        /// <param name="process"></param>
        /// <exception cref="ValidationException"></exception>
        public virtual void SaveFixtureItem(FixtureItemData data, FixtureUphold fixtureUphold,Item item,Process process)
        {
            try
            {
                FixtureItem fixtureItem = new FixtureItem();

                fixtureItem.PersistenceStatus = PersistenceStatus.New;
                fixtureItem.FixtureUpholdId = fixtureUphold.Id;
                fixtureItem.ItemId = item.Id;
                if (process != null)
                    fixtureItem.ProcessId = process.Id;

                RF.Save(fixtureItem);
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
        public virtual void Valid(FixtureItemData data, ref OuterSystemRetVO result)
        {
            if (data.FixtureCode.IsNullOrEmpty())
            {
                var msg = "工装唯一码不能为空";
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
