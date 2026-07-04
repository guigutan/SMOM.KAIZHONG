using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using RazorEngine.Templating;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Datas;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.KZ.Base.SmomControl;
using SIE.LES;
using SIE.Packages.ItemLabels;
using SIE.Resources.Enterprises;
using SIE.Warehouses.ItemStockData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Smom.Download.KaiZhong
{
    public class DownloadEnterpriseController : DomainController
    {
        public virtual ApiCommonRes GroupSaveEnterprise(List<EnterpriseData> datas)
        {
            //返回ERP结果
            ApiCommonRes apiResult = new ApiCommonRes() { DataCount = datas.Count };
            List<EnterpriseData> list = new List<EnterpriseData>();
            var dataJson = JsonConvert.SerializeObject(datas);
            var logController = AppRuntime.Service.Resolve<InfDataLogController>();
            int failCount = 0;

            //记录日志
            var erpDataInfLog = logController.SaveErpDataInfLog(InfType.Enterprise, dataJson, DateTime.Now, CallDirection.NcToMom, CallResult.UnSave, datas.Count);
            try
            {
                if (datas != null || datas.Count > 0)
                {
                    //公司
                    var companys = datas.Select(p => p.BUKRS).Distinct().ToList();
                    //工厂
                    var factorys = datas.Select(p => p.WERKS).Distinct().ToList();
                    //车间
                    var workShops = datas.Select(p => p.DISPO).Distinct().ToList();


                    var str = new List<string>();
                    str.AddRange(companys);
                    str.AddRange(factorys);
                    str.AddRange(workShops);
                    var enterprises = RT.Service.Resolve<EnterpriseController>().GetEnterprises(str);
                    //获取企业层级
                    var levels = RF.GetAll<EnterpriseLevel>();

                    foreach (var item in datas)
                    {
                        try
                        {
                            CreateEnterprise(ref enterprises, item, ref levels);

                            list.Add(item);
                            apiResult.SuccessList.Add(item);
                        }
                        catch (Exception ex)
                        {
                            throw new ValidationException($"公司编码:{item.BUKRS},工厂:{item.WERKS},MRP控制员(车间编码):{item.DISPO}:" + ex.GetBaseException()?.Message);
                        }
                    }

                    apiResult.SuccessCount = list.Count;
                    apiResult.FailCount = failCount;
                    logController.UpadateLogData<EnterpriseData>(erpDataInfLog, list, apiResult);

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
                logController.UpadateLogData<EnterpriseData>(erpDataInfLog, null, apiResult, ex.Message, 1);

            }
            return apiResult;

        }


        public virtual ApiCommonRes GroupSaveEnterprise(List<EnterpriseData> datas, ref InfNcDataLogGroup erpDataInfLog)
        {
            //返回ERP结果
            ApiCommonRes apiResult = new ApiCommonRes() { DataCount = datas.Count };

            List<EnterpriseData> list = new List<EnterpriseData>();
            var dataJson = JsonConvert.SerializeObject(datas);
            int failCount = 0;

            try
            {
                if (datas != null || datas.Count > 0)
                {
                    //公司
                    var companys = datas.Select(p => p.BUKRS).Distinct().ToList();
                    //工厂
                    var factorys = datas.Select(p => p.WERKS).Distinct().ToList();
                    //车间
                    var workShops = datas.Select(p => p.DISPO).Distinct().ToList();


                    var str = new List<string>();
                    str.AddRange(companys);
                    str.AddRange(factorys);
                    str.AddRange(workShops);
                    str = str.Distinct().ToList();
                    var enterprises = RT.Service.Resolve<EnterpriseController>().GetEnterprises(str);
                    //获取企业层级
                    var levels = RF.GetAll<EnterpriseLevel>();

                    foreach (var item in datas)
                    {
                        try
                        {
                            if (item != null)
                            {
                                CreateEnterprise(ref enterprises, item, ref levels);

                                list.Add(item);
                                apiResult.SuccessList.Add(item);
                            }
                        }
                        catch (Exception ex)
                        {
                            apiResult.ErrorList.Add($"公司编码:{item.BUKRS},工厂:{item.WERKS},MRP控制员(车间编码):{item.DISPO}:" + ex.GetBaseException()?.Message);
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
                erpDataInfLog = RT.Service.Resolve<InfNcDataLogGroupController>().UpdateInfNcDataLogGroupData<EnterpriseData>(erpDataInfLog, datas.Count, list, apiResult, false);
            }
            return apiResult;

        }

        /// <summary>
        /// 创建数据
        /// </summary>
        /// <param name="enterprises"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public virtual void CreateEnterprise(ref EntityList<Enterprise> enterprises, EnterpriseData data, ref EntityList<EnterpriseLevel> levels)
        {
            if (data.BUKRS.IsNullOrEmpty())
                return;

            //判断公司是否为空
            var company = enterprises.FirstOrDefault(p => p.Code == data.BUKRS);
            if (company == null)
            {
                //获取公司企业层级
                var level = levels.FirstOrDefault(p => p.Type == EnterpriseType.Company);
                //如果没有公司层级就创建一个
                if (level == null)
                {
                    ////集团企业层级
                    //var groupLevel = levels.FirstOrDefault(p => p.Type == EnterpriseType.Group);
                    ////如果没有集团层级，那么就创建一个
                    //if (groupLevel == null)
                    //{
                    //    //创建集团企业层级
                    //    groupLevel = new EnterpriseLevel();
                    //    groupLevel.Code = "Group";
                    //    groupLevel.Name = "集团";
                    //    groupLevel.InvOrgId = RT.InvOrg;
                    //    groupLevel.IsResource = false;
                    //    groupLevel.IsByHand = YesNo.No;
                    //    groupLevel.Type = EnterpriseType.Group;
                    //    groupLevel.PersistenceStatus = PersistenceStatus.New;
                    //    RF.Save(groupLevel);
                    //    levels.Add(groupLevel);
                    //}
                    //创建公司企业层级
                    level = new EnterpriseLevel();
                    level.Code = "Company";
                    level.Name = "公司";
                    level.InvOrgId = RT.InvOrg;
                    level.IsResource = false;
                    level.IsByHand = YesNo.No;
                    level.Type = EnterpriseType.Company;
                    //level.TreePId = groupLevel.Id;
                    RF.Save(level);
                    levels.Add(level);
                }
                ////获取集团企业模型
                //var group = enterprises.FirstOrDefault(p => p.Level.Type == EnterpriseType.Group);
                ////判断集团是否存在，不存在就创建新的
                //if (group == null)
                //{
                //    group = new Enterprise();
                //    group.PersistenceStatus = PersistenceStatus.New;
                //    group.Code = "KAIZHONG";
                //    group.Name = "凯中集团";
                //    group.InvOrgId = RT.InvOrg;
                //    group.IsResource = false;
                //    group.IsByHand = YesNo.No;
                //    group.LevelId = levels.FirstOrDefault(p => p.Type == EnterpriseType.Group).Id;
                //    RF.Save(group);
                //    enterprises.Add(group);
                //}
                //创建公司企业模型
                company = new Enterprise();
                company.Code = data.BUKRS;
                company.InvOrgId = RT.InvOrg;
                company.IsResource = false;
                company.IsByHand = YesNo.No;
                company.LevelId = level.Id;
                company.Level = level;
                //company.TreePId = group.Id;
                enterprises.Add(company);
            }
            else
            {
                if (company.Level.Type != EnterpriseType.Company)
                {
                    throw new ValidationException("已存在层级为[{0}]的[{1}]，不允许修改该层级".L10nFormat(company.Level.Type.ToLabel(), company.Code));
                }
            }

            company.Name = data.BUTXT;
            if (company.PersistenceStatus != PersistenceStatus.Unchanged)
                RF.Save(company);

            if (data.WERKS.IsNullOrEmpty())
                return;
            var factory = enterprises.FirstOrDefault(p => p.Code == data.WERKS);
            if (factory == null)
            {
                //工厂层级
                var level = levels.FirstOrDefault(p => p.Type == EnterpriseType.Plant);
                //如果不存在工厂的企业层级，就创建新的
                if (level == null)
                {
                    level = new EnterpriseLevel();
                    level.Code = "Factory";
                    level.Name = "工厂";
                    level.InvOrgId = RT.InvOrg;
                    level.IsResource = false;
                    level.IsByHand = YesNo.No;
                    level.Type = EnterpriseType.Plant;
                    level.TreePId = levels.FirstOrDefault(p => p.Type == EnterpriseType.Company).Id;
                    RF.Save(level);
                    levels.Add(level);
                }
                //创建工厂企业模型
                factory = new Enterprise();
                factory.Code = data.WERKS;
                factory.InvOrgId = RT.InvOrg;
                factory.IsResource = false;
                factory.IsByHand = YesNo.No;
                factory.LevelId = level.Id;
                factory.Level = level;
                factory.TreePId = company.Id;
                enterprises.Add(factory);
            }
            else
            {
                if (factory.Level.Type != EnterpriseType.Plant)
                    throw new ValidationException("已存在层级为[{0}]的[{1}]，不允许修改该层级".L10nFormat(factory.Level.Type.ToLabel(), factory.Code));
                if (factory.TreePId != company.Id)
                {
                    throw new ValidationException("工厂[{0}]已存在其他公司层级下，无法修改!".L10nFormat(factory.Code));
                }
            }
            factory.Name = data.NAME1;
            if (factory.PersistenceStatus != PersistenceStatus.Unchanged)
                RF.Save(factory);

            if (data.DISPO.IsNullOrEmpty())
                return;

            var workShop = enterprises.FirstOrDefault(p => p.Code == data.DISPO);
            if (workShop == null)
            {
                //车间层级
                var level = levels.FirstOrDefault(p => p.Type == EnterpriseType.Shop);
                //如果不存在车间的企业层级，就创建新的
                if (level == null)
                {
                    level = new EnterpriseLevel();
                    level.Code = "Workshop";
                    level.Name = "车间";
                    level.InvOrgId = RT.InvOrg;
                    level.IsResource = false;
                    level.IsByHand = YesNo.No;
                    level.Type = EnterpriseType.Shop;
                    level.TreePId = levels.FirstOrDefault(p => p.Type == EnterpriseType.Plant).Id;
                    RF.Save(level);
                    levels.Add(level);
                }
                //创建车间企业模型
                workShop = new Enterprise();
                workShop.Code = data.DISPO;
                workShop.InvOrgId = RT.InvOrg;
                workShop.IsResource = false;
                workShop.IsByHand = YesNo.No;
                workShop.LevelId = level.Id;
                workShop.Level = level;
                enterprises.Add(workShop);
            }
            workShop.Name = data.DSNAM;
            workShop.TreePId = factory.Id;
            if (workShop.PersistenceStatus != PersistenceStatus.Unchanged)
                RF.Save(workShop);
        }


    }
}
