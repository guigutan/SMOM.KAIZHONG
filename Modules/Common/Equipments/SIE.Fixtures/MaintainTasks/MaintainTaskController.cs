using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.Enums;
using SIE.Fixtures.FixtureDemands;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.InboundOrders;
using SIE.Fixtures.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;

namespace SIE.Fixtures.MaintainTasks
{
    /// <summary>
    /// 工治具任务控制器
    /// </summary>
    public class MaintainTaskController : DomainController
    {
        /// <summary>
        /// 通用控制器
        /// </summary>
        private static CommonController _commonController = RT.Service.Resolve<CommonController>();

        #region 保养任务

        /// <summary>
        /// 保养任务查询实体方法
        /// </summary>
        /// <param name="criteria">保养任务查询实体</param>
        /// <returns>保养任务列表</returns>
        public virtual EntityList<MaintainTask> QueryMaintainTaskList(MaintainTaskCriteria criteria)
        {
            var q = Query<MaintainTask>();
            if (criteria.No.IsNotEmpty())
                q.Where(w => w.No.Contains(criteria.No));
            if (criteria.RelatedNo.IsNotEmpty())
                q.Where(w => w.RelatedNo.Contains(criteria.RelatedNo));
            if (criteria.MaintainType.HasValue)
                q.Where(w => w.MaintainType == criteria.MaintainType);
            if (criteria.State.HasValue)
                q.Where(w => w.State == criteria.State);
            if (criteria.ApplyDate != null && criteria.ApplyDate.BeginValue.HasValue)
                q.Where(w => w.ApplyDate >= criteria.ApplyDate.BeginValue);
            if (criteria.ApplyDate != null && criteria.ApplyDate.EndValue.HasValue)
                q.Where(w => w.ApplyDate <= criteria.ApplyDate.EndValue);
            if (criteria.FinishDate.BeginValue.HasValue)
                q.Where(w => w.FinishDate >= criteria.FinishDate.BeginValue);
            if (criteria.FinishDate.EndValue.HasValue)
                q.Where(w => w.FinishDate <= criteria.FinishDate.EndValue);
            if (criteria.IdCode.IsNotEmpty() || criteria.EncodeCode.IsNotEmpty() || criteria.FixtureTypeId.HasValue)
            {
                q.Join<FixtureAccount>((a, b) => a.FixtureAccountId == b.Id)
                 .Join<FixtureAccount, FixtureEncode>((b, c) => b.FixtureEncodeId == c.Id)
                 .Join<FixtureEncode, FixtureModel>((c, d) => c.FixtureModelId == d.Id)
                 .WhereIf<FixtureAccount>(criteria.IdCode.IsNotEmpty(), (a, b) => b.Code.Contains(criteria.IdCode))
                 .WhereIf<FixtureEncode>(criteria.EncodeCode.IsNotEmpty(), (a, c) => c.Code.Contains(criteria.EncodeCode))
                 .WhereIf<FixtureModel>(criteria.FixtureTypeId.HasValue, (a, d) => d.FixtureTypeId == criteria.FixtureTypeId);
            }
            return q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工治具台帐Id获取保养任务列表
        /// </summary>
        /// <param name="accountId">工治具台帐Id</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>保养任务列表</returns>
        public virtual EntityList<MaintainTask> GetMaintainTasks(double accountId, PagingInfo pagingInfo = null)
        {
            return Query<MaintainTask>().Where(p => p.FixtureAccountId == accountId).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据保养任务Id获取保养执行详情列表
        /// </summary>
        /// <param name="taskId">保养任务Id</param>
        /// <returns>保养执行详情列表</returns>
        public virtual EntityList<MaintainTaskDetail> GetMaintainTaskDetails(double taskId)
        {
            return Query<MaintainTaskDetail>().Where(p => p.MaintainTaskId == taskId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 保存保养任务
        /// </summary>
        /// <param name="task">保养任务</param>
        public virtual void SaveMaintainTask(MaintainTask task)
        {
            ValidationMaintainTask(task);
            using (var tran = DB.TransactionScope(KitFixturesEntityDataProvider.ConnectionStringName))
            {
                var now = RF.Find<MaintainTask>().GetDbTime();
                if (task.State == MaintainState.Finish)
                {
                    if (task.FinishDate == null)
                        task.FinishDate = now;
                    //更新工治具台账
                    UpdateFixtureAccount(task);

                }
                else
                {
                    task.FinishDate = null;
                }
                foreach (var detail in task.Details)
                {
                    if (detail.MaintainResult == null)
                    {
                        detail.FinishDate = null;
                        detail.MaintainById = null;
                    }
                    else
                    {
                        detail.FinishDate = now;
                        detail.MaintainById = RT.IdentityId;
                    }
                }
                RF.Save(task);
                tran.Complete();
            }
        }

        /// <summary>
        /// 验证保养任务
        /// </summary>
        /// <param name="task">保养任务</param>
        private void ValidationMaintainTask(MaintainTask task)
        {
            var oldTask = GetById<MaintainTask>(task.Id);
            if (oldTask?.State == MaintainState.Finish)
                throw new ValidationException("该保养任务：【{0}】已经保养完成，不能重复保养！".L10nFormat(oldTask.No));
            var isHaveNoResult = task.Details.Any(p => p.MaintainResult == null);
            if (task.State == MaintainState.Finish)  //即将完成保养
            {
                if (task.PassQty == null || task.NgQty == null)
                    throw new ValidationException("必须维护【合格数量】和【不合格数量】！".L10N());
                if (task.PassQty + task.NgQty != task.Qty)
                    throw new ValidationException("【合格数量】和【不合格数量】之和必须等于【治具数量】！".L10N());
                if (isHaveNoResult)
                    throw new ValidationException("存在无【项目保养结论】的项目时保养状态不能为保养完成！".L10N());
            }
            else  //只是暂存保养任务
            {
                if (task.PassQty != null && task.NgQty != null)
                {
                    if (task.PassQty + task.NgQty > task.Qty)
                        throw new ValidationException("【合格数量】和【不合格数量】之和不能超过【治具数量】！".L10N());
                    if (isHaveNoResult)
                        throw new ValidationException("【保养项目】所有【项目保养结论】都给出后才能维护【合格数量】和【不合格数量】！".L10N());
                }
            }
            if (task.Records.Any(p => p.Code == null && p.Name == null))
            {
                throw new ValidationException("【备件编码】与【备件名称】其中一个必须维护才能保存！".L10N());
            }
        }

        /// <summary>
        /// 更新工治具台账信息
        /// </summary>
        /// <param name="task">保养任务</param>
        private void UpdateFixtureAccount(MaintainTask task)
        {
            var account = GetById<FixtureAccount>(task.FixtureAccountId);
            var fixtureEncode = GetById<FixtureEncode>(account.FixtureEncodeId);
            var fixtureModel = GetById<FixtureModel>(fixtureEncode.FixtureModelId);

            if (fixtureModel == null)
            {
                throw new ValidationException("工治具型号的数据不存在，工治具型号ID:【{0}】"
                    .L10nFormat(fixtureEncode.FixtureModelId));
            }

            var manageMode = fixtureModel.ManageMode;

            //出库保养
            if (task.MaintainType == MaintainType.ToStorage)
            {
                //ID管理，工治具台账状态变更
                if (manageMode == ManageMode.Number)
                {
                    account.AccountState = task.PassQty == 1 ? FixtureAccountState.WaitReceive : FixtureAccountState.WaitShelf;
                    account.QualityState = task.PassQty == 1 ? FixtureQualityState.Pass : FixtureQualityState.Ng;
                }
                else
                {
                    //出库保养，合格：【待保养】减少，【待领用】增加
                    //出库保养，不合格：【合格、待保养】减少，【不合格、待入库】增加
                    account.WaitMaintain -= task.Qty;
                    account.WaitReceive += task.PassQty.HasValue ? task.PassQty.Value : 0;
                    account.WaitShelfQty += task.NgQty.HasValue ? task.NgQty.Value : 0;
                    //台账的合格数减少保养不合格数，台账的不合格数增加保养不合格数
                    account.PassQty -= task.NgQty.HasValue ? task.NgQty.Value : 0;
                    account.NgQty += task.NgQty.HasValue ? task.NgQty.Value : 0;
                }

                ////根据【不合格数量】生成入库任务
                GenerateLaunchTask(task.NgQty, account.Id, manageMode, fixtureEncode, task.RelatedNo, task.MaintainType, null);

                //根据【不合格数量】更新工治具需求清单
                UpdateFixtureDemand(task, account);
            }
            else if (task.MaintainType == MaintainType.InStorage)//入库保养
            {
                if (manageMode == ManageMode.Number)
                {
                    account.AccountState = FixtureAccountState.WaitShelf;
                    account.QualityState = task.PassQty == 1 ? FixtureQualityState.Pass : FixtureQualityState.Ng;
                }
                else
                {
                    account.WaitMaintain -= task.Qty;
                    account.WaitShelfQty += task.Qty;
                    //台账的合格数减少保养不合格数，台账的不合格数增加保养不合格数
                    account.PassQty -= task.NgQty.HasValue ? task.NgQty.Value : 0;
                    account.NgQty += task.NgQty.HasValue ? task.NgQty.Value : 0;
                    ////根据【不合格数量】生成出库任务
                    if (task.PassQty > 0 && task.NgQty > 0)//至少存在有合格 即为部分合格部分不合格
                    {
                        GenerateLaunchTask(task.NgQty, account.Id, manageMode, fixtureEncode, task.RelatedNo, task.MaintainType, task.Id);
                    }
                }

                //更新原上架任务
                UpdateLaunchTask(task, manageMode);

            }
            account.MaintainedHour = 0;
            account.MaintainedNum = 0;
            RF.Save(account);
            //更新ID类台账feeder详情
            UpdateAccountTools(account.Id);

        }

        /// <summary>
        /// 根据保养单获取出库任务单
        /// </summary>
        /// <param name="maintainTaskId"></param>
        private InboundOrder GetLaunchTaskByNo(double maintainTaskId)
        {
            return Query<InboundOrder>().Where(m => m.MaintainTaskId == maintainTaskId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取关联单号获取入库单
        /// </summary>
        /// <param name="relatedNo"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>

        private FixtureUnload GetFixtureDemandWhByNo(string relatedNo, double accountId)
        {
            return Query<FixtureUnload>().Join<FixtureDemand>((x, y) => x.FixtureDemandId == y.Id && y.No == relatedNo)
                .Where(m => m.FixtureAccountId == accountId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据不合格数量创建入库任务
        /// </summary>
        /// <param name="ngQty"></param>
        /// <param name="accountId"></param>
        /// <param name="manageMode"></param>
        /// <param name="fixtureEncode"></param>
        /// <param name="relatedNo"></param>
        ///<param name="maintainType"></param>
        private void GenerateLaunchTask(int? ngQty, double accountId, ManageMode manageMode, FixtureEncode fixtureEncode, string relatedNo, MaintainType maintainType,double? maintainTaskId)
        {
            if (ngQty == null || ngQty <= 0)
                return;
            double? warehouseId = null;
            var task = new InboundOrder();
            task.InboundType = FixtureInboundType.Scene;
            switch (maintainType)
            {
                case MaintainType.InStorage:
                    var oldLaunchTask = GetLaunchTaskByNo(maintainTaskId.Value);
                    if (oldLaunchTask != null)
                    {
                        warehouseId = oldLaunchTask.WarehouseId;
                        task.InboundType = oldLaunchTask.InboundType;
                    }
                    break;
                case MaintainType.ToStorage:
                    {
                        var demandBill = GetFixtureDemandWhByNo(relatedNo, accountId);
                        if (demandBill != null)
                        {
                            warehouseId = demandBill.WarehouseId;
                        }
                    }
                    break;
                default:
                    break;

            }

            task.GenerateId();
            task.No = _commonController.GetNo<InboundOrder>("工治具入库");
            task.Qty = ngQty.Value;
            task.InboundStatus = InboundStatus.ToBe;
            task.QualityState = FixtureQualityState.Ng;
            task.FixtureEncodeId = fixtureEncode.Id;
            task.WarehouseId = warehouseId;
            task.MaintainTaskId = maintainTaskId;
            if (manageMode == ManageMode.Number)
            {
                task.InboundOrderFixtureIdAccountList.Add(new InboundOrderFixtureIdAccount()
                {
                    FixtureIDAccountId = accountId,
                    Qty = ngQty.Value,
                    InboundOrderId = task.Id
                });
            }
            else
            {
                task.InboundOrderFixtureCodeAccountList.Add(new InboundOrderFixtureCodeAccount()
                {
                    InboundOrderId = task.Id,
                    Qty = ngQty.Value,
                });
            }
            RF.Save(task);
        }

        /// <summary>
        /// 更新ID类台账feeder详情
        /// </summary>
        /// <param name="accountId">台账Id</param>
        private void UpdateAccountTools(double accountId)
        {
            var tools = RT.Service.Resolve<CoreFixtureController>().GetFixtureAccountTools(accountId);
            foreach (var tool in tools)
            {
                tool.MaintainedThrowQty = 0;
                tool.MaintainedUseNum = 0;
            }
            RF.Save(tools);
        }

        /// <summary>
        /// 根据【不合格数量】更新工治具需求清单（出库）
        /// </summary>
        /// <param name="maintainTask">保养任务</param>
        /// <param name="account">工治具</param>
        private void UpdateFixtureDemand(MaintainTask maintainTask, FixtureAccount account)
        {
            if (maintainTask.NgQty == null || maintainTask.NgQty <= 0)
                return;
            var fixDemand = RT.Service.Resolve<ElecFixtureController>()
                .GetFixtureDemand(maintainTask.RelatedNo);

            if (fixDemand == null)
                throw new ValidationException("数据异常：找不到工治具需求清单{0}".L10nFormat(maintainTask.RelatedNo));

            var unloads = RT.Service.Resolve<ElecFixtureController>()
                .GetNgFixtureUnloads(fixDemand.Id, account.Id, maintainTask.Id);

            if (!unloads.Any())
                throw new ValidationException("数据异常：找不到工治具需求清单{0}对应的出库明细".L10nFormat(maintainTask.RelatedNo));
            if (unloads.Sum(p => p.UnloadQty) - unloads.Sum(p => p.NgQty) < maintainTask.NgQty.Value)
                throw new ValidationException("数据异常：工治具需求清单{0}对应的出库明细出库数量不足本次不合格数量".L10nFormat(maintainTask.RelatedNo));
            var ngQty = maintainTask.NgQty.Value;
            //修改出库明细
            foreach (var unload in unloads)
            {
                var qty = unload.UnloadQty - unload.NgQty;//此明细可加的不合格数量
                if (qty >= ngQty)
                {
                    unload.NgQty += ngQty;
                    ngQty = 0;
                    break;
                }
                else
                {
                    unload.NgQty = unload.UnloadQty;
                    ngQty -= qty;
                }
            }
            if (ngQty != 0)
                throw new ValidationException("数据异常：工治具出库明细不合格数量更新失败".L10N());
            RF.Save(unloads);

            //修改需求明细
            var demandDetail = RT.Service.Resolve<ElecFixtureController>()
                .GetDemandDetailByEncode(fixDemand.Id, account.FixtureEncodeId);

            if (demandDetail.UnloadQty < maintainTask.NgQty.Value)
                throw new ValidationException("数据异常：工治具需求明细出库数量不足".L10N());
            demandDetail.UnloadQty -= maintainTask.NgQty.Value;
            RF.Save(demandDetail);
            //更新出库状态
            fixDemand.DemandState = SubmitUpdateDemandState(fixDemand.Id);
            RF.Save(fixDemand);
        }

        /// <summary>
        /// 获取出库状态
        /// </summary>
        /// <param name="fixDemandId">需求Id</param>
        /// <returns>出库状态</returns>
        private DemandState SubmitUpdateDemandState(double fixDemandId)
        {
            var details = RT.Service.Resolve<ElecFixtureController>()
                .GetFixtureDemandDetails(fixDemandId);

            if (details.All(p => p.UnloadQty >= p.DemandQty))
            {
                return DemandState.Finish;
            }
            else if (details.All(p => p.UnloadQty == 0))
            {
                return DemandState.None;
            }

            return DemandState.Part;
        }

        /// <summary>
        /// 更新原上架任务
        /// </summary>
        /// <param name="task">保养任务</param>
        /// <param name="manageMode">治具管理方式</param>
        private void UpdateLaunchTask(MaintainTask task, ManageMode? manageMode)
        {
            var launchTask = GetLaunchTaskByNo(task.Id);
            if (launchTask == null)
                return;
            if (manageMode == ManageMode.Code)
            {
                var launchTaskQty = task.PassQty.HasValue ? task.PassQty.Value : 0;
                launchTask.QualityState = FixtureQualityState.Pass;
                launchTask.InboundStatus = InboundStatus.ToBe;
                if (launchTaskQty <= 0)//全部不合格
                {
                    ////B0033984 改成不删除原来的入库单，原入库单的质量状态改成【不合格】,原入库单的保养状态改为保养完成，也不生成新的入库单
                    launchTask.QualityState = FixtureQualityState.Ng;
                    launchTask.PersistenceStatus = PersistenceStatus.Modified;
                }
                else
                { //部分合格
                    launchTask.Qty = task.PassQty.HasValue ? task.PassQty.Value : 0;
                    var relQty = task.PassQty.Value;//剩余合格的数量
                    launchTask.InboundOrderFixtureCodeAccountList.ForEach(detail =>
                    {
                        if (relQty > 0)
                        {
                            if (relQty - detail.Qty > 0)
                            {
                                relQty -= (int)detail.Qty;
                            }
                            else if (relQty - detail.Qty < 0)//不够
                            {
                                detail.Qty = detail.Qty - relQty;
                                relQty = 0;
                            }
                        }
                        else//已存在满足数量，后面的都删除
                        {
                            detail.PersistenceStatus = PersistenceStatus.Deleted;
                        }

                    });
                }
            }
            else
            {
                launchTask.QualityState = task.PassQty == 1 ? FixtureQualityState.Pass : FixtureQualityState.Ng;
            }
            RF.Save(launchTask);
        }

        /// <summary>
        /// 获取关联单号对应的【保养任务】的【保养状态】不为【保养完成】的单号
        /// </summary>
        /// <param name="nos">关联单号列表</param>
        /// <returns>关联单号列表</returns>
        public virtual IList<string> GetRelatedNosByMaintainState(List<string> nos)
        {
            return Query<MaintainTask>().Where(p => nos.Contains(p.RelatedNo) && p.State != MaintainState.Finish)
                .Select(p => p.RelatedNo).ToList<string>();
        }

        /// <summary>
        /// 工治具上线定期保养
        /// </summary>
        /// <returns>工治具上线定期保养结果信息</returns>
        public virtual string AutoMaintain()
        {
            StringBuilder message = new StringBuilder();
            var accounts = GetNeedMaintainAccount();
            if (!accounts.Any())
                return "无使用中（上料管理为是）但未保养的ID类工治具";
            var accountIds = accounts.Select(p => p.Id).ToList();
            Expression<Func<FixtureAccountUseResume, bool>> exp = p => accountIds.Contains(p.FixtureAccountId);
            var totalUseResumes = _commonController.GetDatas(exp);
            foreach (FixtureAccount account in accounts)
            {
                try
                {
                    RT.Logger.Info("开始工治具{0}上线定期保养".L10nFormat(account.Code));
                    var useResumes = totalUseResumes.Where(p => p.FixtureAccountId == account.Id && p.OnlineDate != null && p.OfflineDate == null).ToList();
                    if (useResumes.Count != 1)
                        throw new ValidationException("工治具{0}使用履历异常,无法执行上线保养！".L10nFormat(account.Code));
                    var useResume = useResumes.FirstOrDefault();
                    var now = RF.Find<FixtureAccount>().GetDbTime();
                    var date = useResume.OnlineDate.Value.AddHours((double)account.OnlineHour);
                    if (date > now)
                        continue;
                    using (var trans = DB.TransactionScope(KitFixturesEntityDataProvider.ConnectionStringName))
                    {
                        if (account.MaintainEnforce)//保养强制下线
                        {
                            useResume.OfflineDate = now;
                            account.AccountState = FixtureAccountState.Online;
                            RF.Save(account);
                            RF.Save(useResume);
                        }
                        OnlineCreateMaintainTask(account);
                        trans.Complete();
                    }
                    RT.Logger.Info("结束工治具{0}上线定期保养".L10nFormat(account.Code));
                }
                catch (Exception exc)
                {
                    message.AppendLine("工治具上线定期保养失败，失败信息：{0}".L10nFormat(exc.Message));
                }
                finally
                {
                    Thread.Sleep(10);
                }
            }
            return message.ToString();
        }

        /// <summary>
        /// 获取使用中(上料管理为是)、未保养的ID类工治具
        /// </summary>
        /// <returns>使用中、未保养的工治具ID</returns>
        private EntityList<FixtureAccount> GetNeedMaintainAccount()
        {
            return Query<FixtureAccount>()
                    .Exists<FixtureEncode>((x, y) => y.Join<FixtureModel>((c, d) => c.FixtureModelId == d.Id
                            && d.ManageMode == ManageMode.Number && d.LoadingManage == YesNo.Yes)
                        .Where(p => p.Id == x.FixtureEncodeId))
                    .Exists<MaintainTask>((x, y) => y.Where(z => z.FixtureAccountId == x.Id && z.State == MaintainState.Wait))
                    .Where(p => p.AccountState == FixtureAccountState.Using)
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 上线定期保养,创建保养任务
        /// </summary>
        /// <param name="account">工治具</param>
        private void OnlineCreateMaintainTask(FixtureAccount account)
        {
            var maintainPrjs = RT.Service.Resolve<CoreFixtureController>()
                .GetOnlineMaintainProjects(account.FixtureEncodeId);

            if (!maintainPrjs.Any())
            {
                throw new ValidationException("找不到该工治具{0}的上线定期保养项目,无法执行上线保养！".L10nFormat(account.Code));
            }

            var maintainTask = new MaintainTask()
            {
                No = _commonController.GetNo<MaintainTask>("保养任务编号"),
                MaintainType = MaintainType.Regular,
                FixtureAccountId = account.Id,
                State = MaintainState.Wait,
                ApplyDate = RF.Find<MaintainTask>().GetDbTime(),
                Qty = 1
            };
            foreach (var maintainPrj in maintainPrjs)
            {
                var detail = new MaintainTaskDetail()
                {
                    MaintainProjectId = maintainPrj.MaintainProjectId,
                    MinValue= maintainPrj.MinValue,
                    MaxValue=maintainPrj.MaxValue

                };
                maintainTask.Details.Add(detail);
            }
            RF.Save(maintainTask);
        }

        #endregion
    }
}
