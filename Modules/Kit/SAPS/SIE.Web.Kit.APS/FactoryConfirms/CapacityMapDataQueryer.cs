using SIE.Domain;
using SIE.Kit.APS.FactoryConfirms;
using SIE.Kit.APS.TargetCapacitys;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.Kit.APS.FactoryConfirms
{
    /// <summary>
    /// 产能图数据查找
    /// </summary>
    public class CapacityMapDataQueryer : DataQueryer
    {
        /// <summary>
        /// 产能图
        /// </summary>
        public List<OutputCapacityInfo> outputCapacityInfos
        {
            get; set;
        }

        /// <summary>
        /// 获取工厂最大值
        /// </summary>
        private Dictionary<double, decimal> dicFacityMaxVaule
        {
            get; set;
        }

        /// <summary>
        /// 表头
        /// </summary>
        /// <returns></returns>
        public string TableHead()
        {
            StringBuilder stringBuilder = new StringBuilder();
            //stringBuilder.Append("<div /*style='padding-right:17px'*/> <table style='border-collapse:collapse;table-layout: fixed; text-align:center; margin:auto;width:98%'>");
            stringBuilder.Append("<div style='padding-right:17px'> <table style='border-collapse:collapse;table-layout: fixed; text-align:center; margin:auto;width:98%'>");
            stringBuilder.Append("<thead> <th style ='height:40px; width:6%; border: 1px solid #000; text-align:center;'> 库存组织1 </th>");
            stringBuilder.Append("<th style ='height:40px; width:6%;  border:1px solid #000; text-align:center;'>1月</th>");
            stringBuilder.Append("<th style ='height:40px; width:6%;  border:1px solid #000; text-align:center;'>2月</th>");
            stringBuilder.Append("<th style ='height:40px; width:6%;  border:1px solid #000; text-align:center;'>3月</th>");
            stringBuilder.Append("<th style ='height:40px; width:6%;  border:1px solid #000; text-align:center;'>4月</th>");
            stringBuilder.Append("<th style ='height:40px; width:6%;  border:1px solid #000; text-align:center;'>5月</th>");
            stringBuilder.Append("<th style ='height:40px; width:6%;  border:1px solid #000; text-align:center;'>6月</th>");
            stringBuilder.Append("<th style ='height:40px; width:6%;  border:1px solid #000; text-align:center;'>7月</th>");
            stringBuilder.Append("<th style ='height:40px; width:6%;  border:1px solid #000; text-align:center;'>8月</th>");
            stringBuilder.Append("<th style ='height:40px; width:6%;  border:1px solid #000; text-align:center;'>9月</th>");
            stringBuilder.Append("<th style ='height:40px; width:6%;  border:1px solid #000; text-align:center;'>10月</th>");
            stringBuilder.Append("<th style ='height:40px; width:6%;  border:1px solid #000; text-align:center;'>11月</th>");
            stringBuilder.Append("<th style ='height:40px; width:6%;  border:1px solid #000; text-align:center;'>12月</th></thead></table></div>");
            return stringBuilder.ToString();
        }
        /// <summary>
        /// 通过年份查找当年的产能
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public string GetYearQty(string year)
        { 
            var targetCapacities = RT.Service.Resolve<TargetCapacityController>().getYearTargetCapacity(year);
            StringBuilder stringBuilder = new StringBuilder();

            if (targetCapacities.Count > 1)
                stringBuilder.Append("<div style='overflow - y: scroll; height:100px;'><table style='border-collapse:collapse; table-layout: fixed;text-align:center; margin:auto;overflow-y:scroll; width:98%'><tbody>");
            else
                stringBuilder.Append("<div style='overflow - y: scroll; height:100px;padding-right:17px'><table style='border-collapse:collapse; table-layout: fixed;text-align:center; margin:auto;overflow-y:scroll; width:98%'><tbody>");

            outputCapacityInfos = RT.Service.Resolve<FactoryConfirmsController>().SumMonthLoad(Convert.ToInt32(year));
            var factoryList = outputCapacityInfos.GroupBy(p => new { p.FactoryId, p.FactoryName }).Distinct().Select(x => x.Key.FactoryId).ToList();
            foreach (var outputCapacityInfo in outputCapacityInfos)
            {
                var targetCapacitie = targetCapacities.Where(x => x.EnterpriseId == outputCapacityInfo.FactoryId).FirstOrDefault();
                if (targetCapacitie != null)
                {
                    switch (outputCapacityInfo.Month)
                    {
                        case 1:
                            outputCapacityInfo.CapacityQty = targetCapacitie.M1;
                            break;
                        case 2:
                            outputCapacityInfo.CapacityQty = targetCapacitie.M2;
                            break;
                        case 3:
                            outputCapacityInfo.CapacityQty = targetCapacitie.M3;
                            break;
                        case 4:
                            outputCapacityInfo.CapacityQty = targetCapacitie.M4;
                            break;
                        case 5:
                            outputCapacityInfo.CapacityQty = targetCapacitie.M5;
                            break;
                        case 6:
                            outputCapacityInfo.CapacityQty = targetCapacitie.M6;
                            break;
                        case 7:
                            outputCapacityInfo.CapacityQty = targetCapacitie.M7;
                            break;
                        case 8:
                            outputCapacityInfo.CapacityQty = targetCapacitie.M8;
                            break;
                        case 9:
                            outputCapacityInfo.CapacityQty = targetCapacitie.M9;
                            break;
                        case 10:
                            outputCapacityInfo.CapacityQty = targetCapacitie.M10;
                            break;
                        case 11:
                            outputCapacityInfo.CapacityQty = targetCapacitie.M11;
                            break;
                        case 12:
                            outputCapacityInfo.CapacityQty = targetCapacitie.M12;
                            break;
                    }
                }
            }
            foreach (var targetCapacitie in targetCapacities)
            {
                if (!factoryList.Contains(targetCapacitie.EnterpriseId))
                {
                    OutputCapacityInfo capacityInfo1 = new OutputCapacityInfo();
                    capacityInfo1.FactoryId = targetCapacitie.EnterpriseId;
                    capacityInfo1.FactoryName = targetCapacitie.EnterpriseName;
                    capacityInfo1.CapacityQty = targetCapacitie.M1;
                    outputCapacityInfos.Add(capacityInfo1);

                    OutputCapacityInfo capacityInfo2 = new OutputCapacityInfo();
                    capacityInfo2.FactoryId = targetCapacitie.EnterpriseId;
                    capacityInfo2.FactoryName = targetCapacitie.EnterpriseName;
                    capacityInfo2.CapacityQty = targetCapacitie.M2;
                    outputCapacityInfos.Add(capacityInfo2);

                    OutputCapacityInfo capacityInfo3 = new OutputCapacityInfo();
                    capacityInfo3.FactoryId = targetCapacitie.EnterpriseId;
                    capacityInfo3.FactoryName = targetCapacitie.EnterpriseName;
                    capacityInfo3.CapacityQty = targetCapacitie.M3;
                    outputCapacityInfos.Add(capacityInfo3);

                    OutputCapacityInfo capacityInfo4 = new OutputCapacityInfo();
                    capacityInfo4.FactoryId = targetCapacitie.EnterpriseId;
                    capacityInfo4.FactoryName = targetCapacitie.EnterpriseName;
                    capacityInfo4.CapacityQty = targetCapacitie.M4;
                    outputCapacityInfos.Add(capacityInfo4);

                    OutputCapacityInfo capacityInfo5 = new OutputCapacityInfo();
                    capacityInfo5.FactoryId = targetCapacitie.EnterpriseId;
                    capacityInfo5.FactoryName = targetCapacitie.EnterpriseName;
                    capacityInfo5.CapacityQty = targetCapacitie.M5;
                    outputCapacityInfos.Add(capacityInfo5);

                    OutputCapacityInfo capacityInfo6 = new OutputCapacityInfo();
                    capacityInfo6.FactoryId = targetCapacitie.EnterpriseId;
                    capacityInfo6.FactoryName = targetCapacitie.EnterpriseName;
                    capacityInfo6.CapacityQty = targetCapacitie.M6;
                    outputCapacityInfos.Add(capacityInfo6);

                    OutputCapacityInfo capacityInfo7 = new OutputCapacityInfo();
                    capacityInfo7.FactoryId = targetCapacitie.EnterpriseId;
                    capacityInfo7.FactoryName = targetCapacitie.EnterpriseName;
                    capacityInfo7.CapacityQty = targetCapacitie.M7;
                    outputCapacityInfos.Add(capacityInfo7);

                    OutputCapacityInfo capacityInfo8 = new OutputCapacityInfo();
                    capacityInfo8.FactoryId = targetCapacitie.EnterpriseId;
                    capacityInfo8.FactoryName = targetCapacitie.EnterpriseName;
                    capacityInfo8.CapacityQty = targetCapacitie.M8;
                    outputCapacityInfos.Add(capacityInfo8);

                    OutputCapacityInfo capacityInfo9 = new OutputCapacityInfo();
                    capacityInfo9.FactoryId = targetCapacitie.EnterpriseId;
                    capacityInfo9.FactoryName = targetCapacitie.EnterpriseName;
                    capacityInfo9.CapacityQty = targetCapacitie.M9;
                    outputCapacityInfos.Add(capacityInfo9);

                    OutputCapacityInfo capacityInfo10 = new OutputCapacityInfo();
                    capacityInfo10.FactoryId = targetCapacitie.EnterpriseId;
                    capacityInfo10.FactoryName = targetCapacitie.EnterpriseName;
                    capacityInfo10.CapacityQty = targetCapacitie.M10;
                    outputCapacityInfos.Add(capacityInfo10);

                    OutputCapacityInfo capacityInfo11 = new OutputCapacityInfo();
                    capacityInfo11.FactoryId = targetCapacitie.EnterpriseId;
                    capacityInfo11.FactoryName = targetCapacitie.EnterpriseName;
                    capacityInfo11.CapacityQty = targetCapacitie.M11;
                    outputCapacityInfos.Add(capacityInfo11);

                    OutputCapacityInfo capacityInfo12 = new OutputCapacityInfo();
                    capacityInfo12.FactoryId = targetCapacitie.EnterpriseId;
                    capacityInfo12.FactoryName = targetCapacitie.EnterpriseName;
                    capacityInfo12.CapacityQty = targetCapacitie.M12;
                    outputCapacityInfos.Add(capacityInfo12);
                }
            }

            if (outputCapacityInfos.Count > 0)
            {
                dicFacityMaxVaule = this.GetFacityMaxVaule(outputCapacityInfos);
                var groupFactoryList = outputCapacityInfos.GroupBy(x => x.FactoryId).OrderBy(p => p.Key).ToDictionary(p => p.Key);
                foreach (var i in groupFactoryList)
                {
                    decimal maxValue = 0;
                    if (dicFacityMaxVaule.ContainsKey(i.Key))
                    {
                        maxValue = dicFacityMaxVaule[i.Key];
                    }
                    stringBuilder.Append("<tr><td style ='height:145px; width:6%; text-align:center; border:1px solid #000;'> " + i.Value.FirstOrDefault().FactoryName + "</td>");
                    var values = i.Value.OrderBy(x => x.Month);
                    foreach (var j in values)
                    {
                        decimal proportion = 0, capacityQty = 0, loadQty = 0;
                        if (j.CapacityQty > 0 && j.LoadArea > 0)
                            proportion = Math.Round((j.LoadArea / j.CapacityQty) * 100, 2);
                        if (j.CapacityQty > 0 && maxValue > 0)
                            capacityQty = (j.CapacityQty / maxValue) * 100;
                        if (j.LoadArea > 0 && maxValue > 0)
                            loadQty = (j.LoadArea / maxValue) * 100;
                        stringBuilder.Append("<td  style ='height:145px; width:6%; text-align:center; border:1px solid #000;'>");

                        if (proportion > 100)
                        {
                            stringBuilder.Append("<div style ='height:15px; width:100%; font-size:12px;color:red'> " + proportion + "%</div>");
                        }
                        else
                        {
                            stringBuilder.Append("<div style ='height:15px; width:100%; font-size:12px;'> " + proportion + "%</div>");
                        }
                        stringBuilder.Append("<div style ='height:100px; width:100%;'>");
                        stringBuilder.Append("<div style ='width:50%; float:left;height:100px;display:flex;flex-wrap:wrap; align-content: flex-end;'><div style = 'width:54%; height:" + capacityQty + "%; background-color:coral; float:left; margin:5%  16% 5% 30%;'></div> </div>");
                        stringBuilder.Append("<div style ='width:50%; float:left;height:100px;display:flex;flex-wrap:wrap; align-content: flex-end;'><div style = 'width:54%; height:" + loadQty + "%; background-color:cornflowerblue; float:right; margin:5%  30% 5% 16%;'></div> </div>");
                        stringBuilder.Append("</div>");
                        stringBuilder.Append("<div style ='height:15px; width:100%; font-size:10px; margin:0 8%;  text-align:left;'> C:" + j.CapacityQty + "</div>");
                        stringBuilder.Append("<div style ='height:15px; width:100%; font-size:10px; margin:5% 8%; text-align:left;'> L:" + j.LoadArea + "</div>");
                        stringBuilder.Append("</td>");
                    }
                    stringBuilder.Append("</tr>");
                }
                stringBuilder.Append("</tbody></table></div>");
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 获取工厂最大值
        /// </summary>
        /// <param name="outputCapacityInfos">产能和负载数据列表</param>
        /// <returns>取工厂厂能或者负载中最大的值</returns>
        public Dictionary<double, decimal> GetFacityMaxVaule(List<OutputCapacityInfo> outputCapacityInfos)
        {
            Dictionary<double, decimal> dicFacityMaxValue = new Dictionary<double, decimal>();
            var outGroup = outputCapacityInfos.GroupBy(x => x.FactoryId);
            foreach (var item in outGroup)
            {
                decimal maxCapacityQtyValue = item.Max(x => x.CapacityQty);
                decimal maxLoadAreaValue = item.Max(x => x.LoadArea);
                decimal maxValue = maxCapacityQtyValue;
                if (maxLoadAreaValue > maxCapacityQtyValue)
                {
                    maxValue = maxLoadAreaValue;
                }
                dicFacityMaxValue.Add(item.Key, maxValue);
            }
            return dicFacityMaxValue;
        }
    }
}