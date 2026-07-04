using SIE.Common.Sort;
using SIE.Domain;
using SIE.Items;
using SIE.MES.Edge.Models;
using SIE.MES.InspectionStandards;
using SIE.MES.WIP;
using SIE.Tech.Processs;
using SIE.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIE.MES.Edge
{
    /// <summary>
    /// 边缘在制数据Service类
    /// Edge Service for WIP
    /// </summary>
    public class EdgeWipService : IEdgeWipService
    {
        /// <summary>
        /// ID 格式字符串
        /// </summary>
        private const string ID_FORMAT = "#.###";

        private const int GET_SHIFT_TRY_TIMES = 24;

        private const int SHIFT_TIME_SEGMENT = 30;
        private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// 在制dao类
        /// </summary>
        private readonly IEdgeWipDao edgeWipDao;

        private readonly WipController wipCtl;

        /// <summary>
        /// 构造函数
        /// Construct function
        /// </summary>
        /// <param name="edgeWipDao"></param>
        /// <param name="wipCtl"></param>
        public EdgeWipService(IEdgeWipDao edgeWipDao,
            WipController wipCtl)
        {
            this.edgeWipDao = edgeWipDao;
            this.wipCtl = wipCtl;
        }

        /// <summary>
        /// 获取在制工单信息
        /// Gets WIP work order information
        /// </summary>
        /// <param name="WorkOrderNo"></param>
        /// <returns></returns>
        public WipWorkOrder GetWipWorkOrder(string WorkOrderNo)
        {
            var wo = edgeWipDao.GetWorkOrder(WorkOrderNo);
            if (wo == null)
            {
                return null;
            }

            var wipWo = CreateWipWorkOrderByWorkOrder(wo);
            return SetWipWorkOrder(wipWo, wo);
        }


        /// <summary>
        /// 获取在制工单信息
        /// </summary>
        /// <param name="barcode">生产条码</param>
        /// <returns></returns>
        public WipWorkOrder GetWipWorkOrderByBarcode(string barcode)
        {
            var wo = edgeWipDao.GetWorkOrderByBarcode(barcode);
            if (wo == null)
            {
                return null;
            }
            var wipWo = CreateWipWorkOrderByWorkOrder(wo);
            return SetWipWorkOrder(wipWo, wo);
        }

        /// <summary>
        /// 获取在制工单
        /// </summary>
        /// <param name="wo">工单信息</param>
        /// <returns></returns>
        private WipWorkOrder CreateWipWorkOrderByWorkOrder(WorkOrders.WorkOrder wo)
        {
            WipWorkOrder wipWorkOrder = new WipWorkOrder();
            var product = RF.GetById<Item>(wo.ProductId);
            wipWorkOrder.WorkOrderId = wo.Id.ToString(ID_FORMAT);
            wipWorkOrder.WorkOrderNo = wo.No;
            wipWorkOrder.ProductId = wo.ProductId.ToString(ID_FORMAT);
            wipWorkOrder.ProductCode = product.Code;
            wipWorkOrder.ProductName = product.Name;
            wipWorkOrder.ProductModel = product.Model?.Name;
            wipWorkOrder.ProductModelId = ((product.Model?.Id) ?? 0).ToString();
            wipWorkOrder.LineId = wo.ResourceId?.ToString(ID_FORMAT);
            wipWorkOrder.WorkShopId = wo.WorkShopId?.ToString(ID_FORMAT);
            wipWorkOrder.OnlineQty = wo.OnlineQty;
            wipWorkOrder.Qty = wo.PlanQty;
            wipWorkOrder.FinishedQty = wo.FinishQty;
            wipWorkOrder.Status = (int)wo.State;
            wipWorkOrder.IsPause = (int)wo.IsPause;
            wipWorkOrder.StartDate = wo.PlanBeginDate.ToString("yyyy-MM-dd HH:mm:ss");
            wipWorkOrder.EndDate = wo.PlanEndDate.ToString("yyyy-MM-dd HH:mm:ss");
            return wipWorkOrder;
        }

        private WipWorkOrder SetWipWorkOrder(WipWorkOrder wipWorkOrder, WorkOrders.WorkOrder wo)
        {
            var processList = wipCtl.GetRoutingProcess(wo.Id, wo.No);
            
            // 取条码信息
            var barcodeList = edgeWipDao.GetBarcodes(wo.Id);
            barcodeList.ForEach(item =>
            {
                EdgeBarcode barcode = new EdgeBarcode()
                {
                    Barcode = item.Sn,
                    IsScraped = item.IsScraped,
                    Qty = item.Qty,
                    IsPending = item.IsPending,
                    BoxesQty = item.BoxesQty
                };
                wipWorkOrder.BarcodeList.Add(barcode);
            });

            var bomItemIds = processList.SelectMany(p => p.Boms).Select(p => p.ItemId).ToList();
            bomItemIds.AddRange(processList.SelectMany(p => p.Boms).SelectMany(p => p.AltBom).Select(p => p.ItemId).ToList());
            var items = edgeWipDao.GetItems(bomItemIds);

            // 生成工艺路线-工序-工位数据
            foreach (var proc in processList)
            {
                EdgeRouteProcess rp = new EdgeRouteProcess();
                rp.Id = proc.Id.ToString(ID_FORMAT);
                rp.Name = proc.Name;
                rp.Optional = proc.Optional;
                rp.Sign = (int)proc.Sign;
                rp.ProcessId = (proc.ProcessId ?? 0).ToString(ID_FORMAT);
                rp.PassNum = proc.PassNum;
                rp.MaxPassNum = proc.MaxPassNum;
                rp.Repeat = proc.Repeat;
                rp.CreateSku = proc.CreateSku;

                foreach (var bom in proc.Boms)
                {
                    // 创建Bom数据
                    EdgeBom eb = ConvertBomToEdgeBom(bom, items);
                    if (eb == null)
                    {
                        continue;
                    }
                    // 添加替代料
                    bom.AltBom.ForEach(p =>
                    {
                        var altBom = ConvertBomToEdgeBom(bom, items);
                        if (altBom != null)
                        {
                            eb.AltBom.Add(altBom);
                        }
                    });
                    rp.Boms.Add(eb);
                }

                // 构建下工序数据
                foreach (var item in proc.Next)
                {
                    List<string> strIds = new List<string>();
                    foreach (var dId in item.Value) { strIds.Add(dId.ToString(ID_FORMAT)); }
                    rp.Next.Add(item.Key, strIds);
                }

                wipWorkOrder.ProcesseList.Add(rp);
            }

            // 取包装规则信息
            foreach (var rule in wo.PackageRuleDetailList)
            {
                var edgeRule = new EdgePackRule();
                edgeRule.Id = rule.Id;
                edgeRule.WorkOrderId = rule.WorkOrderId;
                edgeRule.WorkOrderNo = rule.WorkOrder.No;
                edgeRule.PackUnitId = rule.PackageUnitId;
                edgeRule.PackUnitName = rule.PackageUnit.Name;
                edgeRule.Description = rule.Description;
                edgeRule.PackQty = rule.LevelQty;
                edgeRule.Qty = rule.Qty;
                edgeRule.NumberRuleId = rule.NumberRuleId;
                edgeRule.NumberRuleName = rule.NumberRule?.Name;
                edgeRule.IsPrint = rule.IsPrint;
                edgeRule.PrintTemplateId = rule.PrintTemplateId;
                edgeRule.PrintTemplateName = rule.PrintTemplate?.FileName;
                edgeRule.IsPackage = rule.IsPackage; //WorkOrderPackageRuleDetail
                edgeRule.Index = SortExtension.GetIndex(rule);
                edgeRule.ProcessIds = rule.WorkOrderProcessPackingUnitList.Select(p => p.ProcessId).ToList();
                wipWorkOrder.PackRuleList.Add(edgeRule);
            }

            // 取包装号信息
            var packBarcodeList = edgeWipDao.GetPackingBarcodes(wo.Id);
            packBarcodeList.ForEach(item =>
            {
                var barcode = new EdgePackingBarcode() { Code = item.Code, PackUnitName = item.PackageRuleDetail?.PackageUnit?.Name, IsUse = item.IsUse, WorkOrderId = item.WorkOrderId };
                wipWorkOrder.PackBarcodeList.Add(barcode);
            });
            return wipWorkOrder;
        }

        /// <summary>
        /// 把bom转换为边缘数据
        /// </summary>
        /// <param name="rtBom"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        private EdgeBom ConvertBomToEdgeBom(WIP.Runtime.bom rtBom, List<Item> items)
        {
            EdgeBom eb = new EdgeBom();
            eb.BomId = rtBom.BomId.ToString(ID_FORMAT);
            eb.ItemId = rtBom.ItemId.ToString(ID_FORMAT);
            eb.Qty = rtBom.Qty;
            eb.IsExternal = rtBom.IsExternal;
            eb.IsRepeat = rtBom.IsRepeat;
            eb.IsSingleLabel = rtBom.IsSingleLabel;
            var item = items.FirstOrDefault(b => b.Id == rtBom.ItemId);
            if (item == null)
            {
                return null;
            }
            eb.ItemName = item.Name;
            eb.ItemCode = item.Code;
            return eb;
        }

        /// <summary>
        /// 获取在制用户基础信息，包括工序、工位、资源权限等
        /// Gets WIP fundamental infomations by employee Id, inlude processes, work stations, produce resources.
        /// </summary>
        /// <param name="employeeId"></param>
        public WipEmployeeInfo GetWipUserInfo(double employeeId)
        {
            var emp = edgeWipDao.GetEmployeeById(employeeId);
            if (emp == null)
            {
                return null;
            }
            WipEmployeeInfo empInfo = new WipEmployeeInfo();
            empInfo.EmployeeId = emp.Id.ToString(ID_FORMAT);
            empInfo.EmployeeName = emp.Name;

            var lines = edgeWipDao.GetWipResources(employeeId);
            var lineIds = lines.Select(t => t.Id).ToList();
            var procs = edgeWipDao.GetProcesssByEmployeeId(employeeId);
            var processIds = procs.Select(t => t.Id).ToList();
            var stations = edgeWipDao.GetStationsByResourceIds(lineIds);

            var resTask = Task.Run(
                new Action(() =>
                {
                    foreach (var line in lines)
                    {
                        empInfo.Resources.Add(new EdgeResource()
                        {
                            Id = line.Id.ToString(ID_FORMAT),
                            Name = line.Name,
                            Code = line.Code,
                            EdgeShiftInfo = GetShifts(line.Id, 3)
                        });
                    }
                }).WithCurrentThreadContext());

            var procDefIds = procs.Where(p => p.Type == ProcessType.BatchPqc || p.Type == ProcessType.Pqc /*|| p.Type == ProcessType.Fqc*/).Select(p => p.Id).ToList();
            var defectList = edgeWipDao.GetProcessDefects(procDefIds);
            var defectGroups = defectList.GroupBy(p => p.ProcessId);
            var stepsList = edgeWipDao.GetProcessCollectSteps(processIds);

            // 工序信息处理
            var procTask = Task.Run(
                new Action(() =>
                {
                    CreateEmployeeProcessInfo(empInfo, procs, defectGroups, stepsList);

                }).WithCurrentThreadContext());

            // 边缘工位信息
            foreach (var station in stations)
            {
                var edgeStation = new EdgeStation();
                edgeStation.Id = station.Id.ToString(ID_FORMAT);
                edgeStation.Name = station.Name;
                edgeStation.Code = station.Code;
                edgeStation.ResourceId = station.ResourceId.ToString(ID_FORMAT);
                edgeStation.ProcessIds = station.StationProcessList.Select(p=>p.ProcessId.ToString(ID_FORMAT)).ToList();
                empInfo.Stations.Add(edgeStation);
            }
            resTask.Wait();
            procTask.Wait();
            return empInfo;
        }

        /// <summary>
        /// 创建员工对应工序信息
        /// </summary>
        /// <param name="empInfo"></param>
        /// <param name="procs"></param>
        /// <param name="defectGroups"></param>
        /// <param name="stepsList"></param>
        private void CreateEmployeeProcessInfo(WipEmployeeInfo empInfo, List<Process> procs, IEnumerable<IGrouping<double, ProcessDefect>> defectGroups, List<ProcessCollectStep> stepsList)
        {
            foreach (var process in procs)
            {
                // 工序缺陷信息
                var edgeDefects = new List<EdgeDefect>();
                if (process.Type == ProcessType.BatchPqc || process.Type == ProcessType.Pqc /*|| process.Type == ProcessType.Fqc*/)
                {
                    var defectGroup = defectGroups.FirstOrDefault(p => p.Key == process.Id);
                    if (defectGroup != null && defectGroup.Any())
                    {
                        edgeDefects = defectGroup.Select(p => new EdgeDefect
                        {
                            Id = p.Defect.Id,
                            Code = p.Defect.Code,
                            Desc = p.Defect.Description,
                            CategoryId = p.Defect.DefectCategoryId
                        }).ToList();
                    }
                }

                var edgeDefectCategories = edgeWipDao.GetProcessDefectCategorys(edgeDefects);
                var EdgeDefectInfo = new EdgeDefectInfo();
                EdgeDefectInfo.Defects.AddRange(edgeDefects);
                EdgeDefectInfo.DefectCategories.AddRange(edgeDefectCategories);
                // 边缘侧工序
                var edgeProcess = new EdgeProcess()
                {
                    Id = process.Id.ToString(ID_FORMAT),
                    Name = process.Name,
                    Code = process.Code,
                    ProcessType = (int?)process.Type,
                    DefectInfo = EdgeDefectInfo
                };
                // 采集步骤
                var steps = stepsList.Where(s => s.ProcessId == process.Id).OrderBy(s => s.Step).ToList();
                steps.ForEach(s =>
                {
                    edgeProcess.EdgeCollectSteps.Add(new EdgeCollectStep()
                    {
                        ProcessId = process.Id.ToString(ID_FORMAT),
                        Step = s.Step,
                        NeedUnbound = s.IsUnbound,
                        BarcodeType = (int)s.BarcodeType,
                        BarcodeTypeDesc = s.BarcodeType.ToLabel(),
                        PlugType = (int?)s.PlugType,
                        IsGenerateBatch = s.IsGenerateBatch
                    });
                });

                empInfo.Processes.Add(edgeProcess);
            }
        }


        /// <summary>
        /// 取机型检验项目
        /// </summary>
        /// <returns></returns>
        public EdgeInspectionItemInfo GetInspectionItemInfo()
        {
            EdgeInspectionItemInfo info = new EdgeInspectionItemInfo();
            info.LastUpdatedTime = DateTime.Now;
            var inspectionItemList = edgeWipDao.GetInspectionItems();
            foreach (var inspectionItem in inspectionItemList)
            {
                var edgeInspectionItem = new EdgeInspectionItem();
                edgeInspectionItem.Id = inspectionItem.Id;
                edgeInspectionItem.ProcessId = inspectionItem.ProcessId;
                edgeInspectionItem.ProductModelId = RT.Service.Resolve<ModelInspectionItemController>().GetProductModel(inspectionItem.Id);
                edgeInspectionItem.Name = inspectionItem.Name;
                edgeInspectionItem.CheckTag = inspectionItem.CheckTag;
                edgeInspectionItem.LimitLowCompare = inspectionItem.LimitLowCompare;
                edgeInspectionItem.LimitMaxCompare = inspectionItem.LimitMaxCompare;
                edgeInspectionItem.LimitLow = inspectionItem.LimitLow;
                edgeInspectionItem.LimitMax = inspectionItem.LimitMax;
                edgeInspectionItem.UnitName = inspectionItem.Unit?.Name;
                info.InspectionItems.Add(edgeInspectionItem);
            }
            return info;
        }

        /// <summary>
        /// 取缺陷代码、缺陷分类等信息
        /// </summary>
        /// <returns></returns>
        public EdgeDefectInfo GetDefectInfo()
        {
            EdgeDefectInfo info = new EdgeDefectInfo();
            info.LastUpdatedTime = DateTime.Now;
            var defectList = edgeWipDao.GetAllDefects();
            var categoryList = edgeWipDao.GetAllDefectCategory();
            var repairMeasureList = edgeWipDao.GetAllRepairMeasure();
            var defectResponsibilityCategoryList = edgeWipDao.GetAllDefectResponsibilityCategory();
            var defectResponsibilityList = edgeWipDao.GetAllDefectResponsibility();

            foreach (var defect in defectList)
            {
                var edgeDefect = new EdgeDefect();
                edgeDefect.Id = defect.Id;
                edgeDefect.Code = defect.Code;
                edgeDefect.Desc = defect.Description;
                edgeDefect.CategoryId = defect.DefectCategoryId;
                info.Defects.Add(edgeDefect);
            }

            foreach (var category in categoryList)
            {
                var defectCategory = new EdgeDefectCategory();
                defectCategory.Id = category.Id;
                defectCategory.Code = category.Code;
                defectCategory.Desc = category.Description;
                defectCategory.TreePId = category.TreePId;
                info.DefectCategories.Add(defectCategory);
            }

            //维修措施
            foreach (var repairMeasure in repairMeasureList)
            {
                var edgeRepairMeasure = new EdgeRepairMeasure();
                edgeRepairMeasure.Id = repairMeasure.Id;
                edgeRepairMeasure.Code = repairMeasure.Code;
                edgeRepairMeasure.Description = repairMeasure.Description;
                info.RepairMeasures.Add(edgeRepairMeasure);
            }
            //缺陷责任分类
            foreach (var defectResponsibilityCategory in defectResponsibilityCategoryList)
            {
                var edgeDefectResponsibilityCategory = new EdgeDefectResponsibilityCategory();
                edgeDefectResponsibilityCategory.Id = defectResponsibilityCategory.Id;
                edgeDefectResponsibilityCategory.Code = defectResponsibilityCategory.Code;
                edgeDefectResponsibilityCategory.Description = defectResponsibilityCategory.Description;
                edgeDefectResponsibilityCategory.TreePId = defectResponsibilityCategory.TreePId;
                info.DefectResponsibilityCategories.Add(edgeDefectResponsibilityCategory);
            }
            //缺陷责任
            foreach (var defectResponsibility in defectResponsibilityList)
            {
                var edgeDefectResponsibility = new EdgeDefectResponsibility();
                edgeDefectResponsibility.Id = defectResponsibility.Id;
                edgeDefectResponsibility.Code = defectResponsibility.Code;
                edgeDefectResponsibility.Description = defectResponsibility.Description;
                edgeDefectResponsibility.CategoryId = defectResponsibility.CategoryId;
                info.DefectResponsibilities.Add(edgeDefectResponsibility);
            }

            return info;
        }

        /// <summary>
        /// 取生产资源班次信息
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="count">班次数量</param>
        /// <returns></returns>
        public EdgeShiftInfo GetShifts(double resourceId, int count)
        {
            EdgeShiftInfo shiftInfo = new EdgeShiftInfo();
            DateTime currentTime = DateTime.Now;
            int index = 0;
            while (index < GET_SHIFT_TRY_TIMES && shiftInfo.Datas.Count < count)
            {
                index++;
                var shift = GetShift(resourceId, currentTime);
                if (shift != null)
                {
                    shiftInfo.Datas.Add(shift);
                    DateTime beginTime = Convert.ToDateTime(shift.BeginTime);
                    DateTime endTime = Convert.ToDateTime(shift.EndTime);
                    int daysInterval = currentTime.Subtract(beginTime).Days;
                    beginTime = beginTime.AddDays(daysInterval);
                    endTime = endTime.AddDays(daysInterval);
                    // 跨日处理
                    if (endTime <= beginTime)
                    {
                        endTime = endTime.AddDays(1);
                    }
                    shift.BeginTime = beginTime.ToString(DATE_FORMAT);
                    shift.EndTime = endTime.ToString(DATE_FORMAT);
                    shiftInfo.NextRefreshTime = shift.BeginTime;
                    currentTime = endTime.AddMinutes(1);
                    continue;
                }
                currentTime = currentTime.AddMinutes(SHIFT_TIME_SEGMENT - 1);
            }

            if (shiftInfo.Datas.Count == 0)
            {
                // 
                shiftInfo.NextRefreshTime = currentTime.AddMinutes(SHIFT_TIME_SEGMENT).ToString(DATE_FORMAT);
            }
            return shiftInfo;
        }

        /// <summary>
        /// 取生产资源班次信息
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="currentTime"></param>
        /// <returns></returns>
        private EdgeShift GetShift(double resourceId, DateTime currentTime)
        {
            var shift = edgeWipDao.GetShift(resourceId, currentTime);
            if (shift == null)
            {
                return null;
            }
            EdgeShift edgeShift = new EdgeShift();
            edgeShift.Id = shift.Id.ToString(ID_FORMAT);
            edgeShift.Name = shift.Name;
            edgeShift.Code = shift.Code;
            edgeShift.BeginTime = shift.BeginTime.ToString(DATE_FORMAT);
            edgeShift.EndTime = shift.EndTime.ToString(DATE_FORMAT);
            return edgeShift;
        }

        /// <summary>
        /// 更新条码剩余数量
        /// </summary>
        /// <param name="edgeMaterials"></param>
        /// <returns></returns>
        public bool SetBarcodes(List<EdgeMaterial> edgeMaterials)
        {
            return edgeWipDao.SetBarcodes(edgeMaterials);
        }

        /// <summary>
        /// 下料更新条码信息
        /// </summary>
        /// <param name="edgeMaterials">下料来源信息</param>
        /// <returns></returns>
        public bool UpdateUnLoadItemBarcodes(List<EdgeMaterial> edgeMaterials)
        {
            return edgeWipDao.UpdateUnLoadItemBarcodes(edgeMaterials);
        }


        /// <summary>
        /// 获取当前时间计划排产的有效在制工单信息
        /// </summary>
        /// <returns></returns>
        public List<WipWorkOrder> GetPlannedWipWorkOrders(List<string> resourceNos)
        {
            var orders = edgeWipDao.GetPlannedWipWorkOrders(resourceNos);

            var batchRuleList = orders.Select(p => p.ProductId).Distinct().SplitContains(tempIds =>
            {
                return DB.Query<SIE.Core.Items.ItemBatchRule>().Where(p => tempIds.Contains(p.ItemId)).ToList();
            });

            List<WipWorkOrder> wipOrders = new List<WipWorkOrder>();
            if (orders.Count == 0)
            {
                return wipOrders;
            }
            foreach (var order in orders)
            {
                //查询出投料物料对应的批次规则
                var batchRule = batchRuleList.FirstOrDefault(p => p.ItemId == order.ProductId);

                if (batchRule?.RetrospectType == Core.Items.RetrospectType.Single)
                {
                    var wo = CreateWipWorkOrderByWorkOrder(order);
                    wipOrders.Add(wo);
                }
            }
            return wipOrders;
        }


        /// <summary>
        /// 获取当前时间计划排产的有效在制工单信息
        /// </summary>
        /// <returns></returns>
        public WipWorkOrder GetWipWorkOrderByNo(string workOrderNo)
        {
            var order = edgeWipDao.GetWipWorkOrderByNo(workOrderNo);
            if (order == null)
            {
                return null;
            }
            return CreateWipWorkOrderByWorkOrder(order);
        }

        /// <summary>
        /// 获取包装规则单号
        /// </summary>
        /// <param name="ruleId">规则Id</param>
        /// <returns></returns>
        public string GetPackCode(double ruleId)
        {
            return edgeWipDao.GetPackCode(ruleId);
        }
    }
}
