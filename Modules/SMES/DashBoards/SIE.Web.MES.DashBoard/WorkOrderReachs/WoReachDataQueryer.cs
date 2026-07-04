using SIE.Domain;
using SIE.MES.DashBoard.TeamManagement;
using SIE.MES.DashBoard.WorkOrderReachs;
using SIE.Web.Data;
using SIE.Web.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.MES.DashBoard.WorkOrderReachs
{
    /// <summary>
    /// 客制查询数据处理
    /// </summary>
    public class WoReachDataQueryer : DataQueryer
    {
        private const string TD_END = "</td>";
        private const string TD_TAG = "<td>";

        /// <summary>
        /// 获取评分记录数据
        /// </summary>
        /// <param name="criter">查询实体</param>
        /// <returns>评分记录信息</returns>
        public WoReachViewModel GetWoReachData(WorkOrderReachCriteria criter)
        {
            var stores = RT.Service.Resolve<WorkOrderReachController>().GetWorkOrderReachViewModel(criter);
            WoReachViewModel woReachViewModel = new WoReachViewModel();
            if (stores == null || stores.Count == 0)
            {
                return woReachViewModel;
            }

            List<EntityJson> chartData = new List<EntityJson>();
            ////定义图坐标范围                   
            ArrayList monthArrayList = new ArrayList();
            DateDataHandle(criter.DateType, stores, monthArrayList);
            int rowCount = monthArrayList.Count;
            const int columnCount = 5;
            ////定义一个二维数组，ExtChart数据格式使用纵向数据，数据组成，行：时间轴+每一项的数据，列：第一列时间轴，后面每列代表每一项在时间轴的数据
            string[,] chartarr = new string[rowCount, columnCount + 1];
            for (int i = 0; i < rowCount; i++)
            {
                ////第一列存时间轴
                chartarr[i, 0] = monthArrayList[i].ToString();
            }

            string[] rowNameList = new string[] { "totalQty", "completeQty", "completeRate", "closedQty", "closedRate" };
            int j = 1;
            stores.GroupBy(p => p.RowName).ForEach(store =>
              {
                  var list = store.OrderBy(e => e.PlanDate).ToList();
                  ArrayList dataArrList = new ArrayList();
                  ////按时间排序，确保数据顺序一致，dataArrList按日期顺序存放当前员工分数
                  list.ForEach(e =>
                 {
                     ReachDataViewModel dataModel = new ReachDataViewModel();
                     dataModel.WoInfo = store.Key;
                     dataModel.Date = e.PlanDate.Date;
                     if (store.Key.IndexOf("率".L10N()) != -1)
                     {
                         dataModel.ReachData = decimal.Parse((e.Data).ToString("F2"));
                         dataModel.IsRate = true;
                     }
                     else
                     {
                         dataModel.ReachData = (decimal)e.Data;
                         dataModel.IsRate = false;
                     }

                     woReachViewModel.ReachDataList.Add(dataModel);
                     dataArrList.Add(e.Data);
                 });

                  for (int i = 0; i < rowCount; i++)
                  {
                      ////每一个人占一列数据
                      chartarr[i, j] = dataArrList[i].ToString();
                  }

                  j++;
              });

            for (int i = 0; i < rowCount; i++)
            {
                EntityJson chartempnode = new EntityJson();
                chartempnode.SetProperty("monthDay", chartarr[i, 0]);
                for (int k = 1; k <= columnCount; k++)
                {
                    chartempnode.SetProperty(rowNameList[k - 1], chartarr[i, k]);
                }

                chartData.Add(chartempnode);
            }

            woReachViewModel.ChartJsonData.AddRange(chartData);
            return woReachViewModel;
        }

        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="criter">查询</param>
        /// <returns>table数据</returns>
        public string ExportReachRecords(WorkOrderReachCriteria criter)
        {
            var stores = RT.Service.Resolve<WorkOrderReachController>().GetWorkOrderReachViewModel(criter);
            if (stores == null)
                return string.Empty;
            StringBuilder sb = new StringBuilder();
            string head = "<table><tr><td>工单信息</td>";
            List<string> arr = new List<string>() { "工单总数", "准时完工数", "准时达成率", "工单完工数", "工单完工率" };
            if (stores.Count > 0)
            {
                bool first = true;
                arr.ForEach(p =>
                {
                    var list = stores.Where(e => e.RowName == p).ToList();
                    sb.Append("<tr><td>" + p + TD_END);
                    list.OrderBy(e => e.PlanDate).ForEach(f =>
                      {
                          if (first)
                          {
                              switch (criter.DateType)
                              {
                                  case DateType.Year:
                                      head += TD_TAG + f.Year + TD_END;
                                      break;
                                  case DateType.Month:
                                      head += TD_TAG + f.Month + TD_END;
                                      break;
                                  case DateType.Week:
                                      head += TD_TAG + f.Week + TD_END;
                                      break;
                                  case DateType.Day:
                                      head += TD_TAG + f.PlanDate.ToShortDateString() + TD_END;
                                      break;
                                  default:
                                      break;
                              }
                          }
                          sb.Append(p.IndexOf("率") != -1 ? TD_TAG + f.Data.ToString("P2") + TD_END : TD_TAG + f.Data + TD_END);
                      });
                    first = false;
                    sb.Append("</tr>");
                });
                sb.Append("</tr></table>");
                sb = new StringBuilder(head + "</tr>" + sb);
            }

            return sb.ToString();
        }

        /// <summary>
        /// get detaill
        /// </summary>
        /// <param name="colValue"></param>
        /// <param name="criter"></param>
        /// <param name="pageinfo"></param>
        /// <returns></returns>
        public EntityList<WoReachDetailViewModel> GetWoReachDetail(string colValue, WorkOrderReachCriteria criter, PagingInfo pageinfo)
        {
            switch (criter.DateType)
            {
                case DateType.Year:
                    criter.ColumnFieldName = nameof(WorkOrderReachViewModel.Year);
                    break;
                case DateType.Month:
                    criter.ColumnFieldName = nameof(WorkOrderReachViewModel.Month);
                    break;
                case DateType.Week:
                    criter.ColumnFieldName = nameof(WorkOrderReachViewModel.Week);
                    break;
                case DateType.Day:
                    criter.ColumnFieldName = nameof(WorkOrderReachViewModel.PlanDate);
                    break;
                default:
                    break;
            }
            criter.ColumnFieldValue = colValue;
            var rst = RT.Service.Resolve<WorkOrderReachController>().GetWoReachDetailList(criter, pageinfo);
            return rst;
        }

        /// <summary>
        /// 时间范围数据处理
        /// </summary>
        /// <param name="dT">查询类型</param>
        /// <param name="WorkOrderReachList"></param>
        /// <param name="monthArrayList">包含每个时间轴的数组</param>
        private void DateDataHandle(DateType dT, EntityList<WorkOrderReachViewModel> WorkOrderReachList, ArrayList monthArrayList)
        {
            if (dT == DateType.Day)
            {
                var dateList = WorkOrderReachList.Select(p => p.PlanDate).Distinct().ToList();
                dateList.OrderBy(p => p).ForEach(p =>
                  {
                      string d = p.Year.ToString().Substring(2, 2) + "/" + p.Month + "/" + p.Day;
                      monthArrayList.Add(d);
                  });
            }
            else if (dT == DateType.Month)
            {
                var yearList = WorkOrderReachList.OrderBy(p => p.PlanDate).Select(p => p.Month).ToList();
                yearList = yearList.Distinct().ToList();
                yearList.OrderBy(p => p).ForEach(p => monthArrayList.Add(p));
            }
            else if (dT == DateType.Year)
            {
                var yearList = WorkOrderReachList.OrderBy(p => p.PlanDate).Select(p => p.Year).ToList();
                yearList = yearList.Distinct().ToList();
                yearList.OrderBy(p => p).ForEach(p => monthArrayList.Add(p));
            }
            else if (dT == DateType.Week)
            {
                var weekList = WorkOrderReachList.OrderBy(p => p.PlanDate).Select(p => p.Week).ToList();
                weekList = weekList.Distinct().ToList();
                weekList.ForEach(p => monthArrayList.Add(p));
            }
            else
            {
                //
            }
        }


        ///// <summary>
        ///// 定义chart表头的内容
        ///// </summary>
        ///// <returns>表头内容</returns>
        ////private string InitColumnList(EntityList<WorkOrderReachViewModel> stores, DateType dtpye)
        ////{
        ////    string columnDataIndexList = string.Empty;
        ////    var dateList = stores.Select(p => p.PlanDate).Distinct().ToList();
        ////    dateList.OrderBy(p => p.Date).ForEach(e =>
        ////    {
        ////        switch (dtpye)
        ////        {
        ////            case DateType.Year: columnDataIndexList += (e.Year + "^Date" + e.Date.ToString("yyyyMMdd") + ","); break;
        ////            case DateType.Month: columnDataIndexList += (e.Month + "^Date" + e.Date.ToString("yyyyMMdd") + ","); break;
        ////            case DateType.Week:
        ////                {
        ////                    var week = stores.Where(p => p.PlanDate == e).FirstOrDefault();
        ////                    columnDataIndexList += (week + "^Date" + e.Date.ToString("yyyyMMdd") + ",");
        ////                }
        ////                break;
        ////            case DateType.Day: columnDataIndexList += (e.Date.Day + "号^Date" + e.Date.ToString("yyyyMMdd") + "^" + e.Month + ","); break;
        ////        }
        ////    });
        ////    return columnDataIndexList;
        ////}
    }
}
