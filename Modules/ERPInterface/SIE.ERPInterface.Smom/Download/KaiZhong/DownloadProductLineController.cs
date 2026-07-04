using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Datas;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.MES.Andon;
using SIE.MES.ItemLine;
using SIE.MES.LineAndon;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Resources.WorkCenters;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Smom.Download.KaiZhong
{
    public class DownloadProductLineController : DomainController
    {

        /// <summary>
        /// 保存分类到业务表
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public virtual ApiCommonRes SaveProductLineFactory(List<ProductLineData> datas)
        {
            //返回ERP结果
            ApiCommonRes apiResult = new ApiCommonRes() { DataCount = datas.Count };
            List<ProductLineData> list = new List<ProductLineData>();
            var dataJson = JsonConvert.SerializeObject(datas);
            var logController = AppRuntime.Service.Resolve<InfDataLogController>();
            int failCount = 0;

            //记录日志
            var erpDataInfLog = logController.SaveErpDataInfLog(InfType.ProductLine, dataJson, DateTime.Now, CallDirection.NcToMom, CallResult.UnSave, datas.Count);
            try
            {
                if (datas != null || datas.Count > 0)
                {

                    //获取产品
                    var itemCodes = datas.Select(p => p.ItemCode).Distinct().ToList();
                    var items = RT.Service.Resolve<ItemController>().GetItemDataList(itemCodes, new EagerLoadOptions().LoadWithViewProperty());

                    //获取机台/产线
                    var wipResourceCodes = datas.Select(p => p.WipResourceCode).Distinct().ToList();
                    var wipResources = RT.Service.Resolve<WipResourceController>().GetWipResourceByCodes(wipResourceCodes);

                    //获取工序
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

                            var item = items.Where(p => p.Code == data.ItemCode).FirstOrDefault();
                            if (item == null)
                                throw new ValidationException("物料{0}不存在".L10nFormat(data.ItemCode));

                            var wipResource = wipResources.FirstOrDefault(p => p.Code == data.WipResourceCode);
                            if (wipResource == null)
                                throw new ValidationException("产线/机台{0}不存在".L10nFormat(data.WipResourceCode));

                            Process process = null;
                            if (!data.ProcessCode.IsNullOrEmpty())
                            {
                                process = processes.FirstOrDefault(p => p.Code == data.ProcessCode);
                                if (process == null)
                                    throw new ValidationException("工序{0}不存在".L10nFormat(data.ProcessCode));
                            }

                            SaveProductLine(data, item, wipResource, process);
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
                    logController.UpadateLogData<ProductLineData>(erpDataInfLog, list, apiResult);
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
                logController.UpadateLogData<ProductLineData>(erpDataInfLog, null, apiResult, ex.Message, 1);

            }
            return apiResult;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public virtual void SaveProductLine(ProductLineData data, Item item, WipResource wipResource, Process process)
        {
            try
            {
                ProductLine productLine = new ProductLine();

                productLine.PersistenceStatus = PersistenceStatus.New;
                productLine.ItemId = item.Id;
                productLine.WipResourceId = wipResource.Id;
                if (process != null)
                    productLine.ProcessId = process.Id;

                RF.Save(productLine);
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
        public virtual void Valid(ProductLineData data, ref OuterSystemRetVO result)
        {
            if (data.ItemCode.IsNullOrEmpty())
            {
                var msg = "产品编码不能为空";
                result.success = false;
                result.errorMsg = result.errorMsg.IsNullOrEmpty() ? msg : result.errorMsg + ";" + msg;
                result.errorList.Add(data);
                return;
            }
            if (data.WipResourceCode.IsNullOrEmpty())
            {
                var msg = "产线/机台编码不能为空";
                result.success = false;
                result.errorMsg = result.errorMsg.IsNullOrEmpty() ? msg : result.errorMsg + ";" + msg;
                result.errorList.Add(data);
                return;
            }
        }


    }
}
