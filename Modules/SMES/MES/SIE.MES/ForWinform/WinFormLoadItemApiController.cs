using SIE.Api;
using SIE.Core.Barcodes;
using SIE.Defects;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MES.LoadItems;
using SIE.MES.SingleLabels;
using SIE.MES.WIP;
using SIE.MES.WIP.Assemblys;
using SIE.MES.WIP.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.ForWinform
{
    /// <summary>
    /// 上料采集API
    /// </summary>
    internal class WinFormLoadItemApiController : AssemblyController
    {
        /// <summary>
        /// 验证上料数据
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="workcell"></param>
        /// <param name="dicLoadItemSourceType"></param>
        /// <param name="toLoadItemWorkOrderId"></param>
        /// <returns></returns>
        [ApiService("验证上料数据")]
        [return: ApiReturn("验证条码是否可以上料 返回验证")]
        public virtual List<LoadItemBarcodeInfo> ValidateLoadItem([ApiParameter("上料条码")] string barcode, [ApiParameter("工作单元")] WIP.Workcell workcell,
             [ApiParameter("上料条码来源字典")] Dictionary<LoadItemSourceType, bool> dicLoadItemSourceType, [ApiParameter("待上料工单Id")] double toLoadItemWorkOrderId)
        {

            var result = RT.Service.Resolve<LoadItemController>()
                   .ValidateLoadItem(barcode, workcell, dicLoadItemSourceType, toLoadItemWorkOrderId);
            return result.ToList();
        }

        /// <summary>
        /// 获取物料信息
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [ApiService("获取物料信息")]
        [return: ApiReturn("获取物料信息 返回物料信息")]
        public virtual Item GetItemInfo([ApiParameter("物料Id")] double itemId)
        {
            return RF.GetById<Item>(itemId, new EagerLoadOptions().LoadWithViewProperty());
        }
        /// <summary>
        /// 验证工序BOM是否够扣料
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="workcell"></param>
        [ApiService("验证工序BOM是否够扣料")]
        [return: ApiReturn("验证工序BOM")]
        public virtual void ValidateProcessBomApi([ApiParameter("采集条码")] CollectBarcode barcode, [ApiParameter("工作单元")] WIP.Workcell workcell)
        {
            RT.Service.Resolve<AssemblyController>().ValidateProcessBom(barcode, workcell);
        }

        /// <summary>
        ///查找产品
        /// </summary>
        [ApiService("查找产品")]
        [return: ApiReturn("根据条码和类型获取运行时产品")]
        public virtual product FindProduct([ApiParameter("采集条码")] string barcode, [ApiParameter("采集条码类型")] BarcodeType barcodeType)
        {
            return RT.Service.Resolve<RuntimeController>().FindProduct(barcode, barcodeType);
        }

        [ApiService("查找产品")]
        [return: ApiReturn("根据条码和类型获取运行时产品工序")]
        public virtual process GetProductRoutingGetNext([ApiParameter("采集条码")] string barcode, [ApiParameter("采集条码类型")] BarcodeType barcodeType, [ApiParameter("工序Id")] double processId)
        {
            var product= RT.Service.Resolve<RuntimeController>().FindProduct(barcode, barcodeType);
            if (product != null)
            {
              return  product.Routing.GetNext().FirstOrDefault(p => p.ProcessId == processId);
            }
            return null;
        }


        [ApiService("获取上料列表")]
        [return: ApiReturn("获取上料记录列表")]
        public virtual List<LoadItem> GetLoadItemList([ApiParameter("资源Id")]  double resourceId, [ApiParameter("工位Id")] double stationId)
        {
            return RT.Service.Resolve<LoadItemController>().GetLoadItemList(resourceId, stationId).ToList();
        }
        [ApiService("上料")]
        [return: ApiReturn("执行上料")]
        public virtual void NewLoadItem([ApiParameter("换料条码信息")]  LoadItemBarcodeInfo barcodeInfo, [ApiParameter("工作单元")] WIP.Workcell workcell, [ApiParameter("是否验证当前工序BOM")] bool validateCurrentProcessBom)
        {
            RT.Service.Resolve<LoadItemController>().NewLoadItem(barcodeInfo, workcell, validateCurrentProcessBom);
        }

        [ApiService("获取下料明细")]
        [return: ApiReturn("获取下料明细")]
        public virtual List<UnloadItem> GetUnloadItemList([ApiParameter("工序Id")] double processId, [ApiParameter("资源Id")] double resourceId, [ApiParameter("工位Id")] double stationId)
        {
           return RT.Service.Resolve<LoadItemController>().GetUnloadItemList(processId, resourceId, stationId).ToList();
        }

        [ApiService("正常下料")]
        [return: ApiReturn("正常下料")]
        public virtual void UnloadItem([ApiParameter("上料Id")] double loadItemId, [ApiParameter("下料数量")] decimal qty)
        {
            RT.Service.Resolve<LoadItemController>().UnloadItem(loadItemId, qty);
        }

        /// <summary>
        /// 不良下料
        /// </summary>
        /// <param name="loadItemId"></param>
        /// <param name="defects"></param>
        [ApiService("不良下料")]
        [return: ApiReturn("不良下料")]
        public virtual void DefectUnloadItem([ApiParameter("上料Id")] double loadItemId, [ApiParameter("缺陷数据")] List<DefectData> defects)
        {
            RT.Service.Resolve<LoadItemController>().UnloadItem(loadItemId, defects);
        }


        /// <summary>
        /// 一键下料
        /// </summary>
        /// <param name="loadItemIds"></param>
        /// <exception cref="ValidationException"></exception>
        [ApiService("一键下料")]
        [return: ApiReturn("一键下料")]
        public virtual void UnloadAllItem([ApiParameter("上料Id")]  List<double> loadItemIds)
        {
            if (loadItemIds == null)
            {
                throw new ValidationException("请至少选择一条上料记录下料".L10N());
            }

            var loadItemList = Query<LoadItem>().Where(m => loadItemIds.Contains(m.Id)).ToList();

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                loadItemList.ForEach(e => UnloadItem(e.Id, e.Qty));
                tran.Complete();
            }
        }

    }
}
