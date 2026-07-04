using SIE.Common;
using SIE.Common.Catalogs;
using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.NumberRules;
using SIE.Common.Sort;
using SIE.Core.Enums;
using SIE.DataAuth;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Enums;
using SIE.EMS.InspectionRules;
using SIE.EMS.MeteringEquipment.Calibrations.Criterias;
using SIE.EMS.MeteringEquipment.Calibrations.Handle;
using SIE.EMS.MeteringEquipment.Calibrations.ViewModels;
using SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts;
using SIE.Equipments.Common.Controller;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using SIE.Equipments.WorkFlows;
using SIE.EventMessages.EMS.MeteringEquipments;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.MeteringEquipment.Calibrations
{
    /// <summary>
    /// 计量设备定检控制器
    /// </summary>
    public class CalibrationController : DomainController, ICalibration
    {
        /// <summary>
        /// 获取订单评审配置项
        /// </summary>
        public virtual EntityList<Calibration> GetCalibrationList(CalibrationCriteria criteria)
        {
            var query = Query<Calibration>();
            if (criteria.InspectionNo.IsNotEmpty())
            {
                query.Where(p => p.InspectionNo.Contains(criteria.InspectionNo));
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

            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();
            elo.LoadWith(CalibrationEquipment.MeteringEquipmentAccountProperty);
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, elo);
        }


        /// <summary>
        /// 保存计量设备定检
        /// </summary>
        public virtual void SaveCalibration(Calibration cal)
        {
            if (cal == null)
            {
                return;
            }
            cal.InspectionStatus = InspectionStatus.Pending;
            cal.ApprovalStatus = ApprovalStatus.Draft;
            cal.BillSourceType = BillSourceType.Manual;

            if (cal.InspectionRule.CheckCategory == CheckCategory.OutCheck && !cal.AgencyId.HasValue)
            {
                throw new ValidationException("检验机构不能为空!".L10N());
            }
            if (!cal.Remark.IsNotEmpty())
            {
                throw new ValidationException("备注必须输入!".L10N());
            }

            if (!cal.CalibrationEquipmentList.Any() || cal.CalibrationEquipmentList.Count == 0)
            {
                throw new ValidationException("请选择设备明细清单!".L10N());
            }
            if (!cal.CalibrationItemList.Any() || cal.CalibrationItemList.Count == 0)
            {
                throw new ValidationException("请选择检验规程清单!".L10N());
            }

            //创建操作记录对象
            CalibrationResume resume = CreateCalibrationResume(cal, OperationType.CREATE);
            cal.CalibrationResumeList.Add(resume);

            RF.Save(cal);

        }


        #region 录入检验报告
        /// <summary>
        /// 录入保存
        /// </summary>
        /// <param name="cal">计量设备定检</param>
        public virtual void SaveCalibrationInput(Calibration cal)
        {
            if (cal == null)
            {
                return;
            }

            //并行操作校验
            CheckBillState(cal);

            //变更状态检验中
            cal.InspectionStatus = InspectionStatus.Under;
            if (cal.ActualInspectionDate == null)
            {
                cal.ActualInspectionDate = DateTime.Today;
            }

            CalibrationResume resume = CreateCalibrationResume(cal, OperationType.INPUT);
            cal.CalibrationResumeList.Add(resume);

            using (var tran = DB.TransactionScope(EntityDataProvider.ConnectionStringName))
            {
                RF.Save(cal);
                tran.Complete();
            }
        }


        /// <summary>
        /// 录入提交
        /// </summary>
        /// <param name="cal">计量设备定检</param>
        public virtual void SumbitCalibrationInput(Calibration cal)
        {
            if (cal == null)
            {
                return;
            }

            ValidationRegularInspection(cal);
            //状态修改为校验中
            cal.InspectionStatus = InspectionStatus.Under;
            //状态修改为待审核
            cal.ApprovalStatus = ApprovalStatus.PendingReview;

            //创建操作记录-状态为提交
            CalibrationResume resume = CreateCalibrationResume(cal, OperationType.SUBMIT);
            cal.CalibrationResumeList.Add(resume);

            var now = RF.Find<Calibration>().GetDbTime();
            List<double> ids = new List<double>() { cal.Id };

            //根绝设备明细清单id集合与主数据的检验规程id获取   计量设备定检项目
            var MeteringEquipmentAccountIds = cal.CalibrationEquipmentList.Select(p => p.MeteringEquipmentAccountId).ToList();
            EntityList<EquipAccountCalibration> regincList = RT.Service.Resolve<MeteringEquipmentAccountController>().GetEquipAccountCalibration(MeteringEquipmentAccountIds, cal.InspectionRuleId);

            EntityList<MeteringEquipmentAccount> EquipmentAccountList = RT.Service.Resolve<MeteringEquipmentAccountController>().GetMeteringEquipmentAccountList(MeteringEquipmentAccountIds);

            //反写上，下次检验时间
            foreach (var equipaccount in cal.CalibrationEquipmentList)
            {
                var equip = regincList.FirstOrDefault(p => p.MeteringEquipmentAccountId == equipaccount.MeteringEquipmentAccountId && p.InspectionRuleId == cal.InspectionRuleId);

                var meteringEquipmentAccount = EquipmentAccountList.FirstOrDefault(p => p.Id == equipaccount.MeteringEquipmentAccountId);
                if (equip != null)
                {
                    //设备台账计量检验规程中的 上次检验日期=设备台账明细中具体台账的检验日期
                    equip.PrevInspectionDate = equipaccount.InspectionDate;
                    //下次检验日期=检验日期+周期
                    equip.NextInspectionDate = equipaccount.InspectionDate.Value.AddDays(equip.PeriodDays);
                    equip.NotSubmit = true;
                    //反写计量设备台账的下次检验日期
                    meteringEquipmentAccount.NextInspectionDate = equip.NextInspectionDate;
                }
            }

            //根据设备清单中的设备台账为每一台设备创建设备台账计量设备定检类型的设备履历
            EntityList<EquipAccountResume> resList = GenerateEquipAccountResume(cal);
            //先判断计量设备定检是否需要审核,是否开启审核流程
            var config = RT.Service.Resolve<ApprovalController>().GetApprovalConfigValue(typeof(Calibration));
            using (var tran = DB.TransactionScope(EntityDataProvider.ConnectionStringName))
            {
                //保存设备台账中的设备履历信息
                if (resList.Any())
                {
                    RF.Save(resList);
                }

                //反写计量设备台账的下次检验日期
                if (EquipmentAccountList.Any())
                {
                    RF.Save(EquipmentAccountList);
                }

                //根据计量设备台账Id和设备定检规程Id修改计量设备台账设备定检规程的下次检验时间
                if (regincList.Any())
                {
                    RF.Save(regincList);
                }

                //保存审核记录信息
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(ids, typeof(Calibration).FullName, ApprovalResult.Submit, now, cal.ApprovalInfo);
                RF.Save(cal);
                if (!config.EnableAudit)
                {
                    //【是否启用审批】为否时,提交的同时进行审批
                    AuditSumbitRecordInner(cal);
                }
                tran.Complete();
            }
        }


        /// <summary>
        /// 验证录入提交的计量设备定检
        /// </summary>
        /// <param name="cal">计量设备定检</param>
        public virtual void ValidationRegularInspection(Calibration cal)
        {
            if (cal == null)
            {
                return;
            }

            if (cal.ActualInspectionDate == null)
            {
                throw new ValidationException("检验日期不能为空!".L10N());
            }
            if (cal.InspectionResult == null)
            {
                throw new ValidationException("检验结果不能为空!".L10N());
            }
            if (!cal.InspectorId.HasValue)
            {
                throw new ValidationException("检验人不能为空!".L10N());
            }
            if (cal.InspectionRule.CheckCategory == CheckCategory.OutCheck && !cal.AgencyId.HasValue)
            {
                throw new ValidationException("检验机构不能为空!".L10N());
            }
            if (cal.InspectionResult == InspectionResult.Fail && !cal.InspectionRemark.IsNotEmpty())
            {
                throw new ValidationException("检验结果为不合格时,请输入检验说明!".L10N());
            }

            List<AccountUseState> UseState = new List<AccountUseState>() {
             AccountUseState.Using,AccountUseState.Overdue, AccountUseState.Failed, AccountUseState.Repair,AccountUseState.OutsourcedRepair
             };

            List<double> ids = cal.CalibrationEquipmentList.Select(p => p.MeteringEquipmentAccountId).ToList();
            var dic = RT.Service.Resolve<MeteringEquipmentAccountController>().GetMeteringEquipmentAccountList(ids).GroupBy(p => p.Id).ToDictionary(p => p.Key, p => p.FirstOrDefault());

            MeteringEquipmentAccount meq;

            foreach (var item in cal.CalibrationEquipmentList)
            {
                if (dic.TryGetValue(item.MeteringEquipmentAccountId, out meq) && meq != null)
                {
                    //只有设备状态为使用中，超期停用，不合格停用，维修，委外维修才能提交
                    if (!UseState.Contains(meq.UseState))
                    {
                        throw new ValidationException("设备明细【{0}】的计量设备管理状态非使用中,超期停用,不合格停用,维修,委外维修状态请强制关单!".L10nFormat(meq.Name));
                    }
                    if (item.InspectionResult == null)
                    {
                        throw new ValidationException("设备明细【{0}】的检验结果不能为空!".L10nFormat(meq.Name));
                    }
                    if (item.InspectionDate == null)
                    {
                        throw new ValidationException("设备明细【{0}】的实际检验日期不能为空!".L10nFormat(meq.Name));
                    }
                }
                else
                {
                    if (item.MeteringEquipmentAccount == null)
                    {
                        throw new ValidationException("设备明细的设备台账不存在!".L10N());
                    }
                    else
                    {
                        throw new ValidationException("设备明细【{0}】的设备台账不存在!".L10nFormat(item.MeteringEquipmentAccount.Name));
                    }
                }
            }

            //检验类别为外校
            if (cal.CheckCategory == CheckCategory.OutCheck && !GetCalibrationAttachment(cal.Id))
            {
                throw new ValidationException("检验类别为外校,检验报告附件须上传!");
            }
        }


        /// <summary>
        /// 根据计量设备定检ID查询是否有附件
        /// </summary>
        /// <param name="riaId">计量设备定检ID</param>
        public virtual bool GetCalibrationAttachment(double riaId)
        {
            return Query<CalibrationAttachment>().Where(p => p.OwnerId == riaId).Count() > 0;
        }


        /// <summary>
        /// 并行操作校验
        /// </summary>
        /// <param name="bill"></param>
        private static void CheckBillState(Calibration bill)
        {
            // 保存之前先确认此单据是否已经由其他人员操作保存或提交，防止并发操作时程序发生错误
            var result = RF.GetById<Calibration>(bill.Id);
            if (bill.InspectionStatus == InspectionStatus.Pending && result != null && result.InspectionStatus != InspectionStatus.Pending)
            {
                throw new ValidationException("此单据已由其他人员操作并保存，请退出当前界面重新操作".L10N());
            }
            if (result != null && (result.ApprovalStatus == ApprovalStatus.PendingReview || result.ApprovalStatus == ApprovalStatus.UnderReview || result.InspectionStatus == InspectionStatus.Calirated))
            {
                throw new ValidationException("此单据已由其他人员提交，请退出当前界面重新操作".L10N());
            }
        }

        #endregion


        #region 审核检验报告

        /// <summary>
        /// 审核并行操作校验
        /// </summary>
        /// <param name="bill"></param>
        private static void CheckBillAuditState(Calibration bill)
        {
            // 保存之前先确认此单据是否已经由其他人员操作保存或提交，防止并发操作时程序发生错误
            var result = RF.GetById<Calibration>(bill.Id);
            if (bill.ApprovalStatus == ApprovalStatus.PendingReview && result != null && result.ApprovalStatus != ApprovalStatus.PendingReview)
            {
                throw new ValidationException("此单据已由其他人员操作并保存，请退出当前界面重新操作".L10N());
            }
        }


        /// <summary>
        /// 驳回校验记录
        /// </summary>
        /// <param name="ri"></param>
        public virtual void AuditRejectRecord(Calibration ri)
        {
            if (ri == null)
            {
                return;
            }
            if (!ri.ApprovalInfo.IsNotEmpty())
            {
                throw new ValidationException("审核意见必填。".L10N());
            }

            CheckBillAuditState(ri);//并行操作校验
            ri.ApprovalStatus = ApprovalStatus.Reject;
            var now = RF.Find<Calibration>().GetDbTime();
            List<double> ids = new List<double>() { ri.Id };



            using (var tran = DB.TransactionScope(EntityDataProvider.ConnectionStringName))
            {
                //保存特种设备定检记录
                RF.Save(ri);
                //保存审核记录信息
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(ids, typeof(Calibration).FullName, ApprovalResult.Reject, now, ri.ApprovalInfo);
                tran.Complete();
            }
        }

        /// <summary>
        /// 审核提交
        /// </summary>
        /// <param name="ri"></param>
        public virtual void AuditSumbitRecord(Calibration ri)
        {
            if (ri == null)
            {
                return;
            }

            //获取完整单据信息，包括项目明细和样本值
            using (var trans = DB.TransactionScope(EntityDataProvider.ConnectionStringName))
            {
                AuditSumbitRecordInner(ri);
                trans.Complete();
            }
        }


        /// <summary>
        /// 审核提交
        /// </summary>
        /// <param name="ri"></param>
        public virtual void AuditSumbitRecordInner(Calibration ri)
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

            var now = RF.Find<Calibration>().GetDbTime();
            List<double> ids = new List<double>() { ri.Id };

            //根绝设备明细清单id集合与主数据的检验规程id获取   计量设备定检项目
            var MeteringEquipmentAccountIds = ri.CalibrationEquipmentList.Select(p => p.MeteringEquipmentAccountId).ToList();
            EntityList<MeteringEquipmentAccount> regincList = RT.Service.Resolve<MeteringEquipmentAccountController>().GetMeteringEquipmentAccountList(MeteringEquipmentAccountIds);

            //反写定检状态
            foreach (var equipaccount in ri.CalibrationEquipmentList)
            {
                var equip = regincList.FirstOrDefault(p => p.Id == equipaccount.MeteringEquipmentAccountId);

                if (equip != null)
                {
                    //根据设备清单中设备的检验结果反写定检状态
                    if (equipaccount.InspectionResult.Value == InspectionResult.Pass)
                    {
                        //检验结果为合格时：更新定检状态为“合格”；
                        equip.RegularInspectionStatus = RegularInspectionStatus.OK;
                    }
                    else
                    {
                        //检验结果为不合格时：更新设备台账中的定检状态为“不合格”；同时更新资产管理状态“不合格停用”
                        equip.RegularInspectionStatus = RegularInspectionStatus.NG;
                        equip.UseState = AccountUseState.Failed;
                    }
                }
            }
            //更新设备台账
            if (regincList.Any())
            {
                RF.Save(regincList);
            }
            RF.Save(ri);
            //保存审核记录信息
            RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(ids, typeof(Calibration).FullName, ApprovalResult.Pass, now, ri.ApprovalInfo);

        }



        #endregion

        #region 自动生成特种设备定检计划
        /// <summary>
        /// 自动生成特种设备定检计划
        /// </summary>
        public virtual void AtuoSchedule()
        {
            CalibrationAtuoScheduleHandle handle = new CalibrationAtuoScheduleHandle();
            handle.DoSchedule();
        }



        /// <summary>
        /// 获取计量设备台账下设备定检规程关联关系（并且上次检验日期和下次检验日期不可同时为null）
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<EquipAccountCalibration> GetEquipAccountCalibration()
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(EquipAccountCalibration.InspectionRuleProperty);
            elo.LoadWith(EquipAccountCalibration.MeteringEquipmentAccountProperty);
            elo.LoadWithViewProperty();

            List<AccountUseState> UseState = new List<AccountUseState>() {
             AccountUseState.Using,AccountUseState.Overdue, AccountUseState.Failed, AccountUseState.Repair,AccountUseState.OutsourcedRepair
             };

            using (DataAuths.LoadAll())
            {
                //只有设备台账状态为使用中，超期停用，不合格停用，维修，委外维修才能生成计量设备定检
                return Query<EquipAccountCalibration>()
                .Exists<MeteringEquipmentAccount>((a, b) => b.Where(c => a.MeteringEquipmentAccountId == c.Id && UseState.Contains(c.UseState)))
                .Where(p => p.NotSubmit == true && p.PrevInspectionDate != null && p.NextInspectionDate != null)
                .ToList(null, elo);
            }
        }


        #region 生成设备台账的计量设备定检类型的设备履历

        /// <summary>
        /// 生成设备台账的计量设备定检类型的设备履历
        /// </summary>
        /// <param name="cal"></param>
        /// <returns></returns>
        public virtual EntityList<EquipAccountResume> GenerateEquipAccountResume(Calibration cal)
        {
            EntityList<EquipAccountResume> resList = new EntityList<EquipAccountResume>();
            EquipAccountResume res = null;
            foreach (var item in cal.CalibrationEquipmentList)
            {
                res = new EquipAccountResume();
                res.No = cal.InspectionNo;
                res.EquipAccountId = item.MeteringEquipmentAccountId;
                res.ResumeType = ResumeType.Calibration;
                res.State = item.MeteringEquipmentAccountState;
                resList.Add(res);
            }
            return resList;
        }

        #endregion

        #endregion

        #region  新增页面选择的  ViewModel 查询方法

        /// <summary>
        /// 计量设备定检新增页面选择计量设备台账
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<SelCalibrationEquipmentModel> GetMeteringEquipmentAccountList(SelCalibrationEquipmentModelCriteria criteria)
        {
            var lst = new EntityList<SelCalibrationEquipmentModel>();

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
            if (criteria.EquipModelId.HasValue)
            {
                query.Where(p => p.EquipModelId == criteria.EquipModelId.Value);
            }

            query.Exists<EquipAccountCalibration>((x, y) => y.Where(c => c.MeteringEquipmentAccountId == x.Id).Where(c => c.InspectionRuleId == criteria.InspectionRuleId)
            );

            List<AccountUseState> UseState = new List<AccountUseState>() {
             AccountUseState.Using,AccountUseState.Overdue, AccountUseState.Failed, AccountUseState.Repair,AccountUseState.OutsourcedRepair
             };

            query.Exists<EquipModel>((x, y) => y.Where(p => p.Id == x.EquipModelId)
            .WhereIf((arraylist.Any() && arraylist.Count > 0), e => arraylist.Contains(e.TypeCategory))
            .WhereIf(criteria.EquipTypeId.HasValue, e => e.EquipTypeId == criteria.EquipTypeId))
            .Where(p => UseState.Contains(p.UseState));

            var items = query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            foreach (var item in items)
            {
                SelCalibrationEquipmentModel model = new SelCalibrationEquipmentModel();
                model.EquipAccountId = item.Id;
                model.Code = item.Code;
                model.Name = item.Name;
                model.Specifications = item.Specifications;
                model.EquipModelName = item.EquipModelName;
                model.EquipTypeName = item.EquipModelTypeName;
                model.UseDepartmentName = item.UseDepartmentName;
                model.Manufacturer = item.Manufacturer;
                model.CardDate = item.CardDate;
                model.UseState = item.UseState;
                model.IsDowngrade = item.Downgrade;
                model.PrecisionClass = item.PrecisionClass;
                lst.Add(model);
            }
            return lst;
        }


        /// <summary>
        /// 计量设备定检新增页面选择检验项目
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<SelCalibrationItemModel> GetInspectionProjects(SelCalibrationItemModelCriteria criteria)
        {
            var lst = new EntityList<SelCalibrationItemModel>();
            if (criteria.InspectionRuleId == null)
            {
                return lst;
            }
            //检验规程与点检项目明细表
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
                SelCalibrationItemModel model = new SelCalibrationItemModel();
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
                model.ProjectType = item.ProjectType;
                model.CycleType = item.CycleType;
                lst.Add(model);
            }
            return lst;
        }
        #endregion

        /// <summary>
        /// 创建操作记录对象
        /// </summary>
        /// <param name="calibration">计量定检对象</param>
        /// <param name="operationType">操作类型</param>
        /// <returns></returns>
        public virtual CalibrationResume CreateCalibrationResume(Calibration calibration, OperationType operationType)
        {
            if (calibration == null)
            {
                return new CalibrationResume();
            }
            CalibrationResume resume = new CalibrationResume();
            resume.GenerateId();
            resume.CalibrationId = calibration.Id;
            resume.OperationType = operationType;
            resume.InspectionResult = calibration.InspectionResult;
            resume.OperatorId = RT.IdentityId;
            resume.OperationDateTime = DateTime.Now;
            resume.CreateBy = RT.IdentityId;
            resume.CreateDate = DateTime.Now;
            return resume;
        }

        /// <summary>
        /// 获取计量设备定检编码
        /// </summary>
        /// <param name="Qty">编码个数</param>
        /// <returns>计量设备定检编码</returns>
        public virtual List<string> GetCalibrationNo(int Qty = 1)
        {
            var config = ConfigService.GetConfig(new NoConfig(), typeof(Calibration));
            if (config == null || config.BacodeRule == null)
            {
                throw new ValidationException("未找到编码生成规则,请检查规则配置".L10N());
            }
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.BacodeRule.Id, Qty).ToList();
        }


        /// <summary>
        /// 根据计量设备定检明细id取计量设备定检下所包含的所有 计量设备台账数据
        /// </summary>
        ///  <param name="calibrationId">计量设备定检Id</param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<CalibrationItem> GetCalibrationItemListByCalId(double calibrationId, PagingInfo pagingInfo, string keyword)
        {
            var query = Query<CalibrationItem>().Where(p => p.CalibrationId == calibrationId);
            if (keyword.IsNotEmpty())
            {
                query.Where(p => p.Name.Contains(keyword));
            }
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 根据计量设备定检明细id取计量设备定检下所包含的所有的检验规程
        /// </summary>
        ///  <param name="calibrationId">计量设备定检Id</param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<MeteringEquipmentAccount> GetMeteringEquipmentAccountListByCalId(double calibrationId, PagingInfo pagingInfo, string keyword)
        {
            var calibration = GetById<Calibration>(calibrationId);

            var meteringEquipmentAccountIds = calibration.CalibrationEquipmentList
                .Select(p => p.MeteringEquipmentAccountId)
                .ToList();

            return meteringEquipmentAccountIds.SplitContains(tempIds =>
            {
                var query = Query<MeteringEquipmentAccount>().Where(p => tempIds.Contains(p.Id));

                if (keyword.IsNotEmpty())
                {
                    query.Where(p => p.Code.Contains(keyword) || p.Name.Contains(keyword));
                }

                var list = query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

                list.ForEach(p =>
                {
                    p.TreePId = null;
                });

                return list;
            });            
        }

        /// <summary>
        /// 关闭未完结的计量设备定检任务
        /// </summary>
        /// <param name="equipAccountIds">设备台账Id</param>
        /// <returns></returns>
        public virtual void CloseCalibrationByEquipAccountIds(IList<double> equipAccountIds)
        {
            var calibrations = Query<Calibration>().Join<CalibrationEquipment>((p,n)=>p.Id == n.CalibrationId)
                                                         .Where<CalibrationEquipment>((p, n) => equipAccountIds.Contains(n.MeteringEquipmentAccountId))
                                                         .Where(p => (p.InspectionStatus == InspectionStatus.Pending || p.InspectionStatus == InspectionStatus.Under))
                                                         .ToList(null,new EagerLoadOptions().LoadWith(Calibration.CalibrationEquipmentListProperty));

            calibrations.ForEach(bill =>
            {
                if (bill.CalibrationEquipmentList.Count() == 1) 
                {
                    bill.InspectionStatus = InspectionStatus.Closed;
                }
            });

            RF.Save(calibrations);
        }

        /// <summary>
        /// 采购对象获取定检任务单号
        /// </summary>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">关键字</param>
        /// <returns>定检任务单号</returns>
        public virtual List<CalibrationObject> PurchaseGetCalibrationNo(PagingInfo pagingInfo, string keyword)
        {
            var list = Query<Calibration>().Join<InspectionRule>((a, b) => a.InspectionRuleId == b.Id && b.CheckCategory == CheckCategory.OutCheck)
                .Where(p => p.ApprovalStatus == ApprovalStatus.Draft)
                .WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.InspectionNo.Contains(keyword)).ToList(pagingInfo);
            return list.Select(p => new CalibrationObject
            {
                InspectionNo = p.InspectionNo,
                PlanNmae = p.PlanName
            }).ToList();
        }

        #region 公共方法

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="currentIndex"></param>
        /// <returns></returns>
        public virtual EntityList<Catalog> GetCatalogList(string type, double currentIndex)
        {
            return Query<Catalog>().Where(p => p.CatalogType.Code == type && p.GetProperty(SortExtension.INDEX_Property) > currentIndex)
                .ToList();
        }

        /// <summary>
        /// 获取精度等级快码数据
        /// </summary>
        /// <param name="calequ"></param>
        /// <param name="pageInfo"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual EntityList<Catalog> GetPrecisionClassCatalogList(CalibrationEquipment calequ, PagingInfo pageInfo, string key)
        {
            EntityList<Catalog> list = new EntityList<Catalog>();

            var EquipAccount = RT.Service.Resolve<MeteringEquipmentAccountController>().GetMeteringEquipmentAccountById(calequ.MeteringEquipmentAccountId);

            Catalog catalog = RT.Service.Resolve<CatalogController>().GetCatalog(CalibrationEquipment.PrecisionClassType, EquipAccount.PrecisionClass);

            if (catalog != null)
            {
                var index = catalog.GetProperty(SortExtension.INDEX_Property);

                if (calequ.IsDowngrade)
                {
                    return GetCatalogList(CalibrationEquipment.PrecisionClassType, index);
                }
                else
                {
                    list.Add(catalog);
                }
            }
            return list;
        }
        #endregion
    }
}
