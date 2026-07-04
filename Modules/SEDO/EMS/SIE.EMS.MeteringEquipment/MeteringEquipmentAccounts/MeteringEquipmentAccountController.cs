using SIE.Common.Catalogs;
using SIE.Common.Configs;
using SIE.Core.Enums;
using SIE.DataAuth;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Equipments;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.Equipments.Models;
using SIE.EMS.MeteringEquipment.EquipModelExtensions;
using SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts.Handle;
using SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts.Tab;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts
{
    /// <summary>
    /// 计量设备台账控制器
    /// </summary>
    public partial class MeteringEquipmentAccountController : DomainController
    {
        /// <summary>
        /// 查询特种设备台账列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<MeteringEquipmentAccount> GetMeteringEquipmentAccountCriteria(MeteringEquipmentAccountCriteria criteria)
        {
            List<Catalog> CatalogList = RT.Service.Resolve<CatalogController>().GetCatalogList(EquipType.EquipTypeCatalogType).ToList();
            var query = Query<MeteringEquipmentAccount>();
            //获取设备台账配置的计量设备查询信息
            var config = ConfigService.GetConfig(new EquipModelEquipmentCategoryConfig(), typeof(EquipModel));
            List<string> strarr = new List<string>();
            if (config != null && config.EquipmentMeteringIds.IsNotEmpty())
            {
                strarr = config.EquipmentMeteringIds.Split(',').ToList();
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
            return query.OrderByDescending(p => p.UpdateDate).OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 根据Id获取计量设备台账
        /// </summary>
        /// <param name="meteringEquipmentAccountId">计量设备台账id</param>
        /// <returns>计量设备台账</returns>
        public virtual MeteringEquipmentAccount GetMeteringEquipmentAccountById(double meteringEquipmentAccountId)
        {
            using (DataAuths.LoadAll())
            {
                return RF.GetById<MeteringEquipmentAccount>(meteringEquipmentAccountId, new EagerLoadOptions().LoadWithViewProperty());
            }
        }


        /// <summary>
        /// 根据Ids获取计量设备台账
        /// </summary>
        /// <param name="idList">计量设备台账ids</param>

        /// <returns>计量设备台账集合</returns>
        public virtual EntityList<MeteringEquipmentAccount> GetMeteringEquipmentAccountList(List<double> idList)
        {
            using (DataAuths.LoadAll())
            {
                return idList.SplitContains((ids) =>
            {
                return Query<MeteringEquipmentAccount>().Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            }
        }


        /// <summary>
        /// 查询定检超期的计量设备台账
        /// </summary>
        /// <returns>计量设备台账集合</returns>
        public virtual EntityList<MeteringEquipmentAccount> GetMeteringEquipmentAccountList()
        {
            var query = Query<MeteringEquipmentAccount>();

            //获取设备台账配置的计量设备查询信息
            var config = ConfigService.GetConfig(new EquipModelEquipmentCategoryConfig(), typeof(EquipModel));
            List<string> strarr = new List<string>();
            if (config != null && config.EquipmentMeteringIds.IsNotEmpty())
            {
                strarr = config.EquipmentMeteringIds.Split(',').ToList();
            }

            List<Catalog> CatalogList = RT.Service.Resolve<CatalogController>().GetCatalogList(EquipType.EquipTypeCatalogType).ToList();

            List<string> arraylist = CatalogList.Where(p => strarr.Contains(p.Id.ToString())).Select(p => p.Code).ToList();

            query.Exists<EquipModel>((x, y) => y.Join<EquipType>((c, d) => c.EquipTypeId == d.Id)
                .Where(p => p.Id == x.EquipModelId)
                .WhereIf((arraylist.Any() && arraylist.Count > 0), e => arraylist.Contains(e.TypeCategory)));

            //只有设备台账状态为使用中，超期停用，不合格停用，维修，委外维修才更新
            List<AccountUseState> UseState = new List<AccountUseState>() { AccountUseState.Using, AccountUseState.Overdue, AccountUseState.Failed, AccountUseState.Repair, AccountUseState.OutsourcedRepair };
            //下次检验日期不为空且当前日期+1>下次检验日期  
            var now = RF.Find<MeteringEquipmentAccount>().GetDbTime().AddDays(-1);
            query.Where(p => p.NextInspectionDate != null && UseState.Contains(p.UseState) && now < p.NextInspectionDate);

            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 保存选择的检验规程数据
        /// </summary>
        /// <param name="EquipModelCalibrationInfos"></param>
        public virtual void SaveSelEquipAccountCalibration(List<EquipAccountCalibration> EquipModelCalibrationInfos)
        {
            if (EquipModelCalibrationInfos == null)
            {
                return;
            }
            EntityList<EquipAccountCalibration> savedData = new EntityList<EquipAccountCalibration>();
            foreach (var item in EquipModelCalibrationInfos)
            {
                var equipAccountCalibration = new EquipAccountCalibration();
                equipAccountCalibration.MeteringEquipmentAccountId = item.MeteringEquipmentAccountId;
                equipAccountCalibration.InspectionRuleId = item.InspectionRuleId;
                equipAccountCalibration.PeriodDays = item.InspectionRule.PeriodDays;
                equipAccountCalibration.WarningPeriod = item.InspectionRule.WarningPeriod;
                equipAccountCalibration.NotSubmit = true;
                savedData.Add(equipAccountCalibration);
            }
            RF.Save(savedData);
        }

        /// <summary>
        /// 通过设备台账Id列表获取点检项目列表
        /// </summary>
        /// <param name="accountIds">设备台账Id列表</param>
        /// <returns>点检项目列表</returns>
        public virtual EntityList<MeteringEquipAccountCheckProject> GetMeterCheckProjectsOfAccounts(List<double> accountIds)
        {
            return accountIds.SplitContains(tempIds =>
            {
                return Query<MeteringEquipAccountCheckProject>()
                    .Where(p => tempIds.Contains(p.EquipAccountId))
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 通过设备台账Id列表获取保养项目列表(贪婪加载点检保养项目)
        /// </summary>
        /// <param name="accountIds">设备台账Id列表</param>
        /// <returns>保养项目列表</returns>
        public virtual EntityList<MeteringEquipAccountMaintainProject> GetMaintainProjectsOfAccounts(List<double> accountIds)
        {
            return accountIds.SplitContains(tempIds =>
            {
                return Query<MeteringEquipAccountMaintainProject>().Where(p => tempIds.Contains(p.EquipAccountId))
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty().LoadWith(EquipAccountMaintainProject.ProjectDetailProperty));
            });
        }

        /// <summary>
        /// 更新设备台账点检项目(原已存在设备台账点检项目)
        /// </summary>
        /// <param name="dicCheckPrjsOfModels">设备型号点检项目字典</param>
        /// <param name="equipAccount">设备台账</param>
        /// <param name="checkPrjsOfAccount">原有设备台账点检项目列表</param>
        /// <returns>新的设备台账点检项目列表</returns>
        public virtual EntityList<MeteringEquipAccountCheckProject> UpdateCheckProjectList(Dictionary<double, List<EquipModelCheckProject>> dicCheckPrjsOfModels, MeteringEquipmentAccount equipAccount, List<MeteringEquipAccountCheckProject> checkPrjsOfAccount)
        {
            if (dicCheckPrjsOfModels == null || checkPrjsOfAccount == null)
            {
                return new EntityList<MeteringEquipAccountCheckProject>();
            }
            EntityList<MeteringEquipAccountCheckProject> checkProjectList = new EntityList<MeteringEquipAccountCheckProject>();
            List<EquipModelCheckProject> checkPrjsOfModel = null;
            if (dicCheckPrjsOfModels.TryGetValue(equipAccount.EquipModelId, out checkPrjsOfModel))
            {
                var dicCheckPrjsOfAccount = checkPrjsOfAccount.ToDictionary(p => p.ProjectDetailId);
                foreach (var checkPrjOfModel in checkPrjsOfModel)
                {
                    if (!dicCheckPrjsOfAccount.ContainsKey(checkPrjOfModel.ProjectDetailId))
                    {
                        var checkProject = CreateEquipAccountCheckProject(equipAccount, checkPrjOfModel);
                        checkProjectList.Add(checkProject);
                    }
                }
            }

            return checkProjectList;
        }

        /// <summary>
        /// 创建全新设备台账点检项目
        /// </summary>
        /// <param name="dicCheckPrjsOfModels">设备型号点检项目字典</param>
        /// <param name="equipAccount">设备台账</param>
        /// <returns>新的设备台账点检项目列表</returns>
        public virtual EntityList<MeteringEquipAccountCheckProject> CreateCheckProjectList(Dictionary<double, List<EquipModelCheckProject>> dicCheckPrjsOfModels, MeteringEquipmentAccount equipAccount)
        {
            if (dicCheckPrjsOfModels == null || equipAccount == null)
            {
                return new EntityList<MeteringEquipAccountCheckProject>();
            }
            EntityList<MeteringEquipAccountCheckProject> checkProjectList = new EntityList<MeteringEquipAccountCheckProject>();
            List<EquipModelCheckProject> checkPrjsOfModel = null;
            if (dicCheckPrjsOfModels.TryGetValue(equipAccount.EquipModelId, out checkPrjsOfModel))
            {
                foreach (var checkPrjOfModel in checkPrjsOfModel)
                {
                    var checkProject = CreateEquipAccountCheckProject(equipAccount, checkPrjOfModel);
                    checkProjectList.Add(checkProject);
                }
            }

            return checkProjectList;
        }

        /// <summary>
        /// 创建设备台账点检项目
        /// </summary>
        /// <param name="equipAccount">设备台账</param>
        /// <param name="checkPrjOfModel">设备型号点检项目</param>
        /// <returns>设备台账点检项目</returns>
        public virtual MeteringEquipAccountCheckProject CreateEquipAccountCheckProject(MeteringEquipmentAccount equipAccount, EquipModelCheckProject checkPrjOfModel)
        {
            if (equipAccount == null || checkPrjOfModel == null)
            {
                return new MeteringEquipAccountCheckProject();
            }
            return new MeteringEquipAccountCheckProject()
            {
                EquipAccountId = equipAccount.Id,
                ProjectDetailId = checkPrjOfModel.ProjectDetailId,
                DepartmentId = checkPrjOfModel.DepartmentId,
                PersistenceStatus = PersistenceStatus.New,
                ProjectName = checkPrjOfModel.ProjectName,
                ProjectType = checkPrjOfModel.ProjectType,
                CycleType = checkPrjOfModel.CycleType,
                Part = checkPrjOfModel.Part,
                Consumable = checkPrjOfModel.Consumable,
                Method = checkPrjOfModel.Method,
                Standard = checkPrjOfModel.Standard,
                MinValue = checkPrjOfModel.MinValue,
                MaxValue = checkPrjOfModel.MaxValue,
                Unit = checkPrjOfModel.Unit,
                UseTime = checkPrjOfModel.UseTime,
                DepartmentNameView = checkPrjOfModel.DepartmentName,

            };
        }

        /// <summary>
        /// 获取润滑项目列表
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<MeteringEquipAccountLubricationProject> GetLubricationProjectOfAccounts(List<double> accountIds)
        {
            return accountIds.SplitContains(tempIds =>
            {
                return Query<MeteringEquipAccountLubricationProject>()
                    .Where(p => tempIds.Contains(p.EquipAccountId))
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty().LoadWith(EquipAccountLubricationProject.ProjectDetailProperty));
            });
        }


        /// <summary>
        /// 更新设备台账保养项目(原已存在设备台账保养项目)
        /// </summary>
        /// <param name="dicMaintainPrjsOfModels">设备型号保养项目字典</param>
        /// <param name="equipAccount">设备台账</param>
        /// <param name="maintainPrjsOfAccount">原有设备台账保养项目列表</param>
        /// <returns>新的设备台账保养项目列表</returns>
        public virtual EntityList<MeteringEquipAccountMaintainProject> UpdateMaintainProjectList(Dictionary<double, List<EquipModelMaintainProject>> dicMaintainPrjsOfModels, MeteringEquipmentAccount equipAccount, List<MeteringEquipAccountMaintainProject> maintainPrjsOfAccount)
        {
            if (dicMaintainPrjsOfModels == null || equipAccount == null)
            {
                return new EntityList<MeteringEquipAccountMaintainProject>();
            }
            EntityList<MeteringEquipAccountMaintainProject> maintainProjectList = new EntityList<MeteringEquipAccountMaintainProject>();
            List<EquipModelMaintainProject> maintainPrjsOfModel = null;
            if (dicMaintainPrjsOfModels.TryGetValue(equipAccount.EquipModelId, out maintainPrjsOfModel))
            {
                var dicMaintainPrjsOfAccount = maintainPrjsOfAccount.ToDictionary(p => p.ProjectDetailId);
                foreach (var maintainPrjOfModel in maintainPrjsOfModel)
                {
                    if (!dicMaintainPrjsOfAccount.ContainsKey(maintainPrjOfModel.ProjectDetailId))
                    {
                        var maintainProject = CreateEquipAccountMaintainProject(equipAccount, maintainPrjOfModel);
                        maintainProjectList.Add(maintainProject);
                    }
                }
            }

            return maintainProjectList;
        }

        /// <summary>
        /// 创建全新设备台账保养项目
        /// </summary>
        /// <param name="dicMaintainPrjsOfModels">设备型号保养项目字典</param>
        /// <param name="equipAccount">设备台账</param>
        /// <returns>新的设备台账保养项目列表</returns>
        public virtual EntityList<MeteringEquipAccountMaintainProject> CreateMaintainProjectList(Dictionary<double, List<EquipModelMaintainProject>> dicMaintainPrjsOfModels, MeteringEquipmentAccount equipAccount)
        {
            if (dicMaintainPrjsOfModels == null || equipAccount == null)
            {
                return new EntityList<MeteringEquipAccountMaintainProject>();
            }
            EntityList<MeteringEquipAccountMaintainProject> maintainProjectList = new EntityList<MeteringEquipAccountMaintainProject>();
            List<EquipModelMaintainProject> maintainPrjsOfModel = null;
            if (dicMaintainPrjsOfModels.TryGetValue(equipAccount.EquipModelId, out maintainPrjsOfModel))
            {
                foreach (var maintainPrjOfModel in maintainPrjsOfModel)
                {
                    var maintainProject = CreateEquipAccountMaintainProject(equipAccount, maintainPrjOfModel);
                    maintainProjectList.Add(maintainProject);
                }
            }

            return maintainProjectList;
        }

        /// <summary>
        /// 创建设备台账保养项目
        /// </summary>
        /// <param name="equipAccount">设备台账</param>
        /// <param name="maintainPrjOfModel">设备型号保养项目</param>
        /// <returns>设备台账保养项目</returns>
        public virtual MeteringEquipAccountMaintainProject CreateEquipAccountMaintainProject(MeteringEquipmentAccount equipAccount, EquipModelMaintainProject maintainPrjOfModel)
        {
            if (equipAccount == null || maintainPrjOfModel == null)
            {
                return new MeteringEquipAccountMaintainProject();
            }
            return new MeteringEquipAccountMaintainProject()
            {
                EquipAccountId = equipAccount.Id,
                ProjectDetailId = maintainPrjOfModel.ProjectDetailId,
                DepartmentId = maintainPrjOfModel.DepartmentId,
                PersistenceStatus = PersistenceStatus.New,
                ProjectName = maintainPrjOfModel.ProjectName,
                ProjectType = maintainPrjOfModel.ProjectType,
                CycleType = maintainPrjOfModel.CycleType,
                Part = maintainPrjOfModel.Part,
                Consumable = maintainPrjOfModel.Consumable,
                Method = maintainPrjOfModel.Method,
                Standard = maintainPrjOfModel.Standard,
                MinValue = maintainPrjOfModel.MinValue,
                MaxValue = maintainPrjOfModel.MaxValue,
                Unit = maintainPrjOfModel.Unit,
                UseTime = maintainPrjOfModel.UseTime,
                DepartmentNameView = maintainPrjOfModel.DepartmentName,
            };
        }

        /// <summary>
        /// 更新设备台账润滑项目
        /// </summary>
        /// <param name="dicLubricationProjectOfModels">设备型号润滑项目字典</param>
        /// <param name="equipAccount">设备台账</param>
        /// <param name="lubricationProjectsOfAccount">设备台账已存在的润滑项目</param>
        /// <returns></returns>
        public virtual EntityList<MeteringEquipAccountLubricationProject> UpdateLubricationProjectList(Dictionary<double, List<EquipModelLubricationProject>> dicLubricationProjectOfModels,
            MeteringEquipmentAccount equipAccount, List<MeteringEquipAccountLubricationProject> lubricationProjectsOfAccount)
        {
            if (dicLubricationProjectOfModels == null || equipAccount == null)
            {
                return new EntityList<MeteringEquipAccountLubricationProject>();
            }
            EntityList<MeteringEquipAccountLubricationProject> lubricationProjectList = new EntityList<MeteringEquipAccountLubricationProject>();
            List<EquipModelLubricationProject> lubricationPrjsOfModel = null;
            if (dicLubricationProjectOfModels.TryGetValue(equipAccount.EquipModelId, out lubricationPrjsOfModel))
            {
                var dicLubricationPrjsOfAccount = lubricationProjectsOfAccount.ToDictionary(p => p.ProjectDetailId);
                MeteringEquipAccountLubricaSparePart sp = null;
                foreach (var lubricationPrjOfModel in lubricationPrjsOfModel)
                {
                    if (!dicLubricationPrjsOfAccount.ContainsKey(lubricationPrjOfModel.ProjectDetailId))
                    {
                        var lubricationProject = CreateEquipAccountLubricationProject(equipAccount, lubricationPrjOfModel);
                        foreach (var item in lubricationPrjOfModel.LubricaSparePartList)
                        {
                            sp = new MeteringEquipAccountLubricaSparePart();
                            sp.Qty = item.Qty;
                            sp.LubricationProjectId = lubricationProject.Id;
                            sp.SparePart = item.SparePart;
                            lubricationProject.LubricaSparePartList.Add(sp);
                        }
                        lubricationProjectList.Add(lubricationProject);
                    }
                }
            }

            return lubricationProjectList;
        }

        /// <summary>
        /// 创建设备台账润滑项目列表
        /// </summary>
        /// <param name="dicLubricationPrjsOfModels">设备型号润滑项目字典</param>
        /// <param name="equipAccount">设备台账</param>
        /// <returns></returns>
        public virtual EntityList<MeteringEquipAccountLubricationProject> CreateLubricationProjectList(Dictionary<double, List<EquipModelLubricationProject>> dicLubricationPrjsOfModels, MeteringEquipmentAccount equipAccount)
        {
            if (dicLubricationPrjsOfModels == null || equipAccount == null)
            {
                return new EntityList<MeteringEquipAccountLubricationProject>();
            }
            EntityList<MeteringEquipAccountLubricationProject> lubricationProjectList = new EntityList<MeteringEquipAccountLubricationProject>();
            List<EquipModelLubricationProject> lubricationProject = null;
            if (dicLubricationPrjsOfModels.TryGetValue(equipAccount.EquipModelId, out lubricationProject))
            {
                MeteringEquipAccountLubricaSparePart sp = null;
                foreach (var prjOfModel in lubricationProject)
                {
                    var lubricationPjt = CreateEquipAccountLubricationProject(equipAccount, prjOfModel);
                    foreach (var item in prjOfModel.LubricaSparePartList)
                    {
                        sp = new MeteringEquipAccountLubricaSparePart();
                        sp.Qty = item.Qty;
                        sp.LubricationProjectId = lubricationPjt.Id;
                        sp.SparePart = item.SparePart;
                        lubricationPjt.LubricaSparePartList.Add(sp);
                    }
                    lubricationProjectList.Add(lubricationPjt);
                }
            }
            return lubricationProjectList;
        }

        /// <summary>
        ///创建润滑项目
        /// </summary>
        /// <param name="equipAccount"></param>
        /// <param name="lubricationPrjOfModel"></param>
        /// <returns></returns>
        public virtual MeteringEquipAccountLubricationProject CreateEquipAccountLubricationProject(MeteringEquipmentAccount equipAccount, EquipModelLubricationProject lubricationPrjOfModel)
        {
            if (lubricationPrjOfModel == null || equipAccount == null)
            {
                return new MeteringEquipAccountLubricationProject();
            }
            return new MeteringEquipAccountLubricationProject()
            {
                EquipAccountId = equipAccount.Id,
                ProjectDetailId = lubricationPrjOfModel.ProjectDetailId,
                DepartmentId = lubricationPrjOfModel.DepartmentId,
                PersistenceStatus = PersistenceStatus.New,
                ProjectName = lubricationPrjOfModel.ProjectName,
                ProjectType = lubricationPrjOfModel.ProjectType,
                CycleType = lubricationPrjOfModel.CycleType,
                Part = lubricationPrjOfModel.Part,
                Consumable = lubricationPrjOfModel.Consumable,
                Method = lubricationPrjOfModel.Method,
                Standard = lubricationPrjOfModel.Standard,
                MinValue = lubricationPrjOfModel.MinValue,
                MaxValue = lubricationPrjOfModel.MaxValue,
                Unit = lubricationPrjOfModel.Unit,
                UseTime = lubricationPrjOfModel.UseTime,
                DepartmentNameView = lubricationPrjOfModel.Department?.Name,
                ProjectCycle = lubricationPrjOfModel.ProjectCycle,
                WarningPeriod = lubricationPrjOfModel.WarningPeriod,
                LubricatingType = lubricationPrjOfModel.LubricatingType,
            };
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
                EntityList<MeteringEquipAccountCheckProject> checkProjectList = new EntityList<MeteringEquipAccountCheckProject>();

                EntityList<MeteringEquipAccountMaintainProject> maintainProjectList = new EntityList<MeteringEquipAccountMaintainProject>();

                EntityList<MeteringEquipAccountLubricationProject> lubricationProjectList = new EntityList<MeteringEquipAccountLubricationProject>();

                EntityList<EquipAccountCalibration> regularInspectionList = new EntityList<EquipAccountCalibration>();

                var equipCt = RT.Service.Resolve<EquipController>();
                #region 加载初始数据
                //获取设备台账列表
                var equipAccounts = GetMeteringEquipmentAccountList(accountIds);
                var modelIds = equipAccounts.Select(p => p.EquipModelId).Distinct().ToList();

                //获取设备型号对应的点检保养项目列表
                var checkProjectsOfModels = equipCt.GetCheckProjectsOfModels(modelIds);
                var dicCheckPrjsOfModels = checkProjectsOfModels.GroupBy(p => p.EquipModelId).ToDictionary(p => p.Key, p => p.ToList());

                var maintainProjectsOfModels = equipCt.GetMaintainProjectsOfModels(modelIds);
                var dicMaintainPrjsOfModels = maintainProjectsOfModels.GroupBy(p => p.EquipModelId).ToDictionary(p => p.Key, p => p.ToList());

                var lubricationProjectOfModels = equipCt.GetEquipModelLubricationProjects(modelIds);
                var dicLubricationPrjsOfModels = lubricationProjectOfModels.GroupBy(p => p.EquipModelId).ToDictionary(p => p.Key, p => p.ToList());

                //设备型号扩展的数据 设备型号id,检验规程id
                var regularInspectionOfModels = GetEquipModelCalibration(modelIds);
                var dicRegularInspectionOfModels = regularInspectionOfModels.GroupBy(p => p.EquipModelId).ToDictionary(p => p.Key, p => p.ToList());

                //获取设备台账对应的点检保养项目列表           
                var checkProjectsOfAccounts = GetMeterCheckProjectsOfAccounts(accountIds);
                var dicCheckPrjsOfAccounts = checkProjectsOfAccounts.GroupBy(p => p.EquipAccountId).ToDictionary(p => p.Key, p => p.ToList());

                var maintainProjectsOfAccounts = GetMaintainProjectsOfAccounts(accountIds);
                var dicMaintainPrjsOfAccounts = maintainProjectsOfAccounts.GroupBy(p => p.EquipAccountId).ToDictionary(p => p.Key, p => p.ToList());

                var lubricationsOfAccounts = GetLubricationProjectOfAccounts(accountIds);
                var dicLubricationsOfAccounts = lubricationsOfAccounts.GroupBy(p => p.EquipAccountId).ToDictionary(p => p.Key, p => p.ToList());

                //计量设备台账数据  计量设备台账id,检验规程id。
                var regularInspectionOfAccounts = GetRegularInspectionOfAccounts(accountIds);
                var dicRegularInspectionOfAccounts = regularInspectionOfAccounts.GroupBy(p => p.MeteringEquipmentAccountId).ToDictionary(p => p.Key, p => p.ToList());
                //获取设备台账对应的单元组成和单元组成物料列表                
                #endregion

                foreach (var equipAccount in equipAccounts)
                {
                    //更新点检项目列表                   
                    if (dicCheckPrjsOfAccounts.TryGetValue(equipAccount.Id, out List<MeteringEquipAccountCheckProject> checkPrjsOfAccount))
                    {
                        checkProjectList.AddRange(UpdateCheckProjectList(dicCheckPrjsOfModels, equipAccount, checkPrjsOfAccount));
                    }
                    else
                    {
                        checkProjectList.AddRange(CreateCheckProjectList(dicCheckPrjsOfModels, equipAccount));
                    }
                    //更新保养项目列表                    
                    if (dicMaintainPrjsOfAccounts.TryGetValue(equipAccount.Id, out List<MeteringEquipAccountMaintainProject> maintainPrjsOfAccount))
                    {
                        maintainProjectList.AddRange(UpdateMaintainProjectList(dicMaintainPrjsOfModels, equipAccount, maintainPrjsOfAccount));
                    }
                    else
                    {
                        maintainProjectList.AddRange(CreateMaintainProjectList(dicMaintainPrjsOfModels, equipAccount));
                    }

                    //更新润滑项目
                    if (dicLubricationsOfAccounts.TryGetValue(equipAccount.Id, out List<MeteringEquipAccountLubricationProject> lubricationProjectOfAccount))
                    {
                        lubricationProjectList.AddRange(UpdateLubricationProjectList(dicLubricationPrjsOfModels, equipAccount, lubricationProjectOfAccount));
                    }
                    else
                    {
                        lubricationProjectList.AddRange(CreateLubricationProjectList(dicLubricationPrjsOfModels, equipAccount));
                    }
                    //更新计量检验规程
                    if (dicRegularInspectionOfAccounts.TryGetValue(equipAccount.Id, out List<EquipAccountCalibration> regularInspectionOfAccount))
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
        /// <param name="regularInspectionList">计量设备台账设备定检规程</param>
        private void SaveEquipAccountRelateInfos(EntityList<MeteringEquipAccountCheckProject> checkProjectList,
            EntityList<MeteringEquipAccountMaintainProject> maintainProjectList,
            EntityList<MeteringEquipAccountLubricationProject> lubricationProjectsList,
            EntityList<EquipAccountCalibration> regularInspectionList)
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
        public virtual EntityList<EquipModelCalibration> GetEquipModelCalibration(List<double> modelIds)
        {
            return Query<EquipModelCalibration>().Where(p => modelIds.Contains(p.EquipModelId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取检验规程列表
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<EquipAccountCalibration> GetRegularInspectionOfAccounts(List<double> accountIds)
        {
            return accountIds.SplitContains(tempIds =>
            {
                return Query<EquipAccountCalibration>().Where(p => tempIds.Contains(p.MeteringEquipmentAccountId))
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty()
                        .LoadWith(MeteringEquipAccountLubricationProject.ProjectDetailProperty));
            });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dicRegularInspectionOfModels">设备型号检验规程字典</param>
        /// <param name="equipAccount">设备台账</param>
        /// <param name="regularInspectionOfAccount">计量设备台账已存在的检验规程</param>
        /// <returns></returns>
        public virtual EntityList<EquipAccountCalibration> UpdateInspectionRuleList(Dictionary<double, List<EquipModelCalibration>> dicRegularInspectionOfModels, MeteringEquipmentAccount equipAccount, List<EquipAccountCalibration> regularInspectionOfAccount)
        {
            if (dicRegularInspectionOfModels == null || equipAccount == null || regularInspectionOfAccount == null)
            {
                return new EntityList<EquipAccountCalibration>();
            }
            EntityList<EquipAccountCalibration> regularInspectionList = new EntityList<EquipAccountCalibration>();
            List<EquipModelCalibration> regularInspecOfModels = null;
            if (dicRegularInspectionOfModels.TryGetValue(equipAccount.EquipModelId, out regularInspecOfModels))
            {
                var dicRegularInspecOfAccount = regularInspectionOfAccount.ToDictionary(p => p.InspectionRuleId);
                foreach (var regularInspecOfModel in regularInspecOfModels)
                {
                    if (!dicRegularInspecOfAccount.ContainsKey(regularInspecOfModel.InspectionRuleId))
                    {
                        var lubricationProject = CreateEquipAccountCalibration(equipAccount, regularInspecOfModel);
                        regularInspectionList.Add(lubricationProject);
                    }
                }
            }
            return regularInspectionList;
        }


        /// <summary>
        /// 创建计量设备台账检验规程列表
        /// </summary>
        /// <param name="dicRegularInspectionOfModels"></param>
        /// <param name="equipAccount">设备台账</param>
        /// <returns></returns>
        public virtual EntityList<EquipAccountCalibration> CreateInspectionRuleList(Dictionary<double, List<EquipModelCalibration>> dicRegularInspectionOfModels, MeteringEquipmentAccount equipAccount)
        {
            if (dicRegularInspectionOfModels == null || equipAccount == null)
            {
                return new EntityList<EquipAccountCalibration>();
            }
            EntityList<EquipAccountCalibration> regularInspectionList = new EntityList<EquipAccountCalibration>();
            List<EquipModelCalibration> regularInspectionModels = null;
            if (dicRegularInspectionOfModels.TryGetValue(equipAccount.EquipModelId, out regularInspectionModels))
            {
                foreach (var regularInspectionMode in regularInspectionModels)
                {
                    var lubricationPjt = CreateEquipAccountCalibration(equipAccount, regularInspectionMode);
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
        public virtual EquipAccountCalibration CreateEquipAccountCalibration(MeteringEquipmentAccount equipAccount, EquipModelCalibration lubricationPrjOfModel)
        {
            if (equipAccount == null || lubricationPrjOfModel == null)
            {
                return new EquipAccountCalibration();
            }
            return new EquipAccountCalibration()
            {
                MeteringEquipmentAccountId = equipAccount.Id,
                InspectionRuleId = lubricationPrjOfModel.InspectionRuleId,
                PeriodDays = lubricationPrjOfModel.PeriodDays,
                WarningPeriod = lubricationPrjOfModel.WarningPeriod,
                NotSubmit = true
            };
        }

        /// <summary>
        /// 根据特种设备台账Id和设备定检规程Id修改计量设备台账设备定检规程的下次检验时间
        /// </summary>
        /// <param name="MeteringEquipmentAccountId"></param>
        /// <param name="InspectionRuleId"></param>
        /// <param name="BillSourceType">单据类型</param>
        /// <param name="NextInspectionDate"></param>
        public virtual void UpdtaeEquipAccountCalibration(double MeteringEquipmentAccountId, double InspectionRuleId, BillSourceType BillSourceType, DateTime? NextInspectionDate)
        {
            if (BillSourceType == BillSourceType.Automatically)
            {
                DB.Update<EquipAccountCalibration>().Set(p => p.NextInspectionDate, NextInspectionDate).Set(p => p.NotSubmit, false).Where(p => p.MeteringEquipmentAccountId == MeteringEquipmentAccountId && p.InspectionRuleId == InspectionRuleId).Execute();
            }
            else
            {
                DB.Update<EquipAccountCalibration>().Set(p => p.NextInspectionDate, NextInspectionDate).Where(p => p.MeteringEquipmentAccountId == MeteringEquipmentAccountId && p.InspectionRuleId == InspectionRuleId).Execute();
            }
        }

        /// <summary>
        /// 根据设备台账Id获取计量设备台站与设备定检规程关联关系（并且首次定检时间和下次检验时间不可同时为null）
        /// </summary>
        /// <param name="ids">设备台账Id</param>
        /// <returns></returns>
        public virtual EntityList<EquipAccountCalibration> GetEquipAccountRegularInsById(List<double> ids)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(EquipAccountCalibration.InspectionRuleProperty);
            elo.LoadWith(EquipAccountCalibration.MeteringEquipmentAccountProperty);
            elo.LoadWithViewProperty();
            return ids.SplitContains((tmpIds) =>
            {
                return Query<EquipAccountCalibration>().Where(p => ids.Contains(p.MeteringEquipmentAccountId) && p.NotSubmit == true && (p.PrevInspectionDate != null || p.NextInspectionDate != null)).ToList(null, elo);
            });
        }

        /// <summary>
        /// 根据计量设备台账和检验规程编码获取相关联的数据
        /// </summary>
        /// <param name="MeteringEquipmentAccountIds"></param>
        /// <param name="InspectionRuleId"></param>
        /// <returns></returns>
        public virtual EntityList<EquipAccountCalibration> GetEquipAccountCalibration(List<double> MeteringEquipmentAccountIds, double InspectionRuleId)
        {
            using (DataAuths.LoadAll())
            {
                return MeteringEquipmentAccountIds.SplitContains(tempIds =>
            {
                return Query<EquipAccountCalibration>()
                    .Where(p => tempIds.Contains(p.MeteringEquipmentAccountId) && p.InspectionRuleId == InspectionRuleId)
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            }
        }


        /// <summary>
        /// 保存计量校验规程反写设备台账下次检验日期
        /// </summary>
        /// <param name="EquipAccountCalibrationList"></param>
        /// <returns></returns>
        public virtual void SaveEquipAccountCalibration(EntityList<EquipAccountCalibration> EquipAccountCalibrationList)
        {
            if (EquipAccountCalibrationList == null)
            {
                return;
            }
            //此规程的设备台账id
            var MeaId = EquipAccountCalibrationList.FirstOrDefault().MeteringEquipmentAccountId;
            //此规程的设备台账
            var MeteringEquipmentAccount = GetById<MeteringEquipmentAccount>(MeaId);

            //设备台账下次检验日期为null,则修改为修改集合中最小的下次检验日期
            var MaxDate = EquipAccountCalibrationList.Max(p => p.NextInspectionDate);

            if (MeteringEquipmentAccount.NextInspectionDate == null)
            {
                MeteringEquipmentAccount.NextInspectionDate = MaxDate;
            }
            if (MeteringEquipmentAccount.NextInspectionDate != null && MeteringEquipmentAccount.NextInspectionDate < MaxDate)
            {
                MeteringEquipmentAccount.NextInspectionDate = MaxDate;
            }

            using (var tran = DB.TransactionScope(EntityDataProvider.ConnectionStringName))
            {
                if (EquipAccountCalibrationList.Any())
                {
                    RF.Save(EquipAccountCalibrationList);
                }
                RF.Save(MeteringEquipmentAccount);
                tran.Complete();
            }
        }

        /// <summary>
        /// 自动更新计量设备台账超期停用
        /// </summary>
        public virtual void AtuoEditOverdueSchedule()
        {
            EquipAccountOverdueScheduleHandle handle = new EquipAccountOverdueScheduleHandle();
            handle.DoSchedule();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipAccountProcesses"></param>
        public virtual void SaveMeterEquipAccountProcessList(List<MeterEquipAccountProcess> equipAccountProcesses)
        {
            var equipAccountProcess = equipAccountProcesses.FirstOrDefault();

            var processIds = equipAccountProcesses
                .Select(x => x.ProcessId).Distinct().ToList();

            var equipAccountId = equipAccountProcess.EquipAccountId;

            var equipAccountProcessesOfExists = Query<MeterEquipAccountProcess>()
                .Where(x => x.EquipAccountId == equipAccountId)
                .Where(x => processIds.Contains(x.ProcessId))
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            if (equipAccountProcessesOfExists.Any())
            {
                throw new ValidationException("工序【{0}】已经添加，请不要重复添加。"
                    .L10nFormat(string.Join(",", equipAccountProcessesOfExists.Select(x => x.ProcessName).Distinct())));
            }

            EntityList<MeterEquipAccountProcess> equipProcesss = new EntityList<MeterEquipAccountProcess>();

            equipAccountProcesses.ForEach(p =>
            {
                p.PersistenceStatus = PersistenceStatus.New;
                equipProcesss.Add(p);
            });

            RF.Save(equipProcesss);
        }
    }
}
