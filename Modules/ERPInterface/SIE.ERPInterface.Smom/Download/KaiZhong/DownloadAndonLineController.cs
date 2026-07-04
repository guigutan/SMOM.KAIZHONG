using Newtonsoft.Json;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.EquipAccounts;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Datas;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.MES.Andon;
using SIE.MES.LineAndon;
using SIE.Resources.Enterprises;
using SIE.Resources.WorkCenters;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Smom.Download.KaiZhong
{
    public class DownloadAndonLineController : DomainController
    {

        /// <summary>
        /// 保存分类到业务表
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public virtual ApiCommonRes SaveAndonLineFactory(List<AndonLineData> datas)
        {
            //返回ERP结果
            ApiCommonRes apiResult = new ApiCommonRes() { DataCount = datas.Count };
            List<AndonLineData> list = new List<AndonLineData>();
            var dataJson = JsonConvert.SerializeObject(datas);
            var logController = AppRuntime.Service.Resolve<InfDataLogController>();
            int failCount = 0;

            //设备字典
            Dictionary<string, EquipAccount> dicEquipAccount = new Dictionary<string, EquipAccount>();
            //工厂字典
            Dictionary<string, Enterprise> dicFactory = new Dictionary<string, Enterprise>();
            //车间字典
            Dictionary<string, Enterprise> dicWorkShop = new Dictionary<string, Enterprise>();

            //记录日志
            var erpDataInfLog = logController.SaveErpDataInfLog(InfType.AndonLine, dataJson, DateTime.Now, CallDirection.NcToMom, CallResult.UnSave, datas.Count);
            try
            {
                if (datas != null || datas.Count > 0)
                {

                    //获取工作中心
                    var wcCodes = datas.Select(p => p.WorkCenterCode).Distinct().ToList();
                    var wcList = RT.Service.Resolve<WorkCenterController>().GetWorkCentersByCode(wcCodes);

                    //获取安灯区域
                    var andonCodes = datas.Select(p => p.AndonCode).Distinct().ToList();
                    var andonUpholds = RT.Service.Resolve<AndonUpholdController>().GetAndonUpholds(andonCodes);

                    foreach (var data in datas)
                    {
                        try
                        {
                            OuterSystemRetVO result = new OuterSystemRetVO();
                            Valid(data, ref result);
                            if (!result.errorMsg.IsNullOrEmpty())
                                throw new ValidationException(result.errorMsg);

                            EquipAccount equipAccount = null;
                            if (!data.EquipmentNo.IsNullOrEmpty())
                            {
                                if (dicEquipAccount.ContainsKey(data.EquipmentNo))
                                {
                                    equipAccount = dicEquipAccount[data.EquipmentNo];
                                }
                                else
                                {
                                    //获取设备
                                    equipAccount = RT.Service.Resolve<AndonLineController>().GetEquipAccounts(data.EquipmentNo, null).Where(p => p.Code == data.EquipmentNo).FirstOrDefault();
                                    if (equipAccount == null)
                                        throw new ValidationException("设备{0}不存在".L10nFormat(data.EquipmentNo));
                                    else
                                        dicEquipAccount.Add(equipAccount.Code, equipAccount);
                                }
                            }

                            var wc = wcList.FirstOrDefault(p => p.Code == data.WorkCenterCode);
                            if (wc == null)
                                throw new ValidationException("工作中心{0}不存在".L10nFormat(data.WorkCenterCode));

                            //获取工厂
                            Enterprise factory = null;
                            if (dicFactory.ContainsKey(data.FactoryCode))
                            {
                                factory = dicFactory[data.FactoryCode];
                            }
                            else
                            {
                                factory = RT.Service.Resolve<EnterpriseController>().GetFactoriesList(null, data.FactoryCode).Where(p => p.Code == data.FactoryCode).FirstOrDefault();
                                if (factory == null)
                                    throw new ValidationException("工厂{0}不存在".L10nFormat(data.FactoryCode));
                                dicFactory.Add(factory.Code, factory);
                            }

                            //获取车间
                            Enterprise workShop = null;
                            if (dicWorkShop.ContainsKey(data.WorkShopCode))
                            {
                                workShop = dicWorkShop[data.WorkShopCode];
                            }
                            else
                            {
                                workShop = RT.Service.Resolve<EnterpriseController>().GetWorkShops(null, data.WorkShopCode, null).Where(p => p.Code == data.WorkShopCode).FirstOrDefault();
                                if (workShop == null)
                                    throw new ValidationException("车间{0}不存在".L10nFormat(data.FactoryCode));
                                dicWorkShop.Add(workShop.Code, workShop);
                            }
                            AndonUphold andonUphold = null;
                            if (!data.AndonCode.IsNullOrEmpty())
                            {
                                andonUphold = andonUpholds.FirstOrDefault(p => p.AndonCode == data.AndonCode);
                                if (andonUphold == null)
                                    throw new ValidationException("安灯编码{0}不存在".L10nFormat(data.AndonCode));
                            }

                            SaveAndonLine(data, equipAccount, wc, factory, workShop, andonUphold);
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
                    logController.UpadateLogData<AndonLineData>(erpDataInfLog, list, apiResult);
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
                logController.UpadateLogData<AndonLineData>(erpDataInfLog, null, apiResult, ex.Message, 1);

            }
            return apiResult;

        }

        /// <summary>
        /// 保存
        /// </summary>
        public virtual void SaveAndonLine(AndonLineData data, EquipAccount equipAccount, WorkCenter wc, Enterprise factory, Enterprise workShop, AndonUphold andonUphold)
        {
            //该接口只新增，如果出现重复的直接跳过
            try
            {
                LineArea andonLine = new LineArea();

                andonLine.PersistenceStatus = PersistenceStatus.New;
                andonLine.MachineCode = data.MachineCode;
                andonLine.MachineName = data.MachineName;
                if (equipAccount != null)
                    andonLine.EquipmentId = equipAccount.Id;
                andonLine.WorkCenterId = wc.Id;
                andonLine.FactoryId = factory.Id;
                andonLine.WorkShopId = workShop.Id;
                if (andonUphold != null)
                {
                    andonLine.AndonUpholdId = andonUphold.Id;
                    andonLine.AndonCode = andonUphold.AndonCode;
                }

                RF.Save(andonLine);
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
        public virtual void Valid(AndonLineData data, ref OuterSystemRetVO result)
        {
            if (data.MachineCode.IsNullOrEmpty())
            {
                var msg = "产线/机台编码不能为空";
                result.success = false;
                result.errorMsg = result.errorMsg.IsNullOrEmpty() ? msg : result.errorMsg + ";" + msg;
                result.errorList.Add(data);
                return;
            }
            if (data.MachineName.IsNullOrEmpty())
            {
                var msg = "产线/机台名称不能为空";
                result.success = false;
                result.errorMsg = result.errorMsg.IsNullOrEmpty() ? msg : result.errorMsg + ";" + msg;
                result.errorList.Add(data);
                return;
            }
            if (data.WorkCenterCode.IsNullOrEmpty())
            {
                var msg = "工作中心不能为空";
                result.success = false;
                result.errorMsg = result.errorMsg.IsNullOrEmpty() ? msg : result.errorMsg + ";" + msg;
                result.errorList.Add(data);
                return;
            }
            if (data.FactoryCode.IsNullOrEmpty())
            {
                var msg = "工厂编码不能为空";
                result.success = false;
                result.errorMsg = result.errorMsg.IsNullOrEmpty() ? msg : result.errorMsg + ";" + msg;
                result.errorList.Add(data);
                return;
            }
            if (data.WorkShopCode.IsNullOrEmpty())
            {
                var msg = "车间编码不能为空";
                result.success = false;
                result.errorMsg = result.errorMsg.IsNullOrEmpty() ? msg : result.errorMsg + ";" + msg;
                result.errorList.Add(data);
                return;
            }
        }
    }
}
