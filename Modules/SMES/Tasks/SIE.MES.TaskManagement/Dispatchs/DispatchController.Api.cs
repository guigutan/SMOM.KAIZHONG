using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using Microsoft.Scripting.Utils;
using NPOI.SS.Formula.Functions;
using NPOI.SS.Formula.PTG;
using SIE.Andon.Andons;
using SIE.Api;
using SIE.Barcodes.Configs;
using SIE.Barcodes.WipBatchs;
using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Core.ApiModels;
using SIE.Core.Common.Service;
using SIE.Core.Labels;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.EquipAccounts;
using SIE.Items;
using SIE.MES.Checker;
using SIE.MES.Common;
using SIE.MES.FeedingIntervals;
using SIE.MES.Fixture;
using SIE.MES.ItemChecker;
using SIE.MES.ItemEquipAccount;
using SIE.MES.ItemEquipAccount.Configs;
using SIE.MES.ItemFixture;
using SIE.MES.LineAndon;
using SIE.MES.PackingQC;
using SIE.MES.ReworkLayoutVersions;
using SIE.MES.ReworkLayoutVersions.Configs;
using SIE.MES.TaskManagement.Dispatchs.Datas;
using SIE.MES.TaskManagement.FeedingRecords;
using SIE.MES.TaskManagement.Models;
using SIE.MES.TaskManagement.PreStartupSetupRecords;
using SIE.MES.TaskManagement.ProcessPrepareRecords;
using SIE.MES.TaskManagement.SuspectProductLabels;
using SIE.MES.WIP.Runtime;
using SIE.MES.WorkOrders;
using SIE.MES.WorkOrders.Configs;
using SIE.Packages.ItemLabels;
using SIE.Packages.ItemLabels.Configs;
using SIE.ProductIntfc.OutputProducts;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using SIE.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Versioning;
using EquipAccount = SIE.Equipments.EquipAccounts.EquipAccount;
using ItemLabel = SIE.Packages.ItemLabels.ItemLabel;

namespace SIE.MES.TaskManagement.Dispatchs
{
    public partial class DispatchController : DomainController
    {
        /// <summary>
        /// 主菜单数量显示
        /// </summary>
        /// <returns></returns>
        [ApiService("主菜单数量显示")]
        public virtual PdaGetMenuQuantity GetMenuQuantity()
        {
            PdaGetMenuQuantity menuQuantity = new PdaGetMenuQuantity();
            //计算安灯管理
            menuQuantity.andon_manager = Query<AndonManage>().Where(p => p.State == SIE.Andon.Andons.Enum.AndonManageState.Standby || p.State == SIE.Andon.Andons.Enum.AndonManageState.Processing || p.State == SIE.Andon.Andons.Enum.AndonManageState.ToAccepted).Count();
            //计算安灯响应
            menuQuantity.andon_response = Query<AndonManage>().Where(p => p.State == SIE.Andon.Andons.Enum.AndonManageState.Standby).Count();
            //计算上料采集
            menuQuantity.load_items_selection = Query<DispatchTask>().Join<EmployeeResource>((x, y) => x.ResourceId == y.ResourceId && y.EmployeeId == RT.IdentityId).Exists<TaskProcessBom>((x, y) => y.Where(p => p.DispatchTaskId == x.Id)).Where(p => p.IsFeedingClose == false || p.IsFeedingClose == null).Where(p => p.TaskStatus == DispatchTaskStatus.Executing || p.TaskStatus == DispatchTaskStatus.Dispatched).Count();
            //计算产前准备
            menuQuantity.production_prepare_index = Query<ProcessPrepareRecord>().Join<DispatchTask>((x, y) => x.DispatchTaskId == y.Id).Join<DispatchTask, EmployeeResource>((x, y) => x.ResourceId == y.ResourceId && y.EmployeeId == RT.IdentityId).Where(p => p.PrepareState == PrepareProducts.Enums.PrepareRecordState.ToConfirm).Count();
            //计算可疑品处理
            menuQuantity.suspect_product = Query<SuspectProductLabel>().Where(p => p.Qty - p.GoodQty - p.ScrapQty - p.RepairQty > 0).Count();
            //装箱QC确认
            menuQuantity.packing_qc_confirm = Query<PackingQc>()
                //排除工单关闭的
                .NotExists<PackingDetail>((x, y) => y.Join<WorkOrder>((x1, y1) => x1.WorkOrderNo == y1.No && y1.State == Core.WorkOrders.WorkOrderState.Close)
                .Where(p => p.PackingQcId == x.Id))
                .Where(p => p.Confirm == ConfirmEnum.NO || p.Confirm == null).Count();
            //手动报工(任务单为首工序的，首工序的顺序一般是为10)
            menuQuantity.manual_report_index = Query<DispatchTask>().Join<EmployeeResource>((x, y) => x.ResourceId == y.ResourceId && y.EmployeeId == RT.IdentityId).Where(p => p.TaskStatus == DispatchTaskStatus.Executing || p.TaskStatus == DispatchTaskStatus.Dispatched).Join<WorkOrderRoutingProcess>((x, y) => x.WorkOrderId == y.WorkOrderId && x.ProcessId == y.ProcessId && y.Index == 10).Count();
            //扫码报工(任务单不为首工序的，首工序的顺序一般是为10)
            menuQuantity.barcode_report_select = Query<DispatchTask>().Join<EmployeeResource>((x, y) => x.ResourceId == y.ResourceId && y.EmployeeId == RT.IdentityId).Where(p => p.TaskStatus == DispatchTaskStatus.Executing || p.TaskStatus == DispatchTaskStatus.Dispatched).Join<WorkOrderRoutingProcess>((x, y) => x.WorkOrderId == y.WorkOrderId && x.ProcessId == y.ProcessId && y.Index != 10).Count();
            //开机准备
            menuQuantity.power_on_pre_index = GetPreStartupSetupToolTaskInfos(null).Count + GetPreStartupSetupEquipAccountTaskInfos(null).Count;
            //副产品收货
            menuQuantity.output_product = RT.Service.Resolve<OutputProductController>().GetOutputProductInfosCount();

            return menuQuantity;
        }

        /// <summary>
        /// 下拉获取机台(生产资源)
        /// </summary>
        /// <returns></returns>
        [ApiService("上下料:下拉获取机台(生产资源)")]
        public virtual List<Pda_WipResourceInfo> AssemblyGetWipResources(string key)
        {
            List<Pda_WipResourceInfo> infos = new List<Pda_WipResourceInfo>();
            var q = Query<EmployeeResource>().Where(p => p.EmployeeId == RT.IdentityId);
            if (!key.IsNullOrEmpty())
                q.Where(p => p.Resource.Code.Contains(key) || p.Resource.Name.Contains(key));
            var employeeResources = q.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            foreach (var employeeResource in employeeResources)
            {
                infos.Add(new Pda_WipResourceInfo()
                {
                    ResourceId = employeeResource.ResourceId,
                    Code = employeeResource.ResourceCode,
                    Name = employeeResource.ResourceName
                });
            }
            return infos;
        }

        #region 返工确认

        /// <summary>
        /// 返工确认:获取扫码信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [ApiService("返工确认:获取扫码信息")]
        public virtual PdaReworkConfirmScanInfo GetReworkConfirmScanInfo(string key)
        {
            var first = RT.Service.Resolve<WipBatchController>().GetWipBatch(key);
            if (first == null)
                throw new ValidationException("标签{0}不存在".L10nFormat(key));
            if (first.IsRework == false)
                throw new ValidationException("标签{0}非返工标签".L10nFormat(key));

            ReworkInfoRecordDtl reworkInfoRecordDtls = RT.Service.Resolve<ReworkLayoutVersionController>().GetReworkInfoRecordDtl(key);
            if (reworkInfoRecordDtls != null)
            {
                if (reworkInfoRecordDtls.ReworkInfoRecord.ProductOrder.IsNullOrEmpty())
                {
                    throw new ValidationException("标签已扫描过，不允许重复扫描，生产订单还未创建".L10N());
                }
                else
                {
                    throw new ValidationException("标签已扫描过，不允许重复扫描，生产订单{0}成功创建并下达".L10nFormat(reworkInfoRecordDtls.ReworkInfoRecord.ProductOrder));
                }
            }
                

            PdaReworkConfirmScanInfo info = new PdaReworkConfirmScanInfo();

            info.ProductCode = first.ProductCode;
            info.ProductName = first.ProductName;
            info.WipBatchId = first.Id;
            info.Sn = first.BatchNo;
            info.SnQty = first.Qty;

            return info;
        }

        /// <summary>
        /// 返工确认:获取返工工艺路线版本
        /// </summary>
        /// <param name="ProductCode"></param>
        /// <returns></returns>
        [ApiService("返工确认:获取返工工艺路线版本")]
        public virtual List<PdaReworkLayoutVersionInfo> GetPdaReworkLayoutVersionInfos(string ProductCode)
        {
            var versions = Query<ReworkLayoutVersion>().Where(p => p.Item.Code == ProductCode && (p.EffEndDateTime == null || p.EffEndDateTime >= DateTime.Now)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            List<PdaReworkLayoutVersionInfo> infos = new List<PdaReworkLayoutVersionInfo>();

            foreach (var version in versions)
            {
                PdaReworkLayoutVersionInfo info = new PdaReworkLayoutVersionInfo();

                info.VersionId = version.Id;
                info.Version = version.Version;
                info.Desc = version.Desc;

                infos.Add(info);
            }

            return infos;
        }

        /// <summary>
        /// 返工确认:获取返工工艺路线工序信息s
        /// </summary>
        /// <param name="VersionId"></param>
        /// <returns></returns>
        [ApiService("返工确认:获取返工工艺路线工序信息")]
        public virtual List<PdaReworkLayoutInfo> GetPdaReworkLayoutInfos(double VersionId)
        {
            List<PdaReworkLayoutInfo> infos = new List<PdaReworkLayoutInfo>();
            var version = RF.GetById<ReworkLayoutVersion>(VersionId, new EagerLoadOptions().LoadWithViewProperty());
            foreach (var detail in version.ReworkLayoutList)
            {
                PdaReworkLayoutInfo info = new PdaReworkLayoutInfo();
                info.Vornr = detail.Vornr;
                info.ProcessCode = detail.ProcessCode;
                info.WorkCenterCode = detail.WorkCenterCode;
                info.Steus = detail.Steus;
                info.Factory = detail.Factory;
                info.Zcode = detail.Zcode;
                infos.Add(info);
            }
            return infos;
        }

        /// <summary>
        /// 返工确认:提交
        /// </summary>
        /// <param name="data"></param>
        [ApiService("返工确认:提交")]
        public virtual void SubmitReworkLayoutInfo(PdaSubmitReworkLayoutInfo data)
        {
            var curTime = RF.Find<ReworkInfoRecord>().GetDbTime();
            var config = ConfigService.GetConfig(new ReworkInfoRecordEntityConfig(), typeof(ReworkInfoRecord));
            if (config == null || config.NumberRuleId == null || config.NumberRuleId == 0)
                throw new ValidationException("未配置返工信息唯一码编码规则".L10N());
            //获取返工工艺路线信息
            var version = RF.GetById<ReworkLayoutVersion>(data.VersionId, new EagerLoadOptions().LoadWithViewProperty());

            var wipBatchs = RT.Service.Resolve<WipBatchController>().GetWipBatches(data.WipBatchIds);

            ReworkInfoRecord reworkInfoRecord = new ReworkInfoRecord();
            reworkInfoRecord.State = ReworkInfoRecordState.Create;
            reworkInfoRecord.Factory = version.Factory;
            reworkInfoRecord.ItemId = version.ItemId;
            reworkInfoRecord.Qty = data.ReworkQty;
            reworkInfoRecord.BeginDateTime = curTime; //data.BeginDateTime;
            reworkInfoRecord.EndDateTime = curTime;//data.EndDateTime;
            reworkInfoRecord.ReworkLayoutVersionId = version.Id;
            reworkInfoRecord.Department = data.Department;
            reworkInfoRecord.UniqueCode = RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.BacodeRule.Id, 1).FirstOrDefault();//GetUniqueShortGuid(Guid.NewGuid());
            foreach (var wipBatch in wipBatchs)
            {
                ReworkInfoRecordDtl reworkInfoRecordDtl = new ReworkInfoRecordDtl();
                reworkInfoRecordDtl.WipBatchId = wipBatch.Id;
                reworkInfoRecordDtl.ReworkInfoRecord = reworkInfoRecord;
                reworkInfoRecord.ReworkInfoRecordDtlList.Add(reworkInfoRecordDtl);
            }
            RF.Save(reworkInfoRecord);
        }

        /// <summary>
        /// 生成唯一的24位缩短Guid（URL安全，无损转换，100%唯一）
        /// </summary>
        public static string GetUniqueShortGuid(Guid guid)
        {

            // Base64中可能的特殊字符替换（URL安全，不影响唯一性）
            char Base64Plus = '+';
            char Base64Slash = '/';
            char SafePlus = '-';
            char SafeSlash = '_';

            // 步骤1：将Guid转为16字节二进制数组（无损，保留全部128位信息）
            byte[] guidBytes = guid.ToByteArray();

            // 步骤2：转为Base64字符串（24位，无损，唯一映射）
            string base64 = Convert.ToBase64String(guidBytes);

            // 步骤3：替换特殊字符（仅字符替换，不改变二进制本质，仍唯一）
            string safeBase64 = base64.Replace(Base64Plus, SafePlus)
                                      .Replace(Base64Slash, SafeSlash);

            // 最终长度：24位（≤25位），且100%唯一
            return safeBase64;
        }

        #endregion

        #region 余料称重

        /// <summary>
        /// 余料称重:扫描标签
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        [ApiService("余料称重:扫描标签")]
        public virtual PdaSWRecordScanInfo GetPdaSWRecordScanInfo(string sn)
        {
            var itemLabel = Query<ItemLabel>().Where(p => p.Exidv == sn || p.Exidv2 == sn || p.Label == sn).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (itemLabel == null)
                throw new ValidationException("物料标签{0}不存在".L10nFormat(sn));
            if (itemLabel.ItemLabelState != ItemLabelState.Blanking && itemLabel.ItemLabelState != ItemLabelState.Feeding)
                throw new ValidationException("物料标签{0}不为上料、下料状态".L10nFormat(sn));

            var record = Query<FeedingRecord>().Where(p => p.ItemLabelId == itemLabel.Id).OrderByDescending(p => p.CreateDate).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (record == null)
                throw new ValidationException("物料标签{0}不存在,上料记录".L10nFormat(sn));

            PdaSWRecordScanInfo info = new PdaSWRecordScanInfo();
            info.Sn = sn;
            info.ItemLabelId = itemLabel.Id;
            info.FeedingRecordId = record.Id;
            info.Lot = itemLabel.Lot;
            info.ItemCode = itemLabel.ItemCode;
            info.ItemName = itemLabel.ItemName;
            info.ItemLabelState = itemLabel.ItemLabelState?.ToLabel();
            info.FeedingQty = record.FeedingQty ?? 0;
            info.BlankingQty = record.BlankingQty ?? 0;
            info.RemainingQty = itemLabel?.Qty ?? 0;//record.BlankingQty ?? 0;
            info.ItemUnit = itemLabel.Item.Unit.Name;
            return info;
        }

        /// <summary>
        /// 余料称重:提交
        /// </summary>
        /// <param name="datas"></param>
        [ApiService("余料称重:提交")]
        public virtual void SubmitPdaSWRecordScanInfo(List<PdaSWRecordScanInfo> datas)
        {
            if (datas.Count < 1)
                throw new ValidationException("没有提交数据".L10N());
            if (datas.Any(p => p.ActualQty == null))
                throw new ValidationException("实际称重数量不能为空".L10N());
            EntityList<ScrapWeighingRecord> scraps = new EntityList<ScrapWeighingRecord>();
            //获取上料状态的物料标签
            var itemLabelIds = datas.Select(p => p.ItemLabelId).Distinct().ToList();
            var itemLabels = Query<ItemLabel>().Where(p => itemLabelIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //获取上料状态的上料记录
            var feedingRecordIds = datas.Select(p => p.FeedingRecordId).Distinct().ToList();
            var records = Query<FeedingRecord>().Where(p => feedingRecordIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //根据标签获取扣料记录
            var deductions = Query<DeductionRecord>().Where(p => itemLabelIds.Contains(p.ItemLabelId)).ToList();

            foreach (var data in datas)
            {
                //创建余料称重记录
                ScrapWeighingRecord scrap = new ScrapWeighingRecord();
                scrap.Sn = data.Sn;
                scrap.ItemLabelId = data.ItemLabelId;
                scrap.FeedingRecordId = data.FeedingRecordId;
                scrap.ActualQty = data.ActualQty.Value;
                scrap.RemainingQty = 0;
                scrap.PersistenceStatus = PersistenceStatus.New;
                scrap.RemainingQty = data.RemainingQty;
                scraps.Add(scrap);

                //当状态为上料时，将上料记录的剩余数量更新为0。
                var record = records.FirstOrDefault(p => p.Id == data.FeedingRecordId);
                if (record != null && data.ItemLabelState == ItemLabelState.Feeding.ToLabel())
                {
                    record.BlankingQty = record.RemainingQty;
                    record.RemainingQty = 0;
                    record.PersistenceStatus = PersistenceStatus.Modified;

                    scrap.RemainingQty = record.BlankingQty ?? 0;
                }

                //当状态为上料时，将物料标签的状态更新为下料；
                var itemLabel = itemLabels.FirstOrDefault(p => p.Id == data.ItemLabelId);
                if (itemLabel != null)
                {
                    if (data.ItemLabelState == ItemLabelState.Feeding.ToLabel())
                    {
                        itemLabel.ItemLabelState = ItemLabelState.Blanking;
                        itemLabel.PersistenceStatus = PersistenceStatus.Modified;
                    }
                    //当填写的实际称重数量小于理论剩余数量时，将物料标签的可用数量更新为实际称重数量，并且记录数据，该数据需要通过扣料接口上传至SAP：
                    if (data.ActualQty < data.RemainingQty)
                    {
                        itemLabel.Qty = data.ActualQty.Value;

                        var deduction = deductions.Where(p => p.ItemLabelId == itemLabel.Id).OrderByDescending(p => p.CreateDate).FirstOrDefault();

                        if (deduction == null)
                            throw new ValidationException("未找到标签[{0}]的相关扣料记录".L10nFormat(data.Sn));
                        scrap.DispatchTaskId = deduction.ReportRecord.DispatchTaskId;
                        scrap.ResourceId = deduction.ResourceId;
                        scrap.DeductedQty = data.RemainingQty - data.ActualQty;
                    }
                }

                scrap.DiffQty = scrap.ActualQty - scrap.RemainingQty;
            }
            using (var tran = DB.TransactionScope(TaskManagementEntityDataProvider.ConnectionStringName))
            {
                if (scraps.Count > 0)
                    RF.Save(scraps);
                if (records.Count > 0)
                    RF.Save(records);
                if (itemLabels.Count > 0)
                    RF.Save(itemLabels);
                tran.Complete();
            }
        }

        #endregion

        #region 上料

        /// <summary>
        /// 根据资源获取派工单
        /// </summary>
        /// <param name="resourId"></param>
        [ApiService("上料:根据资源获取派工单")]
        public virtual List<Pda_AssemblyGetTaskInfo> AssemblyGetTasks(double ResourceId, string key)
        {
            List<Pda_AssemblyGetTaskInfo> taskInfos = new List<Pda_AssemblyGetTaskInfo>();

            var resource = RF.GetById<WipResource>(ResourceId, new EagerLoadOptions().LoadWithViewProperty());
            if (resource == null)
                throw new ValidationException("未找到该资源!".L10N());
            var resourceIds = new List<double>();

            if (resource.SourceType == SyncSourceType.LineAndon)
            {
                //当选择的是产线的时候，就把产线上面的工作中心，找出来，然后把工作中心下的全部产线对应的任务单都带出来
                var andon = Query<AndonLine>().Where(p => p.MachineCode == resource.Code).FirstOrDefault();
                if (andon == null)
                    throw new ValidationException("产线与安灯区域未维护[{0}]的产线/机台编码数据".L10nFormat(resource.Code));
                //resourceIds = Query<WipResource>().Join<EmployeeResource>((x, y) => x.Id == y.ResourceId && y.EmployeeId == RT.IdentityId).Join<AndonLine>((x, y) => x.Code == y.MachineCode).Join<AndonLine, WorkCenter>((x, y) => x.WorkCenterId == y.Id && y.Code == andon.WorkCenter.Code).Select(p => p.Id).ToList<double>().ToList();
                var wip = Query<WipResource>().Where(p => p.Code == andon.WorkCenter.Code).FirstOrDefault();
                if (wip == null)
                    throw new ValidationException("未找到工作中心编码[{0}]的生产资源".L10nFormat(andon.WorkCenter.Code));
                resourceIds.Add(wip.Id);
                resourceIds.Add(ResourceId);
            }
            else
            {
                var employeeResources = RT.Service.Resolve<EmployeeController>().GetEmployeeResources(RT.IdentityId, ResourceId);
                if (employeeResources == null)
                    throw new ValidationException("该资源不在该员工的资源列表中".L10N());
                resourceIds.Add(ResourceId);
            }

            //默认按派工单创建时间倒序展示当前机台非已完成、非已关闭且当前工单+工序在工单工序BOM中有数据的任务单；
            var q = DB.Query<DispatchTask>("TASK")
                .Join<WorkOrder>("WO", (x, y) => x.WorkOrderId == y.Id)
                .Exists<WorkOrderProcessBom>((x, y) => y.Where(p => p.WorkOrderId == x.WorkOrderId && p.ProcessId == x.ProcessId))
                .Where(p => resourceIds.Contains((double)p.ResourceId) && (p.TaskStatus == DispatchTaskStatus.Executing || p.TaskStatus == DispatchTaskStatus.Dispatched) && (p.IsFeedingClose == null || p.IsFeedingClose == false) /*&& p.SQL<bool>("nvl((SELECT SUM(nvl(FEEDING_RECORD.Remaining_Qty,0)) FROM FEEDING_RECORD WHERE FEEDING_RECORD.IS_PHANTOM = 0 AND FEEDING_RECORD.Dispatch_Task_Id = TASK.ID),0) < (TASK.Single_Qty * WO.Plan_Qty)")*/
            );

            if (!key.IsNullOrEmpty())
                q.Where(p => p.WorkOrder.No.Contains(key) || p.No.Contains(key) || p.Product.ShortDescription.Contains(key));

            var tasks = q.OrderBy(p => p.PlanBeginTime).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            foreach (var task in tasks)
            {
                Pda_AssemblyGetTaskInfo taskInfo = new Pda_AssemblyGetTaskInfo();
                taskInfo.TaskId = task.Id;
                taskInfo.TaskNo = task.No;
                taskInfo.ProductCode = task.ProductCode;
                taskInfo.ProductName = task.ProductName;
                taskInfo.Qty = task.DispatchQty;
                taskInfo.WorkOrderNo = task.WorkOrderNo;
                taskInfo.Process = task.ProcessCode;
                taskInfo.OldItemCode = task.ShortDescription;
                taskInfos.Add(taskInfo);
            }

            return taskInfos;
        }

        /// <summary>
        /// 上料:根据获取工序BOM
        /// </summary>
        /// <param name="TaskId"></param>
        [ApiService("上料:根据获取工序BOM")]
        public virtual List<Pda_AssemblyProcessBomInfo> AssemblyGetProcessBomInfos(double TaskId)
        {
            List<Pda_AssemblyProcessBomInfo> infos = new List<Pda_AssemblyProcessBomInfo>();
            //获取工序BOM
            var processBoms = Query<WorkOrderProcessBom>().Join<DispatchTask>((x, y) => x.WorkOrderId == y.WorkOrderId && x.ProcessId == y.ProcessId && y.Id == TaskId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var itemIds = processBoms.Select(p => p.ItemId).Distinct().ToList();

            var task = RF.GetById<DispatchTask>(TaskId, new EagerLoadOptions().LoadWithViewProperty());

            //供料区域
            var areIds = new List<double?>();
            var areRes = Query<FeedingAreaReource>().Where(p => p.ResourceId == task.ResourceId && p.FeedingArea.State == State.Enable).ToList();
            areIds.AddRange(areRes.Select(p => (double?)p.FeedingAreaId).ToList());

            var records = Query<FeedingRecord>().Where(p => (p.ResourceId == task.ResourceId || areIds.Contains(p.FeedingAreaId)) && itemIds.Contains(p.ItemLabel.ItemId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            var workOrderBomList = Query<WorkOrderBom>().Where(p => p.WorkOrderId == task.WorkOrderId).ToList();

            foreach (var processBom in processBoms)
            {
                var requireQty = workOrderBomList.Where(p => p.ItemId == processBom.ItemId).Max(p => p.RequireQty);
                var rds = records.Where(p => p.ItemId == processBom.ItemId).ToList();
                Pda_AssemblyProcessBomInfo info = new Pda_AssemblyProcessBomInfo();
                info.ProcessBomId = processBom.Id;
                info.ProductCode = processBom.ItemCode;
                info.ProductName = processBom.ItemName;
                info.Qty = task.DispatchQty * processBom.SingleQty;//processBom.SingleQty * processBom.PlanQty;
                info.CardDemandQty = task.DispatchQty/ processBom.PlanQty* requireQty;
                info.RemainingQty = rds.Sum(p => (p.RemainingQty ?? 0));
                info.Unit = processBom.ItemUnitName;
                info.Factory = processBom.Werks;
                infos.Add(info);
            }

            return infos;
        }

        /// <summary>
        /// 上料:校验剩余数量
        /// </summary>
        /// <param name="TaskId"></param>
        /// <param name="ProcessBomId"></param>
        /// <returns></returns>
        [ApiService("上料:校验剩余数量")]
        public virtual string AssemblyValidRemainingQty(double TaskId)
        {
            var task = RF.GetById<DispatchTask>(TaskId, new EagerLoadOptions().LoadWithViewProperty());

            var processBoms = Query<WorkOrderProcessBom>().Join<DispatchTask>((x, y) => x.WorkOrderId == y.WorkOrderId && x.ProcessId == y.ProcessId && y.Id == TaskId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            var itemIds = processBoms.Select(p => p.ItemId).Distinct().ToList();

            //供料区域
            var areIds = new List<double?>();
            var areRes = Query<FeedingAreaReource>().Where(p => p.ResourceId == task.ResourceId && p.FeedingArea.State == State.Enable).ToList();
            areIds.AddRange(areRes.Select(p => (double?)p.FeedingAreaId).ToList());

            var records = Query<FeedingRecord>().Where(p => itemIds.Contains(p.ItemLabel.ItemId) && (p.ResourceId == task.ResourceId || areIds.Contains(p.FeedingAreaId))).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //string msg = "";
            int validQty = 0;
            foreach (var itemId in itemIds)
            {
                var processBom = processBoms.Where(p => p.ItemId == itemId).FirstOrDefault();
                var rds = records.Where(p => p.ItemId == itemId).ToList();
                if (rds.Sum(p => p.RemainingQty) >= processBom.SingleQty * processBom.PlanQty)
                {
                    //msg += "当前机台上料物料{0},上料数量{1}已满足生产;".L10nFormat(processBom.ItemCode, rds.Sum(p => (p.FeedingQty ?? 0)));
                    validQty++;
                }
            }
            if (/*msg.IsNullOrEmpty() ||*/ validQty != itemIds.Count || validQty == 0)
                return string.Empty;
            else
                return "当前机台已满足生产上料,是否继续?";//msg + "是否继续?";
        }

        /// <summary>
        /// 上料:校验剩余数量提交
        /// </summary>
        /// <param name="TaskId"></param>
        [ApiService("上料:校验剩余数量提交")]
        public virtual void AssemblyValidRemainingQtySubmmit(double TaskId)
        {
            //关闭任务单 
            DB.Update<DispatchTask>().Set(p => p.IsFeedingClose, true).Where(p => p.Id == TaskId).Execute();
        }

        /// <summary>
        /// 上料:校验物料剩余数量
        /// </summary>
        /// <param name="TaskId"></param>
        /// <param name="ProcessBomId"></param>
        /// <returns></returns>
        [ApiService("上料:校验物料剩余数量")]
        public virtual string AssemblyValidItemRemainingQty(double TaskId, List<double> ProcessBomIds)
        {
            //var processBom = RF.GetById<WorkOrderProcessBom>(ProcessBomId, new EagerLoadOptions().LoadWithViewProperty());
            var processBoms = Query<WorkOrderProcessBom>().Where(p => ProcessBomIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var itemIds = processBoms.Select(p => p.ItemId).Distinct().ToList();

            var task = RF.GetById<DispatchTask>(TaskId, new EagerLoadOptions().LoadWithViewProperty());

            //供料区域
            var areIds = new List<double?>();
            var areRes = Query<FeedingAreaReource>().Where(p => p.ResourceId == task.ResourceId && p.FeedingArea.State == State.Enable).ToList();
            areIds.AddRange(areRes.Select(p => (double?)p.FeedingAreaId).ToList());

            var records = Query<FeedingRecord>().Where(p => itemIds.Contains(p.ItemLabel.ItemId) && (p.ResourceId == task.ResourceId || areIds.Contains(p.FeedingAreaId))).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //string msg = "";

            int validQty = 0;
            foreach (var processBom in processBoms)
            {
                var rds = records.Where(p => p.ItemId == processBom.ItemId).ToList();
                if (rds.Sum(p => p.RemainingQty) >= processBom.SingleQty * processBom.PlanQty)
                {
                    //msg += "物料{0}，上料数量{1}，已满足生产;".L10nFormat(processBom.ItemCode, rds.Sum(p => (p.FeedingQty ?? 0)));
                    validQty++;
                }
            }
            if (/*msg.IsNullOrEmpty() ||*/ validQty != processBoms.Count || validQty == 0)
                return string.Empty;
            else
                return "当前机台已满足生产上料,是否继续?";//msg + "是否关闭任务?";
        }

        /// <summary>
        /// 上料:校验物料剩余数量提交
        /// </summary>
        [ApiService("上料:校验物料剩余数量提交")]
        public virtual void AssemblyValidItemRemainingQtySubmmit(List<double> ProcessBomId)
        {
            //关闭工序BOM
            //DB.Update<WorkOrderProcessBom>().Set(p => p.IsFeedingClose, true).Where(p => ProcessBomId.Contains(p.Id)).Execute();
        }

        /// <summary>
        /// 上料:获取物料标签数据 (当前接口不适用返回多个返工标签场景,请使用 AssemblyGetItemLabels 替代)
        /// </summary>
        /// <param name="labelNo"></param>
        /// <param name="TaskId"></param>
        [ApiService("上料:获取物料标签数据")]
        [Obsolete("当前接口不适用返回多个返工标签场景,请使用 AssemblyGetItemLabels 替代")]
        public virtual Pda_AssemblyGetItemLabelInfo AssemblyGetItemLabel(string labelNo, double TaskId)
        {

            var itemLabel = RT.Service.Resolve<ItemLabelController>().GetItemLabelKz(labelNo);

            if (itemLabel == null)
            {
                //查询是否有可疑品返工标签
                var suspectLabel = RT.Service.Resolve<SuspectProductLabelController>().GetSuspectProductLabel(labelNo);
                if (suspectLabel != null)
                {
                    var labelNos = suspectLabel.DetailList.Where(p => p.SuspectJudgeResult == SuspectJudgeResult.Repair).Select(p => p.SubBatchNo).ToList();
                    if (labelNos.Count > 0)
                    {
                        var itemLabels = RT.Service.Resolve<ItemLabelController>().GetItemLabelKz(labelNos);

                        if (itemLabels.Count > 1)
                        {
                            throw new ValidationException("请退出程序重新登录,并升级程序到最新版本");
                        }
                        itemLabel = itemLabels.FirstOrDefault();
                    }
                }
            }
            AssemblyGetItemLabelValid(itemLabel, labelNo);

            var task = RF.GetById<DispatchTask>(TaskId, new EagerLoadOptions().LoadWithViewProperty());
            var item = RF.GetById<Item>(itemLabel.ItemId, new EagerLoadOptions().LoadWithViewProperty());
            var processBom = GetWorkOrderProcessBoms(task.ResourceId.Value, itemLabel.ItemId);

            if (processBom == null)
            {
                throw new ValidationException("物料编码{0}不属于当前任务".L10nFormat(itemLabel.ItemCode));
            }

            Pda_AssemblyGetItemLabelInfo info = new Pda_AssemblyGetItemLabelInfo();
            info.ItemLabel = itemLabel.Label;
            info.BatchNo = itemLabel.Lot;
            info.ItemCode = itemLabel.ItemCode;
            info.ItemName = itemLabel.ItemName;
            info.AssemblyQty = itemLabel.Qty;
            info.ItemLabelId = itemLabel.Id;
            info.Unit = item.Unit?.Name;
            info.ItemLabelState = itemLabel.ItemLabelState?.ToLabel();
            return info;
        }

        /// <summary>
        /// 上料:获取物料标签数据
        /// </summary>
        /// <param name="labelNo"></param>
        /// <param name="TaskId"></param>
        [ApiService("上料:获取物料标签数据")]
        public virtual List<Pda_AssemblyGetItemLabelInfo> AssemblyGetItemLabels(string labelNo, double TaskId)
        {
            var datas = new List<ItemLabel>();
            var itemLabel = RT.Service.Resolve<ItemLabelController>().GetItemLabelKz(labelNo);

            if (itemLabel == null)
            {
                //查询是否有可疑品返工标签
                var suspectLabel = RT.Service.Resolve<SuspectProductLabelController>().GetSuspectProductLabel(labelNo);
                if (suspectLabel != null)
                {
                    var labelNos = suspectLabel.DetailList.Where(p => p.SuspectJudgeResult == SuspectJudgeResult.Repair).Select(p => p.SubBatchNo).ToList();
                    if (labelNos.Count > 0)
                    {
                        var itemLabels = RT.Service.Resolve<ItemLabelController>().GetItemLabelKz(labelNos);
                        itemLabels.ForEach(p =>
                        {
                            AssemblyGetItemLabelValid(p, p?.Label);
                        });
                        datas.AddRange(itemLabels);
                    }
                }
            }
            else
            {
                AssemblyGetItemLabelValid(itemLabel, labelNo);
                datas.Add(itemLabel);
            }
            if (datas.Count == 0)
                throw new ValidationException("物料标签[{0}]不存在".L10nFormat(labelNo));
            itemLabel = datas.FirstOrDefault();

            var task = RF.GetById<DispatchTask>(TaskId);
            var item = RF.GetById<Item>(itemLabel.ItemId);
            var processBom = GetWorkOrderProcessBoms(task.ResourceId.Value, itemLabel.ItemId);

            if (processBom == null)
            {
                throw new ValidationException("物料编码{0}不属于当前任务".L10nFormat(itemLabel.ItemCode));
            }

            #region 新增控制：《上料间隔时间》配置表中的物料上料间隔------upd by 20260609

            var feedingInterval = Query<FeedingInterval>().Where(p => p.ItemId == itemLabel.ItemId).FirstOrDefault();
            if (feedingInterval != null)
            {
                var feedingRecord = Query<FeedingRecord>()
                     .Where(p => p.ItemId == itemLabel.ItemId && p.ItemLabel.ItemLabelState == ItemLabelState.Feeding)
                     .OrderByDescending(p => p.UpdateDate)
                     .FirstOrDefault();

                if (feedingRecord != null)
                {
                    int t = (int)(DateTime.Now - feedingRecord.UpdateDate).TotalSeconds;

                    if (feedingInterval.IntervalSecond > t)
                    {
                        throw new ValidationException("{0}最近一次上料时间需达到{1}秒后才允许再次上料".L10nFormat(itemLabel.ItemCode, feedingInterval.IntervalSecond));
                    }

                }

            }   

            #endregion





            var infos = datas.Select(p =>
            {
                Pda_AssemblyGetItemLabelInfo info = new Pda_AssemblyGetItemLabelInfo();
                info.ItemLabel = p.Label;
                info.BatchNo = p.Lot;
                info.ItemCode = p.ItemCode;
                info.ItemName = p.ItemName;
                info.AssemblyQty = p.Qty;
                info.ItemLabelId = p.Id;
                info.Unit = p.Item?.Unit?.Name;
                info.ItemLabelState = p.ItemLabelState?.ToLabel();
                return info;
            }).ToList();
            return infos;
        }

        public virtual void AssemblyGetItemLabelValid(ItemLabel itemLabel, string Label)
        {
            if (itemLabel == null)
                throw new ValidationException("物料标签[{0}]不存在".L10nFormat(Label));
            if (itemLabel.ItemLabelState == ItemLabelState.Feeding)
                throw new ValidationException("标签[{0}]已上料".L10nFormat(itemLabel?.Label));
        }

        public virtual WorkOrderProcessBom GetWorkOrderProcessBoms(double ResourceId, double itemId)
        {

            var processBom = Query<WorkOrderProcessBom>().Join<DispatchTask>((x, y) => x.WorkOrderId == y.WorkOrderId && x.ProcessId == y.ProcessId && y.ResourceId == ResourceId && (y.IsFeedingClose == false || y.IsFeedingClose == null)).Where(p => p.ItemId == itemId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            return processBom;
        }

        /// <summary>
        /// 上料:提交
        /// </summary>
        /// <param name="TaskId"></param>
        /// <param name="infos"></param>
        [ApiService("上料:提交")]
        public virtual void AssemblySubmit(double TaskId, List<Pda_AssemblyGetItemLabelInfo> infos)
        {
            EntityList<FeedingRecord> records = new EntityList<FeedingRecord>();
            using (var tran = DB.TransactionScope(TaskManagementEntityDataProvider.ConnectionStringName))
            {
                //校验是否存在已上料的标签
                var itemLabelIds = infos.Select(p => p.ItemLabelId).Distinct().ToList();
                var itemlabels = Query<ItemLabel>().Where(p => itemLabelIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                var feedingLabels = itemlabels.Where(p => p.ItemLabelState == ItemLabelState.Feeding).ToList();
                if (feedingLabels.Count > 0)
                {
                    var ilids = feedingLabels.Select(p => p.Id).Distinct().ToList();
                    throw new ValidationException("标签[{0}]已上料".L10nFormat(string.Join("、", infos.Where(p => ilids.Contains(p.ItemLabelId)).Select(p => p.ItemLabel).Distinct().ToList())));
                }
                var task = RF.GetById<DispatchTask>(TaskId);
                if (task == null)
                    throw new ValidationException("任务单[ID={0}]不存在".L10nFormat(TaskId));

                var processBomInfos = AssemblyGetProcessBomInfos(TaskId);

                //不校验工厂物料清单
                var itemCodes = infos.Select(p => p.ItemCode).Distinct().ToList();
                var unValidFactoryItems = RT.Service.Resolve<ItemController>().GetUnValidFactoryItemsByItemCodes(itemCodes);

                foreach (var info in infos)
                {
                    var label = itemlabels.FirstOrDefault(p => p.Id == info.ItemLabelId);
                    //校验物料标签所在工厂与工单的工序BOM的发料工厂是否一致
                    var config = ConfigService.GetConfig(new ItemLabelConfig(), typeof(ItemLabel));
                    //维护在不校验工厂物料清单的物料数据不做这个校验
                    if (config != null && config.IsValidFactory == true && !unValidFactoryItems.Any(p => p.ItemCode == info.ItemCode) && processBomInfos.Any(p => p.ProductCode == info.ItemCode && p.Factory != label.FactoryCode))
                    {
                        throw new ValidationException("该标签存在于工厂{0}".L10nFormat(label.FactoryCode));
                    }

                    FeedingRecord record = new FeedingRecord();
                    record.DispatchTaskId = task.Id;
                    record.ResourceId = task.ResourceId;
                    record.ItemLabelId = info.ItemLabelId;
                    record.FeedingQty = info.AssemblyQty;
                    record.RemainingQty = info.AssemblyQty;
                    record.PersistenceStatus = PersistenceStatus.New;
                    record.FeedingItemLabel = info.ItemLabel;
                    record.ItemId = label?.ItemId;
                    records.Add(record);
                    DB.Update<ItemLabel>().Set(p => p.ItemLabelState, ItemLabelState.Feeding).Where(p => p.Id == info.ItemLabelId).Execute();
                }
                if (records.Count > 0)
                {
                    RF.Save(records);
                }
                tran.Complete();
            }
        }

        #endregion

        #region 区域上料

        /// <summary>
        /// 扫描供料区编码
        /// </summary>
        /// <param name="areCode"></param>
        [ApiService("区域上料:扫描供料区编码")]
        public virtual BaseDataInfo GetFeedingAreaInfo(string areCode)
        {
            var area = RT.Service.Resolve<FeedingRecordController>().GetFeedingArea(areCode);
            if (area == null)
                throw new ValidationException("供料区编码[{0}]不存在".L10nFormat(areCode));
            if (area.State == State.Disable)
                throw new ValidationException("供料区[{0}]未启用".L10nFormat(areCode));

            return new BaseDataInfo() { Id = area.Id, Code = area.Code, Name = area.Name };
        }

        /// <summary>
        /// 扫描物料标签 (当前接口不适用返回多个返工标签场景,请使用 AssemblyGetItemLabels 替代)
        /// </summary>
        /// <param name="areCode"></param>
        /// <param name="labelNo"></param>
        /// <returns></returns>
        [ApiService("区域上料:扫描物料标签")]
        [Obsolete("当前接口不适用返回多个返工标签场景,请使用 GetFeedingAreaItemLabelInfos 替代")]
        public virtual Pda_AssemblyGetItemLabelInfo GetFeedingAreaItemLabelInfo(string areCode, string labelNo)
        {
            var itemLabel = RT.Service.Resolve<ItemLabelController>().GetItemLabelKz(labelNo);

            if (itemLabel == null)
            {
                //查询是否有可疑品返工标签
                var suspectLabel = RT.Service.Resolve<SuspectProductLabelController>().GetSuspectProductLabel(labelNo);
                if (suspectLabel != null)
                {
                    var labelNos = suspectLabel.DetailList.Where(p => p.SuspectJudgeResult == SuspectJudgeResult.Repair).Select(p => p.SubBatchNo).ToList();
                    if (labelNos.Count > 0)
                    {
                        var itemLabels = RT.Service.Resolve<ItemLabelController>().GetItemLabelKz(labelNos);
                        if (itemLabels.Count > 1)
                        {
                            throw new ValidationException("请退出程序重新登录,并升级程序到最新版本");
                        }
                        itemLabel = itemLabels.FirstOrDefault();
                    }
                }
            }
            AssemblyGetItemLabelValid(itemLabel, labelNo);

            var area = RT.Service.Resolve<FeedingRecordController>().GetFeedingArea(areCode);
            if (area == null)
                throw new ValidationException("供料区编码[{0}]不存在".L10nFormat(areCode));

            if (!area.ItemList.Any(p => p.ItemId == itemLabel.ItemId))
            {
                throw new ValidationException("物料[{0}]不是区域[{1}]供料物料，请检查".L10nFormat(itemLabel.ItemCode, areCode));
            }

            Pda_AssemblyGetItemLabelInfo info = new Pda_AssemblyGetItemLabelInfo();
            info.ItemLabel = itemLabel.Label;
            info.BatchNo = itemLabel.Lot;
            info.ItemCode = itemLabel.ItemCode;
            info.ItemName = itemLabel.ItemName;
            info.AssemblyQty = itemLabel.Qty;
            info.ItemLabelId = itemLabel.Id;
            info.Unit = itemLabel.Item?.Unit?.Name;
            info.ItemLabelState = itemLabel.ItemLabelState?.ToLabel();
            return info;
        }

        /// <summary>
        /// 扫描物料标签
        /// </summary>
        /// <param name="areCode"></param>
        /// <param name="labelNo"></param>
        /// <returns></returns>
        [ApiService("区域上料:扫描物料标签")]
        public virtual List<Pda_AssemblyGetItemLabelInfo> GetFeedingAreaItemLabelInfos(string areCode, string labelNo)
        {
            var datas = new List<ItemLabel>();
            var itemLabel = RT.Service.Resolve<ItemLabelController>().GetItemLabelKz(labelNo);

            if (itemLabel == null)
            {
                //查询是否有可疑品返工标签
                var suspectLabel = RT.Service.Resolve<SuspectProductLabelController>().GetSuspectProductLabel(labelNo);
                if (suspectLabel != null)
                {
                    var labelNos = suspectLabel.DetailList.Where(p => p.SuspectJudgeResult == SuspectJudgeResult.Repair).Select(p => p.SubBatchNo).ToList();
                    if (labelNos.Count > 0)
                    {
                        var itemLabels = RT.Service.Resolve<ItemLabelController>().GetItemLabelKz(labelNos);
                        itemLabels.ForEach(p =>
                        {
                            AssemblyGetItemLabelValid(p, p?.Label);
                        });
                        itemLabel = itemLabels.FirstOrDefault();
                        datas.AddRange(itemLabels);
                    }
                }
            }
            else
            {
                AssemblyGetItemLabelValid(itemLabel, labelNo);
                datas.Add(itemLabel);
            }
            if (datas.Count == 0)
                throw new ValidationException("物料标签[{0}]不存在".L10nFormat(labelNo));
            itemLabel = datas.FirstOrDefault();

            var area = RT.Service.Resolve<FeedingRecordController>().GetFeedingArea(areCode);
            if (area == null)
                throw new ValidationException("供料区编码[{0}]不存在".L10nFormat(areCode));

            if (!area.ItemList.Any(p => p.ItemId == itemLabel.ItemId))
            {
                throw new ValidationException("物料[{0}]不是区域[{1}]供料物料，请检查".L10nFormat(itemLabel.ItemCode, areCode));
            }
            var infos = datas.Select(p =>
            {
                Pda_AssemblyGetItemLabelInfo info = new Pda_AssemblyGetItemLabelInfo();
                info.ItemLabel = p.Label;
                info.BatchNo = p.Lot;
                info.ItemCode = p.ItemCode;
                info.ItemName = p.ItemName;
                info.AssemblyQty = p.Qty;
                info.ItemLabelId = p.Id;
                info.Unit = p.Item?.Unit?.Name;
                info.ItemLabelState = p.ItemLabelState?.ToLabel();
                return info;
            }).ToList();
            return infos;
        }
        /// <summary>
        /// 区域上料:提交
        /// </summary>
        /// <param name="areCode"></param>
        /// <param name="infos"></param>
        [ApiService("区域上料:提交")]
        public virtual void SubmitFeedingAreaItemLabelInfo(string areCode, List<Pda_AssemblyGetItemLabelInfo> infos)
        {
            var area = RT.Service.Resolve<FeedingRecordController>().GetFeedingArea(areCode);
            if (area == null)
                throw new ValidationException("供料区编码[{0}]不存在".L10nFormat(areCode));

            //校验是否存在已上料的标签
            var itemLabelIds = infos.Select(p => p.ItemLabelId).Distinct().ToList();
            var itemlabels = Query<ItemLabel>().Where(p => itemLabelIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var feedingLabels = itemlabels.Where(p => p.ItemLabelState == ItemLabelState.Feeding).ToList();
            if (feedingLabels.Count > 0)
            {
                var ilids = feedingLabels.Select(p => p.Id).Distinct().ToList();
                throw new ValidationException("标签[{0}]已上料".L10nFormat(string.Join("、", infos.Where(p => ilids.Contains(p.ItemLabelId)).Select(p => p.ItemLabel).Distinct().ToList())));
            }

            EntityList<FeedingRecord> records = new EntityList<FeedingRecord>();

            foreach (var info in infos)
            {
                var label = itemlabels.FirstOrDefault(p => p.Id == info.ItemLabelId);
                FeedingRecord record = new FeedingRecord();
                //record.DispatchTaskId = task.Id;
                //record.ResourceId = task.ResourceId;
                record.FeedingArea = area;
                record.ItemLabelId = info.ItemLabelId;
                record.FeedingQty = info.AssemblyQty;
                record.RemainingQty = info.AssemblyQty;
                record.PersistenceStatus = PersistenceStatus.New;
                record.FeedingItemLabel = info.ItemLabel;
                record.ItemId = label?.ItemId;
                records.Add(record);
            }

            using (var tran = DB.TransactionScope(TaskManagementEntityDataProvider.ConnectionStringName))
            {
                if (records.Count > 0)
                {
                    RF.Save(records);
                    DB.Update<ItemLabel>().Set(p => p.ItemLabelState, ItemLabelState.Feeding).Where(p => itemLabelIds.Contains(p.Id)).Execute();
                }
                tran.Complete();
            }
        }
        #endregion

        #region 下料采集

        /// <summary>
        /// 下料采集:下拉获取生产资源/供料区
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [ApiService("下料采集:下拉获取生产资源/供料区")]
        public virtual List<BlankingAssemblyGetWipResources> BlankingAssemblyGetWipResources(string key)
        {
            List<BlankingAssemblyGetWipResources> infos = new List<BlankingAssemblyGetWipResources>();
            var q = Query<EmployeeResource>().Where(p => p.EmployeeId == RT.IdentityId);
            if (!key.IsNullOrEmpty())
                q.Where(p => p.Resource.Code.Contains(key) || p.Resource.Name.Contains(key));
            var employeeResources = q.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            foreach (var employeeResource in employeeResources)
            {
                infos.Add(new BlankingAssemblyGetWipResources()
                {
                    ResourceId = employeeResource.ResourceId,
                    Code = employeeResource.ResourceCode,
                    Name = employeeResource.ResourceName,
                    IsResource = true
                });
            }

            var feedingQuery = Query<FeedingArea>();
            if (!key.IsNullOrEmpty())
                feedingQuery.Where(p => p.Code.Contains(key) || p.Name.Contains(key));
            var feedingAreas = feedingQuery.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            foreach (var feedingArea in feedingAreas)
            {
                infos.Add(new BlankingAssemblyGetWipResources()
                {
                    ResourceId = feedingArea.Id,
                    Code = feedingArea.Code,
                    Name = feedingArea.Name,
                    IsResource = false
                });
            }

            return infos;
        }

        /// <summary>
        /// 下料采集:获取上料记录
        /// </summary>
        /// <param name="ResourId"></param>
        /// <param name="IsResource">是否为生产资源</param>
        [ApiService("下料采集:获取上料记录")]
        public virtual List<Pda_BlankingGetFeedRecordInfo> BlankingGetFeedRecords(double ResourId, string key, bool IsResource)
        {
            List<Pda_BlankingGetFeedRecordInfo> infos = new List<Pda_BlankingGetFeedRecordInfo>();
            var q = Query<FeedingRecord>().Where(p => p.ItemLabel.ItemLabelState == ItemLabelState.Feeding && p.RemainingQty > 0);

            //判断是资源还是供料区
            if (IsResource == true)
            {
                q.Where(p => p.ResourceId == ResourId);
            }
            else
            {
                q.Where(p => p.FeedingAreaId == ResourId);
            }

            if (!key.IsNullOrEmpty())
            {
                q.Where(p => p.ItemLabel.Label.Contains(key));
            }

            var feedingRecords = q.ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            foreach (var record in feedingRecords)
            {
                Pda_BlankingGetFeedRecordInfo info = new Pda_BlankingGetFeedRecordInfo();
                info.Label = record.FeedingItemLabel;
                info.ItemCode = record.ItemCode;
                info.ItemDesc = record.ItemName;
                info.ItemLabelLot= record.ItemLabelLot;
                info.FeedingTime= record.CreateDate;
                info.FeedingQty = record.FeedingQty;
                info.RemainingQty = record.RemainingQty;
                info.RecordId = record.Id;
                infos.Add(info);
            }
            return infos;
        }

        /// <summary>
        /// 下料采集:提交
        /// </summary>
        [ApiService("下料采集:提交")]
        public virtual void BlankingSubmit(List<double> RecordIds)
        {
            using (var tran = DB.TransactionScope(TaskManagementEntityDataProvider.ConnectionStringName))
            {
                var records = Query<FeedingRecord>().Where(p => RecordIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                var itemLabelIds = records.Select(p => p.ItemLabelId).Distinct().ToList();
                var itemLabels = Query<ItemLabel>().Where(p => itemLabelIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                foreach (var item in records)
                {
                    //要先更新标签数据
                    var itemLabel = itemLabels.FirstOrDefault(p => p.Id == item.ItemLabelId);
                    itemLabel.PersistenceStatus = PersistenceStatus.Modified;
                    itemLabel.Qty = item.RemainingQty.Value;
                    itemLabel.ItemLabelState = ItemLabelState.Blanking;
                    //在更新记录的下料数量，然后是剩余数量
                    item.PersistenceStatus = PersistenceStatus.Modified;
                    item.BlankingQty = item.RemainingQty;
                    item.RemainingQty = 0;
                }
                RF.Save(records);
                RF.Save(itemLabels);
                tran.Complete();
            }
        }

        #endregion

        #region 开机准备

        /// <summary>
        /// 开机准备:工装/检具获取任务单
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [ApiService("开机准备:工装/检具获取任务单")]
        public virtual List<Pda_PreStartupSetupTaskInfo> GetPreStartupSetupToolTaskInfos(string key)
        {
            List<Pda_PreStartupSetupTaskInfo> infos = new List<Pda_PreStartupSetupTaskInfo>();

            var employeeResources = RT.Service.Resolve<EmployeeController>().GetEmployeeResources(RT.IdentityId);
            if (employeeResources.Count <= 0)
                throw new ValidationException("员工没有配置资源权限".L10N());
            var resourceIds = employeeResources.Select(p => p.ResourceId).Distinct().ToList();

            //先找到来源类型是产线与安灯区域的资源
            var resourceCodes = employeeResources.Where(p => p.ResourceSourceType == SyncSourceType.LineAndon).Select(p => p.ResourceCode).Distinct().ToList();
            //找到资源上面的工作中心对应的资源，防止任务单派工到工作中心上，员工分配权限资源只会分配类型为产线与安灯区域的资源 
            var workCenterResources = resourceCodes.SplitContains(codes => {
                return Query<WipResource>().Where(p => p.SourceType == SyncSourceType.WorkCenter).Exists<AndonLine>((x, y) => y.Where(p => p.WorkCenter.Code == x.Code && codes.Contains(p.MachineCode))).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            resourceIds.AddRange(workCenterResources.Select(p => p.Id).Distinct().ToList());
            resourceIds = resourceIds.Distinct().ToList();

            //派工单的产品编码+工序到检具与产品关系中查询是否绑定了检具，并且对应的检具不能在开机准备记录表中存在
            EntityList<DispatchTask> tasks_1 = new EntityList<DispatchTask>();
            EntityList<DispatchTask> tasks_2 = new EntityList<DispatchTask>();
            resourceIds.SplitDataExecute(ids =>
            {
                //var q_1 = DB.Query<DispatchTask>("TASK").Join<CheckerItem>("C", (x, y) => x.ProcessId == y.ProcessId && x.ProductId == y.ItemId && y.State == State.Enable).Join<CheckerItem, CheckerUphold>("U", (x, y) => x.CheckerUpholdId == y.Id).Where(p => ids.Contains((double)p.ResourceId) && p.TaskStatus != DispatchTaskStatus.Closed && p.TaskStatus != DispatchTaskStatus.Finished && p.SQL<bool>("not exists(select 1 from PRE_START_SETUP_REC where PRE_START_SETUP_REC.Tool_Code = U.Checker_Code and TASK.ID = PRE_START_SETUP_REC.Dispatch_Task_Id and PRE_START_SETUP_REC.is_phantom = 0)"));

                var q_1 = DB.Query<DispatchTask>("TASK").Exists<CheckerItem>((x, y) => y.Join<CheckerUphold>("U", (x1, y1) => x1.CheckerUpholdId == y1.Id).Where(p => p.ProcessId == x.ProcessId && p.ItemId == x.ProductId && p.SQL<bool>("not exists(select 1 from PRE_START_SETUP_REC where PRE_START_SETUP_REC.Tool_Code = U.Checker_Code and TASK.ID = PRE_START_SETUP_REC.Dispatch_Task_Id and PRE_START_SETUP_REC.is_phantom = 0)"))).Where(p => ids.Contains((double)p.ResourceId) && (p.TaskStatus == DispatchTaskStatus.Dispatched || p.TaskStatus == DispatchTaskStatus.Executing));

                //派工单的产品编码+工序到工装与产品关系中查询是否绑定了工装，并且对应的工装不能在开机准备记录表中存在
                //var q_2 = DB.Query<DispatchTask>("TASK").Join<FixtureItem>((x, y) => x.ProcessId == y.ProcessId && x.ProductId == y.ItemId && y.State == State.Enable).Join<FixtureItem, FixtureUphold>("U", (x, y) => x.FixtureUpholdId == y.Id).Where(p => ids.Contains((double)p.ResourceId) && p.TaskStatus != DispatchTaskStatus.Closed && p.TaskStatus != DispatchTaskStatus.Finished && p.SQL<bool>("not exists(select 1 from PRE_START_SETUP_REC where PRE_START_SETUP_REC.Tool_Code = U.Fixture_Code AND TASK.ID = PRE_START_SETUP_REC.Dispatch_Task_Id and PRE_START_SETUP_REC.is_phantom = 0)"));

                var q_2 = DB.Query<DispatchTask>("TASK").Exists<FixtureItem>((x, y) => y.Join<FixtureUphold>("U", (x1, y1) => x1.FixtureUpholdId == y1.Id).Where(p => p.ProcessId == x.ProcessId && p.ItemId == x.ProductId && p.SQL<bool>("not exists(select 1 from PRE_START_SETUP_REC where PRE_START_SETUP_REC.Tool_Code = U.Fixture_Code AND TASK.ID = PRE_START_SETUP_REC.Dispatch_Task_Id and PRE_START_SETUP_REC.is_phantom = 0)"))).Where(p => ids.Contains((double)p.ResourceId) && (p.TaskStatus == DispatchTaskStatus.Dispatched || p.TaskStatus == DispatchTaskStatus.Executing));

                if (!key.IsNullOrEmpty())
                {
                    q_1.Where(p => p.No.Contains(key) || p.Resource.Code.Contains(key) || p.Resource.Name.Contains(key) || p.WorkOrder.No.Contains(key));
                    q_2.Where(p => p.No.Contains(key) || p.Resource.Code.Contains(key) || p.Resource.Name.Contains(key) || p.WorkOrder.No.Contains(key));
                }

                var list_1 = q_1.OrderBy(p => p.PlanBeginTime).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                tasks_1.AddRange(list_1);
                var list_2 = q_2.OrderBy(p => p.PlanBeginTime).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                tasks_2.AddRange(list_2);
            });

            foreach (var task in tasks_1.OrderBy(p => p.PlanBeginTime))
            {
                Pda_PreStartupSetupTaskInfo info = new Pda_PreStartupSetupTaskInfo();
                info.TaskId = task.Id;
                info.TaskNo = task.No;
                info.WorkOrderNo = task.WorkOrderNo;
                info.Resource = task.ResourceCode;
                info.ResourceName = task.ResourceName;
                info.ProductCode = task.ProductCode;
                info.ProductName = task.ProductName;
                info.Qty = task.DispatchQty;
                //info.Process = task.ProcessCode;
                info.Process = task.ProcessName;
                infos.Add(info);
            }

            foreach (var task in tasks_2.OrderBy(p => p.PlanBeginTime))
            {
                //如果存在了就直接跳过
                if (infos.Any(p => p.TaskId == task.Id))
                    continue;
                Pda_PreStartupSetupTaskInfo info = new Pda_PreStartupSetupTaskInfo();
                info.TaskId = task.Id;
                info.TaskNo = task.No;
                info.WorkOrderNo = task.WorkOrderNo;
                info.Resource = task.ResourceCode;
                info.ResourceName = task.ResourceName;
                info.ProductCode = task.ProductCode;
                info.ProductName = task.ProductName;
                info.Qty = task.DispatchQty;
                //info.Process = task.ProcessCode;
                info.Process = task.ProcessName;
                infos.Add(info);
            }

            return infos;
        }

        /// <summary>
        /// 开机准备:工装/检具扫描工装/检具
        /// </summary>
        /// <param name="key"></param>
        /// <param name="TaskId"></param>
        /// <returns></returns>
        [ApiService("开机准备:工装/检具扫描工装/检具")]
        public virtual Pda_PreStartupSetupToolInfo GetPreStartupSetupToolInfos(string key, double TaskId)
        {
            Pda_PreStartupSetupToolInfo info = new Pda_PreStartupSetupToolInfo();
            //获取检具
            var checkerItem = Query<CheckerItem>().Join<DispatchTask>((x, y) => x.ItemId == y.ProductId && x.ProcessId == y.ProcessId && y.Id == TaskId).Where(p => p.CheckerUphold.CheckerCode == key).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            //判断检具是否存在，不存在就判断是否工装
            if (checkerItem != null)
            {
                //if (checkerItem.State == State.Disable)
                //    throw new ValidationException("检具为禁用状态".L10N());
                info.Code = checkerItem.CheckerUphold.CheckerCode;
                info.Name = checkerItem.CheckerUphold.CheckerName;
                info.Id = checkerItem.Id;
                info.Type = CheckerFixtureType.Checker.ToLabel();
                info.ScanTime = DateTime.Now;
                return info;
            }
            //检具不存在，就判断是否为工装
            var fixtureItem = Query<FixtureItem>().Join<DispatchTask>((x, y) => x.ItemId == y.ProductId && x.ProcessId == y.ProcessId && y.Id == TaskId).Where(p => p.FixtureUphold.FixtureCode == key).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (fixtureItem == null)
                throw new ValidationException("不存在该检具/工装".L10N());
            //if (fixtureItem.State == State.Disable)
            //    throw new ValidationException("工装为禁用状态".L10N());

            info.Code = fixtureItem.FixtureUphold.FixtureCode;
            info.Name = fixtureItem.FixtureUphold.FixtureName;
            info.Type = CheckerFixtureType.Fixture.ToLabel();
            info.ScanTime = DateTime.Now;
            info.Id = fixtureItem.Id;
            return info;
        }

        /// <summary>
        /// 开机准备:工装/检具提交
        /// </summary>
        [ApiService("开机准备:工装/检具提交")]
        public virtual void PreStartupSetupToolSubmmit(double TaskId, List<Pda_PreStartupSetupToolInfo> infos)
        {
            if (TaskId <= 0)
                throw new ValidationException("派工单不能为空".L10N());
            if (infos.Count < 1)
                throw new ValidationException("提交数据不能为空".L10N());
            EntityList<PreStartupSetupRecord> records = new EntityList<PreStartupSetupRecord>();
            foreach (var info in infos)
            {
                PreStartupSetupRecord record = new PreStartupSetupRecord();

                record.DispatchTaskId = TaskId;
                record.ToolCode = info.Code;
                record.ToolName = info.Name;
                if (info.Type == CheckerFixtureType.Fixture.ToLabel())
                {
                    record.CheckerFixtureType = CheckerFixtureType.Fixture;
                    FixtureUphold fixtureUphold = RT.Service.Resolve<FixtureUpholdController>().GetFixtureUpholdByCode(info.Code);
                    record.DrawingNo = fixtureUphold?.Drawn;
                }
                else
                {
                    record.CheckerFixtureType = CheckerFixtureType.Checker;
                    //CheckerUphold checkerUphold = RT.Service.Resolve<CheckerUpholdController>().GetCheckerUpholdByCode(info.Code);
                    var checkerItem = RF.GetById<CheckerItem>(info.Id, new EagerLoadOptions().LoadWithViewProperty());
                    record.DrawingNo = checkerItem?.DrawingNo;

                    //记录开机准备时如果检具与产品关系中同一个编码 + 物料 + 工序有多个图号，那么开机准是记录要把多个图号都记录下来(每一个图号创建一个开机准备记录)
                    var drawingNos = Query<CheckerItem>().Where(p => p.CheckerUphold.CheckerCode == checkerItem.CheckerCode && p.Process.Code == checkerItem.ProcessCode && p.Item.Code == checkerItem.Item.Code && p.Id != checkerItem.Id && p.DrawingNo != record.DrawingNo).Select(p => p.DrawingNo).Distinct().ToList<string>().ToList();

                    if (drawingNos.Count > 0)
                    {
                        //数据一摸一样，只有图号不同
                        foreach (var drawingNo in drawingNos)
                        {
                            var copyRecord = new PreStartupSetupRecord();
                            copyRecord.Clone(record, new CloneOptions(CloneActions.NormalProperties));
                            copyRecord.DrawingNo = drawingNo;
                            copyRecord.GenerateId();
                            copyRecord.PersistenceStatus = PersistenceStatus.New;
                            records.Add(copyRecord);
                        }
                    }
                }
                record.PersistenceStatus = PersistenceStatus.New;
                records.Add(record);
            }
            if (records.Count > 0)
                RF.Save(records);
        }

        /// <summary>
        /// 开机准备:上模获取任务单
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [ApiService("开机准备:上模获取任务单")]
        public virtual List<PreStartupSetupModelTaskInfo> GetPreStartupSetupEquipAccountTaskInfos(string key)
        {
            List<PreStartupSetupModelTaskInfo> infos = new List<PreStartupSetupModelTaskInfo>();

            var employeeResources = RT.Service.Resolve<EmployeeController>().GetEmployeeResources(RT.IdentityId);
            if (employeeResources.Count <= 0)
                throw new ValidationException("员工没有配置资源权限".L10N());
            var resourceIds = employeeResources.Select(p => p.ResourceId).Distinct().ToList();
            //先找到来源类型是产线与安灯区域的资源
            var resourceCodes = employeeResources.Where(p => p.ResourceSourceType == SyncSourceType.LineAndon).Select(p => p.ResourceCode).Distinct().ToList();
            //找到资源上面的工作中心对应的资源，防止任务单派工到工作中心上，员工分配权限资源只会分配类型为产线与安灯区域的资源 
            var workCenterResources = resourceCodes.SplitContains(codes => {
                return Query<WipResource>().Where(p => p.SourceType == SyncSourceType.WorkCenter).Exists<AndonLine>((x, y) => y.Where(p => p.WorkCenter.Code == x.Code && codes.Contains(p.MachineCode))).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            resourceIds.AddRange(workCenterResources.Select(p => p.Id).Distinct().ToList());
            resourceIds = resourceIds.Distinct().ToList();

            var tasks = resourceIds.SplitContains(ids =>
            {
                //var q = DB.Query<DispatchTask>("TASK").Exists<EquipAccountItem>((x, y) => y.Join<EquipAccount>("U", (x1, y1) => x1.EquipAccountId == y1.Id).Where(p => p.ProcessId == x.ProcessId && p.ItemId == x.ProductId && p.SQL<bool>("not exists(select * from PRE_START_SETUP_REC where PRE_START_SETUP_REC.Tool_Code = U.Code AND TASK.ID = PRE_START_SETUP_REC.Dispatch_Task_Id and PRE_START_SETUP_REC.is_phantom = 0)"))).Where(p => ids.Contains((double)p.ResourceId) && p.TaskStatus != DispatchTaskStatus.Closed && p.TaskStatus != DispatchTaskStatus.Finished);

                var q = DB.Query<DispatchTask>("TASK")
                //.Exists<EquipAccountItem>((x, y) => y.NotExists<PreStartupSetupRecord>((x1, y1) => y1.Where(p1 => p1.UniqueCode == x1.UniqueCode && p1.DispatchTaskId == x.Id)).Where(p1 => p1.ProcessId == x.ProcessId && p1.ItemId == x.ProductId && p1.UniqueCode != null && p1.UniqueCode != "")).Where(p => ids.Contains((double)p.ResourceId) && p.TaskStatus != DispatchTaskStatus.Closed && p.TaskStatus != DispatchTaskStatus.Finished)

                .Exists<EquipAccountItem>((x, y) => y.Join<EquipAccount>("U", (x1, y1) => x1.EquipAccountId == y1.Id).Where(p => p.ProcessId == x.ProcessId && p.ItemId == x.ProductId &&

                (p.SQL<bool>("(T0.UNIQUE_CODE is null and not exists(select * from PRE_START_SETUP_REC where PRE_START_SETUP_REC.Tool_Code = U.Code AND TASK.ID = PRE_START_SETUP_REC.Dispatch_Task_Id and PRE_START_SETUP_REC.is_phantom = 0))") || p.SQL<bool>("(T0.UNIQUE_CODE is not null and not exists(select 1 from PRE_START_SETUP_REC where PRE_START_SETUP_REC.Unique_Code = t0.Unique_Code and PRE_START_SETUP_REC.Dispatch_Task_Id = task.id and PRE_START_SETUP_REC.is_phantom = 0 ))")

                )
                )

                ).Where(p => ids.Contains((double)p.ResourceId) && (p.TaskStatus == DispatchTaskStatus.Dispatched || p.TaskStatus == DispatchTaskStatus.Executing));

                if (!key.IsNullOrEmpty())
                {
                    q.Where(p => p.No.Contains(key) || p.Resource.Code.Contains(key) || p.Resource.Name.Contains(key) || p.WorkOrder.No.Contains(key));
                }
                var list = q.OrderBy(p => p.PlanBeginTime).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                return list;
            });
            //var q = DB.Query<DispatchTask>("TASK").Exists<EquipAccountItem>((x, y) => y.Join<EquipAccount>("U", (x1, y1) => x1.EquipAccountId == y1.Id).Where(p => p.ProcessId == x.ProcessId && p.ItemId == x.ProductId && p.State == State.Enable && p.SQL<bool>("not exists(select * from PRE_START_SETUP_REC where PRE_START_SETUP_REC.Tool_Code = U.Code AND TASK.ID = PRE_START_SETUP_REC.Dispatch_Task_Id and PRE_START_SETUP_REC.is_phantom = 0)"))).Where(p => p.IsFeedingClose == false || p.IsFeedingClose == null).Where(p => resourceIds.Contains((double)p.ResourceId) && p.TaskStatus != DispatchTaskStatus.Closed && p.TaskStatus != DispatchTaskStatus.Finished);

            //if (!key.IsNullOrEmpty())
            //{
            //    q.Where(p => p.No.Contains(key) || p.Resource.Code.Contains(key) || p.Resource.Name.Contains(key));
            //}
            //var tasks = q.OrderByDescending(p => p.CreateDate).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //获取任务单已经扫描过的模具记录
            //var taskIds = tasks.Select(p => p.Id).Distinct().ToList();
            //var records = taskIds.SplitContains(ids =>
            //{
            //    return Query<PreStartupSetupRecord>().Where(p => ids.Contains(p.DispatchTaskId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //});            
            foreach (var task in tasks.OrderBy(p => p.PlanBeginTime))
            {
                PreStartupSetupModelTaskInfo info = new PreStartupSetupModelTaskInfo();
                info.TaskId = task.Id;
                info.TaskNo = task.No;
                info.WorkOrderNo = task.WorkOrderNo;
                info.Resource = task.ResourceCode;
                info.ResourceName = task.ResourceName;
                info.ProductCode = task.ProductCode;
                info.ProductName = task.ProductName;
                info.Qty = task.DispatchQty;
                //info.Process = task.ProcessCode;
                info.Process = task.ProcessName;

                //先找到这个任务单这个设备，上一个开机记录的任务单(用这个设备的任务单)
                PreStartupSetupRecord record = Query<PreStartupSetupRecord>().Where(p => p.DispatchTask.ResourceId == task.ResourceId).OrderByDescending(p => p.UpdateDate).FirstOrDefault();
                if (record != null)
                {
                    //然后查找上个任务单需要用的模具，然后通过模具+机台(资源)
                    var rs = Query<PreStartupSetupRecord>().Join<EquipAccount>((x, y) => x.ToolCode == y.Code).Join<EquipAccount, EquipAccountItem>((x, y) => x.Id == y.EquipAccountId && y.ItemId == record.DispatchTask.ProductId && y.ProcessId == record.DispatchTask.ProcessId).Where(p => p.DispatchTask.ResourceId == record.DispatchTask.ResourceId && p.CheckerFixtureType == CheckerFixtureType.Mold).ToList(null, new EagerLoadOptions().LoadWithViewProperty());//records.Where(p => p.DispatchTaskId == task.Id).ToList();
                    foreach (var r in rs.GroupBy(p => p.ToolCode).Select(p => p.OrderByDescending(p => p.UpdateDate).FirstOrDefault()))
                    {
                        if (r.ToolState != "上模")
                            continue;
                        info.ScannedModel.Add(new Pda_PreStartupSetupScanEquipAccountInfo()
                        {
                            TaskId = r.DispatchTaskId,
                            Code = r.ToolCode,
                            Name = r.ToolName,
                            ScanTime = r.CreateDate,
                            Type = r.CheckerFixtureType.ToLabel()
                        });
                    }
                }
                infos.Add(info);
            }
            return infos;
        }

        /// <summary>
        /// 开机准备:上模校验模具(true:不用确认;false:需要弹窗确认)
        /// </summary>
        /// <param name="TaskId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [ApiService("开机准备:上模校验模具")]
        public virtual List<Pda_ValidPreStartupSetupEquipAccountInfo> ValidPreStartupSetupEquipAccount(double TaskId)
        {
            #region 以前逻辑
            //Pda_ValidPreStartupSetupEquipAccountInfo info = new Pda_ValidPreStartupSetupEquipAccountInfo();
            ////当上一个修改时间最接近的任务单，使用过这个模具，那么就返回false，看是否继续
            //var record = Query<PreStartupSetupRecord>().Where(p => p.DispatchTaskId != TaskId && p.CheckerFixtureType == CheckerFixtureType.Mold).OrderByDescending(p => p.UpdateDate).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            //if (record == null)
            //    return null;
            //var equipAccountItems = Query<EquipAccountItem>().Join<DispatchTask>((x, y) => x.ProcessId == y.ProcessId && x.ItemId == y.ProductId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //if (equipAccountItems.Count < 1)
            //    return null;
            //var equipAccountCodes = equipAccountItems.Select(p => p.EquipAccountCode).Distinct().ToList();
            //if (equipAccountCodes.Any(p => p == record.ToolCode))
            //{
            //    info.RecordId = record.Id;
            //    info.Model = record.ToolCode;
            //    return info;
            //}
            //else
            //    return null;
            #endregion
            List<Pda_ValidPreStartupSetupEquipAccountInfo> list = new List<Pda_ValidPreStartupSetupEquipAccountInfo>();

            var DispatchTask = RF.GetById<DispatchTask>(TaskId, new EagerLoadOptions().LoadWithViewProperty());
            var EquipAccountItemList = Query<EquipAccountItem>().Where(p => p.ItemId == DispatchTask.ProductId && p.ProcessId == DispatchTask.ProcessId).ToList();
            var codes = EquipAccountItemList.Select(p => p.EquipAccount.Code);
            //获取这个任务单下的设备

            //先找到这个任务单这个设备，上一个开机记录的任务单(用这个设备的任务单)
            PreStartupSetupRecord record = Query<PreStartupSetupRecord>().Where(p => p.DispatchTask.ResourceId == DispatchTask.ResourceId).OrderByDescending(p => p.UpdateDate).FirstOrDefault();

            if (record == null)
                return null;

            //然后查找上个任务单需要用的模具，然后通过模具+机台(资源)
            var record1 = Query<PreStartupSetupRecord>().Join<EquipAccount>((x, y) => x.ToolCode == y.Code).Join<EquipAccount, EquipAccountItem>((x, y) => x.Id == y.EquipAccountId && y.ItemId == record.DispatchTask.ProductId && y.ProcessId == record.DispatchTask.ProcessId).Where(p => p.DispatchTask.ResourceId == record.DispatchTask.ResourceId && p.CheckerFixtureType == CheckerFixtureType.Mold).OrderByDescending(p => p.UpdateDate).ToList(null, new EagerLoadOptions().LoadWithViewProperty());//records.Where(p => p.DispatchTaskId == task.Id).ToList();


            if (EquipAccountItemList.Count > 0)//&& EquipAccountItemList.Count == record1.Count
            {
                for (int i = 0; i < EquipAccountItemList.Count; i++)
                {
                    if (record1.Count > 0)
                    {
                        if (record1[i] == null)
                        {
                            return null;
                        }

                        Pda_ValidPreStartupSetupEquipAccountInfo info = new Pda_ValidPreStartupSetupEquipAccountInfo();
                        info.RecordId = record1[i].Id;
                        info.Model = record1[i].ToolCode;
                        list.Add(info);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                return null;
            }
            return list;
            //if (record != null)
            //{
            //    info.RecordId = record.Id;
            //    info.Model = record.ToolCode;
            //    return info;
            //}
            //else
            //    return null;
        }

        /// <summary>
        /// 开机准备:上模校验成功点击确认后提交
        /// </summary>
        [ApiService("开机准备:上模校验成功点击确认后提交")]
        public virtual void ValidSubmmitPreStartupSetupEquipAccount(double TaskId, List<Pda_ValidPreStartupSetupEquipAccountInfo> PreStartupSetupEquipAccountInfos)
        {
            //当确认提交后，直接复制一条出来，除了派工任务单不同
            EntityList<PreStartupSetupRecord> list = new EntityList<PreStartupSetupRecord>();

            //上模
            foreach (var info in PreStartupSetupEquipAccountInfos)
            {
                var record = RF.GetById<PreStartupSetupRecord>(info.RecordId, new EagerLoadOptions().LoadWithViewProperty());
                PreStartupSetupRecord setupRecord = new PreStartupSetupRecord();
                setupRecord.DispatchTaskId = TaskId;
                setupRecord.ToolCode = record.ToolCode;
                setupRecord.ToolName = record.ToolName;
                setupRecord.CheckerFixtureType = record.CheckerFixtureType;
                setupRecord.DrawingNo = record.DrawingNo;
                setupRecord.PersistenceStatus = PersistenceStatus.New;
                setupRecord.ToolState = "上模";
                setupRecord.UniqueCode = record.UniqueCode;
                list.Add(setupRecord);
            }

            //下模
            foreach (var info in PreStartupSetupEquipAccountInfos)
            {
                var record = RF.GetById<PreStartupSetupRecord>(info.RecordId, new EagerLoadOptions().LoadWithViewProperty());
                PreStartupSetupRecord setupRecord = new PreStartupSetupRecord();
                setupRecord.DispatchTaskId = record.DispatchTaskId;
                setupRecord.ToolCode = record.ToolCode;
                setupRecord.ToolName = record.ToolName;
                setupRecord.CheckerFixtureType = record.CheckerFixtureType;
                setupRecord.DrawingNo = record.DrawingNo;
                setupRecord.PersistenceStatus = PersistenceStatus.New;
                setupRecord.UniqueCode = record.UniqueCode;
                setupRecord.ToolState = "下模";
                list.Add(setupRecord);
            }

            RF.Save(list);
        }

        /// <summary>
        ///  开机准备:上模扫描模具
        /// </summary>
        [ApiService("开机准备:上模扫描模具")]
        public virtual Pda_PreStartupSetupScanEquipAccountInfo PreStartupSetupScanEquipAccount(string key, double TaskId)
        {
            var account = Query<EquipAccount>().Where(p => p.Code == key).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (account == null)
            {
                throw new ValidationException("模具不存在".L10N());
            }

            var equipAccountItemConfig = ConfigService.GetConfig(new EquipAccountItemConfig(), typeof(EquipAccountItem));
            //是否需要检验模具穴位
            if (equipAccountItemConfig != null && equipAccountItemConfig.IsValidAcupoint == true)
            {
                //设备台账维护中的穴位必须大于0(不能为空、不能为0、不能非数字)
                if (account.Acupoint.IsNullOrEmpty())
                {
                    throw new ValidationException("模具{0}穴位为空".L10nFormat(account.Code));
                }
                //数字类型转换失败  or  转换成功，但小于等于0
                if (!decimal.TryParse(account.Acupoint, out decimal equipAcupoint) || (decimal.TryParse(account.Acupoint, out equipAcupoint) && equipAcupoint <= 0))
                {
                    throw new ValidationException("模具{0}穴位必须大于0，当前为[{1}]".L10nFormat(account.Code, account.Acupoint));
                }
            }

            var equipAccountItem = Query<EquipAccountItem>().Join<DispatchTask>((x, y) => x.ProcessId == y.ProcessId && x.ItemId == y.ProductId && y.Id == TaskId).Where(p => p.EquipAccountId == account.Id).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (equipAccountItem == null)
            {
                throw new ValidationException("模具不属于当前任务单".L10N());
            }
            //if (equipAccountItem.State == State.Disable)
            //    throw new ValidationException("当前模具与产品的关系为禁用状态".L10N());
            Pda_PreStartupSetupScanEquipAccountInfo info = new Pda_PreStartupSetupScanEquipAccountInfo();
            info.Code = equipAccountItem.EquipAccountCode;
            info.Name = equipAccountItem.EquipAccountName;
            info.ScanTime = DateTime.Now;
            info.TaskId = TaskId;
            info.State = "上模";
            return info;
        }

        /// <summary>
        /// 开机准备:上模更改后/重新判断模具后提交
        /// </summary>
        /// <param name="TaskId"></param>
        /// <param name="infos"></param>
        [ApiService("开机准备:上模更改后/重新判断模具后提交")]
        public virtual void PreStartupSetupEquipAccountChangedSubmmit(double TaskId, List<Pda_PreStartupSetupScanEquipAccountInfo> infos)
        {
            if (TaskId <= 0)
                throw new ValidationException("派工单不能为空".L10N());
            if (infos.Count < 1)
                throw new ValidationException("提交数据不能为空".L10N());

            var DispatchTask = RF.GetById<DispatchTask>(TaskId, new EagerLoadOptions().LoadWithViewProperty());
            var EquipAccountItemList = Query<EquipAccountItem>().Where(p => p.ItemId == DispatchTask.ProductId && p.ProcessId == DispatchTask.ProcessId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            //上模的数据
            //模具
            var pdaSmListData = infos.Where(p => p.State == "上模").ToList();
            var codes = pdaSmListData.Select(p => p.Code).Distinct().ToList();
            //var taskRecords = Query<PreStartupSetupRecord>().Where(p => p.DispatchTaskId == DispatchTask.Id).ToList();
            var equipItems = Query<EquipAccountItem>().Where(p => p.ItemId == DispatchTask.ProductId && p.ProcessId == DispatchTask.ProcessId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var equipAccounts = Query<EquipAccount>().Where(p => codes.Contains(p.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            if (equipItems.Count > 0)
            {
                var notDraws = new List<string>();
                var draws = equipItems.Where(p => !p.UniqueCode.IsNullOrEmpty()).Select(p => p.UniqueCode).Distinct().ToList();
                //有唯一码的时候，每个唯一码必须要有一个开机记录且只要一个就行
                foreach (var draw in draws)
                {
                    if (!equipItems.Any(p => p.UniqueCode == draw && codes.Contains(p.EquipAccountCode)))
                        notDraws.Add(draw);
                    //if (!taskRecords.Any(p => p.CheckerFixtureType == CheckerFixtureType.Mold && p.UniqueCode == draw))
                    //{
                    //    notDraws.Add(draw);
                    //}
                }
                if (notDraws.Count > 0)
                {
                    throw new ValidationException("唯一码【" + string.Join("、", notDraws) + "】未绑定".L10N());
                }
                var notCodes = new List<string>();
                //当没有唯一码的需要校验所有
                foreach (var item in EquipAccountItemList.Where(p => p.UniqueCode.IsNullOrEmpty()))
                {
                    if (equipAccounts.All(p => p.Code != item.EquipAccountCode))
                    {
                        notCodes.Add(item.EquipAccountCode);
                    }
                }
                if (notCodes.Count > 0)
                {
                    throw new ValidationException("模具【" + string.Join("、", notCodes) + "】未绑定".L10N());
                }
            }

            //var pdaSmListData = infos.Where(p => p.State == "上模").ToList();
            //var codes = pdaSmListData.Select(p => p.Code).Distinct().ToList();
            //var equipAccounts = Query<EquipAccount>().Where(p => codes.Contains(p.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //if (EquipAccountItemList.Count > 0)
            //{
            //    //有图号的时候，每个图号必须要有一个就行
            //    var draws = EquipAccountItemList.Where(p => !p.Drawn.IsNullOrEmpty()).Select(p => p.Drawn).Distinct().ToList();
            //    var notDraws = new List<string>();
            //    foreach (var draw in draws)
            //    {
            //        if (equipAccounts.All(p => p.Drawn != draw))
            //        {
            //            notDraws.Add(draw);
            //        }
            //    }
            //    if (notDraws.Count > 0)
            //    {
            //        throw new ValidationException("图号【" + string.Join("、", notDraws) + "】未绑定".L10N());
            //    }
            //    var notCodes = new List<string>();
            //    foreach (var item in EquipAccountItemList.Where(p => p.Drawn.IsNullOrEmpty()))
            //    {
            //        if (equipAccounts.All(p => p.Code != item.EquipAccountCode))
            //        {
            //            notCodes.Add(item.EquipAccountCode);
            //        }
            //    }
            //    if (notCodes.Count > 0)
            //    {
            //        throw new ValidationException("模具【" + string.Join("、", notCodes) + "】未绑定".L10N());
            //    }
            //}
            //foreach (var item in EquipAccountItemList)
            //{
            //if (pdaSmListData.Where(p => p.Code == item.EquipAccount.Code).ToList().Count <= 0)
            //{
            //    throw new ValidationException("模具【" + item.EquipAccount.Code + "】未绑定".L10N());
            //}
            //}

            EntityList<PreStartupSetupRecord> records = new EntityList<PreStartupSetupRecord>();
            foreach (var info in infos)
            {
                PreStartupSetupRecord record = new PreStartupSetupRecord();

                Equipments.EquipAccounts.EquipAccount equipAccount = RT.Service.Resolve<EquipAccountController>().GetEquipAccountsByCode(info.Code);
                var EquipAccountItem = EquipAccountItemList.FirstOrDefault(p => p.EquipAccountId == equipAccount.Id);

                record.DispatchTaskId = info.TaskId;
                record.ToolCode = info.Code;
                record.ToolName = info.Name;
                record.CheckerFixtureType = CheckerFixtureType.Mold;
                record.PersistenceStatus = PersistenceStatus.New;
                record.DrawingNo = equipAccount?.Drawn;
                record.ToolState = info.State;
                record.UniqueCode = EquipAccountItem?.UniqueCode;
                records.Add(record);
            }
            if (records.Count > 0)
            {
                RF.Save(records);
            }
        }

        #endregion

        #region 派工/排程退回
        /// <summary>
        /// 根据资源编码获取任务单(待派工/派工中)列表信息
        /// </summary>
        /// <param name="key">资源编码/工单</param>
        /// <returns></returns>
        [ApiService("根据资源编码获取任务单(待派工/派工中)列表信息")]
        public virtual List<DispatchTaskInfo> GetToDispatchTaskInfos([ApiParameter("资源编码/工单")] string key)
        {
            var resource = RT.Service.Resolve<WipResourceController>().GetWipResourceByCode(key);

            var workCenter = "";
            if (resource != null && resource.SourceType == SyncSourceType.LineAndon)
            {
                //获取安灯产线
                AndonLine andonLine = RT.Service.Resolve<AndonLineController>().GetAndonLineByMachineCode(resource.Code);
                if (andonLine != null)
                {
                    workCenter = andonLine.WorkCenterCode;
                }
            }

            var tasks = Query<DispatchTask>()
                .Where(p => p.TaskStatus <= DispatchTaskStatus.Dispatching
                && (p.Resource.Code.Contains(key)
                || p.Resource.Code == workCenter
                || p.Product.ShortDescription.Contains(key)
                || p.WorkOrder.No.Contains(key)))
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var list = tasks.Select(task => new DispatchTaskInfo()
            {
                TaskId = task.Id,
                TaskNo = task.No,
                TaskStatus = task.TaskStatus.ToLabel(),
                TaskStatusValue = task.TaskStatus,
                ItemCode = task.ProductCode,
                ItemName = task.ProductName,
                TaskQty = task.DispatchQty,
                ReportQty = task.RemainQty,
                RemainQty = task.RemainQty,
                MaxRemainQty = RT.Service.Resolve<DispatchController>().MaxReportQtyAndMaxRemainQty(task).Item2,//task.MaxRemainQty,
                Priority = task.Priority.ToLabel(),
                PriorityValue = task.Priority,
                ResourceId = task.ResourceId,
                ResourceName = task.ResourceName,
                ProcessId = task.ProcessId,
                ProcessName = task.ProcessName,
                PlanBeginDate = task.PlanBeginTime,
                PlanEndDate = task.PlanEndTime,
                ActualBeginDate = task.BeginTime,
                ActualEndDate = task.EndTime,
                //CanStart = ValidateIsCanStartTask(config, task.PlanBeginTime, task.Priority, false),
                IsSyntype = task.IsSyntype,
                //IsCanQuickReport = saveRecord.Any(p => p == task.Id),
                WorkOrderId = task.WorkOrder.Id,
                WorkOrderNo = task.WorkOrder.No,
                //Zcode = layoutInfo?.Zcode ?? 0,
                OldItemCode = task.ShortDescription,
            }).ToList();

            return list;
        }

        /// <summary>
        /// 派工校验是否派工到资源
        /// </summary>
        /// <param name="taskIds"></param>
        /// <returns></returns>
        [ApiService("派工校验是否派工到资源")]
        public virtual List<Pda_WipResourceInfo> DispacthWipResources(List<double> taskIds)
        {
            //获取需要派工到资源的任务单ID
            var ids = RT.Service.Resolve<DispatchController>().GetWorkOrderRoutingProcessesByTaskIds(taskIds);
            if (ids.Count == 0)
                return null;

            var tasks = RT.Service.Resolve<DispatchController>().GetDispatchTasks(ids);

            if (!(tasks.GroupBy(p => p.ResourceSourceType).Count() == 1 && tasks.GroupBy(p => p.ResourceId).Count() == 1) && tasks.GroupBy(p => p.ProductId).Count() == 1)
                throw new ValidationException("只有同一个工作中心下且相同资源、相同产品的情况，才能一起派工到同一资源上");

            var task = tasks.FirstOrDefault();
            var resource = RT.Service.Resolve<WipResourceController>().GetWipResourceByCode(task.ResourceCode);

            var workCenter = "";
            if (resource.SourceType == SyncSourceType.LineAndon)
            {
                //获取安灯产线
                AndonLine andonLine = RT.Service.Resolve<AndonLineController>().GetAndonLineByMachineCode(resource.Code);
                if (andonLine != null)
                {
                    workCenter = andonLine.WorkCenterCode;
                }
            }
            else if (resource.SourceType == SyncSourceType.WorkCenter)
            {
                workCenter = resource.Code;
            }

            var list = RT.Service.Resolve<WipResourceController>().GetWipResourcesByWorkCenterCode(workCenter, tasks.Select(p => p.ProductId).Distinct().ToList());

            List<Pda_WipResourceInfo> resourceInfos = new List<Pda_WipResourceInfo>();
            foreach (var l in list)
            {
                Pda_WipResourceInfo wipResourceInfo = new Pda_WipResourceInfo();
                wipResourceInfo.ResourceId = l.Id;
                wipResourceInfo.Code = l.Code;
                wipResourceInfo.Name = l.Name;

                resourceInfos.Add(wipResourceInfo);
            }

            if (resourceInfos.Count > 0)
                return resourceInfos;
            else
                return null;
        }

        /// <summary>
        /// 提交派工/排程退回数据
        /// </summary>
        /// <param name="taskInfos"></param>
        /// <param name="type">1为派工,2为排程退回</param>
        /// <param name="reason">排程退回原因</param>
        [ApiService("提交派工/排程退回数据")]
        public virtual void SubmitDispatchTaskInfos([ApiParameter("任务单列表")] List<DispatchTaskInfo> taskInfos, [ApiParameter("提交类型: 1为派工,2为排程退回")] int type, [ApiParameter("排程退回原因")] string reason, double? DispacthResourceId = null)
        {
            if (taskInfos == null || taskInfos.Count == 0)
                throw new ValidationException("提交数据为空");
            var taskIds = taskInfos.Select(p => p.TaskId).Distinct().ToList();
            if (type == 1)
            {
                using (var tran = DB.TransactionScope(TaskManagementEntityDataProvider.ConnectionStringName))
                {
                    if (DispacthResourceId != null)
                    {
                        //获取需要派工到资源的任务单ID
                        var ids = RT.Service.Resolve<DispatchController>().GetWorkOrderRoutingProcessesByTaskIds(taskIds);
                        if (ids.Count > 0)
                        {
                            //更新任务单资源
                            RT.Service.Resolve<DispatchController>().UpdateTaskResource(ids, DispacthResourceId.Value);
                        }
                    }

                    //派工
                    var errMsg = RT.Service.Resolve<DispatchController>().DispatchTasks(taskIds);
                    if (errMsg.IsNotEmpty())
                        throw new ValidationException(errMsg);

                    tran.Complete();
                }
            }
            else if (type == 2)
            {
                //排程退回
                if (reason.IsNullOrEmpty())
                    throw new ValidationException("请输入排程退回原因");
                RT.Service.Resolve<DispatchController>().SchedulingInfReturn(taskIds, reason);
            }
        }
        #endregion

        #region 撤消派工

        /// <summary>
        /// 根据资源编码获取(已派工)任务单列表信息
        /// </summary>
        /// <param name="key">资源编码/工单</param>
        /// <returns></returns>
        [ApiService("根据资源编码获取任务单(已派工)列表信息")]
        public virtual List<DispatchTaskInfo> GetDispatchedTaskInfos([ApiParameter("资源编码")] string key)
        {
            var resource = RT.Service.Resolve<WipResourceController>().GetWipResourceByCode(key);

            var workCenter = "";
            if (resource!= null && resource.SourceType == SyncSourceType.LineAndon) {
                //获取安灯产线
                AndonLine andonLine = RT.Service.Resolve<AndonLineController>().GetAndonLineByMachineCode(resource.Code);
                if (andonLine != null)
                {
                    workCenter = andonLine.WorkCenterCode;
                }
            }
            //获取对应资源，如果资源是安灯产线，那么同时，也要获取该产线的工作中心下的任务单
            var tasks = Query<DispatchTask>().Where(p => p.TaskStatus == DispatchTaskStatus.Dispatched && (p.Resource.Code.Contains(key) || p.Resource.Code == workCenter || p.Product.ShortDescription.Contains(key) || p.WorkOrder.No.Contains(key))).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            var list = tasks.Select(task => new DispatchTaskInfo()
            {
                TaskId = task.Id,
                TaskNo = task.No,
                TaskStatus = task.TaskStatus.ToLabel(),
                TaskStatusValue = task.TaskStatus,
                ItemCode = task.ProductCode,
                ItemName = task.ProductName,
                TaskQty = task.DispatchQty,
                ReportQty = task.RemainQty,
                RemainQty = task.RemainQty,
                MaxRemainQty = RT.Service.Resolve<DispatchController>().MaxReportQtyAndMaxRemainQty(task).Item2,//task.MaxRemainQty,
                Priority = task.Priority.ToLabel(),
                PriorityValue = task.Priority,
                ResourceId = task.ResourceId,
                ResourceName = task.ResourceName,
                ProcessId = task.ProcessId,
                ProcessName = task.ProcessName,
                PlanBeginDate = task.PlanBeginTime,
                PlanEndDate = task.PlanEndTime,
                ActualBeginDate = task.BeginTime,
                ActualEndDate = task.EndTime,
                //CanStart = ValidateIsCanStartTask(config, task.PlanBeginTime, task.Priority, false),
                IsSyntype = task.IsSyntype,
                //IsCanQuickReport = saveRecord.Any(p => p == task.Id),
                WorkOrderId = task.WorkOrder.Id,
                WorkOrderNo = task.WorkOrder.No,
                //Zcode = layoutInfo?.Zcode ?? 0,
            }).ToList();

            return list;
        }

        /// <summary>
        /// 提交撤消派工数据
        /// </summary>
        /// <param name="taskInfos">资源编码</param>
        [ApiService("提交撤消派工数据")]
        public virtual void SubmitCancelDispatchTaskInfos([ApiParameter("任务单列表")] List<DispatchTaskInfo> taskInfos)
        {
            if (taskInfos == null || taskInfos.Count == 0)
                throw new ValidationException("提交数据为空");
            var taskIds = taskInfos.Select(p => p.TaskId).Distinct().ToList();
            var errMsg = RT.Service.Resolve<DispatchController>().CancelDispatchTasks(taskIds);
            if (errMsg.IsNotEmpty())
                throw new ValidationException(errMsg); ;
        }
        #endregion
    }
}
