using SIE.Api;
using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Core.ApiModels;
using SIE.Core.Common.Controllers;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.ApiModel;
using SIE.EMS.Checks.ApiModels;
using SIE.EMS.Checks.Plans;
using SIE.EMS.Checks.Projects;
using SIE.EMS.Common.Utils;
using SIE.EMS.Devices.Abnormals;
using SIE.EMS.Devices.Abnormals.ApiModels;
using SIE.EMS.Enums;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.Equipments.Boms;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.EMS.EquipRepair.EquipRepairs.Configs;
using SIE.EMS.EquipRepair.EquipRepairs.Enums;
using SIE.EMS.EquipRepairs.Enums;
using SIE.EMS.InventoryTasks.ApiModels;
using SIE.EMS.Logs;
using SIE.EMS.MainenanceProjects;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.ApiModels;
using SIE.EMS.SpareParts.Applys.Enums;
using SIE.EMS.SpareParts.OutDepots;
using SIE.EMS.SpareParts.OutDepots.Details;
using SIE.EMS.SpareParts.OutDepots.Enums;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.EquipRepair.Controller
{
    /// <summary>
    /// 离线点检控制器(兼容维修所以在此工程下)
    /// </summary>
    public class CheckOfflineController : DomainController
    {
        /// <summary>
        /// 获取离线未执行的点检计划
        /// </summary>
        /// <returns></returns>
        [ApiService("获取离线未执行的点检计划")]
        public virtual List<CheckPlanInfos> GetNotPerformedCheckPlanInfosOffline()
        {
            List<CheckPlanInfos> checkPlanInfos = new List<CheckPlanInfos>();
            var nowDate = DateTime.Now;
            var pageInfo = new PagingInfo { PageNumber = 1, PageSize = 99999, IsNeedCount = true };
            var checkPlanQuery = Query<CheckPlan>().Where(p => p.ExeState == Enums.CheckExeState.NotPerformed && p.CheckBeginDate <= nowDate).ToList(pageInfo, new EagerLoadOptions().LoadWithViewProperty());
            foreach (var checkPlan in checkPlanQuery)
            {
                var state = checkPlan.CheckEndDate < nowDate ? CheckExeState.Overdue : CheckExeState.NotPerformed;
                CheckPlanInfos planInfo = new CheckPlanInfos
                {
                    CheckPlanId = checkPlan.Id,
                    No = checkPlan.CheckPlanNo,
                    EquipId = checkPlan.EquipAccountId,
                    EquipCode = checkPlan.EquipAccountCode,
                    EquipName = checkPlan.EquipAccountName,
                    EquipModelId = checkPlan.EquipModelId,
                    EquipTypeId = checkPlan.EquipTypeId,
                    EquipUseDeptId = checkPlan.EquipUseDeptId,
                    EquipUseDeptName = checkPlan.EquipUseDeptName,
                    State = (int)state,
                    StateName = state.ToLabel().L10N(),
                    Shop = checkPlan.WorkShopName,
                    Line = checkPlan.ResourceName,
                    CheckBeginDate = checkPlan.CheckBeginDate.ToString(),
                    CheckEndDate = checkPlan.CheckEndDate.ToString(),
                    CheckPlanType = (int)checkPlan.CheckPlanType,
                    RFID = checkPlan.RFID,
                    CheckSummary = checkPlan.CheckSummary,
                };
                checkPlanInfos.Add(planInfo);
            }
            return checkPlanInfos;
        }

        /// <summary>
        /// 获取点检计划设备的点检项目
        /// </summary>
        /// <returns></returns>
        [ApiService("获取点检计划设备的点检项目")]
        public virtual List<CheckPlanProjectInfo> GetCheckEquipProjectOffline()
        {
            List<CheckPlanProjectInfo> checkPlanProjectInfos = new List<CheckPlanProjectInfo>();
            var pageInfo = new PagingInfo { PageNumber = 1, PageSize = 99999, IsNeedCount = true };
            var equipProjectQuery = Query<EquipAccountCheckProject>().Exists<CheckPlan>((x, y) => y.Where(p => x.EquipAccountId == p.EquipAccountId)).ToList(pageInfo, new EagerLoadOptions().LoadWithViewProperty());
            foreach (var project in equipProjectQuery)
            {
                CheckPlanProjectInfo checkPlanProjectInfo = new CheckPlanProjectInfo
                {
                    EquipAccountId = project.EquipAccountId,
                    ProjectId = project.Id,
                    ProjectName = project.ProjectName,
                    CycleType = (int)project.CycleType,
                    Part = project.Part,
                    Consumable = project.Consumable,
                    Standard = project.Standard,
                    IsPhoto = 0,
                    Method = project.Method,
                    MinValue = project.MinValue,
                    MaxValue = project.MaxValue,
                    Unit = project.Unit,
                    UseTime = project.UseTime,
                };
                checkPlanProjectInfos.Add(checkPlanProjectInfo);
            }
            return checkPlanProjectInfos;
        }

        /// <summary>
        /// 获取设备异常信息
        /// </summary>
        /// <returns></returns>
        [ApiService("获取设备异常信息")]
        public virtual List<AbnormalBaseInfo> GetDeviceAbnormalsOffline()
        {
            var q = Query<DeviceAbnormal>()
                .Where(p => p.AbnormalType == AbnormalType.Unusual)
                .Select(p => new
                {
                    DeviceAbnormalId = p.Id,
                    EquipTypeId = p.EquipTypeId,
                    Code = p.Code
                }).ToList<AbnormalBaseInfo>().ToList();
            return q;
        }

        /// <summary>
        /// 获取备件BOM和备件信息
        /// </summary>
        /// <returns></returns>
        [ApiService("获取备件BOM和备件信息")]
        public virtual List<SparePartInfo> GetSparePartInfosOffline()
        {
            List<SparePartInfo> sparePartInfos = new List<SparePartInfo>();
            // 获取备件BOM
            var spareBomList = Query<EquipBomDetail>()
                .LeftJoin<EquipBom>((ebd, eb) => ebd.EquipBomId == eb.Id)
                .LeftJoin<EquipBom, Core.Equipments.EquipModel>((eb, em) => eb.EquipModelId == em.Id)
                .LeftJoin<SparePart>((ebd, sp) => ebd.SparePartId == sp.Id)
                .LeftJoin<SparePart, Unit>((sp, u) => sp.UnitId == u.Id)
                .LeftJoin<SparePart, ItemCategory>((sp, ic) => sp.ItemCategoryId == ic.Id)
                .Exists<CheckPlan>((ebd, cp) => cp.LeftJoin<Core.Equipments.EquipAccount>((pl, ea) => pl.EquipAccountId == ea.Id).Where<EquipBom, Core.Equipments.EquipAccount>((pl, eb, ea) => ea.EquipModelId == eb.EquipModelId))
                .Select<EquipBom, SparePart, Unit, ItemCategory, Core.Equipments.EquipModel>((ebd, eb, sp, u, ic, em) => new
                {
                    SparePartId = ebd.SparePartId,
                    SparePartCode = sp.SparePartCode,
                    SparePartName = sp.SparePartName,
                    SparePartUnit = u.Name,
                    SparePartSpecification = sp.Specification,
                    SparePartTypeName = ic.Name,
                    EquipModelId = em.Id,
                    EquipModelCode = em.Code,
                    EquipModelName = em.Name,
                    Manufacturer = sp.Manufacturer,
                }).ToList<SparePartInfo>().ToList();


            // 去重
            var existsSpareIds = spareBomList.Select(p => p.SparePartId).ToList();
            // 获取备件信息
            var spareList = Query<SparePart>()
                .LeftJoin<Core.Equipments.EquipModel>((sp, em) => sp.SpartEquipModelId == em.Id)
                .LeftJoin<Unit>((sp, u) => sp.UnitId == u.Id)
                .LeftJoin<ItemCategory>((sp, ic) => sp.ItemCategoryId == ic.Id)
                .Where(sp => !existsSpareIds.Contains(sp.Id))
                .Select<Core.Equipments.EquipModel, Unit, ItemCategory>((sp, em, u, ic) => new
                {
                    SparePartId = sp.Id,
                    SparePartCode = sp.SparePartCode,
                    SparePartName = sp.SparePartName,
                    SparePartUnit = u.Name,
                    SparePartSpecification = sp.Specification,
                    SparePartTypeName = ic.Name,
                    EquipModelId = em.Id,
                    EquipModelCode = em.Code,
                    EquipModelName = em.Name,
                    Manufacturer = sp.Manufacturer,
                }).ToList<SparePartInfo>().ToList();
            // 获取备件图片信息
            List<double> spareIds = spareList.Select(s => s.SparePartId).ToList();
            spareIds.AddRange(existsSpareIds);
            IList<EmsAttachmentInfo> pictures = RT.Service.Resolve<SparePartController>().GetSparePartPictureAttachments(spareIds);
            spareBomList.ForEach(sp =>
            {
                sp.IsBom = 1;
                var picInfo = pictures.FirstOrDefault(p => p.OwnerId == sp.SparePartId);
                if (picInfo != null)
                {
                    var picBase64Str = FileUrlHelper.GetAttachmentBase64StringData(picInfo.FilePath, picInfo.FileName);
                    sp.PhotoBase64 = picBase64Str;
                    sp.FileExtension = picInfo.FileExtension;
                }
            });
            spareList.ForEach(sp =>
            {
                sp.IsBom = 0;
                var picInfo = pictures.FirstOrDefault(p => p.OwnerId == sp.SparePartId);
                if (picInfo != null)
                {
                    var picBase64Str = FileUrlHelper.GetAttachmentBase64StringData(picInfo.FilePath, picInfo.FileName);
                    sp.PhotoBase64 = picBase64Str;
                    sp.FileExtension = picInfo.FileExtension;
                }
            });

            sparePartInfos.AddRange(spareBomList);
            sparePartInfos.AddRange(spareList);
            return sparePartInfos;
        }

        /// <summary>
        /// 获取出库单信息
        /// </summary>
        /// <returns></returns>
        [ApiService("获取出库单信息")]
        public virtual List<SparePartOutInfo> GetSparePartOutInfoOffline()
        {
            List<SparePartOutInfo> sparePartOutInfos = new List<SparePartOutInfo>();
            var query = Query<PartOutDepotDetail>()
                .LeftJoin<OutDepot>((odd, od) => odd.OutDepotId == od.Id)
                .LeftJoin<OutDepot, Core.Equipments.EquipAccount>((od, eq) => od.EquipAccountId == eq.Id)
                .Where<OutDepot>((odd, od) => od.OutDepotType != OutDepotType.Pucharse && od.OutDepotType != OutDepotType.DgMaintain && od.OutDepotType != OutDepotType.Scrap)
                .LeftJoin<SparePart>((odd, sp) => odd.SparePartId == sp.Id)
                .Where(odd => odd.UseCount < odd.OutDepotCount && odd.OutboundStatus == OutboundStatus.Shipped)
                .Select<OutDepot, Core.Equipments.EquipAccount, SparePart>((odd, od, eq, sp) => new
                {
                    OutDtlId = odd.Id,
                    LineNo = odd.LineNo,
                    OutDepotNo = od.No,
                    EquipId = od.EquipAccountId,
                    EquipCode = eq.Code,
                    EquipName = eq.Name,
                    OutEquipModelId = od.EquipModelId,
                    SparePartId = odd.SparePartId,
                    SparePartCode = sp.SparePartCode,
                    SparePartName = sp.SparePartName,
                    SparePartSpecification = sp.Specification,
                    SourceNo = od.SourceNo,
                    SeriaNo = odd.SeriaNo,
                    SeriaNoId = odd.SeriaNoRefId,
                    BatchNo = odd.BatchNo,
                    BatchNoId = odd.BatchNoRefId,
                    RemainingQty = odd.OutDepotCount - odd.UseCount
                }).ToList<SparePartOutInfo>();
            sparePartOutInfos.AddRange(query);
            return sparePartOutInfos;
        }

        /// <summary>
        /// 创建备件更换信息
        /// </summary>
        /// <param name="checkPlanOfflineSubmits"></param>
        private void CreateSpareChgData(List<CheckPlanOfflineSubmit> checkPlanOfflineSubmits)
        {
            var sparePartChgList = checkPlanOfflineSubmits.SelectMany(p => p.CheckSpareChgList).ToList();
            if (sparePartChgList == null || sparePartChgList.Count <= 0)
            {
                return;
            }
            // 备件更换数据
            EntityList<CheckPlanSparePart> saveChgList = new EntityList<CheckPlanSparePart>();
            // 插入备件履历
            EntityList<SparePartChangedRecord> saveRecordList = new EntityList<SparePartChangedRecord>();
            foreach (var chg in sparePartChgList)
            {
                var checkPlan = checkPlanOfflineSubmits.Select(p => p.CheckPlanInfo).FirstOrDefault(p => p.CheckPlanId == chg.CheckPlanId);
                //回写申请单
                DB.Update<PartOutDepotDetail>().Set(x => x.UseCount, x => x.UseCount + chg.ChangeQty).Where(p => p.Id == chg.OutDtlId).Execute();
                //修改序列号状态
                DB.Update<StoreSummaryDetail>().Where(x => x.Id == chg.SeriaNoId).Set(x => x.StoreStatus, OrdNumStoreStatus.Using).Execute();
                //创建备件更换数据
                CheckPlanSparePart spareChg = new CheckPlanSparePart
                {
                    CheckPlanId = chg.CheckPlanId,
                    SparePartId = chg.SparePartId,
                    State = ChangeSparePartState.Finished,
                    PartOutDepotDetailId = chg.OutDtlId,
                    ChangeQty = chg.ChangeQty,
                };
                saveChgList.Add(spareChg);
                //创建备件履历(并更新备件更换记录标记)
                SparePartChangedRecord record = new SparePartChangedRecord()
                {
                    EquipAccountId = checkPlan.EquipId,
                    Qty = chg.ChangeQty,
                    BatchNumber = chg.BatchNo,
                    SerialNumber = chg.SeriaNo,
                    Source = FromType.SpotCheck,
                    SourceNo = checkPlan.No,
                    SourceId = checkPlan.CheckPlanId,
                    SparePartId = chg.SparePartId,
                    IsSourceCompleted = true,
                };
                saveRecordList.Add(record);
            }
            RT.Service.Resolve<CommonController>().BatchInsertSave(saveChgList); // 备件更换数据
            RT.Service.Resolve<CommonController>().BatchInsertSave(saveRecordList); // 备件履历数据
        }

        /// <summary>
        /// 创建维修信息
        /// </summary>
        /// <param name="checkPlanOfflineSubmits">提交信息</param>
        /// <param name="equipStateInfo">设备状态信息</param>
        /// <param name="repairNoRuleId">维修单号编码规则</param>
        public virtual void CreateRepairData(List<CheckPlanOfflineSubmit> checkPlanOfflineSubmits, List<BaseDataIntInfo> equipStateInfo, double? repairNoRuleId)
        {
            // 维修数据
            var repairList = checkPlanOfflineSubmits.Where(p => p.RepairInfo != null).Select(p => p.RepairInfo).ToList();
            if (repairList == null || repairList.Count <= 0)
            {
                return;
            }
            // 维修图片信息
            var repairPicList = checkPlanOfflineSubmits.SelectMany(p => p.RepairPictureList).ToList();
            var repairNo = RT.Service.Resolve<NumberRuleController>().GenerateSegment(repairNoRuleId.Value, repairList.Count).ToList();
            var hepler = new FileUrlHelper();
            var now = RF.Find<EquipRepairBill>().GetDbTime();
            var employeeId = RT.IdentityId;
            // 保存设备维修
            EntityList<EquipRepairBill> savRepairList = new EntityList<EquipRepairBill>();
            // 保存设备履历
            EntityList<EquipAccountResume> saveResumeList = new EntityList<EquipAccountResume>();
            // 保存设备附件
            EntityList<EquipRepairAttachment> saveAttachments = new EntityList<EquipRepairAttachment>();
            // 保存维修单操作记录
            EntityList<EquipRepairOperationRec> saveRecords = new EntityList<EquipRepairOperationRec>();

            for (var i = 0; i < repairList.Count; i++)
            {
                var repair = repairList[i];
                var checkPlan = checkPlanOfflineSubmits.FirstOrDefault(p => p.CheckPlanInfo.CheckPlanId == repair.CheckPlanId);
                var equipState = equipStateInfo.FirstOrDefault(p => p.Id == checkPlan.CheckPlanInfo.EquipId);
                var picList = repairPicList.Where(p => p.CheckPlanId == checkPlan.CheckPlanInfo.CheckPlanId);
                EquipRepairBill repairBill = new EquipRepairBill
                {
                    RepairNo = repairNo[i],
                    RepairState = EquipRepairState.ApplyRepair,
                    SourceNo = checkPlan.CheckPlanInfo.No,
                    SourceType = RepairSourceType.Check,
                    RepairType = EquipRepairType.EquipRepair,
                    EquipAccountId = checkPlan.CheckPlanInfo.EquipId,
                    ProduceState = (ProduceState)repair.ProduceState,
                    UrgentDegree = (UrgentDegree)repair.UrgentDegree,
                    DeviceAbnormalId = repair.AbnormalId,
                    DeviceAbnormalCode = repair.AbnormalCode,
                    DeviceAbnormalRemark = repair.AbnormalDesc,
                    ApplyRepairDate = now,
                    ApplyRepairEmployeeId = employeeId,
                };
                repairBill.GenerateId();
                savRepairList.Add(repairBill);

                EquipAccountResume resume = new EquipAccountResume
                {
                    EquipAccountId = checkPlan.CheckPlanInfo.EquipId,
                    ResumeType = ResumeType.CallRepair,
                    No = repairBill.RepairNo,
                    State = (AccountState)equipState.Value,
                };
                saveResumeList.Add(resume);

                foreach (var pic in picList)
                {
                    var attachment = hepler.GenerateAttachmentBase64StringContent(new EquipRepairAttachment(), pic.Src, pic.FileName) as EquipRepairAttachment;
                    attachment.OwnerId = repairBill.Id;
                    saveAttachments.Add(attachment);
                }

                EquipRepairOperationRec record = new EquipRepairOperationRec
                {
                    OperationType = RepairOperationType.ApplyRepair,
                    EquipRepairBillId = repairBill.Id,
                    OperationDate = now,
                    OperationerId = RT.IdentityId,
                };
                saveRecords.Add(record);

                if (repairBill.ProduceState == ProduceState.StopWork) // 停机
                {
                    DB.Update<SIE.Core.Equipments.EquipAccount>().Where(p => p.Id == repairBill.EquipAccountId).Set(p => p.State, Core.Enums.AccountState.Fault).Execute();
                }
            }

            RT.Service.Resolve<CommonController>().BatchInsertSave(savRepairList); // 维修单
            RF.Save(saveAttachments); // 维修图片
            RT.Service.Resolve<CommonController>().BatchInsertSave(saveResumeList); // 设备履历
            RT.Service.Resolve<CommonController>().BatchInsertSave(saveRecords); // 维修单操作记录
            // (to do 推送消息方法改为允许批量)
        }

        /// <summary>
        /// 更新点检计划信息
        /// </summary>
        /// <param name="checkPlanOfflineSubmits">提交信息</param>
        /// <param name="equipStateInfo">设备状态信息</param>
        private void UpdateCheckPlanData(List<CheckPlanOfflineSubmit> checkPlanOfflineSubmits, List<BaseDataIntInfo> equipStateInfo)
        {
            // 点检计划
            var planList = checkPlanOfflineSubmits.Select(p => p.CheckPlanInfo).ToList();
            // 点检项目
            var planProjectList = checkPlanOfflineSubmits.SelectMany(p => p.CheckPlanProList).ToList();
            // 维修信息
            var repairList = checkPlanOfflineSubmits.Where(p => p.RepairInfo != null).Select(p => p.RepairInfo).ToList();
            // 点检图片
            var planPicList = checkPlanOfflineSubmits.SelectMany(p => p.CheckPlanPictureList).ToList();
            var hepler = new FileUrlHelper();

            if (planList == null || planList.Count <= 0 || planProjectList == null || planProjectList.Count <= 0)
            {
                return;
            }

            // 保存设备履历
            EntityList<EquipAccountResume> saveResumeList = new EntityList<EquipAccountResume>();
            // 保存点检项目
            EntityList<CheckProject> saveCheckProjectList = new EntityList<CheckProject>();
            // 保存点检图片
            EntityList<CheckPlanAttachment> attachments = new EntityList<CheckPlanAttachment>();
            var nowDateTime = RF.Find<CheckPlan>().GetDbTime();
            planList.ForEach(plan =>
            {
                var exeResult = planProjectList.Where(p => p.CheckPlanId == plan.CheckPlanId).All(p => p.Result == "1") ? ExeResult.Successed : ExeResult.Failed;
                DB.Update<CheckPlan>()
                .Set(p => p.ExeState, CheckExeState.Performed)
                .Set(p => p.CheckSummary, plan.CheckSummary)
                .Set(p => p.CheckDate, nowDateTime)
                .Set(p => p.ExeResult, exeResult)
                .Where(p => p.Id == plan.CheckPlanId).Execute();

                // 判断该设备是否有存在停机维修,状态变更为停机
                var equipState = equipStateInfo.FirstOrDefault(p => p.Id == plan.EquipId);
                var sameEquipCheckPlanIds = checkPlanOfflineSubmits.Select(p => p.CheckPlanInfo).Where(x => x.EquipId == plan.EquipId).Select(x => x.CheckPlanId).ToList();
                var hasRepair = repairList.Count > 0 && repairList.Any(p => sameEquipCheckPlanIds.Contains(p.CheckPlanId) && p.ProduceState == 0);
                EquipAccountResume resume = new EquipAccountResume
                {
                    EquipAccountId = plan.EquipId,
                    No = plan.No,
                    ResumeType = ResumeType.Checked,
                    State = hasRepair ? AccountState.Fault : (AccountState)equipState.Value,
                };
                saveResumeList.Add(resume);

                // 创建点检项目
                var currentProjectList = planProjectList.Where(p => p.CheckPlanId == plan.CheckPlanId).ToList();
                foreach (var project in currentProjectList)
                {
                    CheckProject checkProject = new CheckProject
                    {
                        CheckPlanId = project.CheckPlanId,
                        EquipAccountId = plan.EquipId,
                        EquipCheckProjectId = project.ProjectId,
                        ProjectName = project.ProjectName,
                        EquipParamSource = EquipParamSource.NoValue,
                        CycleType = (CycleType)project.CycleType,
                        Part = project.Part,
                        ProjectConsumable = project.Consumable,
                        Standard = project.Standard,
                        //IsPhoto = project.IsPhoto == 1 ? YesNo.Yes : YesNo.No,
                        Method = project.Method,
                        MinValue = project.MinValue,
                        MaxValue = project.MaxValue,
                        Unit = project.Unit,
                        UseTime = project.UseTime,
                        ActualValue = project.ActualValue,
                        DefectDesc = project.DefectDesc,
                        CheckResult = project.Result == "1" ? CheckMaintainResult.OK : CheckMaintainResult.NG,
                        ExeState = CheckExeState.Performed,
                    };
                    saveCheckProjectList.Add(checkProject);
                }

                // 获取图片信息
                var picList = planPicList.Where(p => p.CheckPlanId == plan.CheckPlanId).ToList();
                foreach (var pic in picList)
                {
                    var attachment = hepler.GenerateAttachmentBase64StringContent(new CheckPlanAttachment(), pic.Src, pic.FileName) as CheckPlanAttachment;
                    attachment.OwnerId = plan.CheckPlanId;
                    attachments.Add(attachment);
                }
            });
            RT.Service.Resolve<CommonController>().BatchInsertSave(saveCheckProjectList); // 点检项目
            RT.Service.Resolve<CommonController>().BatchInsertSave(saveResumeList); // 设备履历
            RF.Save(attachments); // 点检图片
        }

        /// <summary>
        /// 提交离线点检单据
        /// </summary>
        /// <param name="checkPlanOfflineSubmits">离线点检信息</param>
        /// <returns></returns>
        [ApiService("提交离线点检单据")]
        public virtual List<UploadLoadReturnData> SubmitOffLineCheckPlan([ApiParameter("离线点检信息")] List<CheckPlanOfflineSubmit> checkPlanOfflineSubmits)
        {
            List<UploadLoadReturnData> uploadLoadReturnDatas = new List<UploadLoadReturnData>();
            EntityList<EdoOutlineUploadLog> edoOutlineUploadLogs = new EntityList<EdoOutlineUploadLog>();
            var config = ConfigService.GetConfig(new EquipRepairNoConfig(), typeof(EquipRepairBill));
            // 校验出库数量是否大于可用数量
            var outDtlIds = checkPlanOfflineSubmits.SelectMany(p => p.CheckSpareChgList).Select(p => p.OutDtlId).Distinct().ToList();
            var outDtlList = outDtlIds.SplitContains(temps => { return Query<PartOutDepotDetail>().Where(p => temps.Contains(p.Id)).ToList(); });
            // 校验不通过的计划Id
            List<double> failPlanIds = new List<double>();
            foreach (var plan in checkPlanOfflineSubmits)
            {
                try
                {
                    if (plan.RepairInfo != null && (config == null || config.NumberRule == null))
                    {
                        throw new ValidationException("未找到设备维修申请单号生成规则,请检查规则配置");
                    }
                    if (plan.CheckSpareChgList.Count > 0)
                    {
                        var chgList = plan.CheckSpareChgList.GroupBy(p => p.OutDtlId).Select(p => new BaseDataDecimalInfo { Id = p.Key, Value = p.Sum(x => x.ChangeQty) }).ToList();
                        chgList.ForEach(item =>
                        {
                            var chg = plan.CheckSpareChgList.FirstOrDefault(p => p.OutDtlId == item.Id);
                            var outDtl = outDtlList.FirstOrDefault(p => p.Id == item.Id);
                            if (outDtl == null)
                            {
                                throw new ValidationException("出库单明细【Id:{0}】不存在".FormatArgs(item.Id));
                            }
                            if (item.Value + outDtl.UseCount > outDtl.OutDepotCount)
                            {
                                throw new ValidationException("出库单【No:{0}】更换数量大于剩余可用数量".FormatArgs(chg.OutDepotNo));
                            }
                        });
                    }

                    //成功回传
                    UploadLoadReturnData returnData = new UploadLoadReturnData
                    {
                        BillNo = plan.CheckPlanInfo.No,
                        IsSuccess = true,
                    };
                    uploadLoadReturnDatas.Add(returnData);

                    EdoOutlineUploadLog edoOutlineUploadLog = new EdoOutlineUploadLog
                    {
                        BillNo = plan.CheckPlanInfo.No,
                        MachineCode = plan.CheckPlanInfo.EquipCode,
                        MachineName = plan.CheckPlanInfo.EquipName,
                        UploadState = UploadState.Success,
                        UploadType = UploadType.Check,

                    };
                    edoOutlineUploadLogs.Add(edoOutlineUploadLog);
                }
                catch (Exception ex)
                {
                    failPlanIds.Add(plan.CheckPlanInfo.CheckPlanId);
                    UploadLoadReturnData returnData = new UploadLoadReturnData
                    {
                        BillNo = plan.CheckPlanInfo.No,
                        IsSuccess = false,
                        FailReason = ex.Message,
                    };
                    uploadLoadReturnDatas.Add(returnData);

                    EdoOutlineUploadLog edoOutlineUploadLog = new EdoOutlineUploadLog
                    {
                        BillNo = plan.CheckPlanInfo.No,
                        MachineCode = plan.CheckPlanInfo.EquipCode,
                        MachineName = plan.CheckPlanInfo.EquipName,
                        UploadState = UploadState.Fail,
                        FailReason = ex.Message,
                        UploadType = UploadType.Check,

                    };
                    edoOutlineUploadLogs.Add(edoOutlineUploadLog);
                }
            }

            // 通过验证
            var passCheckPlanList = checkPlanOfflineSubmits.Where(p => !failPlanIds.Contains(p.CheckPlanInfo.CheckPlanId)).ToList();
            var repairNoRuleId = config.NumberRuleId;
            // 设备状态信息
            var equipIds = passCheckPlanList.Select(p => p.CheckPlanInfo).Select(p => p.EquipId).ToList();
            List<BaseDataIntInfo> equipStateInfo = new List<BaseDataIntInfo>();
            using (SIE.DataAuth.DataAuths.LoadAll())
            {
                equipIds.SplitDataExecute(temps =>
                {
                    var list = Query<SIE.Core.Equipments.EquipAccount>().Where(p => temps.Contains(p.Id)).Select(p => new
                    {
                        Id = p.Id,
                        Value = (int)p.State
                    }).ToList<BaseDataIntInfo>();
                    equipStateInfo.AddRange(list);
                });
            }

            // 创建备件更换信息并计算出库信息
            // 创建维修信息
            // 上传维修图片
            // 更新点检单状态为已执行，更新点检小结
            // 创建点检项目
            // 上传点检图片
            using (var tran = DB.TransactionScope(EquipRepairEntityDataProvider.ConnectionStringName))
            {
                CreateSpareChgData(passCheckPlanList);
                CreateRepairData(passCheckPlanList, equipStateInfo, repairNoRuleId);
                UpdateCheckPlanData(passCheckPlanList, equipStateInfo);
                RT.Service.Resolve<CommonController>().BatchInsertSave(edoOutlineUploadLogs);
                tran.Complete();
            }
            return uploadLoadReturnDatas;
        }
    }
}
