using NPOI.SS.Formula.Functions;
using NPOI.SS.Formula.PTG;
using SIE.AbnormalInfo.AbnormalMonitors;
using SIE.Common.Catalogs;
using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.NumberRules;
using SIE.Core.ApiModels;
using SIE.Core.Common;
using SIE.Core.Common.Controllers;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Checks.Plans;
using SIE.EMS.Common.Utils;
using SIE.EMS.DataAuth;
using SIE.EMS.DevicePurs;
using SIE.EMS.Devices.Abnormals;
using SIE.EMS.Enums;
using SIE.EMS.Equipments;
using SIE.EMS.Equipments.AlarmStates;
using SIE.EMS.Equipments.ApiModels;
using SIE.EMS.EquipRepair.ApiModels;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.EMS.EquipRepair.EquipRepairs.Configs;
using SIE.EMS.EquipRepair.EquipRepairs.Enums;
using SIE.EMS.EquipRepair.EquipRepairs.ViewModels;
using SIE.EMS.EquipRepair.EquipRepairs.ViewModels.Criterias;
using SIE.EMS.EquipRepair.ExperienceDepots;
using SIE.EMS.EquipRepair.ExperienceDepots.Attachments;
using SIE.EMS.EquipRepair.ExperienceDepots.Controllers;
using SIE.EMS.EquipRepairs.Enums;
using SIE.EMS.Maintains.Plans;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Applys;
using SIE.EMS.SpareParts.Applys.Controllers;
using SIE.EMS.SpareParts.Applys.Details;
using SIE.EMS.SpareParts.Applys.Enums;
using SIE.EMS.SpareParts.OutDepotHandovers;
using SIE.EMS.SpareParts.OutDepots.Controllers;
using SIE.EMS.SpareParts.OutDepots.Details;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using SIE.EventMessages.EMS.Repairs;
using SIE.Rbac.Users;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.EMS.EquipRepair.Controller
{
    /// <summary>
    /// 设备维修控制器
    /// </summary>
    public partial class RepairController : DomainController, IEquipRepairBill
    {
        #region 生成编码
        /// <summary>
        /// 获取自动编码No
        /// </summary>
        /// <returns></returns>
        public virtual string GetNo()
        {
            #region 注释
            //业务逻辑代码，此处生成自动编号
            var config = ConfigService.GetConfig<NoConfigValue>(new NoConfig(), typeof(SparePartApp));
            if (config == null || config.BacodeRule == null)
                throw new ValidationException("未找到备件申请编号生成规则,请检查规则配置".L10N());
            var code = RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.BacodeRule.Id, 1).FirstOrDefault();
            #endregion
            return code;
        }
        #endregion

        #region 校验
        /// <summary>
        /// 验证设备是否允许报修
        /// </summary>
        /// <param name="equipId">设备台账ID</param>
        public virtual string ValidateEquipAccountApplyRepair(double equipId)
        {
            //需求变更：无需再校验
            //获取报修、维修中、暂停、待确认、待维修状态的维修单
            //改成提醒是否存在未完成的报修单 (10.8)
            var errMsg = "";
            var states = new List<EquipRepairState>()
                {
                    EquipRepairState.ApplyRepair,
                    EquipRepairState.Repairing,
                    EquipRepairState.Suspending,
                    EquipRepairState.WaitConfirm,
                    EquipRepairState.WaitRepair
                };
            var repairs = this.GetEquipRepairBills(equipId, states);

            if (repairs.Count > 0)
                errMsg = "设备存在未完成的维修单，报修人为【{0}】".L10nFormat(repairs[0].ApplyRepairEmployee.Name);
            return errMsg;
        }

        /// <summary>
        /// 验证备件是否允许报修
        /// </summary>
        /// <param name="spareId">备件ID</param>
        public virtual string ValidateSparePartApplyRepair(double spareId)
        {
            //需求变更：无需再校验
            //获取报修、维修中、暂停、待确认、待维修状态的维修单
            //改成提醒是否存在未完成的报修单 (10.8)
            var errMsg = "";
            var states = new List<EquipRepairState>()
                {
                    EquipRepairState.ApplyRepair,
                    EquipRepairState.Repairing,
                    EquipRepairState.Suspending,
                    EquipRepairState.WaitConfirm,
                    EquipRepairState.WaitRepair
                };
            var repairs = this.GetSpareRepairs(spareId, states);

            if (repairs.Count > 0)
                errMsg = "设备存在未完成的维修单，报修人为【{0}】".L10nFormat(repairs[0].ApplyRepairEmployee.Name);
            return errMsg;
        }

        /// <summary>
        /// 验证设备维修接单/派工提交数据(公共)
        /// </summary>
        /// <param name="takeRepairInfo">维修接单参数实体</param>
        /// <param name="repairerIds">维修人ID列表</param>
        /// <param name="isTransfer">是否转派</param>
        public virtual void ValidateTakeDispatchRepair(TakeRepairInfo takeRepairInfo, List<double> repairerIds, bool isTransfer = false)
        {
            if (takeRepairInfo == null)
            {
                return;
            }
            var repair = this.GetEquipRepairBill(takeRepairInfo.RepairBillId);
            List<EquipRepairState> state = new List<EquipRepairState>();
            if (isTransfer)
            {
                state.Add(EquipRepairState.WaitRepair);
                state.Add(EquipRepairState.Repairing);
                state.Add(EquipRepairState.Suspending);
            }
            else
            {
                state.Add(EquipRepairState.ApplyRepair);
            }
            if (repair == null)
            {
                throw new ValidationException("设备维修单不存在[ID:{0}]".L10nFormat(takeRepairInfo.RepairBillId));
            }
            if (takeRepairInfo.RepairMasterId == 0)
            {
                throw new ValidationException("维修责任人不能为空".L10N());
            }
            if (repairerIds.Contains(takeRepairInfo.RepairMasterId))
            {
                throw new ValidationException("其他维修人员无需与主责任人重复维护".L10N());
            }
            if (takeRepairInfo.EstimateFinishDate == null)
            {
                throw new ValidationException("预计完成时间不可为空".L10N());
            }
            if (!state.Contains(repair.RepairState))
            {
                throw new ValidationException("设备维修单[{0}]当前是[{1}]状态,不允许提交".L10nFormat(repair.RepairNo, repair.RepairState.ToLabel()));
            }
        }

        /// <summary>
        /// 验证设备维修派单提交数据
        /// </summary>
        /// <param name="dispatchRepairInfo"></param>
        /// <param name="repairerIds"></param>
        /// <param name="isTransfer"></param>
        public virtual void ValidateDispatchRepair(DispatchRepairInfo dispatchRepairInfo, List<double> repairerIds, bool isTransfer = false)
        {
            if (dispatchRepairInfo == null)
            {
                return;
            }
            //调用接单/派工公共验证逻辑
            this.ValidateTakeDispatchRepair(dispatchRepairInfo, repairerIds, isTransfer);

            //外修校验逻辑
            if (dispatchRepairInfo.RepairWay == 1)
            {
                if (dispatchRepairInfo.SendRepairWay == null)
                {
                    throw new ValidationException("送修方式不能为空".L10N());
                }
                if (dispatchRepairInfo.ContactPerson.IsNullOrEmpty())
                {
                    throw new ValidationException("联系人不能为空".L10N());
                }
                if (dispatchRepairInfo.ContactPhone.IsNullOrEmpty())
                {
                    throw new ValidationException("联系电话不能为空".L10N());
                }
                if (dispatchRepairInfo.SendRepairDate == null)
                {
                    throw new ValidationException("外修时间不能为空".L10N());
                }
                if (dispatchRepairInfo.PredictBackDate == null)
                {
                    throw new ValidationException("预计返厂时间不能为空".L10N());
                }
                if (dispatchRepairInfo.SupplierId == null)
                {
                    throw new ValidationException("供应商不能为空".L10N());
                }
            }
        }

        /// <summary>
        /// 验证设备维修转派提交数据
        /// </summary>
        /// <param name="transfeRepairInfo"></param>
        /// <param name="repairerIds"></param>
        public virtual void ValidateTransferRepair(TransfeRepairInfo transfeRepairInfo, List<double> repairerIds)
        {
            if (transfeRepairInfo == null)
            {
                return;
            }
            //调用派工验证逻辑
            this.ValidateDispatchRepair(transfeRepairInfo, repairerIds, true);
            List<double> originalRepairerIds = transfeRepairInfo.OriginalRepairers.Select(p => p.EmployeeId).ToList();

            //判断责任人或维修人是否变化
            if (repairerIds.Count == originalRepairerIds.Count)
            {
                if (repairerIds.Count == 0 && transfeRepairInfo.OriginalRepairMasterId == transfeRepairInfo.RepairMasterId)
                {
                    throw new ValidationException("维修责任人与维修人员未改变，无需转派".L10N());
                }
                else if (transfeRepairInfo.OriginalRepairMasterId == transfeRepairInfo.RepairMasterId && originalRepairerIds.All(a => repairerIds.Any(b => a == b)))
                {
                    throw new ValidationException("维修责任人与维修人员未改变，无需转派".L10N());
                }
            }
        }

        /// <summary>
        /// 校验维修单状态，并返回设备ID
        /// </summary>
        /// <param name="repairBillId">维修单ID</param>
        /// <param name="state">维修单状态</param>
        /// <returns>设备ID</returns>
        public virtual double ValidateRepairState(double repairBillId, EquipRepairState state)
        {
            var accountId = Query<EquipRepairBill>()
                .Where(p => p.Id == repairBillId && p.RepairState == state)
                .Select(p => p.EquipAccountId)
                .FirstOrDefault()?.EquipAccountId;
            if (accountId == null)
            {
                throw new ValidationException("维修单不是[{0}]状态".L10nFormat(state.ToLabel()));
            }
            return accountId.Value;
        }

        /// <summary>
        /// 校验经验库参数
        /// </summary>
        /// <param name="expDepotInfo"></param>
        public virtual void ValidateExperienceDepot(ExperienceDepotInfo expDepotInfo)
        {
            var expCount = Query<ExperienceDepot>().Where(p => p.RepairNo.Contains(expDepotInfo.RepairBillNo)).Count();
            if (expCount > 0)
            {
                throw new ValidationException("已经添加经验了".L10N());
            }
            if ((expDepotInfo.FaultDescriptionId <= 0 || expDepotInfo.FaultDescriptionId == null) && expDepotInfo.FaultDescriptionRemark.IsNullOrEmpty())
            {
                throw new ValidationException("故障描述或故障描述(备注)不能为空".L10N());
            }
            if (expDepotInfo.FaultReasonCode.IsNullOrEmpty())
            {
                throw new ValidationException("故障原因不能为空".L10N());
            }
            if (expDepotInfo.FaultCategoryId <= 0)
            {
                throw new ValidationException("故障类别不能为空".L10N());
            }
/*            if (expDepotInfo.FaultPart.IsNullOrEmpty())
            {
                throw new ValidationException("故障部位不能为空".L10N());
            }*/
            if (expDepotInfo.RepairMethod.IsNullOrEmpty())
            {
                throw new ValidationException("维修方法不能为空".L10N());
            }
          /*  if (expDepotInfo.PreventionAdvice.IsNullOrEmpty())
            {
                throw new ValidationException("预防建议不能为空".L10N());
            }*/
        }

        /// <summary>
        /// 校验维修报告
        /// </summary>
        /// <param name="info"></param>
        public virtual string ValidateRepairReport(RepairSaveSubmitInfo info)
        {
            bool isEngineerConfirm = IsConfigEngineerConfirm();
            List<string> errorProperty = new List<string>();
            if (info.FaultDescriptionId <= 0 || info.FaultDescriptionId == null)
            {
                errorProperty.Add("故障描述".L10N());
            }
            if (info.FaultReasonCode.IsNullOrEmpty())
            {
                errorProperty.Add("故障原因".L10N());
            }
            if (info.FaultCategoryId <= 0)
            {
                errorProperty.Add("故障类别".L10N());
            }
            if (info.FaultLevel == null)
            {
                errorProperty.Add("故障等级".L10N());
            }
            if (info.RepairLevel == null)
            {
                errorProperty.Add("维修等级".L10N());
            }
            if (info.RepairCategory == null)
            {
                errorProperty.Add("维修类别".L10N());
            }
            if (info.RepairMethod.IsNullOrEmpty())
            {
                errorProperty.Add("维修方法".L10N());
            }

            if (errorProperty.Count > 0)
            {
                if (isEngineerConfirm)
                {
                    return "维修报告的{0}未填写".L10nFormat(string.Join("、", errorProperty.ToArray()));
                }
                else
                {
                    throw new ValidationException("维修报告的{0}未填写".L10nFormat(string.Join("、", errorProperty.ToArray())));
                }
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion

        #region 查询

        /// <summary>
        /// 判断当前设备是否有未完成的维修单
        /// </summary>
        /// <param name="equipAccountId">设备台账id</param>
        /// <returns></returns>
        public virtual string CheckPlanWithUnFinishRepairBill(double equipAccountId)
        {
            var bill = Query<EquipRepairBill>().As("erb").Where(p => p.EquipAccountId == equipAccountId 
            && p.RepairState != EquipRepairState.Completed && p.RepairState != EquipRepairState.Cancel && p.RepairState != EquipRepairState.Closed)
                .OrderByDescending(p => p.CreateDate)
            .Join<Employee>("e",(x, y) => x.ApplyRepairEmployeeId == y.Id)
            .Select<Employee>((erb, e) => new
            {
                e.Name
            }).ToList<string>().ToList();

            return bill.Count > 0 ? bill.FirstOrDefault() : string.Empty;
        }

        /// <summary>
        /// 是否配置交机确认
        /// </summary>
        /// <returns>bool</returns>
        public virtual bool IsConfigHandoverConfirm()
        {
            var config = ConfigService.GetConfig(new IsHandoverConfirmConfig(), typeof(EquipRepairBill));
            return config.IsHandoverConfirm;
        }

        /// <summary>
        /// 是否配置工程确认
        /// </summary>
        /// <returns>bool</returns>
        public virtual bool IsConfigEngineerConfirm()
        {
            var config = ConfigService.GetConfig(new IsEngineerConfirmConfig(), typeof(EquipRepairBill));
            return config.IsEngineerConfirm;
        }

        /// <summary>
        /// 获取当前数据库时间
        /// </summary>
        /// <returns></returns>
        public virtual DateTime GetDbTime()
        {
            return RF.Find<EquipRepairBill>().GetDbTime();
        }

        /// <summary>
        /// 获取交机确认为OK的操作时间
        /// </summary>
        /// <returns></returns>
        public virtual DateTime? GetHandoverOkTime(double repairBillId)
        {
            var handoverConfirmResult = Query<EquipRepairOperationRec>()
                .Where(p => p.EquipRepairBillId == repairBillId && p.HandoverConfirmResult == HandoverConfirmResult.OK)
                .FirstOrDefault();

            if (handoverConfirmResult != null)
            {
                return handoverConfirmResult.OperationDate;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取未完成设备维修申请单列表
        /// </summary>
        /// <param name="equipmentId">设备台账ID</param>
        /// <param name="orderInfoList">排序信息</param>
        /// <param name="pagingInfo">分页实体</param>
        /// <param name="state">状态</param>
        /// <returns>设备维修申请单列表</returns>
        public virtual EntityList<EquipRepairBill> GetNotCompletedEquipRepairBills(double equipmentId, List<OrderInfo> orderInfoList, PagingInfo pagingInfo, EquipRepairState? state)
        {
            var query = Query<EquipRepairBill>();
            query.Where(p => p.RepairState != EquipRepairState.Completed);
            if (equipmentId > 0)
                query.Where(p => p.EquipAccountId == equipmentId);
            if (state.HasValue)
                query.Where(p => p.RepairState == state);

            return query.OrderBy(orderInfoList).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 通过备件ID获取维修单
        /// </summary>
        /// <param name="spareId"></param>
        /// <param name="states"></param>
        /// <returns></returns>
        public virtual EntityList<EquipRepairBill> GetSpareRepairs(double spareId, List<EquipRepairState> states)
        {
            var q = Query<EquipRepairBill>();
            q.Where(p => p.SparePartId == spareId);

            if (states.Count > 0)
                q.Where(p => states.Contains(p.RepairState));

            return q.ToList();
        }

        /// <summary>
        /// 获取维修单
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="states"></param>
        /// <param name="type"></param>
        /// <param name="keyword"></param>
        /// <param name="operationType"></param>
        /// <returns></returns>
        public virtual EntityList GetEquipRepairBills(PagingInfo pagingInfo
            , List<EquipRepairState> states, EquipRepairType? type, string keyword, RepairOperationType operationType)
        {
            var q = Query<EquipRepairBill>();

            if (states != null && states.Count > 0)
            {
                q.Where(p => states.Contains(p.RepairState));
            }

            if (type.HasValue)
            {
                q.Where(p => p.RepairType == type);
            }

            if (keyword.IsNotEmpty())
            {
                q.Where(p => p.EquipAccount.Code.Contains(keyword)
                    || p.EquipAccount.Name.Contains(keyword)
                    || p.RepairNo.Contains(keyword)
                    ||p.SparePart.SparePartCode.Contains(keyword)
                    );
            }

            //权限条件
            if (operationType == RepairOperationType.Take)
            {
                q.Exists<DevicePur>((x, y) => y.LeftJoin<UserInUserGroup>((a, b) => a.UserGroupId == b.UserGroupId)
                                             .Where<UserInUserGroup>((a, b) => a.UserId == RT.Identity.UserId || b.UserId == RT.Identity.UserId)
                                             .WhereIf(operationType == RepairOperationType.Take, a => a.EquipMaintain));
            }
            
            if (operationType == RepairOperationType.Begin)
            {
                q.Exists<EquipRepairWorkingHours>((x, y) => y.Where(p => p.EquipRepairBillId == x.Id
                    && p.IsRepairEmployee
                    && p.RepairerId == RT.IdentityId));
            }

            q.OrderBy(p => p.ProduceState).OrderBy(p => p.UrgentDegree);

            var iq = q.ToQuery();

            if (type == EquipRepairType.EquipRepair)
            {
                iq.QueryWithEquipAccountPermissions(EquipRepairBill.EquipAccountIdProperty.Name);
            }

            var billList = q.Repository.QueryList(iq, pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            return billList;
        }

        /// <summary>
        /// 获取维修单
        /// </summary>
        /// <param name="states"></param>
        /// <param name="type"></param>
        /// <param name="operationType"></param>
        /// <returns></returns>
        public virtual int GetEquipRepairBills(List<EquipRepairState> states, EquipRepairType? type, RepairOperationType operationType)
        {
            var q = Query<EquipRepairBill>();

            if (states != null && states.Count > 0)
            {
                q.Where(p => states.Contains(p.RepairState));
            }

            if (type.HasValue)
            {
                q.Where(p => p.RepairType == type);
            }

            //权限条件
            if (operationType == RepairOperationType.Take)
            {
                q.Exists<DevicePur>((x, y) => y.LeftJoin<UserInUserGroup>((a, b) => a.UserGroupId == b.UserGroupId)
                                             .Where<UserInUserGroup>((a, b) => a.UserId == RT.Identity.UserId || b.UserId == RT.Identity.UserId)
                                             .WhereIf(operationType == RepairOperationType.Take, a => a.EquipMaintain));
            }

            if (operationType == RepairOperationType.Begin)
            {
                q.Exists<EquipRepairWorkingHours>((x, y) => y.Where(p => p.EquipRepairBillId == x.Id
                    && p.IsRepairEmployee
                    && p.RepairerId == RT.IdentityId));
            }

            q.OrderBy(p => p.ProduceState).OrderBy(p => p.UrgentDegree);

            var iq = q.ToQuery();

            if (type == EquipRepairType.EquipRepair)
            {
                iq.QueryWithEquipAccountPermissions(EquipRepairBill.EquipAccountIdProperty.Name);
            }

            var count = q.Repository.Count(iq);

            return count;
        }

        /// <summary>
        /// 通过设备ID获取维修单
        /// </summary>
        /// <param name="equipId"></param>
        /// <param name="states"></param>
        /// <returns></returns>
        public virtual EntityList<EquipRepairBill> GetEquipRepairBills(double equipId, List<EquipRepairState> states)
        {
            var q = Query<EquipRepairBill>();
            q.Where(p => p.EquipAccountId == equipId);

            if (states.Count > 0)
                q.Where(p => states.Contains(p.RepairState));

            return q.ToList();
        }

        /// <summary>
        /// 通过设备ID获取维修单
        /// </summary>
        /// <param name="equipId"></param>
        /// <param name="repairId"></param>
        /// <param name="states"></param>
        /// <returns></returns>
        public virtual bool IsExistRepairBillsByRepairId(double equipId,double repairId, List<EquipRepairState> states)
        {
            var isExist = Query<EquipRepairBill>().Where(p => p.EquipAccountId == equipId && p.Id != repairId).Count() > 0; 
            return isExist;
        }

        /// <summary>
        /// 获取日期内的设备维修申请单列表
        /// </summary>
        /// <param name="factoryId">工厂</param>
        /// <param name="departmentId">部门</param>
        /// <param name="repairType">维修类型</param>
        /// <param name="begindate">开始时间</param>
        /// <param name="enddate">结束时间</param>
        /// <returns>设备维修申请单列表</returns>
        public virtual EntityList<EquipRepairBill> GetEquipRepairBills(double? factoryId, double? departmentId, EquipRepairType? repairType, DateTime begindate, DateTime enddate)
        {
            var query = Query<EquipRepairBill>();
            if (factoryId.HasValue)
            {
                query.Where(p => p.EquipAccount.FactoryId == factoryId);
            }
            if (departmentId.HasValue)
            {
                query.Where(p => p.EquipAccount.UseDepartmentId == departmentId);
            }
            if (repairType.HasValue)
            {
                query.Where(p => p.RepairType == repairType);
            }
            query.Where(p => p.CreateDate >= begindate && p.CreateDate < enddate);
            //报修不算总单,待维修，维修中，待确认，待平分，已完成，暂停中，取消，关闭  这些算总单；已完成状态算完成；待维修，维修中，待确认，待平分，暂停中算未完成
            //去掉报修，关闭和取消单
            List<EquipRepairState> repairStates = new List<EquipRepairState>() {
                EquipRepairState.ApplyRepair, EquipRepairState.Cancel, EquipRepairState.Closed
            };
            query.Where(p => !repairStates.Contains(p.RepairState));
            return query.OrderByDescending(p => p.CreateDate).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 查询当前用户参与的维修单数量
        /// </summary>
        /// <param name="state">维修单状态</param>
        /// <param name="repairerIds">维修人ID列表</param>
        /// <returns>单据数量</returns>
        public virtual IList<EquipRepairBill> GetRepairOfEmployeeCount(EquipRepairState? state, List<double> repairerIds)
        {
            var stateBillList = Query<EquipRepairBill>().Where(p => p.RepairState == state).ToList();
            var repairBillList = new EntityList<EquipRepairBill>();
            repairerIds.ForEach(repairId =>
            {
                repairBillList.AddRange(stateBillList.Where(p => p.RepairMasterId == repairId || p.RepairEmployeeIds.Split(',').Contains(repairId.ToString())));
            });
            return repairBillList.Distinct().ToList();
        }

        /// <summary>
        /// 获取设备维修单
        /// </summary>
        /// <param name="repairBillId"></param>
        /// <returns></returns>
        public virtual EquipRepairBill GetEquipRepairBill(double repairBillId)
        {
            return RF.GetById<EquipRepairBill>(repairBillId, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 通过员工编码获取员工信息列表
        /// </summary>
        /// <param name="employCodes">员工编码</param>
        /// <returns>员工信息列表</returns>
        public virtual EntityList<Employee> GetEmployeeListByCodes(string employCodes)
        {
            return employCodes.Split(',').SplitContains(tempCodes =>
            {
                return Query<Employee>().Where(p => tempCodes.Contains(p.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 通过员工ID获取员工信息列表
        /// </summary>
        /// <param name="employIds">员工ID列</param>
        /// <returns>员工信息列表</returns>
        public virtual EntityList<Employee> GetEmployeeListByIdsString(string employIds)
        {
            return employIds.Split(',').Select(x => double.Parse(x)).SplitContains(tempIds =>
            {
                return Query<Employee>().Where(p => tempIds.Contains(p.Id))
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 通过员工Id获取员工信息列表
        /// </summary>
        /// <param name="employIds">员工Id</param>
        /// <returns>员工信息列表</returns>
        public virtual EntityList<Employee> GetEmployeeListByIds(string employIds)
        {
            List<double> IdList = new List<double>();
            List<string> employList = employIds.Split(',').ToList();

            employList.ForEach(Id =>
            {
                if (Id.IsNotEmpty())
                    IdList.Add(double.Parse(Id));
            });

            return IdList.SplitContains(tempCodes =>
            {
                return Query<Employee>().Where(p => tempCodes.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 查询设备维修单
        /// </summary>
        /// <param name="criteria">设备维修单查询实体</param>
        /// <returns>设备维修单列表</returns>
        public virtual EntityList CriteriaEquipRepairBills(EquipRepairCriteria criteria)
        {
            var q = Query<EquipRepairBill>();

            if (criteria.EquipAccountId != null && criteria.EquipAccountId != 0)
                q.Where(p => p.EquipAccountId == criteria.EquipAccountId);
            if (criteria.SparePartId != null && criteria.SparePartId != 0)
                q.Where(p => p.SparePartId == criteria.SparePartId);
            if (!criteria.EquipOrSparePartName.IsNullOrEmpty())
                q.Where(p => p.EquipAccount.Name.Contains(criteria.EquipOrSparePartName) || p.SparePart.SparePartCode.Contains(criteria.EquipOrSparePartName));

            if (criteria.EquipTypeId != null && criteria.EquipTypeId != 0)
                q.Where(p => p.EquipAccount.EquipModel.EquipTypeId == criteria.EquipTypeId);
            if (criteria.EquipModelId != null && criteria.EquipModelId != 0)
                q.Where(p => p.EquipAccount.EquipModelId == criteria.EquipModelId);
            if (!criteria.RepairNo.IsNullOrEmpty())
                q.Where(p => p.RepairNo.Contains(criteria.RepairNo));
            if (criteria.ApplyRepairEmployeeId != null && criteria.ApplyRepairEmployeeId != 0)
                q.Where(p => p.ApplyRepairEmployeeId == criteria.ApplyRepairEmployeeId);
            if (criteria.RepairMasterId != null && criteria.RepairMasterId != 0)
                q.Where(p => p.RepairMasterId == criteria.RepairMasterId);
            if (criteria.RepairType != null)
                q.Where(p => p.RepairType == criteria.RepairType);
            if (criteria.RepairState != null)
                q.Where(p => p.RepairState == criteria.RepairState);
            if (criteria.RepairWay != null)
                q.Where(p => p.RepairWay == criteria.RepairWay);
            if (criteria.WorkshopId != null && criteria.WorkshopId != 0)
                q.Where(p => p.EquipAccount.WorkShopId == criteria.WorkshopId);
            if (criteria.ProcessId != null && criteria.ProcessId != 0)
                q.Where(p => p.EquipAccount.ProcessId == criteria.ProcessId);

            if (criteria.ApplyRepairDate.BeginValue.HasValue)
                q.Where(p => p.ApplyRepairDate >= criteria.ApplyRepairDate.BeginValue);
            if (criteria.ApplyRepairDate.EndValue.HasValue)
                q.Where(p => p.ApplyRepairDate <= criteria.ApplyRepairDate.EndValue);

            if (criteria.IsToFinish)
            {
                q.Where(p => p.RepairState == EquipRepairState.ApplyRepair || p.RepairState == EquipRepairState.WaitRepair ||
                 p.RepairState == EquipRepairState.Repairing || p.RepairState == EquipRepairState.Suspending || p.RepairState == EquipRepairState.WaitScore || p.RepairState == EquipRepairState.WaitConfirm);
            }
            if (criteria.RepairEmployeeId != null && criteria.RepairEmployeeId != 0)
            {
                var repairEId = criteria.RepairEmployeeId.ToString();
                q.Where(p => p.RepairEmployeeIds.Contains(repairEId));
            }
            var elo = new EagerLoadOptions().LoadWithViewProperty().LoadWith(EquipRepairBill.EquipAccountProperty);

            var iquery = q.ToQuery();
            var list = q.Repository.QueryList(iquery, paging: criteria.PagingInfo, eagerLoad: elo);

            foreach (var item in list)
            {
                var repair = item as EquipRepairBill;
                repair.RepairTime = repair.EquipRepairOperationRecList.Sum(p => p.IntervalTime);
            }
            return list;
        }

        /// <summary>
        /// 获取当前设备类型的故障现象
        /// </summary>
        /// <param name="id">台账ID</param>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<DeviceAbnormal> GetDeviceAbnormal(double? id, string keyword, PagingInfo pagingInfo)
        {
            if (id == null || id == 0)
                throw new ValidationException("请先选择设备或者备件".L10N());
            EquipAccount entity = Query<EquipAccount>().Where(p => p.Id == id).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            var query = Query<DeviceAbnormal>().Join<EquipType>((a, b) => a.EquipTypeId == b.Id && b.TypeCode == entity.EquipTypeCode)
                .Where(p => p.AbnormalType == AbnormalType.Unusual);

            if (!keyword.IsNullOrEmpty())
                query = query.Where(p => p.Code.Contains(keyword));
            var list = query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 查询设备类型维修经验库
        /// </summary>
        /// <param name="pagingInfo"></param>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        public virtual EntityList<ExperienceDepot> GetAccountExperienceDepots(PagingInfo pagingInfo, ExpDepotQueryInfo queryInfo)
        {
            var q = Query<ExperienceDepot>();
            q.Where(p => p.RepairType == ExperienceDepots.Enums.ExpRepairType.Account);
            if (queryInfo.EquipAccountId.HasValue)
                q.Where(p => p.EquipAccountId == queryInfo.EquipAccountId);
            if (queryInfo.EquipModelId.HasValue)
                q.Where(p => p.EquipModelId == queryInfo.EquipModelId);
            if (queryInfo.EquipTypeId.HasValue)
                q.Where(p => p.EquipTypeId == queryInfo.EquipTypeId);
            if (queryInfo.DeviceAbnormalId.HasValue)
                q.Where(p => p.FaultPhenomenonId == queryInfo.DeviceAbnormalId);
            if (queryInfo.DeviceAbnormalRemark.IsNotEmpty())
                q.Where(p => p.FaultPhenomenonRemark.Contains(queryInfo.DeviceAbnormalRemark));
            if (queryInfo.FaultDescriptionId.HasValue)
                q.Where(p => p.FaultDescribeId == queryInfo.FaultDescriptionId);
            if (queryInfo.FaultDescriptionRemark.IsNotEmpty())
                q.Where(p => p.FaultDescribeRemark.Contains(queryInfo.FaultDescriptionRemark));

            var elo = new EagerLoadOptions();
            elo.LoadWith(ExperienceDepot.EquipAccountProperty);
            elo.LoadWith(ExperienceDepot.FaultDescribeProperty);
            elo.LoadWith(ExperienceDepot.FaultPhenomenonProperty);
            elo.LoadWith(ExperienceDepot.EquipLargeFaultProperty);

            return q.ToList(pagingInfo);
        }

        /// <summary>
        /// 查询指定维修原因快码
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
        public virtual EntityList<Catalog> GetFaultResons(List<string> codes)
        {
            return Query<Catalog>()
                .Where(p => codes.Contains(p.Code) && p.CatalogType.Code == EquipRepairBill.CatalogExpFaultReson)
                .ToList();
        }

        /// <summary>
        /// 根据维修单ID和申请标记获取备件申请项目列表
        /// </summary>
        /// <param name="repairBillId">维修单ID</param>
        /// <param name="isApply">是否申请</param>
        /// <returns></returns>
        public virtual EntityList<EquipRepairSparePartApl> GetRepairSparePartApls(double repairBillId, bool? isApply = null)
        {
            var query = Query<EquipRepairSparePartApl>();
            query.Where(p => p.EquipRepairBillId == repairBillId);
            if (isApply.HasValue) query.Where(p => p.IsApply == isApply);

            var elo = new EagerLoadOptions();
            elo.LoadWith(EquipRepairSparePartApl.SparePartProperty);
            var list = query.ToList(null, elo);

            if (list.Count > 0)
            {
                var partIds = list.Select(p => p.SparePartId).ToList();
                var storeSummaryDepotList = RT.Service.Resolve<SparePartController>().GetStoreSummaryDepots(partIds);
                list.ForEach(p =>
                {
                    //获取库存明细
                    p.StoreQty = storeSummaryDepotList.FirstOrDefault(x => x.WarehouseId == p.OutStockWarehouseId && x.SparePartId == p.SparePartId)?.StoreQty ?? 0;
                });

            }

            return list;
        }

        /// <summary>
        /// 根据维修单ID和状态获取备件更换项目列表
        /// </summary>
        /// <param name="repairBillId">维修单ID</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public virtual EntityList<EquipRepairSparePartChg> GetRepairSparePartChgs(double repairBillId, ChangeSparePartState? state = null)
        {
            var query = Query<EquipRepairSparePartChg>();
            query.Where(p => p.EquipRepairBillId == repairBillId);
            if (state.HasValue) query.Where(p => p.State == state);

            var elo = new EagerLoadOptions();
            elo.LoadWith(EquipRepairSparePartChg.SparePartProperty);
            elo.LoadWith(EquipRepairSparePartChg.EquipRepairBillProperty);

            return query.ToList(null, elo);
        }


        /// <summary>
        /// 获取维修工时列表
        /// </summary>
        /// <param name="repairBillId"></param>
        /// <returns></returns>
        public virtual EntityList<EquipRepairBillProject> GetEquipRepairBillProjects(double repairBillId)
        {
            var query = Query<EquipRepairBillProject>();
            query.Where(p => p.EquipRepairBillId == repairBillId);

            var elo = new EagerLoadOptions();
            elo.LoadWith(EquipRepairBillProject.ProjectDetailProperty);

            return query.ToList(null, elo);
        }

        /// <summary>
        /// 获取维修工时列表
        /// </summary>
        /// <param name="repairBillId"></param>
        /// <returns></returns>
        public virtual EntityList<EquipRepairWorkingHours> GetEquipRepairWorkingHoursNotEnd(double repairBillId)
        {
            var query = Query<EquipRepairWorkingHours>();
            query.Where(p => p.EquipRepairBillId == repairBillId)
                .Where(x => x.EndTime == null);

            var elo = new EagerLoadOptions();
            elo.LoadWith(EquipRepairWorkingHours.RepairerProperty);

            return query.ToList(null, elo);
        }

        /// <summary>
        /// 获取维修工时列表
        /// </summary>
        /// <param name="repairBillId"></param>
        /// <returns></returns>
        public virtual EntityList<EquipRepairWorkingHours> GetEquipRepairWorkingHours(double repairBillId)
        {
            var query = Query<EquipRepairWorkingHours>();
            query.Where(p => p.EquipRepairBillId == repairBillId);

            var elo = new EagerLoadOptions();
            elo.LoadWith(EquipRepairWorkingHours.RepairerProperty);

            return query.ToList(null, elo);
        }

        /// <summary>
        /// 获取维修操作记录列表
        /// </summary>
        /// <param name="repairBillId"></param>
        /// <returns></returns>
        public virtual EntityList<EquipRepairOperationRec> GetEquipRepairOperationRecs(double repairBillId)
        {
            var query = Query<EquipRepairOperationRec>();
            query.Where(p => p.EquipRepairBillId == repairBillId);

            var elo = new EagerLoadOptions();
            elo.LoadWith(EquipRepairOperationRec.OperationerProperty);
            elo.LoadWith(EquipRepairOperationRec.OriginalRepairMasterProperty);

            return query.ToList(null, elo);
        }

        /// <summary>
        /// 获取维修操作记录
        /// </summary>
        /// <param name="equipRepairBillIds">维修单ID</param>
        /// <returns></returns>
        public virtual EntityList<EquipRepairOperationRec> GetEquipRepairOperationRecs(List<double> equipRepairBillIds)
        {
            var exp = equipRepairBillIds.CreateContainsExpression<EquipRepairOperationRec>("x",
                EquipRepairOperationRec.EquipRepairBillIdProperty.Name);

            if (exp == null)
            {
                return new EntityList<EquipRepairOperationRec>();
            }

            var query = Query<EquipRepairOperationRec>()
                .Where(exp);

            return query.ToList();
        }

        /// <summary>
        /// 查询可以维修此台账的员工
        /// </summary>
        /// <param name="Criteria"></param>
        /// <returns></returns>
        public virtual EntityList<Employee> GetByEmployeeByRepairAccountCriteria(EmployeeByRepairAccountCriteria criteria)
        {
            var employees = RT.Service.Resolve<DevicePurController>().GetDevicePurRepairs(criteria.EquipAccountId, null, criteria.PagingInfo);
            return employees;
        }

        /// <summary>
        /// 获取人员管理的设备数量
        /// </summary>
        /// <param name="usrId">用户id</param>
        /// <returns>用户管理台账的信息</returns>
        public virtual UserManageEquipAccountInfo GetEquipAccountCount(double usrId)
        {
            //当前用户可管理的设备台账
            var equipAccountIds = Query<EquipAccount>().Select(x => x.Id).ToList<double>();

            var accountCount = equipAccountIds.Count();

            //转化成List<double?>
            List<double?> accountIds = new List<double?>();

            foreach (var item in equipAccountIds)
            {
                accountIds.Add(item);
            }

            accountIds = accountIds.Distinct().ToList();

            //查询维修单中出问题的设备台账
            var equipRepairBills = accountIds.SplitContains(tempIds =>
            {
                return Query<EquipRepairBill>()
                .Where(p => tempIds.Contains(p.EquipAccountId))
                .Where(p => p.RepairState == EquipRepairState.ApplyRepair
                    || p.RepairState == EquipRepairState.Repairing
                    || p.RepairState == EquipRepairState.WaitRepair
                    || p.RepairState == EquipRepairState.Suspending)
                .ToList();
            });

            //得到故障数量
            var accountFaultCount = equipRepairBills.Select(p => p.EquipAccountId).Distinct().Count();

            //返回结果

            UserManageEquipAccountInfo userManageEquipAccountInfo = new UserManageEquipAccountInfo()
            {
                EquipAccountCount = accountCount,
                FaultEquipAccountCount = accountFaultCount,
            };

            return userManageEquipAccountInfo;
        }
        /// <summary>
        /// 获取维修附件列表
        /// </summary>
        /// <param name="repairBillId"></param>
        /// <param name="operationType"></param>
        /// <returns></returns>
        public virtual EntityList<EquipRepairAttachment> GetEquipRepairAttachments(double repairBillId, RepairOperationType? operationType = null)
        {
            var query = Query<EquipRepairAttachment>();
            query.Where(p => p.OwnerId == repairBillId);
            if (operationType.HasValue) query.Where(p => p.RepairOperationType == operationType);

            return query.ToList();
        }

        /// <summary>
        /// 获取维修单总工时
        /// </summary>
        /// <param name="repairBillId"></param>
        /// <returns></returns>
        public virtual decimal GetTotalWorkingHour(double repairBillId)
        {
            var q = Query<EquipRepairWorkingHours>();
            q.Where(p => p.EquipRepairBillId == repairBillId && p.BeginTime != null && p.EndTime != null);
            q.Select(p => new { p.BeginTime, p.EndTime });
            return (decimal)Math.Round(q.ToList().Sum(p => (p.EndTime - p.BeginTime).Value.TotalHours), 2);
        }

        /// <summary>
        /// 获取维修备件申请列表
        /// </summary>
        /// <param name="equipRepairId">维修单ID</param>
        /// <param name="orderInfoList">排序信息</param>
        /// <param name="pagingInfo">关键字</param>
        /// <returns></returns>
        public virtual EntityList<EquipRepairSparePartApl> GetEquipRepairSparePartApls(double equipRepairId, List<OrderInfo> orderInfoList, PagingInfo pagingInfo)
        {
            var q = Query<EquipRepairSparePartApl>();
            q.Where(p => p.EquipRepairBillId == equipRepairId);

            var elo = new EagerLoadOptions();
            elo.LoadWith(EquipRepairSparePartApl.ApplyDetailProperty);
            elo.LoadWith(ApplyDetail.SparePartAppProperty);
            elo.LoadWithViewProperty();

            var index = orderInfoList.FindIndex(m => m.Property == EquipRepairSparePartApl.StoreQtyProperty.Name);
            OrderInfo orderInfo = null;
            if (index >= 0)
            {
                orderInfo = orderInfoList[index];
                orderInfoList.RemoveAt(index);
            }
            var list = q.OrderBy(orderInfoList).ToList(pagingInfo, elo);

            //查询、赋值备件库存
            if (list.Count > 0)
            {
                var partIds = list.Select(p => p.SparePartId).ToList();
                var storeSummarys = RT.Service.Resolve<SparePartController>().GetStoreSummarys(partIds);
                list.ForEach(p =>
                {
                    //赋值库存
                    var storeSummary = storeSummarys.FirstOrDefault(x => x.SparePartId == p.SparePartId);
                    p.StoreQty = storeSummary?.GoodNumber ?? 0;
                });
            }
            list.MarkSaved();
            if (index >= 0 && orderInfo != null)
            {
                var res = orderInfo.SortOrder == System.ComponentModel.ListSortDirection.Ascending ? list.OrderBy(m => m.StoreQty).ToList() : list.OrderByDescending(m => m.StoreQty).ToList();
                var temp = new EntityList<EquipRepairSparePartApl>();
                temp.AddRange(res);
                temp.SetTotalCount(list.TotalCount);
                return temp;
            }
            return list;
        }

        /// <summary>
        /// 根据维修单ID和申请标记获取备件申请项目列表
        /// </summary>
        /// <param name="equipRepairBillId">维修单ID</param>
        /// <param name="isApply">是否申请</param>
        /// <returns></returns>
        public virtual EntityList<EquipRepairSparePartApl> GetEquipRepairSparePartApls(double equipRepairBillId, bool isApply)
        {
            var query = Query<EquipRepairSparePartApl>();
            query.Where(p => p.EquipRepairBillId == equipRepairBillId);
            query.Where(p => p.IsApply == isApply);

            var elo = new EagerLoadOptions();
            elo.LoadWith(EquipRepairSparePartApl.SparePartProperty);

            return query.ToList(null, elo);
        }

        /// <summary>
        /// 获取维修备件更换列表
        /// </summary>
        /// <param name="equipRepairId">维修单ID</param>
        /// <param name="orderInfoList">排序信息</param>
        /// <param name="pagingInfo">关键字</param>
        /// <returns></returns>
        public virtual EntityList<EquipRepairSparePartChg> GetEquipRepairSpareParts(double equipRepairId, List<OrderInfo> orderInfoList, PagingInfo pagingInfo)
        {
            //var equipRepair = GetById<EquipRepairBill>(equipRepairId);

            //设备台账ID
            var q = Query<EquipRepairSparePartChg>();
            q.Where(p => p.EquipRepairBillId == equipRepairId);

            var elo = new EagerLoadOptions();
            elo.LoadWith(EquipRepairSparePartChg.PartOutDepotDetailProperty);
            elo.LoadWith(PartOutDepotDetail.OutDepotProperty);
            elo.LoadWithViewProperty();

            var list = q.OrderBy(orderInfoList).ToList(pagingInfo, elo);

            //查询、赋值申请单明细
            if (list.Any(x => x.State == ChangeSparePartState.New && x.PartOutDepotDetailId != null))
            {
                var listNew = list.Where(x => x.State == ChangeSparePartState.New && x.PartOutDepotDetailId != null).ToList();

                var detailIds = listNew.Select(x => x.PartOutDepotDetailId.Value).Distinct().ToList();

                var outDtls = RT.Service.Resolve<OutDepotController>().GetPartOutDepotDetails(detailIds);
                listNew.ForEach(p =>
                {
                    var outDtl = outDtls.FirstOrDefault(x => x.Id == p.PartOutDepotDetailId);

                    if (outDtl != null)
                    {
                        p.RemainingQty = outDtl.RemainingQty;
                    }

                });
            }

            return list;
        }


        /// <summary>
        /// 根据维修单ID和状态获取备件更换项目列表
        /// </summary>
        /// <param name="equipRepairBillId">点检计划单ID</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public virtual EntityList<EquipRepairSparePartChg> GetEquipRepairSpareParts(double equipRepairBillId, ChangeSparePartState state)
        {
            var query = Query<EquipRepairSparePartChg>();
            query.Where(p => p.EquipRepairBillId == equipRepairBillId);
            query.Where(p => p.State == state);

            var elo = new EagerLoadOptions();
            elo.LoadWith(EquipRepairSparePartChg.SparePartProperty);
            elo.LoadWith(EquipRepairSparePartChg.PartOutDepotDetailProperty);
            elo.LoadWith(EquipRepairSparePartChg.EquipRepairBillProperty);

            return query.ToList(null, elo);
        }
        #endregion

        #region 操作

        /// <summary>
        /// 生成维修单号
        /// </summary>
        /// <returns></returns>
        public virtual string GenerateRepairNo()
        {
            var config = ConfigService.GetConfig(new EquipRepairNoConfig(), typeof(EquipRepairBill));
            if (config == null || config.NumberRule == null)
            {
                throw new ValidationException("未找到设备维修申请单号生成规则,请检查规则配置".L10N());
            }
            return RT.Service.Resolve<NumberRuleController>()
                .GenerateSegment(config.NumberRule.Id, 1)
                .FirstOrDefault();
        }

        /// <summary>
        /// 生成维修单号
        /// </summary>
        /// <returns></returns>
        public virtual double? ValidateRepairNoRule()
        {
            var config = ConfigService.GetConfig(new EquipRepairNoConfig(), typeof(EquipRepairBill));
            if (config == null || config.NumberRule == null)
            {
                return null;
            }
            return config.NumberRuleId;
        }

        /// <summary>
        /// 创建维修单
        /// </summary>
        /// <param name="repair"></param>
        /// <returns></returns>
        public virtual string GenerateRepair(EquipRepairBill repair)
        {
            //创建维修单
            var now = this.GetDbTime();
            repair.ApplyRepairDate = now;
            repair.ApplyRepairEmployeeId = RT.IdentityId;
            repair.RepairState = EquipRepairState.ApplyRepair;
            //生成操作记录
            var optRecord = GenerateOperationReccordReturn(RepairOperationType.ApplyRepair, repair.Id, now);
            
            using (var trans = DB.TransactionScope(EquipRepairEntityDataProvider.ConnectionStringName))
            {
                RF.Save(repair);

                RF.Save(optRecord);
                //更新维修单信息
                if (optRecord.IntervalTime.HasValue)
                {
                    DB.Update<EquipRepairBill>().Where(p => p.Id == repair.Id).Set(p => p.RepairTime, p => p.RepairTime + optRecord.IntervalTime.Value).Execute();
                }

                //改变设备状态
                if (repair.ProduceState == ProduceState.StopWork)
                {
                    DB.Update<EquipAccount>().Where(p => p.Id == repair.EquipAccountId).Set(p => p.State, AccountState.Fault).Execute();
                }

                // 更新设备管理状态为维修
                DB.Update<EquipAccount>().Set(p => p.UseState, AccountUseState.Repair).Where(p => p.Id == repair.EquipAccountId).Execute();

                trans.Complete();
            }
            return repair.RepairNo;
        }

        /// <summary>
        /// 设备维修接单提交
        /// </summary>
        /// <param name="takeRepairInfo"></param>
        public virtual void TakeRepair(TakeRepairInfo takeRepairInfo)
        {
            //校验设备接单
            var repairerIds = takeRepairInfo.Repairers.Select(p => p.EmployeeId).ToList();
            this.ValidateTakeDispatchRepair(takeRepairInfo, repairerIds);

            //维修人员名字列表
            var repairerNames = takeRepairInfo.Repairers.Select(p => p.EmployeeName);
            var repairerNamesStr = string.Join(",", repairerNames);
            var repairerIdsStr = string.Join(",", repairerIds);

            //当前数据库操作时间
            var now = RF.Find<EquipRepairBill>().GetDbTime();
            //生成操作记录
            var optRecord = GenerateOperationReccordReturn(RepairOperationType.Take, takeRepairInfo.RepairBillId, now);
            //生成工时记录
            var workHourList = GenerateWorkingHoursReturn(takeRepairInfo.RepairBillId, takeRepairInfo.RepairMasterId, repairerIds);

            using (var trans = DB.TransactionScope(EquipRepairEntityDataProvider.ConnectionStringName))
            {
                //更新维修单信息
                DB.Update<EquipRepairBill>().Where(p => p.Id == takeRepairInfo.RepairBillId)
                        .Set(p => p.RepairState, EquipRepairState.WaitRepair)
                        .Set(p => p.RepairWay, EquipRepairWay.InnerRepair)
                        .Set(p => p.RepairMasterId, takeRepairInfo.RepairMasterId)
                        .Set(p => p.RepairEmployees, repairerNamesStr)
                        .Set(p => p.RepairEmployeeIds, repairerIdsStr)
                        .Set(p => p.ReceiveOrderDate, now)
                        .Set(p => p.EstimateFinishDate, takeRepairInfo.EstimateFinishDate)
                        .Execute();

                //保存操作记录
                RT.Service.Resolve<CommonController>().BatchInsertSave(new EntityList<EquipRepairOperationRec> { optRecord });
                //更新维修单信息
                if (optRecord.IntervalTime.HasValue)
                {
                    DB.Update<EquipRepairBill>().Where(p => p.Id == takeRepairInfo.RepairBillId).Set(p => p.RepairTime, p => p.RepairTime + optRecord.IntervalTime.Value).Execute();
                }

                // 保存工时记录
                RT.Service.Resolve<CommonController>().BatchInsertSave(workHourList);

                //附件列表
                GenerateBase64Photos(takeRepairInfo.PhotoInfos, takeRepairInfo.RepairBillId, RepairOperationType.Take);

                trans.Complete();
            }
        }

        /// <summary>
        /// 设备维修派工提交
        /// </summary>
        /// <param name="dispatchRepairInfo"></param>
        public virtual void DispatchRepair(DispatchRepairInfo dispatchRepairInfo)
        {
            //校验设备派工
            var repairerIds = dispatchRepairInfo.Repairers.Select(p => p.EmployeeId).ToList();
            this.ValidateDispatchRepair(dispatchRepairInfo, repairerIds);

            //维修人员名字列表
            var repairerNames = dispatchRepairInfo.Repairers.Select(p => p.EmployeeName);
            var repairerNamesStr = string.Join(",", repairerNames);
            var repairerIdsStr = string.Join(",", repairerIds);

            //当前数据库操作时间
            var now = this.GetDbTime();

            using (var trans = DB.TransactionScope(EquipRepairEntityDataProvider.ConnectionStringName))
            {
                //更新维修单信息
                DB.Update<EquipRepairBill>().Where(p => p.Id == dispatchRepairInfo.RepairBillId)
                        .Set(p => p.RepairState, EquipRepairState.WaitRepair)
                        .Set(p => p.RepairWay, (EquipRepairWay)dispatchRepairInfo.RepairWay)
                        .Set(p => p.RepairMasterId, dispatchRepairInfo.RepairMasterId)
                        .Set(p => p.RepairEmployees, repairerNamesStr)
                        .Set(p => p.RepairEmployeeIds, repairerIdsStr)
                        .Set(p => p.ReceiveOrderDate, now)
                        .Set(p => p.EstimateFinishDate, dispatchRepairInfo.EstimateFinishDate)
                        // 派工类型为内修时，送修方式为空值
                        .Set(p => p.SendRepairWay, dispatchRepairInfo.RepairWay == 0 ? null : (SendRepairWay?)dispatchRepairInfo.SendRepairWay)
                        .Set(p => p.SupplierId, dispatchRepairInfo.SupplierId)
                        .Set(p => p.DeliveryNo, dispatchRepairInfo.DeliveryNo)
                        .Set(p => p.ContactPerson, dispatchRepairInfo.ContactPerson)
                        .Set(p => p.ContactPhone, dispatchRepairInfo.ContactPhone)
                        .Set(p => p.SendRepairDate, dispatchRepairInfo.SendRepairDate)
                        .Set(p => p.PredictBackDate, dispatchRepairInfo.PredictBackDate)
                        //项目Id,项目事项Id
                        .Set(p => p.ProjectId, dispatchRepairInfo.ProjectId)
                        .Set(p => p.ProjectKeyItemId, dispatchRepairInfo.ProjectKeyItemId)
                        .Execute();

                //生成操作记录
                this.GenerateOperationReccord(RepairOperationType.Dispatch, dispatchRepairInfo.RepairBillId, now);
                //生成工时记录
                this.GenerateWorkingHours(dispatchRepairInfo.RepairBillId, dispatchRepairInfo.RepairMasterId, repairerIds);
                //附件列表
                this.GenerateBase64Photos(dispatchRepairInfo.PhotoInfos, dispatchRepairInfo.RepairBillId, RepairOperationType.Dispatch);

                trans.Complete();
            }
        }

        /// <summary>
        /// 生成维修单附件Base64图片
        /// </summary>
        /// <param name="photoInfos"></param>
        /// <param name="repairBillId"></param>
        /// <param name="type"></param>
        public virtual void GenerateBase64Photos(List<RepairAttachmentInfo> photoInfos, double repairBillId, RepairOperationType type)
        {
            if (photoInfos == null)
            {
                return;
            }
            var hepler = new FileUrlHelper();
            var attachments = new EntityList<EquipRepairAttachment>();
            photoInfos.ForEach(p =>
            {
                if (p.Id == null)
                {
                    var attachment = hepler.GenerateAttachmentBase64StringContent(new EquipRepairAttachment(), p.Content, p.FileName) as EquipRepairAttachment;
                    attachment.OwnerId = repairBillId;
                    attachment.RepairOperationType = RepairOperationType.ApplyRepair;
                    attachments.Add(attachment);
                }
            });
            RF.Save(attachments);
        }

        /// <summary>
        /// 生成设备维修操作记录
        /// </summary>
        /// <param name="type">操作类型</param>
        /// <param name="repairBillId">维修单ID</param>
        /// <param name="operationTime">操作时间</param>
        /// <param name="remark">备注</param>
        /// <param name="originalRepairMasterId">原维修责任人</param>
        /// <param name="originalRepairer">原维修人员</param>
        /// <param name="handoverConfirmResult">交机确认结果</param>
        /// <param name="engineerConfirmResult">工程确认结果</param>
        public virtual void GenerateOperationReccord(RepairOperationType type, double repairBillId, DateTime operationTime
            , string remark = null, double? originalRepairMasterId = null, string originalRepairer = null, HandoverConfirmResult? handoverConfirmResult = null, EngineerConfirmResult? engineerConfirmResult = null)
        {
            //构建维修记录实体数据
            EquipRepairOperationRec operationRec = new EquipRepairOperationRec();
            operationRec.OperationType = type;
            operationRec.EquipRepairBillId = repairBillId;
            operationRec.OperationerId = RT.IdentityId;
            operationRec.OperationDate = operationTime;
            operationRec.Remark = remark;
            operationRec.OriginalRepairMasterId = originalRepairMasterId;
            operationRec.OriginalRepairer = originalRepairer;
            operationRec.HandoverConfirmResult = handoverConfirmResult;
            operationRec.EngineerConfirmResult = engineerConfirmResult;
            operationRec.IntervalTime = this.GetOperationIntervalTime(type, repairBillId, operationTime);

            using (var trans = DB.TransactionScope(EquipRepairEntityDataProvider.ConnectionStringName))
            {
                //更新维修单信息
                if (operationRec.IntervalTime.HasValue)
                {
                    DB.Update<EquipRepairBill>().Where(p => p.Id == repairBillId).Set(p => p.RepairTime, p => p.RepairTime + operationRec.IntervalTime.Value).Execute();
                }
                //保存操作记录
                RF.Save(operationRec);
                //提交事务
                trans.Complete();
            }
        }

        /// <summary>
        /// 获取维修操作工时间隔
        /// </summary>
        /// <param name="type">操作类型</param>
        /// <param name="repairBillId">维修单ID</param>
        /// <param name="operationTime">操作时间</param>
        /// <returns></returns>
        public virtual decimal? GetOperationIntervalTime(RepairOperationType type, double repairBillId, DateTime operationTime)
        {
            decimal? intervalTime = null;
            var q = Query<EquipRepairOperationRec>();

            //维修单，操作时间倒叙排序
            q.Where(p => p.EquipRepairBillId == repairBillId);
            q.OrderByDescending(p => p.OperationDate);

            //暂停维修 、维修完成、交机确认为NG
            if (type == RepairOperationType.Pause || type == RepairOperationType.Completed || type == RepairOperationType.HandoverConfirm)
            {
                //操作类型为暂停维修，往前寻找距离最近的维修开始节点（开始、继续、交机确认为NG）
                q.WhereIf(type == RepairOperationType.Pause, p => p.OperationType == RepairOperationType.Begin || p.OperationType == RepairOperationType.Continue
                        || (p.OperationType == RepairOperationType.HandoverConfirm && p.HandoverConfirmResult == HandoverConfirmResult.NG));

                //操作类型为维修完成，往前寻找距离最近的维修开始节点（开始、继续、交机确认为NG）
                q.WhereIf(type == RepairOperationType.Completed, p => p.OperationType == RepairOperationType.Begin || p.OperationType == RepairOperationType.Continue
                        || (p.OperationType == RepairOperationType.HandoverConfirm && p.HandoverConfirmResult == HandoverConfirmResult.NG));

                //操作类型为交机确认，往前寻找距离最近的维修完成节点
                q.WhereIf(type == RepairOperationType.HandoverConfirm, p => p.OperationType == RepairOperationType.Completed);

                //查询最后节点时间，计算工时间隔
                var lastDatetime = q.Select(p => new { p.OperationDate }).FirstOrDefault<DateTime>();
                if (lastDatetime > DateTime.MinValue)
                {
                    intervalTime = (decimal)Math.Round((operationTime - lastDatetime).TotalHours, 2);
                }
            }

            return intervalTime;
        }

        /// <summary>
        /// 生成设备维修工时记录
        /// </summary>
        /// <param name="repairBillId">维修单ID</param>
        /// <param name="repairMasterId">维修责任人ID</param>
        /// <param name="repairerIds">维修人员ID列表</param>
        public virtual void GenerateWorkingHours(double repairBillId, double repairMasterId, List<double> repairerIds)
        {
            if (repairerIds == null)
            {
                return;
            }

            if (repairerIds.Contains(repairMasterId))
            {
                throw new ValidationException("其他维修人员无需与主责任人重复维护".L10N());
            }

            using (var trans = DB.TransactionScope(EquipRepairEntityDataProvider.ConnectionStringName))
            {
                var repairBill = RF.GetById<EquipRepairBill>(repairBillId, new EagerLoadOptions().LoadWithViewProperty());

                //生成责任人工时记录
                this.GenerateWorkingHour(repairBill, repairMasterId, true);

                //生成维修人员工时记录
                this.GenerateRepairerWorkingHours(repairBill, repairerIds);

                trans.Complete();
            }
        }

        /// <summary>
        /// 生成设备维修，维修人员工时记录
        /// </summary>
        /// <param name="repairBill">维修单</param>
        /// <param name="repairerIds">维修人员ID列表</param>
        /// <param name="beginDateTime">开始时间</param>
        public virtual void GenerateRepairerWorkingHours(EquipRepairBill repairBill, List<double> repairerIds, DateTime? beginDateTime = null)
        {
            if (repairerIds != null)
            {
                //生成维修人员工时记录
                repairerIds.ForEach(p =>
                {
                    this.GenerateWorkingHour(repairBill, p, false, beginDateTime);
                });
            }
        }

        /// <summary>
        /// 生成设备维修工时记录
        /// </summary>
        /// <param name="repairBill">维修单</param>
        /// <param name="repairerId">维修人员ID</param>
        /// <param name="isMaster">是否维修责任人</param>
        /// <param name="beginDateTime">开始时间</param>
        /// <param name="endDateTime">结束时间</param>
        public virtual void GenerateWorkingHour(EquipRepairBill repairBill, double? repairerId, bool isMaster, DateTime? beginDateTime = null, DateTime? endDateTime = null)
        {
            EquipRepairWorkingHours workingHour = new EquipRepairWorkingHours();
            workingHour.EquipRepairBillId = repairBill.Id;
            workingHour.RepairerId = repairerId;
            workingHour.IsRepairMaster = isMaster;
            workingHour.BeginTime = repairBill.RepairState == EquipRepairState.WaitRepair ? null : beginDateTime;
            workingHour.EndTime = repairBill.RepairState == EquipRepairState.WaitRepair ? null : endDateTime;
            workingHour.IsRepairEmployee = true;
            workingHour.PersistenceStatus = PersistenceStatus.New;
            RF.Save(workingHour);
        }

        /// <summary>
        /// 结束工时(责任人)
        /// </summary>
        /// <param name="repairBillId">维修单ID</param>
        /// <param name="repairerId">维修人ID</param>
        /// <param name="endDateTime">结束时间</param>
        /// <param name="isTransfe">是否转派</param>
        /// <param name="state">维修状态</param>
        public virtual void EndRepairMasterWorkingHour(double repairBillId, double repairerId, DateTime? endDateTime, bool isTransfe, EquipRepairState state)
        {
            DB.Update<EquipRepairWorkingHours>()
                .Where(p => p.EquipRepairBillId == repairBillId)
                .Where(p => p.RepairerId == repairerId)
                .Where(p => p.IsRepairMaster)
                .Where(p => p.IsRepairEmployee)
                .Set(p => p.EndTime, state == EquipRepairState.WaitRepair ? null : endDateTime)
                .Set(p => p.IsRepairEmployee, !isTransfe)
                .Execute();
        }

        /// <summary>
        /// 结束工时(维修人员)
        /// </summary>
        /// <param name="repairBillId">维修单ID</param>
        /// <param name="repairerIds">维修人员ID列表</param>
        /// <param name="endDateTime">结束时间</param>
        /// <param name="isTransfe">是否转派</param>
        /// <param name="state">维修状态</param>
        public virtual void EndRepairersWorkingHours(double repairBillId, List<double> repairerIds, DateTime? endDateTime, bool isTransfe, EquipRepairState state)
        {
            DB.Update<EquipRepairWorkingHours>()
                .Where(p => p.EquipRepairBillId == repairBillId)
                .Where(p => repairerIds.Contains((double)p.RepairerId))
                .Where(p => !p.IsRepairMaster)
                .Where(p => !p.IsRepairEmployee)
                .Set(p => p.EndTime, state == EquipRepairState.WaitRepair ? null : endDateTime)
                .Set(p => p.IsRepairEmployee, !isTransfe)
                .Execute();
        }

        /// <summary>
        /// 保存维修工时数据
        /// </summary>
        /// <param name="infos"></param>
        /// <param name="repairBillId"></param>
        public virtual void SaveRepairWorkingHours(List<RepairSaveSubmitWorkingHoursInfo> infos, double repairBillId)
        {
            using (var trans = DB.TransactionScope(EquipRepairEntityDataProvider.ConnectionStringName))
            {
                //查询维修单
                var repairBill = this.GetEquipRepairBill(repairBillId);

                //按照操作类型倒叙排序，先执行删除，修改，最后执行新增，避免逻辑校验冲突
                var descInfos = infos.OrderByDescending(p => p.ActionType);

                //校验
                if (descInfos.Any(p => p.RepairWorkingHourId <= 0 && p.ActionType != 0))
                {
                    throw new ValidationException("存在非新增的维修工时ID为0的数据".L10N());
                }
                if (repairBill.RepairBeginDate != null)
                {
                    var sRepairBeginDate = DateTime.Parse(repairBill.RepairBeginDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (descInfos.Any(p => p.BeginTime != null && p.BeginTime.Value < sRepairBeginDate))
                    {
                        throw new ValidationException("存在维修工时开始时间早于维修单开始时间".L10N());
                    }
                }
                if (descInfos.Any(p => p.BeginTime == null && p.EndTime != null))
                {
                    throw new ValidationException("存在维修工时选择了结束时间，但开始时间为空".L10N());
                }

                //执行逻辑
                foreach (var info in descInfos)
                {
                    switch (info.ActionType)
                    {
                        case 0:
                            {
                                //新增
                                this.GenerateWorkingHour(repairBill, info.RepairerId, info.IsRepairMaster, info.BeginTime, info.EndTime);
                                break;
                            }
                        case 1:
                            {
                                //修改
                                DB.Update<EquipRepairWorkingHours>().Where(p => p.Id == info.RepairWorkingHourId)
                                    .Set(p => p.BeginTime, info.BeginTime)
                                    .Set(p => p.EndTime, info.EndTime)
                                    .Set(p => p.IsRepairMaster, info.IsRepairMaster)
                                    .Execute();
                                break;
                            }
                        case 2:
                            {
                                //删除
                                DB.Delete<EquipRepairWorkingHours>().Where(p => p.Id == info.RepairWorkingHourId).Execute();
                                break;
                            }
                        default:
                            break;
                    }
                }

                //查询变更后维修单的维修工时列表
                var workingHours = Query<EquipRepairWorkingHours>()
                    .Where(p => p.EquipRepairBillId == repairBillId && p.IsRepairEmployee && p.IsRepairEmployee)
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());

                //校验维修单最后是否只有一个维修责任人工时
                if (workingHours.Count(p => p.IsRepairMaster) > 1)
                {
                    throw new ValidationException("维修工时存在两个维修责任人".L10N());
                }
                //变更后，维修人员信息列表
                var repairers = workingHours.Where(p => !p.IsRepairMaster && p.RepairerId != null).ToList();
                var repairerIds = repairers.Select(p => p.RepairerId).ToList();
                var repairerNames = repairers.Select(p => p.RepairerNameView).ToList();
                var repairerNamesStr = string.Join(",", repairerNames);
                var repairerIdsStr = string.Join(",", repairerIds);
                //变更后维修责任人ID
                var repairMasterId = workingHours.Where(p => p.IsRepairMaster).Select(p => p.RepairerId).FirstOrDefault();

                //回写维修单维修责任人和维修人员
                DB.Update<EquipRepairBill>().Where(p => p.Id == repairBillId)
                    .Set(p => p.RepairMasterId, repairMasterId)
                    .Set(p => p.RepairEmployeeIds, repairerIdsStr)
                    .Set(p => p.RepairEmployees, repairerNamesStr)
                    .Execute();

                //提交事务
                trans.Complete();
            }
        }

        /// <summary>
        /// 开始维修
        /// </summary>
        /// <param name="repairBillId">维修单ID</param>
        /// <param name="produceState">生产状态</param>
        public virtual void BeginRepair(double repairBillId, ProduceState produceState)
        {
            //校验单据状态并返回单据设备台账ID
            var accountId = this.ValidateRepairState(repairBillId, EquipRepairState.WaitRepair);

            //当前数据库操作时间
            var now = this.GetDbTime();

            using (var trans = DB.TransactionScope(EquipRepairEntityDataProvider.ConnectionStringName))
            {
                //更新维修单
                DB.Update<EquipRepairBill>().Where(p => p.Id == repairBillId)
                    .Set(p => p.RepairState, EquipRepairState.Repairing)
                    .Set(p => p.RepairBeginDate, now)
                    .Set(p => p.ProduceState, produceState)
                    .Set(p => p.RepairDowntime, produceState == ProduceState.StopWork)
                    .Execute();

                //生成操作记录
                this.GenerateOperationReccord(RepairOperationType.Begin, repairBillId, now);
                //更新维修工时开始时间
                DB.Update<EquipRepairWorkingHours>().Where(p => p.EquipRepairBillId == repairBillId).Set(p => p.BeginTime, now).Execute();
                //改变设备状态
                if (produceState == ProduceState.StopWork)
                    DB.Update<EquipAccount>().Where(p => p.Id == accountId).Set(p => p.State, AccountState.Fault).Execute();

                trans.Complete();
            }
        }

        /// <summary>
        /// 暂停维修
        /// </summary>
        /// <param name="repairBillId">维修单ID</param>
        /// <param name="reason">暂停原因</param>
        public virtual void SuspendRepair(double repairBillId, string reason)
        {
            //当前数据库操作时间
            var now = this.GetDbTime();

            using (var trans = DB.TransactionScope(EquipRepairEntityDataProvider.ConnectionStringName))
            {
                //更新维修单
                DB.Update<EquipRepairBill>().Where(p => p.Id == repairBillId).Set(p => p.RepairState, EquipRepairState.Suspending).Execute();

                //生成操作记录
                this.GenerateOperationReccord(RepairOperationType.Pause, repairBillId, now, reason);

                trans.Complete();
            }
        }

        /// <summary>
        /// 继续维修
        /// </summary>
        /// <param name="repairBillId">维修单ID</param>
        public virtual void ContinueRepair(double repairBillId)
        {
            //当前数据库操作时间
            var now = this.GetDbTime();

            using (var trans = DB.TransactionScope(EquipRepairEntityDataProvider.ConnectionStringName))
            {
                //更新维修单
                DB.Update<EquipRepairBill>().Where(p => p.Id == repairBillId).Set(p => p.RepairState, EquipRepairState.Repairing).Execute();

                //生成操作记录
                this.GenerateOperationReccord(RepairOperationType.Continue, repairBillId, now);

                trans.Complete();
            }
        }

        /// <summary>
        /// 维修继续
        /// </summary>
        /// <param name="equipRepairBill"></param>
        public virtual void Continue(EquipRepairBill equipRepairBill)
        {
            if (equipRepairBill == null)
            {
                return;
            }
            //添加操作记录
            equipRepairBill.EquipRepairOperationRecList.Add(new EquipRepairOperationRec()
            {
                OperationType = RepairOperationType.Continue,

                OperationerId = RT.IdentityId,
                OperationDate = DateTime.Now
            });
            equipRepairBill.RepairState = EquipRepairState.Repairing;
            RF.Save(equipRepairBill);
        }

        /// <summary>
        /// 取消
        /// </summary>
        public virtual void BillCancel(BillCancelViewModel billCancelViewModel)
        {
            if (billCancelViewModel == null)
            {
                return;
            }
            var equipRepairBill = RF.GetById<EquipRepairBill>(billCancelViewModel.Id);
            equipRepairBill.EquipRepairOperationRecList.Add(new EquipRepairOperationRec()
            {
                OperationType = RepairOperationType.Cancel,
                Remark = billCancelViewModel.CancelReason,
                
                OperationerId = RT.IdentityId,
                OperationDate = DateTime.Now
            });
            equipRepairBill.RepairState = EquipRepairState.Cancel;
            equipRepairBill.CancelReason = billCancelViewModel.CancelReason;
            //查询该设备是否还有未完成的维修单，若没有，将使用状态改成设备使用状态改成使用中
            var isCount = Query<EquipRepairBill>().Where(p => p.Id != equipRepairBill.Id && p.EquipAccountId == equipRepairBill.EquipAccountId && (p.RepairState != EquipRepairState.Cancel && p.RepairState != EquipRepairState.Completed && p.RepairState != EquipRepairState.Closed)).Count() > 0;
            if (!isCount)
            {
                DB.Update<EquipAccount>()
                  .Set(s => s.UseState, AccountUseState.Using)
                  .Where(p => p.Id == equipRepairBill.EquipAccountId).Execute();
            }
            RF.Save(equipRepairBill);
        }

        /// <summary>
        /// 强制关单
        /// </summary>
        /// <param name="compelCloseViewModel"></param>
        public virtual void CompelClose(CompelCloseViewModel compelCloseViewModel)
        {
            if (compelCloseViewModel == null)
            {
                return;
            }
            var equipRepairBill = RF.GetById<EquipRepairBill>(compelCloseViewModel.Id, new EagerLoadOptions().LoadWith(EquipRepairBill.EquipAccountProperty));
            //添加操作记录
            equipRepairBill.EquipRepairOperationRecList.Add(new EquipRepairOperationRec()
            {
                OperationType = RepairOperationType.CompelClose,
                Remark = compelCloseViewModel.CloseReason,
                OperationerId = RT.IdentityId,
                OperationDate = DateTime.Now
            });
            //计算备件费用与维修工时
            CalculateSparePartsCostAndRepairHours(equipRepairBill);
            equipRepairBill.RepairState = EquipRepairState.Closed;
            if (equipRepairBill.EquipAccount != null)
            {
                equipRepairBill.EquipAccount.State = AccountState.Fault;
            }
            //查询该设备是否还有未完成的维修单，若没有，将使用状态改成设备使用状态改成使用中
            var isCount= Query<EquipRepairBill>().Where(p => p.Id != equipRepairBill.Id && p.EquipAccountId == equipRepairBill.EquipAccountId && (p.RepairState != EquipRepairState.Cancel && p.RepairState!= EquipRepairState.Completed && p.RepairState != EquipRepairState.Closed)).Count()>0;
            if (!isCount)
            {
                DB.Update<EquipAccount>()
                  .Set(s => s.UseState, AccountUseState.Using)
                  .Where(p => p.Id == equipRepairBill.EquipAccountId).Execute();
            }
            RF.Save(equipRepairBill);
        }

        /// <summary>
        /// 设备维修转派提交
        /// </summary>
        /// <param name="transfeRepairInfo"></param>
        public virtual void TransferRepair(TransfeRepairInfo transfeRepairInfo)
        {
            //校验设备派工
            var repairerIds = transfeRepairInfo.Repairers.Select(p => p.EmployeeId).ToList();
            this.ValidateTransferRepair(transfeRepairInfo, repairerIds);

            //维修人员名字列表
            var repairerNames = transfeRepairInfo.Repairers.Select(p => p.EmployeeName);
            var repairerNamesStr = string.Join(",", repairerNames);
            var repairerIdsStr = string.Join(",", repairerIds);

            //原维修人员名字列表
            var originalRepairerNames = transfeRepairInfo.OriginalRepairers.Select(p => p.EmployeeName);
            var originalRepairerNamesStr = string.Join(",", originalRepairerNames);
            var originalRepairerIds = transfeRepairInfo.OriginalRepairers.Select(p => p.EmployeeId).ToList();


            //当前数据库操作时间
            var now = this.GetDbTime();
            var repairBill = RF.GetById<EquipRepairBill>(transfeRepairInfo.RepairBillId, new EagerLoadOptions().LoadWithViewProperty());

            //获取当前维修单的所有维修工时记录
            var equipRepairWorkingHourLis = this.GetEquipRepairWorkingHoursNotEnd(transfeRepairInfo.RepairBillId);

            using (var trans = DB.TransactionScope(EquipRepairEntityDataProvider.ConnectionStringName))
            {
                //更新维修单信息
                DB.Update<EquipRepairBill>().Where(p => p.Id == transfeRepairInfo.RepairBillId)
                        .Set(p => p.RepairWay, (EquipRepairWay)transfeRepairInfo.RepairWay)
                        .Set(p => p.RepairMasterId, transfeRepairInfo.RepairMasterId)
                        .Set(p => p.RepairEmployees, repairerNamesStr)
                        .Set(p => p.RepairEmployeeIds, repairerIdsStr)
                        .Set(p => p.TransferOrderDate, now)
                        .Set(p => p.EstimateFinishDate, transfeRepairInfo.EstimateFinishDate)

                        .Set(p => p.SendRepairWay, (SendRepairWay?)transfeRepairInfo.SendRepairWay)
                        .Set(p => p.SupplierId, transfeRepairInfo.SupplierId)
                        .Set(p => p.DeliveryNo, transfeRepairInfo.DeliveryNo)
                        .Set(p => p.ContactPerson, transfeRepairInfo.ContactPerson)
                        .Set(p => p.ContactPhone, transfeRepairInfo.ContactPhone)
                        .Set(p => p.SendRepairDate, transfeRepairInfo.SendRepairDate)
                        .Set(p => p.PredictBackDate, transfeRepairInfo.PredictBackDate)
                        .Execute();

                //生成操作记录
                this.GenerateOperationReccord(RepairOperationType.Transfer, transfeRepairInfo.RepairBillId, now, transfeRepairInfo.Remark, transfeRepairInfo.OriginalRepairMasterId, originalRepairerNamesStr);

                //创建维修工时处理
                CreateRepairerWorkingHours(transfeRepairInfo, equipRepairWorkingHourLis, now, repairerIds, originalRepairerIds, repairBill);

                trans.Complete();
            }
        }


        /// <summary>
        /// 派工规则
        /// </summary>
        /// <param name="transfeRepairInfo"></param>
        /// <param name="equipRepairWorkingHourLis"></param>
        /// <param name="now"></param>
        /// <param name="repairerIds"></param>
        /// <param name="originalRepairerIds"></param>
        /// <param name="repairBill"></param>
        public virtual void CreateRepairerWorkingHours(TransfeRepairInfo transfeRepairInfo, EntityList<EquipRepairWorkingHours> equipRepairWorkingHourLis, DateTime now, List<double> repairerIds, List<double> originalRepairerIds, EquipRepairBill repairBill)
        {
            //1、当维修工时页签所有人的开始时间都没值时：
            //     转派后的人和已有的人做比较：交集的人的数据勾选【当前维修人】，新增多了的人的数据，少了的人的数据更新【当前维修】字段为不勾选。如果维修责任人有更新，则原责任人取消勾选，新责任人勾选
            //2、当维修工时页签存在开始时间有值时：
            //  转派后的人和已有的且是当前维修人的数据做比较：交集的人数据不动，新增多了的人的数据（开始时间取值当前时间），少了的人的数据更新【当前维修】字段为不勾选和结束时间更新为当前时间。如果维修责任人有更新，则新责任人勾选，原责任人看还在不在转派后的维修人中，在则取消责任人的勾选，

            //责任人有变更
            if (transfeRepairInfo.RepairMasterId != transfeRepairInfo.OriginalRepairMasterId)
            {
                //查旧负责人的工时数据
                var OriginalRepairMaster = equipRepairWorkingHourLis.FirstOrDefault(p => p.RepairerId == transfeRepairInfo.OriginalRepairMasterId && p.IsRepairEmployee && p.IsRepairMaster);
                if (OriginalRepairMaster != null)
                {
                    //取消旧责任人为负责人和当前维修人
                    OriginalRepairMaster.IsRepairMaster = false;
                    OriginalRepairMaster.IsRepairEmployee = false;
                    //如新维修人员不包含旧负责人 ,包含则不赋值结束时间由责任人变更为普通维修人
                    if (!repairerIds.Contains(transfeRepairInfo.OriginalRepairMasterId) && OriginalRepairMaster.BeginTime.HasValue)
                    {
                        //如旧负责人有开始时间,则终结此条数据
                        OriginalRepairMaster.EndTime = now;
                    }
                }

                //查新负责人的工时数据
                var RepairMaster = equipRepairWorkingHourLis.FirstOrDefault(p => p.RepairerId == transfeRepairInfo.RepairMasterId && p.EndTime == null);
                if (RepairMaster != null)
                {
                    //新负责人存在
                    RepairMaster.IsRepairMaster = true;
                    RepairMaster.IsRepairEmployee = true;
                    if (repairBill.RepairState != EquipRepairState.WaitRepair && RepairMaster.BeginTime == null)
                    {
                        RepairMaster.BeginTime = now;
                    }
                }
                else
                {
                    //新负责人不存在,则新建
                    this.GenerateWorkingHour(repairBill, transfeRepairInfo.RepairMasterId, true, now);
                }
            }
            //生成维修人员工时记录
            var addRepairerIds = new List<double>();


            //处理旧的
            var equipRepairWorking = equipRepairWorkingHourLis.Where(p => originalRepairerIds.Contains((double)p.RepairerId)).ToList();

            foreach (var item in equipRepairWorking)
            {
                //如果当前旧维修人不在新维修人员列表  且 旧维修人员不为新责任人
                if (!repairerIds.Contains((double)item.RepairerId) && item.RepairerId != transfeRepairInfo.RepairMasterId)
                {
                    item.IsRepairEmployee = false;
                    if (item.BeginTime.HasValue)
                    {
                        item.EndTime = now;
                    }
                }
            }

            //处理新的
            //循环此次派工所有的维修人员
            foreach (var employeeId in repairerIds)
            {
                var repairer = equipRepairWorkingHourLis.FirstOrDefault(p => p.RepairerId == employeeId);
                //此人不是新维修人员的，则处理他是不是已经包含在工时列表中了
                if (repairer != null)
                {
                    repairer.IsRepairEmployee = true;
                    if (repairBill.RepairState != EquipRepairState.WaitRepair && repairer.BeginTime == null)
                    {
                        repairer.BeginTime = now;
                    }
                }
                else
                {
                    //新派工的人
                    addRepairerIds.Add(employeeId);
                }
            }

            if (addRepairerIds.Any())
            {
                this.GenerateRepairerWorkingHours(repairBill, addRepairerIds, now);
            }

            RF.Save(equipRepairWorkingHourLis);
        }


        /// <summary>
        /// 添加经验库(设备维修)
        /// </summary>
        /// <param name="expDepotInfo"></param>
        public virtual void AddAccountExperienceDepot(ExperienceDepotInfo expDepotInfo)
        {
            if (expDepotInfo == null)
            {
                return;
            }
            //校验参数
            this.ValidateExperienceDepot(expDepotInfo);
            //创建实体，获取编码值
            ExperienceDepot experienceDepot = new ExperienceDepot();
            var code = RT.Service.Resolve<ExperienceDepotController>().GetCode();
            //赋值
            experienceDepot.Code = code;
            experienceDepot.RepairType = ExperienceDepots.Enums.ExpRepairType.Account;
            experienceDepot.EquipAccountId = expDepotInfo.EquipAccountId;
            experienceDepot.EquipModelId = expDepotInfo.EquipModelId;
            experienceDepot.EquipTypeId = expDepotInfo.EquipTypeId;
            experienceDepot.RepairNo = expDepotInfo.RepairBillNo;
            experienceDepot.FaultReson = expDepotInfo.FaultReasonCode;
            experienceDepot.EquipLargeFaultId = expDepotInfo.FaultCategoryId;
            experienceDepot.FaultPart = expDepotInfo.FaultPart;
            experienceDepot.FaultPhenomenonId = expDepotInfo.DeviceAbnormalId;
            experienceDepot.FaultPhenomenonRemark = expDepotInfo.DeviceAbnormalRemark;
            experienceDepot.FaultDescribeId = expDepotInfo.FaultDescriptionId;
            experienceDepot.FaultDescribeRemark = expDepotInfo.FaultDescriptionRemark;
            experienceDepot.RepairWay = expDepotInfo.RepairMethod;
            experienceDepot.PreventionAdvice = expDepotInfo.PreventionAdvice;
            experienceDepot.FaultCode = expDepotInfo.FaultCode;

            using (var trans = DB.TransactionScope(EquipRepairEntityDataProvider.ConnectionStringName))
            {
                RF.Save(experienceDepot);
                //附件列表(将维修报告的图片上传到维修经验库)
                GenerateExpBase64Photos(expDepotInfo.PhotoInfos, experienceDepot.Id);
                trans.Complete();
            }
        }


        /// <summary>
        /// 验证维修报告
        /// </summary>
        /// <param name="repairId">维修单ID</param>
        /// <returns>错误信息</returns>
        public virtual RepairFinishResultInfo VerifyRepairReport(double repairId)
        {
            var repairBill = RF.GetById<EquipRepairBill>(repairId, new EagerLoadOptions().LoadWithViewProperty());
            List<string> errorProperty = new List<string>();
            if (repairBill.FaultDescriptionId == null || repairBill.FaultDescriptionId == 0)
            {
                errorProperty.Add("故障描述".L10N());
            }
            if (repairBill.FaultReason.IsNullOrWhiteSpace())
            {
                errorProperty.Add("故障原因".L10N());
            }
/*            if (repairBill.FaultPart.IsNullOrWhiteSpace())
            {
                errorProperty.Add("故障部位".L10N());
            }*/
            if (repairBill.FaultCategoryId == null || repairBill.FaultCategoryId == 0)
            {
                errorProperty.Add("故障类别".L10N());
            }
            if (repairBill.FaultLevel == null)
            {
                errorProperty.Add("故障等级".L10N());
            }
            if (repairBill.RepairCategory == null)
            {
                errorProperty.Add("维修类别".L10N());
            }
            if (repairBill.RepairLevel == null)
            {
                errorProperty.Add("维修等级".L10N());
            }
            if (repairBill.RepairMethod.IsNullOrWhiteSpace())
            {
                errorProperty.Add("维修方法".L10N());
            }
           /* if (repairBill.PreventionAdvice.IsNullOrWhiteSpace())
            {
                errorProperty.Add("预防建议".L10N());
            }*/

            return new RepairFinishResultInfo()
            {
                isEngineerConfirm = true,
                ErrMsg = errorProperty.Any() ? "维修报告的".L10N() + string.Join("、", errorProperty.ToArray()) + "不能为空,请先填写".L10N() : ""
            };
        }


        /// <summary>
        /// 生成维修经验库附件Base64图片
        /// </summary>
        /// <param name="photoInfos"></param>
        /// <param name="experienceId"></param>
        public virtual void GenerateExpBase64Photos(List<RepairAttachmentInfo> photoInfos, double experienceId)
        {
            if (photoInfos == null)
            {
                return;
            }
            var hepler = new FileUrlHelper();
            var attachments = new EntityList<ExperienceDepotAttachment>();
            photoInfos.ForEach(p =>
            {
                if (p.Id == null)
                {
                    var attachment = hepler.GenerateAttachmentBase64StringContent(new ExperienceDepotAttachment(), p.Content, p.FileName) as ExperienceDepotAttachment;
                    attachment.OwnerId = experienceId;
                    attachments.Add(attachment);
                }
            });
            RF.Save(attachments);
        }

        /// <summary>
        /// 验证维修报告是否加入过经验库
        /// </summary>
        /// <param name="repairNo">维修单号</param>
        /// <returns>错误信息</returns>
        public virtual bool VerifyRepairReportIsAddDeopt(string repairNo)
        {
            return Query<ExperienceDepot>().Where(p => p.RepairNo == repairNo).ToList().Any();
        }

        /// <summary>
        /// 维修完成校验是否填写完维修报告
        /// </summary>
        /// <param name="repairBill">维修单</param>
        public virtual string FinishiRepairValidate(EquipRepairBill repairBill)
        {
            bool isEngineerConfirm = IsConfigEngineerConfirm();

            List<string> errorProperty = new List<string>();
            if (repairBill.FaultDescriptionId == null || repairBill.FaultDescriptionId == 0)
            {
                errorProperty.Add("故障描述".L10N());
            }
            if (repairBill.FaultReason.IsNullOrWhiteSpace())
            {
                errorProperty.Add("故障原因".L10N());
            }
            if (repairBill.FaultCategoryId == null || repairBill.FaultCategoryId == 0)
            {
                errorProperty.Add("故障类别".L10N());
            }
            if (repairBill.FaultLevel == null)
            {
                errorProperty.Add("故障等级".L10N());
            }
            if (repairBill.RepairCategory == null)
            {
                errorProperty.Add("维修类别".L10N());
            }
            if (repairBill.RepairLevel == null)
            {
                errorProperty.Add("维修等级".L10N());
            }
            if (repairBill.RepairMethod.IsNullOrWhiteSpace())
            {
                errorProperty.Add("维修方法".L10N());
            }
            if (errorProperty.Count > 0)
            {
                if (isEngineerConfirm)
                {
                    return "{0}未填写".L10nFormat(string.Join("、", errorProperty.ToArray()));
                }
                else
                {
                    throw new ValidationException("{0}未填写".L10nFormat(string.Join("、", errorProperty.ToArray())));
                }
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 维修完成更新设备状态
        /// </summary>
        /// <param name="repairId">维修单Id</param>
        /// <param name="equipId">设备id</param>
        private void RepairUpdateEquipState(double repairId, double equipId)
        {
            var count = Query<EquipRepairBill>().Where(p => p.Id != repairId && p.EquipAccountId == equipId && p.RepairState != EquipRepairState.Completed && p.RepairState != EquipRepairState.Cancel && p.RepairState != EquipRepairState.Closed).Count();
            if (count <= 0)
            {
                DB.Update<EquipAccount>().Set(p => p.UseState, AccountUseState.Using).Where(p => p.Id == equipId).Execute();
            }
        }

        /// <summary>
        /// 完成维修
        /// </summary>
        /// <param name="repairBill">维修单ID</param>
        /// <param name="isFillInReport">是否填写完维修报告</param>
        /// <returns>错误信息</returns>
        public virtual RepairFinishResultInfo FinishRepair(EquipRepairBill repairBill, bool isFillInReport)
        {
            var repairId = repairBill.Id;
            bool isHandoverConfirm = IsConfigHandoverConfirm();
            bool isEngineerConfirm = IsConfigEngineerConfirm();

            repairBill.RepairFinishDate = DateTime.Now;

            VailuDate(repairBill);
            if (isHandoverConfirm)
            {
                repairBill.RepairState = EquipRepairState.WaitConfirm;
            }
            else if (isEngineerConfirm)
            {
                repairBill.RepairState = EquipRepairState.WaitScore;
            }
            else
            {
                //计算备件费用与维修工时
                CalculateSparePartsCostAndRepairHours(repairBill);
                repairBill.RepairState = EquipRepairState.Completed;
            }

            repairBill.IsSupplement = !isFillInReport;
            //查询是否还存在其他未完成的维修单，不存在更新设备管理状态为使用中
            using (var trans = DB.TransactionScope(EquipRepairEntityDataProvider.ConnectionStringName))
            {
                //更新设备维修单的状态和完成时间
                RF.Save(repairBill);
                
                //更新该设备维修单下维修工时中的结束时间字段(结束时间为空才更新)
                DB.Update<EquipRepairWorkingHours>().Set(s => s.EndTime, (DateTime)repairBill.RepairFinishDate).Where(p => p.EquipRepairBillId == repairId && p.EndTime == null && p.IsRepairEmployee).Execute();

                //若为设备维修，更新设备台账的设备状态为“运行”状态
                if (repairBill.RepairType == EquipRepairType.EquipRepair)
                {   
                    //配置项【交机确认与工程确认】同时为否时，点击‘维修完成’维修状态变更为‘已完成时，判断当前设备是否存在未完成的维修，不存在时，变更设备管理状态为‘使用中’
                    if (!isHandoverConfirm && !isEngineerConfirm)
                    {
                        RepairUpdateEquipState(repairBill.Id, repairBill.EquipAccountId.Value);
                    }
                    DB.Update<EquipAccount>().Set(s => s.State, AccountState.Running).Where(p => p.Id == repairBill.EquipAccountId).Execute();
                    //保存设备履历
                    using (SIE.DataAuth.DataAuths.LoadAll())
                    {
                        var equip = Query<EquipAccount>().Where(p => p.Id == repairBill.EquipAccountId).Select(p => new
                        {
                            Id = p.Id,
                            State = p.State,
                        }).FirstOrDefault<EquipInfo>();
                        if (repairBill.RepairType == EquipRepairType.EquipRepair && equip != null && equip.Id != 0)
                        {
                            RT.Service.Resolve<EquipController>().GenerateEquipAccountResume(equip.Id, ResumeType.Repair, equip.State, repairBill.RepairNo);
                        }
                    }
                }

                //更新备件更换记录标记
                if (repairBill.RepairState == EquipRepairState.Completed)
                {
                    RT.Service.Resolve<SparePartController>().UpdateSparePartChangedRecordFlag(FromType.Upkeep, repairBill.Id);
                }
                //生成操作记录
                GenerateOperationReccord(RepairOperationType.Completed, repairId, (DateTime)repairBill.RepairFinishDate);
                trans.Complete();
            }
            return new RepairFinishResultInfo()
            {
                isEngineerConfirm = isEngineerConfirm,
                ErrMsg = ""
            };
        }

        /// <summary>
        /// 验证
        /// </summary>
        protected virtual void VailuDate(EquipRepairBill repairBill)
        {
            //if (!repairBill.EquipRepairSparePartChgList.All(p => p.State == Enums.ChangeSparePartState.Finished))
            //{
            //    throw new ValidationException("存在【备件更换】状态为非已完成状态，请确认！".L10N());
            //}
            if (repairBill.EquipRepairWorkingHoursList.Where(p => p.IsRepairEmployee).Any(a => a.BeginTime == null))
            {
                throw new ValidationException("维修工时存在当前维修人员的开始时间为空，请确认！".L10N());
            }
            var sRepairBeginDate = DateTime.Parse(repairBill.RepairBeginDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            if (repairBill.EquipRepairWorkingHoursList.Any(a => a.BeginTime != null && a.BeginTime < sRepairBeginDate))
            {
                throw new ValidationException("维修工时中存在员工开始时间早于维修开始时间，请确认！".L10N());
            }
            if (repairBill.EquipRepairWorkingHoursList.Any(a => a.EndTime != null && a.EndTime > repairBill.RepairFinishDate))
            {
                throw new ValidationException("维修工时中存在员工结束时间晚于当前完成时间，请确认！".L10N());
            }
        }


        /// <summary>
        /// 交机确认
        /// </summary>
        /// <param name="repairBill">设备维修单</param>
        /// <param name="detailList">交机评分明细</param>
        /// <param name="content"></param>
        public virtual void HandoverConfirm(EquipRepairBill repairBill, EntityList<HandoverConfirmDetail> detailList, string content = "")
        {
            //获取拥有维修确认权限员工列表
            if (detailList.Any() && repairBill.HandoverConfirmResult == HandoverConfirmResult.OK && detailList.Any(p => p.EquipRepairScore == null))
            {
                throw new ValidationException("存在项目未完成评分,请确认！".L10N());
            }

            if (repairBill.HandoverConfirmResult == HandoverConfirmResult.NG && (repairBill.HandoverDeviceAbnormalId == null || repairBill.HandoverDeviceAbnormalId == 0) && repairBill.HandoverDeviceAbnormalRem.IsNullOrEmpty())
            {
                throw new ValidationException("故障现象未填写,请确认！".L10N());
            }

            bool isConfigEngineerConfirm = IsConfigEngineerConfirm();//工程确认
            bool isHandoverConfirm = IsConfigHandoverConfirm();//交机确认

            var states = new List<EquipRepairState>()
                {
                    EquipRepairState.ApplyRepair,
                    EquipRepairState.Repairing,
                    EquipRepairState.Suspending,
                    EquipRepairState.WaitConfirm,
                    EquipRepairState.WaitRepair
                };
            //判断是否还存在其他未完成的设备维修单
            var isExist = IsExistRepairBillsByRepairId(repairBill.EquipAccountId.Value, repairBill.Id, states);

            using (var trans = DB.TransactionScope(EquipRepairEntityDataProvider.ConnectionStringName))
            {
                if (repairBill.HandoverConfirmResult == HandoverConfirmResult.OK)
                {
                    var state = isConfigEngineerConfirm ? EquipRepairState.WaitScore : EquipRepairState.Completed;
                    //更新交机确认结果
                    DB.Update<EquipRepairBill>()
                        .Set(s => s.HandoverConfirmEmployeeId, RT.IdentityId)
                        .Set(s => s.RepairState, state)
                        .Set(s => s.HandoverConfirmResult, HandoverConfirmResult.OK)
                        .Where(p => p.Id == repairBill.Id).Execute();

                    //更新备件更换记录标记
                    if (state == EquipRepairState.Completed)
                    {
                        RT.Service.Resolve<SparePartController>().UpdateSparePartChangedRecordFlag(FromType.Upkeep, repairBill.Id);
                    }
                    //保存维修评分项目
                    RF.Save(detailList);
                }
                else
                {
                    //更新交机确认结果和故障信息
                    var bill = RF.GetById<EquipRepairBill>(repairBill.Id, new EagerLoadOptions().LoadWithViewProperty());
                    bill.HandoverConfirmEmployeeId = RT.IdentityId;
                    bill.RepairState = EquipRepairState.Repairing;
                    bill.HandoverConfirmResult = HandoverConfirmResult.NG;
                    bill.HandoverConfirmAbnormal = repairBill.HandoverConfirmAbnormal;
                    bill.HandoverDeviceAbnormalId = repairBill.HandoverDeviceAbnormalId == 0 ? null : repairBill.HandoverDeviceAbnormalId;
                    bill.HandoverDeviceAbnormalRem = repairBill.HandoverDeviceAbnormalRem;
                    bill.HandoverAttachment = repairBill.HandoverAttachment;

                    //保存图片信息到附件中
                    if (repairBill.HandoverAttachment.IsNotEmpty())
                    {
                        EquipRepairAttachment attachment = new EquipRepairAttachment();
                        attachment.FileName = System.IO.Path.GetFileName(repairBill.HandoverAttachment);
                        attachment.FilePath = repairBill.HandoverAttachment;
                        attachment.Content = Convert.FromBase64String(content == "" ? FileUrlHelper.GetAttachmentBase64StringData(attachment.FilePath, attachment.FileName) : content);
                        attachment.FileSize = FileUrlHelper.FormatFileSize(attachment.Content.Length);
                        attachment.FileExtesion = System.IO.Path.GetExtension(repairBill.HandoverAttachment);
                        attachment.RepairOperationType = RepairOperationType.HandoverConfirm;
                        bill.AttachmentList.Add(attachment);
                        bill.PersistenceStatus = PersistenceStatus.Modified;
                    }
                    RF.Save(bill);
                }

                //生成操作记录
                GenerateOperationReccord(RepairOperationType.HandoverConfirm, repairBill.Id, GetDbTime()
                    , remark: (repairBill.HandoverConfirmResult == HandoverConfirmResult.OK ? null : repairBill.HandoverDeviceAbnormalRem)
                    , handoverConfirmResult: repairBill.HandoverConfirmResult);

                //配置项中【交机确认】为是时，【工程确认】为‘否’时，点击‘交机确认’维修状态变更为‘已完成时，判断当前设备是否存在未完成的维修，不存在时，变更设备管理状态为‘使用中’
                if (!isConfigEngineerConfirm)
                {
                    RepairUpdateEquipState(repairBill.Id, repairBill.EquipAccountId.Value);
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 工程确认
        /// </summary>
        /// <param name="repairBill">设备维修单</param>
        /// <param name="detailList">工程评分明细</param>
        public virtual void EngineerConfirm(EquipRepairBill repairBill, EntityList<EngineerConfirmDetail> detailList)
        {
            if (detailList.Any(p => p.EquipRepairScore == null))
            {
                throw new ValidationException("存在项目未完成评分,请确认！".L10N());
            }
            //工程确认配置项
            bool isConfigEngineerConfirm = IsConfigEngineerConfirm();

            var states = new List<EquipRepairState>()
                {
                    EquipRepairState.ApplyRepair,
                    EquipRepairState.Repairing,
                    EquipRepairState.Suspending,
                    EquipRepairState.WaitConfirm,
                    EquipRepairState.WaitRepair
                };
            //判断是否还存在其他未完成的设备维修单
            var isExist = IsExistRepairBillsByRepairId(repairBill.EquipAccountId.Value, repairBill.Id, states);

            //计算备件费用与维修工时
            CalculateSparePartsCostAndRepairHours(repairBill);

            using (var trans = DB.TransactionScope(EquipRepairEntityDataProvider.ConnectionStringName))
            {
                //更新交机确认结果和故障信息
                DB.Update<EquipRepairBill>()
                    .Set(s => s.EngineerConfirmResult, EngineerConfirmResult.Confirmed)
                    .Set(s => s.RepairState, EquipRepairState.Completed)
                    .Set(s => s.RepairHours, repairBill.RepairHours)
                    .Set(s => s.SparePartsCost, repairBill.SparePartsCost)
                    .Where(p => p.Id == repairBill.Id).Execute();

                //更新备件更换记录标记
                RT.Service.Resolve<SparePartController>().UpdateSparePartChangedRecordFlag(FromType.Upkeep, repairBill.Id);

                //保存维修评分项目
                RF.Save(detailList);

                //生成操作记录
                GenerateOperationReccord(RepairOperationType.EngineerConfirm, repairBill.Id, GetDbTime(), engineerConfirmResult: EngineerConfirmResult.Confirmed);

                //更新设备的使用状态
                RepairUpdateEquipState(repairBill.Id, repairBill.EquipAccountId.Value);
                trans.Complete();
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="equipRepairBill"></param>
        public virtual void Save(EquipRepairBill equipRepairBill)
        {
            RF.Save(equipRepairBill);
        }

        /// <summary>
        /// 应用维修经验库
        /// </summary>
        /// <param name="applyExpDepotInfo"></param>
        public virtual void ApplyExperienceDepot(ApplyExpDepotInfo applyExpDepotInfo)
        {
            var expDepot = RF.GetById<ExperienceDepot>(applyExpDepotInfo.ExpDepotId);
            if (expDepot == null)
            {
                throw new ValidationException("维修经验库不存在[ID:{0}]".L10nFormat(applyExpDepotInfo.ExpDepotId));
            }
            DB.Update<EquipRepairBill>().Where(p => p.Id == applyExpDepotInfo.RepairBillId)
                .Set(p => p.FaultReason, expDepot.FaultReson)
                .Set(p => p.FaultCategoryId, expDepot.EquipLargeFaultId)
                .Set(p => p.FaultPart, expDepot.FaultPart)
                .Set(p => p.FaultDescriptionId, expDepot.FaultDescribeId)
                .Set(p => p.FaultDescriptionRemark, expDepot.FaultDescribeRemark)
                .Set(p => p.DeviceAbnormalId, expDepot.FaultPhenomenonId)
                .Set(p => p.DeviceAbnormalRemark, expDepot.FaultPhenomenonRemark)
                .Set(p => p.RepairMethod, expDepot.RepairWay)
                .Set(p => p.PreventionAdvice, expDepot.PreventionAdvice)
                .Set(p => p.DeviceAbnormalCode, expDepot.FaultCode)
                .Execute();
        }

        /// <summary>
        /// 保存维修单备件更换数据
        /// </summary>
        /// <param name="infos"></param>
        /// <param name="repairBillId"></param>
        public virtual void SaveRepairSparePartChg(List<RepairSaveSubmitSparePartChgInfo> infos, double repairBillId)
        {
            if (infos == null)
            {
                return;
            }
            using (var trans = DB.TransactionScope(EquipRepairEntityDataProvider.ConnectionStringName))
            {
                foreach (var info in infos)
                {
                    if (info.RepairSparePartChgId <= 0 && info.ActionType != 0)
                    {
                        throw new ValidationException("存在非新增的备件申请项目ID为0的数据".L10N());
                    }

                    switch (info.ActionType)
                    {
                        case 0:
                            {
                                //新增
                                var repairSparePartChg = new EquipRepairSparePartChg();
                                repairSparePartChg.SparePartId = info.SparePartId;
                                repairSparePartChg.PartOutDepotDetailId = info.OutDtlId;
                                repairSparePartChg.ChangeQty = info.ChangeQty;
                                repairSparePartChg.Remark = info.Remark;
                                repairSparePartChg.EquipRepairBillId = repairBillId;
                                repairSparePartChg.GenerateId();
                                RF.Save(repairSparePartChg);
                                break;
                            }
                        case 1:
                            {
                                //修改(已更换的不允许修改)
                                DB.Update<EquipRepairSparePartChg>().Where(p => p.Id == info.RepairSparePartChgId && p.State == ChangeSparePartState.New)
                                    .Set(p => p.SparePartId, info.SparePartId)
                                    .Set(p => p.PartOutDepotDetailId, info.OutDtlId)
                                    .Set(p => p.ChangeQty, info.ChangeQty)
                                    .Set(p => p.Remark, info.Remark)
                                    .Execute();
                                break;
                            }
                        case 2:
                            {
                                //删除(已更换的不允许删除)
                                DB.Delete<EquipRepairSparePartChg>().Where(p => p.Id == info.RepairSparePartChgId && p.State == ChangeSparePartState.New).Execute();
                                break;
                            }
                        default:
                            break;
                    }
                }

                trans.Complete();
            }
        }

        /// <summary>
        /// 保存维修单备件申请数据
        /// </summary>
        /// <param name="infos"></param>
        /// <param name="repairBillId"></param>
        public virtual void SaveRepairSparePartApl(List<RepairSaveSubmitSparePartAplInfo> infos, double repairBillId)
        {
            if (infos == null)
            {
                return;
            }
            using (var trans = DB.TransactionScope(EquipRepairEntityDataProvider.ConnectionStringName))
            {
                foreach (var info in infos)
                {
                    if (info.RepairSparePartAplId <= 0 && info.ActionType != 0)
                    {
                        throw new ValidationException("存在非新增的备件申请项目ID为0的数据".L10N());
                    }

                    switch (info.ActionType)
                    {
                        case 0:
                            {
                                //新增
                                var repairSparePartApl = new EquipRepairSparePartApl();
                                repairSparePartApl.SparePartId = info.SparePartId;
                                repairSparePartApl.ApplyQty = info.ApplyQty;
                                repairSparePartApl.OutStockWarehouseId = info.OutStockWarehouseId;
                                repairSparePartApl.ApplyDetailId = info.AppDtlId;
                                repairSparePartApl.Remark = info.Remark;
                                repairSparePartApl.EquipRepairBillId = repairBillId;
                                repairSparePartApl.GenerateId();
                                RF.Save(repairSparePartApl);
                                break;
                            }
                        case 1:
                            {
                                //修改(已申请的不允许修改)
                                DB.Update<EquipRepairSparePartApl>().Where(p => p.Id == repairBillId && !p.IsApply)
                                    .Set(p => p.SparePartId, info.SparePartId)
                                    .Set(p => p.ApplyQty, info.ApplyQty)
                                    .Set(p => p.OutStockWarehouseId, info.OutStockWarehouseId)
                                    .Set(p => p.ApplyDetailId, info.AppDtlId)
                                    .Set(p => p.Remark, info.Remark)
                                    .Execute();
                                break;
                            }
                        case 2:
                            {
                                //删除(已申请的不允许删除)
                                DB.Delete<EquipRepairSparePartApl>().Where(p => p.Id == info.RepairSparePartAplId && !p.IsApply).Execute();
                                break;
                            }
                        default:
                            break;
                    }
                }

                trans.Complete();
            }
        }

        /// <summary>
        /// 维修执行生成备件申请单
        /// </summary>
        /// <param name="repairBillId"></param>
        public virtual void GenerateRepairSparePartApp(double repairBillId)
        {
            var datas = this.GetEquipRepairSparePartApls(repairBillId, false);
            var repairBill = RF.GetById<EquipRepairBill>(repairBillId, new EagerLoadOptions().LoadWithViewProperty());
            if (datas.Count <= 0)
            {
                throw new ValidationException("没有备件申请数据".L10N());
            }
            if (datas.Any(p => p.ApplyQty <= 0))
            {
                throw new ValidationException("存在备件申请项申请数量为0".L10nFormat());
            }
            if (datas.Any(p => p.OutStockWarehouseId == null || p.OutStockWarehouseId == 0))
            {
                throw new ValidationException("存在备件申请项没有选择出库仓库".L10nFormat());
            }
            //构建申请单实体
            SparePartApp sparePartApp = new SparePartApp();
            if (repairBill.RepairType == EquipRepairType.EquipRepair)
            {
                if (repairBill.EquipAccountId == null)
                {
                    throw new ValidationException("设备维修单【{0}】的设备资料为空。".L10nFormat(repairBill.RepairNo));
                }
                if (repairBill.UseDepartmentId == null)
                {
                    throw new ValidationException("设备【{0}】的使用部门为空，不能生成备件申请单。".L10nFormat(repairBill.EquipAccountCode));
                }
                sparePartApp.EquipAccountId = repairBill.EquipAccountId;
                sparePartApp.EquipModelId = repairBill.EquipModelId;
                sparePartApp.GetDepartmentId = repairBill.UseDepartmentId.Value;
            }
            sparePartApp.No = RT.Service.Resolve<SparePartAppController>().GetNo();
            sparePartApp.FromType = FromType.Upkeep;
            sparePartApp.FromNo = datas.FirstOrDefault().EquipRepairBill.RepairNo;
            sparePartApp.DemandDate = DateTime.Now;
            sparePartApp.AuditState = AuditState.StandAudit;
            sparePartApp.QualityStatus = QualityStatus.Good;
            
            using (var trans = DB.TransactionScope(EquipRepairEntityDataProvider.ConnectionStringName))
            {
                RF.Save(sparePartApp);

                //构建申请单明细
                datas.ForEach(p =>
                {
                    var dtl = new ApplyDetail();
                    dtl.SparePartAppId = sparePartApp.Id;
                    dtl.SparePartId = p.SparePartId;
                    dtl.WarehouseId = p.OutStockWarehouseId.Value;
                    dtl.ApplyAmount = p.ApplyQty;
                    dtl.GenerateId();
                    RF.Save(dtl);

                    DB.Update<EquipRepairSparePartApl>().Where(x => x.Id == p.Id)
                        .Set(x => x.ApplyDetailId, dtl.Id)
                        .Set(x => x.IsApply, true).Execute();
                });
                trans.Complete();
            }
        }

        /// <summary>
        /// 执行备件更换逻辑
        /// </summary>
        /// <param name="repairBillId"></param>
        public virtual void ChangeRepairSparePart(double repairBillId)
        {
            var datas = this.GetRepairSparePartChgs(repairBillId, ChangeSparePartState.New);
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

            using (var trans = DB.TransactionScope(EquipRepairEntityDataProvider.ConnectionStringName))
            {
                //回写备件申请单使用数量
                list.ForEach(p =>
                {
                    if (p.PartOutDepotDetail.UseCount + p.ChangeQty > p.PartOutDepotDetail.OutDepotCount)
                    {
                        throw new ValidationException("备件[{0}]更换数量大于备件申请单出库数量".L10nFormat(p.SparePart.SparePartCode));
                    }
                    //回写申请单
                    DB.Update<PartOutDepotDetail>().Where(x => x.Id == p.PartOutDepotDetailId).Set(x => x.UseCount, x => x.UseCount + p.ChangeQty).Execute();
                    //修改备件更换状态
                    DB.Update<EquipRepairSparePartChg>().Where(x => x.Id == p.Id).Set(x => x.State, ChangeSparePartState.Finished).Execute();
                    //修改序列号状态
                    DB.Update<StoreSummaryDetail>().Where(x => x.Id == p.PartOutDepotDetail.SeriaNoRefId).Set(x => x.StoreStatus, OrdNumStoreStatus.Using).Execute();
                    //维修单存在设备ID,维修类型是设备维修，查询设备备件履历
                    if (p.EquipRepairBill.EquipAccountId.HasValue && p.EquipRepairBill.RepairType == EquipRepairType.EquipRepair)
                    {
                        //插入备件履历
                        var record = new SparePartChangedRecord()
                        {
                            EquipAccountId = p.EquipRepairBill.EquipAccountId.Value,
                            Qty = p.ChangeQty,
                            OldSerialNumber = p.OldSequence,
                            BatchNumber = p.PartOutDepotDetail?.BatchNo,
                            SerialNumber = p.PartOutDepotDetail?.SeriaNo,
                            Source = FromType.Upkeep,
                            SourceNo = p.EquipRepairBill.RepairNo,
                            SourceId = p.EquipRepairBillId,
                            SparePartId = p.SparePartId
                        };
                        RF.Save(record);
                    }
                    trans.Complete();
                });
            }
        }

        /// <summary>
        /// 添加经验库
        /// </summary>
        public virtual void AddExperienceDepotByRepair(EquipRepairBill repairBill, bool isAddExperienceDepot)
        {
            ExperienceDepot experienceDepot = !isAddExperienceDepot ? Query<ExperienceDepot>().Where(p => p.RepairNo == repairBill.RepairNo).FirstOrDefault() : new ExperienceDepot();

            experienceDepot.Code = isAddExperienceDepot ? RT.Service.Resolve<ExperienceDepotController>().GetCode() : experienceDepot.Code;
            experienceDepot.RepairType = (ExperienceDepots.Enums.ExpRepairType)repairBill.RepairType;
            experienceDepot.SparePartId = repairBill.SparePartId != 0 ? repairBill.SparePartId : null;
            experienceDepot.EquipAccountId = repairBill.EquipAccountId != 0 ? repairBill.EquipAccountId : null;
            experienceDepot.EquipModelId = repairBill.EquipModelId != 0 ? repairBill.EquipModelId : null;
            experienceDepot.EquipTypeId = repairBill.EquipTypeId != 0 ? repairBill.EquipTypeId : null;
            experienceDepot.RepairNo = repairBill.RepairNo;
            experienceDepot.FaultReson = repairBill.FaultReason;
            experienceDepot.EquipLargeFaultId = repairBill.FaultCategoryId ?? 0;
            experienceDepot.FaultPart = repairBill.FaultPart;

            experienceDepot.FaultPhenomenonId = repairBill.DeviceAbnormalId;
            experienceDepot.FaultPhenomenonRemark = repairBill.DeviceAbnormalRemark;
            experienceDepot.FaultDescribeId = repairBill.FaultDescriptionId;
            experienceDepot.FaultDescribeRemark = repairBill.FaultDescriptionRemark;

            experienceDepot.RepairWay = repairBill.RepairMethod;
            experienceDepot.PreventionAdvice = repairBill.PreventionAdvice;
            experienceDepot.FaultCode = repairBill.DeviceAbnormalCode;

            experienceDepot.PersistenceStatus = isAddExperienceDepot ? PersistenceStatus.New : PersistenceStatus.Modified;

            RF.Save(experienceDepot);
        }

        /// <summary>
        /// 添加经验库
        /// </summary>
        public virtual void AddExperienceDepotByRepair(ExperienceDepotViewModel experienceDepotViewModel)
        {
            ExperienceDepot experienceDepot = new ExperienceDepot();

            var code = RT.Service.Resolve<ExperienceDepotController>().GetCode();

            experienceDepot.Code = code;

            //判断是不是添加了经验库了
            var exp = DB.Query<ExperienceDepot>().Where(p => p.RepairNo.Contains(experienceDepotViewModel.RepairNo)).FirstOrDefault();
            if (exp != null)
            {
                throw new ValidationException("已经添加经验了".L10N());
            }
            if (experienceDepotViewModel.DeviceAbnormalId == null || experienceDepotViewModel.DeviceAbnormalId == 0)
            {
                throw new ValidationException("故障现象未填写".L10N());
            }
            if (experienceDepotViewModel.FaultDescriptionId == null)
            {
                throw new ValidationException("故障描述未填写".L10N());
            }
            if (experienceDepotViewModel.FaultCategoryId == null)
            {
                throw new ValidationException("故障类别未填写".L10N());
            }
            //添加所有值
            experienceDepot.RepairNo = experienceDepotViewModel.RepairNo;
            experienceDepot.RepairType = (ExperienceDepots.Enums.ExpRepairType)experienceDepotViewModel.RepairType;
            experienceDepot.EquipAccountId = experienceDepotViewModel.EquipAccountId;
            experienceDepot.FaultPhenomenonId = (double)experienceDepotViewModel.DeviceAbnormalId;
            experienceDepot.FaultPhenomenonRemark = experienceDepotViewModel.DeviceAbnormalRemark;
            experienceDepot.FaultDescribeId = (double)experienceDepotViewModel.FaultDescriptionId;
            experienceDepot.FaultDescribeRemark = experienceDepotViewModel.FaultDescriptionRemark;
            experienceDepot.FaultReson = experienceDepotViewModel.FaultReason;
            experienceDepot.EquipLargeFaultId = (double)experienceDepotViewModel.FaultCategoryId;
            experienceDepot.FaultPart = experienceDepotViewModel.FaultPart;
            experienceDepot.RepairWay = experienceDepotViewModel.RepairMethod;
            experienceDepot.PreventionAdvice = experienceDepotViewModel.PreventionAdvice;
            experienceDepot.FaultCode = experienceDepotViewModel.DeviceAbnormalCode;
            RF.Save(experienceDepot);

        }

        /// <summary>
        /// 保存维修报告字段
        /// </summary>
        /// <param name="info"></param>
        public virtual void SaveRepairReport(RepairSaveSubmitInfo info)
        {

            DB.Update<EquipRepairBill>().Where(p => p.Id == info.RepairBillId)
                .Set(p => p.OutsourcedMaintenanceReport, info.OutsourcedMaintenanceReport)
                .Set(p => p.FaultReason, info.FaultReasonCode)
                .Set(p => p.FaultLevel, (FaultLevel?)info.FaultLevel)
                .Set(p => p.RepairCosts, info.RepairCosts)
                .Set(p => p.FaultCategoryId, info.FaultCategoryId)
                .Set(p => p.FaultPart, info.FaultPart)
                .Set(p => p.RepairCategory, (RepairCategory?)info.RepairCategory)
                .Set(p => p.RepairDowntime, info.RepairDowntime)
                .Set(p => p.RepairLevel, (RepairLevel?)info.RepairLevel)
                .Set(p => p.RepairMethod, info.RepairMethod)
                .Set(p => p.PreventionAdvice, info.PreventionAdvice)
                .Set(p => p.FaultDescriptionId, info.FaultDescriptionId)
                .Set(p => p.FaultDescriptionRemark, info.FaultDescriptionRemark)
                .Set(p => p.DeviceAbnormalCode, info.DeviceAbnormalCode)
                .Execute();
        }

        /// <summary>
        /// 更换维修状态
        /// </summary>
        /// <param name="equipRepairBillId"></param>
        /// <param name="state"></param>
        public virtual void ChangeRepairState(double equipRepairBillId, EquipRepairState state)
        {
            DB.Update<EquipRepairBill>().Where(p => p.Id == equipRepairBillId).Set(p => p.RepairState, state).Execute();
        }

        /// <summary>
        /// 维修生成备件申请单
        /// </summary>
        /// <param name="uiEquipRepairSpareParts">备件更换清单</param>
        public virtual void UIGenerateSparePartApp(List<EquipRepairSparePartApl> uiEquipRepairSpareParts)
        {
            if (uiEquipRepairSpareParts == null)
            {
                return;
            }
            using (var trans = DB.TransactionScope(EquipRepairEntityDataProvider.ConnectionStringName))
            {
                //先执行保存更改数据
                uiEquipRepairSpareParts.ForEach(p => RF.Save(p));

                var equipRepairBillId = uiEquipRepairSpareParts.FirstOrDefault().EquipRepairBillId;
                GenerateRepairSparePartApp(equipRepairBillId);

                trans.Complete();
            }
        }

        /// <summary>
        /// UI执行备件更换
        /// </summary>
        /// <param name="uiEquipRepairSpareParts"></param>
        public virtual void UIChangeEquipRepairSparePart(List<EquipRepairSparePartChg> uiEquipRepairSpareParts)
        {
            if (uiEquipRepairSpareParts == null)
            {
                return;
            }
            using (var trans = DB.TransactionScope(EquipRepairEntityDataProvider.ConnectionStringName))
            {
                //先执行保存更改数据
                uiEquipRepairSpareParts.ForEach(p => RF.Save(p));
                var equipRepairBill = uiEquipRepairSpareParts.FirstOrDefault().EquipRepairBill;
                ChangeEquipRepairSparePart(equipRepairBill.Id);
                trans.Complete();
            }
        }

        /// <summary>
        /// 执行备件更换逻辑
        /// </summary>
        /// <param name="equipRepairBillId"></param>
        public virtual void ChangeEquipRepairSparePart(double equipRepairBillId)
        {
            try
            {
                bool isCreateHandoverBill = RT.Service.Resolve<OutDepotController>().IsCreateHandoverBill();

                var datas = GetEquipRepairSpareParts(equipRepairBillId, EMS.Enums.ChangeSparePartState.New);
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

                var handoverDtlList = isCreateHandoverBill ?
                    list.Select(p => p.PartOutDepotDetail.OutDepotHandoverDetailId).Where(p => p != null).SplitContains(tempIds =>
                   {
                       return Query<OutDepotHandoverDetail>().Where(p => tempIds.Contains(p.Id)).ToList();
                   }) : new EntityList<OutDepotHandoverDetail>();

                list.ForEach(p =>
                {
                    if (p.PartOutDepotDetail.UseCount + p.ChangeQty > p.PartOutDepotDetail.OutDepotCount)
                    {
                        throw new ValidationException("备件[{0}]更换数量大于备件申请单出库数量".L10nFormat(p.SparePart.SparePartCode));
                    }

                    if (isCreateHandoverBill)
                    {
                        var handoverDtl = handoverDtlList.FirstOrDefault(dtl => dtl.Id == p.PartOutDepotDetail.OutDepotHandoverDetailId);

                        if (handoverDtl != null)
                        {
                            if (handoverDtl.HandOverStatus == HandOverStatus.Pending)
                            {
                                throw new ValidationException("备件[{0}]尚未交接，无法更换".L10nFormat(p.SparePart.SparePartCode));
                            }
                        }
                        else
                        {
                            throw new ValidationException("备件[{0}]尚未交接，无法更换".L10nFormat(p.SparePart.SparePartCode));
                        }
                    }
                });
                using (var trans = DB.TransactionScope(EquipRepairEntityDataProvider.ConnectionStringName))
                {
                    //回写备件申请单使用数量
                    list.ForEach(p =>
                    {
                        //回写申请单
                        DB.Update<PartOutDepotDetail>().Where(x => x.Id == p.PartOutDepotDetailId).Set(x => x.UseCount, x => x.UseCount + p.ChangeQty).Execute();
                        //修改备件更换状态
                        DB.Update<EquipRepairSparePartChg>().Where(x => x.Id == p.Id).Set(x => x.State, Enums.ChangeSparePartState.Finished).Execute();
                        //修改序列号状态
                        DB.Update<StoreSummaryDetail>().Where(x => x.Id == p.PartOutDepotDetail.SeriaNoRefId).Set(x => x.StoreStatus, OrdNumStoreStatus.Using).Execute();
                        //插入备件履历
                        var record = new SparePartChangedRecord()
                        {
                            EquipAccountId = p.EquipRepairBill.EquipAccountId == null ? 0 : Convert.ToDouble(p.EquipRepairBill.EquipAccountId),
                            Qty = p.ChangeQty,
                            OldSerialNumber = p.OldSequence,
                            SerialNumber = p.PartOutDepotDetail?.SeriaNo,
                            BatchNumber = p.PartOutDepotDetail?.BatchNo,
                            Source = FromType.Upkeep,
                            SourceNo = p.EquipRepairBill.RepairNo,
                            SourceId = p.EquipRepairBillId,
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
                DB.Update<EquipRepairSparePartChg>().Where(p => p.EquipRepairBillId == equipRepairBillId && p.State == ChangeSparePartState.New).Set(p => p.PartOutDepotDetailId, value).Execute();
                throw new ValidationException(ex.GetBaseException().Message);
            }

        }

        /// <summary>
        /// 查询对应设备类型的异常信息
        /// </summary>
        /// <param name="repairBill"></param>
        /// <param name="abnormalType"></param>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        private EntityList<DeviceAbnormal> QueryTypeRepairBill(EquipRepairBill repairBill, AbnormalType abnormalType, string keyword, PagingInfo pagingInfo)
        {
            var queryer = DB.Query<DeviceAbnormal>().Where(p => p.AbnormalType == abnormalType);
            if (repairBill.RepairType == EquipRepairType.EquipRepair)
            {
                var equipModel = Query<EquipModel>().Exists<EquipAccount>((x, y) => y.Where(p => p.EquipModelId == x.Id && p.Id == repairBill.EquipAccountId)).Select(p => new
                {
                    Id = p.EquipTypeId,
                }).ToList<BaseDataInfo>().FirstOrDefault();
                if (equipModel != null)
                {
                    queryer.Where(p => p.EquipTypeId == equipModel.Id);
                }
            }
            if (keyword.IsNotEmpty())
            {
                queryer.Where(p => p.Code.Contains(keyword) || p.Description.Contains(keyword));
            }
            return queryer.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询对应设备类型为空的异常信息
        /// </summary>
        /// <param name="abnormalType"></param>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        private EntityList<DeviceAbnormal> QueryNoTypeRepairBill(AbnormalType abnormalType, string keyword, PagingInfo pagingInfo)
        {
            var queryer = DB.Query<DeviceAbnormal>().Where(p => p.AbnormalType == abnormalType).Where(p => p.EquipTypeId == null);
            if (keyword.IsNotEmpty())
            {
                queryer.Where(p => p.Code.Contains(keyword) || p.Description.Contains(keyword));
            }
            return queryer.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据设备维修单,获取异常现象列表
        /// </summary>
        /// <param name="repairBill"></param>
        /// <param name="abnormalType"></param>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<DeviceAbnormal> GetDeviceAbnormalsByRepairBill(EquipRepairBill repairBill, AbnormalType abnormalType, string keyword, PagingInfo pagingInfo)
        {
            if (repairBill == null)
            {
                return new EntityList<DeviceAbnormal>();
            }
            if (repairBill.RepairType == EquipRepairType.EquipRepair && repairBill.EquipAccountId == null)
            {
                return new EntityList<DeviceAbnormal>();
            }
            if (repairBill.RepairType == EquipRepairType.SparePartRepair && repairBill.SparePartId == null)
            {
                return new EntityList<DeviceAbnormal>();
            }
            var result = QueryTypeRepairBill(repairBill, abnormalType, keyword, pagingInfo);
            if (result.Count <= 0)
            {
                result = QueryNoTypeRepairBill(abnormalType, keyword, pagingInfo);
            }
            return result;
        }


        /// <summary>
        /// 获取维修规程列表
        /// </summary>
        /// <param name="equipRepairId">维修单ID</param>
        /// <param name="orderInfoList">排序信息</param>
        /// <param name="pagingInfo">关键字</param>
        /// <returns></returns>
        public virtual EntityList<EquipRepairBillProject> GetEquipRepairBillProject(double equipRepairId, List<OrderInfo> orderInfoList, PagingInfo pagingInfo)
        {
            var q = Query<EquipRepairBillProject>();
            q.Where(p => p.EquipRepairBillId == equipRepairId);
            var elo = new EagerLoadOptions();
            elo.LoadWith(EquipRepairBillProject.ProjectDetailProperty);
            elo.LoadWithViewProperty();

            var list = q.OrderBy(orderInfoList).ToList(pagingInfo, elo);
            return list;
        }


        #endregion

        /// <summary>
        /// 关闭未完结的设备维修单
        /// </summary>
        /// <param name="equipAccountIds">设备台账Id</param>
        /// <returns></returns>
        public virtual void CloseEquipRepairBillByEquipAccountIds(IList<double> equipAccountIds)
        {
            var equipRepairBills = equipAccountIds.SplitContains(tempIds =>
            {
                return Query<EquipRepairBill>().Where(p => p.RepairType == EquipRepairType.EquipRepair && tempIds.Contains((double)p.EquipAccountId)
                && (p.RepairState == EquipRepairState.ApplyRepair || p.RepairState == EquipRepairState.WaitRepair ||
                    p.RepairState == EquipRepairState.Repairing || p.RepairState == EquipRepairState.WaitConfirm || p.RepairState == EquipRepairState.Suspending)).ToList();
            });

            equipRepairBills.ForEach(bill =>
            {
                var closeViewModel = new CompelCloseViewModel();
                closeViewModel.Id = bill.Id.ToString();
                closeViewModel.CloseReason = "报废".L10N();
                CompelClose(closeViewModel);
            });
        }

        /// <summary>
        /// 根据设备ID，获取有设备维修权限的员工列表
        /// </summary>
        /// <param name="equipEmployeeCriteria"></param>
        /// <returns></returns>
        public virtual EntityList<EquipEmployee> GetDevicePurRepairs(EquipEmployeeCriteria equipEmployeeCriteria)
        {
            var dpCtl = RT.Service.Resolve<DevicePurController>();
            if (equipEmployeeCriteria.EquipAccountId.HasValue && equipEmployeeCriteria.EquipAccountId != 0)
            {
                //var account = RF.GetById<EquipAccount>(equipEmployeeCriteria.EquipAccountId);
                //if (account == null)
                //{
                //    return new EntityList<EquipEmployee>();
                //}
                //var deptId = account.UseDepartmentId;
                var q = Query<Employee>().As("x");
                //q.Exists<EmployeeEnterprise>((x, y) => y.Join<Employee>("n", (m, n) => m.EmployeeId == n.Id).Where(p => p.EnterpriseId == account.FactoryId && p.EmployeeId == x.Id));
                q.Exists<DevicePur>((x, y) => y.LeftJoin<UserInUserGroup>((a, b) => a.UserGroupId == b.UserGroupId)
                     .Where<UserInUserGroup>((a, b) => (a.UserId == x.UserId || b.UserId == x.UserId) && a.EquipMaintain));
                if (equipEmployeeCriteria.Code.IsNotEmpty())
                {
                    q.Where(x => x.Code.Contains(equipEmployeeCriteria.Code));
                }

                if (equipEmployeeCriteria.Name.IsNotEmpty())
                {
                    q.Where(x => x.Name.Contains(equipEmployeeCriteria.Name));
                }

                var list = q.ToList(equipEmployeeCriteria.PagingInfo);
                q.ToList(equipEmployeeCriteria.PagingInfo);

                EntityList<EquipEmployee> equipEmployees = new EntityList<EquipEmployee>();
                equipEmployees.SetTotalCount(list.TotalCount);

                foreach (var employee in list)
                {
                    equipEmployees.Add(new EquipEmployee()
                    {
                        Id = employee.Id,
                        Code = employee.Code,
                        Name = employee.Name,
                    });
                }

                return equipEmployees;
            }
            else
            {
                //查询出设备与人员权限维护的用户关联且开始设备维修权限的所有员工
                var q = Query<Employee>();
                q.Exists<DevicePur>((x, y) => y.Where(p => p.UserId == x.UserId && p.EquipMaintain));

                if (equipEmployeeCriteria.Code.IsNotEmpty())
                {
                    q.Where(p => p.Code.Contains(equipEmployeeCriteria.Code));
                }

                if (equipEmployeeCriteria.Name.IsNotEmpty())
                {
                    q.Where(p => p.Name.Contains(equipEmployeeCriteria.Name));
                }
                var list = q.ToList(equipEmployeeCriteria.PagingInfo);
                q.ToList(equipEmployeeCriteria.PagingInfo);

                EntityList<EquipEmployee> equipEmployees = new EntityList<EquipEmployee>();
                equipEmployees.SetTotalCount(list.TotalCount);

                foreach (var employee in list)
                {
                    equipEmployees.Add(new EquipEmployee()
                    {
                        Id = employee.Id,
                        Code = employee.Code,
                        Name = employee.Name,
                    });
                }
                return equipEmployees;
            }
        }

        /// <summary>
        /// 计算备件费用与维修工时(工程确认,完成维修,强制关单)
        /// </summary>
        /// <param name="bill">维修单</param>
        public virtual void CalculateSparePartsCostAndRepairHours(EquipRepairBill bill)
        {
            EquipRepairBill billDate = GetById<EquipRepairBill>(bill.Id);
            decimal repairHours = 0;
            decimal sparePartsCost = 0;
            //工时
            foreach (var workinghours in billDate.EquipRepairWorkingHoursList)
            {
                repairHours += (decimal)GetWorkingHours(workinghours);
            }


            //只计算备件更换状态为完成且备件出库单不为空的备件更换数据
            List<EquipRepairSparePartChg> equipRepairSparePartChg = billDate.EquipRepairSparePartChgList.Where(p => p.State == ChangeSparePartState.Finished && p.PartOutDepotDetailId.HasValue).ToList();

            //所有的备件出库明细单
            var PartOutDepotDetailIds = equipRepairSparePartChg.Select(p => (double)p.PartOutDepotDetailId).ToList();

            List<PartOutDepotDetail> PartOutDepotDetailList = RT.Service.Resolve<OutDepotController>().GetPartOutDepotDetailList(PartOutDepotDetailIds).ToList();

            //备件费用
            foreach (var partchg in equipRepairSparePartChg)
            {
                var detail = PartOutDepotDetailList.FirstOrDefault(p => p.Id == partchg.PartOutDepotDetailId);
                if (detail != null)
                {
                    sparePartsCost += (decimal)(partchg.ChangeQty * detail.UnitPrice);
                }
            }

            //工时
            bill.RepairHours = repairHours;
            //备件费用
            bill.SparePartsCost = sparePartsCost;
        }

        /// <summary>
        /// 维修工时
        /// </summary>
        public  virtual  double GetWorkingHours(EquipRepairWorkingHours me)
        {
            double workingHours = 0;
            if (me != null && me.BeginTime.HasValue && me.EndTime.HasValue)
            {
                workingHours = Math.Round((me.EndTime.Value - me.BeginTime.Value).TotalHours, 2);
            }
            return workingHours;
        }

        /// <summary>
        /// 保存委外维修报告
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public virtual void SaveOutsourcedMaintenanceReport(double id, string fileName)
        {
            var equipRepairBill = GetById<EquipRepairBill>(id);
            equipRepairBill.OutsourcedMaintenanceReport = fileName;
            RF.Save(equipRepairBill);
        }

        /// <summary>
        /// 维修开始
        /// </summary>
        /// <param name="repairStartViewModel"></param>
        /// <exception cref="ValidationException"></exception>
        public virtual void StartRepair(RepairStartViewModel repairStartViewModel)
        {
            var dateTimeOfNow = RF.Find<EquipRepairBill>().GetDbTime();

            var equipRepairBill = RF.GetById<EquipRepairBill>(double.Parse(repairStartViewModel.Id), new EagerLoadOptions()
               .LoadWith(EquipRepairBill.EquipRepairWorkingHoursListProperty));

            EntityList<Employee> equipEmployees = null;
            //获取拥有维修权限员工列表
            equipEmployees = GetEmployeeListByIds(equipRepairBill.RepairMasterId
                + (equipRepairBill.RepairEmployeeIds == "" ? "" : ",") + equipRepairBill.RepairEmployeeIds);

            //是否拥有维修的权限
            if (!equipEmployees.Select(p => p.Id).Contains(RT.IdentityId))
            {
                throw new ValidationException("当前用户不是维修责任人或者维修人员,请检查！".L10N());
            }

            //维修开始操作属性
            equipRepairBill.RepairBeginDate = dateTimeOfNow;
            equipRepairBill.RepairState = EquipRepairState.Repairing;

            //维修操作记录
            EntityList<EquipRepairOperationRec> equipRepairOperationRecList = new EntityList<EquipRepairOperationRec>();
            equipRepairOperationRecList.Add(new EquipRepairOperationRec()
            {
                EquipRepairBillId = equipRepairBill.Id,
                OperationerId = RT.IdentityId,
                OperationType = RepairOperationType.Begin,
                OperationDate = dateTimeOfNow
            });

            if (repairStartViewModel.IsStopMachineRepair)
            {
                equipRepairBill.RepairDowntime = true;
                equipRepairBill.ProduceState = ProduceState.StopWork;
            }

            using (var tran = DB.TransactionScope(EquipRepairEntityDataProvider.ConnectionStringName))
            {
                //设备状态更新成停机
                if (repairStartViewModel.IsStopMachineRepair
                    && equipRepairBill.EquipAccountId != null && equipRepairBill.EquipAccountId != 0)
                {
                    DB.Update<EquipAccount>()
                        .Set(x => x.State, AccountState.Downtime)
                        .Where(x => x.Id == equipRepairBill.EquipAccountId)
                        .Execute();
                }

                RF.Save(equipRepairBill);

                //维修操作记录
                RF.Save(equipRepairOperationRecList);

                //更新维修工时记录
                DB.Update<EquipRepairWorkingHours>()
                    .Set(x => x.BeginTime, dateTimeOfNow)
                    .Where(x => x.EquipRepairBillId == equipRepairBill.Id && x.IsRepairEmployee)
                    .Execute();

                tran.Complete();
            }
        }

        /// <summary>
        /// 获取设备的维修工时和成本
        /// </summary>
        /// <param name="equipAccountIds">设备台账Id</param>
        /// <returns>设备的维修工时和成本</returns>
        public virtual List<WorkHourAndCostInfo> GetEquipRepairWorkHourAndCost(IList<double> equipAccountIds)
        {
            var nowDate = RF.Find<EquipRepairBill>().GetDbTime();

            var repairBillList = equipAccountIds.SplitContains(tempIds =>
            {
                return Query<EquipRepairBill>().Where(p => p.RepairType == EquipRepairType.EquipRepair && equipAccountIds.Contains((double)p.EquipAccountId)).ToList();
            });

            repairBillList.ForEach(p => p.RepairCosts = p.RepairCosts ?? 0);

            var infoList = repairBillList.Where(p => p.CreateDate >= nowDate.AddYears(-1))
                          .GroupBy(p => p.EquipAccountId)
                          .Select(bills => new WorkHourAndCostInfo
                          {
                              EquipAccountId = (double)bills.Key,
                              RepairHours = bills.Sum(p => p.RepairHours),
                              SparePartCost = bills.Sum(p => p.SparePartsCost),
                              OutRepairCost = bills.Where(p => p.RepairWay == EquipRepairWay.OuterRepair).Sum(p => (decimal)p.RepairCosts),
                              TotalRepairHours = repairBillList.Sum(p => p.RepairHours),
                              TotalSparePartCost = repairBillList.Sum(p => p.SparePartsCost) + bills.Sum(p => (decimal)p.RepairCosts)
                          }).ToList();

            return infoList;
        }


        /// <summary>
        /// 根据报警明细生成维修工单
        /// </summary>
        /// <param name="equipAlarmRecordIds"></param>
        /// <returns></returns>
        public virtual void AddEquipAlarmRecordRepair(List<double> equipAlarmRecordIds)
        {
            EntityList<EquipRepairBill> equipRepairBillList;
            //获取所有的设备报警明细
            var equipAlarmRecordList = GetEquipAlarmRecordList(equipAlarmRecordIds);

            GenerateRepairOrder(equipAlarmRecordList, out equipRepairBillList);

            using (var tran = DB.TransactionScope(EquipRepairEntityDataProvider.ConnectionStringName))
            {
                if (equipRepairBillList.Any())
                {
                    //保存维修工单
                    RF.Save(equipRepairBillList);
                    //保存报警明细单
                    RF.Save(equipAlarmRecordList);
                }
                tran.Complete();
            }
        }

        /// <summary>
        /// 根据获取报警明细Ids
        /// </summary>
        /// <param name="equipAlarmRecordIds"></param>
        /// <returns></returns>
        public virtual EntityList<EquipAlarmRecord> GetEquipAlarmRecordList(List<double> equipAlarmRecordIds)
        {
            return equipAlarmRecordIds.SplitContains((ids) =>
            {
                return Query<EquipAlarmRecord>().Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }


        /// <summary>
        /// 创建维修单 根据报警明细
        /// </summary>
        /// <param name="equipAlarmRecordList"></param>
        /// <param name="equipRepairBillList"></param>
        public virtual void GenerateRepairOrder(EntityList<EquipAlarmRecord> equipAlarmRecordList, out EntityList<EquipRepairBill> equipRepairBillList)
        {
            equipRepairBillList = new EntityList<EquipRepairBill>();
            var now = this.GetDbTime();
            foreach (var record in equipAlarmRecordList)
            {
                EquipRepairBill repair = new EquipRepairBill();
                // 创建维修单

                repair.GenerateId();
                repair.RepairNo = this.GenerateRepairNo();
                repair.RepairState = EquipRepairState.ApplyRepair;
                repair.SourceNo = record.Code;
                repair.SourceType = RepairSourceType.AlarmDetail;
                repair.RepairType = EquipRepairType.EquipRepair;
                repair.EquipAccountId = record.EquipAccountId;
                repair.ProduceState = ProduceState.Produce;
                switch (record.AlarmLevel)
                {
                    case AlarmLevel.Info:
                    case AlarmLevel.Minor:
                    case AlarmLevel.Medium:
                        repair.UrgentDegree = UrgentDegree.Common;
                        break;
                    case AlarmLevel.Major:
                        repair.UrgentDegree = UrgentDegree.High;
                        break;
                    case AlarmLevel.Serious:
                        repair.UrgentDegree = UrgentDegree.Urgent;
                        break;
                    default:
                        repair.UrgentDegree = UrgentDegree.Common;
                        break;
                }
                repair.DeviceAbnormalRemark = record.AlarmContent;
                repair.ApplyRepairEmployeeId = RT.IdentityId;
                repair.ApplyRepairDate = now;
                //反写报警明细的维修单号
                record.EquipRepairBillId = repair.Id;
                equipRepairBillList.Add(repair);
            }
        }

        /// <summary>
        /// 判断当前点检单是否有维修单
        /// </summary>
        /// <param name="equipAccountId"></param>
        /// <param name="sourceNo"></param>
        /// <param name="sourceType">来源类型</param>
        /// <returns></returns>
        public virtual bool CheckPlanWithRepairBill(double equipAccountId, string sourceNo, int sourceType)
        {
            return Query<EquipRepairBill>().Where(p => p.EquipAccountId == equipAccountId && p.SourceNo == sourceNo && p.SourceType == (RepairSourceType)sourceType).Count() > 0;
        }
    }
}
