using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualBasic;
using SIE.Common;
using SIE.Common.Catalogs;
using SIE.Common.ImportHelper;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.WorkOrders;
using SIE.Items;
using SIE.Items.KzItemCategorys;
using SIE.MES.ItemLine;
using SIE.MES.LineAndon;
using SIE.MES.ProcessProperty;
using SIE.MES.WorkOrders;
using SIE.Rbac.Security;
using SIE.Resources.Enterprises;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkOrderController = SIE.MES.WorkOrders.WorkOrderController;

namespace SIE.MES.TaskManagement.SchedulingInfs.Handles
{
    /// <summary>
    /// 
    /// </summary>
    [Services.Service(FallbackType = typeof(SchedulingInfImportHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class SchedulingInfImportHandle : IDisposable, IBusinessImportExt
    {

        /// <summary>
        /// 列名
        /// </summary>
        public List<string> ColumnNameList { get; set; } = new List<string>
        {
            "工厂", "工单号",  "工序编码","产线编码", "标准产能",  "物料编码","旧物料号","入库日期","开始日期","完成日期"
        };

        /// <summary>
        /// 将日期动态列那部分存起来
        /// </summary>
        public List<string> dataStrs { get; set; } = new List<string>();

        /// <summary>
        /// 工厂字典
        /// </summary>
        public List<string> factorys { get; set; } = new List<string>();

        /// <summary>
        /// 工单字典
        /// </summary>
        public Dictionary<string, double> dicWorkOrder { get; set; } = new Dictionary<string, double>();

        /// <summary>
        /// 工序字典
        /// </summary>
        public Dictionary<string, double> dicProcess { get; set; } = new Dictionary<string, double>();

        /// <summary>
        /// 产线字典
        /// </summary>
        public Dictionary<string, double> dicAndonLine { get; set; } = new Dictionary<string, double>();

        /// <summary>
        /// 物料字典
        /// </summary>
        public Dictionary<string, double> dicItem { get; set; } = new Dictionary<string, double>();

        /// <summary>
        /// 排程中间表字典
        /// </summary>
        public Dictionary<string, SchedulingInf> dicSchedulingInf { get; set; } = new Dictionary<string, SchedulingInf>();

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, EntityList<LayoutInfo>> dicLayoutInfo { get; set; } = new Dictionary<string, EntityList<LayoutInfo>>();

        public Dictionary<string, ValidColumn> ColumnValidList { get; set; }

        public IBusinessImport CreaetColumnValid()
        {
            this.ColumnValidList = new Dictionary<string, ValidColumn>();
            this.ColumnValidList.Add(ColumnNameList[0], new ValidColumn(ImportDataType._String, true, VaildFactory));
            this.ColumnValidList.Add(ColumnNameList[1], new ValidColumn(ImportDataType._String, true, VaildWorkOrder));
            this.ColumnValidList.Add(ColumnNameList[2], new ValidColumn(ImportDataType._String, true, VaildProcess));
            this.ColumnValidList.Add(ColumnNameList[3], new ValidColumn(ImportDataType._String, true, VaildAndonLine));
            this.ColumnValidList.Add(ColumnNameList[4], new ValidColumn(ImportDataType._Double, false, true));
            this.ColumnValidList.Add(ColumnNameList[5], new ValidColumn(ImportDataType._String, true, VaildItem));
            this.ColumnValidList.Add(ColumnNameList[6], new ValidColumn(ImportDataType._String, false, true));
            this.ColumnValidList.Add(ColumnNameList[7], new ValidColumn(ImportDataType._String, false, VaildInstorageDate));
            this.ColumnValidList.Add(ColumnNameList[8], new ValidColumn(ImportDataType._String, true, VaildBeginDate));
            this.ColumnValidList.Add(ColumnNameList[9], new ValidColumn(ImportDataType._String, true, VaildEndDate));

            return this;
        }

        #region 验证

        /// <summary>
        /// 验证完成日期
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="messageTip"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        private bool VaildEndDate(object obj, out string messageTip, DataRow dr)
        {
            messageTip = string.Empty;
            var endDate = obj.ToString().Trim();
            if (endDate == null)
            {
                messageTip = "完成日期不能为空!".L10N();
                return false;
            }
            else
            {
                var result = ConvertDate(endDate);
                if (result == null)
                {
                    messageTip = "完成日期格式错误,请输入正确日期，如20250725(yyyyMMdd)";
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// 验证开始日期
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="messageTip"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        private bool VaildBeginDate(object obj, out string messageTip, DataRow dr)
        {
            messageTip = string.Empty;
            var beginDate = obj.ToString().Trim();
            if (beginDate == null)
            {
                messageTip = "开始日期不能为空!".L10N();
                return false;
            }
            else
            {
                var result = ConvertDate(beginDate);
                if (result == null)
                {
                    messageTip = "开始日期格式错误,请输入正确日期，如20250725(yyyyMMdd)";
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// 验证入库日期
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="messageTip"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        private bool VaildInstorageDate(object obj, out string messageTip, DataRow dr)
        {
            messageTip = string.Empty;
            var inStorageDate = obj.ToString().Trim();
            if (inStorageDate != null)
            {
                var result = ConvertDate(inStorageDate);
                if (result == null)
                {
                    messageTip = "入库日期格式错误,请输入正确日期，如20250725(yyyyMMdd)";
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 验证旧物料
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="messageTip"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        private bool VaildOldItem(object obj, out string messageTip, DataRow dr)
        {
            messageTip = string.Empty;
            var itemCode = obj.ToString().Trim();
            if (itemCode.IsNullOrEmpty())
                return true;
            if (dicItem.ContainsKey(itemCode))
            {
                return true;
            }
            else
            {
                var item = RT.Service.Resolve<ItemController>().GetItem(itemCode);
                if (item == null)
                {
                    messageTip = "物料{0}不存在!".L10nFormat(itemCode);
                    return false;
                }
                else
                {
                    dicItem.Add(itemCode, item.Id);
                    return true;
                }
            }
        }

        /// <summary>
        /// 验证物料
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="messageTip"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        private bool VaildItem(object obj, out string messageTip, DataRow dr)
        {
            messageTip = string.Empty;
            var itemCode = obj.ToString().Trim();
            if (dicItem.ContainsKey(itemCode))
            {
                return true;
            }
            else
            {
                var item = RT.Service.Resolve<ItemController>().GetItem(itemCode);
                if (item == null)
                {
                    messageTip = "物料{0}不存在!".L10nFormat(itemCode);
                    return false;
                }
                else
                {
                    dicItem.Add(itemCode, item.Id);
                    return true;
                }
            }
        }

        /// <summary>
        /// 验证产线
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="messageTip"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        private bool VaildAndonLine(object obj, out string messageTip, DataRow dr)
        {
            messageTip = string.Empty;
            var machineCode = obj.ToString().Trim();

            if (dicAndonLine.ContainsKey(machineCode))
            {
                return true;
            }
            else
            {
                var andonLine = RT.Service.Resolve<AndonLineController>().GetAndonLineByMachineCode(machineCode);
                if (andonLine == null)
                {
                    messageTip = "产线{0}不存在!".L10nFormat(machineCode);
                    return false;
                }
                else
                {
                    dicAndonLine.Add(machineCode, andonLine.Id);
                    return true;
                }
            }
        }

        /// <summary>
        /// 验证工序
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="messageTip"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        private bool VaildProcess(object obj, out string messageTip, DataRow dr)
        {
            messageTip = string.Empty;
            var processCode = obj.ToString().Trim();
            if (dicProcess.ContainsKey(processCode))
            {
                return true;
            }
            else
            {
                var process = RT.Service.Resolve<ProcessController>().GetProcessByCode(processCode);
                if (process == null)
                {
                    messageTip = "工序{0}不存在!".L10nFormat(processCode);
                    return false;
                }
                else
                {
                    var factory = dr["工厂"].ToString();
                    var workOrderNo = dr["工单号"].ToString();
                    //导入时根据工单+工序到工单工艺路线中校验是否属于当前工厂，不属于则报错提示工序不属于当前工厂
                    if (dicLayoutInfo.ContainsKey(workOrderNo))
                    {
                        var layoutInfos = dicLayoutInfo[workOrderNo];
                        if (layoutInfos == null || layoutInfos.Count < 0 || layoutInfos.All(p => p.ProcessCode != processCode || (p.Factory != factory && p.ProcessCode == processCode)))
                        {
                            messageTip = "工序不属于当前工厂".L10N();
                            return false;
                        }
                    }
                    else
                    {
                        messageTip = "工序不属于当前工厂".L10N();
                        return false;
                    }
                    dicProcess.Add(processCode, process.Id);
                    return true;
                }
            }
        }

        /// <summary>
        /// 验证工单
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="messageTip"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        private bool VaildWorkOrder(object obj, out string messageTip, DataRow dr)
        {
            messageTip = string.Empty;
            var workOrderNo = obj.ToString().Trim();
            if (dicWorkOrder.ContainsKey(workOrderNo))
            {
                return true;
            }
            else
            {
                var workOrder = RT.Service.Resolve<WorkOrderController>().GetWorkOrder(workOrderNo);
                if (workOrder == null)
                {
                    messageTip = "工单{0}不存在!".L10nFormat(workOrderNo);
                    return false;
                }
                else
                {
                    if (workOrder.State == WorkOrderState.Close)
                    {
                        messageTip = "工单{0}状态为[关闭]，不允许导入!".L10nFormat(workOrder.No);
                        return false;
                    }
                    else
                    {
                        dicWorkOrder.Add(workOrderNo, workOrder.Id);
                        if (!dicLayoutInfo.ContainsKey(workOrder.No))
                        {
                            //根据工单Id获取工艺路线信息
                            EntityList<LayoutInfo> layoutInfos = RT.Service.Resolve<WorkOrderController>().GetLayoutInfosByWorkOrderId(new List<double>() { workOrder.Id });
                            if (layoutInfos.Count > 0)
                            {
                                dicLayoutInfo.Add(workOrder.No, layoutInfos);
                            }
                        }
                        return true;
                    }
                }
            }
        }

        /// <summary>
        /// 验证工厂
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="messageTip"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        private bool VaildFactory(object obj, out string messageTip, DataRow dr)
        {
            messageTip = string.Empty;
            var factoryCode = obj.ToString().Trim();
            if (factorys.Any(p => p == factoryCode))
            {
                return true;
            }
            else
            {
                //获取工厂
                var factory = RT.Service.Resolve<EnterpriseController>().GetFactoryByCode(factoryCode);
                if (factory == null)
                {
                    messageTip = "工厂{0}不存在!".L10nFormat(factoryCode);
                    return false;
                }
                else
                {
                    factorys.Add(factoryCode);
                    return true;
                }

            }
        }

        #endregion


        public void Dispose()
        {
            
        }

        public void PreBusinessDataHandle(DataTable dt)
        {
            for (int i = this.ColumnNameList.Count; i < dt.Columns.Count - 2; i++)
            {
                var text = (dt.Columns[i] as DataColumn).ColumnName;
                if (!text.Contains("_"))
                    dataStrs.Add(text);
            }
        }

        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            if (drs == null || !drs.Any())
                return;

            EntityList<Dispatchs.DispatchTask> dispatchTasks = new EntityList<Dispatchs.DispatchTask>();

            EntityList<SchedulingInf> SchedulingInfs = new EntityList<SchedulingInf>();
            for (int i = 0; i < drs.Length; i++)
            {
                DataRow row = drs[i];
                try
                {
                    var factory = row["工厂"].ToString();
                    var workOrderNo = row["工单号"].ToString();
                    var processCode = row["工序编码"].ToString();
                    var andonLineCode = row["产线编码"].ToString();
                    var standardCapacity = row["标准产能"].ToString();
                    var itemCode = row["物料编码"].ToString();
                    var oldItemCode = row["旧物料号"].ToString();
                    var inStorageDate = row["入库日期"].ToString();
                    var beginDate = row["开始日期"].ToString();
                    var endDate = row["完成日期"].ToString();

                    //当产品与产线的关系维护的资源与导入的资源不一样的时候，就要提示
                    var productLines = RT.Service.Resolve<ProductLineController>().GetProductLine(itemCode, processCode);
                    if (productLines.Count > 0)
                    {
                        //当全部数据的产线都都有值的时候，就要校验产品与产线的关系的产线与导入的产线，当有一条产线是空的，就直接不校验了产线是否存在在产品与产线的关系中(我觉得有点问题，但是他们硬是要这么做)
                        if (productLines.All(p => !p.LineCode.IsNullOrEmpty()) && productLines.All(p => p.LineCode != andonLineCode))
                        {
                            throw new ValidationException("产品与产线的关系未维护资源{0}".L10nFormat(andonLineCode));
                        }
                    }

                    //获取排程中间表
                    SchedulingInf schedulingInf = null;
                    if (dicSchedulingInf.ContainsKey(dicWorkOrder[workOrderNo] + "_" + dicProcess[processCode] + "_" + dicAndonLine[andonLineCode]))
                    {
                        schedulingInf = dicSchedulingInf[dicWorkOrder[workOrderNo] + "_" + dicProcess[processCode] + "_" + dicAndonLine[andonLineCode]];
                    }
                    else
                    {
                        //查找原数据是否存在
                        schedulingInf = RT.Service.Resolve<SchedulingInfController>().GetSchedulingInfByWoProcessAndonLine(dicWorkOrder[workOrderNo], dicProcess[processCode], dicAndonLine[andonLineCode]);
                        if (schedulingInf != null && schedulingInf.IsCancel == true)
                        {
                            //取消作废
                            schedulingInf.IsCancel = false;
                            schedulingInf.CancelReason = null;
                            schedulingInf.CancelTime = null;
                            schedulingInf.CancerId = null;
                            RT.Service.Resolve<SchedulingInfController>().SaveSchedulingInf(schedulingInf);
                            //throw new ValidationException("工单{0}，工序{1}，资源{2}，原排程单已作废，不能重复导入!".L10nFormat(workOrderNo, processCode, andonLineCode));
                        }
                        //不存在就创建新的
                        if (schedulingInf == null)
                        {
                            schedulingInf = new SchedulingInf();
                            schedulingInf.Factory = factory;
                            schedulingInf.WorkOrderId = dicWorkOrder[workOrderNo];
                            schedulingInf.ProcessId = dicProcess[processCode];
                            schedulingInf.AndonLineId = dicAndonLine[andonLineCode];
                            if (standardCapacity.IsNotEmpty())
                                schedulingInf.StandardCapacity = Convert.ToDecimal(standardCapacity);
                            schedulingInf.ItemId = dicItem[itemCode];
                            schedulingInf.ShortDescription = oldItemCode;
                            if (inStorageDate.IsNotEmpty())
                            {
                                schedulingInf.InStorageDate = ConvertDate(inStorageDate);
                            }
                            schedulingInf.BeginDate = ConvertDate(beginDate).Value;
                            schedulingInf.EndDate = ConvertDate(endDate).Value;
                            //schedulingInf.Key = schedulingInf.WorkOrderId + "_" + schedulingInf.ProcessId + "_" + schedulingInf.AndonLineId;
                            schedulingInf.PersistenceStatus = PersistenceStatus.New;
                            RT.Service.Resolve<SchedulingInfController>().SaveSchedulingInf(schedulingInf);
                        }
                        dicSchedulingInf.Add(dicWorkOrder[workOrderNo] + "_" + dicProcess[processCode] + "_" + dicAndonLine[andonLineCode], schedulingInf);
                    }
                    //查找派工任务，用于下面判断是否超过工单的计划数量
                    var tasks = dispatchTasks.Where(p => p.WorkOrderId == schedulingInf.WorkOrderId).AsEntityList();
                    if (tasks.Count < 1)
                    {
                        tasks = RT.Service.Resolve<Dispatchs.DispatchController>().GetDispatchTasks(schedulingInf.WorkOrderId);
                        if (tasks.Count > 0)
                            dispatchTasks.AddRange(tasks);
                    }

                    EntityList<SchedulingInfValue> values = new EntityList<SchedulingInfValue>();
                    //查出该工单下的所有报工任务单
                    EntityList<Dispatchs.DispatchTask> dispatches = RT.Service.Resolve<Dispatchs.DispatchController>().GetDispatchTasks(schedulingInf.WorkOrderId);

                    var schedulingInfValueList = RT.Service.Resolve<SchedulingInfController>().GetSchedulingInfValues(schedulingInf.Id);

                    //获取排程中间表各个日期的值，进行创建、更新
                    foreach (var dataStr in dataStrs)
                    {
                        //日期转换
                        var date = ConvertDate(dataStr);
                        if (date == null)
                            throw new ValidationException("列头日期格式不正确!".L10N());
                        //只能操作未来的，过去的都不更新、不新增
                        //if (date.Value.Date < DateTime.Now.Date)
                        //    continue;

                        var value1 = row[dataStr].ToString();
                        var value2 = row[dataStr + "_1"].ToString();

                        if (((!value1.IsNullOrEmpty() && value1.Contains('.')) || (!value2.IsNullOrEmpty() && value2.Contains('.'))) && string.Equals(schedulingInf.Item.Unit.Code, "PCS", StringComparison.OrdinalIgnoreCase))
                        {
                            throw new ValidationException("产品[{0}]单位为PCS，不允许导入小数".L10nFormat(schedulingInf.Item.Code));
                        }

                        //白班计算 ,考虑情况2种，1.在这个今天的之前的都不可以导入，2.今天晚班(20点以后)，也不能导入
                        if (!value1.IsNullOrEmpty() && (DateTime.Now.Date > date.Value.Date || (DateTime.Now.Date == date.Value.Date && DateTime.Now.Hour >= 20) ) )
                        {
                            throw new ValidationException("不能导入当前班次前的班次任务");
                        }

                        //晚班计算,考虑情况有两种，1.昨天之前的晚班不能导入，2.今天8点以后，不能导入昨天的晚班（8点前可以导入）
                        if (!value2.IsNullOrEmpty() && (DateTime.Now.Date.AddDays(-1) > date.Value.Date || (DateTime.Now.Date.AddDays(-1) == date.Value.Date && DateTime.Now.Hour >= 8)))
                        {
                            throw new ValidationException("不能导入当前班次前的班次任务");
                        }

                        //查找字典中是否已经存在的，减少数据库查询
                        var schedulingInfValue = schedulingInfValueList.Where(p => p.SchedulingInfId == schedulingInf.Id && p.Date == date).FirstOrDefault();
                        //if (schedulingInfValue == null)
                        //{
                        //字典中没有，就查询数据库
                        //var schedulingInfValues = RT.Service.Resolve<SchedulingInfController>().GetSchedulingInfValues(schedulingInf.Id);
                        //插入字典中
                        //if (schedulingInfValues.Count > 0)
                        //    schedulingInfValueList.AddRange(schedulingInfValues);
                        //如果查找数据库后，还是没有，就证明要新增的
                        //schedulingInfValue = schedulingInfValueList.Where(p => p.SchedulingInfId == schedulingInf.Id && p.Date == date).FirstOrDefault();
                        if (schedulingInfValue == null)
                        {
                            schedulingInfValue = new SchedulingInfValue();
                            schedulingInfValue.SchedulingInfId = schedulingInf.Id;
                            schedulingInfValue.Date = date.Value;
                            schedulingInfValue.PersistenceStatus = PersistenceStatus.New;
                            schedulingInfValueList.Add(schedulingInfValue);
                        }
                        //}

                        //如果是今天的，且是在晚班的时候，就不参与，直接到Value2，Value2存储晚班数据,Value1是白班数据
                        if (date.Value.Date == DateTime.Now.Date && (DateTime.Now.Hour >= 20 || DateTime.Now.Hour < 8))
                        {

                        }
                        else
                        {
                            //只允许更新任务单状态为待派工的、派工中，或者未生成过任务单的
                            if (schedulingInfValue.TaskStatus1 == null || schedulingInfValue.TaskStatus1 == Dispatchs.DispatchTaskStatus.ToDispatch || schedulingInfValue.TaskStatus1 == Dispatchs.DispatchTaskStatus.Dispatching)
                            {
                                if (value1.IsNullOrEmpty() || Convert.ToDecimal(value1) <= 0)
                                {
                                    if (schedulingInfValue.DispatchTask1Id != null)
                                        throw new ValidationException("已创建派工任务单，数量不能更新为空或者0");
                                    schedulingInfValue.Value1 = null;   //如果为空的，那么就设置为空，即使是过去有值的，也要更新为空
                                }
                                else
                                {
                                    schedulingInfValue.Value1 = Convert.ToDecimal(value1);
                                    schedulingInfValue.ImportTime1 = DateTime.Now;
                                }
                                //如果已经创建了派工单，那么就要把派工单数量改了
                                if (schedulingInfValue.DispatchTask1Id != null)
                                {
                                    var dispatche = dispatches.FirstOrDefault(p => p.Id == schedulingInfValue.DispatchTask1Id.Value);
                                    dispatche.DispatchQty = schedulingInfValue.Value1.Value;
                                    dispatche.PersistenceStatus = PersistenceStatus.Modified;
                                    dispatche.ImportTime = schedulingInfValue.ImportTime1;
                                }
                            }
                        }

                        //只允许更新任务单状态为待派工的、派工中，或者未生成过任务单的
                        if (schedulingInfValue.TaskStatus2 == null || schedulingInfValue.TaskStatus2 == Dispatchs.DispatchTaskStatus.ToDispatch || schedulingInfValue.TaskStatus2 == Dispatchs.DispatchTaskStatus.Dispatching)
                        {
                            if (value2.IsNullOrEmpty() || Convert.ToDecimal(value2) <= 0)
                            {
                                if (schedulingInfValue.DispatchTask2Id != null)
                                    throw new ValidationException("已创建派工任务单，数量不能更新为空或者0");
                                schedulingInfValue.Value2 = null;   //如果为空的，那么就设置为空，即使是过去有值的，也要更新为空
                            }
                            else
                            {
                                schedulingInfValue.Value2 = Convert.ToDecimal(value2);
                                schedulingInfValue.ImportTime2 = DateTime.Now;
                            }
                            //如果已经创建了派工单，那么就要把派工单数量改了
                            if (schedulingInfValue.DispatchTask2Id != null)
                            {
                                var dispatche = dispatches.FirstOrDefault(p => p.Id == schedulingInfValue.DispatchTask2Id.Value);
                                dispatche.DispatchQty = schedulingInfValue.Value2.Value;
                                dispatche.PersistenceStatus = PersistenceStatus.Modified;
                                dispatche.ImportTime = schedulingInfValue.ImportTime2;
                            }
                        }
                        if (schedulingInfValue.PersistenceStatus != PersistenceStatus.Unchanged)
                            values.Add(schedulingInfValue);
                    }

                    //同工单、同工序，排除当前的排程任务
                    var vs = RT.Service.Resolve<SchedulingInfController>().GetSchedulingInfValues(schedulingInf.Id, schedulingInf.WorkOrderId, schedulingInf.ProcessId);
                    //该排程任务的值的总和
                    var totalValue = schedulingInfValueList.Where(p => p.SchedulingInfId == schedulingInf.Id).Sum(p => (p.Value1 ?? 0) + (p.Value2 ?? 0));

                    //找出工序属性数据
                    var processPtys = RT.Service.Resolve<ProcessPtyController>().GetProcessPtysByProcessIds(new List<double>() { schedulingInf.ProcessId }, schedulingInf.WorkOrder.ProductId);//.GetProcessPtysByProcessId(schedulingInf.ProcessId);
                    var kzItemCategory = RT.Service.Resolve<KzItemCategorysController>().GetKzItemCategorieByItemId(schedulingInf.WorkOrder.ProductId);
                    var pps = new List<ProcessPty>();
                    if (kzItemCategory != null)
                    {
                        pps = processPtys.Where(p => p.KzCategoryId == kzItemCategory.KzCategoryId).ToList();
                    }
                    ////当找得到分类得时候，优先找到分类的，然后再找工序的
                    if (pps.Count == 0)
                        pps = processPtys.Where(p => p.KzCategoryId == null).ToList();

                    var processPty = pps.FirstOrDefault();

                    //当工序属性中维护了按工序BOM生成任务数量时，就要校验工单计划数 * 工序BOM单机定额总和 必须小于等于 导入数量
                    if (processPty != null && processPty.IsPbcd == true)
                    {
                        var processBoms = schedulingInf.WorkOrder.ProcessBomList.Where(p => p.ProcessId == schedulingInf.ProcessId).ToList();
                        if (processBoms == null || processBoms.Count < 1)
                            throw new ValidationException("未找到工序[{0}]BOM,不能导入".L10nFormat(processPty.ProcessCode));
                        if (schedulingInf.WorkOrder.PlanQty * processBoms.Sum(p => p.SingleQty) < totalValue + vs.Sum(p => (p.Value1 ?? 0) + (p.Value2 ?? 0)))
                            throw new ValidationException("导入的数量不能大于工单的计划数量*工序BOM单机定额的总和".L10N());
                    }
                    else
                    {
                        //当前数量+其他同工单、同工序，排除当前的排程任务数量 <= 工单计划数量
                        if (schedulingInf.WorkOrder.PlanQty < totalValue + vs.Sum(p => (p.Value1 ?? 0) + (p.Value2 ?? 0)))
                        {
                            throw new ValidationException("导入的数量不能大于工单的计划数量".L10N());
                        }
                    }

                    //保存
                    RT.Service.Resolve<SchedulingInfController>().SaveSchedulingInfValue(values);
                    //保存任务单
                    RT.Service.Resolve<SchedulingInfController>().SaveDispatchTasks(dispatches);

                }
                catch (Exception ex)
                {
                    var baseExc = ex.GetBaseException();
                    if (baseExc is ValidationException)
                        row[ImportDataHandle.MessageColumnName] += (baseExc as ValidationException).Message;
                    else
                        row[ImportDataHandle.MessageColumnName] += ex.Message;
                }
            }
        }
        

        /// <summary>
        /// 日期转换
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public DateTime? ConvertDate(string date)
        {
            if (DateTime.TryParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

    }
}
