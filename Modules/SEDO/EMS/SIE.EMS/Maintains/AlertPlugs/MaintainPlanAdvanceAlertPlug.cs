using SIE.Common.Alert;
using SIE.ObjectModel;
using System;
using SIE.EMS.Maintains.Controller;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using SIE.Domain;
using SIE.EMS.Checks.Plans;
using SIE.EMS.Maintains.Plans;


namespace SIE.EMS.Maintains.AlertPlugs
{
    /// <summary>
    /// 保养计划提前预警
    /// </summary>
    [RootEntity, Serializable]
    [Alert("保养计划提前预警", typeof(MaintainPlanAdvanceAlertPlugConfig), typeof(AdvanceAlertResult), "保养计划提前预警")]
    public class MaintainPlanAdvanceAlertPlug : AlertBase
    {

        /// <summary>
        /// 表格列头
        /// </summary>
        private readonly List<string> ColList = new List<string>() { "行", "设备编码", "设备名称", "点检开始时间", "点检结束时间" };

        /// <summary>
        /// Html的Email的模板
        /// </summary>
        private readonly string HtmlEmailTemplate =
            @"<html>
                <head><title>{0}</title></head>
                <body>
                    <div style='background-color:lightgray'><p>{1}</p>
                    <table border='1' cellspacing='0' style='margin-left:25px'>
                        <tr style='background-color:yellow'>{2}</tr>
                        {3}
                    </table></br>                   
                   </div>
                </body>
            </html>";

        /// <summary>
        /// 执行
        /// </summary>
        /// <returns>预警参数</returns>
        public override AlertResultBase Run()
        {
         
            var config = this.Context.Config as MaintainPlanAdvanceAlertPlugConfig;
            var planList = RT.Service.Resolve<MaintainController>().GetAlertTimeOutMaintainPlanList();
            if (planList.Count <= 0)
            {
                return null;
            }
            var result = new AdvanceAlertResult();
            SetBody(result, planList);
            result.Value = config.AlertValue;
            return result;

            /* if (planList.Any())
             {
                 var result = new AdvanceAlertResult();
                 var message = new StringBuilder();
                 var i = 1;
                 foreach (var item in planList)
                 {
                     DateTime? beginDate = isPrecisePlan ? item.PrecisePlanBeginDate : item.PlanBeginDate;
                     DateTime? endDate = isPrecisePlan ? item.PrecisePlanEndDate : item.PlanEndDate;

                     message.Append("({0})设备编码{1}需要在{2}-{3}时间内完成保养任务，请及时保养<br/>"
                         .L10nFormat(i, item.EquipAccount.Code, beginDate, endDate));
                     i++;
                 }
                 result.Message = message.ToString();
                 result.Body = message.ToString();
                 result.Value = config.AlertValue;
                 return result;
             }
             return null;*/
        }


        /// <summary>
        /// 设置邮件内容
        /// </summary>
        /// <returns>返回邮件内容</returns>
        private string SetBody(AdvanceAlertResult result, EntityList<MaintainPlan> bills)
        {

            //是否精确计划保养
            bool isPrecisePlan = RT.Service.Resolve<MaintainController>().IsMaintainForPrecisePlan();
            string tempCol = string.Join("</th><th>", ColList);
            string col = string.Concat("<th>", tempCol, "</th>");

            StringBuilder message = new StringBuilder();
            message.Append("</br>");
            message.Append("&nbsp;&nbsp;");
            message.Append("您好，保养计划提前预警推送：".L10N());
            message.Append("</br></br>");
            message.Append(Environment.NewLine);
            message.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
            message.Append("保养计划提前预警的设备清单，请及时保养".L10N());
            StringBuilder contentBuilder = new StringBuilder();
            string style = "<tr style='color: black'><td>";
            for (int i = 0; i < bills.Count; i++)
            {
                var bill = bills[i];
                DateTime? beginDate = isPrecisePlan ? bill.PrecisePlanBeginDate : bill.PlanBeginDate;
                DateTime? endDate = isPrecisePlan ? bill.PrecisePlanEndDate : bill.PlanEndDate;
                var equip = bill.EquipAccount;
                contentBuilder.Append(style);
                contentBuilder.Append(i + 1);
                contentBuilder.Append("</td><td>");
                contentBuilder.Append(equip.Code);
                contentBuilder.Append("</td><td>");
                contentBuilder.Append(equip.Name);
                contentBuilder.Append("</td><td>");
                contentBuilder.Append(beginDate.Value.ToString("yyyy/MM/dd HH24:MI:SS"));
                contentBuilder.Append("</td><td>");
                contentBuilder.Append(endDate.Value.ToString("yyyy/MM/dd HH24:MI:SS"));
                contentBuilder.Append("</td></tr>");

            }
            result.Body = string.Format(HtmlEmailTemplate, null, message.ToString(), col, contentBuilder.ToString());
            result.Message = string.Format(HtmlEmailTemplate, null, message.ToString(), col, contentBuilder.ToString());
            return result.Body;
        }


    }

    /// <summary>
    /// 推送结果(字符串)
    /// </summary>
    [Serializable]
    public class AdvanceAlertResult : AlertResultBase
    {
        /// <summary>
        /// 推送信息
        /// </summary>
        [Label("推送信息")]
        public string Message { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Label("内容")]
        public string Body { get; set; }
    }

}
