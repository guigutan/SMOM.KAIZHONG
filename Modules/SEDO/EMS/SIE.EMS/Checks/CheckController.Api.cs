using SIE.Api;
using SIE.Common.Configs;
using SIE.Core.ApiModels;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.ApiModel;
using SIE.EMS.Checks.ApiModels;
using SIE.EMS.Checks.Configs;
using SIE.EMS.Checks.Confirmations;
using SIE.EMS.Checks.Data;
using SIE.EMS.Checks.Plans;
using SIE.EMS.Checks.Plans.ViewModels;
using SIE.EMS.Checks.Projects;
using SIE.EMS.Common.Utils;
using SIE.EMS.Enums;
using SIE.EMS.Equipments;
using SIE.EMS.Equipments.ApiModels;
using SIE.EMS.EquipRepairs;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Applys.Controllers;
using SIE.EMS.SpareParts.Applys.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Checks
{
    /// <summary>
    /// 点检控制器API 
    /// </summary>
    public partial class CheckController : DomainController
    {
        #region 设备点检
        /// <summary>
        /// 获取设备待点检计划（当天和昨天跨日未超期未点检任务）
        /// </summary>
        /// <param name="equipCode">设备编码</param>
        /// <returns>点检计划列表</returns>
        [ApiService("获取设备待点检计划（当天和昨天跨日未超期未点检任务）")]
        [return: ApiReturn("点检计划列表 EquipCheckPlanInfo")]
        public virtual EquipCheckPlanInfo GetEquipCheckPlanInfo([ApiParameter("设备编码")] string equipCode)
        {
            var code = equipCode.Trim();
            if (code.IsNullOrEmpty())
                throw new ValidationException("设备编码不能为空".L10N());
            var account = RT.Service.Resolve<EquipController>().GetEquipAccount(p => p.Code == code);
            if (account == null)
                throw new ValidationException("设备[{0}]不存在".L10nFormat(code));
            var equipCheckPlanData = new EquipCheckPlanInfo()
            {
                EquipInfo = new EquipInfo()
                {
                    Id = account.Id,
                    Code = account.Code,
                    Process = account.Process?.Name,
                    Name = account.Name,
                    EquipType = account.EquipModel?.EquipType?.TypeName,
                    WorkShop = account.WorkShop?.Name,
                }
            };
            //根据台账获取点检明细,查询(当天和昨天跨日)未超期未点检任务
            var planlist = GetCheckPlans(account.Id);
            var checkPlanList = new List<CheckPlanInfo>();
            planlist.ForEach(e =>
            {
                checkPlanList.Add(new CheckPlanInfo()
                {
                    Id = e.Id,//点检计划id
                    CheckBeginDate = e.CheckBeginDate.ToString(),//计划执行时间
                    CheckCycleType = e.CheckCycleType.ToLabel(),//类型
                    No = e.CheckPlanNo,//点检单号
                    Qty = e.CheckProjectList.Count(),//项目数量
                });
            });

            equipCheckPlanData.CheckPlans.AddRange(checkPlanList);

            return equipCheckPlanData;
        }

        /// <summary>
        /// 获取点检项目列表
        /// </summary>
        /// <param name="checkPlanId">点检计划ID</param>
        /// <returns>点检项目列表</returns>
        [ApiService("获取点检项目列表")]
        [return: ApiReturn("获取点检项目列表 List<CheckProjectInfo>")]
        public virtual List<CheckProjectInfo> GetCheckProjectInfos([ApiParameter("点检计划ID")] double checkPlanId)
        {
            var checkPlan = GetById<CheckPlan>(checkPlanId);
            if (checkPlan == null)
                throw new ValidationException("点检计划不存在！".L10N());
            if (checkPlan.ActCheckBeginDate == null)
                checkPlan.ActCheckBeginDate = RF.Find<CheckPlan>().GetDbTime();
            var detail = GetCheckProjects(p => p.CheckPlanId == checkPlanId);
            if (detail.Any(p => p.EquipAccount == null))
                throw new ValidationException("找不到对应的设备台账，请确认后再操作".L10N());

            RF.Save(checkPlan);
            var orderData = new EntityList<CheckProject>();

            var checkProjectList = new List<CheckProjectInfo>();
            orderData.ForEach(e =>
            {
                var data = new CheckProjectInfo()
                {
                    Id = e.Id,
                    CheckPlanId = e.CheckPlanId,
                    CheckResult = (int?)e.CheckResult,
                    //IsPhoto = (int)e.ProjectDetail.IsPhoto,
                    //MaxValue = e.ProjectDetail?.MaxValue,
                    //MinValue = e.ProjectDetail?.MinValue,
                    //Unit = e.ProjectDetail?.Unit,
                    Remark = e.Remark,
                    //ProjectName = e.ProjectDetail?.Name,
                    AccountName = e.AccountName,
                    Value = (int?)e.ActualValue,
                };

                //有图片则赋值
                if (e.Photo != null)
                {
                    data.Photo = System.Text.Encoding.Default.GetString(e.Photo);
                }

                checkProjectList.Add(data);
            });
            return checkProjectList;
        }

        /// <summary>
        /// 保存点检项目
        /// </summary>
        /// <param name="datas">点检项目列表</param>
        /// <returns>保存成功返回true，失败抛异常</returns> 
        void SaveCheckProject(List<CheckProjectInfo> datas)
        {
            if (datas.Count <= 0)
                throw new ValidationException("点检项目为空".L10N());
            var ids = datas.Select(p => p.Id);
            var dicCheckProject = GetCheckProjects(p => ids.Contains(p.Id)).ToDictionary(p => p.Id);
            var modifyProjects = new EntityList<CheckProject>();
            var photoList = new EntityList<ProjectPhoto>();
            foreach (CheckProjectInfo info in datas)
            {
                if (dicCheckProject.TryGetValue(info.Id, out CheckProject checkProject))
                {
                    var project = checkProject;
                    SetCheckProject(info, project);
                    //有上传图片
                    if (info.Photo.IsNotEmpty())
                    {
                        try
                        {
                            var photo = CreateProjectPhoto(info, project);
                            photoList.Add(photo);
                        }
                        catch
                        {
                            throw new ValidationException("图片字节转换失败".L10N());
                        }
                    }
                    modifyProjects.Add(project);
                }
            }

            RF.Save(photoList);
            RF.Save(modifyProjects);
        }

        /// <summary>
        /// 设置点检项目
        /// </summary>
        /// <param name="info">点检项目信息</param>
        /// <param name="project">点检项目</param>
        private void SetCheckProject(CheckProjectInfo info, CheckProject project)
        {
            project.CheckResult = (CheckMaintainResult?)info.CheckResult;//结果
            project.ActualValue = info.Value;//实际值
            project.Remark = info.Remark;//备注
            project.ExeState = CheckExeState.Performed;//点检状态--已执行
            project.ProjectPhoto = null;
        }

        /// <summary>
        /// 创建项目图片
        /// </summary>
        /// <param name="info">点检项目信息</param>
        /// <param name="project">点检项目</param>
        /// <returns>项目图片</returns>
        private ProjectPhoto CreateProjectPhoto(CheckProjectInfo info, CheckProject project)
        {
            byte[] imageBytes = System.Text.Encoding.Default.GetBytes(info.Photo);
            var photo = new ProjectPhoto();
            photo.GenerateId();
            photo.Photo = imageBytes;
            project.ProjectPhotoId = photo.Id;//图片
            return photo;
        }

        /// <summary>
        /// 提交点检
        /// </summary>
        /// <param name="datas">点检项目列表</param>
        /// <returns>提交成功返回true，失败抛异常</returns>
        [ApiService("提交点检")]
        public virtual void SubmitCheck([ApiParameter("点检项目列表")] List<CheckProjectInfo> datas)
        {
            try
            {
                using (var tran = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
                {
                    SaveCheckProject(datas);

                    var checkPlan = ValidateCheckPlan(datas);
                    checkPlan.ExeState = CheckExeState.Performed;
                    checkPlan.ActCheckEndDate = RF.Find<CheckPlan>().GetDbTime();
                    bool isExistNg = datas.Any(p => p.CheckResult == 0);
                    checkPlan.IsExsitNgProject = isExistNg ? YesNo.Yes : YesNo.No;

                    RF.Save(checkPlan);
                    if (isExistNg)
                    {
                        UpdateAccountState(checkPlan.EquipAccount);
                    }
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
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

        /// <summary>
        /// 验证点检计划信息
        /// </summary>
        /// <param name="datas">点检项目信息列表</param>
        /// <returns>点检计划</returns>
        private CheckPlan ValidateCheckPlan(List<CheckProjectInfo> datas)
        {
            var checkPlan = GetCheckPlan(p => p.Id == datas[0].CheckPlanId);
            if (checkPlan == null)
                throw new ValidationException("未找到点检计划，请检查".L10N());
            if (checkPlan.CheckProjectList.Any(p => p.ExeState != CheckExeState.Performed))
                throw new ValidationException("请点检完所有项目再提交！".L10N());
            return checkPlan;
        }

        #endregion

        #region PCB设备点检-新PDA后台API

        /// <summary>
        /// 获取当前登录用户所属部门，当天所有未点检的检验单信息
        /// </summary>
        /// <param name="pageSize">每页数据量</param>
        /// <param name="pageNumber">页码</param>
        /// <param name="keyword">关键字</param>
        /// <param name="departmentIds">部门ID</param>
        /// <param name="state">状态(0:未执行;2:超期;4:执行中)</param>
        /// <returns></returns>
        [ApiService("获取当前登录用户所属部门，当天对应设备未点检的检验单信息")]
        [return: ApiReturn("点检计划列表 List<CheckPlanInfo>")]
        public virtual CheckPlanData GetNotPerformedCheckPlanInfos(
            [ApiParameter("每页数据量")] int pageSize, [ApiParameter("页码")] int pageNumber, [ApiParameter("关键字")] string keyword,
            [ApiParameter("部门ID集合")] List<double> departmentIds, [ApiParameter("状态")] List<int?> state)
        {
            if (pageSize <= 0)
                throw new ValidationException("[每页数据量]必须大于0".L10N());
            if (pageNumber <= 0)
                throw new ValidationException("[页码]必须大于0".L10N());

            var exeState = state.Select(p => (CheckExeState?)p).ToList();
            //构建分页实体
            var pageInfo = new PagingInfo() { PageSize = pageSize, PageNumber = pageNumber, IsNeedCount = true };

            //根据台账获取点检明细,获取当前登录用户具有权限的部门当天所有未点检的检验单信息
            var planlist = GetNotPerformedCheckPlans(keyword, departmentIds, pageInfo, exeState);

            //构建返回数据结构
            var data = new CheckPlanData();
            data.TotalCount = planlist.TotalCount;
            foreach (var e in planlist)
            {
                var checkplan = e as CheckPlan;

                data.CheckPlanInfos.Add(new CheckPlanInfos()
                {
                    Id = checkplan.Id,                                      //点检计划id
                    CheckBeginDate = checkplan.CheckBeginDate.ToString(),   //计划执行时间
                    CheckEndDate= checkplan.CheckEndDate.ToString(),//计划结束时间
                    CheckCycleType = checkplan.CheckCycleType.ToLabel().L10N(),    //类型
                    No = checkplan.CheckPlanNo,                             //点检单号
                    Qty = checkplan.CheckProjectList.Count(),               //项目数量
                    EquipId = checkplan.EquipAccountId,                     //设备ID 
                    EquipCode = checkplan.EquipAccountCode,                //设备编码
                    EquipName = checkplan.EquipAccountName,                //设备名称
                    DepartmentId = checkplan.DepartmentId,                  //部门ID
                    DepartmentCode = checkplan.DepartmentCode,             //部门编码
                    DepartmentName = checkplan.DepartmentName,              //部门名称
                    State = (int)checkplan.ExeState,
                    StateName = checkplan.ExeState.ToLabel().L10N(),
                    Shop = checkplan.WorkShopName,
                    Line = checkplan.ResourceName,
                    CheckTime = checkplan.CheckTime,
                    EquipTypeId = checkplan.EquipTypeId,
                    EquipTypeCode = checkplan.EquipTypeCode,
                    EquipTypeName = checkplan.EquipTypeName,
                    EquipModelId = checkplan.EquipModelId,
                    EquipModelCode = checkplan.EquipModelCode,
                    EquipModelName = checkplan.EquipModelName,
                    CheckSummary = checkplan.CheckSummary
                });
            }
            return data;
        }

        /// <summary>
        /// 获取上次点检小结
        /// </summary>
        /// <param name="accountId">设备台账ID</param>
        /// <param name="departmentId">departmentId</param>
        /// <returns></returns>
        [ApiService("获取上次点检小结")]
        [return: ApiReturn("上次点检小结 string")]
        public virtual string GetLastCheckSummary([ApiParameter("设备台账ID")] double accountId, [ApiParameter("部门ID")] double? departmentId)
        {
            return RT.Service.Resolve<CheckPlanController>().GetLastCheckSummary(accountId, departmentId);
        }

        /// <summary>
        /// 一键点检
        /// 将最大值与最小值为空的点检项目且设备参数为手动点检或为否，默认点检结果为【合格】
        /// </summary>
        /// <param name="planId">点检计划ID</param>
        /// <returns></returns>
        [ApiService("将最大值与最小值为空的点检项目且设备参数为手动点检或为否，默认点检结果为【合格】")]
        public virtual void QuickCheck([ApiParameter("点检计划ID")] double planId)
        {
            DB.Update<CheckProject>()
                .Set(p => p.ExeState, CheckExeState.Performed)
                .Set(p => p.CheckResult, CheckMaintainResult.OK)
                .Where(p => !p.MaxValue.HasValue && !p.MinValue.HasValue && p.CheckPlanId == planId)
                .Execute();
        }

        /// <summary>
        /// 生成设备点检计划
        /// </summary>
        /// <param name="equipCode">设备编码</param>
        /// <returns></returns>
        [ApiService("生成设备点检计划")]
        [return: ApiReturn("点检计划ID double")]
        public virtual double GenerateCheckPlan([ApiParameter("设备编码")] string equipCode)
        {
            var equipId = RT.Service.Resolve<Equipments.EquipController>().GetEquipAccountId(equipCode);

            AddCheckPlanViewModel model = new AddCheckPlanViewModel();
            model.EquipCheckType = EquipCheckType.Equip;
            model.BeginDate = DateTime.Now.Date;
            model.EndDate = DateTime.Now.Date;
            model.CheckTime = 30;
            model.EquipAccountId = equipId;

            var ctl = RT.Service.Resolve<CheckPlanController>();
            var checkPlan = ctl.AddCheckPlan(model, CheckSourceType.PDA);
            if (checkPlan == null)
                throw new ValidationException("生成点检计划单失败，设备[{0}]在当前时间节点已生成过点检计划。".L10nFormat(equipCode));
            return checkPlan.Id;
        }

        /// <summary>
        /// 生成设备点检计划点检项目
        /// </summary>
        /// <param name="checkPlanId">设备编码</param>
        /// <returns></returns>
        [ApiService("生成设备点检计划点检项目")]
        public virtual void GenerateCheckPlanProject([ApiParameter("计划单ID")] double checkPlanId)
        {
            var ctl = RT.Service.Resolve<CheckPlanController>();
            ctl.GeneratePlanProject(checkPlanId);
        }

        /// <summary>
        /// 获取设备点检计划点检项目
        /// </summary>
        /// <param name="checkPlanId">点检计划单ID</param>
        /// <returns></returns>
        [ApiService("获取设备点检计划点检项目")]
        [return: ApiReturn("设备点检计划点检项目列表 List<CheckPlanProjectInfo>")]
        public virtual List<CheckPlanProjectInfo> GetCheckPlanProjects([ApiParameter("计划单ID")] double checkPlanId)
        {
            var projects = this.GetCheckProjects(checkPlanId);

            //构建返回数据
            List<CheckPlanProjectInfo> infos = new List<CheckPlanProjectInfo>();
            projects.ForEach(p =>
            {
                infos.Add(new CheckPlanProjectInfo()
                {
                    ProjectId = p.Id,
                    ProjectName = p.ProjectName,
                    Part = p.Part,
                    Consumable = p.ProjectConsumable,
                    Method = p.Method,
                    MaxValue = p.MaxValue,
                    MinValue = p.MinValue,
                    ParaCode = p.ParaCode,
                    ParaName = p.ParaName,
                    EquipPara = (int)p.EquipParamSource,
                    EquipParaName = p.EquipParamSource.ToLabel(),
                    ActualValue = p.ActualValue,
                    DefectDesc = p.DefectDesc,
                    Result = p.CheckResult == null ? "" : ((int)p.CheckResult).ToString(),
                });
            });

            return infos;
        }

        /// <summary>
        /// 创建备件申请单
        /// </summary>
        /// <param name="checkPlanId">点检单Id</param>
        /// <param name="sparePartIds">备件Ids</param>
        [ApiService("创建备件申请单")]
        public virtual void CreateApiSelSpareApplyList([ApiParameter("点检单Id")] double checkPlanId, [ApiParameter("备件Ids")] List<double> sparePartIds)
        {
            this.CreateSelSpareApplyList(checkPlanId, sparePartIds);
        }

        /// <summary>
        /// 删除备件申请单
        /// </summary>
        /// <param name="Id"></param>
        [ApiService("删除备件申请单")]
        public virtual void DelSelSpareApply([ApiParameter("申请单Id")] double Id)
        {
            this.DeleteSelSpareApply(Id);
        }

        /// <summary>
        /// 创建备件更换单
        /// </summary>
        /// <param name="checkPlanId">点检单Id</param>
        /// <param name="sparePartIds">备件Ids</param>
        [ApiService("创建备件更换单")]
        public virtual void CreateApiSelSpareChangeList([ApiParameter("点检单Id")] double checkPlanId, [ApiParameter("备件Ids")] List<double> sparePartIds)
        {
            this.CreateSelSpareChangeList(checkPlanId, sparePartIds);
        }

        /// <summary>
        /// 删除备件更换单
        /// </summary>
        /// <param name="Id"></param>
        [ApiService("删除备件更换单")]
        public virtual void DelSelSpareChange([ApiParameter("更换单Id")] double Id)
        {
            this.DeleteSelSpareChange(Id);
        }

        /// <summary>
        /// 获取设备点检计划备件更换项目
        /// </summary>
        /// <param name="checkPlanId">点检计划单ID</param>
        /// <returns></returns>
        [ApiService("获取设备点检计划备件更换项目")]
        [return: ApiReturn("设备点检计划备件更换项目列表 List<CheckSparePartInfo>")]
        public virtual List<CheckSparePartInfo> GetCheckSpareParts([ApiParameter("计划单ID")] double checkPlanId)
        {
            var checkSpareParts = this.GetCheckPlanSpareParts(checkPlanId);

            //构建返回数据
            List<CheckSparePartInfo> infos = new List<CheckSparePartInfo>();
            checkSpareParts.ForEach(p =>
            {
                int? state = null;
                infos.Add(new CheckSparePartInfo()
                {
                    SparePartId = p.SparePartId,
                    SparePartCode = p.SparePart.SparePartCode,
                    SparePartName = p.SparePart.SparePartName,
                    OutDtlId = p.PartOutDepotDetailId,
                    ChangeQty = p.ChangeQty,
                    Remark = p.Remark,
                    CheckSparePartId = p.Id,
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
        /// 获取设备点检计划备件申请项目
        /// </summary>
        /// <param name="checkPlanId">点检计划单ID</param>
        /// <returns></returns>
        [ApiService("获取设备点检计划备件申请项目")]
        [return: ApiReturn("设备点检计划备件申请项目列表 List<CheckSparePartInfo>")]
        public virtual List<CheckSparePartAplInfo> GetCheckSparePartApls([ApiParameter("计划单ID")] double checkPlanId)
        {
            var checkSparePartApls = this.GetCheckPlanSparePartApls(checkPlanId);

            //构建返回数据
            List<CheckSparePartAplInfo> infos = new List<CheckSparePartAplInfo>();
            checkSparePartApls.ForEach(p =>
            {
                int? state = null;
                infos.Add(new CheckSparePartAplInfo()
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
                    CheckSparePartId = p.Id,
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
        /// 获取设备物联参数实时值
        /// </summary>
        /// <param name="equipCode">设备编码</param>
        /// <param name="dtlIds">点检项目ID</param>
        /// <returns></returns>
        [ApiService("获取设备物联参数实时值")]
        [return: ApiReturn("获取设备物联参数实时值 List<EquipEapRTValueInfoData>")]
        public virtual List<EquipEapRTValueInfoData> GetPhysicalUnionRealValue([ApiParameter("设备编码")] string equipCode, [ApiParameter("点检项目ID")] List<double> dtlIds)
        {
            var para = new CheckPlanEapData();
            para.EquipmentCode = equipCode;
            para.ProjectDetailIds = dtlIds;
            var rtn = RT.Service.Resolve<CheckPlanController>().GetProjectRealTimeData(para);
            if (rtn.IsSuccess)
                return rtn.Data;
            else
                throw new ValidationException(rtn.Error.Message);
        }

        /// <summary>
        /// 获取EDO首页数据
        /// </summary>
        /// <returns></returns>
        [ApiService("获取EDO首页数据")]
        [return: ApiReturn("EDO首页数据 EdoFrontPageInfo")]
        public virtual EdoFrontPageInfo GetFrontPageInfo()
        {
            var info = new EdoFrontPageInfo();
            return info;
        }

        /// <summary>
        /// 保存或提交检验单
        /// </summary>
        /// <param name="info"></param>
        [ApiService("保存或提交检验单")]
        public virtual void SaveSubmitCheckPlans([ApiParameter("点检保存提交信息")] CheckSaveSubmitInfo info)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                if (info.CheckPlanId <= 0) throw new ValidationException("点检单ID不正确".L10N());
                //防并发行锁
                DB.Update<CheckPlan>().Where(p => p.Id == info.CheckPlanId).Set(p => p.UpdateBy, RT.IdentityId).Execute();
                var plan = RF.GetById<CheckPlan>(info.CheckPlanId);

                if (plan == null)
                {
                    throw new ValidationException("点检单不存在，[ID:{0}]".L10nFormat(info.CheckPlanId));
                }

                if (plan.ExeState != CheckExeState.NotPerformed && plan.ExeState != CheckExeState.Performing)
                {
                    throw new ValidationException("点检单[{0}]是[{1}]状态，不允许保存"
                        .L10nFormat(plan.CheckPlanNo, plan.ExeState.ToLabel()));
                }

                //保存逻辑
                RT.Service.Resolve<CheckPlanController>().SaveCheckPlan(info, plan);

                //提交逻辑
                if (info.IsSubmit)
                {
                    var checkPlan = Query<CheckPlan>().Where(p => p.Id == info.CheckPlanId).FirstOrDefault();
                    checkPlan.IsAbnormalInfoPush = info.IsAbnormalInfoPush;
                    RT.Service.Resolve<CheckPlanController>().SubmitCheckPlan(checkPlan);
                }

                //提交事务
                trans.Complete();
            }
        }

        /// <summary>
        /// 执行点检备件更换
        /// </summary>
        /// <param name="info"></param>
        [ApiService("执行点检备件更换")]
        public virtual void ChangeCheckSpareParts([ApiParameter("计划单参数实体")] CheckSaveSubmitInfo info)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RT.Service.Resolve<CheckPlanController>().SaveCheckPlanChangeInfo(info);
                RT.Service.Resolve<CheckPlanController>().ChangeCheckPlanSparePart(info.CheckPlanId);
                trans.Complete();
            }
        }

        /// <summary>
        /// 点检申请备件申请单
        /// </summary>
        /// <param name="info"></param>
        [ApiService("点检申请备件申请单")]
        public virtual void GenerateSparePartApp([ApiParameter("计划单参数实体")] CheckSaveSubmitInfo info)
        {
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RT.Service.Resolve<CheckPlanController>().SaveCheckPlanApplyInfo(info);
                RT.Service.Resolve<SparePartAppController>().GenerateCheckSparePartApp(info.CheckPlanId);
                trans.Complete();
            }
        }

        /// <summary>
        /// 查看图片
        /// </summary>
        /// <param name="checkPlanId">计划单ID</param>
        /// <returns></returns>
        [ApiService("查看图片")]
        public virtual EmsAttachmentInfoList GetCheckPlanAttachmentPhotos([ApiParameter("计划单ID")] double checkPlanId)
        {            
            EmsAttachmentInfoList res = new EmsAttachmentInfoList();
            res.AttachmentList = new List<EmsAttachmentInfo>();
            // 前端只返回图片格式的数据
            var exts = new List<string> { ".png", ".jpg", ".jpeg", ".bmp", ".gif", ".webp", ".psd", ".svg", ".tiff", ".jfif" };
            var q = Query<CheckPlanAttachment>().Where(p => p.OwnerId == checkPlanId && exts.Contains(p.FileExtesion));

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
        /// 获取当前登录用户所属部门所有需要点检确认的检验单信息
        /// </summary>
        /// <param name="pageSize">每页数据量</param>
        /// <param name="pageNumber">页码</param>
        /// <param name="equipCode">设备编码</param>
        /// <param name="departmentId">部门ID</param>
        /// <returns></returns>
        [ApiService("获取当前登录用户所属部门对应设备需要点检确认的检验单信息")]
        [return: ApiReturn("点检计划列表 List<CheckPlanInfo>")]
        public virtual CheckPlanData GetNotConfirmedCheckPlanInfos(
            [ApiParameter("每页数据量")] int pageSize,
            [ApiParameter("页码")] int pageNumber,
            [ApiParameter("设备编码")] string equipCode,
            [ApiParameter("确认部门ID")] double? departmentId)
        {
            if (pageSize <= 0) { throw new ValidationException("[每页数据量]必须大于0".L10N()); }
            if (pageNumber <= 0) { throw new ValidationException("[页码]必须大于0".L10N()); }

            //构建分页实体
            var pageInfo = new PagingInfo() { PageSize = pageSize, PageNumber = pageNumber, IsNeedCount = true };

            //根据台账获取点检明细,获取当前登录用户所属部门所有未点检的检验单信息
            var planlist = RT.Service.Resolve<CheckPlanController>().GetNotConfirmedCheckPlans(equipCode, departmentId, pageInfo);

            //构建返回数据结构
            var data = new CheckPlanData();
            data.TotalCount = planlist.Count();

            var reuslt = (planlist.Skip((pageNumber - 1) * pageSize).Take(pageSize)).ToList();
            reuslt.ForEach(e =>
            {
                data.CheckPlanInfos.Add(e);
            });

            return data;
        }

        /// <summary>
        /// 获取某个点检计划的点检确认单信息
        /// </summary>
        /// <param name="checkPlanId">点检计划ID</param>
        /// <param name="confirmDeptId">部门ID</param>
        /// <returns></returns>
        [ApiService("获取某个点检计划指定确认部门的点检确认单信息")]
        [return: ApiReturn("点检计划 CheckPLan")]
        public virtual EntityList<CheckConfirmation> GetCheckPlanConfirmations(
            [ApiParameter("点检计划ID")] double checkPlanId,
            [ApiParameter("确认部门ID")] double confirmDeptId)
        {
            #region 验证计划单
            var checkPlans = Query<CheckPlan>().Where(p => p.Id == checkPlanId && p.ExeState == CheckExeState.NotConfirm).ToList();
            if (checkPlans.Count == 0)
            {
                throw new ValidationException((checkPlanId.ToString() + "不是有效的计划ID或者执行状态不是待确认状态").L10N());
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

            var checkPLan = RT.Service.Resolve<CheckPlanController>().GetCheckPlanConfirmations(checkPlanId, department.Id, null, null);

            return checkPLan;
        }

        /// <summary>
        /// 获取是否评分配置项
        /// </summary>
        /// <returns></returns>
        [ApiService("获取是否评分配置项")]
        public virtual bool GetIsNeedScore()
        {
            var needScoreConfig = ConfigService.GetConfig<CheckConfirmDepartConfigValue>(new CheckConfirmDepartConfig(), typeof(CheckPlanViewModel));
            bool isNeedScore = false;
            if (needScoreConfig != null)
            {
                isNeedScore = needScoreConfig.IsMarkScore;
            }
            return isNeedScore;
        }

        /// <summary>
        /// 提交点检确认单
        /// </summary>
        /// <param name="info">点检确认提交信息</param>
        /// <param name="planInfo">点检计划信息</param>
        [ApiService("提交点检确认单")]
        public virtual void SubmitCheckConfirmation([ApiParameter("点检确认评分项信息")] CheckConfirmationSubmitInfo[] info, [ApiParameter("点检计划信息")] CheckPlanInfo planInfo)
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
            bool isNeedScore = IsNeedMark();

            // 验证信息
            CheckConfirmSubmitValidate(info, isNeedScore);

            // 数据库时间
            var dbDate = RF.Find<CheckPlan>().GetDbTime();

            // 评分项
            EntityList<CheckConfirmation> checkConfirmations = new EntityList<CheckConfirmation>();
            if (isNeedScore)
            {
                CheckConfirmSubmitMark(info, planInfo, checkConfirmations, dbDate);
            }

            // 更新点检确认结果、备注以及确认项
            var plan = RF.GetById<CheckPlan>(planInfo.Id);
            EntityList<CheckPlanConfirmItem> checkPlanConfirmItems = new EntityList<CheckPlanConfirmItem>();
            CheckConfirmSubmitCheckItem(plan, planInfo, dbDate);

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                //防并发行锁
                DB.Update<CheckPlan>().Where(p => p.Id == planInfo.Id).Set(p => p.UpdateBy, RT.IdentityId).Set(p => p.UpdateDate, RF.Find<CheckPlan>().GetDbTime()).Execute();

                // 更新评分项
                if (checkConfirmations.Any()) 
                {
                    DB.Delete<CheckConfirmation>().Where(p => p.OwnerId == planInfo.Id && p.ConfirmDeptId == planInfo.ConfirmDeptId).Execute();
                    RF.Save(checkConfirmations);
                }

                // 更新执行状态为已完成，若启用评分则为已评分。
                CheckConfirmUpdateState(isNeedScore, plan, planInfo, checkPlanConfirmItems);
                RF.Save(plan);
                RF.Save(checkPlanConfirmItems);

                //更新备件更换记录标记
                if (plan.ExeState == CheckExeState.Scored)
                    RT.Service.Resolve<SparePartController>().UpdateSparePartChangedRecordFlag(FromType.SpotCheck, plan.Id);

                //提交事务
                trans.Complete();
            }
        }

        /// <summary>
        /// 判断当前点检单是否有维修单
        /// </summary>
        /// <param name="equipAccountId"></param>
        /// <param name="sourceNo"></param>
        /// <returns></returns>
        [ApiService("判断当前点检单是否有维修单")]
        public virtual bool CheckPlanWithRepairBill([ApiParameter("点检设备id")] double equipAccountId, [ApiParameter("点检单号")] string sourceNo)
        {
            return Query<EquipRepairBill>().Where(p => p.EquipAccountId == equipAccountId && p.SourceNo == sourceNo).Count() > 0;
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
            List <SparepartWareInfo> sparepartWareInfos = new List<SparepartWareInfo>();
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
                foreach(var ware in wareInfos)
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