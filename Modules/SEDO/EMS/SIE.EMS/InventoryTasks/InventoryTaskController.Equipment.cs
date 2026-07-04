using MailKit.Net.Smtp;
using MimeKit;
using SIE.Common.Configs;
using SIE.Common.Import;
using SIE.Common.Sender;
using SIE.Domain;
using SIE.Domain.Query;
using SIE.Domain.Validation;
using SIE.EMS.Enums;
using SIE.EMS.InventoryPlans;
using SIE.EMS.InventoryTasks.ViewModels;
using SIE.Equipments;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Security;
using SIE.Senders;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.EMS.InventoryTasks
{
    public partial class InventoryTaskController : DomainController
    {

        /// <summary>
        /// 查询设备清单
        /// </summary>
        /// <param name="task">盘点任务</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="sortInfo">排序</param>
        /// <returns>设备清单</returns>
        public virtual EntityList<InventoryTaskEquipment> GetTaskEquipments(InventoryTask task, PagingInfo pagingInfo, IList<OrderInfo> sortInfo)
        {
            var list = new EntityList<InventoryTaskEquipment>();
            var curId = RT.IdentityId;
            var counter = Query<InventoryTaskCounter>().Where(p => p.InventoryTaskId == task.Id && p.EmployeeId == curId).FirstOrDefault();
            //如果当前用户为创建人或这在【盘点人】的盘点范围为【所有设备】则可以查询到所有设备；
            if (task.CreateBy == curId || (counter != null && counter.InventoryScope == InventoryScope.All))
            {
                list = Query<InventoryTaskEquipment>().Where(p => p.InventoryTaskId == task.Id)
               .OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            }
            //如果当前用户为【盘点人】的盘点范围为【自有】管理，则可以查询到设备当前的【使用责任人】或【初盘人】为当前用户的设备
            if (counter != null && counter.InventoryScope == InventoryScope.Own)
            {
                list = Query<InventoryTaskEquipment>().Where(p => p.InventoryTaskId == task.Id &&
               (p.EquipAccount.UserId == curId || p.FirstCounterId == curId))
               .OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            }
            //用户在盘点人中有初盘/复盘权限才可编辑
            if (counter != null)
            {
                list.ForEach(p =>
                {
                    p.FirstPower = counter.First;
                    p.SecondPower = counter.Second;
                });
            }
            else
            {
                list.ForEach(p =>
                {
                    p.FirstPower = false;
                    p.SecondPower = false;
                });
            }
            return list;
        }

        /// <summary>
        /// 获取任务盘点设备清单
        /// </summary>
        /// <param name="taskIds">任务id列表</param>
        /// <returns>任务盘点设备清单</returns>
        public virtual EntityList<InventoryTaskEquipment> GetInventoryEquipByTaskIds(List<double> taskIds)
        {
            return taskIds.SplitContains(ids => Query<InventoryTaskEquipment>().Where(p => ids.Contains(p.InventoryTaskId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
        }

        /// <summary>
        /// 获取盘点任务设备清单信息
        /// </summary>
        /// <param name="planIds">计划id列表</param>
        /// <returns>盘点任务设备清单信息</returns>
        public virtual IList<InventoryTaskEquipmentInfo> GetInventoryEquipByPlanIds(List<double> planIds)
        {
            var list = Query<InventoryTaskEquipment>().Join<InventoryTask>((a, b) => a.InventoryTaskId == b.Id && planIds.Contains(b.InventoryPlanId))
                .Select<InventoryTask>((a, b) => new
                {
                    InventoryPlanId = b.InventoryPlanId,
                    InventoryTaskId = a.InventoryTaskId,
                    InventoryAssetSource = a.InventoryAssetSource,
                    FirstInventoryResult = a.FirstInventoryResult
                }).ToList<InventoryTaskEquipmentInfo>();
            return list;
        }

        /// <summary>
        /// 保存设备盘点
        /// </summary>
        /// <param name="taskList"></param>
        private void SaveEquipTaskList(EntityList<InventoryTask> taskList)
        {
            var taskIds = taskList.Select(p => p.Id).ToList();
            var oldEquips = GetInventoryEquipByTaskIds(taskIds);
            var now = RF.Find<InventoryTask>().GetDbTime();
            foreach (var task in taskList)
            {
                foreach (var newEquip in task.InventoryTaskEquipmentList)
                {
                    //校验实盘设备字段
                    CheckRealEquip(newEquip);

                    //初盘结果或复盘结果有值时，更新为【已盘点】；都没值时更新为【未盘点】
                    if (newEquip.FirstInventoryResult.HasValue || newEquip.SecondInventoryResult.HasValue)
                    {
                        newEquip.InventoryStatus = InventoryStatus.Done;
                    }
                    else
                    {
                        newEquip.InventoryStatus = InventoryStatus.Not;
                    }
                    //初盘结果修改时，除了保存修改的字段，还要更新初盘人和初盘时间；
                    var oldEquip = oldEquips.FirstOrDefault(p => p.Id == newEquip.Id);
                    if (oldEquip != null && oldEquip.FirstInventoryResult != newEquip.FirstInventoryResult)
                    {
                        newEquip.FirstCounterId = RT.IdentityId;
                        newEquip.InventoryDateTime = now;
                    }
                    //复盘结果修改时，除了保存修改的字段，还要更新复盘人和复盘时间
                    if (oldEquip != null && oldEquip.SecondInventoryResult != newEquip.SecondInventoryResult)
                    {
                        newEquip.SecondCounterId = RT.IdentityId;
                        newEquip.SecondDateTime = now;
                    }
                }
                //主表盘点状态为【初盘完成】时，更新为【复盘中】
                if (task.InventoryTaskStatus == InventoryTaskStatus.FirstDone)
                {
                    task.InventoryTaskStatus = InventoryTaskStatus.ScondDoing;
                }
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //保存界面数据
                RF.Save(taskList);

                //更新盘点任务和盘点计划的盘点进度
                UpdatePercentage(taskList);

                //保存时，盘点责任人是否存在于盘点人中，存在则更新初盘、复盘都勾选，盘点范围为【所有设备】；不存在则新增一条数据
                var allCounters = GetTaskCountersByTaskIds(taskIds);
                UpdateTaskCounters(taskList, allCounters);

                //提交事务
                trans.Complete();
            }
        }

        /// <summary>
        /// 校验实盘设备字段
        /// </summary>
        /// <param name="newEquip">设备清单</param>
        private void CheckRealEquip(InventoryTaskEquipment newEquip)
        {
            var result = newEquip.SecondInventoryResult.HasValue ? newEquip.SecondInventoryResult : newEquip.FirstInventoryResult;
            if (result == InventoryResult.InfoChange || result == InventoryResult.Profit)
            {
                //以下字段盘盈和信息变动的时候不能为空
                if (newEquip.RealManageDeptId == null)
                {
                    throw new ValidationException("设备：{0}实盘管理部门不能为空".L10nFormat(newEquip.EquipmentCode));
                }
                if (newEquip.RealUseDeptId == null)
                {
                    throw new ValidationException("设备：{0}实盘使用部门不能为空".L10nFormat(newEquip.EquipmentCode));
                }
                if (newEquip.AccountUseState == null)
                {
                    throw new ValidationException("设备：{0}实盘管理状态不能为空".L10nFormat(newEquip.EquipmentCode));
                }
                if (newEquip.AccountState == null)
                {
                    throw new ValidationException("设备：{0}实盘设备状态不能为空".L10nFormat(newEquip.EquipmentCode));
                }
              /*  if (newEquip.UserId == null)
                {
                    throw new ValidationException("设备：{0}实盘使用责任人不能为空".L10nFormat(newEquip.EquipmentCode));
                }*/
            }
            //车间和仓库不能同时有值
            if (newEquip.RealWorkShopId.HasValue && newEquip.RealWarehouseId.HasValue)
            {
                throw new ValidationException("车间和仓库不能同时有值".L10N());
            }
        }

        /// <summary>
        /// 更新设备盘点任务和盘点计划的盘点进度
        /// </summary>
        /// <param name="taskList">盘点任务</param>
        private void UpdatePercentage(EntityList<InventoryTask> taskList)
        {
            var planIds = taskList.Select(p => p.InventoryPlanId).Distinct().ToList();
            var plans = RT.Service.Resolve<InventoryPlanController>().GetInventoryPlansByIds(planIds);
            var allEquips = GetInventoryEquipByPlanIds(planIds);
            foreach (var task in taskList)
            {
                //这个盘点单下所有来源为【账内资产】的数据中，初盘结果有值的数量除以所有来源为【账内资产】的数量，保留2位小数
                decimal allQty = allEquips.Count(p => p.InventoryTaskId == task.Id && p.InventoryAssetSource == InventoryAssetSource.Account);
                decimal qty = allEquips.Count(p => p.InventoryTaskId == task.Id && p.InventoryAssetSource == InventoryAssetSource.Account && p.FirstInventoryResult != null);
                if (allQty > 0)
                {
                    var percentage = Math.Floor(qty / allQty * 100);
                    DB.Update<InventoryTask>().Set(p => p.Percentage, percentage).Where(p => p.Id == task.Id).Execute();
                }
            }
            foreach (var plan in plans)
            {
                //盘点计划关联的盘点单下所有来源为【账内资产】的数据中，初盘结果有值的数量除以所有来源为【账内资产】的数量，保留2位小数
                decimal allQty = allEquips.Count(p => p.InventoryPlanId == plan.Id && p.InventoryAssetSource == InventoryAssetSource.Account);
                decimal qty = allEquips.Count(p => p.InventoryPlanId == plan.Id && p.InventoryAssetSource == InventoryAssetSource.Account && p.FirstInventoryResult != null);
                if (allQty > 0)
                {
                    var percentage = Math.Floor(qty / allQty * 100);
                    DB.Update<InventoryPlan>().Set(p => p.Percentage, percentage).Where(p => p.Id == plan.Id).Execute();
                }
            }
        }

        #region 导入盘点任务设备清单
        /// <summary>
        /// 导入盘点任务设备清单
        /// </summary>
        /// <param name="batch">导入数据</param>
        /// <returns>导入返回信息</returns>
        public virtual List<ImportMessageResult> ImportTaskEquip(IList<RowData> batch)
        {
            if (batch == null)
            {
                throw new ValidationException("数据异常".L10N());
            }
            List<ImportMessageResult> messageList = new List<ImportMessageResult>();
            var taskEquips = batch.Select(p => p.Entity as InventoryTaskEquipment).ToList();
            var taskIds = taskEquips.Select(p => p.InventoryTaskId).Distinct().ToList();
            //校验导入信息
            var tuple = CheckImportTask(messageList, taskEquips, taskIds);
            var task = tuple.Item1;
            if (messageList.Any())
            {
                return messageList;
            }
            #region 获取数据
            //盘点任务工厂下的部门id
            var departmentIds = Query<Enterprise>().Where(p => p.Level.Type == EnterpriseType.Department && p.InvOrgId == RT.InvOrg && p.TreePId == task.FactoryId)
                .Select(p => p.Id).ToList();
            //盘点任务工厂下的车间
            var workshops = RT.Service.Resolve<EnterpriseController>().GetWorkShops(null, string.Empty, task.FactoryId);
            //盘点任务的实盘产线
            var resourceIds = taskEquips.Where(p => p.RealResourceId != null).Select(p => (double)p.RealResourceId).Distinct().ToList();
            var resources = RT.Service.Resolve<EnterpriseController>().GetLinesByIds(resourceIds);
            //盘点任务的实盘库位
            var storageLocationIds = taskEquips.Where(p => p.StorageLocationId != null).Select(p => (double)p.StorageLocationId).Distinct().ToList();
            var locations = RT.Service.Resolve<WarehouseController>().GetStorageLocations(storageLocationIds, null);
            //盘点任务设备类型
            var equipTypeIds = taskEquips.Where(p => p.EquipTypeId != null).Select(p => (double)p.EquipTypeId).Distinct().ToList();
            var equipTypes = RT.Service.Resolve<CoreEquipController>().GetEquipTypesByIds(equipTypeIds);
            //盘点任务设备型号
            var equipModelIds = taskEquips.Where(p => p.EquipModelId != null).Select(p => (double)p.EquipModelId).Distinct().ToList();
            var equipModels = RT.Service.Resolve<EquipModelController>().GetEquipModels(equipModelIds);
            //盘点任务的设备
            var equips = RT.Service.Resolve<EquipAccountController>().GetEquipAccountsNoLimit(tuple.Item2);
            var oldTaskEquips = GetInventoryEquipByTaskIds(taskIds);
            var now = RF.Find<InventoryTask>().GetDbTime();

            var entitList = batch.Select(m => m.Entity as InventoryTaskEquipment).ToList();
            var storageLocationCodes = entitList.Select(m => m.StorageLocationCode).Distinct().ToList();//按照库位Code加载全部库位数据
            Dictionary<double, List<StorageLocation>> StorageLocationDic = RT.Service.Resolve<WarehouseController>().GetStorageLocationByCodes(storageLocationCodes, null).GroupBy(p => p.WarehouseId).ToDictionary(p => p.Key, p => p.ToList());


            #endregion
            foreach (var row in batch)
            {
                try
                {
                    var newTaskEquip = row.Entity as InventoryTaskEquipment;
                    if (newTaskEquip.RealWarehouseId.HasValue && StorageLocationDic.ContainsKey(newTaskEquip.RealWarehouseId.Value))
                    {
                        var storageLocation = StorageLocationDic[newTaskEquip.RealWarehouseId.Value].FirstOrDefault(m => m.Code == newTaskEquip.StorageLocationCode);
                        if (storageLocation != null)
                        {
                            newTaskEquip.StorageLocationId = storageLocation.Id;
                        }
                    }

                    var oldTaskEquip = oldTaskEquips.FirstOrDefault(p => p.EquipmentCode == newTaskEquip.EquipmentCode);
                    var equip = equips.FirstOrDefault(p => p.Code == newTaskEquip.EquipmentCode);
                    //校验导入的部门车间产线仓库
                    CheckImportLevel(newTaskEquip, departmentIds, workshops, resources, locations);
                    //校验导入设备信息
                    CheckImportTaskEquip(newTaskEquip, oldTaskEquip, equip, equipTypes, equipModels);
                    using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
                    {
                        if (newTaskEquip.EquipmentCode.IsNullOrWhiteSpace())//设备编码为空
                        {
                            ProfitNewEquip(newTaskEquip, task, now);//盘盈新增设备
                        }
                        else
                        {
                            if (oldTaskEquip == null)//不存在于设备清单
                            {
                                ProfitNewEquip(newTaskEquip, task, now);//盘盈新增设备
                            }
                            else
                            {
                                //导入数据赋值到原数据
                                UpdateOldTaskEquip(oldTaskEquip, newTaskEquip);

                                //账内资产正常、盘亏时赋值
                                NormalOrLossUpdate(oldTaskEquip, newTaskEquip.ImportResult, equip);

                                //更新初盘/复盘结果
                                UpdateOldTaskEquipResult(oldTaskEquip, newTaskEquip.ImportResult, task.InventoryTaskStatus, now);

                                //保存更新数据
                                RF.Save(oldTaskEquip);
                            }
                        }
                        trans.Complete();
                    }
                    messageList.Add(new ImportMessageResult { RowNum = row.RowIndex + 1, MsgType = ImportMessageType.SaveSucess, Message = "保存成功！".L10N() });
                }
                catch (Exception exc)
                {
                    messageList.Add(new ImportMessageResult { RowNum = row.RowIndex + 1, MsgType = ImportMessageType.SaveFail, Message = exc.GetBaseException().Message });
                }
            }
            //如果盘点任务的盘点状态为【盘点中】，导入完后更新盘点任务和盘点计划的盘点进度
            if (task.InventoryTaskStatus == InventoryTaskStatus.Doing)
            {
                UpdatePercentage(new EntityList<InventoryTask> { task });
            }
            return messageList;
        }

        /// <summary>
        /// 校验导入信息
        /// </summary>
        /// <param name="messageList">导入结果信息</param>
        /// <param name="taskEquips">导入设备信息</param>
        /// <param name="taskIds">任务id</param>
        /// <returns>盘点任务、设备编码</returns>
        private Tuple<InventoryTask, List<string>> CheckImportTask(List<ImportMessageResult> messageList, List<InventoryTaskEquipment> taskEquips, List<double> taskIds)
        {
            if (taskIds.Count != 1)
            {
                messageList.Add(new ImportMessageResult { RowNum = 0, MsgType = ImportMessageType.SaveFail, Message = "一次只能导入一个盘点任务的数据".L10N() });
                return null;
            }
            var task = GetById<InventoryTask>(taskIds.FirstOrDefault());
            if (task == null)
            {
                messageList.Add(new ImportMessageResult { RowNum = 0, MsgType = ImportMessageType.SaveFail, Message = "盘点任务不存在".L10N() });
                return new Tuple<InventoryTask, List<string>>(null, new List<string> { });
            }
            //盘点状态为【盘点中、初盘完成、复盘中】才允许导入
            if (task.InventoryTaskStatus != InventoryTaskStatus.Doing && task.InventoryTaskStatus != InventoryTaskStatus.FirstDone
                && task.InventoryTaskStatus != InventoryTaskStatus.ScondDoing)
            {
                messageList.Add(new ImportMessageResult { RowNum = 0, MsgType = ImportMessageType.SaveFail, Message = "盘点状态为【盘点中、初盘完成、复盘中】才允许导入".L10N() });
            }
            var equipCodes = taskEquips.Where(p => !p.EquipmentCode.IsNullOrWhiteSpace()).Select(p => p.EquipmentCode).ToList();
            var codeGroups = equipCodes.Distinct().ToList();
            if (equipCodes.Count != codeGroups.Count)
            {
                messageList.Add(new ImportMessageResult { RowNum = 0, MsgType = ImportMessageType.SaveFail, Message = "导入的数据中，设备编码不能重复".L10N() });
            }
            return new Tuple<InventoryTask, List<string>>(task, codeGroups);
        }

        /// <summary>
        /// 校验导入的部门车间产线仓库
        /// </summary>
        /// <param name="newTaskEquip">导入设备信息</param>
        /// <param name="departmentIds">盘点任务工厂下的部门id</param>
        /// <param name="workshops">盘点任务工厂下的车间</param>
        /// <param name="resources">产线列表</param>
        /// <param name="locations">库位列表</param>
        private void CheckImportLevel(InventoryTaskEquipment newTaskEquip, EntityList<Enterprise> departmentIds, EntityList<Enterprise> workshops,
            EntityList<Enterprise> resources, EntityList<StorageLocation> locations)
        {
            if (newTaskEquip.RealManageDeptId.HasValue && !departmentIds.Any(m => m.Id == newTaskEquip.RealManageDeptId))
            {
                throw new ValidationException("实盘管理部门不是盘点任务工厂下的部门".L10N());
            }
            if (newTaskEquip.RealUseDeptId.HasValue && !departmentIds.Any(m => m.Id == newTaskEquip.RealUseDeptId))
            {
                throw new ValidationException("实盘使用部门不是盘点任务工厂下的部门".L10N());
            }
            if (newTaskEquip.RealWorkShopId.HasValue)
            {
                if (newTaskEquip.RealWarehouseId.HasValue)
                {
                    throw new ValidationException("车间和仓库不能同时有值".L10N());
                }
                if (!workshops.Any(p => p.Id == newTaskEquip.RealWorkShopId.Value))
                {
                    throw new ValidationException("实盘车间不在盘点任务工厂下".L10N());
                }
            }
            if (newTaskEquip.RealResourceId.HasValue)
            {
                if (newTaskEquip.RealWorkShopId == null)
                {
                    throw new ValidationException("实盘产线有值,车间不能为空".L10N());
                }
                var resource = resources.FirstOrDefault(p => p.Id == newTaskEquip.RealResourceId.Value);
                if (resource == null)
                {
                    throw new ValidationException("实盘产线不存在或者不是产线".L10N());
                }
                if (resource.TreePId != newTaskEquip.RealWorkShopId.Value)
                {
                    throw new ValidationException("实盘产线不在车间下".L10N());
                }
            }
            if (newTaskEquip.StorageLocationId.HasValue)
            {
                if (newTaskEquip.RealWarehouseId == null)
                {
                    throw new ValidationException("实盘库位有值,仓库不能为空".L10N());
                }
                var location = locations.FirstOrDefault(p => p.Id == newTaskEquip.StorageLocationId.Value);
                if (location == null)
                {
                    throw new ValidationException("实盘库位不存在".L10N());
                }
                if (location.WarehouseId != newTaskEquip.RealWarehouseId.Value)
                {
                    throw new ValidationException("实盘库位不在仓库下".L10N());
                }
            }
        }

        /// <summary>
        /// 校验导入设备信息
        /// </summary>
        /// <param name="newTaskEquip">导入设备信息</param>
        /// <param name="oldTaskEquip">原设备信息</param>
        /// <param name="equip">设备台账</param>
        /// <param name="equipTypes">设备类型列表</param>
        /// <param name="equipModels">设备型号列表</param>
        private void CheckImportTaskEquip(InventoryTaskEquipment newTaskEquip, InventoryTaskEquipment oldTaskEquip, EquipAccount equip,
            EntityList<SIE.Equipments.EquipTypes.EquipType> equipTypes, EntityList<EquipModel> equipModels)
        {
            //当设备编码为空或者设备编码不存在于设备台账时必输
            if (equip == null && newTaskEquip.EquipmentName.IsNullOrWhiteSpace())
            {
                throw new ValidationException("设备编码不存在于设备台账时,设备名称必输".L10N());
            }
            if (equip != null)
            {
                //设备编码存在于设备台账时
                newTaskEquip.EquipmentName = equip.Name;
                newTaskEquip.EquipAccountId = equip.Id;
                newTaskEquip.TypeCategory = equip.EquipTypeCategory;
                newTaskEquip.EquipTypeId = equip.EquipTypeViewId;
                newTaskEquip.EquipModelId = equip.EquipModelId;
                newTaskEquip.InventoryAssetSource = InventoryAssetSource.Account;

                SetEquipOldInfo( equip, newTaskEquip);

            }
            else
            {
                //当设备编码为空或者设备编码不存在于设备台账时才需要
                //设备类别不为空时校验是否在设备类别下，设备类别为空时，补全设备类别
                if (newTaskEquip.EquipTypeId.HasValue)
                {
                    var equipType = equipTypes.FirstOrDefault(p => p.Id == newTaskEquip.EquipTypeId);
                    if (equipType == null)
                    {
                        throw new ValidationException("设备类型不存在".L10N());
                    }
                }
                //设备类别或设备类型不为空时校验是否在设备类别和设备类型下，设备类别和设备类型为空时，补全设备类别和设备类型
                if (newTaskEquip.EquipModelId.HasValue)
                {
                    var equipModel = equipModels.FirstOrDefault(p => p.Id == newTaskEquip.EquipModelId);
                    if (equipModel == null)
                    {
                        throw new ValidationException("设备型号不存在".L10N());
                    }
                    if (newTaskEquip.EquipTypeId == null)
                    {
                        newTaskEquip.EquipTypeId = equipModel.EquipTypeId;
                    }
                    else
                    {
                        if (newTaskEquip.EquipTypeId != equipModel.EquipTypeId)
                        {
                            throw new ValidationException("设备型号不在设备类型下".L10N());
                        }
                    }

                    if (newTaskEquip.TypeCategory.IsNullOrWhiteSpace())
                    {
                        newTaskEquip.TypeCategory = equipModel.TypeCategory;
                    }
                    else
                    {
                        if (newTaskEquip.TypeCategory != equipModel.TypeCategory)
                        {
                            throw new ValidationException("设备型号不在设备类别下".L10N());
                        }
                    }
                }
            }
            //设备编码存在于设备清单且来源为【账内资产】时
            var check = true;
            if (oldTaskEquip != null && oldTaskEquip.InventoryAssetSource == InventoryAssetSource.Account)
            {
                if (newTaskEquip.ImportResult == null || newTaskEquip.ImportResult == InventoryResult.Profit)
                {
                    throw new ValidationException("盘点结果必输，且不能为【盘盈】".L10N());
                }
                if (newTaskEquip.ImportResult != InventoryResult.InfoChange)
                {
                    check = false;
                }
            }
            //校验必输字段
            CheckRealRequired(newTaskEquip, check);
        }

        /// <summary>
        /// 校验必输字段
        /// </summary>
        /// <param name="newTaskEquip">盘点任务设备清单</param>
        /// <param name="check">是否不能为空</param>
        private void CheckRealRequired(InventoryTaskEquipment newTaskEquip, bool check)
        {
            if (newTaskEquip.EquipmentCode.IsNullOrWhiteSpace() && newTaskEquip.EquipmentName.IsNullOrWhiteSpace())
            {
                throw new ValidationException("设备编码为空,设备名称必输".L10N());
            }
            //设备编码存在于设备清单且来源为【账内资产】且盘点状态不为【信息变动】时，用不上,其他情况必输，
            if (check)
            {
                if (newTaskEquip.RealManageDeptId == null)
                {
                    throw new ValidationException("实盘管理部门不能为空".L10N());
                }
                if (newTaskEquip.RealUseDeptId == null)
                {
                    throw new ValidationException("实盘使用部门不能为空".L10N());
                }
                if (newTaskEquip.AccountUseState == null)
                {
                    throw new ValidationException("实盘管理状态不能为空".L10N());
                }
                if (newTaskEquip.AccountState == null)
                {
                    throw new ValidationException("实盘设备状态不能为空".L10N());
                }
                if (newTaskEquip.UserId == null)
                {
                    throw new ValidationException("实盘使用责任人不能为空".L10N());
                }
            }
        }

        /// <summary>
        /// 盘盈新增设备
        /// </summary>
        /// <param name="newTaskEquip">新增设备</param>
        /// <param name="task">盘点任务</param>
        /// <param name="now">盘点时间</param>
        private void ProfitNewEquip(InventoryTaskEquipment newTaskEquip, InventoryTask task, DateTime now)
        {
            newTaskEquip.InventoryStatus = InventoryStatus.Done;
            newTaskEquip.InventoryAssetSource = InventoryAssetSource.Profit;
            if (task.InventoryTaskStatus == InventoryTaskStatus.Doing)
            {
                newTaskEquip.FirstInventoryResult = InventoryResult.Profit;
                newTaskEquip.FirstCounterId = RT.IdentityId;
                newTaskEquip.InventoryDateTime = now;
            }
            else
            {
                newTaskEquip.SecondInventoryResult = InventoryResult.Profit;
                newTaskEquip.SecondCounterId = RT.IdentityId;
                newTaskEquip.SecondDateTime = now;
            }
            RF.Save(newTaskEquip);
        }

        /// <summary>
        /// 导入数据赋值到原数据
        /// </summary>
        /// <param name="oldTaskEquip">原数据</param>
        /// <param name="newTaskEquip">导入数据</param>
        private void UpdateOldTaskEquip(InventoryTaskEquipment oldTaskEquip, InventoryTaskEquipment newTaskEquip)
        {
            oldTaskEquip.EquipmentName = newTaskEquip.EquipmentName;
            oldTaskEquip.SuggestProcessMethod = newTaskEquip.SuggestProcessMethod;
            oldTaskEquip.RealManageDeptId = newTaskEquip.RealManageDeptId;
            oldTaskEquip.RealUseDeptId = newTaskEquip.RealUseDeptId;
            oldTaskEquip.AccountUseState = newTaskEquip.AccountUseState;
            oldTaskEquip.AccountState = newTaskEquip.AccountState;
            oldTaskEquip.UserId = newTaskEquip.UserId;
            oldTaskEquip.RealWorkShopId = newTaskEquip.RealWorkShopId;
            oldTaskEquip.RealResourceId = newTaskEquip.RealResourceId;
            oldTaskEquip.RealWarehouseId = newTaskEquip.RealWarehouseId;
            oldTaskEquip.StorageLocationId = newTaskEquip.StorageLocationId;
            oldTaskEquip.RealLocation = newTaskEquip.RealLocation;
            oldTaskEquip.TypeCategory = newTaskEquip.TypeCategory;
            oldTaskEquip.EquipTypeId = newTaskEquip.EquipTypeId;
            oldTaskEquip.EquipModelId = newTaskEquip.EquipModelId;
        }

        /// <summary>
        /// 账内资产正常、盘亏时赋值
        /// </summary>
        /// <param name="oldTaskEquip">原数据</param>
        /// <param name="newResult">盘点结果</param>
        /// <param name="equip">设备台账</param>
        private void NormalOrLossUpdate(InventoryTaskEquipment oldTaskEquip, InventoryResult? newResult, EquipAccount equip)
        {
            if (oldTaskEquip.InventoryAssetSource == InventoryAssetSource.Account)
            {
                if (newResult == InventoryResult.Normal && equip != null)//正常
                {
                    oldTaskEquip.RealManageDeptId = equip.ManageDepartmentId;
                    oldTaskEquip.RealUseDeptId = equip.UseDepartmentId;
                    oldTaskEquip.AccountUseState = equip.UseState;
                    oldTaskEquip.AccountState = equip.State;
                    oldTaskEquip.UserId = equip.UserId;
                    oldTaskEquip.RealWorkShopId = equip.WorkShopId;
                    oldTaskEquip.RealResourceId = equip.ResourceId;
                    oldTaskEquip.RealWarehouseId = equip.WarehouseId;
                    oldTaskEquip.StorageLocationId = equip.StorageLocationId;
                    oldTaskEquip.RealLocation = equip.InstallationLocation;
                }
                if (newResult == InventoryResult.Loss)//盘亏
                {
                    oldTaskEquip.RealManageDeptId = null;
                    oldTaskEquip.RealUseDeptId = null;
                    oldTaskEquip.AccountUseState = null;
                    oldTaskEquip.AccountState = null;
                    oldTaskEquip.UserId = null;
                    oldTaskEquip.RealWorkShopId = null;
                    oldTaskEquip.RealResourceId = null;
                    oldTaskEquip.RealWarehouseId = null;
                    oldTaskEquip.StorageLocationId = null;
                    oldTaskEquip.RealLocation = "";
                }
            }
        }

        /// <summary>
        /// 更新初盘/复盘结果
        /// </summary>
        /// <param name="oldTaskEquip">原数据</param>
        /// <param name="newResult">盘点结果</param>
        /// <param name="taskStatus">盘点任务状态</param>
        /// <param name="now">盘点时间</param>
        private void UpdateOldTaskEquipResult(InventoryTaskEquipment oldTaskEquip, InventoryResult? newResult, InventoryTaskStatus taskStatus, DateTime now)
        {
            if (taskStatus == InventoryTaskStatus.Doing)//初盘
            {
                if (oldTaskEquip.InventoryAssetSource == InventoryAssetSource.Profit)
                {
                    oldTaskEquip.FirstInventoryResult = InventoryResult.Profit;
                }
                else
                {
                    oldTaskEquip.InventoryStatus = InventoryStatus.Done;
                    oldTaskEquip.FirstInventoryResult = newResult;
                }
                oldTaskEquip.FirstCounterId = RT.IdentityId;
                oldTaskEquip.InventoryDateTime = now;
            }
            else//复盘
            {
                if (oldTaskEquip.InventoryAssetSource == InventoryAssetSource.Profit)
                {
                    oldTaskEquip.SecondInventoryResult = InventoryResult.Profit;
                }
                else
                {
                    oldTaskEquip.SecondInventoryResult = newResult;
                }
                oldTaskEquip.SecondCounterId = RT.IdentityId;
                oldTaskEquip.SecondDateTime = now;
            }
        }
        #endregion
    }
}
