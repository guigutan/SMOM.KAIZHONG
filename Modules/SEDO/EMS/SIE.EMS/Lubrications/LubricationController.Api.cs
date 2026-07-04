using SIE.Api;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.ApiModel;
using SIE.EMS.Common.Utils;
using SIE.EMS.DevicePurs;
using SIE.EMS.Enums;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.Lubrications.ApiModels;
using SIE.EMS.MainenanceProjects;
using SIE.EMS.SpareParts.Applys.Controllers;
using SIE.Equipments.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using SIE.EMS.DataAuth;

namespace SIE.EMS.Lubrications
{
    /// <summary>
    /// 润滑API控制器
    /// </summary>
    public partial class LubricationController : DomainController
    {
        /// <summary>
        /// 获取当前登录用户所在部门待润滑的润滑单
        /// </summary>
        /// <param name="pageSize">每页数据量</param>
        /// <param name="pageNumber">页码</param>
        /// <param name="keyword">关键字</param>
        /// <param name="departmentIds">部门ID集合</param>
        /// <param name="state">状态</param>
        /// <returns>润滑记录列表</returns>
        [ApiService("获取当前登录用户所在部门待润滑的润滑单")]
        [return: ApiReturn("润滑记录列表")]
        public virtual LubricationData GetNotDoneLubricationInfos([ApiParameter("每页数据量")] int pageSize, [ApiParameter("页码")] int pageNumber,
            [ApiParameter("关键字")] string keyword, [ApiParameter("部门ID集合")] List<double> departmentIds, [ApiParameter("状态")] int? state)
        {
            if (pageSize <= 0)
            {
                throw new ValidationException("[每页数据量]必须大于0".L10N());
            }
            if (pageNumber <= 0)
            {
                throw new ValidationException("[页码]必须大于0".L10N());
            }
            //构建分页实体
            var pageInfo = new PagingInfo() { PageSize = pageSize, PageNumber = pageNumber, IsNeedCount = true };

            //获取当前登录用户所在部门待润滑的润滑单
            var list = GetNotDoneLubrications(keyword, departmentIds, pageInfo, (LubricationStatus?)state);

            //构建返回数据结构
            var data = new LubricationData();
            data.TotalCount = list.TotalCount;
            foreach (var e in list)
            {
                var lubrication = e as Lubrication;

                var info = new EquLubricationInfo();
                info.Id = lubrication.Id;
                info.No = lubrication.LubricationNo;
                info.EquipCode = lubrication.EquipAccountCode;
                info.EquipName = lubrication.EquipAccountName;
                info.EquipId = lubrication.EquipAccountId;
                info.DepartmentId = lubrication.DepartmentId;
                info.DepartmentCode = lubrication.DepartmentCode;
                info.DepartmentName = lubrication.DepartmentName;
                info.EquipTypeId = lubrication.EquipTypeId;
                info.State = (int)lubrication.LubricationStatus;
                info.StateName = lubrication.LubricationStatus.ToLabel().L10N();
                info.Shop = lubrication.WorkShopName;
                info.Line = lubrication.ResourceName;
                info.InstallationLocation = lubrication.InstallationLocation;
                info.BeginTime = lubrication.StartDateTime;
                info.EndTime = lubrication.EndDateTime;
                info.Implementation = lubrication.LubricationSummary;
                data.LubricationInfos.Add(info);

            }

            return data;
        }

        /// <summary>
        /// 获取当前登录用户所在部门待润滑的润滑单
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="departmentIds">部门ID</param>
        /// <param name="pageInfo">分页实体</param>
        /// <param name="exeState">单据状态</param>
        /// <returns>润滑单</returns>
        private EntityList GetNotDoneLubrications(string keyword, List<double> departmentIds, PagingInfo pageInfo, LubricationStatus? exeState)
        {
            var q = Query<Lubrication>();

            //过滤部门
            var deptIds = RT.Service.Resolve<DevicePurController>()
                .GetDutyDepartments(RT.Identity.UserId)
                .Select(p => p.Id)
                .ToList();

            if (departmentIds != null && departmentIds.Count >= 1)
            {
                departmentIds = departmentIds.Where(p => deptIds.Contains(p)).ToList();
            }
            else
            {
                departmentIds = deptIds;
            }

            var nullableDeptIds = departmentIds.Cast<double?>();

            q.Where(p => nullableDeptIds.Contains(p.DepartmentId) || p.DepartmentId == null);

            //模糊查询
            if (keyword.IsNotEmpty())
            {
                q.Where(p => p.EquipAccount.Code.Contains(keyword) || p.EquipAccount.Name.Contains(keyword));
            }
            //过滤状态
            if (exeState.HasValue)
            {
                q.Where(p => p.LubricationStatus == exeState);
            }
            else
            {
                q.Where(p => p.LubricationStatus == LubricationStatus.Pending || p.LubricationStatus == LubricationStatus.Doing);
            }

            //当前用户可管理的设备台账
            var iq = q.ToQuery();
            //iq.QueryWithEquipAccountPermissions(Lubrication.EquipAccountIdProperty.Name);

            return q.Repository.QueryList(iq, pageInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取润滑记录的润滑项目
        /// </summary>
        /// <param name="lubricationId">润滑记录ID</param>
        /// <returns>润滑项目</returns>
        [ApiService("获取润滑记录的润滑项目")]
        [return: ApiReturn("润滑项目")]
        public virtual List<LubricationProjectInfo> GetLubricationProjects([ApiParameter("润滑记录ID")] double lubricationId)
        {
            var projects = Query<LubricationDetail>().Where(p => p.LubricationId == lubricationId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            //构建返回数据
            var infos = new List<LubricationProjectInfo>();
            projects.ForEach(p =>
            {
                infos.Add(new LubricationProjectInfo()
                {
                    ProjectId = p.Id,
                    ProjectName = p.ProjectName,
                    Part = p.Part,
                    LubricatingType = p.LubricatingType.ToLabel().L10N(),
                    Consumable = p.Consumable,
                    Method = p.Method,
                    MaxValue = p.MaxValue,
                    MinValue = p.MinValue,
                    ActualValue = p.ActualValue,
                    DelayDays = p.DelayDays,
                });
            });
            return infos;
        }

        /// <summary>
        /// 获取润滑记录的备件更换
        /// </summary>
        /// <param name="lubricationId">润滑记录ID</param>
        /// <returns>润滑记录的备件更换</returns>
        [ApiService("获取润滑记录的备件更换")]
        [return: ApiReturn("润滑记录的备件更换")]
        public virtual List<LubricationSaveSparePartInfo> GetLubricationSparePartInfos([ApiParameter("润滑记录ID")] double lubricationId)
        {
            var lubSpareParts = Query<LubricationSparePart>().Where(p => p.LubricationId == lubricationId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            //构建返回数据
            var infos = new List<LubricationSaveSparePartInfo>();
            lubSpareParts.ForEach(p =>
            {
                var partOutDepotDetail = p.PartOutDepotDetail;
                infos.Add(new LubricationSaveSparePartInfo()
                {
                    LubricationSparePartId = p.Id,
                    SparePartId = p.SparePartId,
                    SparePartCode = p.SparePartCodeView,
                    SparePartName = p.SparePartNameView,
                    OutDtlId = p.PartOutDepotDetailId,
                    ChangeQty = p.ChangeQty,
                    Remark = p.Remark,
                    OutDepotNo = p.OutDepotNoView,
                    State = (int)p.State,
                    StateName = p.State.ToLabel().L10N(),
                    SeriaNo = p.SeriaNoView,
                    BatchNo = p.BatchNoView,
                    RemainingQty = partOutDepotDetail == null ? 0 : partOutDepotDetail.OutDepotCount - partOutDepotDetail.UseCount
                });
            });
            return infos;
        }

        /// <summary>
        /// 获取润滑记录的备件申请
        /// </summary>
        /// <param name="lubricationId">润滑记录ID</param>
        /// <returns>润滑记录的备件申请</returns>
        [ApiService("获取润滑记录的备件申请")]
        [return: ApiReturn("润滑记录的备件申请")]
        public virtual List<LubricationSparePartAplInfo> GetLubricationSparePartApls([ApiParameter("润滑记录ID")] double lubricationId)
        {
            var lubSparePartApls = Query<LubricationSparePartApply>().Where(p => p.LubricationId == lubricationId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            //构建返回数据
            var infos = new List<LubricationSparePartAplInfo>();
            lubSparePartApls.ForEach(p =>
            {
                infos.Add(new LubricationSparePartAplInfo()
                {
                    LubricationSparePartId = p.Id,
                    SparePartId = p.SparePartId,
                    SparePartCode = p.SparePartCodeView,
                    SparePartName = p.SparePartNameView,
                    ApplyQty = p.ApplyQty,
                    OutStockWarehouseId = p.OutStockWarehouseId,
                    OutStockWarehouseName = p.OutStockWarehouseName,
                    AppDtlId = p.ApplyDetailId,
                    Remark = p.Remark,
                    SparePartApplyNo = p.ApplyNoView,
                    SparePartApplyState = (int?)p.AppStateView,
                    ApplyStateName = p.AppStateView.ToLabel().L10N(),
                    StoreQty = (int)p.StoreQty,
                    IsApply = p.IsApply
                });
            });
            return infos;
        }

        /// <summary>
        /// 获取润滑记录的备件清单
        /// </summary>
        /// <param name="lubricationId">润滑记录ID</param>
        /// <returns>润滑记录的备件清单</returns>
        [ApiService("获取润滑记录的备件清单")]
        [return: ApiReturn("润滑记录的备件清单")]
        public virtual EntityList<EquipAccountLubricaSparePart> GetEquLubricaSparePartInfos([ApiParameter("润滑记录ID")] double lubricationId)
        {
            var proIds = Query<LubricationDetail>().Where(p => p.LubricationId == lubricationId).Select(p => p.ProjectDetailId).ToList<double>();

            //获取润滑记录的备件清单
            return Query<EquipAccountLubricaSparePart>().Where(p => proIds.Contains(p.LubricationProjectId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取设备润滑工时登记列表
        /// </summary>
        /// <param name="lubricationId">润滑记录ID</param>
        [ApiService("获取设备润滑工时登记列表")]
        [return: ApiReturn("设备润滑工时登记列表")]
        public virtual List<LubricationWorkHourInfo> GetLubricationWorkHourInfos([ApiParameter("润滑记录ID")] double lubricationId)
        {
            var workHoursRegisters = Query<LubricationWorkHour>().Where(p => p.LubricationId == lubricationId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            //构建返回数据
            List<LubricationWorkHourInfo> infos = new List<LubricationWorkHourInfo>();
            workHoursRegisters.ForEach(p =>
            {
                infos.Add(new LubricationWorkHourInfo()
                {
                    LubricationWorkHourId = p.Id,
                    EmployeeId = p.ExecutorId,
                    EmployeeName = p.ExecutorName,
                    BeginDay = p.StartDateTime.ToString(),
                    EndDay = p.EndDateTime.ToString(),
                    WorkHours = p.Hours
                });
            });
            return infos;
        }

        /// <summary>
        /// 查看设备润滑图片
        /// </summary>
        /// <param name="lubricationId">润滑记录ID</param>
        /// <returns>设备润滑图片</returns>
        [ApiService("查看设备润滑图片")]
        [return: ApiReturn("设备润滑图片")]
        public virtual EmsAttachmentInfoList GetLubricationAttachmentPhotos([ApiParameter("润滑记录ID")] double lubricationId)
        {
            
            EmsAttachmentInfoList res = new EmsAttachmentInfoList();
            res.AttachmentList = new List<EmsAttachmentInfo>();
            var attachList = Query<LubricationAttachment>().Where(p => p.OwnerId == lubricationId).ToList();
            attachList.ForEach(attach =>
            {
                var tempAttach = new EmsAttachmentInfo()
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
            return res;
        }

        /// <summary>
        /// 生成润滑记录
        /// </summary>
        /// <param name="equipCode">设备编码</param>
        [ApiService("生成润滑记录")]
        public virtual void GenerateLubrication([ApiParameter("设备编码")] string equipCode)
        {
            var equipId = RT.Service.Resolve<Equipments.EquipController>().GetEquipAccountId(equipCode);
            var olds = Query<Lubrication>().Where(p => p.EquipAccountId == equipId && p.LubricationStatus != LubricationStatus.Done).Count();

            if (olds > 0)
            {
                throw new ValidationException("设备[{0}]存在未提交的润滑工单".L10nFormat(equipCode));
            }

            var list = Query<EquipAccountLubricationProject>().Where(p => p.EquipAccountId == equipId).ToList();

            if (!list.Any())
            {
                throw new ValidationException("设备[{0}]未维护润滑项目".L10nFormat(equipCode));
            }

            var groups = list.GroupBy(p => p.DepartmentId);

            List<string> codeList = RT.Service.Resolve<LubricationController>().GetLubricationNo(groups.Count());
            int i = 0;
            var now = RF.Find<Lubrication>().GetDbTime();

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                foreach (var projects in groups)
                {
                    Lubrication model = new Lubrication();
                    model.LubricationNo = codeList[i];
                    i++;

                    model.CycleType = CycleType.Day;
                    model.BillSourceType = BillSourceType.Manual;
                    model.PlanDate = now;
                    model.EquipAccountId = equipId;
                    model.LubricationStatus = LubricationStatus.Pending;
                    model.ApprovalStatus = ApprovalStatus.Draft;
                    model.DepartmentId = projects.Key;

                    RF.Save(model);

                    foreach (var projectDetail in projects)
                    {
                        if (!projectDetail.ProjectCycle.HasValue)
                        {
                            throw new ValidationException("润滑项目【{0}】的周期为空".L10nFormat(projectDetail.ProjectName));
                        }

                        if (!projectDetail.LubricatingType.HasValue)
                        {
                            throw new ValidationException("润滑项目【{0}】的润滑方式为空".L10nFormat(projectDetail.ProjectName));
                        }

                        if (!projectDetail.WarningPeriod.HasValue)
                        {
                            throw new ValidationException("润滑项目【{0}】的预警期为空".L10nFormat(projectDetail.ProjectName));
                        }

                        var detail = new LubricationDetail();
                        detail.LubricationId = model.Id;
                        detail.ProjectCycle = projectDetail.ProjectCycle.Value;
                        detail.WarningPeriod = projectDetail.WarningPeriod.Value.ToString();
                        detail.Part = projectDetail.Part;
                        detail.Consumable = projectDetail.Consumable;
                        detail.Method = projectDetail.Method;
                        detail.Standard = projectDetail.Standard;
                        detail.MinValue = projectDetail.MinValue;
                        detail.MaxValue = projectDetail.MaxValue;
                        detail.LubricatingType = projectDetail.LubricatingType.Value;
                        detail.ProjectDetailId = projectDetail.Id;
                        detail.ProjectName = projectDetail.ProjectName;
                        detail.CycleType = projectDetail.CycleType;
                        detail.Unit = projectDetail.Unit;
                        detail.UseTime = projectDetail.UseTime;
                        RF.Save(detail);
                    }
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 保存或提交润滑记录
        /// </summary>
        /// <param name="info">润滑保存提交信息</param>
        [ApiService("保存或提交润滑记录")]
        public virtual void SaveSubmitLubrications([ApiParameter("润滑保存提交信息")] LubricationSaveSubmitInfo info)
        {
            var lub = RF.GetById<Lubrication>(info.LubricationId);
            if (lub == null)
            {
                throw new ValidationException("润滑记录不存在，[ID:{0}]".L10nFormat(info.LubricationId));
            }
            if (lub.LubricationStatus == LubricationStatus.Done)
            {
                throw new ValidationException("润滑单[{0}]已执行，不允许操作".L10nFormat(lub.LubricationNo));
            }
            if (info.WorkHourDetails.Any(p => p.EmployeeId == null))
            {
                throw new ValidationException("工时登记员工不能为空".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //保存主单数据
                lub.StartDateTime = info.BeginTime;
                lub.EndDateTime = info.EndTime;
                lub.LubricationSummary = info.Implementation;
                var hours = (decimal)Math.Round((info.EndTime - info.BeginTime).Value.TotalMilliseconds / 1000 / 3600);
                lub.TotalHours = hours;
                if (!info.IsSubmit)
                {
                    lub.LubricationStatus = LubricationStatus.Doing;
                    lub.ApprovalStatus = ApprovalStatus.Draft;
                }
                RF.Save(lub);

                //保存润滑项目
                SaveLubricationItems(info);

                //保存备件更换
                SaveLubricationSparePartInfo(info);

                //保存备件申请数据
                SaveSparePartApplyInfo(info);

                //保存工时登记数据
                SaveWorkHoursData(info);

                //保存图片
                var hepler = new FileUrlHelper();
                var attachments = new EntityList<LubricationAttachment>();
                //找出保养单已经存在表里的图片
                var MaintainPlanAttachment = Query<LubricationAttachment>().Where(p => p.OwnerId == info.LubricationId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                var ExitPhotoIds = MaintainPlanAttachment.Select(p => p.Id).ToList();
                var submitPhotoIds = info.Photoes.Where(p => p.Id.HasValue).Select(p => p.Id).ToList();
                if (ExitPhotoIds.Count > 0)
                {
                    var DeleteIds = ExitPhotoIds.Where(x => !submitPhotoIds.Any(a => x == a)).ToList();
                    if (DeleteIds.Count > 0)
                    {
                        DeleteIds.ForEach(P => {
                            DB.Delete<LubricationAttachment>().Where(x => x.Id == P).Execute();
                        });
                    }
                }
                //润滑执行只能保存一张图片 
                info.Photoes.ForEach(p =>
                {
                    if (p.Id == null)
                    {
                        var attachment = hepler.GenerateAttachmentBase64StringContent(new LubricationAttachment(), p.Content, p.FileName) as LubricationAttachment;
                        attachment.OwnerId = info.LubricationId;
                        attachments.Add(attachment);
                    }
                });
                RF.Save(attachments);

                //提交逻辑
                if (info.IsSubmit)
                {
                    LubricationDetailSumbit(lub);
                }
                //提交事务
                trans.Complete();
            }
        }

        /// <summary>
        /// 执行润滑备件更换
        /// </summary>
        /// <param name="info">润滑保存提交信息</param>
        [ApiService("执行润滑备件更换")]
        public virtual void ChangeLubricationSpareParts([ApiParameter("润滑保存提交信息")] LubricationSaveSubmitInfo info)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                this.SaveSubmitLubrications(info);
                ChangeLubricationSparePart(info.LubricationId);
                trans.Complete();
            }
        }

        /// <summary>
        /// 润滑申请备件申请单
        /// </summary>
        /// <param name="info">润滑保存提交信息</param>
        [ApiService("润滑申请备件申请单")]
        public virtual void GenerateSparePartApp([ApiParameter("润滑保存提交信息")] LubricationSaveSubmitInfo info)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                this.SaveSubmitLubrications(info);
                RT.Service.Resolve<SparePartAppController>().GenerateLubricationSparePartApp(info.LubricationId);
                trans.Complete();
            }
        }

        /// <summary>
        /// 保存润滑项目
        /// </summary>
        /// <param name="info">润滑保存提交信息</param>
        private void SaveLubricationItems(LubricationSaveSubmitInfo info)
        {
            foreach (var detail in info.ProjectDetails)
            {
                if (detail.ProjectId <= 0)
                {
                    throw new ValidationException("存在提交的润滑项目ID为0的数据".L10N());
                }
                decimal? valueNull = null;
                var value = detail.ActualValue.IsNullOrEmpty() ? valueNull : decimal.Parse(detail.ActualValue);
                var day = detail.DelayDays.IsNullOrEmpty() ? valueNull : decimal.Parse(detail.DelayDays);
                DB.Update<LubricationDetail>().Where(p => p.Id == detail.ProjectId).Set(p => p.ActualValue, value).Set(p => p.DelayDays, day).Execute();
            }
        }

        /// <summary>
        /// 保存工时登记数据
        /// </summary>
        /// <param name="info">润滑保存提交信息</param>
        private void SaveWorkHoursData(LubricationSaveSubmitInfo info)
        {
            info.WorkHourDetails.ForEach(x =>
            {
                if (x.LubricationWorkHourId <= 0 && x.ActionType != 0)
                {
                    throw new ValidationException("存在非新增的工时登记ID为0的数据".L10N());
                }
                switch (x.ActionType)
                {
                    case 0:
                        {
                            //新增
                            var workHoursRegister = new LubricationWorkHour();
                            workHoursRegister.ExecutorId = x.EmployeeId ?? 0;
                            workHoursRegister.StartDateTime = DateTime.Parse(x.BeginDay);
                            workHoursRegister.EndDateTime = DateTime.Parse(x.EndDay);
                            workHoursRegister.Hours = x.WorkHours;
                            workHoursRegister.LubricationId = info.LubricationId;
                            RF.Save(workHoursRegister);
                            break;
                        }
                    case 1:
                        {
                            //修改
                            DB.Update<LubricationWorkHour>().Where(p => p.Id == x.LubricationWorkHourId)
                                .Set(p => p.ExecutorId, x.EmployeeId)
                                .Set(p => p.StartDateTime, DateTime.Parse(x.BeginDay))
                                .Set(p => p.EndDateTime, DateTime.Parse(x.EndDay))
                                .Set(p => p.Hours, x.WorkHours)
                                .Execute();
                            break;
                        }
                    case 2:
                        {
                            //删除
                            DB.Delete<LubricationWorkHour>().Where(p => p.Id == x.LubricationWorkHourId).Execute();
                            break;
                        }
                    default:
                        break;
                }
            });
        }

        /// <summary>
        /// 保存备件更换
        /// </summary>
        /// <param name="info">润滑保存提交信息</param>
        private void SaveLubricationSparePartInfo(LubricationSaveSubmitInfo info)
        {
            info.SparePartDetails.ForEach(x =>
            {
                if (x.LubricationSparePartId <= 0 && x.ActionType != 0)
                {
                    throw new ValidationException("存在非新增的备件申请项目ID为0的数据".L10N());
                }
                switch (x.ActionType)
                {
                    case 0:
                        {
                            //新增
                            var lubricationSparePart = new LubricationSparePart();
                            lubricationSparePart.SparePartId = x.SparePartId;
                            lubricationSparePart.PartOutDepotDetailId = x.OutDtlId;
                            lubricationSparePart.ChangeQty = x.ChangeQty;
                            lubricationSparePart.Remark = x.Remark;
                            lubricationSparePart.LubricationId = info.LubricationId;
                            RF.Save(lubricationSparePart);
                            break;
                        }
                    case 1:
                        {
                            //修改(已更换的不允许删除)
                            DB.Update<LubricationSparePart>().Where(p => p.Id == x.LubricationSparePartId && p.State == ChangeSparePartState.New)
                                .Set(p => p.SparePartId, x.SparePartId)
                                .Set(p => p.PartOutDepotDetailId, x.OutDtlId)
                                .Set(p => p.ChangeQty, x.ChangeQty)
                                .Set(p => p.Remark, x.Remark)
                                .Execute();
                            break;
                        }
                    case 2:
                        {
                            //删除(已更换的不允许删除)
                            DB.Delete<LubricationSparePart>().Where(p => p.Id == x.LubricationSparePartId && p.State == ChangeSparePartState.New).Execute();
                            break;
                        }
                    default:
                        break;
                }
            });
        }

        /// <summary>
        /// 保存备件申请
        /// </summary>
        /// <param name="info">润滑保存提交信息</param>
        private void SaveSparePartApplyInfo(LubricationSaveSubmitInfo info)
        {
            info.SparePartAplDetails.ForEach(x =>
            {
                if (x.LubricationSparePartId <= 0 && x.ActionType != 0)
                {
                    throw new ValidationException("存在非新增的备件申请项目ID为0的数据".L10N());
                }
                switch (x.ActionType)
                {
                    case 0:
                        {
                            //新增
                            var lubSparePartApl = new LubricationSparePartApply();
                            lubSparePartApl.SparePartId = x.SparePartId;
                            lubSparePartApl.ApplyQty = x.ApplyQty;
                            lubSparePartApl.OutStockWarehouseId = x.OutStockWarehouseId;
                            lubSparePartApl.ApplyDetailId = x.AppDtlId;
                            lubSparePartApl.Remark = x.Remark;
                            lubSparePartApl.LubricationId = info.LubricationId;
                            RF.Save(lubSparePartApl);
                            break;
                        }
                    case 1:
                        {
                            //修改
                            DB.Update<LubricationSparePartApply>().Where(p => p.Id == x.LubricationSparePartId && !p.IsApply)
                                .Set(p => p.SparePartId, x.SparePartId)
                                .Set(p => p.ApplyQty, x.ApplyQty)
                                .Set(p => p.OutStockWarehouseId, x.OutStockWarehouseId)
                                .Set(p => p.ApplyDetailId, x.AppDtlId)
                                .Set(p => p.Remark, x.Remark)
                                .Execute();
                            break;
                        }
                    case 2:
                        {
                            //删除(已申请的不允许删除)
                            DB.Delete<LubricationSparePartApply>().Where(p => p.Id == x.LubricationSparePartId && !p.IsApply).Execute();
                            break;
                        }
                    default:
                        break;
                }
            });
        }

        /// <summary>
        /// 获取上次润滑小结
        /// </summary>
        /// <param name="accountId">设备台账ID</param>
        /// <param name="departmentId">departmentId</param>
        /// <returns></returns>
        [ApiService("获取上次润滑小结")]
        [return: ApiReturn("上次润滑小结 string")]
        public virtual string GetLastLubricationSummary([ApiParameter("设备台账ID")] double accountId, [ApiParameter("部门ID")] double? departmentId)
        {
            return RT.Service.Resolve<LubricationController>().GetLastLubricationSummaryInfo(accountId, departmentId);
        }
    }
}
