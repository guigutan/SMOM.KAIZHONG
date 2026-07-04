using SIE.Domain;
using SIE.MES.DashBoard.Reports.Commons;
using SIE.MES.Statistics.Fpy;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.DashBoard.Reports.LineFPY
{
    /// <summary>
    /// 产线直通率报表控制器
    /// </summary>
    public class LineReportViewModelController : DomainController
    {
        LineReportViewModelCriteria _criteria;

        /// <summary>
        /// 查询产线报表ViewModel
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>车间报表ViewModel</returns>
        public virtual LineReportViewModel GetLineReportViewModel(LineReportViewModelCriteria criteria)
        {
            this._criteria = criteria;

            var lineFpyStatistics = RT.Service.Resolve<FpyController>().GetLineFpyStatistics(criteria?.Resource?.Name, criteria?.Shift?.Name, dateRange: criteria?.CollectDate);
            //获取资源Id与资源名称的字典，因为名称可能被修改
            var resIds = lineFpyStatistics.Select(p => p.ResourceId).Distinct().ToList();
            var resNameDics = RT.Service.Resolve<WipResourceController>().GetResourceList(resIds).ToDictionary(k => k.Id, v => v.Name);

            var viewModel = new LineReportViewModel();
            viewModel.LayoutFileName = viewModel.GetType().Name;
            lineFpyStatistics.GroupBy(p => p.CollectedDate).ForEach(p =>
            {
                viewModel.LineDirectRateList.AddRange(CreateModel(p.ToList(), resNameDics));
            });

            var lineids = viewModel.LineDirectRateList.Select(p => p.LineId).OfType<double>().ToList();
            var lineSettingList = RT.Service.Resolve<FpySettings.FpySettingController>().GetLineFpySettingsByLineIds(lineids);
            viewModel.LineDirectRateList.ForEach(p =>
            {
                var setting = lineSettingList.FirstOrDefault(t => t.ResourceId == p.LineId) ?? new FpySettings.LineFpySetting();
                p.LineDirectRate = setting;
            });

            viewModel.MarkSaved();
            return viewModel;
        }

        /// <summary>
        /// 创建报表数据ViewModel
        /// </summary>
        /// <returns>报表数据ViewModel</returns>
        private List<LineDirectRateViewModel> CreateModel(List<ProcessFpyStatistics> fpys, Dictionary<double, string> dics)
        {
            List<LineDirectRateViewModel> models = new List<LineDirectRateViewModel>();

            fpys.GroupBy(p => "{0}:{1}".FormatArgs(p.ResourceId, p.ShiftId)).ForEach(p =>
            {
                decimal processRate = 1;
                var tempList = p.ToList();
                var temp = tempList[0];
                tempList.GroupBy(q => q.ProcessId).ForEach(q =>
                {
                    if (q.Sum(x => x.InputQty) > 0)
                    {
                        processRate *= q.Sum(o => o.PassQty) / q.Sum(o => o.InputQty);
                    }
                });

                var viewModel = new LineDirectRateViewModel()
                {
                    Year = temp.CollectedDate.Year,
                    Month = temp.CollectedDate.ToString("yyyy年MM月"),
                    Week = "{0}年第{1}周".FormatArgs(temp.CollectedDate.Year, RT.Service.Resolve<CommonController>().GetWeekOfYear(temp.CollectedDate)),
                    Date = temp.CollectedDate.Date,
                    LineId = temp.ResourceId,
                    LineName = dics.ContainsKey(temp.ResourceId) ? dics[temp.ResourceId] : temp.ResourceName,
                    ShiftId = temp.ShiftId,
                    Shift = temp.ShiftName,
                    DirectRate = processRate
                };

                viewModel.GenerateId();
                models.Add(viewModel);
            });

            return models;
        }

        /// <summary>
        /// 获取产线自定义直通率值
        /// </summary>
        /// <returns>自定义值字典</returns>
        public virtual Dictionary<string, decimal> GetCustomSummeries()
        {
            if (_criteria == null)
            {
                _criteria = new LineReportViewModelCriteria();
            }
            var lineFpyStatistics = RT.Service.Resolve<FpyController>().GetLineFpyStatistics(_criteria?.Resource?.Name, _criteria?.Shift?.Name, dateRange: _criteria?.CollectDate);

            //获取资源Id与资源名称的字典，因为名称可能被修改
            var resIds = lineFpyStatistics.Select(p => p.ResourceId).Distinct().ToList();
            var resNameDics = RT.Service.Resolve<WipResourceController>().GetResourceList(resIds).ToDictionary(k => k.Id, v => v.Name);

            var dics = new Dictionary<string, decimal>();
            FpyStatisticsGroup(dics, lineFpyStatistics, p =>
            {
                var resName = resNameDics.ContainsKey(p.ResourceId) ? resNameDics[p.ResourceId] : p.ResourceName;
                return p.CollectedDate + ":" + resName;
            });

            FpyStatisticsGroup(dics, lineFpyStatistics, p =>
            {
                var resName = resNameDics.ContainsKey(p.ResourceId) ? resNameDics[p.ResourceId] : p.ResourceName;
                return p.CollectedDate.Year + ":" + resName;
            });

            FpyStatisticsGroup(dics, lineFpyStatistics, p =>
            {
                var resName = resNameDics.ContainsKey(p.ResourceId) ? resNameDics[p.ResourceId] : p.ResourceName;
                return p.CollectedDate.ToString("yyyy年MM月") + ":" + resName;
            });

            FpyStatisticsGroup(dics, lineFpyStatistics, p =>
            {
                var resName = resNameDics.ContainsKey(p.ResourceId) ? resNameDics[p.ResourceId] : p.ResourceName;
                return "{0}年第{1}周".FormatArgs(p.CollectedDate.Year, RT.Service.Resolve<CommonController>().GetWeekOfYear(p.CollectedDate)) + ":" + resName;
            });

            FpyStatisticsGroup(dics, lineFpyStatistics, p =>
            {
                var resName = resNameDics.ContainsKey(p.ResourceId) ? resNameDics[p.ResourceId] : p.ResourceName;
                return p.CollectedDate.Year + ":" + resName + ":" + p.ShiftName;
            });

            FpyStatisticsGroup(dics, lineFpyStatistics, p =>
            {
                var resName = resNameDics.ContainsKey(p.ResourceId) ? resNameDics[p.ResourceId] : p.ResourceName;
                return p.CollectedDate.ToString("yyyy年MM月") + ":" + resName + ":" + p.ShiftName;
            });

            FpyStatisticsGroup(dics, lineFpyStatistics,p =>
            {
                var resName = resNameDics.ContainsKey(p.ResourceId) ? resNameDics[p.ResourceId] : p.ResourceName;
                return "{0}年第{1}周".FormatArgs(p.CollectedDate.Year, RT.Service.Resolve<CommonController>().GetWeekOfYear(p.CollectedDate)) + ":" + resName + ":" + p.ShiftName;
            });

            return dics;
        }

        /// <summary>
        /// 直通率Group执行
        /// </summary>
        /// <param name="dics"></param>
        /// <param name="lineFpyStatistics"></param>
        /// <param name="keySelector"></param>
        private void FpyStatisticsGroup(Dictionary<string, decimal> dics, EntityList<ProcessFpyStatistics> lineFpyStatistics, Func<ProcessFpyStatistics, string> keySelector)
        {
            lineFpyStatistics.GroupBy(p =>
            {
                return keySelector(p);
            })
            .ForEach(p =>
            {
                decimal processRate = 1M;
                p.GroupBy(q => q.ProcessId).ForEach(q =>
                {
                    if (q.Sum(x => x.InputQty) > 0)
                    {
                        processRate *= q.Sum(x => x.PassQty) / q.Sum(x => x.InputQty);
                    }
                });

                dics[p.Key] = processRate;
            });
        }
    }
}
