using SIE.Common;
using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.BatchWIP.Products;
using SIE.MES.QTimes.Configs;
using SIE.MES.QTimes.Enums;
using SIE.MES.QTimes.Services;
using SIE.MES.QTimes.ViewModels;
using SIE.MES.WIP.Models;
using SIE.MES.WIP.Products;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SIE.MES.QTimes.Handles
{
    /// <summary>
    /// QT超时报表处理类
    /// </summary>
    public class QTimeReportHandle
    {
        /// <summary>
        /// 查询构造
        /// </summary>
        public QTimeReportHandle() 
        {
            SingleWipProductVersionInfos = new List<WipProductVersionInfo>();
            BatchWipProductVersionInfos = new List<WipProductVersionInfo>();
            QTimeStandards = new List<QTimeStandard>();
            TaskQTimeReports = new ConcurrentBag<QTimeReportViewModel>();
            QTimeReports = new EntityList<QTimeReportViewModel>();
        }

        #region 属性
        /// <summary>
        /// 生产条码信息
        /// </summary>
        private List<WipProductVersionInfo> SingleWipProductVersionInfos {  get; set; }

        /// <summary>
        /// 生产条码信息
        /// </summary>
        private List<WipProductVersionInfo> BatchWipProductVersionInfos { get; set; }

        /// <summary>
        /// QT规则
        /// </summary>
        private List<QTimeStandard> QTimeStandards { get; set; }

        /// <summary>
        /// QT报表数据
        /// </summary>
        private EntityList<QTimeReportViewModel> QTimeReports { get; set; }

        /// <summary>
        /// 多线程安全集合
        /// </summary>
        private ConcurrentBag<QTimeReportViewModel> TaskQTimeReports {  get; set; }

        /// <summary>
        /// 采集开始时间
        /// </summary>
        private DateTime ConfigBegin {  get; set; }

        /// <summary>
        /// 采集结束时间
        /// </summary>
        private DateTime ConfigEnd { get; set; }
        #endregion

        #region 方法
        /// <summary>
        /// 查询
        /// </summary>
        public void Query(QTimeReportViewModelCriteria criteria)
        {
            // 按优先级获取QT标准规则
            GetQTimeStandards();
            if (this.QTimeStandards.Count <= 0)
            {
                throw new ValidationException("没有可用的QT标准规则！".L10N());
            }

            // 根据时间找出符合的采集记录(含过滤)
            GetWipProductRecords(criteria);
            if (this.SingleWipProductVersionInfos.Count + this.BatchWipProductVersionInfos.Count <= 0)
            {
                throw new ValidationException("没有符合QT标准的条码！".L10N());
            }

            // 根据QT标准规则匹配数据
            GenerateReports();

            // 根据开始工序和结束工序二次过滤
            if (criteria.StartProcessId != null || criteria.EndProcessId != null)
            {
                RecordsByStartEndProcessId(criteria);
            }

            // 根据是否超时二次过滤
            if (criteria.IsOverTime.HasValue)
            {
                RecordsByOverTime(criteria);
            }
        }

        

        /// <summary>
        /// 调度
        /// </summary>
        public void JobSend()
        {
            // 配置项获取时间范围
            GetQtimeStandarConfig();
            QTimeReportViewModelCriteria criteria = new QTimeReportViewModelCriteria();
            DateRange dateRange = new DateRange()
            {
                BeginValue = ConfigBegin,
                EndValue = ConfigEnd,
            };
            criteria.CollectTime = dateRange;

            // 按优先级获取QT标准规则
            GetQTimeStandards();
            if (this.QTimeStandards.Count <= 0)
            {
                throw new ValidationException("没有可用的QT标准规则！".L10N());
            }

            // 根据时间找出符合的采集记录(含过滤)
            GetWipProductRecords(criteria);
            if (this.SingleWipProductVersionInfos.Count + this.BatchWipProductVersionInfos.Count <= 0)
            {
                throw new ValidationException("没有符合QT标准的条码！".L10N());
            }

            // 根据QT标准规则匹配数据
            GenerateReports();
        }

        /// <summary>
        /// 解析QT配置项时间
        /// </summary>
        private void AnalysisQTConfig(QTimeConfigTimeRange collectTime, int day)
        {
            switch (collectTime)
            {
                case QTimeConfigTimeRange.Today:
                    ConfigBegin = DateTime.Today;
                    ConfigEnd = DateTime.Today.AddDays(1).AddTicks(-1);
                    break;
                case QTimeConfigTimeRange.ThisWeek:
                    ConfigBegin = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                    ConfigEnd = DateTime.Today.AddDays(7 - (int)DateTime.Today.DayOfWeek).AddTicks(-1);
                    break;
                case QTimeConfigTimeRange.ThisMonth:
                    ConfigBegin = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    ConfigEnd = ConfigBegin.AddMonths(1).AddTicks(-1);
                    break;
                case QTimeConfigTimeRange.RecentlyMonth:
                    ConfigBegin = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1);
                    ConfigEnd = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddTicks(-1);
                    break;
                case QTimeConfigTimeRange.ThisYear:
                    ConfigBegin = new DateTime(DateTime.Today.Year, 1, 1);
                    ConfigEnd = new DateTime(DateTime.Today.Year, 12, 31).AddTicks(-1);
                    break;
                case QTimeConfigTimeRange.Customize:
                    ConfigBegin = DateTime.Today.AddDays(-day);
                    ConfigEnd = DateTime.Today.AddDays(1).AddTicks(-1);
                    break;
                default:
                    break;

            }
        }

        /// <summary>
        /// 获取QT调度获取条码时间范围配置项
        /// </summary>
        /// <exception cref="ValidationException"></exception>
        private void GetQtimeStandarConfig()
        {

            // DateRange类型不行
            var config = ConfigService.GetConfig(new QTimeJobTimeRangeConfig(), typeof(QTimeStandard));
            if (config == null)
            {
                throw new ValidationException("请维护配置项！".L10N());
            }
            else
            {
                if (!config.CollectTime.HasValue || !config.CollectTime.HasValue)
                {
                    throw new ValidationException("请维护采集时间！".L10N());
                }
                else
                {
                    AnalysisQTConfig(config.CollectTime.Value, config.Day);
                }
            }
        }


        /// <summary>
        /// 按优先级获取QT标准规则
        /// </summary>
        private void GetQTimeStandards()
        {
            var qtStandards = RT.Service.Resolve<QTimeStandardService>().GetQTByPriority();
            QTimeStandards.AddRange(qtStandards);
        }

        /// <summary>
        /// 根据规则找出符合开始工序+开始状态+结束工序+结束状态的数据
        /// </summary>
        /// <param name="criteria"></param>
        private void GetWipProductRecords(QTimeReportViewModelCriteria criteria)
        {
            // 单体采集记录
            var wipRecords = RT.Service.Resolve<WipProductVersionController>().GetWipProductVersionInfos(criteria);
            // 批次采集记录
            var batchRecords = RT.Service.Resolve<BatchWipProductVersionController>().GetWipProductVersionInfos(criteria);
            SingleWipProductVersionInfos.AddRange(wipRecords);
            BatchWipProductVersionInfos.AddRange(batchRecords);
        }

        /// <summary>
        /// 单体数据计算报表
        /// </summary>
        private void GenerateReports()
        {
            int totalRecordCount = SingleWipProductVersionInfos.Count + BatchWipProductVersionInfos.Count;
            int processorCount;
            if (totalRecordCount < 10000)
            {
                processorCount = Environment.ProcessorCount / (totalRecordCount < 1000 ? 4 : 2);
            }
            else
            {
                processorCount = Environment.ProcessorCount;
            }
            // 多线程
            ParallelOptions options = new ParallelOptions
            {
                MaxDegreeOfParallelism = processorCount // Set the maximum degree of parallelism
            };
            foreach (var standard in QTimeStandards)
            {
                if (!IsBatchProcess(standard.StartProcessType)) // 单体
                {
                    GenerateSingleReports(standard, options);
                }
                else // 批次
                {
                    GenerateBatchReports(standard, options);
                }
            }
            QTimeReports.AddRange(TaskQTimeReports);

        }

        // 数据分块方法
        private IEnumerable<List<T>> ChunkData<T>(List<T> data, int chunkSize)
        {
            for (int i = 0; i < data.Count; i += chunkSize)
            {
                yield return data.GetRange(i, Math.Min(chunkSize, data.Count - i));
            }
        }

        /// <summary>
        /// 判断是否为批次工序
        /// </summary>
        /// <param name="processType"></param>
        /// <returns></returns>
        private bool IsBatchProcess(ProcessType? processType)
        {
            return processType == ProcessType.BatchAssembly || processType == ProcessType.BatchPqc || processType == ProcessType.BatchFix || processType == ProcessType.BatchPacking;
        }

        /// <summary>
        /// QT标准维护状态转化
        /// </summary>
        /// <param name="qtState"></param>
        /// <returns></returns>
        private WipProductProcessState SingleStateChange(QTProcessState? qtState)
        {
            switch (qtState)
            {
                case QTProcessState.Start: return WipProductProcessState.Start;
                case QTProcessState.Finish: return WipProductProcessState.Finish;
                default: return WipProductProcessState.Finish;
            }
        }

        /// <summary>
        /// 判断是否是入站状态
        /// </summary>
        /// <param name="qtState"></param>
        /// <returns></returns>
        private bool IsInStation(QTProcessState? qtState)
        {
            switch (qtState)
            {
                case QTProcessState.In:return true;
                case QTProcessState.Out:return false;
                default:return false;
            }
        }

        /// <summary>
        /// QT标准维护状态转化为
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private QTProcessState SingleStateChange(WipProductProcessState state)
        {
            switch (state)
            {
                case WipProductProcessState.Start:
                    return QTProcessState.Start;
                case WipProductProcessState.Finish:
                    return QTProcessState.Finish;
                default: return QTProcessState.Finish;
            }
        }

        private decimal CalculateQT(DateTime? endTime, DateTime? startTime)
        {
            return Convert.ToDecimal(Math.Round((endTime - startTime).Value.TotalMinutes, 3));
        }

        /// <summary>
        /// QT标准规则时间值转化为分钟
        /// </summary>
        /// <param name="time"></param>
        /// <param name="qTTimeUnit"></param>
        /// <returns></returns>
        private decimal StandardConvertMinutes(decimal time, QTTimeUnit? qTTimeUnit)
        {
            switch (qTTimeUnit)
            {
                case QTTimeUnit.Minute:
                    return time;
                case QTTimeUnit.Hour:
                    return time * 60;
                case QTTimeUnit.Day:
                    return time * 60 * 24;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 生成报表数据(含末工序)
        /// </summary>
        /// <param name="start">开始工序</param>
        /// <param name="end">结束工序</param>
        /// <param name="qTimeStandard">QT标准</param>
        /// <param name="isBatch">是否为批次</param>
        private void WithEndReportData(WipProductVersionInfo start, WipProductVersionInfo end, QTimeStandard qTimeStandard, bool isBatch)
        {
            decimal qTime = 0;
            if (!isBatch)
            {
                qTime = CalculateQT(end.OperateTime, start.OperateTime);
            }
            else 
            {
                qTime = CalculateQT(IsInStation(qTimeStandard.EndState) ? end.InTime : end.OutTime, IsInStation(qTimeStandard.StartState) ? start.InTime : start.OutTime);
            }
            var standardQtime = StandardConvertMinutes(qTimeStandard.Time, qTimeStandard.TimeUnit);
            QTimeReportViewModel qTimeReportViewModel = new QTimeReportViewModel()
            {
                Qtime = qTime,
                QTStandard = standardQtime,
                Barcode = end.Sn,
                WoNo = end.WoNo,
                WipResource = end.WipResourceCode,
                ProductCode = end.ProductCode,
                ProductName = end.ProductName,
                BarcodeQty = !isBatch?1:end.Qty,
                StartProcessId = start.ProcessId, 
                StartProcess = start.ProcessName,
                StartState = qTimeStandard.StartState.Value,
                StartCollectTime = !isBatch ? start.OperateTime:start.InTime,
                EndProcessId = end.ProcessId,
                EndProcess = end.ProcessName,
                EndState = qTimeStandard.EndState.Value,
                EndCollectTime = !isBatch ? end.OperateTime:end.OutTime,
                QueryTime = DateTime.Now,
                IsOverTime = qTime > standardQtime,
                QTId = qTimeStandard.Id,
            };
            TaskQTimeReports.Add(qTimeReportViewModel);
        }

        /// <summary>
        /// 生成报表数据(不含末工序)
        /// </summary>
        /// <param name="start">开始工序</param>
        /// <param name="qTimeStandard">QT标准</param>
        /// <param name="isBatch">是否为批次</param>
        private void WithOutEndReportData(WipProductVersionInfo start, QTimeStandard qTimeStandard, bool isBatch)
        {
            decimal qTime = 0;
            if (!isBatch)
            {
                qTime = CalculateQT(DateTime.Now, start.OperateTime);
            }
            else
            {
                qTime = CalculateQT(DateTime.Now, IsInStation(qTimeStandard.StartState) ? start.InTime : start.OutTime);
            }
            var standardQtime = StandardConvertMinutes(qTimeStandard.Time, qTimeStandard.TimeUnit);
            QTimeReportViewModel qTimeReportViewModel = new QTimeReportViewModel()
            {
                Qtime = qTime,
                QTStandard = standardQtime,
                Barcode = start.Sn,
                WoNo = start.WoNo,
                WipResource = start.WipResourceCode,
                ProductCode = start.ProductCode,
                ProductName = start.ProductName,
                BarcodeQty = !isBatch ? 1 : start.Qty,
                StartProcessId = start.ProcessId,
                StartProcess = start.ProcessName,
                StartState = SingleStateChange(start.State),
                StartCollectTime = !isBatch ? start.OperateTime : start.InTime,
                QueryTime = DateTime.Now,
                IsOverTime = qTime > standardQtime,
                QTId = qTimeStandard.Id,
            };
            TaskQTimeReports.Add(qTimeReportViewModel);
        }

        /// <summary>
        /// 单体处理
        /// </summary>
        /// <param name="qTimeStandard"></param>
        /// <param name="options"></param>
        private void GenerateSingleReports(QTimeStandard qTimeStandard, ParallelOptions options)
        {
            // 根据开始工序+开始状态+结束工序+结束状态匹配采集记录
            var groupBySn = SingleWipProductVersionInfos.GroupBy(p => p.ParentId).ToList();
            Parallel.ForEach(groupBySn, options, group =>
            {
                List<WipProductVersionInfo> filterGroup = new List<WipProductVersionInfo>();
                if (qTimeStandard.ProductId != null)
                {
                    filterGroup = group.Where(p => p.ProductId == qTimeStandard.ProductId).ToList();
                }
                else
                {
                    filterGroup = group.Where(p => p.WipResourceId == qTimeStandard.WipResourceId).ToList();
                }
                //group
                //.Where(p => qTimeStandard.ProductId != null && p.ProductId == qTimeStandard.ProductId)
                //.Where(p => qTimeStandard.WipResourceId != null && p.WipResourceId == qTimeStandard.WipResourceId).ToList();
                // 只找出一条
                var start = filterGroup.FirstOrDefault(p => p.ProcessId == qTimeStandard.StartProcessId && p.State == SingleStateChange(qTimeStandard.StartState));
                var end = filterGroup.FirstOrDefault(p => p.ProcessId == qTimeStandard.EndProcessId && p.State == SingleStateChange(qTimeStandard.EndState));
                if (start != null)
                {
                    if (end != null && start.OperateTime <= end.OperateTime)
                    {
                        WithEndReportData(start, end, qTimeStandard, false);
                    }
                    else
                    {
                        WithOutEndReportData(start, qTimeStandard, false);
                    }
                }
            });
            
        }


        /// <summary>
        /// 批次处理
        /// </summary>
        /// <param name="qTimeStandard"></param>
        /// <param name="options"></param>
        private void GenerateBatchReports(QTimeStandard qTimeStandard, ParallelOptions options)
        {
            var groupBySn = BatchWipProductVersionInfos.GroupBy(p => p.ParentId).ToList();
            Parallel.ForEach(groupBySn, options, group =>
            {
                var start = qTimeStandard.ProductId != null ? group
                .Where(p => p.ProductId == qTimeStandard.ProductId).FirstOrDefault(): group
                .Where(p => p.WipResourceId == qTimeStandard.WipResourceId).FirstOrDefault(p => p.ProcessId == qTimeStandard.StartProcessId);
                var end = qTimeStandard.ProductId != null ? group
                .Where(p => p.ProductId == qTimeStandard.ProductId).FirstOrDefault():group
                .Where(p => p.WipResourceId == qTimeStandard.WipResourceId).FirstOrDefault(p => p.ProcessId == qTimeStandard.EndProcessId);
                if (start != null)
                {
                    if (end != null && start.InTime <= end.InTime)
                    {
                        WithEndReportData(start, end, qTimeStandard, true);
                    }
                    else
                    {
                        WithOutEndReportData(start, qTimeStandard, true);
                    }
                }
            });
        }

        /// <summary>
        /// 根据开始工序和结束工序二次过滤
        /// </summary>
        /// <param name="criteria"></param>
        private void RecordsByStartEndProcessId(QTimeReportViewModelCriteria criteria)
        {
            // 开始工序结束工序
            if (criteria.StartProcessId != null && criteria.EndProcessId == null)
            {
                QTimeReports = QTimeReports.Where(p => p.StartProcessId == criteria.StartProcessId).AsEntityList();
            }
            else if (criteria.StartProcessId == null && criteria.EndProcessId != null)
            {
                QTimeReports = QTimeReports.Where(p => p.EndProcessId == criteria.EndProcessId).AsEntityList();
            }
            else if (criteria.StartProcessId != null && criteria.EndProcessId != null)
            {
                QTimeReports = QTimeReports.Where(p => p.StartProcessId == criteria.StartProcessId && p.EndProcessId == criteria.EndProcessId).AsEntityList();
            }
            else
            {

            }

            
        }

        /// <summary>
        /// 根据是否超时二次过滤
        /// </summary>
        /// <param name="criteria"></param>
        private void RecordsByOverTime(QTimeReportViewModelCriteria criteria)
        {
            // 是否超时
            if (criteria.IsOverTime.HasValue)
            {
                if (criteria.IsOverTime == YesNo.Yes)
                {
                    QTimeReports = QTimeReports.Where(p => p.IsOverTime).AsEntityList();
                }
                else
                {
                    QTimeReports = QTimeReports.Where(p => !p.IsOverTime).AsEntityList();
                }
            }
        }

        /// <summary>
        /// 返回匹配到的标准规则Id
        /// </summary>
        /// <returns></returns>
        public List<QTimeStandard> GetQTStandards()
        {
            return QTimeStandards;
        }

        /// <summary>
        /// 按开始采集时间降序排序
        /// </summary>
        public void OrderByDescStartOperateTime()
        {
            this.QTimeReports = this.QTimeReports.OrderByDescending(p => p.StartCollectTime).AsEntityList();
        }

        /// <summary>
        /// 返回计算完的QT报表数据
        /// </summary>
        /// <returns></returns>
        public EntityList<QTimeReportViewModel> OutputData()
        {
            return QTimeReports;
        }

        /// <summary>
        /// 返回请求数据的分页信息
        /// </summary>
        /// <returns></returns>
        public int OutputPageInfo()
        {
            return QTimeReports.Count;
        }
        #endregion

        #region 接口

        #endregion
    }
}
