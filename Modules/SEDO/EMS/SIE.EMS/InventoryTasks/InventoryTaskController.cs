using MailKit.Net.Smtp;
using MimeKit;
using SIE.Common.Configs;
using SIE.Common.Sender;
using SIE.Domain;
using SIE.Domain.Query;
using SIE.Domain.Validation;
using SIE.EMS.Enums;
using SIE.EMS.InventoryPlans;
using SIE.EMS.InventoryTasks.ViewModels;
using SIE.Equipments.EquipAccounts;
using SIE.Resources.Employees;
using SIE.Security;
using SIE.Senders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.EMS.InventoryTasks
{
    /// <summary>
    /// 盘点任务控制器
    /// </summary>
    public partial class InventoryTaskController : DomainController
    {
        /// <summary>
        /// 查询盘点任务
        /// </summary>
        /// <param name="criteria">盘点任务查询</param>
        /// <returns>盘点任务</returns>
        public virtual EntityList CriteriaInventoryTasks(InventoryTaskCriteria criteria)
        {
            var query = Query<InventoryTask>();
            if (!criteria.InventoryAssetObject.HasValue)
            {
                throw new ValidationException("请先选择资产对象".L10N());
            }
            query.Where(p => p.InventoryPlan.InventoryAssetObject == criteria.InventoryAssetObject);
            if (!criteria.TaskNo.IsNullOrWhiteSpace())
            {
                query.Where(p => p.TaskNo.Contains(criteria.TaskNo));
            }
            if (criteria.FactoryId.HasValue)
            {
                query.Where(p => p.FactoryId == criteria.FactoryId.Value);
            }
            if (!criteria.PlanNo.IsNullOrWhiteSpace())
            {
                query.Where(p => p.InventoryPlan.PlanNo.Contains(criteria.PlanNo));
            }
            if (!criteria.InventoryType.IsNullOrWhiteSpace())
            {
                query.Where(p => p.InventoryType == criteria.InventoryType);
            }
            if (criteria.ResponsibleId.HasValue)
            {
                query.Where(p => p.ResponsibleId == criteria.ResponsibleId.Value);
            }
            if (criteria.InventoryTaskStatus.HasValue)
            {
                query.Where(p => p.InventoryTaskStatus == criteria.InventoryTaskStatus.Value);
            }
            if (criteria.CreateDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue.Value);
            }
            if (criteria.CreateDate.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue.Value);
            }

            var iquery = query.OrderBy(criteria.OrderInfoList).ToQuery();

            IColumnNode idColumn = iquery.MainTable.FindColumn(InventoryTask.IdProperty);
            IColumnNode responsibleIdColumn = iquery.MainTable.FindColumn(InventoryTask.ResponsibleIdProperty);

            //只能查询创建人为当前用户或者盘点人页签存在当前用户的数据
      /*      var curId = RT.IdentityId;

            var f = QueryFactory.Instance;

            switch (criteria.InventoryAssetObject)
            {
                case InventoryAssetObject.Equipment:
                    {
                        var subQuery = DB.Query<InventoryTaskCounter>("cnt")                            
                            .Where(cnt => cnt.EmployeeId == curId )
                            .ToQuery();
                        IColumnNode inventoryTaskIdColumn = subQuery.MainTable.FindColumn(InventoryTaskCounter.InventoryTaskIdProperty);
                        subQuery.Where = subQuery.Where.And(f.Constraint(idColumn, inventoryTaskIdColumn));

                        iquery.Where = f.And(iquery.Where, f.Or(f.Exists(subQuery), f.Constraint(responsibleIdColumn, curId)));
                    }
                    break;
                case InventoryAssetObject.Spare:
                    {
                        var subQuery = DB.Query<InventoryTaskSparePartCounter>("cnt")
                            .Where(cnt => cnt.EmployeeId == curId)
                            .ToQuery();
                        IColumnNode inventoryTaskIdColumn = subQuery.MainTable.FindColumn(InventoryTaskSparePartCounter.InventoryTaskIdProperty);
                        subQuery.Where = subQuery.Where.And(f.Constraint(idColumn, inventoryTaskIdColumn));

                        iquery.Where = f.And(iquery.Where, f.Or(f.Exists(subQuery), f.Constraint(responsibleIdColumn, curId)));
                    }
                    break;
                case InventoryAssetObject.Fixture:
                    {
                        var subQuery = DB.Query<InventoryTaskFixtureCounter>("cnt")
                            .Where(cnt => cnt.EmployeeId == curId)
                            .ToQuery();
                        IColumnNode inventoryTaskIdColumn = subQuery.MainTable.FindColumn(InventoryTaskFixtureCounter.InventoryTaskIdProperty);
                        subQuery.Where = subQuery.Where.And(f.Constraint(idColumn, inventoryTaskIdColumn));

                        iquery.Where = f.And(iquery.Where, f.Or(f.Exists(subQuery), f.Constraint(responsibleIdColumn, curId)));                        
                    }
                    break;
                default:
                    break;
            }*/
            
            return query.Repository.QueryList(iquery, criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据id列表获取盘点任务
        /// </summary>
        /// <param name="taskIds">id列表</param>
        /// <returns>盘点任务</returns>
        public virtual EntityList<InventoryTask> GetInventoryTasksByIds(List<double> taskIds)
        {
            return taskIds.SplitContains(ids => Query<InventoryTask>().Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
        }

        /// <summary>
        /// 根据计划id获取盘点任务
        /// </summary>
        /// <param name="planId">计划id</param>
        /// <param name="sortInfo">排序</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>盘点任务</returns>
        public virtual EntityList<InventoryTask> GetInventoryTaskByPlanId(double planId, IList<OrderInfo> sortInfo, PagingInfo pagingInfo)
        {
            return Query<InventoryTask>().Where(p => p.InventoryPlanId == planId)
                .OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取任务盘点人列表
        /// </summary>
        /// <param name="taskIds">任务id列表</param>
        /// <returns>任务盘点人列表</returns>
        public virtual EntityList<InventoryTaskCounter> GetTaskCountersByTaskIds(List<double> taskIds)
        {
            return taskIds.SplitContains(ids => Query<InventoryTaskCounter>().Where(p => ids.Contains(p.InventoryTaskId)).ToList());
        }



        /// <summary>
        /// 保存盘点任务
        /// </summary>
        /// <param name="taskList">盘点任务</param>
        public virtual void SaveInventoryTaskList(EntityList<InventoryTask> taskList)
        {
            if (taskList == null)
            {
                throw new ValidationException("数据异常".L10N());
            }
            //设备盘点任务
            var equipmentTaskList = taskList.Where(m => m.InventoryPlan.InventoryAssetObject == InventoryAssetObject.Equipment).ToList();
            if (equipmentTaskList.Any())
            {
                var elist = new EntityList<InventoryTask>();
                elist.AddRange(equipmentTaskList);
                SaveEquipTaskList(elist);
            }

            //工治具盘点任务
            var fixtureTaskList = taskList.Where(m => m.InventoryPlan.InventoryAssetObject == InventoryAssetObject.Fixture).ToList();
            if (fixtureTaskList.Any())
            {
                var elist = new EntityList<InventoryTask>();
                elist.AddRange(fixtureTaskList);
                SaveFixtureTaskList(elist);
            }

            //备件盘点任务
            var sparePartTaskList = taskList.Where(m => m.InventoryPlan.InventoryAssetObject == InventoryAssetObject.Spare).ToList();
            if (sparePartTaskList.Any())
            {
                var elist = new EntityList<InventoryTask>();
                elist.AddRange(sparePartTaskList);
                RT.Service.Resolve<InventoryTaskSpartPartController>().SaveSparePartTaskList(elist);
            }
        }

        /// <summary>
        /// 保存时，盘点责任人是否存在于盘点人中，存在则更新初盘、复盘都勾选，盘点范围为【所有设备】；不存在则新增一条数据
        /// </summary>
        /// <param name="taskList">盘点任务</param>
        /// <param name="allCounters">盘点人</param>
        private void UpdateTaskCounters(EntityList<InventoryTask> taskList, EntityList<InventoryTaskCounter> allCounters)
        {
            foreach (var task in taskList)
            {
                var oldCounter = allCounters.FirstOrDefault(p => p.InventoryTaskId == task.Id && p.EmployeeId == task.ResponsibleId);
                if (oldCounter != null)
                {
                    oldCounter.First = true;
                    oldCounter.Second = true;
                    oldCounter.InventoryScope = InventoryScope.All;
                    RF.Save(oldCounter);
                }
                else
                {
                    if (task.ResponsibleId.HasValue)
                    {
                        var newCounter = new InventoryTaskCounter();
                        newCounter.EmployeeId = task.ResponsibleId.Value;
                        newCounter.InventoryTaskId = task.Id;
                        newCounter.First = true;
                        newCounter.Second = true;
                        newCounter.InventoryScope = InventoryScope.All;
                        RF.Save(newCounter);
                    }
                }
            }
        }

        /// <summary>
        /// 下达
        /// </summary>
        /// <param name="taskId">任务id</param>
        public virtual void ReleaseTask(double taskId)
        {
            var task = GetById<InventoryTask>(taskId);
            if (task == null)
            {
                throw new ValidationException("找不到此盘点任务，数据异常".L10N());
            }
            //盘点状态为【未开始】才能点击
            if (task.InventoryTaskStatus != InventoryTaskStatus.NotBegin)
            {
                throw new ValidationException("盘点状态为【未开始】才能下达".L10N());
            }
            //更新盘点状态为【盘点中】
            task.InventoryTaskStatus = InventoryTaskStatus.Doing;
            RF.Save(task);

            //给有初盘权限的盘点人发送邮件,不需要开事务，邮件失败不影响下达成功
            MailNoticeCounter(task);
        }

        /// <summary>
        /// 给有初盘权限的盘点人发送邮件
        /// </summary>
        /// <param name="task">盘点任务</param>
        private void MailNoticeCounter(InventoryTask task)
        {
            var config = GetEmailSenderConfig();
            var allTaskEquips = GetInventoryEquipByTaskIds(new List<double> { task.Id });
            var counters = Query<InventoryTaskCounter>().Where(p => p.InventoryTaskId == task.Id && p.First).ToList();
            //所有设备
            var allCounters = counters.Where(p => p.InventoryScope == InventoryScope.All).ToList();
            if (allCounters.Any())
            {
                var employeeIds = allCounters.Select(p => p.EmployeeId).ToList();
                var employees = Query<Employee>().Where(p => employeeIds.Contains(p.Id) && p.Email != "").ToList();
                if (employees.Any())
                {
                    MailNotice(config, task, allTaskEquips.ToList(), employees.ToList());
                }
            }
            //自有设备
            var ownCounters = counters.Where(p => p.InventoryScope == InventoryScope.Own).ToList();
            if (ownCounters.Any())
            {
                var employeeIds = ownCounters.Select(p => p.EmployeeId).ToList();
                var employees = Query<Employee>().Where(p => employeeIds.Contains(p.Id) && p.Email != "").ToList();
                foreach (var employee in employees)
                {
                    var taskEquips = allTaskEquips.Where(p => p.UserId == employee.Id || p.OldUserId == employee.Id).ToList();
                    if (!taskEquips.Any())
                    {
                        continue;
                    }
                    MailNotice(config, task, taskEquips, new List<Employee> { employee });
                }
            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="config">邮件推送配置</param>
        /// <param name="task">盘点任务</param>
        /// <param name="taskEquips">盘点任务设备清单</param>
        /// <param name="employees">员工列表</param>
        private void MailNotice(EmailSenderConfig config, InventoryTask task, List<InventoryTaskEquipment> taskEquips, List<Employee> employees)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(config.SendFrom));
            foreach (var employee in employees)
            {
                MailboxAddress mailboxAddress = new MailboxAddress(employee.Email) { Encoding = System.Text.Encoding.UTF8 };
                mimeMessage.To.Add(mailboxAddress);
            }
            mimeMessage.Subject = "盘点任务下达".L10N();
            var bodyBuilder = new BodyBuilder();
            var message = new StringBuilder();
            message.Append("您有待完成的盘点任务，任务单号：{0}，盘点类型：{1}，盘点说明：{2}，计划完成日期：{3}。设备清单如下："
                .L10nFormat(task.TaskNo, task.InventoryExecuteType.ToLabel(), task.Remark, task.PlanEndDate));
            message.Append("<br/>");
            message.Append("<table border='1'>");
            message.Append("<tr><th>序号</th><th>设备编码</th><th>设备名称</th></tr>");
            int i = 1;
            foreach (var item in taskEquips)
            {
                message.Append("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr> ".L10nFormat(i, item.EquipmentCode, item.EquipmentName));
                i++;
            }
            message.Append("</table>");
            bodyBuilder.HtmlBody = message.ToString();
            mimeMessage.Body = bodyBuilder.ToMessageBody();
            try
            {
                using (var client = new SmtpClient())
                {
#pragma warning disable S4830 // Server certificates should be verified during SSL/TLS connections
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
#pragma warning restore S4830 // Server certificates should be verified during SSL/TLS connections
                    client.Connect(config.Host, config.Port, config.EnableSSL);
                    client.Authenticate(config.SendFrom, config.Password);
                    client.Send(mimeMessage);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                throw new ValidationException("邮件发送失败：{0}".L10nFormat(ex.Message));
            }
            finally
            {
                if (mimeMessage.To != null)
                    mimeMessage.To.Clear();
                if (mimeMessage.Cc != null)
                    mimeMessage.Cc.Clear();
                if (mimeMessage.Bcc != null)
                    mimeMessage.Bcc.Clear();
                mimeMessage.Subject = string.Empty;
                mimeMessage.Body = null;
            }
        }

        /// <summary>
        /// 获取邮件参数
        /// </summary>
        /// <returns>邮件参数</returns>
        private EmailSenderConfig GetEmailSenderConfig()
        {
            var pushPlug = Query<PushPlug>().Where(p => p.PushClass == "SIE.Senders.EmailSender").FirstOrDefault();
            if (pushPlug == null)
                throw new ValidationException("找不到【邮件】的推送模块管理信息".L10N());
            var config = ConfigValueSerializer.DeserializeObject(pushPlug.Config, typeof(EmailSenderConfig)) as EmailSenderConfig;
            if (config == null)
                throw new ValidationException("【邮件】的推送模块管理信息异常".L10N());
            if (config.Host.IsNullOrWhiteSpace())
                throw new ValidationException("【邮件】的服务器未配置".L10N());
            if (config.SendFrom.IsNullOrWhiteSpace())
                throw new ValidationException("【邮件】的发件邮箱未配置".L10N());
            if (config.Password.IsNullOrWhiteSpace())
                throw new ValidationException("【邮件】的密码未配置".L10N());
            config.Password = Encoding.UTF8.GetString(SecurityManager.Decrypt(config.Password));
            return config;
        }

        /// <summary>
        /// 盘点任务下达邮件Job
        /// </summary>
        public virtual void ReleaseTaskJob()
        {
            //获取邮件配置
            var config = GetEmailSenderConfig();
            //获取盘点状态为【盘点中】的盘点任务，给有初盘权限的盘点人发送邮件
            var tasks = Query<InventoryTask>().Where(p => p.InventoryTaskStatus == InventoryTaskStatus.Doing).ToList();
            var taskIds = tasks.Select(p => p.Id).ToList();
            var allTaskEquips = GetInventoryEquipByTaskIds(taskIds);
            var counters = Query<InventoryTaskCounter>().Where(p => taskIds.Contains(p.InventoryTaskId) && p.First).ToList();
            var allEmployeeIds = counters.Select(p => p.EmployeeId).ToList();
            var allEmployees = Query<Employee>().Where(p => allEmployeeIds.Contains(p.Id)).ToList();
            foreach (var task in tasks)
            {
                //所有设备
                var allCounters = counters.Where(p => p.InventoryTaskId == task.Id && p.InventoryScope == InventoryScope.All).ToList();
                if (allCounters.Any())
                {
                    var employeeIds = allCounters.Select(p => p.EmployeeId).ToList();
                    var employees = allEmployees.Where(p => employeeIds.Contains(p.Id) && p.Email != "").ToList();
                    if (employees.Any())
                    {
                        var taskEquips = allTaskEquips.Where(p => p.InventoryTaskId == task.Id).ToList();
                        MailNotice(config, task, taskEquips, employees);
                    }
                }
                //自有设备
                var ownCounters = counters.Where(p => p.InventoryTaskId == task.Id && p.InventoryScope == InventoryScope.Own).ToList();
                if (ownCounters.Any())
                {
                    var employeeIds = ownCounters.Select(p => p.EmployeeId).ToList();
                    var employees = allEmployees.Where(p => employeeIds.Contains(p.Id) && p.Email != "").ToList();
                    foreach (var employee in employees)
                    {
                        var taskEquips = allTaskEquips.Where(p => p.InventoryTaskId == task.Id && (p.UserId == employee.Id || p.OldUserId == employee.Id)).ToList();
                        if (!taskEquips.Any())
                        {
                            continue;
                        }
                        MailNotice(config, task, taskEquips, new List<Employee> { employee });
                    }
                }
            }
        }

        /// <summary>
        /// 初盘完成
        /// </summary>
        /// <param name="taskIds">任务id</param>
        public virtual void FirstComplete(List<double> taskIds)
        {
            var tasks = GetInventoryTasksByIds(taskIds);
            //盘点状态为【盘点中】才能点击
            if (tasks.Any(p => p.InventoryTaskStatus != InventoryTaskStatus.Doing))
            {
                throw new ValidationException("盘点状态为【盘点中】才能初盘完成".L10N());
            }
            //校验主表的盘点进度是否为【100%】，否则报错：盘点未完成，存在数据缺少盘点结果；
            var task = tasks.FirstOrDefault();
            if (task.InventoryPlan.InventoryAssetObject == InventoryAssetObject.Equipment && tasks.SelectMany(p => p.InventoryTaskEquipmentList).Any(p => p.InventoryStatus !=  InventoryStatus.Done))
            {
                throw new ValidationException("盘点未完成，存在状态为【未盘点】的设备".L10N());
            }
            if (task.InventoryPlan.InventoryAssetObject == InventoryAssetObject.Spare && tasks.SelectMany(p => p.InventoryTaskSparePartDetailList).Any(p => p.InventoryStatus != InventoryStatus.Done))
            {
                throw new ValidationException("盘点未完成，存在状态为【未盘点】的备件".L10N());
            }
            if (task.InventoryPlan.InventoryAssetObject == InventoryAssetObject.Fixture && tasks.SelectMany(p => p.InventoryTaskFixtureEncodeList).Any(p => p.InventoryStatus != InventoryStatus.Done))
            {
                throw new ValidationException("盘点未完成，存在状态为【未盘点】的工治具".L10N());
            }
            //校验通过更新状态为【初盘完成】
            tasks.ForEach(p => p.InventoryTaskStatus = InventoryTaskStatus.FirstDone);
            RF.Save(tasks);
        }

        /// <summary>
        /// 盘点完成
        /// </summary>
        /// <param name="taskIds">任务id</param>
        public virtual void SecondComplete(List<double> taskIds)
        {
            var tasks = GetInventoryTasksByIds(taskIds);
            //盘点状态为【初盘完成】、【复盘中】才能点击
            if (tasks.Any(p => p.InventoryTaskStatus != InventoryTaskStatus.FirstDone && p.InventoryTaskStatus != InventoryTaskStatus.ScondDoing))
            {
                throw new ValidationException("盘点状态为【初盘完成】、【复盘中】才能盘点完成".L10N());
            }

            var allTaskEquips = GetInventoryEquipByTaskIds(taskIds);

            //生成备件盘点差异
            EntityList<InventoryTaskSparePartDiff> inventoryTaskSparePartDiffs = null;
            var saprePartTaskIds = tasks.Where(x => x.InventoryAssetObject == InventoryAssetObject.Spare)
                .Select(x => x.Id).ToList();

            if (saprePartTaskIds.Any())
            {
                inventoryTaskSparePartDiffs = RT.Service.Resolve<InventoryTaskSpartPartController>()
                  .GenerateSparePartDiff(saprePartTaskIds);
            }

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                foreach (var task in tasks)
                {
                    //更新状态为【盘点完成】
                    task.InventoryTaskStatus = InventoryTaskStatus.Completed;
                }

                //生成原因分析
                GenerateInventoryCause(allTaskEquips);

                //保存备件盘点差异
                if (inventoryTaskSparePartDiffs != null && inventoryTaskSparePartDiffs.Any())
                {
                    RF.Save(inventoryTaskSparePartDiffs);
                }

                RF.Save(tasks);

                trans.Complete();
            }
        }

        /// <summary>
        /// 生成原因分析
        /// </summary>
        /// <param name="allTaskEquips">盘点任务设备清单</param>        
        private void GenerateInventoryCause(EntityList<InventoryTaskEquipment> allTaskEquips)
        {
            //结果为盘盈或盘亏的设备清单数据，复盘结果为空取初盘结果，复盘结果不为空的取复盘结果，但是剔除初盘盘盈，复盘盘亏的数据
            foreach (var taskEquip in allTaskEquips)
            {
                if (taskEquip.FirstInventoryResult == InventoryResult.Profit && taskEquip.SecondInventoryResult == InventoryResult.Loss)
                {
                    continue;
                }

                var result = taskEquip.SecondInventoryResult.HasValue ? taskEquip.SecondInventoryResult : taskEquip.FirstInventoryResult;

                if (result == InventoryResult.Profit || result == InventoryResult.Loss)
                {
                    var cause = new InventoryCause();

                    cause.EquipmentCode = taskEquip.EquipmentCode;
                    cause.EquipmentName = taskEquip.EquipmentName;
                    cause.InventoryResult = result.Value;
                    cause.KeeperId = taskEquip.AdministratorId;
                    cause.InventoryTaskId = taskEquip.InventoryTaskId;

                    RF.Save(cause);
                }

                taskEquip.InventoryProcessMethod = taskEquip.SuggestProcessMethod;
            }
            RF.Save(allTaskEquips);
        }

        /// <summary>
        /// 新增盘盈
        /// </summary>
        /// <param name="model">新增盘盈信息</param>
        public virtual void AddProfit(AddProfitViewModel model)
        {
            if (model.AddProfitUIState == AddProfitUIState.B && model.EquipmentName.IsNullOrWhiteSpace())
            {
                throw new ValidationException("设备名称必输".L10N());
            }
            if (model.RealWorkShopId.HasValue && model.RealWarehouseId.HasValue)
            {
                throw new ValidationException("车间和仓库不能同时有值".L10N());
            }
            var task = GetById<InventoryTask>(model.InventoryTaskId);
            if (task == null)
            {
                throw new ValidationException("找不到此盘点任务，数据异常".L10N());
            }
            if (task.InventoryTaskStatus != InventoryTaskStatus.Doing && task.InventoryTaskStatus != InventoryTaskStatus.FirstDone
                && task.InventoryTaskStatus != InventoryTaskStatus.ScondDoing)
            {
                throw new ValidationException("盘点状态为【盘点中、初盘完成、复盘中】才可新增盘盈".L10N());
            }
            if (task.NeedPhoto && model.PhotoFilePath.IsNullOrWhiteSpace())
            {
                throw new ValidationException("上传的图片不能为空".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //盘点任务状态为【初盘完成】时，更新为【复盘中】
                if (task.InventoryTaskStatus == InventoryTaskStatus.FirstDone)
                {
                    task.InventoryTaskStatus = InventoryTaskStatus.ScondDoing;
                    RF.Save(task);
                }
                var equip = RT.Service.Resolve<EquipAccountController>().GetEquipAccountsByCode(model.EquipmentCode);
                var taskEquip = Query<InventoryTaskEquipment>().Where(p => p.InventoryTaskId == task.Id && p.EquipmentCode == model.EquipmentCode)
                    .FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
                if (taskEquip == null || model.EquipmentCode.IsNullOrWhiteSpace())
                {
                    //有设备编码不存在于设备清单或无设备编码时，新增一条数据
                    var newTaskEquip = new InventoryTaskEquipment();
                    newTaskEquip.InventoryTaskId = task.Id;
                    newTaskEquip.InventoryAssetSource = InventoryAssetSource.Profit;
                    newTaskEquip.EquipmentCode = model.EquipmentCode;
                    newTaskEquip.RealManageDeptId = task.ManageDeptId;
                    if (equip != null)
                    {

                        SetEquipOldInfo(equip, newTaskEquip);
                    }
                    //更新界面填写的字段值
                    AddProfitUpdateValue(newTaskEquip, model, task);
                    RF.Save(newTaskEquip);
                }
                else
                {
                    //有设备编码且设备编码存在于设备清单时，更新对应的数据；
                    //有设备编码且设备编码存在于设备清单时,设备名称还是取设备台账的名称 不取前端传来的设备名称
                    //model.EquipmentName = equip.Name;
                    AddProfitUpdateValue(taskEquip, model, task);

                    //来源为【账内资产】时，全部值一样时取值【正常】，有不一样时取值【信息变动】
                    UpdateInventoryResult(taskEquip, task);
                    RF.Save(taskEquip);
                    //盘点任务的盘点状态为【盘点中】且设备存在于已有的设备清单且来源为【账内资产】时，更新盘点任务和盘点计划的执行进度 
                    if (task.InventoryTaskStatus == InventoryTaskStatus.Doing && taskEquip.InventoryAssetSource == InventoryAssetSource.Account)
                    {
                        UpdatePercentage(new EntityList<InventoryTask> { task });
                    }
                }
                //提交事务
                trans.Complete();
            }
        }

        /// <summary>
        /// 设置设备原信息
        /// </summary>
        /// <param name="equip"></param>
        /// <param name="newTaskEquip"></param>
        private void SetEquipOldInfo(EquipAccount equip, InventoryTaskEquipment newTaskEquip)
        {
            newTaskEquip.EquipAccountId = equip.Id;
            newTaskEquip.EquipmentCode = equip.Code;
            newTaskEquip.EquipmentName = equip.Name;
            newTaskEquip.OldManageDeptId = equip.ManageDepartmentId;
            newTaskEquip.OldManageDept = equip.ManageDepartmentName;
            newTaskEquip.OldUseDeptId = equip.UseDepartmentId;
            newTaskEquip.OldUseDeptName = equip.UseDepartmentName;
            newTaskEquip.OldAccountUseState = equip.UseState;
            newTaskEquip.OldAccountState = equip.State;
            newTaskEquip.OldUserId = equip.UserId;
            if (equip.UserId.HasValue)
            {
                newTaskEquip.OldUserName = RF.GetById<Employee>(equip.UserId)?.Name;
            }
            newTaskEquip.OldWorkShopId = equip.WorkShopId;
            newTaskEquip.OldWorkShopName = equip.WorkShopName;
            newTaskEquip.OldResourceId = equip.ResourceId;
            newTaskEquip.OldResourceName = equip.ResourceName;
            newTaskEquip.OldWarehouseId = equip.WarehouseId;
            newTaskEquip.OldWarehouseCode = equip.WarehouseName;
            newTaskEquip.OldStorageLocationId = equip.StorageLocationId;
            if (equip.StorageLocationId.HasValue)
            {
                newTaskEquip.OldStorageLocationCode = equip.StorageLocation.Code;
            }
            newTaskEquip.OldLocation = equip.InstallationLocation;
        }

        /// <summary>
        /// 更新界面填写的字段值
        /// </summary>
        /// <param name="newTaskEquip">盘点任务设备清单</param>
        /// <param name="model">新增盘盈信息</param>
        /// <param name="task">盘点任务</param>
        private void AddProfitUpdateValue(InventoryTaskEquipment newTaskEquip, AddProfitViewModel model, InventoryTask task)
        {
            var now = RF.Find<InventoryTask>().GetDbTime();
            newTaskEquip.InventoryStatus = InventoryStatus.Done;
            newTaskEquip.RealManageDeptId = task.ManageDeptId;
            newTaskEquip.EquipmentName = model.EquipmentName;
            newTaskEquip.AccountUseState = model.AccountUseState;
            newTaskEquip.AccountState = model.AccountState;
            newTaskEquip.TypeCategory = model.TypeCategory;
            newTaskEquip.EquipTypeId = model.EquipTypeId;
            newTaskEquip.EquipModelId = model.EquipModelId;
            newTaskEquip.RealUseDeptId = model.UseDeptId;
            newTaskEquip.UserId = model.UserId;
            newTaskEquip.RealWorkShopId = model.RealWorkShopId;
            newTaskEquip.RealResourceId = model.RealResourceId;
            newTaskEquip.RealWarehouseId = model.RealWarehouseId;
            newTaskEquip.StorageLocationId = model.StorageLocationId;
            newTaskEquip.RealLocation = model.RealLocation;
            newTaskEquip.PhotoFilePath = model.PhotoFilePath;
            //来源为【盘盈新增】时取值【盘盈】
            if (newTaskEquip.InventoryAssetSource == InventoryAssetSource.Profit)
            {
               /* if (model.PhotoFilePath.IsNullOrWhiteSpace())
                {
                    throw new ValidationException("上传的图片不能为空".L10N());
                }*/
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
            }
        }

        /// <summary>
        /// 更新盘点结果
        /// </summary>
        /// <param name="taskEquip">盘点设备</param>
        /// <param name="task">盘点任务</param>
        private void UpdateInventoryResult(InventoryTaskEquipment taskEquip, InventoryTask task)
        {
            if (taskEquip.InventoryAssetSource == InventoryAssetSource.Account)
            {
                var result = InventoryResult.Normal;
                if (taskEquip.OldManageDeptId != taskEquip.RealManageDeptId || taskEquip.OldUseDeptId != taskEquip.RealUseDeptId ||
                    taskEquip.OldAccountUseState != taskEquip.AccountUseState || taskEquip.OldAccountState != taskEquip.AccountState ||
                    taskEquip.OldUserId != taskEquip.UserId || taskEquip.OldWorkShopId != taskEquip.RealWorkShopId ||
                    taskEquip.OldResourceId != taskEquip.RealResourceId || taskEquip.OldWarehouseId != taskEquip.RealWarehouseId ||
                    taskEquip.OldStorageLocationId != taskEquip.StorageLocationId || taskEquip.OldLocation != taskEquip.RealLocation)
                {
                    result = InventoryResult.InfoChange;
                }
                var now = RF.Find<InventoryTask>().GetDbTime();
                if (task.InventoryTaskStatus == InventoryTaskStatus.Doing)
                {
                    taskEquip.FirstInventoryResult = result;
                    taskEquip.FirstCounterId = RT.IdentityId;
                    taskEquip.InventoryDateTime = now;
                }
                else
                {
                    taskEquip.SecondInventoryResult = result;
                    taskEquip.SecondCounterId = RT.IdentityId;
                    taskEquip.SecondDateTime = now;
                }
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="remark"></param>
        public virtual void Shutdown(List<double> selectedIds, string remark)
        {
            var tasks = GetListInventoryTaskByIds(selectedIds);
            if (tasks.Any(p => p.InventoryTaskStatus == InventoryTaskStatus.Closed))
            {
                throw new ValidationException("已关闭单据不能再关闭".L10N());
            }
            tasks.ForEach(item =>
            {
                item.CloseRemark = remark;
                item.InventoryTaskStatus = InventoryTaskStatus.Closed;
            });
            RF.Save(tasks);
        }

        /// <summary>
        /// 根据Id集合获取批量盘点任务
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <returns></returns>
        private EntityList<InventoryTask> GetListInventoryTaskByIds(List<double> selectedIds)
        {
            return selectedIds.SplitContains(ids =>
            {
                return Query<InventoryTask>().Where(m => ids.Contains(m.Id)).ToList();
            });
        }

        #region 离线盘点
        #region 离线设备盘点
        /// <summary>
        /// 获取符合的盘点任务
        /// </summary>
        /// <param name="elo">是否贪婪加载</param>
        /// <returns></returns>
        public virtual EntityList<InventoryTask> GetOffLineInvTask(EagerLoadOptions elo = null)
        {
            var query = Query<InventoryTask>().Join<InventoryPlan>((x, y) => x.InventoryPlanId == y.Id && y.InventoryAssetObject == InventoryAssetObject.Equipment);
            query.Exists<InventoryTaskEquipment>((x, y) => y.Where(a => a.InventoryTaskId == x.Id && a.InventoryStatus == InventoryStatus.Not));
            query.Exists<InventoryTaskCounter>((x, y) => y.Where(a => a.InventoryTaskId == x.Id && a.EmployeeId == RT.IdentityId && a.First == true));
            query.Where(p => p.InventoryTaskStatus == InventoryTaskStatus.Doing || p.InventoryTaskStatus == InventoryTaskStatus.ScondDoing);
            return query.ToList(null, elo);
        }

        /// <summary>
        /// 获取符合的盘点任务（工治具）
        /// </summary>
        /// <param name="elo">是否贪婪加载</param>
        /// <returns></returns>
        public virtual EntityList<InventoryTask> GetOffLineFixtureInvTask(EagerLoadOptions elo = null)
        {
            var query = Query<InventoryTask>().Join<InventoryPlan>((x, y) => x.InventoryPlanId == y.Id && y.InventoryAssetObject == InventoryAssetObject.Fixture);
            query.Exists<InventoryTaskFixtureCounter>((x, y) => y.Where(a => a.InventoryTaskId == x.Id && a.EmployeeId == RT.IdentityId && a.First));
            query.Where(p => p.InventoryTaskStatus == InventoryTaskStatus.Doing || p.InventoryTaskStatus == InventoryTaskStatus.ScondDoing);
            return query.ToList(null, elo);
        }
        #endregion
        #endregion
    }
}
