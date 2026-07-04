using SIE.Api;
using SIE.Common.Attachments;
using SIE.Common.Configs;
using SIE.Core.ApiModels;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.ApiModel;
using SIE.EMS.Checks.ApiModels;
using SIE.EMS.Checks.Configs;
using SIE.EMS.Checks.Confirmations;
using SIE.EMS.Checks.Plans;
using SIE.EMS.Checks.Plans.ViewModels;
using SIE.EMS.Checks.Projects;
using SIE.EMS.Common.Utils;
using SIE.EMS.Enums;
using SIE.EMS.Equipments.ApiModels;
using SIE.EMS.EquipRepairs;
using SIE.EMS.Maintains.ApiModels;
using SIE.EMS.Maintains.Configs;
using SIE.EMS.Maintains.Confirmations;
using SIE.EMS.Maintains.Plans;
using SIE.EMS.Maintains.Plans.ViewModels;
using SIE.EMS.Maintains.Projects;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Applys.Controllers;
using SIE.EMS.SpareParts.Applys.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SIE.EMS.Maintains.Controller
{
    /// <summary>
    /// 设备保养API接口控制器
    /// </summary>
    [ApiName("MaintainApiController")]
    public partial class MaintainController
    {
        #region PCB设备保养-新PDA后台API

        /// <summary>
        /// 获取当前登录用户所属部门，当天所有未执行的保养单信息
        /// </summary>
        /// <param name="pageSize">每页数据量</param>
        /// <param name="pageNumber">页码</param>
        /// <param name="keyword">关键字</param>
        /// <param name="departmentIds">部门ID集合</param>
        /// <param name="state">状态(0:未执行;2:超期;4:执行中)</param>
        /// <returns></returns>
        [ApiService("获取当前登录用户所属部门，当天所有未执行的保养单信息")]
        [return: ApiReturn("保养计划列表 MaintainPlanData")]
        public virtual MaintainPlanData GetNotPerformedMaintainPlanInfos(
            [ApiParameter("每页数据量")] int pageSize, [ApiParameter("页码")] int pageNumber, [ApiParameter("关键字")] string keyword,
            [ApiParameter("部门ID集合")] List<double> departmentIds, [ApiParameter("状态")] List<int?> state)
        {
            if (pageSize <= 0)
                throw new ValidationException("[每页数据量]必须大于0".L10N());
            if (pageNumber <= 0)
                throw new ValidationException("[页码]必须大于0".L10N());
            var exeState = state.Select(p => (MaintExeState?)p).ToList();
            //构建分页实体
            var pageInfo = new PagingInfo() { PageSize = pageSize, PageNumber = pageNumber, IsNeedCount = true };
            //根据台账获取保养明细,获取当前登录用户所属部门当天所有未保养的检验单信息
            var planlist = RT.Service.Resolve<MaintainController>()
                .GetNotPerformedMaintainPlans(keyword, departmentIds, pageInfo, exeState);

            //构建返回数据结构
            var data = new MaintainPlanData();
            data.TotalCount = planlist.TotalCount;

            foreach (var e in planlist)
            {
                var maintainPlan = e as MaintainPlan;

                data.MaintainPlanInfos.Add(new MaintainPlanInfos()
                {
                    Id = maintainPlan.Id,                                      //保养计划id
                    PlanBeginDate = maintainPlan.PlanBeginDate.ToString(),     //计划执行时间
                    PlanEndDate = maintainPlan.PlanEndDate.ToString(),
                    ActBeginDate = maintainPlan.ActBeginDate.HasValue ? maintainPlan.ActBeginDate.Value.ToString("yyyy/MM/dd HH:mm") : "",
                    ActEndDate = maintainPlan.ActEndDate.HasValue ? maintainPlan.ActEndDate.Value.ToString("yyyy/MM/dd HH:mm") : "",
                    No = maintainPlan.MaintainNo,                              //保养单号
                    Qty = maintainPlan.ProjectList.Count(),                    //项目数量
                    EquipId = maintainPlan.EquipAccountId,                     //设备ID 
                    EquipCode = maintainPlan.EquipAccountCode,                //设备编码
                    EquipName = maintainPlan.EquipAccountName,                //设备名称
                    DepartmentId = maintainPlan.DepartmentId,                  //部门ID
                    DepartmentCode = maintainPlan.DepartmentCode,             //部门编码
                    DepartmentName = maintainPlan.DepartmentName,              //部门名称
                    State = (int)maintainPlan.ExeState,
                    StateName = maintainPlan.ExeState.ToLabel().L10N(),
                    Shop = maintainPlan.WorkShopName,
                    Line = maintainPlan.ResourceName,
                    MaintainTime = maintainPlan.MaintainTime,
                    EquipTypeId = maintainPlan.EquipTypeId,
                    EquipTypeCode = maintainPlan.EquipTypeCode,
                    EquipTypeName = maintainPlan.EquipTypeName,
                    EquipModelId = maintainPlan.EquipModelId,
                    EquipModelCode = maintainPlan.EquipModelCode,
                    EquipModelName = maintainPlan.EquipModelName,
                    MaintainSummary = maintainPlan.MaintainSummary,
                    MaintainType = (int)maintainPlan.MaintainType,
                    MaintainTypeDisplay = maintainPlan.MaintainType.ToLabel().L10N(),
                    WhetherBegin = maintainPlan.WhetherBegin,
                });
            }

            return data;
        }

        /// <summary>
        /// 判断当前保养单是否有维修单
        /// </summary>
        /// <param name="equipAccountId"></param>
        /// <param name="sourceNo"></param>
        /// <returns></returns>
        [ApiService("判断当前保养单是否有维修单")]
        public virtual bool CheckPlanWithRepairBill([ApiParameter("保养设备id")] double equipAccountId, [ApiParameter("保养单号")] string sourceNo)
        {
            return Query<EquipRepairBill>().Where(p => p.EquipAccountId == equipAccountId && p.SourceNo == sourceNo).Count() > 0;
        }

        /// <summary>
        /// 获取上次保养小结
        /// </summary>
        /// <param name="accountId">设备台账ID</param>
        /// <param name="departmentId">departmentId</param>
        /// <returns></returns>
        [ApiService("获取上次保养小结")]
        [return: ApiReturn("获取上次保养小结 string")]
        public virtual string GetLastSummary([ApiParameter("设备台账ID")] double accountId, [ApiParameter("部门ID")] double? departmentId)
        {
            return RT.Service.Resolve<MaintainController>().GetLastMaintainSummary(accountId, departmentId);
        }

        /// <summary>
        /// 一键保养
        /// 将最大值与最小值为空的保养项目，默认保养结果为【合格】
        /// </summary>
        /// <param name="planId">保养计划ID</param>
        /// <returns></returns>
        [ApiService("将最大值与最小值为空的保养项目，默认保养结果为【合格】")]
        public virtual void QuickMaintain([ApiParameter("保养计划ID")] double planId)
        {
            DB.Update<MaintainProject>()
                .Set(p => p.ExeState, MaintExeState.Performed)
                .Set(p => p.MaintainResult, CheckMaintainResult.OK)
                .Where(p => !p.MaxValue.HasValue && !p.MinValue.HasValue && p.MaintainPlanId == planId)
                .Execute();
        }

        /// <summary>
        /// 生成设备保养计划保养项目
        /// </summary>
        /// <param name="maintainPlanId">计划单ID</param>
        /// <returns></returns>
        [ApiService("生成设备保养计划保养项目")]
        public virtual void GenerateMaintainPlanProject([ApiParameter("计划单ID")] double maintainPlanId)
        {
            var maintainPlan = Query<MaintainPlan>()
                   .Where(p => p.Id == maintainPlanId && p.ExeState == MaintExeState.NotPerformed)
                   .FirstOrDefault();

            ValidationMaintainDate(maintainPlan);

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //行锁数据，防止并发
                DB.Update<MaintainPlan>().Where(p => p.Id == maintainPlanId).Set(p => p.UpdateBy, RT.IdentityId).Execute();

                var ctl = RT.Service.Resolve<MaintainController>();
                ctl.GeneratePlanProject(maintainPlan);
                trans.Complete();
            }
        }

        /// <summary>
        /// 获取设备保养计划保养项目
        /// </summary>
        /// <param name="maintainPlanId">保养计划单ID</param>
        /// <returns></returns>
        [ApiService("获取设备保养计划保养项目")]
        [return: ApiReturn("设备保养计划保养项目列表 List<MaintainPlanProjectInfo>")]
        public virtual List<MaintainPlanProjectInfo> GetMaintainPlanProjects([ApiParameter("计划单ID")] double maintainPlanId)
        {
            var maintainPlan = GetMaintainPlanById(maintainPlanId);

            ValidationMaintainDate(maintainPlan);

            var projects = RT.Service.Resolve<MaintainController>().GetMaintainProjects(maintainPlanId);

            //构建返回数据
            List<MaintainPlanProjectInfo> infos = new List<MaintainPlanProjectInfo>();
            projects.ForEach(p =>
            {
                int? reslut = null;
                infos.Add(new MaintainPlanProjectInfo()
                {
                    ProjectId = p.Id,
                    ProjectName = p.ProjectName,
                    Part = p.Part,
                    Consumable = p.ProjectConsumable,
                    Method = p.Method,
                    MaxValue = p.MaxValue,
                    MinValue = p.MinValue,
                    ActualValue = p.ActualValue,
                    DefectDesc = p.Defect,
                    Result = p.MaintainResult == null ? reslut : (int)(p.MaintainResult.Value)
                });
            });

            return infos;
        }

        /// <summary>
        /// 创建备件申请单
        /// </summary>
        /// <param name="mainPlanId">保养单Id</param>
        /// <param name="sparePartIds">备件Ids</param>
        [ApiService("创建备件申请单")]
        public virtual void CreateApiSelSpareApplyList([ApiParameter("保养单Id")] double mainPlanId, [ApiParameter("备件Ids")] List<double> sparePartIds)
        {
            var dbApplyList = Query<MaintainPlanSparePartApl>().Where(p => p.MaintainPlanId == mainPlanId && sparePartIds.Contains(p.SparePartId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            foreach (var spareId in sparePartIds)
            {
                var record = dbApplyList.FirstOrDefault(p => p.SparePartId == spareId);
                if (record != null)
                {
                    throw new ValidationException("已存在备件[{0}]的申请记录".L10nFormat(record.SparePartNameView));
                }
            }

            EntityList<MaintainPlanSparePartApl> maintainPlanSparePartApls = new EntityList<MaintainPlanSparePartApl>();
            foreach (var sparePartId in sparePartIds)
            {
                MaintainPlanSparePartApl apply = new MaintainPlanSparePartApl
                {
                    MaintainPlanId = mainPlanId,
                    SparePartId = sparePartId,
                    ApplyQty = 1,
                };
                maintainPlanSparePartApls.Add(apply);
            }
            RF.BatchInsert(maintainPlanSparePartApls);
        }

        /// <summary>
        /// 删除备件申请单
        /// </summary>
        /// <param name="Id"></param>
        [ApiService("删除备件申请单")]
        public virtual void DelSelSpareApply([ApiParameter("申请单Id")] double Id)
        {
            DB.Delete<MaintainPlanSparePartApl>().Where(p => p.Id == Id).Execute();
        }

        /// <summary>
        /// 创建备件更换单
        /// </summary>
        /// <param name="mainPlanId">保养单Id</param>
        /// <param name="sparePartIds">备件Ids</param>
        [ApiService("创建备件更换单")]
        public virtual void CreateApiSelSpareChangeList([ApiParameter("保养单Id")] double mainPlanId, [ApiParameter("备件Ids")] List<double> sparePartIds)
        {
            // 盘点是否已存在备件更换
            var dbChangeList = Query<MaintainPlanSparePart>().Where(p => p.MaintainPlanId == mainPlanId && sparePartIds.Contains(p.SparePartId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            foreach (var spareId in sparePartIds)
            {
                var record = dbChangeList.FirstOrDefault(p => p.SparePartId == spareId);
                if (record != null)
                {
                    throw new ValidationException("已存在备件[{0}]的更换记录".L10nFormat(record.SparePartNameView));
                }
            }

            EntityList<MaintainPlanSparePart> maintainPlanSpareParts = new EntityList<MaintainPlanSparePart>();
            foreach (var sparePartId in sparePartIds)
            {
                MaintainPlanSparePart change = new MaintainPlanSparePart
                {
                    MaintainPlanId = mainPlanId,
                    SparePartId = sparePartId,
                    ChangeQty = 1,
                };
                maintainPlanSpareParts.Add(change);
            }
            RF.BatchInsert(maintainPlanSpareParts);
        }

        /// <summary>
        /// 删除备件更换单
        /// </summary>
        /// <param name="Id"></param>
        [ApiService("删除备件更换单")]
        public virtual void DelSelSpareChange([ApiParameter("更换单Id")] double Id)
        {
            DB.Delete<MaintainPlanSparePart>().Where(p => p.Id == Id).Execute();
        }

        /// <summary>
        /// 获取设备保养计划备件更换项目
        /// </summary>
        /// <param name="maintainPlanId">保养计划单ID</param>
        /// <returns></returns>
        [ApiService("获取设备保养计划备件更换项目")]
        [return: ApiReturn("设备保养计划备件更换项目列表 List<MaintainSparePartInfo>")]
        public virtual List<MaintainSparePartInfo> GetMaintainSpareParts([ApiParameter("计划单ID")] double maintainPlanId)
        {
            var maintainSpareParts = RT.Service.Resolve<MaintainController>().GetMaintainPlanSpareParts(maintainPlanId);

            //构建返回数据
            List<MaintainSparePartInfo> infos = new List<MaintainSparePartInfo>();
            maintainSpareParts.ForEach(p =>
            {
                int? state = null;
                infos.Add(new MaintainSparePartInfo()
                {
                    SparePartId = p.SparePartId,
                    SparePartCode = p.SparePart.SparePartCode,
                    SparePartName = p.SparePart.SparePartName,
                    OutDtlId = p.PartOutDepotDetailId,
                    ChangeQty = p.ChangeQty,
                    Remark = p.Remark,
                    MaintainSparePartId = p.Id,
                    UseQty = p.PartOutDepotDetail?.UseCount ?? 0,
                    OutDepotNo = p.PartOutDepotDetail?.OutDepot.No,
                    OutDepotState = p.PartOutDepotDetail?.OutDepot == null ? state : (int)p.PartOutDepotDetail.OutDepot.OutDepotState,
                    OutDepotStateName = p.PartOutDepotDetail?.OutDepot == null ? string.Empty : p.PartOutDepotDetail.OutDepot.OutDepotState.ToLabel().L10N(),
                    State = (int)p.State,
                    StateName = p.State.ToLabel().L10N(),
                    SeriaNo = p.PartOutDepotDetail?.SeriaNo,
                    BatchNo = p.PartOutDepotDetail?.BatchNoRef?.BatchNumber,
                    RemainingQty = p.PartOutDepotDetail == null ? 0 : p.PartOutDepotDetail.OutDepotCount - p.PartOutDepotDetail.UseCount
                });
            });

            return infos;
        }

        /// <summary>
        /// 获取设备保养计划备件申请项目
        /// </summary>
        /// <param name="maintainPlanId">保养计划单ID</param>
        /// <returns></returns>
        [ApiService("获取设备保养计划备件申请项目")]
        [return: ApiReturn("设备保养计划备件申请项目列表 List<MaintainSparePartAplInfo>")]
        public virtual List<MaintainSparePartAplInfo> GetMaintainSparePartApls([ApiParameter("计划单ID")] double maintainPlanId)
        {
            var maintainSparePartApls = RT.Service.Resolve<MaintainController>().GetMaintainPlanSparePartApls(maintainPlanId);

            //构建返回数据
            List<MaintainSparePartAplInfo> infos = new List<MaintainSparePartAplInfo>();
            maintainSparePartApls.ForEach(p =>
            {
                int? state = null;
                infos.Add(new MaintainSparePartAplInfo()
                {
                    SparePartId = p.SparePartId,
                    SparePartCode = p.SparePart.SparePartCode,
                    SparePartName = p.SparePart.SparePartName,
                    ApplyQty = p.ApplyQty,
                    OutStockWarehouseId = p.OutStockWarehouseId,
                    OutStockWarehouseCode = p.OutStockWarehouse?.Code,
                    OutStockWarehouseName = p.OutStockWarehouse?.Name,
                    AppDtlId = p.ApplyDetailId,
                    Remark = p.Remark,
                    MaintainSparePartId = p.Id,
                    UseQty = p.ApplyDetail == null ? 0 : p.ApplyDetail.UseAmount,
                    SparePartApplyNo = p.ApplyDetail?.SparePartApp.No,
                    SparePartApplyState = p.ApplyDetail?.SparePartApp == null ? state : (int)p.ApplyDetail.SparePartApp.AuditState,
                    ApplyStateName = p.ApplyDetail?.SparePartApp == null ? string.Empty : p.ApplyDetail.SparePartApp.AuditState.ToLabel().L10N(),
                    StoreQty = (int)p.StoreQty,
                    IsApply = p.IsApply
                });
            });

            return infos;
        }

        /// <summary>
        /// 保存或提交检验单
        /// </summary>
        /// <param name="info"></param>
        [ApiService("保存或提交检验单")]
        public virtual void SaveSubmitMaintainPlans([ApiParameter("保养保存提交信息")] MaintainSaveSubmitInfo info)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                if (info.MaintainPlanId <= 0) throw new ValidationException("保养单ID不正确".L10N());

                //防并发行锁
                DB.Update<MaintainPlan>().Where(p => p.Id == info.MaintainPlanId).Set(p => p.UpdateBy, RT.IdentityId).Execute();
                var plan = RF.GetById<MaintainPlan>(info.MaintainPlanId);

                if (plan == null)
                {
                    throw new ValidationException("保养单不存在，[ID:{0}]".L10nFormat(info.MaintainPlanId));
                }

                if (plan.ExeState != MaintExeState.NotPerformed && plan.ExeState != MaintExeState.Performing)
                {
                    throw new ValidationException("保养单[{0}]是[{1}]状态，不允许保存"
                        .L10nFormat(plan.MaintainNo, plan.ExeState.ToLabel()));
                }

                //保存逻辑
                RT.Service.Resolve<MaintainController>().SaveMaintainPlan(info);

                //提交逻辑
                if (info.IsSubmit)
                {
                    var maintainPlan = Query<MaintainPlan>().Where(p => p.Id == info.MaintainPlanId).FirstOrDefault();
                    maintainPlan.IsAbnormalInfoPush = info.IsAbnormalInfoPush;
                    RT.Service.Resolve<MaintainController>().SubmitMaintainPlan(maintainPlan);
                }

                //提交事务
                trans.Complete();
            }
        }

        /// <summary>
        /// 执行保养备件更换
        /// </summary>
        /// <param name="info"></param>
        [ApiService("执行保养备件更换")]
        public virtual void ChangeMaintainSpareParts([ApiParameter("计划单参数实体")] MaintainSaveSubmitInfo info)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                this.SaveMaintainChangeInfo(info);
                RT.Service.Resolve<MaintainController>().ChangeMaintainPlanSparePart(info.MaintainPlanId);
                trans.Complete();
            }
        }

        /// <summary>
        /// 保养申请备件申请单
        /// </summary>
        /// <param name="info"></param>
        [ApiService("保养申请备件申请单")]
        public virtual void GenerateSparePartApp([ApiParameter("计划单参数实体")] MaintainSaveSubmitInfo info)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                this.SaveMaintainApplyInfo(info);
                RT.Service.Resolve<SparePartAppController>().GenerateMaintainSparePartApp(info.MaintainPlanId, FromType.Maintain);
                trans.Complete();
            }
        }

        /// <summary>
        /// 查看图片
        /// </summary>
        /// <param name="maintainPlanId">计划单ID</param>
        /// <returns></returns>
        [ApiService("查看图片")]
        public virtual EmsAttachmentInfoList GetMaintainPlanAttachmentPhotos([ApiParameter("计划单ID")] double maintainPlanId)
        {
            
            EmsAttachmentInfoList res = new EmsAttachmentInfoList();
            res.AttachmentList = new List<EmsAttachmentInfo>();
            var exts = new List<string> { ".png", ".jpg", ".jpeg", ".bmp", ".gif", ".webp", ".psd", ".svg", ".tiff", ".jfif" };
            var q = Query<MaintainPlanAttachment>().Where(p => p.OwnerId == maintainPlanId && exts.Contains(p.FileExtesion));

            var attachList = q.ToList();
            if (attachList?.Count() > 0)
            {
                attachList?.ForEach(attach =>
                {

                    EmsAttachmentInfo tempAttach = new EmsAttachmentInfo()
                    {
                        Id = attach.Id,
                        FileExtension = attach.FileExtesion,
                        FileName = attach.FileName,
                        FilePath = attach.FilePath,
                        FileSize = attach.FileSize,
                        Content = FileUrlHelper.GetAttachmentBase64StringData(attach.FilePath, attach.FileName)
                    };
                    res.AttachmentList.Add(tempAttach);
                });
            }

            return res;
        }

        /// <summary>
        /// 获取设备保养计划工时登记列表
        /// </summary>
        /// <param name="maintainPlanId"></param>
        [ApiService("获取设备保养计划工时登记列表")]
        [return: ApiReturn("设备保养计划工时登记列表 List<MaintainWorkHourInfos>")]
        public virtual List<MaintainWorkHourInfo> GetMaintainWorkHours([ApiParameter("计划单ID")] double maintainPlanId)
        {
            var workHoursRegisters = RT.Service.Resolve<MaintainController>().GetMaintainWorkHoursRegisters(maintainPlanId);

            //构建返回数据
            List<MaintainWorkHourInfo> infos = new List<MaintainWorkHourInfo>();
            workHoursRegisters.ForEach(p =>
            {
                infos.Add(new MaintainWorkHourInfo()
                {
                    MaintainWorkHourId = p.Id,
                    EmployeeId = p.EmployeeId,
                    EmployeeCode = p.Employee?.Code,
                    EmployeeName = p.Employee?.Name,
                    BeginDay = p.BeginDay.ToString(),
                    EndDay = p.EndDay.ToString(),
                    WorkHours = p.WorkHours
                });
            });

            return infos;
        }

        /// <summary>
        /// 获取当前登录用户所属部门所有需要保养确认的检验单信息
        /// </summary>
        /// <param name="pageSize">每页数据量</param>
        /// <param name="pageNumber">页码</param>
        /// <param name="equipCode">设备编码</param>
        /// <param name="departmentId">部门ID</param>
        /// <returns></returns>
        [ApiService("获取当前登录用户所属部门对应设备需要保养确认的检验单信息")]
        [return: ApiReturn("保养计划列表 List<MaintainPlanInfo>")]
        public virtual MaintainPlanData GetNotConfirmedMaintainPlanInfos(
            [ApiParameter("每页数据量")] int pageSize,
            [ApiParameter("页码")] int pageNumber,
            [ApiParameter("设备编码")] string equipCode,
            [ApiParameter("确认部门ID")] double? departmentId)
        {
            if (pageSize <= 0) { throw new ValidationException("[每页数据量]必须大于0".L10N()); }
            if (pageNumber <= 0) { throw new ValidationException("[页码]必须大于0".L10N()); }

            //构建分页实体
            var pageInfo = new PagingInfo() { PageSize = pageSize, PageNumber = pageNumber, IsNeedCount = true };

            //根据台账获取保养明细,获取当前登录用户所属部门所有未保养的检验单信息
            var planlist = RT.Service.Resolve<MaintainController>().GetNotConfirmedMaintainPlans(equipCode, departmentId, pageInfo);

            //构建返回数据结构
            var data = new MaintainPlanData();
            data.TotalCount = planlist.Count();
            //planlist.ForEach(e =>
            //{
            //    data.MaintainPlanInfos.Add(e);
            //});

            data.MaintainPlanInfos = planlist.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }

        /// <summary>
        /// 获取某个保养计划的保养确认单信息
        /// </summary>
        /// <param name="MaintainPlanId">保养计划ID</param>
        /// <param name="confirmDeptId">部门ID</param>
        /// <returns></returns>
        [ApiService("获取某个保养计划指定确认部门的保养确认单信息")]
        [return: ApiReturn("保养计划 MaintainPLan")]
        public virtual EntityList<MaintainConfirmation> GetMaintainPlanConfirmations(
            [ApiParameter("保养计划ID")] double MaintainPlanId,
            [ApiParameter("确认部门ID")] double confirmDeptId)
        {
            #region 验证计划单
            var maintainPlans = Query<MaintainPlan>().Where(p => p.Id == MaintainPlanId && p.ExeState == MaintExeState.NotConfirm).ToList();
            if (maintainPlans.Count == 0)
            {
                throw new ValidationException((MaintainPlanId.ToString() + "不是有效的计划ID或者执行状态不是待确认状态").L10N());
            }
            #endregion

            #region 验证确认部门ID
            Enterprise department = null;
            department = RT.Service.Resolve<EnterpriseController>().GetEnterpriseById(confirmDeptId);
            if (department == null || department.Id == 0)
            {
                throw new ValidationException((confirmDeptId.ToString() + "不是有效的部门ID").L10N());
            }
            #endregion

            var maintainPlanConfirmations = RT.Service.Resolve<MaintainController>().GetMaintainPlanConfirmations(MaintainPlanId, department.Id, null, null);

            return maintainPlanConfirmations;
        }

        /// <summary>
        /// 获取是否评分配置项
        /// </summary>
        /// <returns></returns>
        [ApiService("获取是否评分配置项")]
        public virtual bool GetIsNeedScore()
        {
            var needScoreConfig = ConfigService.GetConfig<MaintainConfirmDepartConfigValue>(new MaintainConfirmDepartConfig(), typeof(MaintainPlanViewModel));
            bool isNeedScore = false;
            if (needScoreConfig != null)
            {
                isNeedScore = needScoreConfig.IsMarkScore;
            }
            return isNeedScore;
        }

        /// <summary>
        /// 提交保养确认单
        /// </summary>
        /// <param name="info">保养确认提交信息</param>
        /// <param name="planInfo">保养确认信息</param>
        [ApiService("提交保养确认单")]
        public virtual void SubmitMaintainConfirmation([ApiParameter("保养确认要提交的信息")] MaintainConfirmationSubmitInfo[] info, [ApiParameter("保养确认信息")]  MaintainPlanInfo planInfo)
        {
            if (planInfo.ConfirmResult == null)
            {
                throw new ValidationException("点检确认结果不能为空！".L10N());
            }
            else
            {
                if (planInfo.ConfirmResult == 2 && planInfo.ConfirmNote.IsNullOrEmpty())
                {
                    throw new ValidationException("点检确认结果为不合格时要进行备注！".L10N());
                }
            }


            // 是否启用评分
            bool isNeedScore = IsNeedMarkScore();

            // 提交验证
            MaintainConfirmSubmitVali(info, isNeedScore);

            // 数据库时间
            var dbDate = RF.Find<CheckPlan>().GetDbTime();

            // 评分项
            var maintainConfirmations = new EntityList<MaintainConfirmation>();
            if (isNeedScore)
            {
                CreateMaintainMarks(info, planInfo, maintainConfirmations, dbDate);
            }

            // 更新保养确认结果、备注及确认项
            var plan = RF.GetById<MaintainPlan>(planInfo.Id);
            EntityList<MaintainPlanConfirmItem> confirmItems = new EntityList<MaintainPlanConfirmItem>();
            UpdatePlanResult(plan, planInfo, dbDate);

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //防并发行锁
                DB.Update<MaintainPlan>().Where(p => p.Id == planInfo.Id).Set(p => p.UpdateBy, RT.IdentityId).Set(p => p.UpdateDate, RF.Find<MaintainPlan>().GetDbTime()).Execute();

                // 更新评分项
                if (maintainConfirmations.Any())
                {
                    DB.Delete<MaintainConfirmation>().Where(p => p.MaintainPlanId == planInfo.Id && p.ConfirmDeptId ==  planInfo.ConfirmDeptId).Execute();
                    RF.Save(maintainConfirmations);
                }

                // 更新执行状态为已完成，若启用评分则为已评分。
                UpdatePlanState(isNeedScore, plan, planInfo, confirmItems);
                RF.Save(plan);
                RF.Save(confirmItems);

                //更新备件更换记录标记
                if (plan.ExeState == MaintExeState.Scored)
                    RT.Service.Resolve<SparePartController>().UpdateSparePartChangedRecordFlag(FromType.Maintain, plan.Id);

                //提交事务
                trans.Complete();
            }
        }


        private static void UploadFile(MaintainConfirmationSubmitInfo confirmSubmitInfo,  MaintainConfirmation maintainConfirmationNew)
        {
            try
            {
                FileUrlHelper.GenerateMaintainConfirmationBase64StringContent(maintainConfirmationNew, confirmSubmitInfo.Content, confirmSubmitInfo.FileName);
            }
            catch (FormatException)
            {
                throw new ValidationException("图片转换的字符串不是有效的Base64字符串".L10N());
            }

            var path = "";

            if (!maintainConfirmationNew.FilePath.IsNotEmpty())
            {
                path = "MaintainConfirmation/" + Guid.NewGuid().ToString("N");
                maintainConfirmationNew.FilePath = path + "/" + maintainConfirmationNew.FileName;
            }
            else
            {
                path = maintainConfirmationNew.FilePath.Replace("/" + maintainConfirmationNew.FileName, "");
            }

            RT.Service.Resolve<AttachmentController>().FileStorage(maintainConfirmationNew.FileName, maintainConfirmationNew.Content, path);
        }

        /// <summary>
        /// 保养开始
        /// </summary>
        /// <param name="id">保养单Id</param>
        [ApiService("保养开始")]
        public virtual void PdaBegingMaintain([ApiParameter("保养单Id")] double id)
        {
            this.BegingMaintain(id);
        }
        #endregion

        #region 设备保养 EIS-OLD
        /// <summary>
        /// 获取设备保养计划（查询30天的未点检未超期）
        /// </summary>
        /// <param name="equipCode">设备编码</param>
        /// <returns>设备保养计划列表</returns>
        [ApiService("获取设备保养计划（查询30天的未点检未超期）")]
        [return: ApiReturn("设备保养计划列表")]
        public virtual EquipMaintainPlanInfo GetEquipMaintainPlanInfo([ApiParameter("设备编码")] string equipCode)
        {
            var code = equipCode.Trim();
            if (code.IsNullOrEmpty())
                throw new ValidationException("设备编码不能为空".L10N());
            var account = RT.Service.Resolve<Equipments.EquipController>().GetEquipAccount(p => p.Code == code);
            if (account == null)
                throw new ValidationException("未找到设备[{0}]".L10nFormat(equipCode));
            var equipMaintainPlanData = new EquipMaintainPlanInfo()
            {
                EquipInfo = new EquipInfo()
                {
                    Id = account.Id,
                    Code = account.Code,
                    Process = account.Process?.Name,
                    Name = account.Name,
                    WorkShop = account.WorkShop?.Name,
                    EquipType = account.EquipModel?.EquipType.TypeName
                }
            };
            //根据台账获取保养明细,查询30天的未点检未超期
            var plans = GetMaintainPlans(account.Id);
            var maintainPlanList = new List<MaintainPlanInfo>();
            plans.ForEach(e =>
            {
                maintainPlanList.Add(new MaintainPlanInfo()
                {
                    Id = e.Id,
                    No = e.MaintainNo,
                    PlanBeginDate = e.PlanBeginDate.ToString(),
                    PlanEndDate = e.PlanEndDate.ToString(),
                    Qty = e.ProjectList.Count(),//项目数量
                });
            });

            equipMaintainPlanData.MaintainPlans.AddRange(maintainPlanList);

            return equipMaintainPlanData;
        }

        /// <summary>
        /// 获取保养项目列表 
        /// </summary>
        /// <param name="planId">保养计划ID</param>
        /// <returns>设备保养项目列表</returns>
        [ApiService("获取保养项目列表")]
        [return: ApiReturn("设备保养项目列表")]
        public virtual List<MaintainProjectInfo> GetMaintainProjectInfos([ApiParameter("保养计划ID")] double planId)
        {
            var now = RF.Find<MaintainPlan>().GetDbTime();
            var maintainPlan = GetById<MaintainPlan>(planId);
            if (maintainPlan == null)
                throw new ValidationException("保养计划不能为空".L10N());
            if (maintainPlan.ActBeginDate == null)
                maintainPlan.ActBeginDate = now;//保养开始时间 
            var detail = GetMaintainProjects(p => p.MaintainPlanId == planId);
            if (detail.Any(p => p.EquipAccount == null))
                throw new ValidationException("找不到对应的设备台账，请确认后再操作".L10N());
            if (detail.Any(p => p.EquipMaintainProject == null))
                throw new ValidationException("找不到对应的点检保养项目，请确认后再操作".L10N());
            RF.Save(maintainPlan);

            var orderData = new EntityList<MaintainProject>();
            orderData.AddRange(detail.Where(p => p.EquipMaintainProject.MinValue != null).ToList());
            orderData.AddRange(detail.Where(p => p.EquipMaintainProject.MinValue == null).ToList());

            Expression<Func<MaintainProject, bool>> exp = p => p.MaintainPlan.EquipAccountId == detail[0].MaintainPlan.EquipAccountId && p.ExeState == MaintExeState.Performed;
            var lastMaintain = GetMaintainProject(exp, p => p.UpdateDate);
            var maintainProjectList = new List<MaintainProjectInfo>();
            orderData.ForEach(e =>
            {
                var data = new MaintainProjectInfo()
                {
                    Id = e.Id,
                    MaintainPlanId = e.MaintainPlanId,
                    MaintainResult = (int?)e.MaintainResult,
                    MaxValue = e.EquipMaintainProject?.MaxValue,
                    MinValue = e.EquipMaintainProject?.MinValue,
                    Unit = e.EquipMaintainProject?.Unit,
                    Remark = e.Remark,
                    ProjectName = e.EquipMaintainProject?.ProjectName,
                    AccountName = e.AccountName,
                    Value = e.ActualValue,
                    Cycle = e.EquipMaintainProject?.CycleType.ToLabel(),
                    LastTime = lastMaintain == null ? (int?)null : (DateTime.Now - lastMaintain.UpdateDate).Days,//距离上次保养时间
                };

                //有图片则赋值
                if (e.ProjectPhoto?.Photo != null)
                {
                    data.Photo = System.Text.Encoding.Default.GetString(e.ProjectPhoto.Photo);
                }
                maintainProjectList.Add(data);
            });
            return maintainProjectList;
        }

        /// <summary>
        /// 保存保养项目
        /// </summary>
        /// <param name="datas">设备保养项目列表</param> 
        [ApiService("保存保养项目")]
        public virtual void SaveMaintainProject([ApiParameter("设备保养计划列表")] List<MaintainProjectInfo> datas)
        {
            if (datas == null || datas.Count <= 0)
                throw new ValidationException("保养计划不能为空".L10N());
            var ids = datas.Select(p => p.Id);
            var projects = GetMaintainProjects(p => ids.Contains(p.Id));
            var equipId = projects[0].MaintainPlan.EquipAccountId;
            var maintain = projects[0].MaintainPlan;
            var now = RF.Find<MaintainPlan>().GetDbTime();
            if (maintain.PlanBeginDate > now)
            {
                throw new ValidationException("该保养单还没有到开始保养日期".L10N());
            }

            //有多条保养记录时,请按时间顺序来执行保养
            var isExist = IsExistMaintainPlan(equipId, maintain);
            if (isExist)
            {
                throw new ValidationException("有多条保养记录时,请按时间顺序来执行保养".L10N());
            }
            var modifyProjects = new EntityList<MaintainProject>();
            datas.ForEach(e =>
            {
                var maintainProject = projects.FirstOrDefault(p => p.Id == e.Id);

                maintainProject.ActualValue = e.Value;
                maintainProject.Remark = e.Remark;
                maintainProject.ExeState = MaintExeState.Performed;
                maintainProject.ProjectPhoto = null;
                //有上传图片
                if (e.Photo.IsNotEmpty())
                {
                    try
                    {
                        byte[] imageBytes = System.Text.Encoding.Default.GetBytes(e.Photo);
                        var projectPhoto = new ProjectPhoto() { Photo = imageBytes };
                        RF.Save(projectPhoto);
                        maintainProject.ProjectPhoto = projectPhoto;//图片
                    }
                    catch
                    {
                        throw new ValidationException("图片字节转换失败".L10N());
                    }
                }
                modifyProjects.Add(maintainProject);
            });
            RF.Save(modifyProjects);
        }

        /// <summary>
        /// 提交保养
        /// </summary>
        /// <param name="datas">设备保养项目列表</param> 
        [ApiService("提交保养")]
        public virtual void SubmitMaintain([ApiParameter("设备保养计划列表")] List<MaintainProjectInfo> datas)
        {
            SaveMaintainProject(datas);
            var maintainPlan = GetMaintainPlan(p => p.Id == datas[0].MaintainPlanId);
            if (maintainPlan == null)
                throw new ValidationException("未找到保养计划，请检查".L10N());
            if (maintainPlan.ProjectList.Any(p => p.ExeState != MaintExeState.Performed))
            {
                throw new ValidationException("请保养完所有项目再提交！".L10N());
            }
            maintainPlan.ExecuteById = RT.IdentityId;
            maintainPlan.ExeState = MaintExeState.Performed;
            maintainPlan.ActEndDate = RF.Find<MaintainPlan>().GetDbTime();
            bool isExistNg = datas.Any(p => p.MaintainResult == 0);
            maintainPlan.IsExsitNgProject = isExistNg ? YesNo.Yes : YesNo.No;

            RF.Save(maintainPlan);

            if (maintainPlan.IsExsitNgProject == YesNo.Yes)
            {
                UpdateAccountState(maintainPlan.EquipAccount);
            }
        }

        /// <summary>
        /// 更新设置台账状态
        /// </summary>
        /// <param name="equipAccount"></param>
        private void UpdateAccountState(EquipAccount equipAccount)
        {
            equipAccount.QualityState = QualityState.Disable;
            equipAccount.State = AccountState.Downtime;
            equipAccount.UseState = AccountUseState.InIdle;
            RF.Save(equipAccount);
        }
        #endregion


        /// <summary>
        /// 获取仓库
        /// </summary>
        /// <param name="spartId"></param>
        /// <returns></returns>
        [ApiService("获取仓库")]
        public virtual List<SparepartWareInfo> SpartApplyGetWarehouses(double spartId)
        {
            var wareList = RT.Service.Resolve<WarehouseController>().GetEnableWarehouses(pagingInfo: null, string.Empty);
            List<BaseDataInfo> wareInfos = new List<BaseDataInfo>();
            List<SparepartWareInfo> sparepartWareInfos = new List<SparepartWareInfo>();
            foreach (var ware in wareList)
            {
                BaseDataInfo info = new BaseDataInfo
                {
                    Id = ware.Id,
                    Code = ware.Code,
                    Name = ware.Name,
                };
                wareInfos.Add(info);
            }
            var wareIds = wareInfos.Select(p => p.Id).ToList();
            var sparePart = RF.GetById<SparePart>(spartId);
            if (sparePart.ControlMethod == SpareParts.Enums.ControlMethod.ItemCode)
            {
                var queryStoreSummaryLocation = Query<StoreSummaryLocation>()
                        .Where(p => p.StoreSummary.SparePartId == spartId && wareIds.Contains(p.WarehouseId)).ToList();
                foreach (var ware in wareInfos)
                {
                    var storeQty = queryStoreSummaryLocation.Where(p => p.WarehouseId == ware.Id).Sum(p => p.GoodNumber);
                    SparepartWareInfo sparepartWareInfo = new SparepartWareInfo
                    {
                        SparePartId = spartId,
                        WarehouseId = ware.Id,
                        WareCode = ware.Code,
                        WareName = ware.Name,
                        StoreQty = storeQty,
                    };
                    sparepartWareInfos.Add(sparepartWareInfo);
                }
                return sparepartWareInfos;
            }
            else if (sparePart.ControlMethod == SpareParts.Enums.ControlMethod.Batch)
            {
                var queryStoreSummaryLocation = Query<StoreSummaryLot>()
                        .Where(p => p.StoreSummary.SparePartId == spartId && wareIds.Contains(p.WarehouseId)).ToList();
                foreach (var ware in wareInfos)
                {
                    var storeQty = queryStoreSummaryLocation.Where(p => p.WarehouseId == ware.Id).Sum(p => p.GoodNumber);
                    SparepartWareInfo sparepartWareInfo = new SparepartWareInfo
                    {
                        SparePartId = spartId,
                        WarehouseId = ware.Id,
                        WareCode = ware.Code,
                        WareName = ware.Name,
                        StoreQty = storeQty,
                    };
                    sparepartWareInfos.Add(sparepartWareInfo);
                }
                return sparepartWareInfos;
            }
            else if (sparePart.ControlMethod == SpareParts.Enums.ControlMethod.Sn)
            {
                var queryStoreSummaryLocation = Query<StoreSummaryDetail>()
                        .Where(p => p.StoreSummary.SparePartId == spartId && wareIds.Contains(p.WarehouseId)).ToList();
                foreach (var ware in wareInfos)
                {
                    var storeQty = queryStoreSummaryLocation.Where(p => p.WarehouseId == ware.Id).Sum(p => p.GoodNumber);
                    SparepartWareInfo sparepartWareInfo = new SparepartWareInfo
                    {
                        SparePartId = spartId,
                        WarehouseId = ware.Id,
                        WareCode = ware.Code,
                        WareName = ware.Name,
                        StoreQty = storeQty,
                    };
                    sparepartWareInfos.Add(sparepartWareInfo);
                }
                return sparepartWareInfos;
            }
            else
            {
                return new List<SparepartWareInfo>();
            }
        }
    }
}
