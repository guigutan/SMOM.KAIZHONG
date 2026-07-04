using Newtonsoft.Json;
using SIE.Core.Barcodes;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.Items.ProductFamilys;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Datas;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.KZ.Base.SmomControl;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Smom.Download.KaiZhong
{
    public class DownloadProcessController : DomainController
    {

        /// 从API下载数据到业务表
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public virtual ApiCommonRes SaveProcesss(List<ProcessData> datas)
        {
            //返回ERP结果
            ApiCommonRes apiResult = new ApiCommonRes() { DataCount = datas.Count };
            List<ProcessData> list = new List<ProcessData>();
            var dataJson = JsonConvert.SerializeObject(datas);
            var logController = AppRuntime.Service.Resolve<InfDataLogController>();
            int failCount = 0;


            //记录日志
            var erpDataInfLog = logController.SaveErpDataInfLog(InfType.Process, dataJson, DateTime.Now, CallDirection.NcToMom, CallResult.UnSave, datas.Count);
            try
            {
                if (datas != null || datas.Count > 0)
                {
                    var codes = datas.Select(p => p.VLSCH).Distinct().ToList();
                    var process = RT.Service.Resolve<ProcessController>().GetProcessesList(codes);

                    ProductFamily productFamily = RT.Service.Resolve<ProductFamilyController>().GetProductFamilyFirst();
                    if (productFamily == null)
                        throw new ValidationException("请维护一个产品族信息!".L10N());

                    foreach (var item in datas)
                    {
                        try
                        {
                            var pro = process.FirstOrDefault(p => p.Code == item.VLSCH);
                            pro = CreateProcess(pro, item, productFamily);

                            if (process.All(p => p.Id != pro.Id))
                                process.Add(pro);

                            list.Add(item);
                            apiResult.SuccessList.Add(item);
                        }
                        catch (Exception ex)
                        {
                            throw new ValidationException($"工序编码{item.VLSCH}:" + ex.GetBaseException()?.Message);
                        }

                    }

                    apiResult.SuccessCount = list.Count;
                    apiResult.FailCount = failCount;
                    logController.UpadateLogData<ProcessData>(erpDataInfLog, list, apiResult);

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
                logController.UpadateLogData<ProcessData>(erpDataInfLog, null, apiResult, ex.Message, 1);

            }
            return apiResult;
        }

        /// 从API下载数据到业务表
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public virtual ApiCommonRes SaveProcesss(List<ProcessData> datas, ref InfNcDataLogGroup erpDataInfLog)
        {
            //返回ERP结果
            ApiCommonRes apiResult = new ApiCommonRes() { DataCount = datas.Count };

            List<ProcessData> list = new List<ProcessData>();
            var dataJson = JsonConvert.SerializeObject(datas);
            int failCount = 0;
            var codes = datas.Select(p => p.VLSCH).Distinct().ToList();
            var process = RT.Service.Resolve<ProcessController>().GetProcessesList(codes);

            ProductFamily productFamily = RT.Service.Resolve<ProductFamilyController>().GetProductFamilyFirst();
            if (productFamily == null)
                throw new ValidationException("请维护一个产品族信息!".L10N());
            try
            {
                if (datas != null || datas.Count > 0)
                {

                    foreach (var item in datas)
                    {
                        try
                        {
                            if (item != null)
                            {
                                var pro = process.FirstOrDefault(p => p.Code == item.VLSCH);
                                pro = CreateProcess(pro, item, productFamily);

                                if (process.All(p => p.Id != pro.Id))
                                    process.Add(pro);

                                list.Add(item);
                                apiResult.SuccessList.Add(item);
                            }
                        }
                        catch (Exception ex)
                        {
                            apiResult.ErrorList.Add($"工序编码{item.VLSCH}:" + ex.GetBaseException()?.Message);
                            failCount++;
                            continue;
                        }

                    }

                    apiResult.SuccessCount = list.Count;
                    apiResult.FailCount = failCount;
                }
                else
                {
                    apiResult.ErrorList.Add("同步数据不能未空!".L10N());
                }
            }
            catch (Exception ex)
            {
                apiResult.ErrorList.Clear();
                apiResult.FailCount = datas.Count;
                apiResult.ErrorList.Add(ex.Message);
            }
            finally
            {
                erpDataInfLog = RT.Service.Resolve<InfNcDataLogGroupController>().UpdateInfNcDataLogGroupData<ProcessData>(erpDataInfLog, datas.Count, list, apiResult, false);
            }
            return apiResult;

        }


        private Process CreateProcess(Process pro, ProcessData item, ProductFamily productFamily)
        {
            if (pro == null)
            {
                pro = new Process();
                pro.Code = item.VLSCH;
                pro.ReferenceTimes = 0;
                pro.Type = ProcessType.BatchAssembly;
                pro.ProductFamilyId = productFamily.Id;
                pro.IsOutsourcing = false;
                //创建工序参数
                pro.ParameterList.Add(new ProcessParameter
                {
                    Type = ResultTypeForDesign.Pass,
                    Description = ResultTypeForDesign.Pass.ToLabel(),
                    ProcessId = pro.Id
                });
                //创建采集步骤
                pro.CollectStepList.Add(new ProcessCollectStep()
                {
                    BarcodeType = BarcodeType.BatchBarocde,
                    Step = 1,
                    PlugType = PlugType.In
                });
                pro.CollectStepList.Add(new ProcessCollectStep()
                {
                    BarcodeType = BarcodeType.BatchBarocde,
                    Step = 2,
                    PlugType = PlugType.Out
                });
                pro.PersistenceStatus = PersistenceStatus.New;
            }
            else
            {
                pro.PersistenceStatus = PersistenceStatus.Modified;
            }
            pro.Name = item.TXT;

            RF.Save(pro);
            return pro;
        }

    }
}
