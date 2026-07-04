using Newtonsoft.Json;
using SIE.Items.ProductFamilys;
using SIE.Items;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Datas;
using SIE.KZ.Base.SmomControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIE.Domain.Validation;
using SIE.Equipments.EquipAccounts;
using EquipAccountData = SIE.KZ.Base.Interfaces.Datas.EquipAccountData;
using SIE.Domain;
using SIE.Equipments.EquipModels;
using SIE.Core.Equipments;
using EquipModel = SIE.Equipments.EquipModels.EquipModel;
using SIE.Core.Enums;
using EquipAccount = SIE.Equipments.EquipAccounts.EquipAccount;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using SIE.Resources.Enterprises;
using SIE.Resources.WorkCenters;
using SIE.KZ.Base.Interfaces.Enums;
using DocumentFormat.OpenXml.InkML;

namespace SIE.ERPInterface.Smom.Download.KaiZhong
{
    public class DownloadEquipAccountController : DomainController
    {

        /// 从API下载数据到业务表
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public virtual ApiCommonRes SaveEquipAccount(List<EquipAccountData> datas)
        {
            //返回ERP结果
            ApiCommonRes apiResult = new ApiCommonRes() { DataCount = datas.Count };
            List<EquipAccountData> list = new List<EquipAccountData>();
            var dataJson = JsonConvert.SerializeObject(datas);
            var logController = AppRuntime.Service.Resolve<InfDataLogController>();
            int failCount = 0;

            //记录日志
            var erpDataInfLog = logController.SaveErpDataInfLog(InfType.EquipAccount, dataJson, DateTime.Now, CallDirection.NcToMom, CallResult.UnSave, datas.Count);
            try
            {
                if (datas != null || datas.Count > 0)
                {
                    var codes = datas.Select(p => p.EQUNR).Distinct().ToList();
                    var modelCodes = datas.Select(p => (p.TYPBZ.IsNullOrEmpty() ? (p.EQTYP == "M" ? "NULL" : "null") : p.TYPBZ)).Distinct().ToList();
                    //获取设备台账
                    var equipAccounts = RT.Service.Resolve<EquipAccountController>().GetEquipAccountsByCode(codes);
                    //获取设备型号
                    var equipModels = RT.Service.Resolve<EquipAccountController>().GetEquipModelsByCodes(modelCodes);

                    //获取车间
                    var WorkShopCodes = datas.Select(p => p.INGRP).Distinct().ToList();
                    var workShops = RT.Service.Resolve<EnterpriseController>().GetWorkShopByCodes(WorkShopCodes);

                    //获取部门
                    var departmentCodes = datas.Select(p => p.GEWRK).Distinct().ToList();
                    var departments = RT.Service.Resolve<EnterpriseController>().GetDepartmentByCode(departmentCodes);

                    //管理部门
                    var controlloerDepartmentCodes = datas.Select(p => p.ZUSER_ID).Distinct().ToList();
                    var controlloerDepartments = RT.Service.Resolve<EnterpriseController>().GetDepartmentByCode(controlloerDepartmentCodes);

                    //获取工厂
                    var factoryCodes = datas.Select(p => p.SWERK).Distinct().ToList();
                    var factorys = RT.Service.Resolve<EnterpriseController>().GetFactoryByCode(factoryCodes);

                    //工作中心
                    var workCenterCodes = datas.Select(p => p.ARBPL).Distinct().ToList();
                    var workCenters = RT.Service.Resolve<WorkCenterController>().GetWorkCentersByCode(workCenterCodes);

                    foreach (var item in datas)
                    {
                        try
                        {
                            var equipAccount = equipAccounts.FirstOrDefault(p => p.Code == item.EQUNR);
                            var equipModel = equipModels.FirstOrDefault(p => p.Code == (item.TYPBZ.IsNullOrEmpty() ? (item.EQTYP == "M" ? "NULL" : "null") : item.TYPBZ));
                            var workShop = workShops.FirstOrDefault(p => p.Code == item.INGRP);
                            var department = departments.FirstOrDefault(p => p.Code == item.GEWRK);
                            var controlloerDepartment = controlloerDepartments.FirstOrDefault(p => p.Code == item.ZUSER_ID);
                            var workCenter = workCenters.FirstOrDefault(p => p.Code == item.ARBPL);
                            var factory = factorys.FirstOrDefault(p => p.Code == item.SWERK);
                            if (factory == null)
                                throw new ValidationException("企业模型中,编码:[{0}]工厂不存在!".L10nFormat(item.SWERK));
                            var tup = CreateEquipAccount(equipAccount, item, equipModel, workShop, department, controlloerDepartment, workCenter, factory);

                            equipAccount = tup.Item1;
                            if (equipAccounts.All(p => p.Id != equipAccount.Id))
                                equipAccounts.Add(equipAccount);
                            if (equipModels.Any(p => p.Id == tup.Item2.Id))
                            {
                            }
                            else
                            {
                                equipModels.Add(tup.Item2);
                            }
                            list.Add(item);
                            apiResult.SuccessList.Add(item);
                        }
                        catch (Exception ex)
                        {
                            throw new ValidationException($"编码{item.EQUNR}:" + ex.GetBaseException()?.Message);
                        }

                    }

                    apiResult.SuccessCount = list.Count;
                    apiResult.FailCount = failCount;
                    logController.UpadateLogData<EquipAccountData>(erpDataInfLog, list, apiResult);

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
                logController.UpadateLogData<EquipAccountData>(erpDataInfLog, null, apiResult, ex.Message, 1);

            }
            return apiResult;
        }

        /// 从API下载数据到业务表
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public virtual ApiCommonRes SaveEquipAccount(List<EquipAccountData> datas, ref InfNcDataLogGroup erpDataInfLog)
        {
            //返回ERP结果
            ApiCommonRes apiResult = new ApiCommonRes() { DataCount = datas.Count };

            List<EquipAccountData> list = new List<EquipAccountData>();
            var dataJson = JsonConvert.SerializeObject(datas);
            int failCount = 0;
            var codes = datas.Select(p => p.EQUNR).Distinct().ToList();
            var modelCodes = datas.Select(p => (p.TYPBZ.IsNullOrEmpty() ? (p.EQTYP == "M" ? "NULL" : "null") : p.TYPBZ)).Distinct().ToList();
            //获取设备台账
            var equipAccounts = RT.Service.Resolve<EquipAccountController>().GetEquipAccountsByCode(codes);
            //获取设备型号
            var equipModels = RT.Service.Resolve<EquipAccountController>().GetEquipModelsByCodes(modelCodes);

            //获取车间
            var WorkShopCodes = datas.Select(p => p.INGRP).Distinct().ToList();
            var workShops = RT.Service.Resolve<EnterpriseController>().GetWorkShopByCodes(WorkShopCodes);

            //获取部门
            var departmentCodes = datas.Select(p => p.GEWRK).Distinct().ToList();
            var departments = RT.Service.Resolve<EnterpriseController>().GetDepartmentByCode(departmentCodes);

            //管理部门
            var controlloerDepartmentCodes = datas.Select(p => p.ZUSER_ID).Distinct().ToList();
            var controlloerDepartments = RT.Service.Resolve<EnterpriseController>().GetDepartmentByCode(controlloerDepartmentCodes);

            //获取工厂
            var factoryCodes = datas.Select(p => p.SWERK).Distinct().ToList();
            var factorys = RT.Service.Resolve<EnterpriseController>().GetFactoryByCode(factoryCodes);

            //工作中心
            var workCenterCodes = datas.Select(p => p.ARBPL).Distinct().ToList();
            var workCenters = RT.Service.Resolve<WorkCenterController>().GetWorkCentersByCode(workCenterCodes);

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
                                var equipAccount = equipAccounts.FirstOrDefault(p => p.Code == item.EQUNR);
                                var equipModel = equipModels.FirstOrDefault(p => p.Code == (item.TYPBZ.IsNullOrEmpty() ? (item.EQTYP == "M" ? "NULL" : "null") : item.TYPBZ));
                                var workShop = workShops.FirstOrDefault(p => p.Code == item.INGRP);
                                var department = departments.FirstOrDefault(p => p.Code == item.GEWRK);
                                var controlloerDepartment = controlloerDepartments.FirstOrDefault(p => p.Code == item.ZUSER_ID);
                                var workCenter = workCenters.FirstOrDefault(p => p.Code == item.ARBPL);
                                var factory = factorys.FirstOrDefault(p => p.Code == item.SWERK);
                                if (factory == null)
                                    throw new ValidationException("企业模型中编码:[{0}]工厂不存在!".L10nFormat(item.SWERK));
                                var tup = CreateEquipAccount(equipAccount, item, equipModel, workShop, department, controlloerDepartment, workCenter, factory);

                                equipAccount = tup.Item1;
                                if (equipAccounts.All(p => p.Id != equipAccount.Id))
                                    equipAccounts.Add(equipAccount);

                                if (equipModels.Any(p => p.Id == tup.Item2.Id))
                                {
                                }
                                else
                                {
                                    equipModels.Add(tup.Item2);
                                }

                                list.Add(item);
                                apiResult.SuccessList.Add(item);
                            }
                        }
                        catch (Exception ex)
                        {
                            apiResult.ErrorList.Add($"编码{item.EQUNR}:" + ex.GetBaseException()?.Message);
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
                erpDataInfLog = RT.Service.Resolve<InfNcDataLogGroupController>().UpdateInfNcDataLogGroupData<EquipAccountData>(erpDataInfLog, datas.Count, list, apiResult, false);
            }
            return apiResult;
        }


        private (EquipAccount, EquipModel) CreateEquipAccount(EquipAccount equipAccount, EquipAccountData item, EquipModel equipModel, Enterprise workShop, Enterprise department, Enterprise controlloerDepartment, WorkCenter workCenter, Enterprise factory)
        {
            if (equipAccount == null)
            {
                equipAccount = new EquipAccount();
                equipAccount.Code = item.EQUNR;
                equipAccount.State = AccountState.Running;
                equipAccount.PersistenceStatus = PersistenceStatus.New;
            }

            equipAccount.FactoryId = factory.Id;
            equipAccount.Name = item.SHTXT;
            //当设备种类为模具时，就是模具图号；当设备种类设备时，就是设备型号
            //当设备种类为设备时，就是设备系列号；当设备种类为模具时，就是模具穴位数；
            if (item.EQTYP == "P")
            {
                equipAccount.Drawn = item.TYPBZ;
                equipAccount.Acupoint = item.SERGE;
            }
            else
            {
                equipAccount.SerialNumber = item.SERGE;
            }

            var type = item.EQTYP == "M" ? "生产设备" : "模具设备";
            //当设备型号为空的时候，创建一个新的
            if (equipModel == null || (equipModel != null && equipModel.TypeCategory != type))
            {
                equipModel = CreateequipModel(equipModel, item.TYPBZ.IsNullOrEmpty() ? (item.EQTYP == "M" ? "NULL" : "null") : item.TYPBZ, type);
            }
            equipAccount.EquipModelId = equipModel.Id;

            if (DateTime.TryParseExact(item.ANSDT, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                equipAccount.PurchaseDate = result;
            }
            if (workShop != null)
                equipAccount.WorkShopId = workShop.Id;
            if (department != null)
                equipAccount.UseDepartmentId = department.Id;
            equipAccount.Manufacturer = item.HERST;
            if (controlloerDepartment != null)
                equipAccount.ManageDepartmentId = controlloerDepartment.Id;

            equipAccount.CostCenterCode = item.KOSTL;
            if (workCenter != null)
                equipAccount.WorkCenterId = workCenter.Id;
            equipAccount.FunctionalLocation = item.TPLNR;
            if (item.STTXT == "E0001")
                equipAccount.UseState = AccountUseState.ToAccepted;
            else if (item.STTXT == "E0002")
                equipAccount.UseState = AccountUseState.Using;
            else if (item.STTXT == "E0003")
                equipAccount.UseState = AccountUseState.InIdle;
            else if (item.STTXT == "E0004" || item.STTXT == "E0006")
                equipAccount.UseState = AccountUseState.DisposedOf;
            else
                equipAccount.UseState = AccountUseState.Scrap;

            RF.Save(equipAccount);
            return (equipAccount, equipModel);
        }

        /// <summary>
        /// 创建设备型号维护
        /// </summary>
        /// <param name="equipModel"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        private EquipModel CreateequipModel(EquipModel equipModel, string key, string type)
        {
            if (equipModel == null)
            {
                equipModel = new EquipModel();
                equipModel.PersistenceStatus = PersistenceStatus.New;
            }
            equipModel.Code = key;
            equipModel.Name = key;

            equipModel.TypeCategory = type;//type == "M" ? "生产设备" : "模具设备";
            equipModel.IndustryCategory = Core.Enums.IndustryCategory.GeneralEquipment;

            EquipType equipType = Query<EquipType>().Where(p => p.TypeName == equipModel.TypeCategory).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());

            if (equipType == null)
            {
                equipType = new EquipType();
                equipType.PersistenceStatus = PersistenceStatus.New;
                equipType.TypeCode = equipModel.TypeCategory;
                equipType.TypeName = equipModel.TypeCategory;
                equipType.Num = 1;
                RF.Save(equipType);
            }
            else if(equipType.TypeCode != equipModel.TypeCategory)
            {
                equipType.TypeCode = equipModel.TypeCategory;
                equipType.TypeName = equipModel.TypeCategory;
                RF.Save(equipType);
            }

            //if (equipType == null)
            //    throw new ValidationException("请在[设备类型维护]中，维护类型名称为[{0}]的数据!".L10nFormat(equipModel.TypeCategory));


            equipModel.EquipTypeId = equipType.Id;
            RF.Save(equipModel);

            return equipModel;
        }

    }
}
