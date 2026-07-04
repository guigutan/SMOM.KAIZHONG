using Newtonsoft.Json;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Datas;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.KZ.Base.SmomControl;
using SIE.Resources.WorkCenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Smom.Download.KaiZhong
{
    public class DownloadWorkCenterController : DomainController
    {

        /// <summary>
        /// 从API下载数据到业务表
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="erpDataInfLog">日志</param>
        /// <returns></returns>
        public virtual ApiCommonRes SaveWorkCenters(List<WorkCenterData> datas, string str)
        {
            //返回ERP结果
            ApiCommonRes apiResult = new ApiCommonRes() { DataCount = datas.Count };
            List<WorkCenterData> list = new List<WorkCenterData>();
            var dataJson = JsonConvert.SerializeObject(datas);
            var logController = AppRuntime.Service.Resolve<InfDataLogController>();
            int failCount = 0;

            //记录日志
            var erpDataInfLog = logController.SaveErpDataInfLog(InfType.WorkCenter, dataJson, DateTime.Now, CallDirection.NcToMom, CallResult.UnSave, datas.Count);
            try
            {
                if (datas != null || datas.Count > 0)
                {
                    var codes = datas.Select(p => p.ARBPL).Distinct().ToList();
                    var workCenters = RT.Service.Resolve<WorkCenterController>().GetWorkCentersByCode(codes);

                    foreach (var item in datas)
                    {
                        try
                        {
                            WorkCenter workCenter = null;
                            if (str == "IsDistribute")
                            {
                                //按照编码+负责人去找
                                workCenter = workCenters.FirstOrDefault(p => p.Code == item.ARBPL && p.Person == item.VERAN);
                                //假如编码+负责人去找，找不到 ，就要判断是不是已经存在相同编码的，如果存在，就直接跳过，不参与更新，只有编码+负责人找的到的，或者连编码都找不到的，才能更新和创建
                                if (workCenter == null && workCenters.Any(p => p.Code == item.ARBPL))
                                    continue;
                            }
                            else
                            {
                                //按照编码
                                workCenter = workCenters.FirstOrDefault(p => p.Code == item.ARBPL);
                            }

                            if (workCenter == null)
                            {
                                workCenter = new WorkCenter();
                                workCenter.Code = item.ARBPL;
                                workCenter.State = State.Enable;
                                workCenter.PersistenceStatus = PersistenceStatus.New;
                            }

                            workCenter.Name = item.KTEXT;
                            if (item.LVORM == "X")
                                workCenter.State = State.Disable;
                            workCenter.Category = item.VERWE;
                            workCenter.Person = item.VERAN;

                            RF.Save(workCenter);

                            if (workCenters.All(p => p.Id != workCenter.Id))
                                workCenters.Add(workCenter);

                            list.Add(item);
                            apiResult.SuccessList.Add(item);
                        }
                        catch (Exception ex)
                        {
                            throw new ValidationException($"工作中心编码{item.ARBPL}:" + ex.GetBaseException()?.Message);
                        }

                    }

                    apiResult.SuccessCount = list.Count;
                    apiResult.FailCount = failCount;
                    logController.UpadateLogData<WorkCenterData>(erpDataInfLog, list, apiResult);

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
                logController.UpadateLogData<WorkCenterData>(erpDataInfLog, null, apiResult, ex.Message, 1);

            }
            return apiResult;

        }

        /// <summary>
        /// 从API下载数据到业务表
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="erpDataInfLog">日志</param>
        /// <returns></returns>
        public virtual ApiCommonRes SaveWorkCenters(List<WorkCenterData> datas, ref InfNcDataLogGroup erpDataInfLog)
        {
            //返回ERP结果
            ApiCommonRes apiResult = new ApiCommonRes() { DataCount = datas.Count };

            List<WorkCenterData> list = new List<WorkCenterData>();
            var dataJson = JsonConvert.SerializeObject(datas);
            int failCount = 0;
            var codes = datas.Select(p => p.ARBPL).Distinct().ToList();
            var workCenters = RT.Service.Resolve<WorkCenterController>().GetWorkCentersByCode(codes);

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

                                var workCenter = workCenters.FirstOrDefault(p => p.Code == item.ARBPL && p.Person == item.VERAN);
                                //假如编码+负责人去找，找不到 ，就要判断是不是已经存在相同编码的，如果存在，就直接跳过，不参与更新，只有编码+负责人找的到的，或者连编码都找不到的，才能更新和创建
                                if (workCenter == null && workCenters.Any(p => p.Code == item.ARBPL))
                                    continue;

                                if (workCenter == null)
                                {
                                    workCenter = new WorkCenter();
                                    workCenter.Code = item.ARBPL;
                                    workCenter.State = State.Enable;
                                    workCenter.PersistenceStatus = PersistenceStatus.New;
                                }

                                workCenter.Name = item.KTEXT;
                                if (item.LVORM == "X")
                                    workCenter.State = State.Disable;
                                workCenter.Category = item.VERWE;
                                workCenter.Person = item.VERAN;

                                RF.Save(workCenter);

                                if (workCenters.All(p => p.Id != workCenter.Id))
                                    workCenters.Add(workCenter);

                                list.Add(item);
                                apiResult.SuccessList.Add(item);
                            }
                        }
                        catch (Exception ex)
                        {
                            apiResult.ErrorList.Add($"工作中心编码{item.ARBPL}:" + ex.GetBaseException()?.Message);
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
                erpDataInfLog = RT.Service.Resolve<InfNcDataLogGroupController>().UpdateInfNcDataLogGroupData<WorkCenterData>(erpDataInfLog, datas.Count, list, apiResult, false);
            }
            return apiResult;

        }
    }
}
