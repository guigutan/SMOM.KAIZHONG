using Newtonsoft.Json;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Datas;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.MES.Threshold;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Smom.Download.KaiZhong
{
    public class DownloadThresholdController : DomainController
    {

        /// <summary>
        /// 保存分类到业务表
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public virtual ApiCommonRes SaveThresholdFactory(List<ThresholdData> datas)
        {
            //返回ERP结果
            ApiCommonRes apiResult = new ApiCommonRes() { DataCount = datas.Count };
            List<ThresholdData> list = new List<ThresholdData>();
            var dataJson = JsonConvert.SerializeObject(datas);
            var logController = AppRuntime.Service.Resolve<InfDataLogController>();
            int failCount = 0;

            //记录日志
            var erpDataInfLog = logController.SaveErpDataInfLog(InfType.Threshold, dataJson, DateTime.Now, CallDirection.NcToMom, CallResult.UnSave, datas.Count);
            try
            {
                if (datas != null || datas.Count > 0)
                {
                    //获取工序
                    var processCodes = datas.Select(p => p.ProcessCode).Distinct().ToList();
                    var processes = RT.Service.Resolve<ProcessController>().GetProcessesList(processCodes);
                    //获取产品
                    var itemCodes = datas.Select(p => p.ItemCode).Distinct().ToList();
                    var items = RT.Service.Resolve<ItemController>().GetItemDataList(itemCodes, new EagerLoadOptions().LoadWithViewProperty());

                    foreach (var data in datas)
                    {
                        try
                        {
                            if (data.ItemCode.IsNullOrEmpty())
                                throw new ValidationException("物料编码不能为空");
                            if (data.ProcessCode.IsNullOrEmpty())
                                throw new ValidationException("工序编码不能为空");
                            var item = items.FirstOrDefault(p => p.Code == data.ItemCode);
                            if (item == null)
                                throw new ValidationException("物料{0}不存在!".L10nFormat(data.ItemCode));

                            var process = processes.FirstOrDefault(p => p.Code == data.ProcessCode);
                            if (process == null)
                                throw new ValidationException("工序{0}不存在!".L10nFormat(data.ProcessCode));
                            if(!decimal.TryParse(data.AlertValue,out decimal AlertValue))
                                throw new ValidationException("预警值{0}非数字类型,无法转换!".L10nFormat(data.AlertValue));
                            if (!decimal.TryParse(data.ThresholdValue, out decimal ThresholdValue))
                                throw new ValidationException("目标值{0}非数字类型,无法转换!".L10nFormat(data.ThresholdValue));

                            SaveThreshold(data, item, process, AlertValue);
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
                    logController.UpadateLogData<ThresholdData>(erpDataInfLog, list, apiResult);
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
                logController.UpadateLogData<ThresholdData>(erpDataInfLog, null, apiResult, ex.Message, 1);

            }
            return apiResult;

        }

        /// <summary>
        /// 保存可疑品阈值
        /// </summary>
        /// <param name="data"></param>
        /// <param name="item"></param>
        /// <param name="process"></param>
        public virtual void SaveThreshold(ThresholdData data, Item item, Process process, decimal AlertValue)
        {
            try
            {
                //此处接口只做新增不做更新和删除，所以遇到 相同的数据，直接跳过就行
                Threshold threshold = new Threshold();

                threshold.ProcessId = process.Id;
                threshold.ItemId = item.Id;
                threshold.ThresholdValue = data.ThresholdValue;
                threshold.AlertValue = AlertValue;

                RF.Save(threshold);
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
        public virtual void Valid(ThresholdData data, ref OuterSystemRetVO result)
        {
            if (!decimal.TryParse(data.AlertValue, out decimal value))
            {
                result.success = false;
                result.errorMsg = result.errorMsg.IsNullOrEmpty() ? "预警值非数字，转换失败" : result.errorMsg + ";" + "预警值非数字，转换失败";
                result.errorList.Add(data);
                return;
            }
            if (!decimal.TryParse(data.ThresholdValue, out value))
            {
                result.success = false;
                result.errorMsg = result.errorMsg.IsNullOrEmpty() ? "目标值非数字，转换失败" : result.errorMsg + ";" + "目标值非数字，转换失败";
                result.errorList.Add(data);
                return;
            }
            if (data.ProcessCode.IsNullOrEmpty())
            {
                result.success = false;
                result.errorMsg = result.errorMsg.IsNullOrEmpty() ? "工序编码不能为空" : result.errorMsg + ";" + "工序编码不能为空";
                result.errorList.Add(data);
                return;
            }
            if (data.ItemCode.IsNullOrEmpty())
            {
                result.success = false;
                result.errorMsg = result.errorMsg.IsNullOrEmpty() ? "产品编码不能为空" : result.errorMsg + ";" + "产品编码不能为空";
                result.errorList.Add(data);
                return;
            }
        }

    }
}
