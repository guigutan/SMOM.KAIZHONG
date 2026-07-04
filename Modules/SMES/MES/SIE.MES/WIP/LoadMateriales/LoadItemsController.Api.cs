using SIE.Api;
using SIE.Defects;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MES.LoadItems;
using SIE.MES.SingleLabels;
using SIE.MES.WIP.ApiModels;
using SIE.MES.WIP.Assemblys;
using SIE.MES.WIP.LoadMateriales.ApiModels;
using SIE.MES.WIP.Moves;
using SIE.MES.WorkOrders;
using SIE.Packages.Packings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.MES.WIP.LoadMateriales
{
    /// <summary>
    /// 上料API
    /// </summary>
    public class LoadItemsController : DomainController
    {
        /// <summary>
        /// 上料采集扫描
        /// </summary>
        /// <param name="loadItemQueryInfo"></param>
        /// <returns></returns>
        [ApiService("上料采集")]
        [return: ApiReturn("上料采集 ScanBarcode")]
        public virtual RstLoadMaterialesInfo LoadItemScanbarcode([ApiParameter("上料查询对象")] LoadItemQueryInfo loadItemQueryInfo)
        {
            return RT.Service.Resolve<LoadItemsService>().ScanBarcodeLoadItem(loadItemQueryInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loadItemQueryInfo"></param>
        /// <returns></returns>
        [ApiService("装配采集")]
        [return: ApiReturn("装配采集 ScanBarcode")]
        public virtual RstLoadMaterialesInfo ScanBarcodeAssembly([ApiParameter("上料查询对象")] LoadItemQueryInfo loadItemQueryInfo)
        {

            return RT.Service.Resolve<LoadItemsService>().ScanBarcodeAssembly(loadItemQueryInfo);

        }

        /// <summary>
        /// 切换工单
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        [ApiService("切换工单")]
        [return: ApiReturn("切换工单 ChangeWipResourceWorkOrder")]
        public virtual RstWipLoadItemInfo ChangeWipResourceWorkOrder([ApiParameter("上料查询对象")] LoadItemQueryInfo queryInfo)
        {
            if (!queryInfo.WorkOrderId.HasValue || queryInfo.WorkOrderId == -1)
            {
                throw new ValidationException("切换工单失败，请选择工单".L10N());
            }
            var workcell = new Workcell() { EmployeeId = queryInfo.EmployeeId, ResourceId = queryInfo.ResourceId, ProcessId = queryInfo.ProcessId, StationId = queryInfo.StationId };
            var wo = RT.Service.Resolve<WipController>().ChangeWipResourceWorkOrder(queryInfo.WorkOrderId.Value, workcell);
            var resultWo = RF.GetById<WorkOrder>(wo.Id, new EagerLoadOptions().LoadWithViewProperty());

            RstWipLoadItemInfo rstWipInfo = new RstWipLoadItemInfo()
            {
                Sn = queryInfo.Sn,
                WorkOrderNo = resultWo.No,
                ProductCode = resultWo.ProductCode,
                ProductName = resultWo.ProductName,
                ProductModel = resultWo.ProductModelName
            };
            //获取上料记录
            var loadItems = RT.Service.Resolve<LoadItemController>().GetLoadItemList(workcell.ResourceId, workcell.StationId);
            loadItems.ForEach(item =>
            {
                rstWipInfo.LoadItemInfos.Add(RT.Service.Resolve<LoadItemsService>().CreateLoadItem(item));
            });


            return rstWipInfo;
        }

        /// <summary>
        /// 正常下料
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <param name="loadItemId"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [ApiService("正常下料")]
        [return: ApiReturn("下料后剩余的上料")]

        public virtual RstWipLoadItemInfo UnloadItem([ApiParameter("查询对象")] LoadItemQueryInfo queryInfo, double loadItemId)
        {
            var workcell = new Workcell() { EmployeeId = queryInfo.EmployeeId, ResourceId = queryInfo.ResourceId, ProcessId = queryInfo.ProcessId, StationId = queryInfo.StationId };
            var loadItem = RF.GetById<LoadItem>(loadItemId);
            if (loadItem == null)
            {

                throw new ValidationException("上料记录不存在，请检查！".L10N());
            }
            RT.Service.Resolve<LoadItemController>().UnloadItem(loadItem.Id, loadItem.Qty);
            var loadItems = RT.Service.Resolve<LoadItemController>().GetLoadItemList(workcell.ResourceId, workcell.StationId);
            RstWipLoadItemInfo rstWipLoadItemInfo = new RstWipLoadItemInfo();
            loadItems.ForEach(item =>
            {
                rstWipLoadItemInfo.LoadItemInfos.Add(RT.Service.Resolve<LoadItemsService>().CreateLoadItem(item));
            });
            return rstWipLoadItemInfo;
        }
        /// <summary>
        /// 一键下料
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <returns></returns>
        [ApiService("一键下料")]
        [return: ApiReturn("返回下料结果")]
        public virtual RstWipLoadItemInfo UnloadAllItem([ApiParameter("查询对象")] LoadItemQueryInfo queryInfo)
        {
            var matchItems = RT.Service.Resolve<LoadItemController>().GetUnloadAllItems(queryInfo.ResourceId, queryInfo.StationId);
            if (!matchItems.Any())
            {
                throw new ValidationException("未找到上料记录".L10N());
            }
            RT.Service.Resolve<LoadItemController>().UnloadAllItem(matchItems.ToList());
            return new RstWipLoadItemInfo();
        }

        /// <summary>
        /// 不良下料
        /// </summary>
        /// <param name="defectDatas"></param>
        /// <param name="loadItemId"></param>
        /// <exception cref="ValidationException"></exception>
        [ApiService("不良下料")]
        [return: ApiReturn("返回下料结果")]
        public virtual void UnloadDefectItem([ApiParameter("缺陷Id")] List<DefectData> defectDatas, [ApiParameter("上料Id")] double loadItemId)
        {
            var loadItem = RF.GetById<LoadItem>(loadItemId);
            if (loadItem == null)
            {
                throw new ValidationException("未找到上料记录".L10N());
            }
            if (!defectDatas.Any())
            {
                throw new ValidationException("请选择缺陷".L10N());
            }
            defectDatas.ForEach(item =>
            {
                item.Qty = RT.Service.Resolve<ItemUnitController>().SetItemPrecisionValue(item.ItemId, item.Qty);
            });


            RT.Service.Resolve<LoadItemController>().UnloadItem(loadItemId, defectDatas);
        }
    }
}
