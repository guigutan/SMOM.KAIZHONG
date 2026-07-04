using SIE.Domain;
using SIE.MES.DashBoard.TeamManagement;
using SIE.Web.Data;
using SIE.Web.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.MES.DashBoard.TeamManagement
{
    /// <summary>
    /// 客制查询数据处理
    /// </summary>
    public class ScoreRecordDataQueryer : DataQueryer
    {
        private const string TD_TAG = "<td>";
        private const string TD_END = "</td>";

        /// <summary>
        /// 获取评分记录数据
        /// </summary>
        /// <param name="criter">查询实体</param>
        /// <returns>评分记录信息</returns>
        public EntityJson GetScoreRecordsVM(ScoreRecordVMCriteria criter)
        {
            var stores = RT.Service.Resolve<ScoreRecordVMController>().GetScoreRecordsVM(criter);
            List<EntityJson> res = new List<EntityJson>();
            List<EntityJson> chartData = new List<EntityJson>();
            int rowCount = 0;
            int columnCount = stores.Select(p => p.EmpId).Distinct().Count();
            string empName = string.Empty;
            string empId = string.Empty;
            ////定义图坐标范围
            decimal min = stores.Count > 0 ? stores.Select(p => p.Score).Min() - 5 : 0;
            decimal max = stores.Count > 0 ? stores.Select(p => p.Score).Max() + 10 : 100;
            var begin = criter.OccurDate.BeginValue.Value;
            var end = criter.OccurDate.EndValue.Value;
            ArrayList monthArrayList = new ArrayList();
            DateDataHandle(criter.DateType, begin, end, monthArrayList);
            rowCount = monthArrayList.Count;
            ////定义一个二维数组，ExtChart数据格式使用纵向数据，数据组成，行：时间轴+每个人的分数，列：第一列时间轴，后面每列代表一个人在时间轴的数据
            string[,] chartarr = new string[rowCount, columnCount + 1];
            for (int i = 0; i < rowCount; i++)
            {
                ////第一列存时间轴
                chartarr[i, 0] = monthArrayList[i].ToString();
            }

            ArrayList empIdList = new ArrayList();
            if (stores.Count > 0)
            {
                int j = 1;
                stores.GroupBy(p => p.EmpId).ForEach(store =>
                  {
                      EntityJson node = new EntityJson();
                      var list = store.OrderBy(e => e.ActualDate).ToList();
                      var itemone = list.FirstOrDefault();
                      node.SetProperty("empId", store.Key);
                      node.SetProperty("workgroupId", itemone.WorkgroupId);
                      node.SetProperty("workgroupName", itemone.WorkGroupName);
                      node.SetProperty("empName", itemone.EmpName);
                      empIdList.Add(store.Key);
                      empId += store.Key + ",";
                      empName += itemone.EmpName + ",";
                      ArrayList dataArrList = new ArrayList();
                      ////按时间排序，确保数据顺序一致，dataArrList按日期顺序存放当前员工分数
                      list.ForEach(e =>
                         {
                             node.SetProperty("Date" + e.OccurDate, e.Score);

                             dataArrList.Add(e.Score);
                             var d = e.ActualDate.Value;
                             if (criter.DateType == DateType.Day && (d.AddDays(1).Day == 1 || d.Date == end))
                             {
                                 var mTotal = list.Where(f => f.ActualDate.Value.Year == d.Year && f.ActualDate.Value.Month == d.Month).Sum(f => f.Score) + 100;
                                 node.SetProperty("Total" + d.Year.ToString() + d.Month, mTotal);
                             }
                         });
                      ////用于点击其中一行改变图的坐标范围                                     
                      node.SetProperty("min", list.Select(p => p.Score).Min());
                      node.SetProperty("max", list.Select(p => p.Score).Max());
                      res.Add(node);

                      for (int i = 0; i < rowCount; i++)
                      {
                          ////每一个人占一列数据
                          chartarr[i, j] = dataArrList[i].ToString();
                      }

                      j++;
                  });
            }

            for (int i = 0; i < rowCount; i++)
            {
                EntityJson chartempnode = new EntityJson();
                chartempnode.SetProperty("monthDay", chartarr[i, 0]);
                for (int j = 1; j <= columnCount; j++)
                {
                    chartempnode.SetProperty("emp" + empIdList[j - 1], chartarr[i, j]);
                }

                chartData.Add(chartempnode);
            }

            EntityJson resNode = new EntityJson();
            resNode.SetProperty("gridData", res);
            resNode.SetProperty("chartMin", min);
            resNode.SetProperty("chartMax", max);
            resNode.SetProperty("chartEmpId", empId.TrimEnd(','));
            resNode.SetProperty("chartEmp", empName.TrimEnd(','));
            resNode.SetProperty("chartData", chartData);
            return resNode;
        }

        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="criter">查询</param>
        /// <returns>table数据</returns>
        public string ExportScoreRecords(ScoreRecordVMCriteria criter)
        {
            var stores = RT.Service.Resolve<ScoreRecordVMController>().GetScoreRecordsVM(criter);
            StringBuilder sb = new StringBuilder();
            StringBuilder head =new StringBuilder ( "<tr><td>员工</td><td>班组</td>");
            var begin = criter.OccurDate.BeginValue.Value;
            var end = criter.OccurDate.EndValue.Value;
            #region head数据处理
            if (criter.DateType == DateType.Day)
            {
                for (; begin <= end; begin = begin.AddDays(1))
                {
                    string d = begin.Year.ToString().Substring(2, 2) + "/" + begin.Month + "/" + begin.Day;
                    head.Append(TD_TAG + d + TD_END);
                }

                head.Append("<td>合计</td>");
            }
            else
            {
                bool isOverYear = false;
                if (begin.Year != end.Year)
                {
                    isOverYear = true;
                }
                for (; begin <= end; begin = begin.AddMonths(1))
                {
                    string m = isOverYear ? begin.Year + "年" + begin.Month + "月" : begin.Month + "月";
                    head.Append(TD_TAG + m + TD_END);
                }
            }

            head.Append("</tr>");
            #endregion
            if (stores.Count > 0)
            {
                sb.Append("<table>" + head);
                stores.GroupBy(p => p.EmpId).ForEach(store =>
                {
                    var list = store.OrderBy(e => e.ActualDate).ToList();
                    var itemone = list.FirstOrDefault();
                    sb.Append(string.Format("<tr><td>{0}</td><td>{1}</td>", itemone.EmpName, itemone.WorkGroupName));
                    list.ForEach(e =>
                    {
                        sb.Append(TD_TAG + e.Score + TD_END);
                        var d = e.ActualDate.Value;
                        if (criter.DateType == DateType.Day && (d.AddDays(1).Day == 1 || d.Date == end))
                        {
                            var mTotal = list.Where(f => f.ActualDate.Value.Year == d.Year && f.ActualDate.Value.Month == d.Month).Sum(f => f.Score) + 100;
                            sb.Append(TD_TAG + mTotal + TD_END);
                        }
                    });
                    sb.Append("</tr>");
                });
                sb.Append("</table>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 时间范围数据处理
        /// </summary>
        /// <param name="dT">查询类型</param>
        /// <param name="begin">开始日期</param>
        /// <param name="end">结束日期</param>
        /// <param name="monthArrayList">包含每个时间轴的数组</param>
        private void DateDataHandle(DateType dT, DateTime begin, DateTime end, ArrayList monthArrayList)
        {
            if (dT == DateType.Day)
            {
                for (; begin <= end; begin = begin.AddDays(1))
                {
                    string d = begin.Year.ToString().Substring(2, 2) + "/" + begin.Month + "/" + begin.Day;
                    monthArrayList.Add(d);
                }
            }
            else if (dT == DateType.Month)
            {
                bool isOverYear = false;
                if (begin.Year != end.Year)
                {
                    isOverYear = true;
                }
                for (; begin <= end; begin = begin.AddMonths(1))
                {
                    string m = isOverYear ? begin.Year + "年".L10N() + begin.Month + "月".L10N() : begin.Month + "月".L10N();
                    monthArrayList.Add(m);
                }
            }
            else
            {
                //
            }
        }
    }
}
