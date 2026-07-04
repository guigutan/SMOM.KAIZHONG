using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using DotLiquid.Util;
using Irony.Parsing;
using Microsoft.Scripting.Utils;
using Newtonsoft.Json;
using SIE.Andon.Andons;
using SIE.Api;
using SIE.Common;
using SIE.Core.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.SmomControl;
using SIE.MES.Capacitys;
using SIE.MES.DashBoard.DashBoards.ProductionLine;
using SIE.MES.DashBoard.DashBoards.WorkShop;
using SIE.MES.DashBoard.KzBoard.Datas;
using SIE.MES.DashBoard.KzBoard.RegionBoards;
using SIE.MES.ProductAgingProcesss;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.HeatTreatments;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.WorkOrders;
using SIE.Security;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using WorkShopController = SIE.MES.DashBoard.DashBoards.ProductionLine.WorkShopController;

namespace SIE.MES.DashBoard.KzBoard
{
    /// <summary>
    /// 看板控制器
    /// </summary>
    public class KzBoardController : DomainController
    {

        #region 获取工厂

        /// <summary>
        /// 获取工厂
        /// </summary>
        /// <returns></returns>
        [ApiService("获取工厂")]
        public virtual List<FactoryData> GetFactoryDatas()
        {
            List<FactoryData> datas = new List<FactoryData>();
            var settings = RF.GetAll<SmomControlSetting>();
            foreach (var setting in settings)
            {
                FactoryData data = new FactoryData();
                data.label = setting.FactoryName;
                data.value = setting.FactoryCode;
                datas.Add(data);
            }
            return datas;
        }

        #endregion

        #region 获取区域

        /// <summary>
        /// 获取集团区域
        /// </summary>
        /// <param name="factoryData"></param>
        /// <returns></returns>
        [ApiService("获取区域-集团")]
        public virtual List<RegionData> GetRegionDatasGroup(FactoryData factoryData)
        {
            var setting = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingByFactoryCode(factoryData.value);
            if (setting == null || setting.FactoryUrl.IsNullOrEmpty())
            {
                throw new ValidationException("未配置Url地址;");
            }
            var smomParam = new List<SmomParam>()
                    {
                    new SmomParam { Value =factoryData }
                                 }.ToArray();

            var result = SmomPost<List<RegionData>>(setting.FactoryUrl, "GetRegionDatasFactory", smomParam);
            var datas = result;
            return datas;
        }

        /// <summary>
        /// 获取工厂区域
        /// </summary>
        /// <param name="factoryData"></param>
        /// <returns></returns>
        [ApiService("获取区域-工厂")]
        [AllowAnonymous]
        public virtual List<RegionData> GetRegionDatasFactory(FactoryData factoryData)
        {
            //登录
            RT.Service.Resolve<KzLoginController>().Login(factoryData.value);

            var regions = RT.Service.Resolve<RegionBoardsController>().GetRegionBoard(factoryData.label,factoryData.regionBoardType);
            List<RegionData> datas = new List<RegionData>();
            foreach (var region in regions)
            {
                RegionData data = new RegionData();
                data.label = region.Region;
                data.value = region.Id;
                datas.Add(data);
            }
            if (datas.Count == 0)
                datas.Add(new RegionData());
            return datas;
        }

        #endregion

        #region 区域生产产量看板

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Factory"></param>
        /// <param name="Region"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [ApiService("区域生产产量看板-集团")]
        public virtual RegionOutputData GetRegionOutputDataGroup(string Factory,string Region, RegionBoardType regionBoardType)
        {
            var setting = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingByFactoryCode(Factory);
            if (setting == null || setting.FactoryUrl.IsNullOrEmpty())
            {
                throw new ValidationException("未配置Url地址;");
            }
            var smomParam = new List<SmomParam>()
                    {
                    new SmomParam { Value =Factory },
                    new SmomParam{ Value = Region},
                    new SmomParam{ Value = regionBoardType}
                                 }.ToArray();

            var result = SmomPost<RegionOutputData>(setting.FactoryUrl, "GetRegionOutputDataFactory", smomParam);
            var data = result;
            return data;

        }

        /// <summary>
        /// 区域生产产量看板-工厂
        /// </summary>
        /// <param name="Factory"></param>
        /// <param name="Region"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [ApiService("区域生产产量看板-工厂")]
        [AllowAnonymous]
        public virtual RegionOutputData GetRegionOutputDataFactory(string Factory, string Region, RegionBoardType regionBoardType)
        {
            //登录
            RT.Service.Resolve<KzLoginController>().Login(Factory);

            RegionOutputData data = new RegionOutputData();

            var curTime = RF.Find<RegionBoard>().GetDbTime();

            var tasks = Query<DispatchTask>().Join<RegionBoardDetail>((x, y) => x.ResourceId == y.WipResourceId)
                .Join<RegionBoardDetail, RegionBoard>((x, y) => x.RegionBoardId == y.Id && y.Region == Region && y.RegionBoardType == regionBoardType)
                .Where(p => p.PlanBeginTime < curTime && p.PlanEndTime > curTime).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            //0代表白班，1代表晚班，2代表跨天晚班
            var Shift = 0;

            //当班计划数
            data.PlanQty = tasks.Sum(p => p.DispatchQty);
            //累计计划数
            //当前班次已经工作的小时数
            decimal workingTime = 0;
            if (curTime.Hour >= 8 && curTime.Hour < 20)
            {
                workingTime = (decimal)(curTime - DateTime.Now.Date.AddHours(8)).TotalHours;
                Shift = 0;
            }
            else
            {
                if (curTime.Hour < 8 && curTime.Hour >= 0)
                {
                    workingTime = (decimal)(4 + (curTime - DateTime.Now.Date).TotalHours);
                    Shift = 2;
                }
                else
                {
                    workingTime = (decimal)(curTime - DateTime.Now.Date.AddHours(20)).TotalHours;
                    Shift = 1;
                }
            }
            data.CumulativePlan = data.PlanQty / 12 * workingTime;

            //累计实际
            DateTime beginTime = DateTime.Now;
            DateTime endTime = DateTime.Now;
            if (Shift == 0)
            {
                beginTime = DateTime.Now.Date.AddHours(8);
                endTime = DateTime.Now;
            }
            else
            {
                beginTime = DateTime.Now.Date.AddHours(20);
                endTime = DateTime.Now;
            }
            //报工记录
            var reprotRecords = Query<ReportRecordExamine>().Join<RegionBoardDetail>((x, y) => x.ResourceId == y.WipResourceId)
                .Join<RegionBoardDetail, RegionBoard>((x, y) => x.RegionBoardId == y.Id && y.Region == Region && y.RegionBoardType == regionBoardType)
                .Where(p => p.ReportTime >= beginTime && p.ReportTime < endTime).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            data.CumulativeActive = reprotRecords.Sum(p => p.ReportQty);

            //差异
            data.DifferenceQty = data.CumulativeActive - data.CumulativePlan;

            WorkSafety workSafety = RT.Service.Resolve<WorkShopController>().GetWorkSafetyByFactory(Factory);
            data.SafetyProductionDays = 0;
            if (workSafety != null && workSafety.SafetyDate != null)
            {
                data.SafetyProductionDays = (decimal)(curTime - workSafety.SafetyDate.Value).TotalDays;
            }

            //小时产量
            Dictionary<DateTime, DateTime> timestamp = new Dictionary<DateTime, DateTime>();
            beginTime = new DateTime(curTime.Year, curTime.Month, curTime.Day, curTime.Hour, 0, 0).AddHours(-1);//DateTime.Now.Date.AddHours(-1);
            endTime = beginTime.AddHours(1);
            if (Shift == 0)
            {
                for (; beginTime <= curTime; beginTime = beginTime.AddHours(1))
                {
                    endTime = beginTime.AddHours(1);
                    timestamp.Add(beginTime, endTime);
                }
            }
            else if (Shift == 1)
            {
                for (; beginTime <= curTime; beginTime = beginTime.AddHours(1))
                {
                    endTime = beginTime.AddHours(1);
                    timestamp.Add(beginTime, endTime);
                }
            }
            foreach (var tt in timestamp)
            {
                //var ts = tasks.Where(p => p.PlanBeginTime >= tt.Key && p.PlanEndTime < tt.Value).ToList();
                var rs = reprotRecords.Where(p => p.ReportTime >= tt.Key && p.ReportTime < tt.Value).ToList();
                HourOutputData data1 = new HourOutputData();
                data1.Time = $"{tt.Key.Hour}:00 - {tt.Value.Hour}:00";
                data1.PlanQty = Math.Round(data.PlanQty / 12, MidpointRounding.AwayFromZero);/* ts.Sum(p => p.DispatchQty);*/
                data1.ActiveQty = rs.Sum(p => p.ReportQty);
                data1.DifferenceQty = data1.ActiveQty - data1.PlanQty;
                data.gridData1.Add(data1);
            }

            //产量看板
            var dic = tasks.GroupBy(p => new { p.ResourceCode, p.ResourceName, p.ProductCode, p.ProductName, p.ShortDescription }).ToDictionary(p => p.Key, p => p.ToList());
            foreach (var d in dic)
            {
                var rs = reprotRecords.Where(p => p.ResourceCode == d.Key.ResourceCode && p.Product.Code == d.Key.ProductCode).ToList();

                var activeQty = rs.Sum(p => p.ReportQty);
                var planQty = d.Value.Sum(p => p.DispatchQty);
                //前端只显示实际达成率<应达成率的明细记录，实际达成率=实际数/计划数，应达成率 = 当班已经工作小时数（当前时间-当班开始时间）/12
                if (activeQty != 0 && (planQty == 0 ? 0 : (activeQty / planQty)) >= (workingTime / 12))
                    continue;
                OutputData data1 = new OutputData();

                data1.WipResource = d.Key.ResourceCode;
                data1.ProductCode = d.Key.ProductCode;
                data1.WipResourceName = d.Key.ResourceName;
                data1.ProductName = d.Key.ProductName;
                data1.ShortDescription = d.Key.ShortDescription;
                data1.PlanQty = d.Value.Sum(p => p.DispatchQty);
                data1.ActiveQty = activeQty;
                data1.DifferenceQty = data1.ActiveQty - data1.PlanQty;
                data.gridData2.Add(data1);
            }

            return data;

        }



        #endregion

        /// <summary>
        /// 调用接口方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="smomParam"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual T SmomPost<T>(string url, string method, params SmomParam[] smomParam)
        {
            if (url.IsNullOrEmpty())
            {
                throw new ValidationException("未配置Url地址;");
            }
            var response = SmomControlHepler.SmomPost<T>("KzBoardController", method, url, smomParam);
            return response;
        }

        /// <summary>
        /// 产线设备状态看板
        /// </summary>
        /// <param name="Factory"></param>
        /// <param name="Region"></param>
        /// <returns></returns>
        [ApiService("产线设备状态看板")]
        public virtual List<LineEquipmentStatusData> LineEquipmentStatus(string Factory, string Region, RegionBoardType regionBoardType)
        {
            return GetSmomPost<List<LineEquipmentStatusData>>(Factory, Region, "FactoryLineEquipmentStatus", regionBoardType);
        }

        /// <summary>
        /// 产线设备状态看板--工厂
        /// </summary>
        /// <param name="Factory"></param>
        /// <param name="Region"></param>
        /// <returns></returns>
        [ApiService("产线设备状态看板--工厂")]
        [AllowAnonymous]
        public virtual List<LineEquipmentStatusData> FactoryLineEquipmentStatus(string Factory, string Region, RegionBoardType regionBoardType)
        {
            //登录
            RT.Service.Resolve<KzLoginController>().Login(Factory);
            var curTime = RF.Find<DispatchTask>().GetDbTime();

            //获取产线明细(看板区域)
            var entityList = RT.Service.Resolve<RegionBoardsController>().GetRegionBoardDetailsByRegion(Region, regionBoardType);
            var wipResourceIds = entityList.Select(p => p.WipResourceId).ToList();

            //两小时以内有报工记录的，也算正常排产
            var twoHourResourceIds = new List<double>();

            #region twoHourResourceIds取值逻辑修改：20260528
            // 构建资源ID -> ReportHours 映射（默认2小时）
            var resourceHoursMap = entityList.ToDictionary(item => item.WipResourceId, item => item.ReportHours > 0 ? (double)item.ReportHours : 2.0);
            // 遍历所有资源ID，逐个查询
            foreach (var rid in wipResourceIds)
            {
                double hours = resourceHoursMap[rid];
                var count = Query<ReportRecordExamine>().Where(p => p.ReportTime >= curTime.AddHours(-hours) && (double)p.ResourceId == rid).Count();
                if (count > 0) { twoHourResourceIds.Add(rid); }
            }
            #endregion

            //wipResourceIds.SplitDataExecute(temp =>
            //{
            //    twoHourResourceIds.AddRange(Query<ReportRecordExamine>().Where(p => p.ReportTime >= curTime.AddHours(-2) && temp.Contains((double)p.ResourceId)).Select(p => (double)p.ResourceId).Distinct().ToList<double>().ToList());
            //});



            List<LineEquipmentStatusData> listData = new List<LineEquipmentStatusData>();
            //获取安灯异常产线
            var andonWipResourceIds = RT.Service.Resolve<AndonManageController>().GetAndonManageIds(wipResourceIds);

            var taskWipResourceIds = RT.Service.Resolve<DispatchTaskController>().GetSchedulingWipResourceIds(wipResourceIds);
            foreach (var item in entityList.OrderBy(p => p.Sort))
            {
                var entity = new LineEquipmentStatusData()
                {
                    LineName = item.ResourceName,
                    StatuType = StatuType.Scheduling
                };
                //绿色：该产线两个小时内有报工记录
                //黄色：该产线当前班次无排程任务单，且两个小时内没有报工记录
                //红色：存在安灯事件未关闭，或当前产线有排程任务但是两个小时内没有报工
                if (twoHourResourceIds.Any(p => p == item.WipResourceId))
                    entity.StatuType = StatuType.Scheduling;
                if (taskWipResourceIds.All(p => p != item.WipResourceId) && twoHourResourceIds.All(p => p != item.WipResourceId))
                    entity.StatuType = StatuType.Unscheduled;
                if (andonWipResourceIds.Any(p => p == item.WipResourceId) || (taskWipResourceIds.Any(p => p == item.WipResourceId) && twoHourResourceIds.All(p => p != item.WipResourceId)))
                    entity.StatuType = StatuType.Abnormal;

                listData.Add(entity);
            }
            if(listData.Count==0)
            {
                var entity = new LineEquipmentStatusData();
                listData.Add(entity);
            }
            return listData;
        }

        /// <summary>
        /// 调用工厂服务接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Factory"></param>
        /// <param name="Region"></param>
        /// <param name="FunctionName"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual T GetSmomPost<T>(string Factory, string Region,string FunctionName, RegionBoardType regionBoardType)
        {
            var setting = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingByFactoryCode(Factory);
            if (setting == null || setting.FactoryUrl.IsNullOrEmpty())
            {
                throw new ValidationException("未配置Url地址;");
            }
            var smomParam = new List<SmomParam>()
                    {
                    new SmomParam { Value =Factory },
                    new SmomParam{ Value = Region},
                    new SmomParam{ Value = regionBoardType}
                                 }.ToArray();

            var result = SmomPost<T>(setting.FactoryUrl, FunctionName, smomParam);
            var data = result;
            return data;
        }

        /// <summary>
        /// 老化看板
        /// </summary>
        /// <param name="Factory"></param>
        /// <param name="Region"></param>
        /// <returns></returns>
        [ApiService("老化看板")]
        public virtual List<ProductAgingData> GetProductAging(string Factory, string Region, RegionBoardType regionBoardType)
        {
            return GetSmomPost< List<ProductAgingData>>(Factory, Region, "FactoryGetProductAging", regionBoardType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ApiService("老化看板-工厂")]
        [AllowAnonymous]
        public virtual List<ProductAgingData> FactoryGetProductAging(string Factory, string Region, RegionBoardType regionBoardType)
        {
            //登录
            RT.Service.Resolve<KzLoginController>().Login(Factory);

            //获取产线明细(看板区域)
            var entityList = RT.Service.Resolve<RegionBoardsController>().GetRegionBoardMRBByRegion(Region, regionBoardType);
            var mrbs = entityList.Select(p => p.Code).ToList();
            var workOrders = RT.Service.Resolve<WorkOrderController>().GetWorkOrdersByMRB(mrbs);
            var workOrderNos = workOrders.Select(p => p.No).Distinct().ToList();
            //报工记录
            var reportRecordExamines = workOrderNos.SplitContains(temp =>
            {
                return Query<ReportRecordExamine>().Where(p => (p.Process.Code == "挤塑" || p.Process.Code == "热处理" || p.Process.Code == "精加工") && temp.Contains(p.Wo)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });

            var extrusions = reportRecordExamines.Where(p => p.ProcessCode == "挤塑").AsEntityList();
            var extrusionsIds = extrusions.Select(p => p.Id).ToList();
            var reportWipBatchList = RT.Service.Resolve<ReportController>().GetReportWipBatchsByReportIds(extrusionsIds);
            var lables = reportWipBatchList.Select(p => p.BatchNo).ToList();

            var heatTreatmentList =RT.Service.Resolve<HeatTreatmentController>().GetHeatTreatmentList(lables);
            //生成老化看板数据
            var data = new  KzBoardCommon().GenerateProductAgingData(reportRecordExamines, heatTreatmentList, workOrders, reportWipBatchList);
            data.ForEach(item =>
            {
                if (item.CurrentInProcessNum < item.MinInProcessNum)
                    item.IsInProcessNumMax = false;
                else if (item.CurrentInProcessNum > item.MaxInProcessNum)
                    item.IsInProcessNumMax = true;
                else
                    item.IsInProcessNumMax = null;
            });
            if (data.Count == 0)
            {
                data.Add(new ProductAgingData());
                return data;
            }
            return data.Where(p => p.IsInProcessNumMax != null).ToList();
        }

    }
}
