using SIE.Api;
using SIE.Common.Attachments;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Common.Utils;
using SIE.EMS.Enums;
using SIE.EMS.InventoryPlans;
using SIE.EMS.InventoryTasks.ApiModels;
using SIE.EMS.InventoryTasks.ViewModels;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.EMS.InventoryTasks
{
    /// <summary>
    /// 备件盘点PDA接口控制器
    /// </summary>
    public class InventoryTaskSparePartController : DomainController
    {

        /// <summary>
        /// 获取备件盘点任务
        /// </summary>
        /// <param name="pdaType"></param>
        /// <returns></returns>
        [ApiService("获取备件盘点任务")]
        [return: ApiReturn("备件盘点任务")]
        public virtual List<InventorySparePartInfo> GetSparePartInventoryTaskInfos([ApiParameter("盘点PDA类型")] int pdaType)
        {
            var list = new List<InventorySparePartInfo>();
            var curId = RT.IdentityId;
            var query = Query<InventoryTask>().Where(p => p.InventoryPlan.InventoryAssetObject == InventoryAssetObject.Spare);
            if (pdaType == 1)
            {
                //只能查询创建人为当前用户或者盘点人页签存在当前用户且有初盘权限的数据
                query.Where(p => p.InventoryTaskStatus == InventoryTaskStatus.Doing);
                query.Exists<InventoryTaskSparePartCounter>((a, y) => y.Where(b => (a.CreateBy == curId || (b.EmployeeId == curId && b.First)) && a.Id == b.InventoryTaskId));
            }
            else if (pdaType == 2)
            {
                //只能查询创建人为当前用户或者盘点人页签存在当前用户且有复盘权限的数据
                query.Where(p => p.InventoryTaskStatus == InventoryTaskStatus.FirstDone || p.InventoryTaskStatus == InventoryTaskStatus.ScondDoing);
                query.Exists<InventoryTaskSparePartCounter>((a, y) => y.Where(b => (a.CreateBy == curId || (b.EmployeeId == curId && b.Second)) && a.Id == b.InventoryTaskId));
            }
            else
            {
                throw new ValidationException("获取备件盘点任务失败，请输入正确的参数".L10N());
            }
            var tasks = query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var planIds = tasks.Select(p => p.InventoryPlanId).ToList();
            var ranges = RT.Service.Resolve<InventoryPlanController>().GetInventoryPlanSpareParts(planIds);
            var now = RF.Find<InventoryTask>().GetDbTime();
            foreach (var task in tasks)
            {
                var info = new InventorySparePartInfo();
                info.InventoryTaskId = task.Id;
                info.TaskNo = task.TaskNo;
                info.PlanEndDate = task.PlanEndDate;
                info.InventoryType = task.InventoryType;
                info.FactoryId = task.FactoryId;
                info.NeedPhoto = task.NeedPhoto;
                info.Remark = task.Remark;
                info.InventoryTaskStatus = task.InventoryTaskStatus.ToLabel().L10N();
                info.Progress = task.PercentageString;
                info.IsOverdue = now.Date > task.PlanEndDate;
                var range = ranges.FirstOrDefault(p => p.InventoryPlanId == task.InventoryPlanId);
                if (range != null)//盘点范围
                {
                    info.SparePartType = range.PartType?.ToLabel();
                    info.ItemCategoryName = range.ItemCategoryName;
                    info.WarehouseName = task.WarehouseName;
                }
                list.Add(info);
            }
            return list;
        }

        /// <summary>
        /// 备件盘点执行-扫描设备编码/RFID
        /// </summary>
        /// <param name="pdaType">盘点PDA类型:1-盘点执行,2-复盘执行</param>
        /// <param name="code">扫描条码</param>
        /// <param name="taskId">盘点任务id</param>
        [ApiService("备件盘点执行-扫描设备编码/RFID")]
        [return: ApiReturn("设备盘点执行扫描结果")]
        public virtual List<InventorySparePartExcuteInfo> SparePartInventoryExecute([ApiParameter("盘点PDA类型")] int pdaType, [ApiParameter("扫描条码")] string code,
            [ApiParameter("盘点任务id")] double taskId)
        {

            List<InventorySparePartExcuteInfo> result = new List<InventorySparePartExcuteInfo>();
            var task = RF.GetById<InventoryTask>(taskId, new EagerLoadOptions().LoadWithViewProperty());
            if (task == null)
            {
                throw new ValidationException("盘点任务信息丢失，请确认".L10N());
            }
            //扫描的编码作为设备编码或RFID获取设备台账
            EntityList<InventoryTaskSparePartDetail> sparePartDetailList;
            if (!code.IsNullOrEmpty())
            {
                sparePartDetailList = Query<InventoryTaskSparePartDetail>().Where(p => (p.SparePart.SparePartCode == code || p.Sn == code || p.LotNo == code || p.StorageLocation.Code == code)
             && p.InventoryTaskId == taskId)
                   .WhereIf(pdaType == 1, m => m.FirstResult == null || m.InventoryStatus == InventoryStatus.Not)
                   .WhereIf(pdaType == 2, m => m.SecondResult == null && m.InventoryStatus == InventoryStatus.Done)
                   .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                if (!sparePartDetailList.Any())
                {
                    throw new ValidationException("扫描条码无相关信息，请重新输入".L10N());
                }
            }
            else
            {
                sparePartDetailList = Query<InventoryTaskSparePartDetail>().Where(p => p.InventoryTaskId == taskId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            }
            //盘点任务的设备清单
            if (pdaType == 1)//初盘
            {
                var firstInvList = sparePartDetailList.Where(m => !m.FirstResult.HasValue || m.InventoryStatus == InventoryStatus.Not).ToList();
                foreach (var firstInv in firstInvList)
                {
                    SetDetailInfo(pdaType, task, result, firstInv);
                }


            }
            else if (pdaType == 2)//复盘
            {
                var secondInvList = sparePartDetailList.Where(m => m.FirstResult.HasValue && !m.SecondResult.HasValue).ToList();
                foreach (var secondInv in secondInvList)
                {
                    SetDetailInfo(pdaType, task, result, secondInv);
                }
            }
            else
            {
                throw new ValidationException("备件盘点执行失败，请输入正确的参数".L10N());
            }
            return result;

        }

        /// <summary>
        /// 备件盘点提交
        /// </summary>
        /// <param name="pdaResultInfo"></param>
        /// <returns></returns>
        [ApiService("备件盘点执行-提交")]
        [return: ApiReturn("")]
        public virtual void SparePartInventorySubmit([ApiParameter("提交结果")] List<InventorySparePartExcuteInfo> pdaResultInfo)
        {
            if (!pdaResultInfo.Any())
            {
                throw new ValidationException("备件盘点执行失败，请输入正确的参数".L10N());
            }
            var ids = pdaResultInfo.Select(m => m.Id).ToList();
            var sparePartList = Query<InventoryTaskSparePartDetail>().Where(m => ids.Contains(m.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var hadResult = new EntityList<InventoryTaskSparePartDetail>();
            var now = RF.Find<InventoryTaskSparePartDetail>().GetDbTime();
            foreach (var saveInfo in pdaResultInfo)
            {
                //序列号类型看ProduceState ，编码和批次号看CountStr 为结果
                if ((ControlMethod)saveInfo.ControlMode == ControlMethod.Sn)//序列号看List
                {
                    var detail = sparePartList.FirstOrDefault(m => m.Id == saveInfo.Id);
                    if (saveInfo.ProduceState.Any(m => m.isBoole) && detail != null)
                    {
                        detail.InventoryStatus = InventoryStatus.Done;
                        var resultType = saveInfo.ProduceState.FirstOrDefault(m => m.isBoole).Type;
                        SetResult(detail, resultType, saveInfo.PdaType);
                        SetQtyInfo(now, saveInfo, detail);
                        hadResult.Add(detail);
                    }
                }
                else
                {
                    var detail = sparePartList.FirstOrDefault(m => m.Id == saveInfo.Id);
                    if (saveInfo.CountStr != "" && detail != null && saveInfo.InputNgQty != null && saveInfo.InputPassQty != null)
                    {
                        detail.InventoryStatus = InventoryStatus.Done;
                        SetLotCodeInfo(now, saveInfo, detail);
                        hadResult.Add(detail);
                    }
                }
            }

            if (hadResult.Any())
            {
                EntityList<InventoryTask> saveList = new EntityList<InventoryTask>();
                var task = RF.GetById<InventoryTask>(hadResult.First().InventoryTaskId);
                saveList.Add(task);
                RT.Service.Resolve<InventoryTaskSpartPartController>().SaveSparePartTaskList(saveList, true, hadResult);
            }
            else
            {
                throw new ValidationException("未提交任何数据,请确认盘点数据是否填写完整".L10N());
            }
        }

        /// <summary>
        /// 获取盘点明细
        /// </summary>
        /// <param name="invetorySpareDetailId"></param>
        /// <param name="Pdatype"></param>
        /// <returns></returns>
        [ApiService("获取盘点明细")]
        [return: ApiReturn("返回指定ID的盘点明细")]
        public virtual InventorySpartPartDetailInfo GetInventorySparePartDetailInfo([ApiParameter("盘点明细ID")] double invetorySpareDetailId, int Pdatype)
        {
            var detail = Query<InventoryTaskSparePartDetail>().Where(m => m.Id == invetorySpareDetailId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (detail == null)
            {
                throw new ValidationException("无相关信息,请重新选择".L10N());
            }
            var task = RF.GetById<InventoryTask>(detail.InventoryTaskId, new EagerLoadOptions().LoadWithViewProperty());
            if (task == null)
            {
                throw new ValidationException("盘点任务信息无法找到,请确认".L10N());
            }
            InventorySpartPartDetailInfo inventorySpartPartDetailInfo = new InventorySpartPartDetailInfo();
            var pics = RT.Service.Resolve<SparePartController>().GetSparePartPictureAttachments(new List<double> { detail.SparePartId });
            
            foreach (var pic in pics)
            {
                var picBase64Str = FileUrlHelper.GetAttachmentBase64StringData(pic.FilePath, pic.FileName);
                inventorySpartPartDetailInfo.PhotoBase64List.Add(picBase64Str);
            }
            inventorySpartPartDetailInfo.ControlMethod = detail.ControlMethod.ToLabel();
            inventorySpartPartDetailInfo.ControlMethodValue = detail.ControlMethod;
            inventorySpartPartDetailInfo.SparePartCode = detail.SparePartCode;
            inventorySpartPartDetailInfo.SparePartName = detail.SparePartName;
            inventorySpartPartDetailInfo.SpartType = detail.SpartType.ToLabel();
            inventorySpartPartDetailInfo.Specification = detail.Specification;

            inventorySpartPartDetailInfo.StorageLocationName = detail.StorageLocationName;
            inventorySpartPartDetailInfo.PdaType = Pdatype;
            inventorySpartPartDetailInfo.RealNg = Pdatype == 1 ? detail.FirstNg : detail.SecondNgQty;
            inventorySpartPartDetailInfo.RealGood = Pdatype == 1 ? detail.FirstGood : detail.SecondGoodQty;
            inventorySpartPartDetailInfo.Sn = detail.Sn;
            inventorySpartPartDetailInfo.Total = detail.Total;
            inventorySpartPartDetailInfo.NgQty = detail.NgQty;
            inventorySpartPartDetailInfo.LotNo = detail.LotNo;
            inventorySpartPartDetailInfo.GoodQty = detail.GoodQty;
            inventorySpartPartDetailInfo.Id = detail.Id;
            inventorySpartPartDetailInfo.InventoryType = ((int)task.InventoryExecuteType).ToString();
            if (!task.PhotoFilePath.IsNullOrWhiteSpace())
            {
                var strs = task.PhotoFilePath.Split('/');
                var fileName = strs[strs.Length - 1];
                var fileBytes = RT.Service.Resolve<AttachmentController>().FileDownload(task.PhotoFilePath, fileName);
                inventorySpartPartDetailInfo.PictureFileName = fileName;
                inventorySpartPartDetailInfo.Picture = Convert.ToBase64String(fileBytes);
            }
            return inventorySpartPartDetailInfo;
        }


        /// <summary>
        /// 提交备件盘点明细确认
        /// </summary>
        /// <param name="inventorySpartPartDetailInfo"></param>
        [ApiService("提交备件盘点明细确认")]
        [return: ApiReturn("")]
        public virtual void InventorySpartPartDetailInfo([ApiParameter("盘点明细")] InventorySpartPartDetailInfo inventorySpartPartDetailInfo)
        {
            if (inventorySpartPartDetailInfo == null)
            {
                throw new ValidationException("请输入正确的参数".L10N());
            }
            var now = RF.Find<InventoryTaskSparePartDetail>().GetDbTime();
            var sparePartDetail = Query<InventoryTaskSparePartDetail>().Where(m => m.Id == inventorySpartPartDetailInfo.Id).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (sparePartDetail != null)
            {
                sparePartDetail.InventoryStatus = InventoryStatus.Done;
                if (inventorySpartPartDetailInfo.PdaType == 1)//初盘
                {
                    sparePartDetail.FirstCounterId = RT.IdentityId;
                    sparePartDetail.FirstGood = inventorySpartPartDetailInfo.RealGood;
                    sparePartDetail.FirstNg = inventorySpartPartDetailInfo.RealNg;
                    sparePartDetail.FirstTotal = sparePartDetail.FirstGood + sparePartDetail.FirstNg;
                    sparePartDetail.FirstDiff = sparePartDetail.FirstTotal - sparePartDetail.Total;
                    sparePartDetail.FirstDateTime = now;
                }
                if (inventorySpartPartDetailInfo.PdaType == 2)//复盘
                {
                    sparePartDetail.SecondCounterId = RT.IdentityId;
                    sparePartDetail.SecondGoodQty = inventorySpartPartDetailInfo.RealGood;
                    sparePartDetail.SecondNgQty = inventorySpartPartDetailInfo.RealNg;
                    sparePartDetail.SecondTotal = sparePartDetail.SecondGoodQty + sparePartDetail.SecondNgQty;
                    sparePartDetail.SecondDiff = sparePartDetail.SecondTotal - sparePartDetail.Total;
                    sparePartDetail.SecondDateTime = now;
                }
                EntityList<InventoryTask> saveList = new EntityList<InventoryTask>();
                var task = RF.GetById<InventoryTask>(sparePartDetail.InventoryTaskId);
                if (!inventorySpartPartDetailInfo.Picture.IsNullOrWhiteSpace())
                {
                    var bytes = Convert.FromBase64String(inventorySpartPartDetailInfo.Picture);
                    const string prePath = "InventoryPlanPhoto";
                    var path = $"{prePath}/{Guid.NewGuid()}";
                    RT.Service.Resolve<SIE.Common.Attachments.AttachmentController>().FileStorage(inventorySpartPartDetailInfo.PictureFileName, bytes, path);
                    task.PhotoFilePath = $"{path}/{inventorySpartPartDetailInfo.PictureFileName}";
                }

                saveList.Add(task);
                RT.Service.Resolve<InventoryTaskSpartPartController>().SaveSparePartTaskList(saveList, true, new EntityList<InventoryTaskSparePartDetail>() { sparePartDetail });
            }
        }

        /// <summary>
        /// 备件盘盈扫描
        /// </summary>
        /// <param name="PdaType"></param>
        /// <param name="Code"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [ApiService("备件盘盈扫描")]
        [return: ApiReturn("")]
        public virtual AddSparePartProfitViewModel ScandInventorySparePartAddInfo([ApiParameter("PDA盘点状态")] int PdaType, [ApiParameter("扫描的条码")] string code, [ApiParameter("扫描的条码")] double taskId)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ValidationException("请输入备件编码/序列号/批次号！".L10N());
            }
            if (PdaType <= 0)
            {
                throw new ValidationException("参数异常，请检查页面参数".L10N());
            }
            if (taskId <= 0)
            {
                throw new ValidationException("盘点任务ID参数异常，请检查页面参数".L10N());
            }

            var result = new AddSparePartProfitViewModel();
            result.PdaType = PdaType;
            result.InventoryTaskId = taskId;
            //获取盘点范围
            var inventoryTaskSparePartScope = Query<InventoryTaskSparePartScope>().Where(m => m.InventoryTaskId == taskId).FirstOrDefault();
            var storeSummaryLot = Query<StoreSummaryLot>().Where(x => x.BatchNumber == code).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (storeSummaryLot != null)//批次号有数据
            {
                result.SparePartCode = storeSummaryLot.SparePartCode;
                result.SparePartName = storeSummaryLot.SparePartName;
                result.SparePartId = storeSummaryLot.SparePartId;
                result.ControlMethod = storeSummaryLot.ControlMethod;
                result.WarehouseId = inventoryTaskSparePartScope.WarehouseId;//盘点范围的仓库
                result.LotNo = storeSummaryLot.BatchNumber;
                return result;
            }

            var storeSummaryDetail = Query<StoreSummaryDetail>().Where(x => x.OrderNumberCode == code).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (storeSummaryDetail != null)//序列号
            {
                var taskSparePartDetailList = Query<InventoryTaskSparePartDetail>().Where(m => m.InventoryTaskId == taskId && m.Sn == code).FirstOrDefault();
                if (taskSparePartDetailList != null)
                {
                    throw new ValidationException("序列号【{0}】已存在备件清单中！".L10nFormat(code));
                }
                result.SparePartCode = storeSummaryDetail.SparePartCode;
                result.SparePartName = storeSummaryDetail.SparePartName;
                result.SparePartId = storeSummaryDetail.SparePartId;
                result.ControlMethod = storeSummaryDetail.ControlMethod;
                result.WarehouseId = inventoryTaskSparePartScope.WarehouseId;//盘点范围的仓库
                result.LotNo = storeSummaryDetail.OrderNumberCode;
                return result;
            }

            var storeSummary = Query<StoreSummary>().Where(m => m.SparePart.SparePartCode == code).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (storeSummary != null)
            {
                result.SparePartCode = storeSummary.SparePartCode;
                result.SparePartName = storeSummary.SparePartName;
                result.SparePartId = storeSummary.SparePartId;
                result.ControlMethod = storeSummary.ControlMethod;
                result.WarehouseId = inventoryTaskSparePartScope.WarehouseId;//盘点范围的仓库
                return result;
            }

            throw new ValidationException("扫描的条码【{0}】获取不到数据".L10nFormat(code));
        }


        /// <summary>
        /// 生成序列号或批次号
        /// </summary>
        /// <param name="createSn"></param>
        /// <returns></returns>
        [ApiService("生成序列号或批次号")]
        [return: ApiReturn("")]
        public virtual string GetBatchNumberNoOrSn([ApiParameter("是否生成SN")] bool createSn)
        {
            var ctr = RT.Service.Resolve<SparePartController>();
            return createSn ? ctr.GetSnCode(1).First() : ctr.GetBatchCode();
        }

        /// <summary>
        /// 提交备件盘盈新增
        /// </summary>
        /// <param name="model"></param>
        [ApiService("提交备件盘盈新增")]
        [return: ApiReturn("")]
        public virtual void SparePartInventoryProfitSubmit([ApiParameter("盘盈新增数据")] AddSparePartProfitViewModel model)
        {
            if (model == null)
            {
                throw new ValidationException("新增盘盈执行失败，请输入正确的参数".L10N());
            }
            if (model.ControlMethod == ControlMethod.Sn)
            {
                if ((model.GoodQty == 1 && model.NgQty == 1) || (model.GoodQty == 0 && model.NgQty == 0))
                {
                    throw new ValidationException("序列号管控时，实盘良品数、实盘不良品数只能一个值为0,另一个值为1".L10N());
                }
                model.IsGood = model.GoodQty == 1;
                model.IsNg = model.NgQty == 1;
            }
            RT.Service.Resolve<InventoryTaskSpartPartController>().AddProfit(model);
        }

        /// <summary>
        /// 获取备件清单界面待盘点列表
        /// </summary>
        /// <param name="PdaType"></param>
        /// <param name="taskId"></param>
        [ApiService("获取备件清单列表")]
        [return: ApiReturn("返回备件清单")]
        public virtual List<InventorySparePartListInfo> GetInventoryTaskSpartPartList([ApiParameter("PDA盘点状态")] int PdaType, [ApiParameter("任务Id")] double taskId)
        {
            var result = new List<InventorySparePartListInfo>();
            if (PdaType <= 0 || taskId <= 0)
            {
                throw new ValidationException("获取备件清单列表失败,请输入正确的参数".L10N());
            }
            //未盘点
            GetNotInvSparePartInfoList(PdaType, taskId, result);
            //已盘点
            GetInventorySpartpartDoneInfo(PdaType, taskId, result);

            return result;
        }

        /// <summary>
        /// 获取盘点备件清单(未盘点)查看更多
        /// </summary>
        /// <param name="PdaType">1-初盘 2复盘</param>
        /// <param name="taskId">盘点任务Id</param>
        /// <param name="sparePartId">备件Id</param>
        /// <returns></returns>
        [ApiService("获取备件未盘点清单-查看更多列表")]
        [return: ApiReturn("返回未盘点清单的更多信息")]
        public virtual List<InventorySparePartListInfo> GetInventorySpartpartMoreInfo([ApiParameter("PDA盘点状态")] int PdaType, [ApiParameter("任务Id")] double taskId, [ApiParameter("备件Id")] double sparePartId)
        {
            var result = new List<InventorySparePartListInfo>();
            if (PdaType <= 0 || taskId <= 0)
            {
                throw new ValidationException("获取备件清单详细列表失败,请输入正确的参数".L10N());
            }
            var details = Query<InventoryTaskSparePartDetail>().Where(p => p.InventoryTaskId == taskId && p.SparePartId == sparePartId)
                .WhereIf(PdaType == 1, p => p.FirstResult == null)
                .WhereIf(PdaType == 2, p => p.FirstResult != null && p.SecondResult == null)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            details.ForEach(item =>
            {
                var res = new InventorySparePartListInfo();
                res.SparePartCode = item.SparePartCode;
                res.SparePartName = item.SparePartName;
                res.Sn = item.Sn;
                res.PassQty = item.GoodQty;
                res.NgQty = item.NgQty;
                res.PdaType = PdaType;
                res.SparePartId = item.SparePartId;
                res.LotNo = item.LotNo;
                res.Location = item.StorageLocationName;
                res.ControlMethod = (int)item.ControlMethod;
                res.Id = item.Id;
                if (item.ControlMethod == ControlMethod.Sn)
                {
                    res.QualityList.ForEach(it =>
                    {
                        if (it.Value == 5 && item.GoodQty == 1)
                        { it.isBoole = true; }
                        if (it.Value == 10 && item.NgQty == 1)
                        {
                            it.isBoole = true;
                        }
                    });
                }
                result.Add(res);
            });

            return result;
        }

        /// <summary>
        /// 获取未盘点的备件清单
        /// </summary>
        /// <param name="PdaType"></param>
        /// <param name="taskId"></param>
        /// <param name="result"></param>
        private void GetNotInvSparePartInfoList(int PdaType, double taskId, List<InventorySparePartListInfo> result)
        {
            if (PdaType == 1)//初盘
            {
                //取所有未盘点的明细
                var allInventoryTaskSparePartDetails = Query<InventoryTaskSparePartDetail>().Where(p => p.InventoryTaskId == taskId && p.InventoryStatus != InventoryStatus.Done)
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                if (allInventoryTaskSparePartDetails.Any())
                {
                    GetSpartPartListInfo(taskId, PdaType, result, allInventoryTaskSparePartDetails);
                }
            }
            if (PdaType == 2)//复盘
            {
                //取所有未盘点的明细
                var allInventoryTaskSparePartDetails = Query<InventoryTaskSparePartDetail>().Where(p => p.InventoryTaskId == taskId && p.InventoryStatus == InventoryStatus.Done && p.SecondResult == null)
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                if (allInventoryTaskSparePartDetails.Any())
                {
                    GetSpartPartListInfo(taskId, PdaType, result, allInventoryTaskSparePartDetails);
                }
            }
        }

        /// <summary>
        /// 获取备件清单列表(已盘点)
        /// </summary>
        /// <param name="PdaType"></param>
        /// <param name="taskId"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private void GetInventorySpartpartDoneInfo(int PdaType, double taskId, List<InventorySparePartListInfo> result)
        {
            //取所有已有初盘/复盘结果的明细
            var allInventoryTaskSparePartDetails = Query<InventoryTaskSparePartDetail>()
                .WhereIf(PdaType == 1, p => p.InventoryTaskId == taskId && p.InventoryStatus == InventoryStatus.Done && p.FirstResult != null)
                .WhereIf(PdaType == 2, p => p.InventoryTaskId == taskId && p.InventoryStatus == InventoryStatus.Done && p.SecondResult != null)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            allInventoryTaskSparePartDetails.ForEach(item =>
            {
                var res = new InventorySparePartListInfo()
                {
                    ControlMethod = (int)item.ControlMethod,
                    IsFinishInventory = true,
                    Id = item.Id,
                    InventoryTaskId = taskId,
                    InvResult = PdaType == 1 ? item.FirstResult.Value.ToLabel().L10N() : item.SecondResult.Value.ToLabel().L10N(),
                    LotNo = item.LotNo,
                    PdaType = PdaType,
                    Sn = item.Sn,
                    SparePartCode = item.SparePartCode,
                    SparePartId = item.SparePartId,
                    SparePartName = item.SparePartName
                };
                result.Add(res);
            });
        }

        /// <summary>
        /// 获取备件列表
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="PdaType">1初盘 2复盘</param>
        /// <param name="result"></param>
        /// <param name="allInventoryTaskSparePartDetails"></param>
        private void GetSpartPartListInfo(double taskId, int PdaType, List<InventorySparePartListInfo> result, EntityList<InventoryTaskSparePartDetail> allInventoryTaskSparePartDetails)
        {
            var sparePartIds = allInventoryTaskSparePartDetails.Select(p => p.SparePartId).ToList();
            var allInventoryTaskSpareParts = sparePartIds.SplitContains(ids =>
            {
                return Query<InventoryTaskSparePart>().Where(p => p.InventoryTaskId == taskId && ids.Contains(p.SparePartId)).ToList(null, new EagerLoadOptions()
                   .LoadWithViewProperty());
            });
            allInventoryTaskSpareParts.ForEach(
                 item =>
                 {
                     var res = new InventorySparePartListInfo()
                     {
                         PdaType = PdaType,
                         ControlMethod = (int)item.ControlMethod,
                         Id = item.Id,
                         InventoryTaskId = taskId,
                         SparePartId = item.SparePartId,
                         SparePartCode = item.SparePartCode,
                         SparePartName = item.SparePartName,
                         Total = item.Total,
                         NotInvQty = allInventoryTaskSparePartDetails.Count(p => p.SparePartId == item.SparePartId)
                     };
                     result.Add(res);
                 }
                );
        }


        /// <summary>
        /// 设置批次号或编码管理的盘点信息
        /// </summary>
        /// <param name="now"></param>
        /// <param name="saveInfo"></param>
        /// <param name="detail"></param>
        private void SetLotCodeInfo(DateTime now, InventorySparePartExcuteInfo saveInfo, InventoryTaskSparePartDetail detail)
        {

            if (saveInfo.PdaType == 1)
            {
                detail.FirstDateTime = now;
                detail.FirstCounterId = RT.IdentityId;


                detail.FirstNg = int.Parse(saveInfo.InputNgQty);
                detail.FirstGood = int.Parse(saveInfo.InputPassQty);
                detail.FirstTotal = detail.FirstNg + detail.FirstGood;
                detail.FirstDiff = detail.FirstTotal - detail.Total;
                SetResult(detail, saveInfo.CountStr, saveInfo.PdaType);
            }
            if (saveInfo.PdaType == 2)
            {
                detail.SecondDateTime = now;
                detail.SecondNgQty = int.Parse(saveInfo.InputNgQty);
                detail.SecondGoodQty = int.Parse(saveInfo.InputPassQty);
                detail.SecondCounterId = RT.IdentityId;
                detail.SecondTotal = detail.SecondNgQty + detail.SecondGoodQty;
                detail.SecondDiff = detail.SecondTotal - detail.Total;
                SetResult(detail, saveInfo.CountStr, saveInfo.PdaType);
            }
        }

        /// <summary>
        /// 设置盘点结果数量信息
        /// </summary>
        /// <param name="now"></param>
        /// <param name="saveInfo"></param>
        /// <param name="detail"></param>
        private void SetQtyInfo(DateTime now, InventorySparePartExcuteInfo saveInfo, InventoryTaskSparePartDetail detail)
        {
            if (saveInfo.PdaType == 1)
            {
                detail.FirstDateTime = now;
                detail.FirstNg = saveInfo.QualityList.First(m => m.Type == "不良品").isBoole ? 1 : 0;
                detail.FirstGood = saveInfo.QualityList.First(m => m.Type == "良品").isBoole ? 1 : 0;
                detail.FirstCounterId = RT.IdentityId;
                detail.FirstTotal = detail.FirstNg + detail.FirstGood;
                detail.FirstDiff = detail.FirstTotal - detail.Total;
                
            }
            if (saveInfo.PdaType == 2)
            {
                detail.SecondDateTime = now;

                detail.SecondNgQty = saveInfo.QualityList.First(m => m.Type == "不良品").isBoole ? 1 : 0;
                detail.SecondGoodQty = saveInfo.QualityList.First(m => m.Type == "良品").isBoole ? 1 : 0;
                detail.SecondCounterId = RT.IdentityId;
                detail.SecondTotal = detail.SecondNgQty + detail.SecondGoodQty;
                detail.SecondDiff = detail.SecondTotal - detail.Total;
            }
        }

        /// <summary>
        /// 设置盘点结果
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="resultType"></param>
        /// <param name="PdaType"></param>
        private void SetResult(InventoryTaskSparePartDetail detail, string resultType, int PdaType)
        {
            switch (resultType)
            {
                case "正常":
                    if (PdaType == 1)//初盘
                        detail.FirstResult = InventoryResult.Normal;

                    else
                        detail.SecondResult = InventoryResult.Normal;
                    break;
                case "信息变动":
                    if (PdaType == 1)//初盘
                        detail.FirstResult = InventoryResult.InfoChange;
                    else
                        detail.SecondResult = InventoryResult.InfoChange;
                    break;
                case "盘亏":
                    if (PdaType == 1)//初盘
                        detail.FirstResult = InventoryResult.Loss;
                    else
                        detail.SecondResult = InventoryResult.Loss;
                    break;
                case "盘盈":
                    if (PdaType == 1)//初盘
                        detail.FirstResult = InventoryResult.Profit;
                    else
                        detail.SecondResult = InventoryResult.Profit;
                    break;
            }
        }

        /// <summary>
        /// 设置备件盘点明细数据
        /// </summary>
        /// <param name="pdaType"></param>
        /// <param name="task"></param>
        /// <param name="result"></param>
        /// <param name="firstInv"></param>
        private void SetDetailInfo(int pdaType, InventoryTask task, List<InventorySparePartExcuteInfo> result, InventoryTaskSparePartDetail firstInv)
        {
            var info = new InventorySparePartExcuteInfo(true);
            info.Id = firstInv.Id;
            info.PdaType = pdaType;
            info.InventoryTaskId = task.Id;
            info.Code = firstInv.SparePartCode;
            info.Name = firstInv.SparePartName;
            info.Location = firstInv.StorageLocationName;
            info.IsBlind = firstInv.InventoryExecuteType == InventoryExecuteType.Blind;
            info.LotNo = firstInv.LotNo;
            info.Sn = firstInv.Sn;
            info.PassQty = firstInv.GoodQty;
            info.NgQty = firstInv.NgQty;
            info.ControlMode = (int)firstInv.ControlMethod;

            if (firstInv.ControlMethod == SpareParts.Enums.ControlMethod.Sn)
            {
                info.CountStatus = 2;
                info.QualityState = (int)(firstInv.GoodQty == 1 ? PdaQualityState.Pass : PdaQualityState.NG);
            }
            else
                info.CountStatus = 1;

            info.InventoryType = ((int)task.InventoryExecuteType).ToString();
            result.Add(info);
        }
    }
}
