using SIE.Common.Configs;
using SIE.Core.Common.Controllers;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Enums;
using SIE.EMS.FixedAssets.Accounts;
using SIE.EMS.InventoryTasks;
using SIE.EMS.InventoryTasks.ViewModels;
using SIE.Equipments.Configs;
using SIE.Equipments.DeviceIOTParas.Details;
using SIE.Equipments.DeviceIOTParas;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.WorkFlows;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using SIE.Resources.Employees;
using SIE.Fixtures;
using SIE.Common;

namespace SIE.EMS.InventoryPlans
{
    /// <summary>
    /// 盘点计划控制器
    /// </summary>
    public partial class InventoryPlanController : DomainController
    {
        /// <summary>
        /// 根据id获取盘点计划
        /// </summary>
        /// <param name="planId">id</param>
        /// <returns>盘点计划</returns>
        public virtual InventoryPlan GetInventoryPlanById(double planId)
        {
            return Query<InventoryPlan>().Where(p => p.Id == planId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取工治具盘点范围的所有可用工治具编码,且管理方式为ID管理
        /// </summary>
        /// <param name="c"></param>
        /// <param name="entity"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual EntityList<FixtureEncode> GetTaskFixtureEncodeList(PagingInfo c, AddFixtureProfitViewModel entity, string r)
        {
            var encodeIds = GetTaskFixtureEncodeId(entity.InventoryTaskId);
            if (encodeIds.Any())
            {
                return Query<FixtureEncode>().Where(p => encodeIds.Contains(p.Id)).Where(p=>p.FixtureModel.ManageMode== ManageMode.Number)
                    .WhereIf(!r.IsNullOrEmpty(), m => m.Code.Contains(r))
                    .ToList(c, new EagerLoadOptions().LoadWithViewProperty());
            }
            return new EntityList<FixtureEncode>();
        }

        /// <summary>
        /// 获取盘点任务的盘点范围可用的工治具编码ID
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>

        public virtual List<double> GetTaskFixtureEncodeId(double taskId)
        {
            var task = RF.GetById<InventoryTask>(taskId);
            if (task == null)
            {
                throw new ValidationException("盘点任务信息丢失".L10N());
            }
            var plan = GetInventoryPlanById(task.InventoryPlanId);
            InventoryPlanFixture inventoryPlanFixture = plan.GetProperty(InventoryPlanExtEquip.InventoryPlanFixtureProperty);
            if (inventoryPlanFixture == null)
            {
                inventoryPlanFixture = Query<InventoryPlanFixture>().Where(m => m.InventoryPlanId == plan.Id).FirstOrDefault();
            }
            var fixtureAccounts = GetInventoryFixtureAccount(inventoryPlanFixture);
            if (!fixtureAccounts.Any())
            {
                throw new ValidationException("盘点范围获取不到工治具数据".L10N());
            }
            return fixtureAccounts.Select(m => m.FixtureEncodeId).ToList();
        }

        /// <summary>
        /// 查询盘点计划
        /// </summary>
        /// <param name="criteria">盘点计划查询</param>
        /// <returns>盘点计划</returns>
        public virtual EntityList<InventoryPlan> CriteriaInventoryPlans(InventoryPlanCriteria criteria)
        {
            var query = Query<InventoryPlan>();
            if (!criteria.PlanNo.IsNullOrWhiteSpace())
            {
                query.Where(p => p.PlanNo.Contains(criteria.PlanNo));
            }
            if (criteria.FactoryId.HasValue)
            {
                query.Where(p => p.FactoryId == criteria.FactoryId.Value);
            }
            if (criteria.InventoryAssetObject.HasValue)
            {
                query.Where(p => p.InventoryAssetObject == criteria.InventoryAssetObject.Value);
            }
            if (!criteria.InventoryType.IsNullOrWhiteSpace())
            {
                query.Where(p => p.InventoryType == criteria.InventoryType);
            }
            if (criteria.ResponsibleId.HasValue)
            {
                query.Where(p => p.ResponsibleId == criteria.ResponsibleId.Value);
            }
            if (criteria.ApprovalStatus.HasValue)
            {
                query.Where(p => p.ApprovalStatus == criteria.ApprovalStatus.Value);
            }
            if (criteria.CreateDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue.Value);
            }
            if (criteria.CreateDate.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue.Value);
            }
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 保存盘点计划上传的图片
        /// </summary>
        /// <param name="dataId"></param>
        /// <param name="returnFilePath"></param>
        /// <param name="type"></param>
        public virtual void SaveInventoryPlanImage(double dataId, string returnFilePath, string type)
        {
            if (typeof(InventoryTask).FullName == type)
            {
                DB.Update<InventoryTask>().Set(p => p.PhotoFilePath, returnFilePath).Where(m => m.Id == dataId).Execute();
            }
            if (typeof(InventoryTaskEquipment).FullName == type)
            {
                DB.Update<InventoryTaskEquipment>().Set(p => p.PhotoFilePath, returnFilePath).Where(m => m.Id == dataId).Execute();
            }
        }

        /// <summary>
        /// 根据id列表获取盘点计划
        /// </summary>
        /// <param name="ids">id列表</param>
        /// <returns>盘点计划</returns>
        public virtual EntityList<InventoryPlan> GetInventoryPlansByIds(List<double> ids)
        {
            return ids.SplitContains(id => Query<InventoryPlan>().Where(p => id.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
        }

        /// <summary>
        /// 根据盘点计划id获取盘点计划范围（设备）
        /// </summary>
        /// <param name="planId">盘点计划id</param>
        /// <returns>盘点计划范围（设备）</returns>
        public virtual InventoryPlanEquipment GetInventoryPlanEquipment(double planId)
        {
            return Query<InventoryPlanEquipment>().Where(p => p.InventoryPlanId == planId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据盘点计划id获取盘点计划范围（工治具）
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        public virtual InventoryPlanFixture GetInventoryPlanFixture(double planId)
        {
            return Query<InventoryPlanFixture>().Where(p => p.InventoryPlanId == planId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据盘点计划id列表获取盘点计划范围（设备）
        /// </summary>
        /// <param name="planIds">盘点计划id列表</param>
        /// <returns>盘点计划范围（设备）</returns>
        public virtual EntityList<InventoryPlanEquipment> GetInventoryPlanEquipments(List<double> planIds)
        {
            return planIds.SplitContains(ids => Query<InventoryPlanEquipment>().Where(p => ids.Contains(p.InventoryPlanId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
        }

        /// <summary>
        /// 获取盘点备件计划
        /// </summary>
        /// <param name="planIds"></param>
        /// <returns></returns>

        public virtual EntityList<InventoryPlanSparePart> GetInventoryPlanSpareParts(List<double> planIds)
        {
            return planIds.SplitContains(ids => Query<InventoryPlanSparePart>().Where(p => ids.Contains(p.InventoryPlanId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
        }

        /// <summary>
        /// 根据盘点计划id列表获取盘点人
        /// </summary>
        /// <param name="planIds">盘点计划id列表</param>
        /// <returns>盘点人</returns>
        public virtual EntityList<InventoryCounter> GetInventoryCounters(List<double> planIds)
        {
            return planIds.SplitContains(ids => Query<InventoryCounter>().Where(p => ids.Contains(p.InventoryPlanId)).ToList());
        }

        /// <summary>
        /// 创建一个新的盘点计划
        /// </summary>
        /// <returns>新的盘点计划</returns>
        public virtual InventoryPlan GetNewInventoryPlan()
        {
            var plan = new InventoryPlan();
            plan.PlanNo = RT.Service.Resolve<CommonController>().GetNo<InventoryPlan>("盘点计划");
            plan.ApprovalStatus = ApprovalStatus.Draft;
            plan.ApplyDate = DateTime.Today;
            plan.InventoryExecuteType = InventoryExecuteType.Bright;
            plan.PlanEndDate = DateTime.Today;
            plan.InventoryAssetObject = InventoryAssetObject.Equipment;
            return plan;
        }

        /// <summary>
        /// 保存盘点计划
        /// </summary>
        /// <param name="plan">盘点计划</param>
        public virtual void SaveInventoryPlan(InventoryPlan plan)
        {
            if (plan == null)
            {
                throw new ValidationException("保存失败，数据异常".L10N());
            }

            if (plan.PersistenceStatus != PersistenceStatus.New)
            {
                var old = GetById<InventoryPlan>(plan.Id);

                if (old == null)
                {
                    throw new ValidationException("保存失败，数据异常".L10N());
                }

                if (old.ApprovalStatus != ApprovalStatus.Draft && old.ApprovalStatus != ApprovalStatus.Reject)
                {
                    throw new ValidationException("保存失败，审核状态为【待提交】、【驳回】的数据才能修改".L10N());
                }
            }

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //保存盘点计划
                RF.Save(plan);

                switch (plan.InventoryAssetObject)
                {
                    case InventoryAssetObject.Equipment:
                        {
                            SaveInventoryPlanEquipment(plan);
                        }
                        break;
                    case InventoryAssetObject.Spare:
                        {
                            //保存盘点计划备件盘点范围
                            SaveInventoryPlanSparePart(plan);
                        }
                        break;
                    case InventoryAssetObject.Fixture:
                        {
                            SaveInventoryPlanFixture(plan);
                        }
                        break;
                    default:
                        break;
                }

                trans.Complete();
            }
        }

        /// <summary>
        /// 保存盘点计划-工治具
        /// </summary>
        /// <param name="plan"></param>
        private void SaveInventoryPlanFixture(InventoryPlan plan)
        {
            //保存盘点计划范围
            InventoryPlanFixture inventoryPlanFixture = plan.GetProperty(InventoryPlanExtEquip.InventoryPlanFixtureProperty);
            if (inventoryPlanFixture == null)
            {
                inventoryPlanFixture = GetInventoryPlanFixture(plan.Id);
                if (inventoryPlanFixture == null)
                {
                    inventoryPlanFixture = new InventoryPlanFixture() { InventoryPlan = plan };
                }
            }
            else
            {
                inventoryPlanFixture.InventoryPlanId = plan.Id;
            }

            RF.Save(inventoryPlanFixture);

            UpdateInventoryFixtureCounter(plan);

            if (plan.PersistenceStatus != PersistenceStatus.New)
            {
                DB.Delete<InventoryPlanEquipment>().Where(m => m.Id == plan.Id).Execute();
                DB.Delete<InventoryCounter>().Where(m => m.InventoryPlanId == plan.Id).Execute();
                //删除备件盘点范围
                DB.Delete<InventoryPlanSparePart>().Where(m => m.InventoryPlanId == plan.Id).Execute();
            }
        }

        /// <summary>
        /// 保存备件盘点计划
        /// </summary>
        /// <param name="plan"></param>
        private void SaveInventoryPlanSparePart(InventoryPlan plan)
        {
            InventoryPlanSparePart inventoryPlanSparePart
                                            = plan.GetProperty(InventoryPlanExtEquip.InventoryPlanSparePartProperty);
            if (inventoryPlanSparePart == null)
            {
                inventoryPlanSparePart = GetInventoryPlanSparePart(plan.Id);
                if (inventoryPlanSparePart == null)
                {
                    inventoryPlanSparePart = new InventoryPlanSparePart() { InventoryPlan = plan };
                }
            }
            else
            {
                inventoryPlanSparePart.InventoryPlanId = plan.Id;
            }
            if (inventoryPlanSparePart.WarehouseIds.IsNullOrEmpty())
            {
                throw new ValidationException("备件盘点范围仓库必填".L10N());
            }
            RF.Save(inventoryPlanSparePart);

            if (plan.PersistenceStatus != PersistenceStatus.New)
            {
                //删除设备盘点数据
                DB.Delete<InventoryPlanEquipment>().Where(m => m.Id == plan.Id).Execute();
                DB.Delete<InventoryCounter>().Where(m => m.InventoryPlanId == plan.Id).Execute();

                //删除工治具盘点数据
                DB.Delete<InventoryPlanFixture>().Where(m => m.InventoryPlanId == plan.Id).Execute();
                DB.Delete<InventoryFixtureCounter>().Where(m => m.InventoryPlanId == plan.Id).Execute();
            }
        }

        /// <summary>
        /// 保存设备盘点计划
        /// </summary>
        private void SaveInventoryPlanEquipment(InventoryPlan plan)
        {
            //保存盘点计划范围
            InventoryPlanEquipment invPlanEquip = plan.GetProperty(InventoryPlanExtEquip.InventoryEquipmentProperty);
            if (invPlanEquip == null)
            {
                invPlanEquip = GetInventoryPlanEquipment(plan.Id);
                if (invPlanEquip == null)
                {
                    invPlanEquip = new InventoryPlanEquipment() { InventoryPlan = plan };
                }
            }
            else
            {
                invPlanEquip.InventoryPlanId = plan.Id;
            }

            RF.Save(invPlanEquip);

            //保存时，盘点责任人是否存在于盘点人中，存在则更新初盘、复盘都勾选，盘点范围为【所有设备】；不存在则新增一条数据
            UpdateInventoryCounter(plan);

            if (plan.PersistenceStatus != PersistenceStatus.New)
            {
                DB.Delete<InventoryPlanFixture>().Where(m => m.InventoryPlanId == plan.Id).Execute();
                DB.Delete<InventoryFixtureCounter>().Where(m => m.InventoryPlanId == plan.Id).Execute();

                //删除备件盘点范围
                DB.Delete<InventoryPlanSparePart>().Where(m => m.InventoryPlanId == plan.Id).Execute();
            }
        }

        /// <summary>
        /// 保存时，盘点责任人是否存在于盘点人中，存在则更新初盘、复盘都勾选，盘点范围为【所有设备】；不存在则新增一条数据
        /// </summary>
        /// <param name="plan">盘点计划</param>
        public virtual void UpdateInventoryCounter(InventoryPlan plan)
        {
            var oldCounter = Query<InventoryCounter>().Where(p => p.InventoryPlanId == plan.Id && p.EmployeeId == plan.ResponsibleId).FirstOrDefault();
            if (oldCounter != null)
            {
                oldCounter.First = true;
                oldCounter.Second = true;
                oldCounter.InventoryScope = InventoryScope.All;
                RF.Save(oldCounter);
            }
            else
            {
                if (plan.ResponsibleId.HasValue)
                {
                    var newCounter = new InventoryCounter();
                    newCounter.EmployeeId = plan.ResponsibleId.Value;
                    newCounter.InventoryPlanId = plan.Id;
                    newCounter.First = true;
                    newCounter.Second = true;
                    newCounter.InventoryScope = InventoryScope.All;
                    RF.Save(newCounter);
                }
            }
        }

        /// <summary>
        /// 保存时，盘点责任人是否存在于盘点人中，存在则更新初盘、复盘都勾选，盘点范围为【所有设备】；不存在则新增一条数据
        /// </summary>
        /// <param name="plan"></param>
        public virtual void UpdateInventoryFixtureCounter(InventoryPlan plan)
        {
            var oldCounter = Query<InventoryFixtureCounter>().Where(p => p.InventoryPlanId == plan.Id && p.EmployeeId == plan.ResponsibleId).FirstOrDefault();
            if (oldCounter != null)
            {
                oldCounter.First = true;
                oldCounter.Second = true;
                RF.Save(oldCounter);
            }
            else
            {
                if (plan.ResponsibleId.HasValue)
                {
                    var newCounter = new InventoryFixtureCounter();
                    newCounter.EmployeeId = plan.ResponsibleId.Value;
                    newCounter.InventoryPlanId = plan.Id;
                    newCounter.First = true;
                    newCounter.Second = true;
                    RF.Save(newCounter);
                }
            }
        }


        /// <summary>
        /// 删除前校验最新状态
        /// </summary>
        /// <param name="ids">实体id</param>
        public virtual void DeleteInventoryPlan(List<double> ids)
        {
            var entity = GetInventoryPlansByIds(ids);
            if (entity.Any(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject))
            {
                throw new ValidationException("只有审核状态为【待提交】、【驳回】的数据才能删除".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                DB.Delete<InventoryPlan>().Where(p => ids.Contains(p.Id)).Execute();
                DB.Delete<InventoryPlanEquipment>().Where(p => ids.Contains(p.InventoryPlanId)).Execute();
                trans.Complete();
            }
        }

        ///<summary>
        /// 获取审批流程配置
        /// </summary>
        /// <returns>审批流程配置</returns>
        public virtual ApprovalConfigValue GetApprovalConfigValue()
        {
            var config = ConfigService.GetConfig(new ApprovalConfig(), typeof(InventoryPlan));
            if (config == null)
            {
                throw new ValidationException("未找到审批流程配置,请检查规则配置".L10N());
            }
            return config;
        }

        /// <summary>
        /// 提交盘点计划(只能单个)
        /// </summary>
        /// <param name="planId">选择行id</param>
        public virtual void SubmitInventoryPlan(double planId)
        {
            var plan = GetInventoryPlanById(planId);
            if (plan == null)
            {
                throw new ValidationException("提交数据异常".L10N());
            }
            //只有审核状态为【待提交】、【驳回】的数据才能点击，点击时还要后台获取最新状态进行校验
            if (plan.ApprovalStatus != ApprovalStatus.Draft && plan.ApprovalStatus != ApprovalStatus.Reject)
            {
                throw new ValidationException("只有审核状态为【待提交】、【驳回】的数据才能提交".L10N());
            }
            var config = GetApprovalConfigValue();
            plan.ApprovalStatus = ApprovalStatus.PendingReview;

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //是否启用审批为false时提交后自动审批
                if (!config.EnableAudit)
                {
                    ExamineInventoryPlanInner(planId, ApprovalResult.Pass, "通过".L10N(), plan);
                }
                else
                {
                    if (plan.InventoryAssetObject == InventoryAssetObject.Spare)
                    {
                        //提交前验证备件盘点范围内是否有备件
                        RT.Service.Resolve<InventoryPlanSparePartController>().ValidationInventoryScopeHasSparePartDetail(plan);
                    }

                    //根据配置项【是否启用审批】，【是】则更新审核状态为【待审核】，【否】则更新审核状态为【通过】且执行以下逻辑                    
                    RF.Save(plan);
                }
                //生成审核结果为提交的审核记录数据
                var now = RF.Find<InventoryPlan>().GetDbTime();
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(new List<double> { planId }, typeof(InventoryPlan).FullName, ApprovalResult.Submit, now, "");
                trans.Complete();
            }
        }

        /// <summary>
        /// 生成盘点任务
        /// </summary>
        /// <param name="plan">盘点计划</param>
        private void GenerateInventoryTask(InventoryPlan plan)
        {
            switch (plan.InventoryAssetObject)
            {
                case InventoryAssetObject.Equipment:
                    {
                        //资产对象为【设备】时，执行逻辑：
                        GenerateEuipInventoryTask(plan);
                    }
                    break;
                case InventoryAssetObject.Spare:
                    {
                        //资产对象为【备件】时，执行逻辑：
                        RT.Service.Resolve<InventoryPlanSparePartController>().GenerateSparePartInventoryTask(plan);
                    }
                    break;
                case InventoryAssetObject.Fixture:
                    {
                        //资产对象为【工治具】时，执行逻辑：
                        GenerateFixtureInventoryTask(plan);
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 生成工治具盘点任务
        /// </summary>
        /// <param name="plan"></param>
        private void GenerateFixtureInventoryTask(InventoryPlan plan)
        {
            /*根据【工治具类型、工治具型号、工治具编码、管控方式】获取符合的工治具编码，按工治具的管控方式逻辑
        	编码管控：到工治具编码台账获取数据，一条数据生成一条编码明细
        	序列号管控：到工治具ID台账获取数据，一条数据生成一条序列号明细，工治具编码相同的生成一条编码明细；【固定资产、资产分类、资产责任人】有值时，限制为符合的数据
            */
            InventoryPlanFixture inventoryPlanFixture = plan.GetProperty(InventoryPlanExtEquip.InventoryPlanFixtureProperty);
            if (inventoryPlanFixture == null)
            {
                inventoryPlanFixture = Query<InventoryPlanFixture>().Where(m => m.InventoryPlanId == plan.Id).FirstOrDefault();
            }
            var fixtureAccounts = GetInventoryFixtureAccount(inventoryPlanFixture);
            if (!fixtureAccounts.Any())
            {
                throw new ValidationException("盘点范围获取不到工治具数据".L10N());
            }
            var task = GetInventoryTask(plan);
            task.GenerateId();
            plan.InventoryFixtureCounterList.ForEach(item =>
            {
                task.InventoryTaskFixtureCounterList.Add(new InventoryTaskFixtureCounter()
                {
                    EmployeeId = item.EmployeeId,
                    InventoryTaskId = task.Id,
                    First = item.First,
                    Second = item.Second,
                });

            });
            var fixtureAccountCodeList = fixtureAccounts.Where(m => m.ManageMode == Fixtures.ManageMode.Code).ToList();
            var fixtureAccountIdList = fixtureAccounts.Where(m => m.ManageMode == Fixtures.ManageMode.Number).ToList();
            foreach (var code in fixtureAccountCodeList)//编码类
            {
                task.InventoryTaskFixtureEncodeList.Add(new InventoryTaskFixtureEncode()
                {
                    FixtureEncodeId = code.FixtureEncodeId,
                    InventoryStatus = InventoryStatus.Not,
                    InventoryAssetSource = InventoryAssetSource.Account,
                    StockQty = code.InStockQty,
                    StockPassQty = code.StockList.Sum(p => p.PassQty),
                    StockNgQty = code.StockList.Sum(p => p.NgQty),
                    Online = code.OnlineQty,
                    LibPassQty = code.StockList.Sum(p => p.PassQty),
                    LibNgQty = code.StockList.Sum(p => p.NgQty),
                    Total = code.OnlineQty + code.InStockQty,
                    InventoryTaskId = task.Id
                });
            }
            var fixtureEncodeIds = fixtureAccountIdList.Select(m => m.FixtureEncodeId).Distinct().ToList();

            foreach (var fixtureEncodeId in fixtureEncodeIds)//Id类
            {
                var fixtureAccountIdEncodeList = fixtureAccountIdList.Where(m => m.FixtureEncodeId == fixtureEncodeId);
                var stockQty = fixtureAccountIdEncodeList.Count(m => m.AccountState == FixtureAccountState.InStorage);
                var onlineQty = fixtureAccountIdEncodeList.Count(m => m.AccountState == FixtureAccountState.Online);

                if (!task.InventoryTaskFixtureEncodeList.Any(m => m.FixtureEncodeId == fixtureEncodeId))
                {
                    if (stockQty == 0 && onlineQty == 0)//去除数据为0的
                    {
                        var exsited = task.InventoryTaskFixtureEncodeList.FirstOrDefault(m => m.FixtureEncodeId == fixtureEncodeId);
                        task.InventoryTaskFixtureEncodeList.Remove(exsited);
                    }
                    else
                    {
                        task.InventoryTaskFixtureEncodeList.Add(new InventoryTaskFixtureEncode()
                        {
                            FixtureEncodeId = fixtureEncodeId,
                            InventoryStatus = InventoryStatus.Not,
                            InventoryAssetSource = InventoryAssetSource.Account,
                            StockQty = stockQty,
                            Online = onlineQty,
                            Total = stockQty + onlineQty,
                            InventoryTaskId = task.Id,

                        });
                    }
                }
                foreach (var accounNumber in fixtureAccountIdEncodeList)
                {
                    if (accounNumber.AccountState != FixtureAccountState.Online && accounNumber.AccountState != FixtureAccountState.InStorage)
                    {
                        continue;
                    }
                    var inventoryTaskFixtureIdAccount = new InventoryTaskFixtureIdAccount()
                    {
                        InventoryStatus = InventoryStatus.Not,
                        FixtureEncodeId = fixtureEncodeId,
                        InventoryAssetSource = InventoryAssetSource.Account,
                        InventoryTaskId = task.Id,
                        Sn = accounNumber.Code,
                        //SoftVersionNo = accounNumber.SoftwareVersion,
                        //ValidTime = accounNumber.EffectDate,
                    };
                    //在线时取值：工治具ID台账的履历中操作类型为【领用】的最新的数据的【产线】，再获取产线所属车间
                    if (accounNumber.AccountState == FixtureAccountState.Online)
                    {
                        SetOnline(accounNumber, inventoryTaskFixtureIdAccount);
                        inventoryTaskFixtureIdAccount.FixtureStatus = FixtureStatus.OnLine;
                    }
                    //在库时取值：工治具ID台账的库存详情
                    if (accounNumber.AccountState == FixtureAccountState.InStorage)
                    {
                        var stock = Query<FixtureAccountStock>().Where(m => m.FixtureAccountId == accounNumber.Id).FirstOrDefault();
                        if (stock != null)
                        {
                            inventoryTaskFixtureIdAccount.WarehouseId = stock.FixtureWarehouseId;
                            inventoryTaskFixtureIdAccount.StorageLocationId = stock.FixtureStorageLocationId;
                        }
                        inventoryTaskFixtureIdAccount.FixtureStatus = FixtureStatus.InStorage;
                    }
                    task.InventoryTaskFixtureIdAccountList.Add(inventoryTaskFixtureIdAccount);
                }
            }
            RF.Save(task);
        }

        /// <summary>
        /// 设置在线状态的工治具车间产线
        /// </summary>
        /// <param name="accounNumber"></param>
        /// <param name="inventoryTaskFixtureIdAccount"></param>
        private void SetOnline(FixtureAccount accounNumber, InventoryTaskFixtureIdAccount inventoryTaskFixtureIdAccount)
        {
            var useResume = Query<FixtureAccountUseResume>().Where(m => m.FixtureAccountId == accounNumber.Id && m.OperationType == UseResumeType.Receive).FirstOrDefault();
            if (useResume != null)
            {
                inventoryTaskFixtureIdAccount.ResourceId = useResume.ResourceId;
                inventoryTaskFixtureIdAccount.WorkshopId = useResume.Resource.WorkShopId;
            }
        }

        /// <summary>
        /// 获取单个盘点任务
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        private InventoryTask GetInventoryTask(InventoryPlan plan)
        {
            var taskNo = RT.Service.Resolve<CommonController>().GetNo<InventoryTask>("盘点任务");
            var task = new InventoryTask();
            task.TaskNo = taskNo;
            task.FactoryId = plan.FactoryId;
            task.InventoryPlanId = plan.Id;
            task.Percentage = 0;
            task.ApprovalStatus = ApprovalStatus.Draft;
            task.InventoryTaskStatus = InventoryTaskStatus.NotBegin;
            task.InventoryType = plan.InventoryType;
            task.Remark = plan.Remark;
            task.PlanEndDate = plan.PlanEndDate;
            task.ResponsibleId = plan.ResponsibleId;
            task.InventoryExecuteType = plan.InventoryExecuteType;
            task.NeedPhoto = plan.NeedPhoto;
            task.PhotoFilePath = plan.PhotoFilePath;
            task.IsAsset = plan.IsAsset;
            task.ApplyDate = plan.ApplyDate;
            task.GenerateId();
            return task;
        }

        /// <summary>
        /// 获取符合条件工治具台账
        /// </summary>
        /// <returns></returns>
        public virtual  EntityList<FixtureAccount> GetInventoryFixtureAccount(InventoryPlanFixture inventoryPlanFixture)
        {
            var splitChar = new char[] { ',' };
            var qurey = Query<FixtureAccount>();
            if (!inventoryPlanFixture.FixtureTypeIds.IsNullOrEmpty())
            {
                List<double?> ids = inventoryPlanFixture.FixtureTypeIds.Split(splitChar, StringSplitOptions.RemoveEmptyEntries).Select(m => (double?)double.Parse(m)).ToList();
                qurey.Where(m => ids.Contains(m.FixtureEncode.FixtureModel.FixtureTypeId));
            }
            if (!inventoryPlanFixture.FixtureModelIds.IsNullOrEmpty())
            {
                List<double> ids = inventoryPlanFixture.FixtureModelIds.Split(splitChar, StringSplitOptions.RemoveEmptyEntries).Select(m => double.Parse(m)).ToList();
                qurey.Where(m => ids.Contains(m.FixtureEncode.FixtureModelId));
            }
            if (!inventoryPlanFixture.FixtureEncodeIds.IsNullOrEmpty())
            {
                List<double> ids = inventoryPlanFixture.FixtureEncodeIds.Split(splitChar, StringSplitOptions.RemoveEmptyEntries).Select(m => double.Parse(m)).ToList();
                qurey.Where(m => ids.Contains(m.FixtureEncodeId));
            }

            if (inventoryPlanFixture.ManageMode.HasValue)
            {
                qurey.Where(p => p.FixtureEncode.FixtureModel.ManageMode == inventoryPlanFixture.ManageMode);
                if (inventoryPlanFixture.ManageMode == Fixtures.ManageMode.Number)
                {
                    qurey.WhereIf(inventoryPlanFixture.IsFixAsset != null && inventoryPlanFixture.IsFixAsset == YesNo.Yes, m => m.FixedAssetsAccountId != null);
                    qurey.WhereIf(!inventoryPlanFixture.AssetsCategory.IsNullOrEmpty(), m => m.FixedAssetsAccount.AssetsCategory == inventoryPlanFixture.AssetsCategory);
                    qurey.WhereIf(inventoryPlanFixture.AssetOwnerId.HasValue, m => m.FixedAssetsAccount.AssetOwnerId == inventoryPlanFixture.AssetOwnerId);
                }
            }

            return qurey.ToList(null, new EagerLoadOptions().LoadWithViewProperty().LoadWith(FixtureAccount.StockListProperty));
        }


        /// <summary>
        /// 生成设备盘点任务
        /// </summary>
        /// <param name="plan"></param>
        private void GenerateEuipInventoryTask(InventoryPlan plan)
        {
            //获取符合盘点范围且管理状态不为【待验收、报废、处置】的设备台账
            var equips = GetInventoryEquipAccount(plan);

            //按设备的【管理部门】分组，一组生成一个盘点任务
            var groups = equips.GroupBy(p => p.ManageDepartmentId);
            var taskNos = RT.Service.Resolve<CommonController>().GetNos<InventoryTask>(groups.Count(), "盘点任务");
            var i = 0;
            foreach (var group in groups)
            {
                //创建一个盘点任务
                var task = SaveInventoryTask(taskNos[i], group.Key, plan);
                i++;
                var userIds = new List<double>();
                foreach (var equipAccount in group)
                {
                    //创建盘点任务设备清单
                    SaveInventoryTaskEquipment(equipAccount, task.Id);
                    if (equipAccount.UserId.HasValue)
                    {
                        userIds.Add(equipAccount.UserId.Value);
                    }
                }
                //创建盘点任务盘点人
                var counters = GetInventoryCounters(new List<double> { plan.Id });
                SaveInventoryTaskCounter(counters, task.Id, userIds);
            }
        }

        /// <summary>
        /// 获取符合盘点范围（盘点范围字段为空的表示不限制条件）且管理状态不为【待验收、报废、处置】的设备台账，获取不到数据时报错：盘点范围获取不到设备数据
        /// </summary>
        /// <param name="plan">盘点计划id</param>
        /// <returns>设备台账</returns>
        private EntityList<EquipAccount> GetInventoryEquipAccount(InventoryPlan plan)
        {
            var planId = plan.Id;
            var equipCtl = RT.Service.Resolve<EquipAccountController>();
            var query = Query<EquipAccount>();
            query.Where(p => p.UseState != AccountUseState.ToAccepted && p.UseState != AccountUseState.Scrap && p.UseState != AccountUseState.DisposedOf);
            var equips = new EntityList<EquipAccount>();
            //盘点范围
            var range = GetInventoryPlanEquipment(planId);
            //判断是否存在盘点查询条件
            var isExistRange = false;

            //获取是否存在设备清单id
            var equipListIds = GetEquipmentListIds(planId);

            if (range != null)
            {
                query.Where(p => p.FactoryId == plan.FactoryId);//主表的工厂
                if (range.ManageDeptId.HasValue)//管理部门
                {
                    query.Where(p => p.ManageDepartmentId == range.ManageDeptId);
                    isExistRange = true;
                }
                if (range.UseDeptId.HasValue)//使用部门
                {
                    query.Where(p => p.UseDepartmentId == range.UseDeptId);
                    isExistRange = true;
                }
                if (range.WarehouseId.HasValue)//仓库
                {
                    query.Where(p => p.WarehouseId == range.WarehouseId);
                    isExistRange = true;
                }
                if (range.WorkShopId.HasValue)//车间
                {
                    query.Where(p => p.WorkShopId == range.WorkShopId);
                    isExistRange = true;
                }
                if (range.EquipModelId.HasValue)//设备型号
                {
                    query.Where(p => p.EquipModelId == range.EquipModelId);
                    isExistRange = true;
                }
                else if (range.EquipTypeId.HasValue)  //设备类型
                {
                    query.Where(p => p.EquipModel.EquipTypeId == range.EquipTypeId);
                    isExistRange = true;
                }
                else if (!range.TypeCategory.IsNullOrWhiteSpace())  //设备类别
                {
                    query.Where(p => p.EquipModel.TypeCategory == range.TypeCategory);
                    isExistRange = true;
                }
                if (!range.UseLevel.IsNullOrWhiteSpace())//ABC分类
                {
                    query.Where(p => p.UseLevel == range.UseLevel);
                    isExistRange = true;
                }
                if (range.IsAsset == YesNo.Yes)//固定资产
                {
                    query.Where(p => p.IssAsset == true || p.FixedAssetsAccountId != null);
                    if (!range.AssetsCategory.IsNullOrWhiteSpace())//资产分类
                    {
                        query.Exists<FixedAssetsAccount>((a, y) => y.Where(b => a.FixedAssetsAccountId == b.Id && b.AssetsCategory == range.AssetsCategory));
                    }
                    if (range.AssetOwnerId.HasValue)//资产责任人
                    {
                        query.Where(p => p.ResPersonId == range.AssetOwnerId);
                    }
                    isExistRange = true;
                }
            }
            using (SIE.DataAuth.DataAuths.LoadAll())
            {
                //存在设备清单
                if (equipListIds.Count > 0)
                {
                    var equipLists = equipCtl.GetEquipAccountsByIds(equipListIds);
                    equips.AddRange(equipLists);
                    //存在盘点范围条件,去重
                    if (isExistRange)
                    {
                        var equipAccounts = query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                        equipAccounts.ForEach(e =>
                        {
                            if (equips.FirstOrDefault(p => p.Id == e.Id) == null)
                            {
                                equips.AddRange(equipAccounts);
                            }
                        });
                    }
                }
                else
                {
                    equips = query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                }
            }
            if (!equips.Any())
            {
                throw new ValidationException("盘点范围获取不到设备数据".L10N());
            }
            return equips;
        }

        /// <summary>
        /// 创建一个盘点任务
        /// </summary>
        /// <param name="taskNo">单号</param>
        /// <param name="manageDeptId">管理部门id</param>
        /// <param name="plan">盘点计划</param>
        /// <returns>盘点任务</returns>
        private InventoryTask SaveInventoryTask(string taskNo, double? manageDeptId, InventoryPlan plan)
        {
            var task = new InventoryTask();
            task.TaskNo = taskNo;
            task.ManageDeptId = manageDeptId;
            task.FactoryId = plan.FactoryId;
            task.InventoryPlanId = plan.Id;
            task.Percentage = 0;
            task.ApprovalStatus = ApprovalStatus.Draft;
            task.InventoryTaskStatus = InventoryTaskStatus.NotBegin;
            task.InventoryType = plan.InventoryType;
            task.Remark = plan.Remark;
            task.PlanEndDate = plan.PlanEndDate;
            task.ResponsibleId = plan.ResponsibleId;
            task.InventoryExecuteType = plan.InventoryExecuteType;
            task.NeedPhoto = plan.NeedPhoto;
            task.PhotoFilePath = plan.PhotoFilePath;
            task.IsAsset = plan.IsAsset;
            task.ApplyDate = plan.ApplyDate;
            RF.Save(task);
            return task;
        }

        /// <summary>
        /// 创建盘点任务设备清单
        /// </summary>
        /// <param name="equipAccount">设备</param>
        /// <param name="taskId">盘点任务id</param>
        private void SaveInventoryTaskEquipment(EquipAccount equipAccount, double taskId)
        {
            var taskEquip = new InventoryTaskEquipment();
            taskEquip.InventoryTaskId = taskId;
            taskEquip.InventoryStatus = InventoryStatus.Not;
            taskEquip.InventoryAssetSource = InventoryAssetSource.Account;
            taskEquip.EquipAccountId = equipAccount.Id;
            taskEquip.EquipmentCode = equipAccount.Code;
            taskEquip.EquipmentName = equipAccount.Name;
            taskEquip.Alias = equipAccount.Alias;
            taskEquip.TypeCategory = equipAccount.EquipTypeCategory;
            taskEquip.EquipTypeId = equipAccount.EquipTypeViewId;
            taskEquip.EquipModelId = equipAccount.EquipModelId;

            taskEquip.OldManageDeptId = equipAccount.ManageDepartmentId;
            taskEquip.OldManageDept = equipAccount.ManageDepartmentName;
            taskEquip.OldUseDeptId = equipAccount.UseDepartmentId;
            taskEquip.OldUseDeptName = equipAccount.UseDepartmentName;
            taskEquip.OldAccountUseState = equipAccount.UseState;
            taskEquip.OldAccountState = equipAccount.State;
            taskEquip.OldUserId = equipAccount.UserId;
            if (equipAccount.UserId.HasValue)
            {
                taskEquip.OldUserName = RF.GetById<Employee>(equipAccount.UserId)?.Name;
            }
            taskEquip.OldWorkShopId = equipAccount.WorkShopId;
            taskEquip.OldWorkShopName = equipAccount.WorkShopName;
            taskEquip.OldResourceId = equipAccount.ResourceId;
            taskEquip.OldResourceName = equipAccount.ResourceName;
            taskEquip.OldWarehouseId = equipAccount.WarehouseId;
            taskEquip.OldWarehouseCode = equipAccount.WarehouseName;
            taskEquip.OldStorageLocationId = equipAccount.StorageLocationId;
            if (equipAccount.StorageLocationId.HasValue)
            {
                taskEquip.OldStorageLocationCode = equipAccount.StorageLocation.Code;
            }
            taskEquip.OldLocation = equipAccount.InstallationLocation;


            RF.Save(taskEquip);
        }

        /// <summary>
        /// 创建盘点任务盘点人
        /// </summary>
        /// <param name="counters">盘点人</param>
        /// <param name="taskId">盘点任务id</param>
        /// <param name="userIds">使用责任人</param>
        private void SaveInventoryTaskCounter(EntityList<InventoryCounter> counters, double taskId, List<double> userIds)
        {
            var taskCounters = new EntityList<InventoryTaskCounter>();
            foreach (var counter in counters)
            {
                var taskCounter = new InventoryTaskCounter();
                taskCounter.EmployeeId = counter.EmployeeId;
                taskCounter.First = counter.First;
                taskCounter.Second = counter.Second;
                taskCounter.InventoryScope = counter.InventoryScope;
                taskCounter.InventoryTaskId = taskId;
                taskCounters.Add(taskCounter);
            }
            //额外增加设备清单中所有的【使用责任人】，初盘勾选，盘点范围为【自有管理】
            foreach (var userId in userIds.Distinct())
            {
                var oldCounter = taskCounters.FirstOrDefault(p => p.EmployeeId == userId);
                if (oldCounter == null)
                {
                    var taskCounter = new InventoryTaskCounter();
                    taskCounter.First = true;
                    taskCounter.InventoryScope = InventoryScope.Own;
                    taskCounter.InventoryTaskId = taskId;
                    taskCounter.EmployeeId = userId;
                    taskCounters.Add(taskCounter);
                }
                else
                {
                    oldCounter.First = true;
                }
            }
            RF.Save(taskCounters);
        }

        /// <summary>
        /// 撤回盘点计划
        /// </summary>
        /// <param name="planId">选择行id</param>
        public virtual void CancelInventoryPlan(double planId)
        {
            var plan = GetInventoryPlanById(planId);
            if (plan == null)
            {
                throw new ValidationException("撤回数据异常".L10N());
            }
            //只有状态为【待审核】的数据才能点击，点击时还要后台获取最新状态进行校验
            if (plan.ApprovalStatus != ApprovalStatus.PendingReview)
            {
                throw new ValidationException("只有状态为【待审核】的数据才能操作".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //更新状态为【待提交】
                plan.ApprovalStatus = ApprovalStatus.Draft;
                RF.Save(plan);

                //生成审核结果为撤回的审核记录数据
                var now = RF.Find<InventoryPlan>().GetDbTime();
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(new List<double> { planId }, typeof(InventoryPlan).FullName, ApprovalResult.Retract, now, "");
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核盘点计划（只能单个）
        /// </summary>
        /// <param name="planId">选择行id</param>
        /// <param name="value">结果</param>
        /// <param name="remark">备注</param>
        /// <param name="plan">盘点任务</param>
        public virtual void ExamineInventoryPlan(double planId, ApprovalResult value, string remark, InventoryPlan plan = null)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                ExamineInventoryPlanInner(planId, ApprovalResult.Pass, remark, plan);
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核盘点计划（只能单个）
        /// </summary>
        /// <param name="planId">选择行id</param>
        /// <param name="value">结果</param>
        /// <param name="remark">备注</param>
        /// <param name="plan">盘点任务</param>
        private void ExamineInventoryPlanInner(double planId, ApprovalResult value, string remark, InventoryPlan plan = null)
        {
            if (plan == null)
            {
                plan = GetInventoryPlanById(planId);
                if (plan == null)
                {
                    throw new ValidationException("审核数据异常".L10N());
                }
            }

            //只有状态为【待审核】的数据才能点击，点击时还要后台获取最新状态进行校验
            if (plan.ApprovalStatus != ApprovalStatus.PendingReview)
            {
                throw new ValidationException("只有状态为【待审核】的数据才能审核".L10N());
            }

            //更新审核状态为【通过】或【驳回】
            plan.ApprovalStatus = value == ApprovalResult.Pass ? ApprovalStatus.Audited : ApprovalStatus.Reject;
            RF.Save(plan);

            //审核【通过】时，执行【提交】按钮写的重复的逻辑
            if (value == ApprovalResult.Pass)
            {
                //生成盘点任务
                GenerateInventoryTask(plan);
            }

            //往审批记录子表插入一条数据
            var now = RF.Find<InventoryPlan>().GetDbTime();
            RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(new List<double> { planId }, typeof(InventoryPlan).FullName, value, now, remark);
        }


        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="remark"></param>
        public virtual void Shutdown(List<double> selectedIds, string remark)
        {
            var tasks = GetListInventoryPlanByIds(selectedIds);
            if (tasks.Any(p => !p.CloseRemark.IsNullOrEmpty()))
            {
                throw new ValidationException("已关闭单据不能再关闭".L10N());
            }
            List<double> Ids = new List<double>();
            tasks.ForEach(item =>
            {
                item.CloseRemark = remark;
                Ids.Add(item.Id);
            });
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                DB.Update<InventoryTask>().Set(p => p.InventoryTaskStatus, InventoryTaskStatus.Closed)
                    .Set(p => p.CloseRemark, remark)
                    .Where(m => Ids.Contains(m.InventoryPlanId)).Execute();
                RF.Save(tasks);
                trans.Complete();
            }
        }

        /// <summary>
        /// 根据ID集合获取盘点计划
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <returns></returns>
        private EntityList<InventoryPlan> GetListInventoryPlanByIds(List<double> selectedIds)
        {
            return selectedIds.SplitContains(ids =>
            {
                return Query<InventoryPlan>().Where(m => ids.Contains(m.Id)).ToList();
            });
        }

        /// <summary>
        /// 获取工具治台账信息
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>

        public virtual FixtureAccount GetFixtureAccountByCode(string Code)
        {
            return Query<FixtureAccount>().Where(P => P.Code == Code).ToList(null, new EagerLoadOptions().LoadWithViewProperty()).FirstOrDefault();
        }

        /// <summary>
        /// 根据盘点计划id获取盘点计划范围（备件）
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        public virtual InventoryPlanSparePart GetInventoryPlanSparePart(double planId)
        {
            return Query<InventoryPlanSparePart>().Where(p => p.InventoryPlanId == planId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 获取设备清单
        /// </summary>
        /// <param name="inventoryPlanId"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<EquipmentList> GetEquipmentLists(double inventoryPlanId, PagingInfo pagingInfo =null)
        {
            var query = Query<EquipmentList>().Where(p => p.InventoryPlanId == inventoryPlanId);
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取设备清单Id
        /// </summary>
        /// <param name="inventoryPlanId"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual  List<double> GetEquipmentListIds(double inventoryPlanId)
        {
            var query = Query<EquipmentList>().Where(p => p.InventoryPlanId == inventoryPlanId).Select(p => p.EquipAccoutId);
            return query.ToList<double>().ToList(); ;
        }

        /// <summary>
        /// 删除设备列表
        /// </summary>
        /// <param name="selectIds"></param>
        /// <exception cref="ValidationException"></exception>
        public virtual void DeleteEquipList(List<double> selectIds)
        {
            var equipmentLists = selectIds.SplitContains(tempId =>
            {
                return Query<EquipmentList>().Where(p => tempId.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            if (equipmentLists.Count == 0)
            {
                throw new ValidationException("数据异常，删除失败".L10N());
            }
            equipmentLists.ForEach(item =>
            {
                item.PersistenceStatus = PersistenceStatus.Deleted;
            });
            RF.Save(equipmentLists);
        }

        /// <summary>
        /// 获取备件清单
        /// </summary>
        /// <param name="inventoryPlanId"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<SparePartList> GetSpartLists(double inventoryPlanId, PagingInfo pagingInfo = null)
        {
            var query = Query<SparePartList>().Where(p => p.InventoryPlanId == inventoryPlanId);
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }



        /// <summary>
        /// 删除备件列表
        /// </summary>
        /// <param name="selectIds"></param>
        /// <exception cref="ValidationException"></exception>
        public virtual void DeleteSpareList(List<double> selectIds)
        {
            var spareParts = selectIds.SplitContains(tempId =>
            {
                return Query<SparePartList>().Where(p => tempId.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            if (spareParts.Count == 0)
            {
                throw new ValidationException("数据异常，删除失败".L10N());
            }
            spareParts.ForEach(item =>
            {
                item.PersistenceStatus = PersistenceStatus.Deleted;
            });
            RF.Save(spareParts);
        }

        #region 离线盘点
        /// <summary>
        /// 通过盘点计划ID获取设备盘点范围
        /// </summary>
        /// <param name="InvPlanIds">盘点计划Ids</param>
        /// <returns></returns>
        public virtual EntityList<InventoryPlanEquipment> GetInventoryPlanEquipmentByIds(List<double> InvPlanIds)
        {

            return InvPlanIds.SplitContains(ids =>
            {
                return Query<InventoryPlanEquipment>().Where(m => ids.Contains(m.InventoryPlanId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 通过盘点计划ID获取设备盘点范围
        /// </summary>
        /// <param name="InvPlanIds">盘点计划Ids</param>
        /// <returns></returns>
        public virtual EntityList<InventoryPlanFixture> GetInventoryPlanFixtureByIds(List<double> InvPlanIds)
        {

            return InvPlanIds.SplitContains(ids =>
            {
                return Query<InventoryPlanFixture>().Where(m => ids.Contains(m.InventoryPlanId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }
        #endregion
    }
}
