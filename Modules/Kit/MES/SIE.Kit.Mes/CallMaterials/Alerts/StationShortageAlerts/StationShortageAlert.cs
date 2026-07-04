using MimeKit;
using SIE.Common.Alert;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Kit.MES.Stations;
using SIE.MES.LoadItems;
using SIE.Resources.WipResources;
using SIE.Tech.Stations;
using System;
using System.Linq;

namespace SIE.Kit.MES.CallMaterials.Alerts
{
    /// <summary>
    /// 工位缺料预警插件类
    /// </summary>
    [RootEntity, Serializable]
    [Alert("工位缺料预警插件", typeof(StationShortageAlertConfig), typeof(StationShortageAlertResult), "工位缺料预警插件")]
    public class StationShortageAlert : CallMaterialAlert
    {
        /// <summary>
        /// 执行预警逻辑
        /// </summary>
        /// <returns>预警参数</returns>
        public override AlertResultBase Run()
        {
            var stationShortageAlertResults = GetAlertResults();
            return stationShortageAlertResults;
        }

        /// <summary>
        /// 获取工位物料预警信息集合
        /// </summary>
        /// <returns>工位缺料预警参数集合</returns>
        public AlertResultListBase GetAlertResults()
        {
            var stationShortageAlertResults = new StationShortageAlertResultList();
            double lineId = (this.Context.Config as StationShortageAlertConfig).LineId;
            var stations = RT.Service.Resolve<StationController>().GetStations(lineId);
            var loadItems = RT.Service.Resolve<LoadItemController>().GetLoadItems(lineId);
            foreach (var station in stations)
            {
                var curStationItemList = station.GetStationItemList();
                if (curStationItemList.Count <= 0)
                    continue;

                StationShortageAlertResult curStationShortageAlertResult = new StationShortageAlertResult();
                curStationShortageAlertResult.Line = RF.GetById<WipResource>(lineId)?.Name;
                curStationShortageAlertResult.StationCode = station.Code;
                foreach (var stationItem in curStationItemList)
                {
                    var curLoadItems = loadItems.Where(x => x.StationId == station.Id && x.ItemId == stationItem.ItemId
                    && x.WorkOrder?.State == WorkOrderState.Producing && x.WorkOrder?.IsPause == YesNo.No).ToList();
                    var defaultLoadItem = curLoadItems?.FirstOrDefault();
                    if (defaultLoadItem == null)
                        continue;
                    decimal curLoadItemsQty = curLoadItems.Sum(x => x.Qty);
                    var curMaterialAlertItem = new MaterialAlert(defaultLoadItem.Item.Code, defaultLoadItem.Item.Name,
                        curLoadItemsQty / stationItem.Warning, defaultLoadItem?.WorkOrder.No);
                    curStationShortageAlertResult.MaterialAlerts.Add(curMaterialAlertItem);
                }

                if (curStationShortageAlertResult.MaterialAlerts.Count > 0)
                {
                    ProcessStationShortageAlertResult(curStationShortageAlertResult);
                    stationShortageAlertResults.ResultList.Add(curStationShortageAlertResult);
                }
            }

            return stationShortageAlertResults;
        }

        /// <summary>
        /// 获取当前工位物料列表中缺料最验证的物料信息
        /// </summary>
        /// <param name="curStationShortageAlertResult">工位缺料预警参数</param>
        private void ProcessStationShortageAlertResult(StationShortageAlertResult curStationShortageAlertResult)
        {
            var minMaterialAlert = curStationShortageAlertResult.MaterialAlerts.OrderBy(x => x.MaterialAlertValue).FirstOrDefault();
            curStationShortageAlertResult.WorkOrderNO = minMaterialAlert.MaterialWONo;
            curStationShortageAlertResult.Value = minMaterialAlert.MaterialAlertValue;
            curStationShortageAlertResult.AlertMaterialCode = minMaterialAlert.MaterialCode;
            curStationShortageAlertResult.AlertMaterialName = minMaterialAlert.MaterialName;
        }

        /// <summary>
        /// 预警结果处理
        /// </summary>
        /// <param name="alretResult">预警参数</param>
        /// <returns>bool</returns>
        public override bool AlertResultProcess(AlertResultBase alretResult)
        {
            return base.AlertResultProcess(alretResult);
        }

        /// <summary>
        /// 创建邮件附件
        /// </summary>
        /// <param name="result">预警参数</param>
        /// <returns>邮件附件</returns>
        public override AttachmentCollection CreateEmailAttachments(AlertResultBase result)
        {
            return base.CreateEmailAttachments(result);
        }
    }
}
