using SIE.Domain;
using SIE.WorkBenchCommon;
using SIE.WorkBenchCommon.Workbench.KPI;
using SIE.Wpf.Command;
using System;
using System.Collections;

namespace SIE.Wpf.WorkBenchCommon.Workbench.KPI
{
    /// <summary>
    /// 批量复制命令
    /// </summary>
    [Command(ImageName = "CopyEntity", Label = "批量复制", ToolTip = "添加数据", Gestures = "Ctrl+Shift+N", GroupType = 101)]
    class BlukAddQuotaTargetDetailCommand : ListAddCommand
    {
        /// <summary>
        /// 是否能被执行
        /// </summary>
        /// <param name="view">详细逻辑视图</param>
        /// <returns>返回是否能被执行</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            var quotatargetdetail = view.Current as QuotaTargetDetail;
            return quotatargetdetail != null;
        }

        /// <summary>
        /// 执行代码块
        /// </summary>
        /// <param name="view">详细逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var quotadetail = view.Current as QuotaTargetDetail;
            var quotadetail_ = view.Data as EntityList<QuotaTargetDetail>;
            var choseType = view.Parent.Current as QuotaTargetSetting;
            ArrayList arr = new ArrayList();

            foreach (var quota in quotadetail_)
            {
              if (choseType.DataType == DateType.YEAR)
              {
                arr.Add(quota.Year);
              }
              else
              {
                if (quota.Year == quotadetail.Year)
                {
                    if (choseType.DataType == DateType.MONTH)
                    {
                        arr.Add(quota.Month);
                    }
                    else
                    {
                        arr.Add(quota.Week);
                    }
                 }
               }          
           }
            
                if (choseType.DataType == DateType.YEAR)
                {
                    var year = quotadetail.Year;
                    var year_end = DateTime.Now.Year + 5;
                    for (var year_start = DateTime.Now.Year; year_start <= year_end; year_start++)
                    {
                        if (year_start == year || arr.Contains(year_start))
                            continue;
                        QuotaTargetDetail detail = new QuotaTargetDetail();
                        detail.Year = year_start;
                        detail.Target = quotadetail.Target;
                        detail.KpiOperators = quotadetail.KpiOperators;
                        detail.DataType = quotadetail.DataType;
                        view.Data.Add(detail);
                    }
                }

                if (choseType.DataType == DateType.MONTH)
                {
                    var month = quotadetail.Month;
                    for (int i = 1; i <= 12; i++)
                    {
                        if (i == month || arr.Contains(i))
                            continue;
                        QuotaTargetDetail detail = new QuotaTargetDetail();
                        detail.Year = quotadetail.Year;
                        detail.Month = i;
                        detail.Target = quotadetail.Target;
                        detail.KpiOperators = quotadetail.KpiOperators;
                        detail.DataType = quotadetail.DataType;
                        view.Data.Add(detail);
                    }
                }

                if (choseType.DataType == DateType.WEEK)
                {
                    var week = quotadetail.Week;
                    for (int i = 1; i <= 53; i++)
                    {
                        if (i == week || arr.Contains(i))
                            continue;
                        QuotaTargetDetail detail = new QuotaTargetDetail();
                        detail.Year = quotadetail.Year;
                        detail.Week = i;
                        detail.Target = quotadetail.Target;
                        detail.KpiOperators = quotadetail.KpiOperators;
                        detail.DataType = quotadetail.DataType;
                        DataHelper.GetFirstEndDayOfWeek(quotadetail.Year ?? 0, quotadetail.Week ?? 0);
                        view.Data.Add(detail);
                    }
                } 
        }
    }
}
