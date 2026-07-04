using System;
using System.Collections.Generic;
using SIE.Security;
using SIE.Web.Data;
using SIE.Web.Json;
using SIE.WorkBenchCommon.Workbench.KPI;

namespace SIE.Web.WorkBenchCommon.DataQueryer
{
    /// <summary>
    /// 工作台数据查询器
    /// </summary>
    [AllowAnonymous]
    public class WorkBenchDataQueryer : Data.DataQueryer
    {
        public List<EntityJson> LineChartDemoData()
        {
            Random rd = new Random();
            List<EntityJson> ret = new List<EntityJson>();

            string[] months = new string[] { "1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月" };
            for (int i = 0; i < 12; i++)
            {
                string month = months[i];

                int data1 = rd.Next(1, 30);
                int data2 = rd.Next(10, 40);
                int data3 = rd.Next(50, 100);

                EntityJson mv = new EntityJson();
                mv.SetProperty("month", month);
                mv.SetProperty("data1", data1);
                mv.SetProperty("data2", data2);
                mv.SetProperty("data3", data3);
                ret.Add(mv);
            }

            return ret;
        }

        /// <summary>
        /// 获取绩效分析
        /// </summary>
        /// <returns>绩效分析列表</returns>
        public List<KpiAnalysisData> GetKpiAnalysisDatas()
        {
            var kpiAnalysis = new List<KpiAnalysisData>();
            kpiAnalysis.Add(new KpiAnalysisData() { Name = "首检通过率", GoalFormat = "≥95.00%", ActualFormat = "94.36%", KpiGrade = KpiGrade.Great });
            kpiAnalysis.Add(new KpiAnalysisData() { Name = "产品市场维修率", GoalFormat = "<4.00%", ActualFormat = "0.00%", KpiGrade = KpiGrade.Great });
            kpiAnalysis.Add(new KpiAnalysisData() { Name = "改善关闭率", GoalFormat = "≥95.00%", ActualFormat = "80.00%", KpiGrade = KpiGrade.Good });
            kpiAnalysis.Add(new KpiAnalysisData() { Name = "来料批次合格率", GoalFormat = "≥95.00%", ActualFormat = "100.00%", KpiGrade = KpiGrade.Great });
            kpiAnalysis.Add(new KpiAnalysisData() { Name = "物料下线率", GoalFormat = "≤5.00%", ActualFormat = "90.29%", KpiGrade = KpiGrade.Poor });
            kpiAnalysis.Add(new KpiAnalysisData() { Name = "成品批次合格率", GoalFormat = "≥95.00%", ActualFormat = "100.00%", KpiGrade = KpiGrade.Great });
            kpiAnalysis.Add(new KpiAnalysisData() { Name = "产品市场维修率", GoalFormat = "<4.00%", ActualFormat = "0.00%", KpiGrade = KpiGrade.Great });
            kpiAnalysis.Add(new KpiAnalysisData() { Name = "每日首检直通率", GoalFormat = ">95.00%", ActualFormat = "90.00%", KpiGrade = KpiGrade.Good });
            kpiAnalysis.Add(new KpiAnalysisData() { Name = "JIHUA", GoalFormat = "=0", ActualFormat = "58", KpiGrade = KpiGrade.Great });
            return kpiAnalysis;
        }

        /// <summary>
        /// 获取任务闭环管理测试
        /// </summary>
        /// <returns>任务闭环管理测试</returns>
        public List<EntityJson> GetTaskManagerStore()
        {
            List<EntityJson> ret = new List<EntityJson>();
            EntityJson mv = new EntityJson();
            mv.SetProperty("Title", "任务标题1");
            mv.SetProperty("Content", "任务内容测试1");
            ret.Add(mv);
            EntityJson mv1 = new EntityJson();
            mv1.SetProperty("Title", "任务标题2");
            mv1.SetProperty("Content", "任务内容测试2");
            ret.Add(mv1);
            EntityJson mv2 = new EntityJson();
            mv2.SetProperty("Title", "任务标题3");
            mv2.SetProperty("Content", "任务内容测试3");
            ret.Add(mv2);
            return ret;
        }
    }
}
