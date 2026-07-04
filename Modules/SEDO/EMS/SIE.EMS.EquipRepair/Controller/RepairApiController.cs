using SIE.Api;
using SIE.Common.Catalogs;
using SIE.Core.ApiModels;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Common.Utils;
using SIE.EMS.DevicePurs;
using SIE.EMS.Devices.Abnormals;
using SIE.EMS.Equipments;
using SIE.EMS.Equipments.ApiModels;
using SIE.EMS.EquipRepair.ApiModels;
using SIE.EMS.EquipRepair.ApiModels.ResultInfo;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.EMS.EquipRepair.EquipRepairs.Enums;
using SIE.EMS.EquipRepairs.Enums;
using SIE.EMS.Faults;
using SIE.EMS.Projects;
using SIE.EMS.SpareParts;
using SIE.EMS.Tpms;
using SIE.Equipments.EquipModels;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SIE.EMS.EquipRepair.Controller
{
    /// <summary>
    /// 设备维修API接口控制器
    /// </summary>
    public class RepairApiController : DomainController
    {
        #region PCB设备维修-新PDA后台API

        /// <summary>
        /// 获取报修设备
        /// </summary>
        /// <param name="code">设备/备件编码</param>
        /// <returns></returns>
        [ApiService("获取报修设备")]
        [return: ApiReturn("设备信息 MaintainPlanData")]
        public virtual EquipInfo GetRepairEquipAccountInfo([ApiParameter("设备/备件编码")] string code)
        {
            EquipInfo info = new EquipInfo();

            //获取设备实体
            var equip = RT.Service.Resolve<EquipController>().GetEquipAccountNoAuth(code);
            if (equip != null)
            {
                info.Id = equip.Id;
                info.Code = equip.Code;
                info.Name = equip.Name;
                info.EquipTypeId = equip.EquipModel?.EquipTypeId;
                info.EquipType = equip.EquipModel?.EquipType?.TypeName;
                info.WorkShop = equip.WorkShop?.Name;
                info.Process = equip.Process?.Name;
                info.Line = equip.Resource?.Name;
                info.Department = equip.UseDepartment?.Name;
                //查询出来的是设备
                info.EquipRepairType = 0;
            }
            else
            {
                //获取备件实体
                var sparePart = RT.Service.Resolve<SparePartController>().GetSparePart(code);
                if (sparePart != null)
                {
                    info.Id = sparePart.Id;
                    info.Code = sparePart.SparePartCode;
                    info.Name = sparePart.SparePartName;
                    //查询出来的是备件
                    info.EquipRepairType = 1;
                }
            }
            if (info.Code.IsNullOrEmpty())
            {
                throw new ValidationException("设备/备件[{0}]不存在".L10nFormat(code));
            }
            //校验设备是否允许报修
            //RT.Service.Resolve<RepairController>().ValidateEquipAccountApplyRepair(equip.Id);
            return info;
        }

        /// <summary>
        /// 查询异常现象列表
        /// </summary>
        /// <param name="queryInfo">分页查询参数</param>
        /// <returns></returns>
        [ApiService("查询异常现象列表")]
        [return: ApiReturn("异常现象列表 AbnormalsPagingResultInfo")]
        public virtual AbnormalsPagingResultInfo GetDeviceAbnormalInfos([ApiParameter("分页查询参数")] AbnormalsQueryInfo queryInfo)
        {
            //构建分页实体
            var pageInfo = this.GeneratePagingInfo(queryInfo);

            //查询故障信息
            var abnormals = RT.Service.Resolve<DeviceAbnormalController>()
                .GetDeviceAbnormals(pageInfo, queryInfo.Keyword, (AbnormalType)queryInfo.AbnormalType, queryInfo.EquipTypeId, queryInfo.RepairType);

            //构建返回实体
            var info = new AbnormalsPagingResultInfo();
            info.PageNumber = queryInfo.PageNumber;
            info.PageSize = queryInfo.PageSize;
            info.TotalCount = abnormals.TotalCount;
            abnormals.ForEach(p =>
            {
                info.AbnormalsResultInfos.Add(new AbnormalsResultInfo()
                {
                    Id = p.Id,
                    Code = p.Code,
                    EquipTypeId = p.EquipTypeId,
                    EquipTypeName = p.EquipType?.TypeName,
                    AbnormalType = (int)p.AbnormalType,
                    AbnormalTypeName = p.AbnormalType.ToLabel(),
                    Description = p.Description
                });
            });

            return info;
        }

        /// <summary>
        /// 设备维修报修提交
        /// </summary>
        /// <param name="applyRepairInfo">报修参数</param>
        /// <returns></returns>
        [ApiService("设备维修报修提交")]
        [return: ApiReturn("维修单号 string")]
        public virtual string ApplyRepair([ApiParameter("报修参数")] ApplyRepairInfo applyRepairInfo)
        {
            var hepler = new FileUrlHelper();
            //构建报修单
            var repair = new EquipRepairBill()
            {

                RepairType = (EquipRepairType)applyRepairInfo.RepairType,
                EquipAccountId = applyRepairInfo.EquipAccountId,
                SparePartId = applyRepairInfo.SparePartId,
                UrgentDegree = (UrgentDegree)applyRepairInfo.UrgentDegree,
                ProduceState = (ProduceState)applyRepairInfo.ProduceState,
                DeviceAbnormalId = applyRepairInfo.DeviceAbnormalId,
                DeviceAbnormalRemark = applyRepairInfo.DeviceAbnormalRemark,
                DeviceAbnormalCode = applyRepairInfo.DeviceAbnormalCode,
                SourceNo = applyRepairInfo.SourceNo,
                SourceType = (RepairSourceType)applyRepairInfo.RepairSourceType
            };
            repair.RepairNo =RT.Service.Resolve<RepairController>().GenerateRepairNo();
            repair.GenerateId();

            //附件列表
            applyRepairInfo.PhotoInfos.ForEach(p =>
            {
                if (p.Id == null)
                {
                    var attachment = hepler.GenerateAttachmentBase64StringContent(new EquipRepairAttachment(), p.Content, p.FileName) as EquipRepairAttachment;
                    attachment.OwnerId = repair.Id;
                    attachment.RepairOperationType = RepairOperationType.ApplyRepair;
                    repair.AttachmentList.Add(attachment);

                }
            });

            return RT.Service.Resolve<RepairController>().GenerateRepair(repair);
        }

        /// <summary>
        /// 查询维修单列表
        /// </summary>
        /// <param name="queryInfo">分页查询参数</param>
        /// <returns></returns>
        [ApiService("查询维修单列表")]
        [return: ApiReturn("维修单列表信息 RepairPagingResultInfo")]
        public virtual RepairPagingResultInfo GetRepairBillInfos([ApiParameter("分页查询参数")] RepairBillQueryInfo queryInfo)
        {
            //构建分页实体
            var pageInfo = this.GeneratePagingInfo(queryInfo);

            //查询维修单数据
            var states = queryInfo.RepairStates.Select(p => (EquipRepairState)p).ToList();//单据状态
            var type = (EquipRepairType?)queryInfo.EquipRepairType;//维修类型
            var operationType = (RepairOperationType)queryInfo.Action;
            var repairs = RT.Service.Resolve<RepairController>().GetEquipRepairBills(pageInfo, states, type, queryInfo.Keyword, operationType);

            //构建返回实体
            var info = new RepairPagingResultInfo();
            info.PageNumber = queryInfo.PageNumber;
            info.PageSize = queryInfo.PageSize;
            info.TotalCount = repairs.TotalCount;

            foreach (var e in repairs)
            {
                var repairBill = e as EquipRepairBill;

                info.RepairResultInfos.Add(new RepairResultInfo()
                {
                    RepairId = repairBill.Id,
                    RepairNo = repairBill.RepairNo,
                    EquipId = repairBill.EquipAccountId.Value,
                    EquipCode = repairBill.EquipAccountCode,
                    EquipName = repairBill.EquipAccountName,
                    SparePartId = repairBill.SparePartId.Value,
                    SparePartCode = repairBill.SparePartCode,
                    SparePartName = repairBill.SparePartName,
                    WarrantyPeriod = repairBill.WarrantyPeriod,
                    WorkShop = repairBill.WorkShopName,
                    Process = repairBill.ProcessName,
                    Line = repairBill.ResourceName,
                    Department = repairBill.UseDepartmentName,
                    DeviceAbnormalCode = repairBill.DeviceAbnormalCode,
                    DeviceAbnormalRemark = repairBill.DeviceAbnormalRemark,
                    DeviceAbnormalId = repairBill.DeviceAbnormalId,
                    DeviceAbnormalDesc = repairBill.DeviceAbnormalCode,
                    ProduceState = (int)repairBill.ProduceState,
                    RepairType = (int)repairBill.RepairType,
                    UrgentDegree = (int)repairBill.UrgentDegree,
                    ApplyRepairDate = repairBill.ApplyRepairDate,
                    ApplyRepairEmployee = repairBill.ApplyRepairEmployeeName,
                    RepairState = (int)repairBill.RepairState,
                    ReceiveOrderDate = repairBill.ReceiveOrderDate,
                    EstimateFinishDate = repairBill.EstimateFinishDate,
                    SourceTypeStr = repairBill.SourceType.ToLabel().L10N(),
                });
            }

            return info;
        }

        /// <summary>
        /// 查询维修人员(设备/备件)
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        [ApiService("查询维修人员")]
        [return: ApiReturn("维修人员列表 RepairerPagingResultInfo")]
        public virtual RepairerPagingResultInfo GetRepairerInfos([ApiParameter("分页查询参数")] RepairerQueryInfo queryInfo)
        {
            //构建分页实体
            var pageInfo = this.GeneratePagingInfo(queryInfo);
            EntityList<Employee> employees = new EntityList<Employee>();
            if (queryInfo.EquipId.HasValue && queryInfo.EquipId > 0)
            {
                //获取拥有该设备维修权限员工列表
                employees = RT.Service.Resolve<DevicePurController>().GetDevicePurRepairs(queryInfo.EquipId.Value, queryInfo.Keyword, pageInfo);
            }
            if (queryInfo.SparePartId.HasValue && queryInfo.SparePartId > 0)
            {
                //获取拥有备件维修权限员工列表
                employees = RT.Service.Resolve<DevicePurController>().GetSparePartDevicePurRepairs(queryInfo.Keyword, pageInfo);
            }

            var employeeIds = employees.Select(p => p.Id).ToList();
            var ctl = RT.Service.Resolve<RepairController>();
            var waitRepairBill = ctl.GetRepairOfEmployeeCount(EquipRepairState.WaitRepair, employeeIds);//待维修数据
            var repairingBill = ctl.GetRepairOfEmployeeCount(EquipRepairState.Repairing, employeeIds);//已维修数据

            //构建返回实体
            var info = new RepairerPagingResultInfo();
            info.PageNumber = queryInfo.PageNumber;
            info.PageSize = queryInfo.PageSize;
            info.TotalCount = employees.TotalCount;
            employees.ForEach(p =>
            {
                info.RepairerResultInfos.Add(new RepairerResultInfo()
                {
                    EmployeeId = p.Id,
                    EmployeeCode = p.Code,
                    EmployeeName = p.Name,
                    WaitRepairCount = waitRepairBill.Where(b => b.RepairEmployeeIds.Split(',').Contains(p.Id.ToString()) || b.RepairMasterId == p.Id).ToList().Count,
                    RepairingCount = repairingBill.Where(b => b.RepairEmployeeIds.Split(',').Contains(p.Id.ToString()) || b.RepairMasterId == p.Id).ToList().Count,
                });
            });
            return info;
        }

        /// <summary>
        /// 设备维修接单提交
        /// </summary>
        /// <param name="takeRepairInfo"></param>
        [ApiService("设备维修接单提交")]
        public virtual void TakeRepair([ApiParameter("接单参数")] TakeRepairInfo takeRepairInfo)
        {
            RT.Service.Resolve<RepairController>().TakeRepair(takeRepairInfo);
        }

        /// <summary>
        /// 设备维修派工提交
        /// </summary>
        /// <param name="dispatchRepairInfo"></param>
        [ApiService("设备维修派工提交")]
        public virtual void DispatchRepair([ApiParameter("派工参数")] DispatchRepairInfo dispatchRepairInfo)
        {
            RT.Service.Resolve<RepairController>().DispatchRepair(dispatchRepairInfo);
        }

        /// <summary>
        ///查询指定维修单
        /// </summary>
        /// <param name="queryInfo">维修单查询参数实体</param>
        /// <returns></returns>
        [ApiService("查询指定维修单")]
        [return: ApiReturn("维修单详细信息 RepairDetailResultInfo")]
        public virtual RepairDetailResultInfo GetRepairBillDetailsInfo([ApiParameter("维修单查询参数实体")] RepairDetailQueryInfo queryInfo)
        {
            //维修单主数据
            var repairBill = RT.Service.Resolve<RepairController>().GetEquipRepairBill(queryInfo.RepairBillId);
            //获取故障原因快码
            var catalog = RT.Service.Resolve<CatalogController>().GetCatalog(EquipRepairBill.CatalogExpFaultReson, repairBill.FaultReason);

            //计算响应时间
            decimal? responseTime = null;
            if (repairBill.RepairBeginDate.HasValue)
            {
                responseTime = (decimal)Math.Round((repairBill.RepairBeginDate.Value - repairBill.ApplyRepairDate).TotalHours, 2);
            }
            //计算执行时间
            decimal? executeTime = null;

            //构建返回实体
            var info = new RepairDetailResultInfo()
            {
                RepairId = repairBill.Id,
                RepairNo = repairBill.RepairNo,
                EquipId = repairBill.EquipAccountId.Value,
                EquipCode = repairBill.EquipAccountCode,
                EquipName = repairBill.EquipAccountName,
                SparePartId = repairBill.SparePartId.Value,
                SparePartCode = repairBill.SparePartCode,
                SparePartName = repairBill.SparePartName,
                EquipTypeId = repairBill.EquipTypeId,
                EquipTypeCode = repairBill.EquipAccountTypeCode,
                EquipTypeName = repairBill.EquipAccountTypeName,
                EquipModelId = repairBill.EquipModelId,
                EquipModelCode = repairBill.EquipModelCode,
                EquipModelName = repairBill.EquipAccountMode,
                WarrantyPeriod = repairBill.WarrantyPeriod,
                WorkShop = repairBill.WorkShopName,
                Process = repairBill.ProcessName,
                Line = repairBill.ResourceName,
                Department = repairBill.UseDepartmentName,
                DeviceAbnormalCode = repairBill.DeviceAbnormalCode,
                DeviceAbnormalRemark = repairBill.DeviceAbnormalRemark,
                DeviceAbnormalId = repairBill.DeviceAbnormalId,
                DeviceAbnormalDesc = repairBill.DeviceAbnormal?.Code,
                ProduceState = (int)repairBill.ProduceState,
                RepairType = (int)repairBill.RepairType,
                UrgentDegree = (int)repairBill.UrgentDegree,
                ApplyRepairDate = repairBill.ApplyRepairDate,
                RepairBeginDate = repairBill.RepairBeginDate,
                ApplyRepairEmployee = repairBill.ApplyRepairEmployeeName,
                RepairState = (int)repairBill.RepairState,
                ReceiveOrderDate = repairBill.ReceiveOrderDate,
                EstimateFinishDate = repairBill.EstimateFinishDate,
                RepairResponseTime = responseTime,
                RepairExecuteTime = executeTime,
                RepairTotalWorkingHour = repairBill.RepairTime,
                SourceTypeStr = repairBill.SourceType.ToLabel().L10N(),
                //维修报告
                RepairWay = (int?)repairBill.RepairWay,
                OutsourcedMaintenanceReport = repairBill.OutsourcedMaintenanceReport,
                FaultReasonCode = catalog?.Code,
                FaultReasonName = catalog?.Name,
                FaultReasonDesc = catalog?.Description,
                FaultLevel = (int?)repairBill.FaultLevel,
                RepairCosts = repairBill.RepairCosts,
                FaultCategoryId = repairBill.FaultCategoryId,
                FaultCategoryCode = repairBill.FaultCategoryCode,
                FaultCategoryName = repairBill.FaultCategoryName,
                FaultPart = repairBill.FaultPart,
                RepairCategory = (int?)repairBill.RepairCategory,
                RepairDowntime = repairBill.RepairDowntime,
                RepairLevel = (int?)repairBill.RepairLevel,
                RepairMethod = repairBill.RepairMethod,
                PreventionAdvice = repairBill.PreventionAdvice,
                FaultDescriptionId = repairBill.FaultDescriptionId,
                FaultDescriptionCode = repairBill.FaultDescriptionCode,
                FaultDescriptionDesc = repairBill.FaultDescriptionDesc,
                FaultDescriptionRemark = repairBill.FaultDescriptionRemark,
                ProjectCode = repairBill.ProjectCode,
                ProjectItemCode = repairBill.ProjectKeyItemDesc,
            };

            QueryChildPage(info, queryInfo);

            return info;
        }

        /// <summary>
        /// 是否查询子页签
        /// </summary>
        /// <param name="info"></param>
        /// <param name="queryInfo"></param>
        private void QueryChildPage(RepairDetailResultInfo info, RepairDetailQueryInfo queryInfo)
        {
            //设备维修规程
            if (queryInfo.IsQueryBillProject)
            {
                var billProjectInfos = this.GenerateBillProjectInfos(queryInfo.RepairBillId);
                info.BillProjectInfos.AddRange(billProjectInfos);
            }

            //设备维修工时
            if (queryInfo.IsQueryWorkingHours)
            {
                var workingHoursInfos = this.GenerateRepairWorkingHoursInfos(queryInfo.RepairBillId);
                info.WorkingHoursInfos.AddRange(workingHoursInfos);
            }

            //设备维修操作记录
            if (queryInfo.IsQueryOperationRec)
            {
                var operationRecInfos = this.GenerateRepairOperationRecInfos(queryInfo.RepairBillId);
                info.OperationRecInfos.AddRange(operationRecInfos);
            }

            //设备维修备件申请
            if (queryInfo.IsQuerySparePartApl)
            {
                var sparePartAplInfos = this.GenerateRepairSparePartAplInfos(queryInfo.RepairBillId);
                info.SparePartAplInfos.AddRange(sparePartAplInfos);
            }

            //设备维修备件更换
            if (queryInfo.IsQuerySparePartChg)
            {
                var sparePartChgInfos = this.GenerateRepairSparePartChgInfos(queryInfo.RepairBillId);
                info.SparePartChgInfos.AddRange(sparePartChgInfos);
            }

            //设备维修附件
            if (queryInfo.IsQueryReportAttachment)
            {
                var attachmentInfos = this.GenerateRepairAttachmentInfos(queryInfo.RepairBillId, RepairOperationType.Report);
                info.AttachmentInfos.AddRange(attachmentInfos);
            }

            //设备报修附件
            if (queryInfo.IsQueryApplyRepairAttachment)
            {
                var attachmentInfos = this.GenerateRepairAttachmentInfos(queryInfo.RepairBillId, RepairOperationType.ApplyRepair);
                info.ApplyRepairAttachmentInfos.AddRange(attachmentInfos);
            }
        }

        /// <summary>
        /// 开始维修
        /// </summary>
        /// <param name="actionInfo"></param>
        [ApiService("开始维修")]
        public virtual void BeginRepair([ApiParameter("操作参数")] RepairBillActionInfo actionInfo)
        {
            RT.Service.Resolve<RepairController>().BeginRepair(actionInfo.RepairId, (ProduceState)actionInfo.ProduceState);
        }

        /// <summary>
        /// 暂停维修
        /// </summary>
        /// <param name="actionInfo"></param>
        [ApiService("暂停维修")]
        public virtual void SuspendRepair([ApiParameter("操作参数")] RepairBillActionInfo actionInfo)
        {
            RT.Service.Resolve<RepairController>().SuspendRepair(actionInfo.RepairId, actionInfo.Remark);
        }

        /// <summary>
        /// 继续维修
        /// </summary>
        /// <param name="actionInfo"></param>
        [ApiService("继续维修")]
        public virtual void ContinueRepair([ApiParameter("操作参数")] RepairBillActionInfo actionInfo)
        {
            RT.Service.Resolve<RepairController>().ContinueRepair(actionInfo.RepairId);
        }

        /// <summary>
        /// 设备维修转派提交
        /// </summary>
        /// <param name="transfeRepairInfo"></param>
        [ApiService("设备维修转派提交")]
        public virtual void TransferRepair([ApiParameter("转派参数")] TransfeRepairInfo transfeRepairInfo)
        {
            RT.Service.Resolve<RepairController>().TransferRepair(transfeRepairInfo);
        }

        /// <summary>
        /// 查询设备维修故障原因信息(快码)
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        [ApiService("查询设备维修故障原因信息(快码)")]
        [return: ApiReturn("设备维修故障原因信息(快码) FaultReasonPagingResultInfo")]
        public virtual FaultReasonPagingResultInfo GetFaultReasonInfo([ApiParameter("分页查询参数")] PagingKeywordQueryInfo queryInfo)
        {
            //构建分页实体
            var pageInfo = this.GeneratePagingInfo(queryInfo);

            //获取故障原因快码
            var catalogs = RT.Service.Resolve<CatalogController>().GetCatalogList(EquipRepairBill.CatalogExpFaultReson, pageInfo, queryInfo.Keyword);

            //构建返回实体
            var info = new FaultReasonPagingResultInfo();
            info.TotalCount = catalogs.TotalCount;
            info.PageNumber = queryInfo.PageNumber;
            info.PageSize = queryInfo.PageSize;
            catalogs.ForEach(p =>
            {
                info.FaultReasonResultInfos.Add(new FaultReasonResultInfo()
                {
                    Id = p.Id,
                    Code = p.Code,
                    Name = p.Name,
                    Description = p.Description
                });
            });

            return info;
        }

        /// <summary>
        /// 查询设备故障类别信息
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        [ApiService("查询设备故障类别信息")]
        [return: ApiReturn("设备故障类别信息 FaultReasonPagingResultInfo")]
        public virtual FaultCategoryPagingResultInfo GetFaultCategoryInfo([ApiParameter("分页查询参数")] PagingKeywordQueryInfo queryInfo)
        {
            //构建分页实体
            var pageInfo = this.GeneratePagingInfo(queryInfo);

            //获取故障原因快码
            Expression<Func<EquipLargeFault, bool>> exp = (p => p.Code.Contains(queryInfo.Keyword) || p.Name.Contains(queryInfo.Keyword));
            var equipLargeFaults = RT.Service.Resolve<EquipFaultController>().GetEquipLargeFaults(exp, pageInfo);

            //构建返回实体
            var info = new FaultCategoryPagingResultInfo();
            info.TotalCount = equipLargeFaults.TotalCount;
            info.PageNumber = queryInfo.PageNumber;
            info.PageSize = queryInfo.PageSize;
            equipLargeFaults.ForEach(p =>
            {
                info.FaultCategoryResultInfos.Add(new BaseDataInfo()
                {
                    Id = p.Id,
                    Code = p.Code,
                    Name = p.Name
                });
            });

            return info;
        }

        /// <summary>
        /// 添加经验库
        /// </summary>
        /// <param name="expDepotInfo"></param>
        [ApiService("添加经验库")]
        public virtual void AddExperienceDepot([ApiParameter("经验库参数")] ExperienceDepotInfo expDepotInfo)
        {
            RT.Service.Resolve<RepairController>().AddAccountExperienceDepot(expDepotInfo);
        }

        /// <summary>
        /// 查询维修经验库信息
        /// </summary>
        /// <param name="queryInfo">查询参数</param>
        /// <returns></returns>
        [ApiService("查询维修经验库信息")]
        [return: ApiReturn("维修经验库信息 ExpDepotPagingResultInfo")]
        public virtual ExpDepotPagingResultInfo GetExperienceDepotInfo([ApiParameter("查询参数")] ExpDepotQueryInfo queryInfo)
        {
            //构建分页实体
            var pageInfo = this.GeneratePagingInfo(queryInfo);

            //设备维修控制器
            var ctl = RT.Service.Resolve<RepairController>();
            //查询维修经验库
            var experienceDepots = ctl.GetAccountExperienceDepots(pageInfo, queryInfo);
            //查询相关故障原因快码数据
            var catalogCodes = experienceDepots.Select(p => p.FaultReson).Distinct().ToList();
            var catalogs = ctl.GetFaultResons(catalogCodes);

            //构建返回实体
            var info = new ExpDepotPagingResultInfo();
            info.TotalCount = experienceDepots.TotalCount;
            info.PageNumber = queryInfo.PageNumber;
            info.PageSize = queryInfo.PageSize;
            experienceDepots.ForEach(p =>
            {
                var faultReson = catalogs.FirstOrDefault(x => x.Code == p.FaultReson);  //故障原因快码数据
                info.ExpDepotResultInfos.Add(new ExpDepotResultInfo()
                {
                    Id = p.Id,
                    Code = p.Code,
                    EquipAccountCode = p.EquipAccount?.Code,
                    EquipAccountName = p.EquipAccount?.Name,
                    SparePartCode = p.SparePart?.SparePartCode,
                    SparePartName = p.SparePart?.SparePartName,
                    DeviceAbnormalDesc = p.FaultPhenomenon?.Code,
                    FaultDescriptionDesc = p.FaultDescribe?.Code,
                    DeviceAbnormalRemark = p.FaultPhenomenonRemark,
                    FaultDescriptionRemark = p.FaultDescribeRemark,
                    FaultReasonName = faultReson?.Name,
                    FaultReasonDesc = faultReson?.Description,
                    FaultCategoryName = p.EquipLargeFault?.Name,
                    FaultPart = p.FaultPart,
                    RepairMethod = p.RepairWay,
                    PreventionAdvice = p.PreventionAdvice,
                    FaultCode = p.FaultCode
                });
            });

            return info;
        }

        /// <summary>
        /// 应用维修经验库信息
        /// </summary>
        /// <param name="applyExpDepotInfo"></param>
        [ApiService("应用维修经验库信息")]
        public virtual void ApplyExperienceDepot([ApiParameter("查询参数")] ApplyExpDepotInfo applyExpDepotInfo)
        {
            RT.Service.Resolve<RepairController>().ApplyExperienceDepot(applyExpDepotInfo);
        }

        /// <summary>
        /// 执行维修备件更换
        /// </summary>
        /// <param name="info"></param>
        [ApiService("执行维修备件更换")]
        public virtual void ChangeRepairSpareParts([ApiParameter("维修单参数实体")] RepairSaveSubmitInfo info)
        {
            var ctl = RT.Service.Resolve<RepairController>();
            using (var trans = DB.TransactionScope(EquipRepairEntityDataProvider.ConnectionStringName))
            {
                //保存维修备件更换信息
                ctl.SaveRepairSparePartChg(info.SparePartChgDetails, info.RepairBillId);
                //执行维修备件更换
                ctl.ChangeRepairSparePart(info.RepairBillId);
                //提交事务
                trans.Complete();
            }
        }

        /// <summary>
        /// 维修申请备件申请单
        /// </summary>
        /// <param name="info"></param>
        [ApiService("执行维修备件申请")]
        public virtual void GenerateSparePartApp([ApiParameter("维修单参数实体")] RepairSaveSubmitInfo info)
        {
            var ctl = RT.Service.Resolve<RepairController>();
            using (var trans = DB.TransactionScope(EquipRepairEntityDataProvider.ConnectionStringName))
            {
                //保存维修备件申请信息
                ctl.SaveRepairSparePartApl(info.SparePartAplDetails, info.RepairBillId);
                //执行维修备件申请
                ctl.GenerateRepairSparePartApp(info.RepairBillId);
                //提交事务
                trans.Complete();
            }
        }

        /// <summary>
        /// 保存维修工时数据
        /// </summary>
        /// <param name="info"></param>
        [ApiService("保存维修工时数据")]
        public virtual void SaveRepairWorkingHours([ApiParameter("维修单参数实体")] RepairSaveSubmitInfo info)
        {
            RT.Service.Resolve<RepairController>().SaveRepairWorkingHours(info.WorkingHoursDetails, info.RepairBillId);
        }

        /// <summary>
        /// 提交是否跳过维修报告
        /// </summary>
        /// <param name="info">维修单参数实体</param>
        [ApiService("完成设备维修单")]
        public virtual string CompleteRepairValidate([ApiParameter("维修单参数实体")] RepairSaveSubmitInfo info)
        {
            var ctl = RT.Service.Resolve<RepairController>();
            var errorStr = ctl.ValidateRepairReport(info);
            return errorStr;
        }

        /// <summary>
        /// 完成设备维修单
        /// </summary>
        /// <param name="info">维修单参数实体</param>
        [ApiService("完成设备维修单")]
        public virtual void CompleteRepair([ApiParameter("维修单参数实体")] RepairSaveSubmitInfo info)
        {
            var ctl = RT.Service.Resolve<RepairController>();
            using (var trans = DB.TransactionScope(EquipRepairEntityDataProvider.ConnectionStringName))
            {
                //保存维修备件更换信息
                ctl.SaveRepairSparePartChg(info.SparePartChgDetails, info.RepairBillId);
                //保存维修备件申请信息
                ctl.SaveRepairSparePartApl(info.SparePartAplDetails, info.RepairBillId);
                //保存设备维修工时信息
                ctl.SaveRepairWorkingHours(info.WorkingHoursDetails, info.RepairBillId);
                //保存维修报告字段
                ctl.SaveRepairReport(info);
                //保存附件图片
                ctl.GenerateBase64Photos(info.PhotoeDetails, info.RepairBillId, RepairOperationType.Report);
                //校验参数
                bool isFillInReport = info.IsFillInReport;
                //完成维修
                var repairBill = RF.GetById<EquipRepairBill>(info.RepairBillId, new EagerLoadOptions().LoadWithViewProperty());
                ctl.FinishRepair(repairBill, isFillInReport);
                //提交事务
                trans.Complete();
            }
        }

        /// <summary>
        /// 查询维修评分项
        /// </summary>
        [ApiService("查询维修评分项")]
        [return: ApiReturn("维修评分项信息列表 List<TpmWeekInspectScoreResultInfo>")]
        public virtual List<TpmScoreResultInfo> GetRepirTpmScoreResultInfos()
        {
            //查询评分项数据
            var data = RT.Service.Resolve<TpmController>().GetTpmWeekInspectScores(ScoreType.Repair);

            //构建返回实体
            var infos = new List<TpmScoreResultInfo>();
            data.ForEach(p =>
            {
                infos.Add(new TpmScoreResultInfo()
                {
                    ProjectId = p.Id,
                    ProjectName = p.ProjectName,
                    IsPhoto = p.IsPhoto
                });
            });

            return infos;
        }

        /// <summary>
        /// 提交维修交机确认
        /// </summary>
        [ApiService("提交维修交机确认")]
        public virtual void SubmitRepirHandoverConfirm([ApiParameter("交机确认参数实体")] HandoverConfirmInfo info)
        {
            //构建临时实体，用于传参，非保存
            var repairBill = RF.GetById<EquipRepairBill>(info.RepairBillId, new EagerLoadOptions().LoadWithViewProperty());
            repairBill.HandoverConfirmResult = (HandoverConfirmResult)info.HandoverConfirmResult;
            repairBill.HandoverDeviceAbnormalId = info.HandoverDeviceAbnormalId;
            repairBill.HandoverDeviceAbnormalRem = info.HandoverDeviceAbnormalRem;
            repairBill.HandoverConfirmAbnormal = (HandoverConfirmAbnormal)info.HandoverConfirmAbnormal;
            repairBill.Id = info.RepairBillId;
            repairBill.HandoverAttachment = "";
            repairBill.MarkSaved();

            //构建评分项明细
            const string confirmPath = "ConfirmAttachment";
            var handoverConfirmDtls = new EntityList<HandoverConfirmDetail>();
            info.TpmScoreInfos.ForEach(p =>
            {
                //暂存文件路径
                p.Path = $"{confirmPath}/{Guid.NewGuid()}";
                //构建评分项实体列表
                handoverConfirmDtls.Add(new HandoverConfirmDetail()
                {
                    EquipRepairBillId = info.RepairBillId,
                    TpmWeekInspectScoreId = p.ProjectId,
                    EquipRepairScore = (EquipRepairScore?)p.EquipRepairScore,
                    HandoverAttachment = p.PhotoContent.IsNotEmpty() ? $"{p.Path}/{p.FileName}" : string.Empty,
                });
            });

            using (var trans = DB.TransactionScope(EquipRepairEntityDataProvider.ConnectionStringName))
            {
                //执行维修交机确认
                RT.Service.Resolve<RepairController>().HandoverConfirm(repairBill, handoverConfirmDtls, info.PhotoContent.IsNotEmpty() ? info.PhotoContent.Substring(info.PhotoContent.IndexOf(",") + 1) : "");
                //上传图片文件到附件服务器
                info.TpmScoreInfos.ForEach(p =>
                {
                    if (p.PhotoContent.IsNotEmpty())
                    {
                        var content = p.PhotoContent.Substring(p.PhotoContent.IndexOf(",") + 1);
                        var bytes = Convert.FromBase64String(content);//转换base64图片字符串
                        RT.Service.Resolve<SIE.Common.Attachments.AttachmentController>().FileStorage(p.FileName, bytes, p.Path);
                    }
                });

                trans.Complete();
            }
        }

        /// <summary>
        /// 提交维修工程确认
        /// </summary>
        /// <param name="info"></param>
        [ApiService("提交维修工程确认")]
        public virtual void SubmitRepirEngineerConfirm([ApiParameter("交机确认参数实体")] EngineerConfirmInfo info)
        {
            //构建临时实体，用于传参，非保存
            var repairBill = RF.GetById<EquipRepairBill>(info.RepairBillId, new EagerLoadOptions().LoadWithViewProperty());
            //维修报告字段赋值
            repairBill.DeviceAbnormalCode = info.DeviceAbnormalCode;
            repairBill.FaultCategoryId = info.FaultCategoryId;
            repairBill.FaultDescriptionId = info.FaultDescriptionId;
            repairBill.FaultDescriptionRemark = info.FaultDescriptionRemark;
            repairBill.FaultLevel = (FaultLevel?)info.FaultLevel;
            repairBill.FaultReason = info.FaultReasonCode;
            repairBill.FaultPart = info.FaultPart;
            repairBill.OutsourcedMaintenanceReport = info.OutsourcedMaintenanceReport;
            repairBill.RepairCategory = (RepairCategory?)info.RepairCategory;
            repairBill.PreventionAdvice = info.PreventionAdvice;
            repairBill.RepairCosts = info.RepairCosts;
            repairBill.RepairDowntime = info.RepairDowntime;
            repairBill.RepairLevel = (RepairLevel?)info.RepairLevel;
            repairBill.RepairMethod = info.RepairMethod;

            repairBill.EngineerConfirmResult = EngineerConfirmResult.Confirmed;
            repairBill.Id = info.RepairBillId;
            repairBill.MarkSaved();
            repairBill.PersistenceStatus = PersistenceStatus.Modified;

            //构建评分项明细
            const string confirmPath = "ConfirmAttachment";
            var engineerConfirmDtls = new EntityList<EngineerConfirmDetail>();
            info.TpmScoreInfos.ForEach(p =>
            {
                //暂存文件路径
                p.Path = $"{confirmPath}/{Guid.NewGuid()}";
                //构建评分项实体列表
                engineerConfirmDtls.Add(new EngineerConfirmDetail()
                {
                    EquipRepairBillId = info.RepairBillId,
                    TpmWeekInspectScoreId = p.ProjectId,
                    EquipRepairScore = (EquipRepairScore?)p.EquipRepairScore,
                    EngineerAttachment = p.PhotoContent.IsNotEmpty() ? $"{p.Path}/{p.FileName}" : string.Empty
                });
            });

            using (var trans = DB.TransactionScope(EquipRepairEntityDataProvider.ConnectionStringName))
            {
                //先更新维修报告的内容
                RF.Save(repairBill);
                //执行维修工程确认(PC逻辑)
                RT.Service.Resolve<RepairController>().EngineerConfirm(repairBill, engineerConfirmDtls);
                //上传图片文件到附件服务器
                info.TpmScoreInfos.Where(p => p.PhotoContent.IsNotEmpty()).ForEach(p =>
                {
                    var bytes = Convert.FromBase64String(p.PhotoContent.Split(',')[1]);//转换base64图片字符串
                    RT.Service.Resolve<SIE.Common.Attachments.AttachmentController>().FileStorage(p.FileName, bytes, p.Path);
                });

                trans.Complete();
            }
        }

        #endregion

        #region Private

        /// <summary>
        /// 生成查询实体
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        private PagingInfo GeneratePagingInfo(PagingKeywordQueryInfo queryInfo)
        {
            if (queryInfo.PageSize == null || queryInfo.PageNumber == null)
            {
                return null;
            }

            if (queryInfo.PageSize <= 0)
                throw new ValidationException("[每页数据量]必须大于0".L10N());
            if (queryInfo.PageNumber <= 0)
                throw new ValidationException("[页码]必须大于0".L10N());

            //构建分页实体
            var pageInfo = new PagingInfo()
            {
                PageSize = queryInfo.PageSize.Value,
                PageNumber = queryInfo.PageNumber.Value,
                IsNeedCount = true
            };

            return pageInfo;
        }

        /// <summary>
        /// 生成维修规程数据信息
        /// </summary>
        /// <param name="repairBillId"></param>
        /// <returns></returns>
        private List<RepairBillProjectResultInfo> GenerateBillProjectInfos(double repairBillId)
        {
            var list = new List<RepairBillProjectResultInfo>();
            var datas = RT.Service.Resolve<RepairController>().GetEquipRepairBillProjects(repairBillId);
            datas.ForEach(p =>
            {
                list.Add(new RepairBillProjectResultInfo()
                {
                    ProjectId = p.ProjectDetailId,
                    ProjectName = p.ProjectDetail?.Name,
                    Part = p.Part,
                    Consumable = p.Consumable,
                    Method = p.Method,
                    Standard = p.Standard,
                    MinValue = p.MinValue,
                    MaxValue = p.MaxValue,
                    Unit = p.Unit,
                    UseTime = p.UseTime,
                });
            });
            return list;
        }

        /// <summary>
        /// 生成维修工时数据信息
        /// </summary>
        /// <param name="repairBillId"></param>
        /// <returns></returns>
        private List<RepairWorkingHoursResultInfo> GenerateRepairWorkingHoursInfos(double repairBillId)
        {
            var list = new List<RepairWorkingHoursResultInfo>();
            var datas = RT.Service.Resolve<RepairController>().GetEquipRepairWorkingHours(repairBillId);
            datas.ForEach(p =>
            {
                list.Add(new RepairWorkingHoursResultInfo()
                {
                    RepairWorkingHourId = p.Id,
                    RepairerId = p.RepairerId,
                    RepairerCode = p.Repairer?.Code,
                    RepairerName = p.Repairer?.Name,
                    BeginTime = p.BeginTime,
                    EndTime = p.EndTime,
                    IsRepairEmployee = p.IsRepairEmployee,
                    IsRepairMaster = p.IsRepairMaster
                });
            });

            return list;
        }

        /// <summary>
        /// 生成维修操作记录数据信息
        /// </summary>
        /// <param name="repairBillId"></param>
        /// <returns></returns>
        private List<RepairOperationRecResultInfo> GenerateRepairOperationRecInfos(double repairBillId)
        {
            var list = new List<RepairOperationRecResultInfo>();
            var datas = RT.Service.Resolve<RepairController>().GetEquipRepairOperationRecs(repairBillId);
            datas.ForEach(p =>
            {
                list.Add(new RepairOperationRecResultInfo()
                {
                    Operationer = p.Operationer.Name,
                    OperationType = (int)p.OperationType,
                    OperationDate = p.OperationDate,
                    Remark = p.Remark,
                    EngineerConfirmResult = (int?)p.EngineerConfirmResult,
                    HandoverConfirmResult = (int?)p.HandoverConfirmResult
                });
            });

            return list;
        }

        /// <summary>
        /// 生成维修备件申请数据信息
        /// </summary>
        /// <param name="repairBillId"></param>
        /// <returns></returns>
        private List<RepairSparePartAplResultInfo> GenerateRepairSparePartAplInfos(double repairBillId)
        {
            var list = new List<RepairSparePartAplResultInfo>();
            var datas = RT.Service.Resolve<RepairController>().GetRepairSparePartApls(repairBillId);
            datas.ForEach(p =>
            {
                int? state = null;
                list.Add(new RepairSparePartAplResultInfo()
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
                    RepairSparePartAplId = p.Id,
                    UseQty = p.ApplyDetail == null ? 0 : p.ApplyDetail.UseAmount,
                    SparePartApplyNo = p.ApplyDetail?.SparePartApp.No,
                    SparePartApplyState = p.ApplyDetail?.SparePartApp == null ? state : (int)p.ApplyDetail.SparePartApp.AuditState,
                    ApplyStateName = p.ApplyDetail?.SparePartApp == null ? string.Empty : p.ApplyDetail.SparePartApp.AuditState.ToLabel(),
                    StoreQty = (int)p.StoreQty,
                    IsApply = p.IsApply
                });
            });

            return list;
        }

        /// <summary>
        /// 生成维修备件更换数据信息
        /// </summary>
        /// <param name="repairBillId"></param>
        /// <returns></returns>
        private List<RepairSparePartChgResultInfo> GenerateRepairSparePartChgInfos(double repairBillId)
        {
            var list = new List<RepairSparePartChgResultInfo>();
            var datas = RT.Service.Resolve<RepairController>().GetRepairSparePartChgs(repairBillId);
            datas.ForEach(p =>
            {
                int? state = null;
                list.Add(new RepairSparePartChgResultInfo()
                {
                    SparePartId = p.SparePartId,
                    SparePartCode = p.SparePart.SparePartCode,
                    SparePartName = p.SparePart.SparePartName,
                    OutDtlId = p.PartOutDepotDetailId,
                    ChangeQty = p.ChangeQty,
                    Remark = p.Remark,
                    RepairSparePartChgId = p.Id,
                    UseQty = p.PartOutDepotDetail?.UseCount ?? 0,
                    OutDepotNo = p.PartOutDepotDetail?.OutDepot.No,
                    OutDepotState = p.PartOutDepotDetail?.OutDepot == null ? state : (int)p.PartOutDepotDetail.OutDepot.OutDepotState,
                    OutDepotStateName = p.PartOutDepotDetail?.OutDepot == null ? string.Empty : p.PartOutDepotDetail.OutDepot.OutDepotState.ToLabel(),
                    State = (int)p.State,
                    StateName = p.State.ToLabel(),
                    SeriaNo = p.PartOutDepotDetail?.SeriaNo,
                    BatchNo = p.PartOutDepotDetail?.BatchNoRef?.BatchNumber,
                    RemainingQty = p.PartOutDepotDetail == null ? 0 : p.PartOutDepotDetail.OutDepotCount - p.PartOutDepotDetail.UseCount
                });
            });

            return list;
        }

        /// <summary>
        /// 生成维修附件数据信息
        /// </summary>
        /// <param name="repairBillId"></param>
        /// <param name="operationType"></param>
        /// <returns></returns>
        private List<RepairAttachmentInfo> GenerateRepairAttachmentInfos(double repairBillId, RepairOperationType operationType)
        {
            var list = new List<RepairAttachmentInfo>();
            var datas = RT.Service.Resolve<RepairController>().GetEquipRepairAttachments(repairBillId, operationType);
            datas.ForEach(p =>
            {
                var picBase64Str = FileUrlHelper.GetAttachmentBase64StringData(p.FilePath, p.FileName);
                list.Add(new RepairAttachmentInfo()
                {
                    Id = p.Id,
                    Content = picBase64Str,
                    FileName = p.FileName,
                    FileExtension = p.FileExtesion,
                    Type = (int?)p.RepairOperationType
                });
            });

            return list;
        }


        /// <summary>
        /// 判断当前单是否有未完成的维修单
        /// </summary>
        /// <param name="equipAccountId">设备台账id</param>
        /// <param name="sourceNo">来源单号</param>
        /// <param name="sourceType">来源类型</param>
        /// <returns></returns>
        [ApiService("判断当前单是否有未完成的维修单")]
        public virtual string CheckPlanWithUnFinishRepairBill([ApiParameter("设备台账id")] double equipAccountId, [ApiParameter("来源单号")] string sourceNo, [ApiParameter("来源类型")] int? sourceType)
        {
            return RT.Service.Resolve<RepairController>().CheckPlanWithUnFinishRepairBill(equipAccountId);
        }
        #endregion

        #region 用户管理接口

        /// <summary>
        /// 获取人员管理的设备数量
        /// </summary>
        /// <returns>用户管理台账的信息</returns>
        [ApiService("获取人员管理的设备数量")]
        public virtual UserManageEquipAccountInfo GetEquipAccountCount()
        {
            var usrId = RT.Identity.UserId;
            var result = RT.Service.Resolve<RepairController>().GetEquipAccountCount(usrId);
            return result;
        }

        #endregion

        #region 项目管理接口

        /// <summary>
        /// 获取项目编号
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns>项目</returns>
        [ApiService("获取项目编号")]
        public virtual List<ProjectResultInfo> GetProjects(string keyword)
        {
            var data = RT.Service.Resolve<ProjectController>().GetAuditedProjects(null, keyword);
            var infos = new List<ProjectResultInfo>();
            data.ForEach(p =>
            {
                infos.Add(new ProjectResultInfo()
                {
                    Id = p.Id,
                    Code = p.Code,
                    Name = p.Name
                });
            });
            return infos;
        }

        /// <summary>
        /// 获取项目事项
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <param name="keyword"></param>
        /// <returns>项目事项</returns>
        [ApiService("获取项目事项")]
        public virtual List<ProjectKeyItemResultInfo> GetProjectKeyItems(double projectId, string keyword)
        {
            var data = RT.Service.Resolve<ProjectController>().GetProjectKeyItemsOfProject(projectId, null, keyword);
            var infos = new List<ProjectKeyItemResultInfo>();
            data.ForEach(p =>
            {
                infos.Add(new ProjectKeyItemResultInfo()
                {
                    Id = p.Id,
                    Description = p.Description
                });
            });
            return infos;
        }
        #endregion
    }
}
