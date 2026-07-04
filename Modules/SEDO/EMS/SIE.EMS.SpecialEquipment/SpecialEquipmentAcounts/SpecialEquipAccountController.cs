using SIE.Common.Catalogs;
using SIE.Common.Configs;
using SIE.DataAuth;
using SIE.Domain;
using SIE.EMS.Equipments;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.SpecialEquipment.Models;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.SpecialEquipment.SpecialEquipmentAcounts
{
    /// <summary>
    /// 特种设备台账控制器
    /// </summary>
    public partial class SpecialEquipAccountController : DomainController
    {
        /// <summary>
        /// 查询特种设备台账列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<SpecialEquipmentAccount> GetSpecialEquipAccountCriteria(SpecialEquipmentAccountCriteria criteria)
        {
            List<Catalog> CatalogList = RT.Service.Resolve<CatalogController>().GetCatalogList(EquipType.EquipTypeCatalogType).ToList();
            var query = Query<SpecialEquipmentAccount>();
            //获取设备台账配置的特种设备查询信息
            var config = ConfigService.GetConfig(new EquipModelEquipmentCategoryConfig(), typeof(EquipModel));
            List<string> strarr = new List<string>();
            if (config != null && config.SpecialIds.IsNotEmpty())
            {
                strarr = config.SpecialIds.Split(',').ToList();
            }

            List<string> arraylist = CatalogList.Where(p => strarr.Contains(p.Id.ToString())).Select(p => p.Code).ToList();

            if (criteria.Code.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(criteria.Code));
            }
            if (criteria.Name.IsNotEmpty())
            {
                query.Where(p => p.Name.Contains(criteria.Name));
            }

            query.Exists<EquipModel>((x, y) => y
            .Where(p => p.Id == x.EquipModelId)
            .Where(e => arraylist.Contains(e.TypeCategory))
            .WhereIf(criteria.ModelCode.IsNotEmpty(), c => c.Code.Contains(criteria.ModelCode))
            .WhereIf(criteria.ModelName.IsNotEmpty(), c => c.Name.Contains(criteria.ModelName))
            .WhereIf(criteria.TypeCategory.IsNotEmpty(), e => e.TypeCategory == criteria.TypeCategory));

            if (criteria.NextInspectionDate.BeginValue.HasValue)
            {
                query.Where(p => p.NextInspectionDate >= criteria.NextInspectionDate.BeginValue);
            }
            if (criteria.NextInspectionDate.EndValue.HasValue)
            {
                query.Where(p => p.NextInspectionDate <= criteria.NextInspectionDate.EndValue);
            }

            if (criteria.RegularInspectionStatus.HasValue)
            {
                query.Where(p => p.RegularInspectionStatus == criteria.RegularInspectionStatus);
            }

            if (criteria.State.HasValue)
            {
                query.Where(p => p.State == criteria.State);
            }
            if (criteria.AccountUseState.HasValue)
            {
                query.Where(p => p.UseState == criteria.AccountUseState);
            }
            if (criteria.WorkShopId.HasValue)
            {
                query.Where(p => p.WorkShopId == criteria.WorkShopId);
            }
            if (criteria.ResourceId.HasValue && criteria.ResourceId != 0)
            {
                query.Where(p => p.ResourceId == criteria.ResourceId);
            }
            if (criteria.ProcessId.HasValue)
            {
                query.Where(p => p.ProcessId == criteria.ProcessId.Value);
            }
            if (criteria.CreateDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue);
            }
            if (criteria.CreateDate.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue);
            }
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 获取特种设备台账的所有数据
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<EquipAccountSelect> GetSpecialEquipAccountList(PagingInfo pagingInfo, string keyword)
        {
            List<Catalog> CatalogList = RT.Service.Resolve<CatalogController>().GetCatalogList(EquipType.EquipTypeCatalogType).ToList();

            var query = Query<EquipAccountSelect>();

            //获取设备台账配置的特种设备查询信息
            var config = ConfigService.GetConfig(new EquipModelEquipmentCategoryConfig(), typeof(EquipModel));

            List<string> strarr = new List<string>();

            if (config != null && config.SpecialIds.IsNotEmpty())
            {
                strarr = config.SpecialIds.Split(',').ToList();
            }

            List<string> arraylist = CatalogList.Where(p => strarr.Contains(p.Id.ToString())).Select(p => p.Code).ToList();

            if (arraylist == null || !arraylist.Any())
            {
                return new EntityList<EquipAccountSelect>();
            }

            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }

            query.Exists<EquipModel>((x, y) => y
                .Where(p => p.Id == x.EquipModelId)
                .Where(e => arraylist.Contains(e.TypeCategory)));

            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

        }

        /// <summary>
        /// 获取所有特种设备台账的所有数据
        /// </summary>        
        /// <returns></returns>
        public virtual EntityList<EquipAccountSelect> GetAllSpecialEquipAccountList()
        {
            List<Catalog> CatalogList = RT.Service.Resolve<CatalogController>().GetCatalogList(EquipType.EquipTypeCatalogType).ToList();

            var query = Query<EquipAccountSelect>();

            //获取设备台账配置的特种设备查询信息
            var config = ConfigService.GetConfig(new EquipModelEquipmentCategoryConfig(), typeof(EquipModel));

            List<string> strarr = new List<string>();

            if (config != null && config.SpecialIds.IsNotEmpty())
            {
                strarr = config.SpecialIds.Split(',').ToList();
            }

            List<string> arraylist = CatalogList.Where(p => strarr.Contains(p.Id.ToString())).Select(p => p.Code).ToList();

            if (arraylist == null || !arraylist.Any())
            {
                return new EntityList<EquipAccountSelect>();
            }

            query.Exists<EquipModel>((x, y) => y.Join<EquipType>((c, d) => c.EquipTypeId == d.Id)
                .Where(p => p.Id == x.EquipModelId)
                .Where(e => arraylist.Contains(e.TypeCategory)));

            using (DataAuths.LoadAll())

            {
                return query.ToList(eagerLoad: new EagerLoadOptions().LoadWithViewProperty());
            }
        }
        /// <summary>
        /// 保存选择的检验规程数据
        /// </summary>
        /// <param name="equipModelRegularInspectionInfos"></param>
        public virtual void SaveSelEquipAccountRegularInspection(List<EquipAccountRegularInspection> equipModelRegularInspectionInfos)
        {
            if (equipModelRegularInspectionInfos == null)
            {
                return;
            }
            EntityList<EquipAccountRegularInspection> savedData = new EntityList<EquipAccountRegularInspection>();
            foreach (var item in equipModelRegularInspectionInfos)
            {
                var equipAccountRegularInspection = new EquipAccountRegularInspection();
                equipAccountRegularInspection.SpecialEquipmentAccountId = item.SpecialEquipmentAccountId;
                equipAccountRegularInspection.InspectionRuleId = item.InspectionRuleId;
                equipAccountRegularInspection.NotSubmit = true;
                equipAccountRegularInspection.PeriodDays = item.InspectionRule.PeriodDays;
                equipAccountRegularInspection.WarningPeriod = item.InspectionRule.WarningPeriod;
                savedData.Add(equipAccountRegularInspection);
            }
            RF.Save(savedData);
        }

        /// <summary>
        /// 根据设备台账Id列表更新点检保养项目、单元组成和单元组成物料清单
        /// </summary>
        /// <param name="accountIds">设备台账Id列表</param>
        public virtual string SynSpecialModelDatas(List<double> accountIds)
        {
            string errMsg = string.Empty;
            try
            {
                EntityList<EquipAccountCheckProject> checkProjectList = new EntityList<EquipAccountCheckProject>();

                EntityList<EquipAccountMaintainProject> maintainProjectList = new EntityList<EquipAccountMaintainProject>();

                EntityList<EquipAccountLubricationProject> lubricationProjectList = new EntityList<EquipAccountLubricationProject>();

                EntityList<EquipAccountRegularInspection> regularInspectionList = new EntityList<EquipAccountRegularInspection>();

                var equipCt = RT.Service.Resolve<EquipController>();

                #region 加载初始数据
                //获取设备台账列表
                var equipAccounts = equipCt.GetEquipAccountsByIds(accountIds);
                var modelIds = equipAccounts.Select(p => p.EquipModelId).Distinct().ToList();

                //获取设备型号对应的点检保养项目列表
                var checkProjectsOfModels = equipCt.GetCheckProjectsOfModels(modelIds);
                var dicCheckPrjsOfModels = checkProjectsOfModels.GroupBy(p => p.EquipModelId).ToDictionary(p => p.Key, p => p.ToList());

                var maintainProjectsOfModels = equipCt.GetMaintainProjectsOfModels(modelIds);
                var dicMaintainPrjsOfModels = maintainProjectsOfModels.GroupBy(p => p.EquipModelId).ToDictionary(p => p.Key, p => p.ToList());

                var lubricationProjectOfModels = equipCt.GetEquipModelLubricationProjects(modelIds);
                var dicLubricationPrjsOfModels = lubricationProjectOfModels.GroupBy(p => p.EquipModelId).ToDictionary(p => p.Key, p => p.ToList());

                //设备型号扩展的数据 设备型号id,检验规程id
                var regularInspectionOfModels = GetEquipModelRegularInspection(modelIds);
                var dicRegularInspectionOfModels = regularInspectionOfModels.GroupBy(p => p.EquipModelId).ToDictionary(p => p.Key, p => p.ToList());

                //获取设备台账对应的点检保养项目列表           
                var checkProjectsOfAccounts = equipCt.GetCheckProjectsOfAccounts(accountIds);
                var dicCheckPrjsOfAccounts = checkProjectsOfAccounts.GroupBy(p => p.EquipAccountId).ToDictionary(p => p.Key, p => p.ToList());

                var maintainProjectsOfAccounts = equipCt.GetMaintainProjectsOfAccounts(accountIds);
                var dicMaintainPrjsOfAccounts = maintainProjectsOfAccounts.GroupBy(p => p.EquipAccountId).ToDictionary(p => p.Key, p => p.ToList());

                var lubricationsOfAccounts = equipCt.GetLubricationProjectOfAccounts(accountIds);
                var dicLubricationsOfAccounts = lubricationsOfAccounts.GroupBy(p => p.EquipAccountId).ToDictionary(p => p.Key, p => p.ToList());

                //特种设备台账数据  特种设备台账id,检验规程id。
                var regularInspectionOfAccounts = GetRegularInspectionOfAccounts(accountIds);
                var dicRegularInspectionOfAccounts = regularInspectionOfAccounts.GroupBy(p => p.SpecialEquipmentAccountId).ToDictionary(p => p.Key, p => p.ToList());
                //获取设备台账对应的单元组成和单元组成物料列表                
                #endregion

                foreach (var equipAccount in equipAccounts)
                {
                    //更新点检项目列表                   
                    if (dicCheckPrjsOfAccounts.TryGetValue(equipAccount.Id, out List<EquipAccountCheckProject> checkPrjsOfAccount))
                    {
                        checkProjectList.AddRange(equipCt.UpdateCheckProjectList(dicCheckPrjsOfModels, equipAccount, checkPrjsOfAccount));
                    }
                    else
                    {
                        checkProjectList.AddRange(equipCt.CreateCheckProjectList(dicCheckPrjsOfModels, equipAccount));
                    }
                    //更新保养项目列表                    
                    if (dicMaintainPrjsOfAccounts.TryGetValue(equipAccount.Id, out List<EquipAccountMaintainProject> maintainPrjsOfAccount))
                    {
                        maintainProjectList.AddRange(equipCt.UpdateMaintainProjectList(dicMaintainPrjsOfModels, equipAccount, maintainPrjsOfAccount));
                    }
                    else
                    {
                        maintainProjectList.AddRange(equipCt.CreateMaintainProjectList(dicMaintainPrjsOfModels, equipAccount));
                    }

                    //更新润滑项目
                    if (dicLubricationsOfAccounts.TryGetValue(equipAccount.Id, out List<EquipAccountLubricationProject> lubricationProjectOfAccount))
                    {
                        lubricationProjectList.AddRange(equipCt.UpdateLubricationProjectList(dicLubricationPrjsOfModels, equipAccount, lubricationProjectOfAccount));
                    }
                    else
                    {
                        lubricationProjectList.AddRange(equipCt.CreateLubricationProjectList(dicLubricationPrjsOfModels, equipAccount));
                    }
                    //更新检验规程
                    if (dicRegularInspectionOfAccounts.TryGetValue(equipAccount.Id, out List<EquipAccountRegularInspection> regularInspectionOfAccount))
                    {
                        regularInspectionList.AddRange(UpdateInspectionRuleList(dicRegularInspectionOfModels, equipAccount, regularInspectionOfAccount));
                    }
                    else
                    {
                        regularInspectionList.AddRange(CreateInspectionRuleList(dicRegularInspectionOfModels, equipAccount));
                    }
                }

                SaveEquipAccountRelateInfos(checkProjectList, maintainProjectList, lubricationProjectList, regularInspectionList);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            return errMsg;

        }

        /// <summary>
        /// 保存设备台账点检/保养/单元组成/单元物料信息
        /// </summary>
        /// <param name="checkProjectList">设备台账点检项目列表</param>
        /// <param name="maintainProjectList">设备台账保养项目列表</param>                
        /// <param name="lubricationProjectsList">设备台账润滑项目</param>
        /// <param name="regularInspectionList">特种设备台账设备定检规程</param>
        private void SaveEquipAccountRelateInfos(EntityList<EquipAccountCheckProject> checkProjectList,
            EntityList<EquipAccountMaintainProject> maintainProjectList,
            EntityList<EquipAccountLubricationProject> lubricationProjectsList,
            EntityList<EquipAccountRegularInspection> regularInspectionList)
        {
            using (var tran = DB.TransactionScope(EntityDataProvider.ConnectionStringName))
            {
                RF.Save(checkProjectList);
                RF.Save(maintainProjectList);

                RF.Save(lubricationProjectsList);
                RF.Save(regularInspectionList);
                tran.Complete();
            }
        }


        /// <summary>
        /// 根据设备型号集合获取检验规程列表
        /// </summary>
        /// <param name="modelIds">设备型号Id列表</param>
        /// <returns>检验规程列表</returns>
        public virtual EntityList<EquipModelRegularInspection> GetEquipModelRegularInspection(List<double> modelIds)
        {
            return Query<EquipModelRegularInspection>().Where(p => modelIds.Contains(p.EquipModelId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取检验规程列表
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<EquipAccountRegularInspection> GetRegularInspectionOfAccounts(List<double> accountIds)
        {
            return accountIds.SplitContains(tempIds =>
            {
                return Query<EquipAccountRegularInspection>().Where(p => tempIds.Contains(p.SpecialEquipmentAccountId))
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty().LoadWith(EquipAccountLubricationProject.ProjectDetailProperty));
            });
        }


        /// <summary>
        /// 修改特种设备台账的检验规程
        /// </summary>
        /// <param name="dicRegularInspectionOfModels">设备型号检验规程字典</param>
        /// <param name="equipAccount">设备台账</param>
        /// <param name="regularInspectionOfAccount">特种设备台账已存在的检验规程</param>
        /// <returns></returns>
        public virtual EntityList<EquipAccountRegularInspection> UpdateInspectionRuleList(Dictionary<double, List<EquipModelRegularInspection>> dicRegularInspectionOfModels, EquipAccount equipAccount, List<EquipAccountRegularInspection> regularInspectionOfAccount)
        {
            if (dicRegularInspectionOfModels == null || equipAccount == null || regularInspectionOfAccount == null)
            {
                return new EntityList<EquipAccountRegularInspection>();
            }
            EntityList<EquipAccountRegularInspection> regularInspectionList = new EntityList<EquipAccountRegularInspection>();
            List<EquipModelRegularInspection> regularInspecOfModels = null;
            if (dicRegularInspectionOfModels.TryGetValue(equipAccount.EquipModelId, out regularInspecOfModels))
            {
                var dicRegularInspecOfAccount = regularInspectionOfAccount.ToDictionary(p => p.InspectionRuleId);
                foreach (var regularInspecOfModel in regularInspecOfModels)
                {
                    if (!dicRegularInspecOfAccount.ContainsKey(regularInspecOfModel.InspectionRuleId))
                    {
                        var lubricationProject = CreateEquipAccountRegularInspection(equipAccount, regularInspecOfModel);
                        regularInspectionList.Add(lubricationProject);
                    }
                }
            }
            return regularInspectionList;
        }


        /// <summary>
        /// 创建特种设备台账检验规程列表
        /// </summary>
        /// <param name="dicRegularInspectionOfModels"></param>
        /// <param name="equipAccount">设备台账</param>
        /// <returns></returns>
        public virtual EntityList<EquipAccountRegularInspection> CreateInspectionRuleList(Dictionary<double, List<EquipModelRegularInspection>> dicRegularInspectionOfModels, EquipAccount equipAccount)
        {
            if (dicRegularInspectionOfModels == null || equipAccount == null)
            {
                return new EntityList<EquipAccountRegularInspection>();
            }
            EntityList<EquipAccountRegularInspection> regularInspectionList = new EntityList<EquipAccountRegularInspection>();
            List<EquipModelRegularInspection> regularInspectionModels = null;
            if (dicRegularInspectionOfModels.TryGetValue(equipAccount.EquipModelId, out regularInspectionModels))
            {
                foreach (var regularInspectionMode in regularInspectionModels)
                {
                    var lubricationPjt = CreateEquipAccountRegularInspection(equipAccount, regularInspectionMode);
                    regularInspectionList.Add(lubricationPjt);
                }
            }
            return regularInspectionList;
        }


        /// <summary>
        ///创建设备定检规程
        /// </summary>
        /// <param name="equipAccount"></param>
        /// <param name="lubricationPrjOfModel"></param>
        /// <returns></returns>
        public virtual EquipAccountRegularInspection CreateEquipAccountRegularInspection(EquipAccount equipAccount, EquipModelRegularInspection lubricationPrjOfModel)
        {
            if (equipAccount == null || lubricationPrjOfModel == null)
            {
                return new EquipAccountRegularInspection();
            }
            return new EquipAccountRegularInspection()
            {
                SpecialEquipmentAccountId = equipAccount.Id,
                InspectionRuleId = lubricationPrjOfModel.InspectionRuleId,
                NotSubmit = true
            };
        }

        /// <summary>
        /// 根据Id获取设备台账
        /// </summary>
        /// <param name="id">设备台账Id</param>
        /// <returns></returns>
        public virtual SpecialEquipmentAccount GetSpecialEquipAccountById(double id)
        {
            return Query<SpecialEquipmentAccount>().Where(w => w.Id == id).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据特种设备台账Id和设备定检规程Id修改特种设备台账设备定检规程的下次检验时间
        /// </summary>
        /// <param name="SpecialEquipmentAccountId"></param>
        /// <param name="InspectionRuleId"></param>
        /// <param name="BillSourceType">单据类型</param>
        /// <param name="NextInspectionDate"></param>
        public virtual void UpdtaeEquipAccountRegularInspection(double SpecialEquipmentAccountId, double InspectionRuleId, BillSourceType BillSourceType, DateTime? NextInspectionDate)
        {
            if (BillSourceType == BillSourceType.Automatically)
            {
                DB.Update<EquipAccountRegularInspection>().Set(p => p.NextInspectionDate, NextInspectionDate).Set(p => p.NotSubmit, false).Where(p => p.SpecialEquipmentAccountId == SpecialEquipmentAccountId && p.InspectionRuleId == InspectionRuleId).Execute();
            }
            else
            {
                DB.Update<EquipAccountRegularInspection>().Set(p => p.NextInspectionDate, NextInspectionDate).Where(p => p.SpecialEquipmentAccountId == SpecialEquipmentAccountId && p.InspectionRuleId == InspectionRuleId).Execute();
            }
        }

        /// <summary>
        /// 根据特种设备台账和检验规程编码获取相关联的数据
        /// </summary>
        /// <param name="SpecialEquipmentAccountId"></param>
        /// <param name="InspectionRuleId"></param>
        /// <returns></returns>
        public virtual EquipAccountRegularInspection GetEquipAccountRegularInspection(double SpecialEquipmentAccountId, double InspectionRuleId)
        {
            return Query<EquipAccountRegularInspection>().Where(p => p.SpecialEquipmentAccountId == SpecialEquipmentAccountId && p.InspectionRuleId == InspectionRuleId).FirstOrDefault();
        }



        /// <summary>
        /// 根据设备台账Id获取特种设备台站与设备定检规程关联关系（并且首次定检时间和下次检验时间不可同时为null）
        /// </summary>
        /// <param name="ids">设备台账Id</param>
        /// <returns></returns>
        public virtual EntityList<EquipAccountRegularInspection> GetEquipAccountRegularInsById(List<double> ids)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(EquipAccountRegularInspection.InspectionRuleProperty);
            elo.LoadWith(EquipAccountRegularInspection.SpecialEquipmentAccountProperty);
            elo.LoadWithViewProperty();
            return ids.SplitContains((tmpIds) =>
            {
                return Query<EquipAccountRegularInspection>().Where(p => ids.Contains(p.SpecialEquipmentAccountId) && p.NotSubmit == true && (p.FirstInspectionDate != null || p.NextInspectionDate != null)).ToList(null, elo);
            });
        }
    }
}
