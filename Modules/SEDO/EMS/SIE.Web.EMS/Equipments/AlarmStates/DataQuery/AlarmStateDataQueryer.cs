using SIE.Equipments.SMDC.Equipments.Infos;
using SIE.SMDC;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.Equipments.AlarmStates.DataQuery
{
    /// <summary>
    /// 查询
    /// </summary>
    public class AlarmStateDataQueryer : DataQueryer
    {
        /// <summary>
        /// 查询Tag历史值
        /// </summary>        
        /// <returns> </returns>
        public EchartInfoModel GetHistoryTagValue(List<string> tags, DateTime dateTimeBegin, DateTime dateTimeEnd)
        {
            //List<string> tags = new List<string>();

            //tags.Add("IO.Simulator1.IOTag");
            //tags.Add("IO.Simulator1.IOTag1");
            ////tags.Add("IO.Simulator1.IOTag2");
            ////tags.Add("IO.Simulator1.IOTag3");
            ////tags.Add("IO.Simulator1.IOTag4");
            ////tags.Add("IO.Simulator1.IOTag5");
            ////tags.Add("IO.Simulator1.IOTag6");
            ////tags.Add("IO.Simulator1.IOTag7");
            ////tags.Add("IO.Simulator1.IOTag8");
            ////tags.Add("IO.Simulator1.IOTag9");
            //DateTime dateTimeBegin = DateTime.Parse("2022-04-20 14:30:00");
            //DateTime dateTimeEnd = DateTime.Parse("2022-04-20 14:40:00");

            var tagHistoryValues = RT.Service.Resolve<EquipmentSmdcController>().GetHistoryTagValue(tags, dateTimeBegin, dateTimeEnd);

            var tagHistoryValuesDictionary = new Dictionary<string, Dictionary<DateTime, TagHistoryValue>>();

            foreach (var tagHistory in tagHistoryValues)
            {
                var fullTagName = tagHistory.ParentPath + "." + tagHistory.Name;
                if (!tagHistoryValuesDictionary.ContainsKey(fullTagName))
                {
                    tagHistoryValuesDictionary.Add(fullTagName, new Dictionary<DateTime, TagHistoryValue>());
                }

                var tagHistoryValuesOfTagDictionary = tagHistoryValuesDictionary[fullTagName];

                if (!tagHistoryValuesOfTagDictionary.ContainsKey(tagHistory.Time))
                {
                    tagHistoryValuesOfTagDictionary.Add(tagHistory.Time, tagHistory);
                }
            }

            EchartInfoModel echartInfoModel = new EchartInfoModel();

            var times = tagHistoryValues.Select(x => x.Time)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            echartInfoModel.XaxisList = new List<DateTime>();
            foreach (var time in times)
            {
                echartInfoModel.XaxisList.Add(time);
            }

            echartInfoModel.ChartSeries = new List<ChartSerie>();

            foreach (var tag in tags)
            {
                ChartSerie chartSerie = new ChartSerie();
                chartSerie.Type = "line";
                chartSerie.Name = tag;
                chartSerie.DataValues = new List<decimal>();

                foreach (var time in times)
                {
                    decimal tagHistoryValue = 0;

                    if (tagHistoryValuesDictionary.ContainsKey(tag))
                    {
                        var tagHistoryValuesOfTagDictionary = tagHistoryValuesDictionary[tag];

                        if (tagHistoryValuesOfTagDictionary.ContainsKey(time))
                        {
                            var tagHistory = tagHistoryValuesOfTagDictionary[time];
                            tagHistoryValue = tagHistory.Value;
                        }
                    }
                    
                    chartSerie.DataValues.Add(tagHistoryValue);
                }

                echartInfoModel.ChartSeries.Add(chartSerie);
            }

            return echartInfoModel;
        }
    }
}
