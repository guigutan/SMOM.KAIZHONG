using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.NumberRules;
using SIE.DataAuth;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Enums;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.Equipments.Models;
using SIE.EMS.Lubrications.Configs;
using SIE.EMS.Lubrications.Handle;
using SIE.EMS.Lubrications.ViewModels;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Applys.Details;
using SIE.EMS.SpareParts.Applys.Enums;
using SIE.EMS.SpareParts.OutDepots.Details;
using SIE.Equipments.Common.Controller;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.Equipments.WorkFlows;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Lubrications
{
    /// <summary>
    /// 润滑项目控制器
    /// </summary>
    public partial class LubricationController : DomainController
    {

        /// <summary>
        /// 获取润滑记录
        /// </summary>
        public virtual EntityList<Lubrication> GetLubricationList(LubricationCriteria criteria)
        {
            var query = Query<Lubrication>();
            //润滑单号
            if (criteria.LubricationNo.IsNotEmpty())
            {
                query.Where(p => p.LubricationNo.Contains(criteria.LubricationNo));
            }
            //设备台账
            if (criteria.EquipAccountId.HasValue)
            {
                query.Where(p => p.EquipAccountId == criteria.EquipAccountId);
            }
            //设备台账名称
            if (criteria.EquipAccountName.IsNotEmpty())
            {
                query.Where(p => p.EquipAccount.Name.Contains(criteria.EquipAccountName));
            }
            //设备类型
            if (criteria.EquipTypeId.HasValue)
            {
                query.Where(p => p.EquipAccount.EquipModel.EquipType.Id == criteria.EquipTypeId);
            }
            //设备型号
            if (criteria.EquipModelId.HasValue)
            {
                query.Where(p => p.EquipAccount.EquipModelId == criteria.EquipModelId);
            }
            //润滑状态
            if (criteria.LubricationStatus.HasValue)
            {
                query.Where(p => p.LubricationStatus == criteria.LubricationStatus);
            }
            //车间
            if (criteria.WorkShopId.HasValue)
            {
                query.Where(p => p.EquipAccount.WorkShopId == criteria.WorkShopId);
            }
            //使用部门
            if (criteria.UseDepartmentId.HasValue)
            {
                query.Where(p => p.EquipAccount.UseDepartmentId == criteria.UseDepartmentId);
            }
            //计划日期
            if (criteria.PlanDate.BeginValue.HasValue)
            {
                query.Where(p => p.PlanDate >= criteria.PlanDate.BeginValue);
            }
            if (criteria.PlanDate.EndValue.HasValue)
            {
                query.Where(p => p.PlanDate <= criteria.PlanDate.EndValue);
            }
            //润滑日期
            if (criteria.StartDateTime.BeginValue.HasValue)
            {
                query.Where(p => p.StartDateTime >= criteria.StartDateTime.BeginValue);
            }
            if (criteria.StartDateTime.EndValue.HasValue)
            {
                query.Where(p => p.StartDateTime <= criteria.StartDateTime.EndValue);
            }
            //是否预期
            if (criteria.IsOverdue.HasValue)
            {
                if (criteria.IsOverdue.Value)
                {
                    query.Where(p => DateTime.Today < p.PlanDate && p.LubricationStatus == LubricationStatus.Pending);
                }
                else
                {
                    query.Where(p => DateTime.Today > p.PlanDate);
                }

            }

            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();
            elo.LoadWith(EquipAccount.EquipModelProperty);
            elo.LoadWith(EquipModel.EquipTypeProperty);
            elo.LoadWith(EquipAccount.UseDepartmentProperty);
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, elo);
        }


        #region 查找Lubrication对象数据
        /// <summary>
        /// 根据Id集合查找润滑记录
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public virtual EntityList<Lubrication> GetLubricationList(List<double> idList)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(Lubrication.LubricationDetailListProperty);
            elo.LoadWith(Lubrication.LubricationWorkHourListProperty);
            elo.LoadWithViewProperty();
            return idList.SplitContains((ids) =>
             {
                 return Query<Lubrication>().Where(p => ids.Contains(p.Id)).ToList(null, elo);
             });
        }

        #endregion

        #region  查找LubricationDetail对象数据
        /// <summary>
        /// 根据润滑项目明细Id获取润滑项目
        /// </summary>
        /// <returns></returns>
        public virtual LubricationDetail GetLubricationDetailById(double lubdetailId)
        {
            return Query<LubricationDetail>().Where(p => p.Id == lubdetailId).ToList(null, new EagerLoadOptions().LoadWithViewProperty()).FirstOrDefault();
        }


        /// <summary>
        /// 查询设备台账润滑项目超过预警期的润滑项目
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<EquipAccountLubricationProject> GetEquipAccountLubricationProjectList(bool? notSubmit)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(EquipAccountLubricationProject.ProjectDetailProperty);
            elo.LoadWithViewProperty();
            var query = Query<EquipAccountLubricationProject>()
               .Exists<EquipAccount>((a, b) => b.Where(c => a.EquipAccountId == c.Id && c.UseState == Core.Enums.AccountUseState.Using))
               .Where(p => p.NextDate != null && p.WarningPeriod != null);

            if (notSubmit.HasValue)
            {
                query.Where(p => p.NotSubmit == notSubmit);
            }
            using (DataAuths.LoadAll())
            {
                return query.ToList(null, elo);
            }
        }
        #endregion

        /// <summary>
        /// 润滑项目添加ViewModels的数据查询
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<SelLubricationDetailViewModel> GetLubricationDetail(SelLubricationDetailCriteria criteria)
        {
            var lst = new EntityList<SelLubricationDetailViewModel>();
            //必须要有设备类型
            if (criteria.EquipAccountId == null)
            {
                return lst;
            }
            var query = Query<EquipAccountLubricationProject>().Where(p => p.EquipAccountId == criteria.EquipAccountId);
            //有责任部门按部门，无部门则取此设备类型下所有的润滑项目
            if (criteria.DepartmentId.HasValue)
            {
                query.Where(p => p.DepartmentId == criteria.DepartmentId);
            }
            //项目名称
            if (criteria.Name.IsNotEmpty())
            {
                query.Where(p => p.ProjectDetail.Name.Contains(criteria.Name));
            }
            var items = query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //转换
            SelLubricationDetailViewModel model = null;
            foreach (var item in items)
            {
                model = new SelLubricationDetailViewModel();
                model.ProjectDetailId = item.Id;
                model.ProjectName = item.ProjectName;
                model.Part = item.Part;
                model.Consumable = item.Consumable;
                model.Method = item.Method;
                model.Standard = item.Standard;
                model.MinValue = item.MinValue;
                model.MaxValue = item.MaxValue;
                model.Unit = item.Unit;
                model.UseTime = item.UseTime;
                model.ProjectType = item.ProjectType;
                model.CycleType = item.CycleType;
                model.ProjectCycle = item.ProjectCycle;
                model.WarningPeriod = item.WarningPeriod;
                model.LubricatingType = item.LubricatingType;
                model.NotSubmit = false;
                lst.Add(model);
            }
            return lst;
        }

        /// <summary>
        /// 根据Id获取设备台账的润滑项目
        /// </summary>
        /// <param name="proList"></param>
        /// <returns></returns>
        public virtual EntityList<EquipAccountLubricationProject> GetEquipAccountLubricationProject(List<double> proList)
        {
            return proList.SplitContains((ids) =>
            {
                return Query<EquipAccountLubricationProject>().Where(p => ids.Contains(p.Id)).ToList();
            });
        }

        /// <summary>
        /// 获取设配台账润滑项目的备件清单
        /// </summary>
        /// <param name="detailId"></param>
        /// <param name="order"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public virtual EntityList<EquipAccountLubricaSparePart> GetEquipAccountLubricaSparePart(double detailId, IList<OrderInfo> order, PagingInfo page)
        {
            var detail = GetLubricationDetailById(detailId);
            var list = Query<EquipAccountLubricaSparePart>().Where(p => p.LubricationProjectId == detail.ProjectDetailId).ToList(page, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }



        #region 润滑记录操作

        #region 保存润滑记录(添加)

        /// <summary>
        /// 保存润滑记录(添加)
        /// </summary>
        /// <param name="lub"></param>
        public virtual void SaveLubrication(Lubrication lub)
        {
            if (lub.LubricationDetailList == null || !lub.LubricationDetailList.Any() || lub.LubricationDetailList.Count <= 0)
            {
                throw new ValidationException("润滑记录【{0}】未添加润滑记录。".L10nFormat(lub.LubricationNo));
            }

            var Oldlub = Query<Lubrication>().Where(p => p.EquipAccountId == lub.EquipAccountId && p.BillSourceType == BillSourceType.Manual && (p.LubricationStatus == LubricationStatus.Pending || p.LubricationStatus == LubricationStatus.Doing)).FirstOrDefault();
            if (Oldlub != null)
            {
                throw new ValidationException("已存在该设备手工创建未完成的润滑记录。".L10N());
            }

            //临时新增保存待执行，待提交
            lub.LubricationStatus = LubricationStatus.Pending;
            lub.ApprovalStatus = ApprovalStatus.Draft;
            //验证
            RF.Save(lub);
        }

        #endregion

        #region 保存润滑记录(添加记录)
        /// <summary>
        /// 保存润滑记录(添加记录)
        /// </summary>
        /// <param name="lub"></param>
        public virtual void LubricationDetailSave(Lubrication lub)
        {
            //检查单据润滑状态是否发生变更
            CheckBillState(lub);
            //验证润滑项目明细的加油量与延期天数不能同时为空 
            //临时新增保存待执行，待提交
            lub.LubricationStatus = LubricationStatus.Doing;
            //保存
            RF.Save(lub);
        }

        #endregion

        #region 添加记录提交(逐个)
        /// <summary>
        /// 添加记录提交(逐个)
        /// </summary>
        public virtual void LubricationDetailSumbit(Lubrication lub)
        {
            //先判断润滑记录是否需要审核,是否开启审核流程
            var config = RT.Service.Resolve<ApprovalController>().GetApprovalConfigValue(typeof(Lubrication));
            var now = RF.Find<Lubrication>().GetDbTime();
            //获取计划规则配置项
            var PlanType = GetPlanType();
            //检查单据润滑状态是否发生变更
            CheckBillState(lub);
            //验证润滑项目明细的加油量与延期天数不能同时为空
            var NotlubDetailIds = lub.LubricationDetailList.Select(p => p.Id).ToList();
            //可能前端传入的明细不完整，则可能漏验证,在此处添加完整明细集合再验证
            var Detaillist = GetLubricationDetailList(lub.Id, NotlubDetailIds);
            if (Detaillist.Any())
            {
                lub.LubricationDetailList.AddRange(Detaillist);
            }
            CheckData(lub);

            List<double> ids = lub.LubricationDetailList.Select(p => p.ProjectDetailId).ToList();
            List<EquipAccountLubricationProject> list = GetEquipAccountLubricationProject(ids).ToList();

            EntityList<EquipAccountLubricationProject> UpdateProjectList = CalculationDate(lub, list, PlanType);

            //更新执行状态为已完成
            lub.LubricationStatus = LubricationStatus.Done;
            //更新审核状态为待审核
            lub.ApprovalStatus = ApprovalStatus.PendingReview;
            //更新执行人
            lub.ExecutorId = RT.IdentityId;

            //创建履历
            EntityList<EquipAccountResume> resList = GenerateEquipAccountResume(new List<Lubrication> { lub });

            //保存
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //保存设备履历
                if (resList.Any())
                {
                    RF.Save(resList);
                }
                //保存润滑记录修改过的计算上下次润滑日期
                if (UpdateProjectList.Any())
                {
                    RF.Save(UpdateProjectList);
                }

                if (!config.EnableAudit)
                {
                    //通过
                    lub.LubricationStatus = LubricationStatus.Done;
                    lub.ApprovalStatus = ApprovalStatus.Audited;
                    //保存成功之后添加审核记录
                    RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(ids, typeof(Lubrication).FullName, ApprovalResult.Pass, now, "同意".L10N());
                }
                //保存润滑计划
                RF.Save(lub);
                trans.Complete();
            }
        }

        /// <summary>
        /// 根据润滑记录获取润滑明细且不包含也有的明细
        /// </summary>
        /// <param name="lubId"></param>
        /// <param name="NotlubDetailIds"></param>
        /// <returns></returns>
        public virtual EntityList<LubricationDetail> GetLubricationDetailList(double lubId, List<double> NotlubDetailIds)
        {
            return Query<LubricationDetail>().Where(p => p.LubricationId == lubId && !NotlubDetailIds.Contains(p.Id)).ToList();
        }

        #endregion

        #region 添加记录提交(批量)
        /// <summary>
        /// 添加记录提交(批量)
        /// </summary>
        public virtual void LubricationSumbit(List<double> ids)
        {
            //先判断润滑记录是否需要审核,是否开启审核流程
            var config = RT.Service.Resolve<ApprovalController>().GetApprovalConfigValue(typeof(Lubrication));
            EntityList<EquipAccountLubricationProject> EquipAccountLubricationProjectList = new EntityList<EquipAccountLubricationProject>();
            EntityList<EquipAccountLubricationProject> UpdateProjectList;
            List<EquipAccountLubricationProject> ProjectList = null;
            List<double> projectDetailids = new List<double>();

            //获取计划规则配置项
            var PlanType = GetPlanType();
            //获取所有批量提交润滑记录的数据
            var LubricationList = GetLubricationList(ids);

            if (LubricationList.Any(p => p.LubricationStatus != LubricationStatus.Pending && p.LubricationStatus != LubricationStatus.Doing))
            {
                throw new ValidationException("只有执行状态为【待执行】或【执行中】的数据才能提交".L10N());
            }

            foreach (var lub in LubricationList)
            {
                //检查数据有效性
                CheckData(lub);
                //添加所有润滑明细引用的标准润滑项目Id
                projectDetailids.AddRange(lub.LubricationDetailList.Select(p => p.ProjectDetailId).ToList());
            }

            var listDic = GetEquipAccountLubricationProject(projectDetailids).GroupBy(p => p.EquipAccountId).ToDictionary(p => p.Key, p => p.ToList());

            //循环所有需提交的润滑记录
            foreach (var lub in LubricationList)
            {
                //设备台账润滑项目字典中有此润滑记录的设备台账,取设备台账所有的标准润滑项目
                if (listDic.TryGetValue(lub.EquipAccountId, out ProjectList))
                {
                    //按计划规则(基准日期或完工日期)计算下一次润滑日期
                    UpdateProjectList = CalculationDate(lub, ProjectList, PlanType);
                    if (UpdateProjectList.Any() && UpdateProjectList.Count > 0)
                    {
                        EquipAccountLubricationProjectList.AddRange(UpdateProjectList);
                    }
                }
                //更新执行状态为已完成
                lub.LubricationStatus = LubricationStatus.Done;
                //更新审核状态为待审核
                lub.ApprovalStatus = ApprovalStatus.PendingReview;
                //更改执行人
                lub.ExecutorId = RT.IdentityId;
            }

            //创建设备台账润滑类型的设备履历
            EntityList<EquipAccountResume> resList = GenerateEquipAccountResume(LubricationList.ToList());

            //数据库执行
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //保存设备履历
                if (resList.Any())
                {
                    RF.Save(resList);
                }
                if (EquipAccountLubricationProjectList.Any())
                {
                    RF.Save(EquipAccountLubricationProjectList);
                }
                if (!config.EnableAudit)
                {
                    //【是否启用审批】为否时,提交的同时进行审批
                    LubricationApproval(ids, ApprovalResult.Pass, "通过");
                }
                RF.Save(LubricationList);
                trans.Complete();
            }
        }

        #endregion

        #region 单个Or批量 提交计算设备台账标准润滑项目的下次润滑日期

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lub">润滑记录</param>
        /// <param name="list">设备台账标准润滑项目集合</param>
        /// <param name="PlanType">规则类型</param>
        /// <returns></returns>
        public virtual EntityList<EquipAccountLubricationProject> CalculationDate(Lubrication lub, List<EquipAccountLubricationProject> list, PlanType PlanType)
        {
            EntityList<EquipAccountLubricationProject> UpdateProjectList = new EntityList<EquipAccountLubricationProject>();
            //按计划规则(基准日期或完工日期)计算下一次润滑日期
            foreach (var detail in lub.LubricationDetailList)
            {
                //根据润滑项目明细找到设备台账中的标准润滑项目
                var project = list.FirstOrDefault(p => p.Id == detail.ProjectDetailId);
                if (project != null)
                {
                    //如果计划规则为基准日期
                    if (PlanType == PlanType.BaseDate)
                    {
                        //润滑计划延期
                        if (detail.DelayDays.HasValue)
                        {
                            //延期 上一次润滑时间不变
                            //延期 下一次润滑时间等于原先的下一次润滑日期+延期天数
                            project.NextDate = project.NextDate.Value.AddDays(detail.DelayDays.Value);
                        }
                        else
                        {
                            //润滑计划未延期
                            //上一次润滑时间 = 下次润滑日期
                            project.LastDate = lub.EndDateTime;
                            //下一次润滑时间 = 上一次的下次润滑日期+周期
                            project.NextDate = project.LastDate.Value.AddDays(project.ProjectCycle.Value);
                        }
                    }
                    //如果计划规则为完工日期
                    if (PlanType == PlanType.CompleteDate)
                    {
                        //润滑计划延期
                        if (detail.DelayDays.HasValue)
                        {
                            //延期 上一次润滑时间不变
                            //延期 下一次润滑时间等于原先的下一次润滑日期+延期天数
                            project.NextDate = project.NextDate.Value.AddDays(detail.DelayDays.Value);
                        }
                        else
                        {
                            //润滑计划未延期
                            //上一次润滑时间 = 下次润滑日期
                            project.LastDate = lub.EndDateTime;
                            //下一次润滑时间 = 上一次的下次润滑日期+周期
                            project.NextDate = project.LastDate.Value.AddDays(project.ProjectCycle.Value);
                        }
                    }
                    project.NotSubmit = false;
                    UpdateProjectList.Add(project);
                }
            }
            return UpdateProjectList;
        }


        #endregion

        #region 生成设备台账的润滑类型的设备履历

        /// <summary>
        /// 生成设备台账的润滑类型的设备履历
        /// </summary>
        /// <param name="lubs"></param>
        /// <returns></returns>
        public virtual EntityList<EquipAccountResume> GenerateEquipAccountResume(List<Lubrication> lubs)
        {
            EntityList<EquipAccountResume> resList = new EntityList<EquipAccountResume>();
            EquipAccountResume res = new EquipAccountResume();
            lubs.ForEach(lub =>
            {
                res = new EquipAccountResume();
                res.No = lub.LubricationNo;
                res.EquipAccountId = lub.EquipAccountId;
                res.ResumeType = ResumeType.Lubrication;
                res.State = lub.EquipAccount.State;
                resList.Add(res);
            });
            return resList;
        }

        #endregion

        #region 审核润滑项目
        /// <summary>
        /// 审核润滑项目
        /// </summary>
        /// <param name="lubIds">润滑项目</param>
        /// <param name="result">审核结果</param>
        /// <param name="remark">审核意见</param>
        public virtual void LubricationApproval(List<double> lubIds, ApprovalResult result, string remark)
        {
            var now = RF.Find<Lubrication>().GetDbTime();
            //执行状态
            LubricationStatus lStatus;
            //审核状态
            ApprovalStatus aStatus;

            //查询润滑项目数据
            var LubricationList = GetLubricationList(lubIds);
            var ids = LubricationList.Select(p => p.Id).ToList();

            //验证只有执行中的数据才能审核
            if (LubricationList.Any(p => p.LubricationStatus != LubricationStatus.Done || p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有执行状态为【已完成】且审核状态为【待审核】的数据才能审核".L10N());
            }
            if (result == ApprovalResult.Pass)
            {
                //通过
                lStatus = LubricationStatus.Done;
                aStatus = ApprovalStatus.Audited;
            }
            else
            {
                //驳回
                lStatus = LubricationStatus.Pending;
                aStatus = ApprovalStatus.Reject;
            }

            //批量修改状态为已执行
            LubricationList.ForEach(p =>
            {
                p.LubricationStatus = lStatus;
                p.ApprovalStatus = aStatus;
            });

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                if (LubricationList.Any())
                {
                    RF.Save(LubricationList);
                }
                //保存成功之后添加审核记录
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(ids, typeof(Lubrication).FullName, result, now, remark);
                trans.Complete();
            }
        }

        #endregion

        #region 删除润滑记录
        /// <summary>
        /// 删除润滑记录
        /// </summary>
        /// <param name="ids">润滑记录ids</param>
        public virtual void Delete(List<double> ids)
        {
            var LubricationList = GetLubricationList(ids);
            if (LubricationList.Any(p => p.BillSourceType != BillSourceType.Manual || p.ApprovalStatus != ApprovalStatus.Draft))
            {
                throw new ValidationException("选中的数据已被提交,请刷新重试!".L10nFormat(BillSourceType.Manual.ToLabel(), ApprovalStatus.Draft.ToLabel()));
            }

            LubricationList.ForEach(p =>
            {
                p.PersistenceStatus = PersistenceStatus.Deleted;
            });

            RF.Save(LubricationList);
        }

        #endregion

        #endregion

        /// <summary>
        /// 获取上次润滑小结
        /// </summary>
        /// <param name="accountId">设备台账ID</param>
        /// <param name="departmentId">部门ID</param>
        /// <returns></returns>
        public virtual string GetLastLubricationSummaryInfo(double accountId, double? departmentId)
        {
            var q = Query<Lubrication>();
            q.Where(p => p.EquipAccountId == accountId);
            //if (departmentId.HasValue)
            //{
            //    q.Where(p => p.DepartmentId == departmentId);
            //}
            q.Where(p => p.LubricationStatus == LubricationStatus.Done);
            q.OrderByDescending(p => p.EndDateTime);

            return q.ToList(new PagingInfo(1, 1)).FirstOrDefault()?.LubricationSummary;
        }


        /// <summary>
        /// 检查数据
        /// </summary>
        /// <param name="lub">润滑项目对象</param>
        public virtual void CheckData(Lubrication lub)
        {
            //润滑记录验证
            if (!lub.LubricationDetailList.Any())
            {
                throw new ValidationException("润滑单号【{0}】没有润滑项目。".L10nFormat(lub.LubricationNo));
            }
            if (lub.StartDateTime == null || lub.EndDateTime == null)
            {
                throw new ValidationException("润滑单号【{0}】的润滑开始时间或润滑结束时间不能为空。".L10nFormat(lub.LubricationNo));
            }
            if (lub.EndDateTime < lub.StartDateTime)
            {
                throw new ValidationException("润滑单号【{0}】的润滑结束时间不能小于润滑开始时间。".L10nFormat(lub.LubricationNo));
            }
            //润滑项目验证
            foreach (var lubdetail in lub.LubricationDetailList)
            {
                if (lubdetail.ActualValue == null && lubdetail.DelayDays == null)
                {
                    throw new ValidationException("润滑项目的加油量和延期天数不能同时为空。".L10N());
                }
                if (lubdetail.ActualValue != null && lubdetail.DelayDays != null)
                {
                    throw new ValidationException("润滑项目的加油量和延期天数不能同时填写。".L10N());
                }
            }
            //润滑工时登记验证
            foreach (var lubworkHour in lub.LubricationWorkHourList)
            {
                if (lubworkHour.EndDateTime < lubworkHour.StartDateTime)
                {
                    throw new ValidationException("润滑单号【{0}】的工时登记的润滑结束时间不能小于润滑开始时间。".L10nFormat(lub.LubricationNo));
                }
            }
        }

        /// <summary>
        /// 并行操作校验
        /// </summary>
        /// <param name="lub"></param>
        private static void CheckBillState(Lubrication lub)
        {
            // 保存之前先确认此单据是否已经由其他人员操作保存或提交，防止并发操作时程序发生错误
            var result = RF.GetById<Lubrication>(lub.Id);
            if ((lub.LubricationStatus == LubricationStatus.Pending || lub.LubricationStatus == LubricationStatus.Doing || lub.ApprovalStatus == ApprovalStatus.Reject) && result != null && (result.ApprovalStatus != ApprovalStatus.Draft && result.ApprovalStatus != ApprovalStatus.Reject))
            {
                throw new ValidationException("单号【{0}】已由其他人员操作并提交，请退出当前界面重新操作".L10nFormat(lub.LubricationNo));
            }

        }

        /// <summary>
        /// 获取润滑记录编码
        /// </summary>
        /// <param name="Qty">编码个数</param>
        /// <returns>设备台账单号</returns>
        public virtual List<string> GetLubricationNo(int Qty = 1)
        {
            var config = ConfigService.GetConfig(new NoConfig(), typeof(Lubrication));
            if (config == null || config.BacodeRule == null)
            {
                throw new ValidationException("未找到编码生成规则,请检查规则配置".L10N());
            }
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.BacodeRule.Id, Qty).ToList();
        }

        /// <summary>
        /// 获取润滑记录计划规则
        /// </summary>
        /// <returns>计划规则</returns>
        public virtual PlanType GetPlanType()
        {
            var config = ConfigService.GetConfig(new PlanTypeConfig(), typeof(Lubrication));
            if (config == null)
            {
                throw new ValidationException("未找到计划规则,请检查规则配置".L10N());
            }
            if (config.PlanType == null)
            {
                throw new ValidationException("未配置计划规则,请检查规则配置".L10N());
            }
            return config.PlanType.Value;
        }


        #region 备件更换,备件申请

        /// <summary>
        /// 更换润滑计划状态
        /// </summary>
        /// <param name="lubricationId"></param>
        /// <param name="state"></param>
        public virtual void ChangeLubricationState(double lubricationId, LubricationStatus state)
        {
            DB.Update<Lubrication>().Where(p => p.Id == lubricationId).Set(p => p.LubricationStatus, state).Execute();
        }

        /// <summary>
        /// UI执行备件更换
        /// </summary>
        /// <param name="uiLubricationSpareParts"></param>
        public virtual void UIChangeLubricationSparePart(List<LubricationSparePart> uiLubricationSpareParts)
        {
            if (uiLubricationSpareParts == null)
            {
                return;
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //先执行保存更改数据
                uiLubricationSpareParts.ForEach(p => RF.Save(p));
                var lubricationId = uiLubricationSpareParts.FirstOrDefault().LubricationId;
                ChangeLubricationSparePart(lubricationId);
                trans.Complete();
            }
        }

        /// <summary>
        /// 执行备件更换逻辑
        /// </summary>
        /// <param name="lubricationId"></param>
        public virtual void ChangeLubricationSparePart(double lubricationId)
        {
            try
            {
                var datas = RT.Service.Resolve<LubricationPlanController>().GetCheckPlanSpareParts(lubricationId, ChangeSparePartState.New);
                if (datas.Count <= 0)
                {
                    throw new ValidationException("没有备件更换数据".L10N());
                }
                var list = datas.Where(p => p.PartOutDepotDetail != null).ToList();
                if (list.Count <= 0)
                {
                    throw new ValidationException("存在备件更换数据没有选择备件出库单".L10N());
                }
                if (list.Any(p => p.ChangeQty <= 0))
                {
                    throw new ValidationException("存在备件更换数据更换数量为0".L10N());
                }

                using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
                {
                    //回写备件申请单使用数量
                    list.ForEach(p =>
                    {
                        if (p.PartOutDepotDetail.UseCount + p.ChangeQty > p.PartOutDepotDetail.OutDepotCount)
                        {
                            throw new ValidationException("备件[{0}]更换数量不能大于剩余数量".L10nFormat(p.SparePart.SparePartCode));
                        }
                        //回写申请单
                        DB.Update<PartOutDepotDetail>().Where(x => x.Id == p.PartOutDepotDetailId).Set(x => x.UseCount, x => x.UseCount + p.ChangeQty).Execute();
                        //修改备件更换状态
                        DB.Update<LubricationSparePart>().Where(x => x.Id == p.Id).Set(x => x.State, ChangeSparePartState.Finished).Execute();
                        //修改序列号状态
                        DB.Update<StoreSummaryDetail>().Where(x => x.Id == p.PartOutDepotDetail.SeriaNoRefId).Set(x => x.StoreStatus, OrdNumStoreStatus.Using).Execute();
                        //插入备件履历
                        var record = new SparePartChangedRecord()
                        {
                            EquipAccountId = p.Lubrication.EquipAccountId,
                            Qty = p.ChangeQty,
                            OldSerialNumber = p.OldSequence,
                            BatchNumber = p.PartOutDepotDetail?.BatchNo,
                            SerialNumber = p.PartOutDepotDetail?.SeriaNo,
                            Source = FromType.Lubrication,
                            SourceNo = p.Lubrication.LubricationNo,
                            SourceId = p.LubricationId,
                            SparePartId = p.SparePartId
                        };
                        RF.Save(record);
                        trans.Complete();
                    });
                }
            }
            catch (Exception ex)
            {
                //清空未完成的更换单的出库单
                double? value = null;
                DB.Update<LubricationSparePart>().Where(p => p.LubricationId == lubricationId && p.State == ChangeSparePartState.New).Set(p => p.PartOutDepotDetailId, value).Execute();
                throw new ValidationException(ex.GetBaseException().Message);
            }

        }

        #endregion
       

        #region 自动生成润滑计划
        /// <summary>
        /// 自动生成特种设备定检计划
        /// </summary>
        public virtual void AtuoSchedule()
        {
            AtuoScheduleHandle handle = new AtuoScheduleHandle();
            handle.DoSchedule();
        }
        #endregion


        #region 润滑记录命令状态验证
        /// <summary>
        /// 润滑记录命令状态验证
        /// </summary>
        /// <param name="lubricationIds"></param>
        /// <returns></returns>
        public virtual bool CheckParentState(List<double> lubricationIds)
        {
            var lubricationList = lubricationIds.SplitContains(tempIds =>
            {
                return Query<Lubrication>().Where(p => tempIds.Contains(p.Id)).ToList();
            });
            return lubricationList.Any(p => p.LubricationStatus != LubricationStatus.Pending && p.LubricationStatus != LubricationStatus.Doing);
        }
        #endregion
    }
}
