using SIE.Common;
using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.NumberRules;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Enums;
using SIE.EMS.InspectionRules;
using SIE.EMS.SpecialEquipment.RegularInspections.Configs;
using SIE.EMS.SpecialEquipment.RegularInspections.Criterias;
using SIE.EMS.SpecialEquipment.RegularInspections.Handle;
using SIE.EMS.SpecialEquipment.RegularInspections.ViewModels;
using SIE.EMS.SpecialEquipment.SpecialEquipmentAcounts;
using SIE.Equipments.Common.Controller;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.WorkFlows;
using SIE.EventMessages.EMS.SpecialEquipments;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.SpecialEquipment.RegularInspections
{
    /// <summary>
    /// 特种设备定检控制器
    /// </summary>
    public partial class RegularInspectionController : DomainController, IRegularInspection
    {
        /// <summary>
        /// 获取订单评审配置项
        /// </summary>
        public virtual EntityList<RegularInspection> GetRegularInspectionList(RegularInspectionCriteria criteria)
        {
            var query = Query<RegularInspection>();
            if (criteria.InspectionNo.IsNotEmpty())
            {
                query.Where(p => p.InspectionNo.Contains(criteria.InspectionNo));
            }
            if (criteria.SpecialEquipmentAccountId.HasValue)
            {
                query.Where(p => p.SpecialEquipmentAccountId == criteria.SpecialEquipmentAccountId);
            }
            if (criteria.InspectionStatus.HasValue)
            {
                query.Where(p => p.InspectionStatus == criteria.InspectionStatus.Value);
            }
            if (criteria.InspectionResult.HasValue)
            {
                query.Where(p => p.InspectionResult == criteria.InspectionResult);
            }
            if (criteria.AgencyId.HasValue)
            {
                query.Where(p => p.AgencyId == criteria.AgencyId);
            }
            //计划检验日期
            if (criteria.PlanInspectionDate.BeginValue.HasValue)
            {
                query.Where(p => p.PlanInspectionDate >= criteria.PlanInspectionDate.BeginValue);
            }
            if (criteria.PlanInspectionDate.EndValue.HasValue)
            {
                query.Where(p => p.PlanInspectionDate <= criteria.PlanInspectionDate.EndValue);
            }
            //实际检验日期
            if (criteria.ActualInspectionDate.BeginValue.HasValue)
            {
                query.Where(p => p.ActualInspectionDate >= criteria.ActualInspectionDate.BeginValue);
            }
            if (criteria.ActualInspectionDate.EndValue.HasValue)
            {
                query.Where(p => p.ActualInspectionDate <= criteria.ActualInspectionDate.EndValue);
            }
            if (criteria.UseDepartmentId.HasValue || criteria.ResPersonId.HasValue)
            {
                query.Exists<SpecialEquipmentAccount>((r, s) => s.Where(c => c.Id == r.SpecialEquipmentAccountId)
                .WhereIf(criteria.UseDepartmentId.HasValue, c => c.UseDepartmentId == criteria.UseDepartmentId)
                .WhereIf(criteria.ResPersonId.HasValue, c => c.ResPersonId == criteria.ResPersonId));
            }
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();
            elo.LoadWith(SpecialEquipmentAccount.EquipModelProperty);
            elo.LoadWith(SpecialEquipmentAccount.ResPersonProperty);
            elo.LoadWith(SpecialEquipmentAccount.UseDepartmentProperty);
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, elo);
        }

        /// <summary>
        /// 保存选择的检验规程的检验项目数据
        /// </summary>
        /// <param name="regularInspectionInfos"></param>
        public virtual void SaveSelRegularInspection(List<RegularInspection> regularInspectionInfos)
        {
            if (regularInspectionInfos == null)
            {
                return;
            }
            EntityList<RegularInspection> savedData = new EntityList<RegularInspection>();
            foreach (var item in regularInspectionInfos)
            {
                var checkProject = new RegularInspection();
                checkProject.Id = item.Id;

                checkProject.InspectionRuleId = item.InspectionRuleId;
                savedData.Add(checkProject);
            }
            RF.Save(savedData);
        }


        /// <summary>
        /// 特种设备定检新增页面选择点检项目
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<SelInspectionProjectViewModel> GetInspectionProjects(SelInspectionProjectCriteria criteria)
        {
            var lst = new EntityList<SelInspectionProjectViewModel>();
            if (criteria.SpecialEquipmentAccountId == null || criteria.InspectionRuleId == null)
            {
                return lst;
            }
            var query = Query<InspectionProjectItem>();

            if (criteria.Name.IsNotEmpty())
            {
                query.Where(p => p.ProjectDetail.Name.Contains(criteria.Name));
            }
            if (criteria.ProjectType.HasValue)
            {
                query.Where(p => p.ProjectDetail.ProjectType == criteria.ProjectType);
            }
            var items = query.Where(p => p.InspectionRuleId == criteria.InspectionRuleId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            foreach (var item in items)
            {
                SelInspectionProjectViewModel model = new SelInspectionProjectViewModel();
                model.ProjectDetailId = item.ProjectDetailId;
                model.Name = item.ProjectName;
                model.Part = item.Part;
                model.Consumable = item.Consumable;
                model.Method = item.Method;
                model.Standard = item.Standard;
                model.MinValue = item.MinValue;
                model.MaxValue = item.MaxValue;
                model.Unit = item.Unit;
                model.UseTime = item.UseTime;
                model.CycleType = item.CycleType;
                lst.Add(model);
            }
            return lst;
        }


        /// <summary>
        /// 根绝设备型号获取检验规程
        /// </summary>
        /// <param name="SpecialEquipmentAccountId"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<InspectionRule> GetInspectionRuleList(double SpecialEquipmentAccountId, PagingInfo pagingInfo, string keyword)
        {
            var query = Query<InspectionRule>();
            query.Exists<EquipAccountRegularInspection>((i, e) => e.Where(c => c.InspectionRuleId == i.Id)
            .Where(c => c.SpecialEquipmentAccountId == SpecialEquipmentAccountId));
            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
            }

            var list = query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 创建或修改特种设备定检
        /// </summary>
        /// <param name="ri">待保存的特种设备定检</param>
        /// <returns></returns>
        public virtual void CreateOrEditRegularInspection(RegularInspection ri)
        {
            if (ri == null)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(ri.InspectionNo))
            {
                ri.InspectionNo = GetInspectionNo().FirstOrDefault();
                ri.InspectionStatus = InspectionStatus.Pending;
                ri.ApprovalStatus = ApprovalStatus.Draft;

                //手动创建的特种设备定检单，备注必须输入
                if (!ri.Remark.IsNotEmpty())
                {
                    throw new ValidationException("备注必须输入!".L10N());
                }

                //数据来源手工创建
                ri.BillSourceType = BillSourceType.Manual;
                //创建操作记录对象
                RegularInspectionResume resume = CreateRegularInspectionResume(ri, OperationType.CREATE);
                ri.RegularInspectionResumeList.Add(resume);
            }

            using (var tran = DB.TransactionScope(EntityDataProvider.ConnectionStringName))
            {
                if (ri.InspectionNo.IsNotEmpty())
                {
                    RF.Save(ri);
                }
                tran.Complete();
            }
        }

        /// <summary>
        /// 创建操作记录对象
        /// </summary>
        /// <param name="regularInspection">设备定检对象</param>
        /// <param name="operationType">操作类型</param>
        /// <returns></returns>
        public virtual RegularInspectionResume CreateRegularInspectionResume(RegularInspection regularInspection, OperationType operationType)
        {
            if (regularInspection == null)
            {
                return new RegularInspectionResume();
            }
            RegularInspectionResume resume = new RegularInspectionResume();
            resume.GenerateId();
            resume.RegularInspectionId = regularInspection.Id;
            resume.OperationType = operationType;
            resume.InspectionResult = regularInspection.InspectionResult;
            resume.OperatorId = RT.IdentityId;
            resume.OperationDateTime = DateTime.Now;
            resume.CreateBy = RT.IdentityId;
            resume.CreateDate = DateTime.Now;
            return resume;
        }


        /// <summary>
        /// 获取新的销售订单编号
        /// </summary>
        /// <param name="qty">生成多少个新的销售订单编号</param>
        /// <returns>返回销售订单编号集合</returns>
        public virtual List<string> GetInspectionNo(int qty = 1)
        {
            var config = ConfigService.GetConfig(new NoConfig(), typeof(RegularInspection));
            if (config == null || config.NumberRuleId == null)
            {
                throw new ValidationException("未找到特种设备定检配置规则，请检查规则配置".L10N());
            }
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.BacodeRule.Id, qty).ToList();
        }



        /// <summary>
        /// 根据明细ID查询数据集合
        /// </summary>
        /// <param name="dtlIds"></param>
        /// <returns></returns>
        public virtual EntityList<RegularInspectionValue> GetDetailValues(List<double> dtlIds)
        {
            if (dtlIds.IsNotEmpty())
            {
                return Query<RegularInspectionValue>().Where(p => dtlIds.Contains(p.RegularInspectionDetailId)).ToList();
            }
            else
            {
                return new EntityList<RegularInspectionValue>();
            }
        }



        #region 录入命令方法

        /// <summary>
        /// 录入保存
        /// </summary>
        /// <param name="ri">特种设备定检</param>
        public virtual void SaveRegularInspection(RegularInspection ri)
        {
            if (ri == null)
            {
                return;
            }
            CheckBillState(ri);//并行操作校验
                               //获取完整单据信息，包括项目明细和样本值
            RegularInspection rie = GetAllBillInfo(ri);

            //变更状态检验中
            rie.InspectionStatus = InspectionStatus.Under;
            if (rie.ActualInspectionDate == null)
            {
                rie.ActualInspectionDate = DateTime.Today;
            }
            //打开录入数据，保存，状态更新为“检验中”同步更新特种设备台账中的定检状态为“检验中”；
            var equAccount = GetById<SpecialEquipmentAccount>(rie.SpecialEquipmentAccountId);

            RegularInspectionResume resume = CreateRegularInspectionResume(rie, OperationType.INPUT);
            rie.RegularInspectionResumeList.Add(resume);

            if (rie.RegularInspectionDetailList.Count == 0 && !GetRegularInspectionAttachment(rie.Id))
            {
                throw new ValidationException("录入保存必须添加明细或上传附件".L10N());
            }

            using (var tran = DB.TransactionScope(EntityDataProvider.ConnectionStringName))
            {
                if (equAccount != null)
                {
                    RF.Save(equAccount);
                    RF.Save(ri);
                }
                tran.Complete();
            }
        }

        /// <summary>
        /// 录入提交
        /// </summary>
        /// <param name="ri">特种设备定检</param>
        public virtual void SumbitRegularInspection(RegularInspection ri)
        {
            if (ri == null)
            {
                return;
            }

            ValidationRegularInspection(ri);
            //状态修改为校验中
            ri.InspectionStatus = InspectionStatus.Under;
            //状态修改为待审核
            ri.ApprovalStatus = ApprovalStatus.PendingReview;
            //创建操作记录-状态为提交
            RegularInspectionResume resume = CreateRegularInspectionResume(ri, OperationType.SUBMIT);
            ri.RegularInspectionResumeList.Add(resume);

            //检验报告录入`提交`时，按`设备编码`+`检验规程`更新`设备定检规程`中的`下次检验日期`为`当前日期（不含时分秒、数据库日期）`+`检验规程的周期`；
            var inspectionRule = GetById<InspectionRule>(ri.InspectionRuleId);
            DateTime? NextInspectionDate = null;
            if (inspectionRule != null)
            {
                NextInspectionDate = DateTime.Today.AddDays(inspectionRule.PeriodDays);
            }
            var now = RF.Find<RegularInspection>().GetDbTime();
            List<double> ids = new List<double>() { ri.Id };

            EquipAccountRegularInspection reginc = null;
            //手动创建不反写上，下次检验时间
            if (ri.BillSourceType == BillSourceType.Automatically)
            {
                reginc = RT.Service.Resolve<SpecialEquipAccountController>().GetEquipAccountRegularInspection(ri.SpecialEquipmentAccountId, ri.InspectionRuleId);
                reginc.PrevInspectionDate = DateTime.Today;
                reginc.NextInspectionDate = NextInspectionDate;
                //反写之后标识此数据已经提交，可再次自动生成此数据
                reginc.NotSubmit = true;
            }

            //根据设备清单中的设备台账为每一台设备创建设备台账特种设备定检类型的设备履历
            EquipAccountResume res = GenerateEquipAccountResume(ri);

            //先判断计量设备定检是否需要审核,是否开启审核流程
            var config = RT.Service.Resolve<ApprovalController>().GetApprovalConfigValue(typeof(RegularInspection));
            using (var tran = DB.TransactionScope(EntityDataProvider.ConnectionStringName))
            {
                //提交时保存设备台账的操作记录
                if (res != null)
                {
                    RF.Save(res);
                }

                //根据特种设备台账Id和设备定检规程Id修改特种设备台账设备定检规程的下次检验时间
                if (reginc != null)
                {
                    RF.Save(reginc);
                }

                //保存审核记录信息
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(ids, typeof(RegularInspection).FullName, ApprovalResult.Submit, now, ri.ApprovalInfo);
                RF.Save(ri);
                if (!config.EnableAudit)
                {
                    //【是否启用审批】为否时,提交的同时进行审批
                    AuditSumbitRecordInner(ri);
                }
                tran.Complete();
            }
        }

        /// <summary>
        /// 验证录入提交的特种设备定检
        /// </summary>
        /// <param name="ri">特种设备定检</param>
        public virtual void ValidationRegularInspection(RegularInspection ri)
        {
            if (ri == null)
            {
                return;
            }

            if (ri.InspectionResult == InspectionResult.Fail && !ri.InspectionRemark.IsNotEmpty())
            {
                throw new ValidationException("检验结果为不合格时,请输入检验说明!".L10N());
            }
            if (ri.ApprovalStatus == null)
            {
                throw new ValidationException("审核状态不能为空!".L10N());
            }
            if (ri.ActualInspectionDate == null)
            {
                throw new ValidationException("检验日期不能为空!".L10N());
            }

            foreach (var item in ri.RegularInspectionDetailList)
            {
                if (item.InspectionResult == null)
                {
                    throw new ValidationException("检验明细的检验结果不能为空!".L10nFormat(item.ProjectName));
                }
            }
            //检验类别为外校
            if (ri.CheckCategory == CheckCategory.OutCheck && !GetRegularInspectionAttachment(ri.Id))
            {
                throw new ValidationException("检验类别为外校,检验报告附件须上传!".L10N());
            }
        }


        /// <summary>
        /// 根据特种设备定检ID查询是否有附件
        /// </summary>
        /// <param name="riaId">特种设备定检ID</param>
        public virtual bool GetRegularInspectionAttachment(double riaId)
        {
            return Query<RegularInspectionAttachment>().Where(p => p.OwnerId == riaId).Count() > 0;
        }


        /// <summary>
        /// 并行操作校验
        /// </summary>
        /// <param name="bill"></param>
        private static void CheckBillState(RegularInspection bill)
        {
            // 保存之前先确认此单据是否已经由其他人员操作保存或提交，防止并发操作时程序发生错误
            var result = RF.GetById<RegularInspection>(bill.Id);
            if (bill.InspectionStatus == InspectionStatus.Pending && result != null && result.InspectionStatus != InspectionStatus.Pending)
            {
                throw new ValidationException("此单据已由其他人员操作并保存，请退出当前界面重新操作".L10N());
            }
            if (result != null && (result.ApprovalStatus == ApprovalStatus.PendingReview || result.ApprovalStatus == ApprovalStatus.UnderReview || result.InspectionStatus == InspectionStatus.Calirated))
            {
                throw new ValidationException("此单据已由其他人员提交，请退出当前界面重新操作".L10N());
            }
        }

        /// <summary>
        /// 获取完整单据信息
        /// </summary>
        /// <param name="bill">提交的单据</param>
        /// <returns></returns>
        public virtual RegularInspection GetAllBillInfo(RegularInspection bill)
        {
            if (bill == null)
            {
                return new RegularInspection();
            }
            var srcBill = RF.GetById<RegularInspection>(bill.Id, new EagerLoadOptions().LoadWithViewProperty().LoadWith(RegularInspection.RegularInspectionDetailListProperty).LoadWith(RegularInspectionDetail.RegularInspectionValueListProperty));//数据库单据信息
            if (srcBill == null)
            {
                return bill;
            }
            //先补充提交项目的样本值，再补充原有的项目，减少原有项目再去走获取样本值的逻辑
            foreach (var billDetail in bill.RegularInspectionDetailList)
            {
                var srcDetail = srcBill.RegularInspectionDetailList.FirstOrDefault(p => p.Id == billDetail.Id);
                if (srcDetail != null)
                {
                    foreach (var srcValue in srcDetail.RegularInspectionValueList)
                    {
                        if (billDetail.RegularInspectionValueList.All(p => p.Id != srcValue.Id) && billDetail.RegularInspectionValueList.DeletedList.All(p => Convert.ToDouble(p.GetId()) != srcValue.Id))
                        {
                            billDetail.RegularInspectionValueList.Add(srcValue);
                        }
                    }
                }
                //拿到项目的所有样本（剔除要删除的样本），并对样本的Index重新排序
                var tempValList = billDetail.RegularInspectionValueList.ToList();
                tempValList.Sort((x, y) => x.Index.CompareTo(y.Index));
                for (int i = 0; i < tempValList.Count; i++)
                {
                    tempValList[i].Index = i + 1;
                }
            }

            foreach (var srcBillDetail in srcBill.RegularInspectionDetailList)
            {
                if (bill.RegularInspectionDetailList.All(x => x.Id != srcBillDetail.Id) && bill.RegularInspectionDetailList.DeletedList.All(x => Convert.ToDouble(x.GetId()) != srcBillDetail.Id))
                {
                    bill.RegularInspectionDetailList.Add(srcBillDetail);
                }
            }
            return bill;
        }
        #endregion

        #region 审核命令方法

        /// <summary>
        /// 审核并行操作校验
        /// </summary>
        /// <param name="bill"></param>
        private static void CheckBillAuditState(RegularInspection bill)
        {
            // 保存之前先确认此单据是否已经由其他人员操作保存或提交，防止并发操作时程序发生错误
            var result = RF.GetById<RegularInspection>(bill.Id);
            if (bill.ApprovalStatus == ApprovalStatus.PendingReview && result != null && result.ApprovalStatus != ApprovalStatus.PendingReview)
            {
                throw new ValidationException("此单据已由其他人员操作并保存，请退出当前界面重新操作".L10N());
            }
        }


        /// <summary>
        /// 驳回校验记录
        /// </summary>
        /// <param name="ri"></param>
        public virtual void AuditRejectRecord(RegularInspection ri)
        {
            if (ri == null)
            {
                return;
            }

            CheckBillAuditState(ri);//并行操作校验
            ri.ApprovalStatus = ApprovalStatus.Reject;
            var now = RF.Find<RegularInspection>().GetDbTime();
            List<double> ids = new List<double>() { ri.Id };

            using (var tran = DB.TransactionScope(EntityDataProvider.ConnectionStringName))
            {
                //保存特种设备定检记录
                RF.Save(ri);
                //保存审核记录信息
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(ids, typeof(RegularInspection).FullName, ApprovalResult.Reject, now, ri.ApprovalInfo);
                tran.Complete();
            }
        }

        /// <summary>
        /// 保存校验记录
        /// </summary>
        /// <param name="ri"></param>
        public virtual void AuditSaveRecord(RegularInspection ri)
        {
            if (ri == null)
            {
                return;
            }
            CheckBillAuditState(ri);//并行操作校验
            if (ri.ApprovalStatus == ApprovalStatus.PendingReview)
            {
                ri.ApprovalStatus = ApprovalStatus.UnderReview;
            }
            RF.Save(ri);
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="ri"></param>
        public virtual void AuditSumbitRecord(RegularInspection ri)
        {
            //获取完整单据信息，包括项目明细和样本值
            using (var trans = DB.TransactionScope(EntityDataProvider.ConnectionStringName))
            {
                AuditSumbitRecordInner(ri);
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="ri"></param>
        public virtual void AuditSumbitRecordInner(RegularInspection ri)
        {
            if (ri == null)
            {
                return;
            }

            //无审核意见,默认通过
            if (ri.ApprovalInfo.IsNullOrEmpty())
            {
                ri.ApprovalInfo = "通过".L10N();
            }

            CheckBillAuditState(ri);//并行操作校验

            ri.ApprovalStatus = ApprovalStatus.Audited;
            ri.InspectionStatus = InspectionStatus.Calirated;

            var now = RF.Find<RegularInspection>().GetDbTime();
            List<double> ids = new List<double>() { ri.Id };

            //更新设备台账
            DB.Update<SpecialEquipmentAccount>().Set(P => P.RegularInspectionStatus, ri.InspectionResult == InspectionResult.Fail ? RegularInspectionStatus.NG : RegularInspectionStatus.OK)
              .Where(p=>p.Id == ri.SpecialEquipmentAccountId).Execute();
            RF.Save(ri);
            //保存审核记录信息
            RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(ids, typeof(RegularInspection).FullName, ApprovalResult.Pass, now, ri.ApprovalInfo);
        }

        #endregion

        #region 自动生成特种设备定检计划
        /// <summary>
        /// 自动生成特种设备定检计划
        /// </summary>
        public virtual void AtuoSchedule()
        {
            AtuoScheduleHandle handle = new AtuoScheduleHandle();
            handle.DoSchedule();
        }
        #endregion

        #region 生成设备台账的特种设备定检类型的设备履历

        /// <summary>
        /// 生成设备台账的特种设备定检类型的设备履历
        /// </summary>
        /// <param name="regularIn"></param>
        /// <returns></returns>
        public virtual EquipAccountResume GenerateEquipAccountResume(RegularInspection regularIn)
        {
            if (regularIn == null)
            {
                return new EquipAccountResume();
            }
            EquipAccountResume res = new EquipAccountResume();
            res.No = regularIn.InspectionNo;
            res.EquipAccountId = regularIn.SpecialEquipmentAccountId;
            res.ResumeType = ResumeType.RegularInspection;
            res.State = regularIn.SpecialEquipmentAccount.State;
            return res;
        }
        #endregion

        /// <summary>
        /// 采购对象获取定检任务单号
        /// </summary>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <returns>定检任务单号</returns>
        public virtual List<string> PurchaseGetRegularInspectionNo(PagingInfo pagingInfo, string keyword)
        {
            var list = Query<RegularInspection>().Join<InspectionRule>((a, b) => a.InspectionRuleId == b.Id && b.CheckCategory == CheckCategory.OutCheck)
                .Select(p => p.InspectionNo)
                .Where(p => p.InspectionStatus == InspectionStatus.Pending || p.InspectionStatus == InspectionStatus.Under)
                .WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.InspectionNo.Contains(keyword)).ToList(pagingInfo);
            return list.Select(p => p.InspectionNo).ToList();
        }

        /// <summary>
        /// 关闭未完结的特种设备定检任务
        /// </summary>
        /// <param name="equipAccountIds">设备台账Id</param>
        /// <returns></returns>
        public virtual void CloseRegularInspectionByEquipAccountIds(IList<double> equipAccountIds)
        {
            var regularInspections = equipAccountIds.SplitContains(tempIds =>
            {
                return Query<RegularInspection>().Where(p => tempIds.Contains(p.SpecialEquipmentAccountId)
                && (p.InspectionStatus == InspectionStatus.Pending || p.InspectionStatus == InspectionStatus.Under)).ToList();
            });

            regularInspections.ForEach(bill =>
            {
                bill.InspectionStatus = InspectionStatus.Closed;
            });

            RF.Save(regularInspections);
        }

    }
}
