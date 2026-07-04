using Castle.Core.Internal;
using Microsoft.Scripting.Utils;
using SIE.Api;
using SIE.Common;
using SIE.Core.Barcodes;
using SIE.Core.Common.Service;
using SIE.Defects;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.Barcodes;
using SIE.EventMessages.MES.ProcessStatistics;
using SIE.Items;
using SIE.MES.LoadItems;
using SIE.MES.SingleLabels;
using SIE.MES.WIP.Assemblys;
using SIE.MES.WIP.LoadMateriales.ApiModels;
using SIE.MES.WIP.Models;
using SIE.MES.WIP.Moves;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.Runtime;
using SIE.MES.WIP.TaskExtensions;
using SIE.MES.WorkOrders;
using SIE.Packages.Packings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.MES.WIP.LoadMateriales
{
    /// <summary>
    /// 上料服务
    /// </summary>
    public class LoadItemsService : AssemblyController
    {
        /// <summary>
        /// 扫描上料条码
        /// </summary>
        /// <param name="loadItemQueryInfo"></param>
        /// <returns></returns>
        public virtual RstLoadMaterialesInfo ScanBarcodeLoadItem(LoadItemQueryInfo loadItemQueryInfo)
        {
            if (loadItemQueryInfo == null)
            {
                return null;
            }
            //校验工作单元是否正常
            RT.Service.Resolve<MoveController>().ValidateWipQueryInfo(loadItemQueryInfo);
            var workcell = new Workcell() { EmployeeId = loadItemQueryInfo.EmployeeId, ResourceId = loadItemQueryInfo.ResourceId, ProcessId = loadItemQueryInfo.ProcessId, StationId = loadItemQueryInfo.StationId };

            RstLoadMaterialesInfo rstLoadMaterialesInfo = new RstLoadMaterialesInfo();
            Dictionary<LoadItemSourceType, bool> dicLoadItemSourceType = new Dictionary<LoadItemSourceType, bool>
            {
                { LoadItemSourceType.SN, true }
            };
            if (!loadItemQueryInfo.WorkOrderId.HasValue)
            {
                throw new ValidationException("当前工位的在制工单为空，不允许上料，请先扫描条码或切换工单".L10N());
            }

            EntityList<LoadItems.LoadItemBarcodeInfo> loadItemBarcodeInfos = new EntityList<LoadItems.LoadItemBarcodeInfo>();
            var barcodeInfos = GetLoadBarcodeInfo(loadItemQueryInfo.Sn, workcell, dicLoadItemSourceType, loadItemQueryInfo.WorkOrderId, loadItemBarcodeInfos);
            if (loadItemQueryInfo.SelectedBarcodeInfoId <= 0)
            {
                if (barcodeInfos.Count > 1)
                {
                    rstLoadMaterialesInfo.BarcodeInfos = barcodeInfos;
                    rstLoadMaterialesInfo.IsNeetToSelectedBarcodeInfo = true;//标记要求选一条
                    return rstLoadMaterialesInfo;
                }
                else
                {//物料标签唯一
                    var barcodeInfo = loadItemBarcodeInfos.First();
                    LocalAddLoadItem(loadItemQueryInfo, workcell, barcodeInfo, rstLoadMaterialesInfo);

                }
            }
            else
            {
                var barcodeInfo = loadItemBarcodeInfos.First(m => m.Id == loadItemQueryInfo.SelectedBarcodeInfoId.ToString());
                LocalAddLoadItem(loadItemQueryInfo, workcell, barcodeInfo, rstLoadMaterialesInfo);
            }
            var collectQty = RT.Service.Resolve<IProcessStatistics>().GetCollectQty(workcell.ResourceId, workcell.ProcessId, workcell.StationId, loadItemQueryInfo.WorkOrderId.Value);
            rstLoadMaterialesInfo.CollectQty = collectQty;
            rstLoadMaterialesInfo.WorkOrderId = loadItemQueryInfo.WorkOrderId;
            rstLoadMaterialesInfo.Msg = "上料成功！".L10N();
            return rstLoadMaterialesInfo;
        }

        /// <summary>
        /// 本地添加上料
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <param name="workcell"></param>
        /// <param name="barcodeInfo"></param>
        /// <param name="rstLoadMaterialesInfo"></param>
        /// <exception cref="ValidationException"></exception>
        private void LocalAddLoadItem(LoadItemQueryInfo queryInfo, Workcell workcell,
            LoadItems.LoadItemBarcodeInfo barcodeInfo, RstLoadMaterialesInfo rstLoadMaterialesInfo)
        {
            string barcode = queryInfo.Sn;
            if (queryInfo.IsSnCollected)
            {
                if (queryInfo.AssemblyItemsDictionary.ContainsKey(barcode))
                {
                    throw new ValidationException("条码{0}已装配，请扫描其他物料条码"
                        .L10nFormat(barcode));
                }

                if (!queryInfo.AssemblyDetailList.Any(p => p.ItemId == barcodeInfo.ItemId)
                    && !queryInfo.AssemblyDetailList.SelectMany(x => x.AltItemList).Any(p => p.ItemId == barcodeInfo.ItemId))
                {
                    throw new ValidationException("物料[{0}]非装配需求物料，请扫描其他物料条码"
                        .L10nFormat(RF.GetById<Item>(barcodeInfo.ItemId)?.Code));
                }
                var wo = RF.GetById<WorkOrder>(queryInfo.WorkOrderId, new EagerLoadOptions().LoadWith(WorkOrder.ProcessBomListProperty)) ?? throw new ValidationException("当前工位不存在工单，请扫描条码后上料！".L10nFormat(RF.GetById<Item>(barcodeInfo.ItemId)?.Code));
                //扫描SN后再上料，匹配条码是否满足装配条件
                if (ValidationAssemblyProperty(wo, barcodeInfo.ItemId, barcodeInfo.ItemExtProp, workcell))
                {
                    var paramaBarcodeInfo = CreatBarcodeInfo(barcodeInfo);
                    AddAssemblyLabel(queryInfo.AssemblyDetailList, paramaBarcodeInfo, barcodeInfo.ItemId, barcodeInfo.Qty, queryInfo.AssemblyItemsDictionary);
                    rstLoadMaterialesInfo.AssemblyItemsDictionary = queryInfo.AssemblyItemsDictionary;

                    //SN不为空，直接扣料过站
                    AssemblyCollect(queryInfo, queryInfo.Sn, workcell, rstLoadMaterialesInfo);
                }
            }
            else
            {
                //单体、半成品不提前上料
                if (barcodeInfo.Type == LoadItemSourceType.SN)
                {
                    throw new ValidationException("半成品[{0}]不能提前上料".L10nFormat(barcodeInfo.Barcode));
                }

                //物料匹配 保存上料                 
                RT.Service.Resolve<LoadItemController>().NewLoadItem(barcodeInfo, workcell);
                var loadItems = RT.Service.Resolve<LoadItemController>().GetLoadItemList(workcell.ResourceId, workcell.StationId);
                loadItems.ForEach(item =>
                {
                    rstLoadMaterialesInfo.LoadItemInfos.Add(CreateLoadItem(item));
                });
            }
        }

        /// <summary>
        /// /装配采集
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <param name="barcode"></param>
        /// <param name="workcell"></param>
        /// <param name="rstLoadMaterialesInfo"></param>
        /// <exception cref="ValidationException"></exception>
        protected virtual void AssemblyCollect(LoadItemQueryInfo queryInfo, string barcode, Workcell workcell, RstLoadMaterialesInfo rstLoadMaterialesInfo)
        {
            var collectBarcode = new CollectBarcode { Code = barcode, Type = Core.Barcodes.BarcodeType.SN };
            if (!queryInfo.IsSnCollected)
            {
                var info = Validate(collectBarcode, workcell, rstLoadMaterialesInfo);
                queryInfo.WipProductProcessState = info.WipProductProcessState;
                if (!rstLoadMaterialesInfo.Msg.IsNullOrEmpty())//存在错误信息则跳出
                {
                    return;
                }
            }

            if (queryInfo.WipProductProcessState == SIE.MES.WIP.Products.WipProductProcessState.Finish
                && IsLackItem(queryInfo, barcode, workcell))
            {
                //当前过站状态非Start=> Move In，且验证上料不满足时，返回不提交
                return;
            }
            var result = Submit(queryInfo, workcell, collectBarcode, rstLoadMaterialesInfo);
            rstLoadMaterialesInfo.Msg = result;

        }

        /// <summary>
        /// 验证任务单
        /// </summary>
        /// <param name="collectBarcode"></param>
        /// <param name="workcell"></param>
        /// <param name="rstLoadMaterialesInfo"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        protected virtual ProductInfo Validate(CollectBarcode collectBarcode, Workcell workcell, RstLoadMaterialesInfo rstLoadMaterialesInfo)
        {
            int? reportModel = null;
            var product = RT.Service.Resolve<WipController>().Validate(collectBarcode, workcell);
            WorkOrder wo = null;
            if (product.WorkOrderId != rstLoadMaterialesInfo.WorkOrderId && product.WorkOrderId != 0)
            {
                var WorkOrder = RF.GetById<WorkOrder>(rstLoadMaterialesInfo.WorkOrderId);
                wo = RF.GetById<WorkOrder>(product.WorkOrderId);
                if (WorkOrder != null)
                {
                    rstLoadMaterialesInfo.Msg = "工单已切换,由[{0}]切换到[{1}]".L10nFormat(WorkOrder.No, wo.No);
                }
                RT.Service.Resolve<WipController>().ChangeWipResourceWorkOrder(wo.Id, workcell);
                RT.EventBus.Publish(new ChangeWipResourceWorkOrderEvent { WorkOrderId = product.WorkOrderId });
                reportModel = RT.Service.Resolve<IWipTaskReport>().GetWorkOrdeReportModel(wo.Id);
            }
            if (reportModel == -1)
            {
                reportModel = RT.Service.Resolve<IWipTaskReport>().GetWorkOrdeReportModel(wo.Id);
            }
            if (reportModel == null)
            {
                return product;
            }
            if (reportModel == 1)
            {
                throw new ValidationException("不允许采集，当前条码所属工单任务单报工方式为手动报工".L10N());
            }
            RT.Service.Resolve<IWipTaskReport>().ValidateAutoReport(wo.Id, workcell.EmployeeId, workcell.ProcessId);
            return product;
        }



        /// <summary>
        /// 检查是否满足过站条件
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <param name="barcode">条码</param>
        /// <param name="workcell">工作单元</param>
        /// <returns>true--未满足，false--满足</returns>
        public virtual bool IsLackItem(LoadItemQueryInfo queryInfo, string barcode, Workcell workcell)
        {
            if (!queryInfo.AssemblyDetailList.Any())
            {
                try
                {
                    RT.Service.Resolve<AssemblyController>()
                        .ValidateProcessBom(new CollectBarcode(barcode, Core.Barcodes.BarcodeType.SN), workcell);

                }
                catch (Exception exc)
                {
                    throw new ValidationException(exc.Message);
                }
            }
            else
            {
                //验证装配清单是否满足过站
                if (queryInfo.AssemblyDetailList.Any(p => p.ItemLabel == null || p.DemandQty - p.Qty > 0))
                {
                    throw new ValidationException("请扫描上料的物料标签".L10N());
                }
            }
            return false;
        }

        /// <summary>
        /// 提交装配采集
        /// </summary>
        /// <param name="queryInfo"></param>
        /// <param name="workcell"></param>
        /// <param name="collectBarcode"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        private string Submit(LoadItemQueryInfo queryInfo, Workcell workcell, CollectBarcode collectBarcode, RstLoadMaterialesInfo rstLoadMaterialesInfo)
        {
            //var barcodes = Step.Barcodes.ToArray();

            try
            {

                var collectData = new CollectData();

                collectData.State = queryInfo.WipProductProcessState;
                collectData.CollectBarcode = collectBarcode;
                collectBarcode.Code = queryInfo.Sn;
                Collect(new string[] { collectBarcode.Code }, collectData, workcell);

                if (queryInfo.WipProductProcessState == WipProductProcessState.Finish)
                {
                    return "[{0}]过站成功，请扫描条码".L10nFormat(collectBarcode);
                }
                else
                {
                    return ("[{0}]入站成功，请扫描条码".L10nFormat(collectBarcode));
                }

            }
            catch (Exception exc)
            {
                throw new ValidationException(exc.Message);
            }
            return "";
        }


        /// <summary>
        /// 创建上料物料
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual LoadItemInfo CreateLoadItem(LoadItem item)
        {
            return new LoadItemInfo()
            {
                Id = item.Id,
                Alter = item.Alter,
                AlterGroup = item.AlterGroup,
                IsMoveItem = item.IsMoveItem,
                ItemCode = item.ItemCode,
                ItemConsumeMode = item.ItemConsumeMode,
                ItemExtProp = item.ItemExtProp,
                ItemExtPropName = item.ItemExtPropName,
                ItemId = item.ItemId,
                ItemName = item.ItemName,
                LoadQty = item.LoadQty,
                NgQty = item.NgQty,
                Qty = item.Qty,
                ResourceName = item.Resource.Name,
                ResourceId = item.ResourceId,
                ShiftName = item.Shift.Name,
                ShiftId = item.ShiftId,
                SourceCode = item.SourceCode,
                SourceId = item.SourceId,
                SourceType = item.SourceType,
                StationId = item.StationId,
                StationName = item.StationName,
                UnloadQty = item.UnloadQty,
                WorkOrderId = item.WorkOrderId,
                WorkOrderNo = item.WorkOrder.No,
            };
        }

        /// <summary>
        /// 添加装配物料标签
        /// </summary>
        /// <param name="assemblyDetailList"></param>
        /// <param name="barcodeInfo"></param>
        /// <param name="itemId"></param>
        /// <param name="qty"></param>
        /// <param name="assemblyItemsDictionary"></param>
        /// <exception cref="ValidationException"></exception>
        private void AddAssemblyLabel(List<ApiModels.AssemblyDetailViewModel> assemblyDetailList, ApiModels.LoadItemBarcodeInfo barcodeInfo,
            double itemId, decimal qty, Dictionary<string, ApiModels.LoadItemBarcodeInfo> assemblyItemsDictionary
            )
        {
            bool useBarcode = false;
            var barcode = barcodeInfo.BillNo.IsNotEmpty() ? barcodeInfo.BillNo : barcodeInfo.Barcode;

            decimal remainQty = qty;

            foreach (var detail in assemblyDetailList)
            {
                if ((detail.ItemId == itemId
                        || detail.AltItemList.FirstOrDefault(p => p.ItemId == itemId) != null)
                    && (detail.DemandQty - detail.Qty) > 0)
                {
                    var lackQty = detail.DemandQty - detail.Qty;

                    if (remainQty >= lackQty)
                    {
                        detail.Qty = detail.DemandQty;
                        detail.RemainQty = remainQty - lackQty;
                    }
                    else
                    {
                        detail.Qty += remainQty;
                        detail.RemainQty = 0m;
                    }

                    detail.ItemLabel = string.Join(";", new string[] { detail.ItemLabel, barcode });

                    assemblyItemsDictionary.Add(barcode, barcodeInfo);
                    useBarcode = true;
                }
            }

            if (!useBarcode)
            {
                throw new ValidationException("物料[{0}]已满足装配，请扫描其他物料条码"
                            .L10nFormat(RF.GetById<Item>(itemId)?.Code));
            }
        }


        /// <summary>
        /// 扫描装配条码
        /// </summary>
        /// <param name="loadItemQueryInfo">上料查询器</param>
        /// <returns></returns>
        public virtual RstLoadMaterialesInfo ScanBarcodeAssembly(LoadItemQueryInfo loadItemQueryInfo)
        {
            RstLoadMaterialesInfo rstLoadMaterialesInfo = new RstLoadMaterialesInfo();
            RT.Service.Resolve<MoveController>().ValidateWipQueryInfo(loadItemQueryInfo);
            var workcell = new Workcell() { EmployeeId = loadItemQueryInfo.EmployeeId, ResourceId = loadItemQueryInfo.ResourceId, ProcessId = loadItemQueryInfo.ProcessId, StationId = loadItemQueryInfo.StationId };
            var collectBarcode = new CollectBarcode { Code = loadItemQueryInfo.Sn, Type = BarcodeType.SN };
            var product = RT.Service.Resolve<WipController>().Validate(collectBarcode, workcell);
            var loadItems = RT.Service.Resolve<LoadItemController>().GetLoadItemList(workcell.ResourceId, workcell.StationId);
            if (product == null)
            {
                throw new ValidationException("当前扫描条码未找到产品运行时".L10N());
            }
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                try
                {
                    AssemblyCollect(loadItemQueryInfo, loadItemQueryInfo.Sn, workcell, rstLoadMaterialesInfo);
                    if (!loadItemQueryInfo.WorkOrderId.HasValue)
                    {
                        loadItemQueryInfo.WorkOrderId = product.WorkOrderId;
                    }
                    var collectQty = RT.Service.Resolve<IProcessStatistics>().GetCollectQty(workcell.ResourceId, workcell.ProcessId, workcell.StationId, loadItemQueryInfo.WorkOrderId.Value);
                    rstLoadMaterialesInfo.CollectQty = collectQty;
                    rstLoadMaterialesInfo.IsOK = true;
                }
                catch (Exception ex)
                {
                    rstLoadMaterialesInfo.Msg = ex.Message;
                    rstLoadMaterialesInfo.IsOK = false;
                }
                finally
                {
                    rstLoadMaterialesInfo.LoadItemInfos.Clear();
                    loadItems.ForEach(item =>
                    {
                        rstLoadMaterialesInfo.LoadItemInfos.Add(RT.Service.Resolve<LoadItemsService>().CreateLoadItem(item));
                    });
                    GetAssemblyDetail(loadItemQueryInfo, rstLoadMaterialesInfo);
                    AssemblyBuckle(loadItemQueryInfo, rstLoadMaterialesInfo, loadItems);
                    var wo = RF.GetById<WorkOrder>(product.WorkOrderId, new EagerLoadOptions().LoadWithViewProperty());
                    rstLoadMaterialesInfo.WorkOrderId = product.WorkOrderId;
                    rstLoadMaterialesInfo.WorkOrderNo = wo.No;
                    rstLoadMaterialesInfo.ProduceCode = wo.ProductCode;
                    rstLoadMaterialesInfo.ProduceName = wo.ProductName;
                    rstLoadMaterialesInfo.ProduceModel = wo.ProductModelName;
                }
                tran.Complete();
            }
            return rstLoadMaterialesInfo;
        }

        /// <summary>
        /// 装配扣料后回写扣料信息
        /// </summary>
        /// <param name="loadItemQueryInfo"></param>
        /// <param name="rstInspValidateInfo"></param>
        /// <param name="loadItems"></param>
        private void AssemblyBuckle(LoadItemQueryInfo loadItemQueryInfo, RstLoadMaterialesInfo rstInspValidateInfo, EntityList<LoadItem> loadItems)
        {
            if (!loadItemQueryInfo.WorkOrderId.HasValue)
            {
                return;
            }
            string alterGroup = string.Empty;

            foreach (var detail in rstInspValidateInfo.AssemblyDetailViewModels)
            {
                //已经满足
                if (detail.DemandQty - detail.Qty <= 0)
                {
                    continue;
                }

                if (alterGroup.IsNullOrEmpty() || alterGroup == detail.AlterGroup)
                {
                    //匹配上料列表满足扣料的条码，添加到装配明细                
                    foreach (var loadItemEntity in loadItems.Where(p => p.WorkOrderId == loadItemQueryInfo.WorkOrderId
                            && p.ItemId == detail.ItemId
                            && p.ItemExtProp == detail.ItemExtProp
                            && p.Qty > 0))
                    {

                        var lackQty = detail.DemandQty - detail.Qty;
                        if (loadItemEntity.Qty >= lackQty)
                        {
                            detail.Qty = detail.DemandQty;
                            loadItemEntity.Qty = loadItemEntity.Qty - lackQty;
                        }
                        else
                        {
                            detail.Qty += loadItemEntity.Qty;
                            loadItemEntity.Qty = 0;
                        }

                        detail.ItemLabel = string.Join(";", new string[] { detail.ItemLabel, loadItemEntity.SourceCode });
                        detail.ItemLabel = detail.ItemLabel.TrimStart(';');
                        if (!detail.AlterGroup.IsNullOrEmpty())
                        {
                            alterGroup = detail.AlterGroup;
                        }

                        //已经满足
                        if (detail.DemandQty - detail.Qty <= 0)
                        {
                            break;
                        }
                    }
                }

                //已经满足
                if (detail.DemandQty - detail.Qty <= 0)
                {
                    continue;
                }

                alterGroup = UseAltItem(alterGroup, detail, loadItems, loadItemQueryInfo.WorkOrderId.Value);
            }
        }

        /// <summary>
        /// 使用替代料
        /// </summary>
        /// <param name="alterGroup">替代组分组</param>
        /// <param name="detail">主料的物料需求</param>
        /// <param name="loadItems">以上料数据</param>
        /// <param name="workOrderId"></param>
        /// <returns>替代组分组</returns>
        private string UseAltItem(string alterGroup, ApiModels.AssemblyDetailViewModel detail, EntityList<LoadItem> loadItems, double workOrderId)
        {
            foreach (var alt in detail.AltItemList)
            {
                if (alterGroup.IsNullOrEmpty() || alterGroup == alt.AlterGroup)
                {
                    //匹配上料列表满足扣料的条码，添加到装配明细                
                    foreach (var loadItemEntity in loadItems.Where(p => p.WorkOrderId == workOrderId
                        && p.ItemId == alt.ItemId
                        && p.ItemExtProp == alt.ItemExtProp
                        && p.Qty > 0))
                    {
                        var lackQty = detail.DemandQty - detail.Qty;
                        if (loadItemEntity.Qty >= lackQty)
                        {
                            detail.Qty = detail.DemandQty;
                            loadItemEntity.Qty = loadItemEntity.Qty - lackQty;
                        }
                        else
                        {
                            detail.Qty += loadItemEntity.Qty;
                            loadItemEntity.Qty = 0;
                        }

                        detail.ItemLabel = string.Join(";", new string[] { detail.ItemLabel, loadItemEntity.SourceCode });
                        detail.ItemLabel = detail.ItemLabel.TrimStart(';');
                        if (!alt.AlterGroup.IsNullOrEmpty())
                        {
                            alterGroup = alt.AlterGroup;
                        }

                        //已经满足
                        if (detail.DemandQty - detail.Qty <= 0)
                        {
                            break;
                        }
                    }
                }

                //已经满足
                if (detail.DemandQty - detail.Qty <= 0)
                {
                    break;
                }
            }

            return alterGroup;
        }

        /// <summary>
        /// 获取上料条码信息
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="workcell"></param>
        /// <param name="dicLoadItemSourceType"></param>
        /// <param name="workOrderId"></param>
        /// <param name="loadItemBarcodeInfos"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual List<ApiModels.LoadItemBarcodeInfo> GetLoadBarcodeInfo(string barcode, Workcell workcell,
            Dictionary<LoadItemSourceType, bool> dicLoadItemSourceType, double? workOrderId
            , EntityList<LoadItems.LoadItemBarcodeInfo> loadItemBarcodeInfos)
        {
            if (!workOrderId.HasValue)
            {
                throw new ValidationException("当前工位的在制工单为空，不允许上料，请先扫描条码或直接点击【切换在制工单】再上料！".L10N());
            }

            List<ApiModels.LoadItemBarcodeInfo> result = new List<ApiModels.LoadItemBarcodeInfo>();
            var barcodeInfos = RT.Service.Resolve<LoadItemController>()
                .ValidateLoadItem(barcode, workcell, dicLoadItemSourceType, workOrderId.Value);

            barcodeInfos.ForEach(item =>
            {
                loadItemBarcodeInfos.Add(item);//传出去后续用
                result.Add(CreatBarcodeInfo(item));
            });

            return result;

        }

        /// <summary>
        /// 条码信息从复杂类型转为简单类型
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private ApiModels.LoadItemBarcodeInfo CreatBarcodeInfo(LoadItems.LoadItemBarcodeInfo item)
        {
            return new ApiModels.LoadItemBarcodeInfo()
            {
                Id = item.Id,
                Barcode = item.Barcode,
                BillNo = item.BillNo,
                ConsumeMode = item.ConsumeMode,
                IsSerialNumber = item.IsSerialNumber,
                ItemCode = item.ItemCode,
                ItemExtProp = item.ItemExtProp,
                ItemExtPropName = item.ItemExtPropName,
                ItemId = item.ItemId,
                ItemName = item.ItemName,
                Label = item.Label,
                LotNo = item.LotNo,
                Qty = item.Qty,
                SourceId = item.SourceId,
                StorageLocationCode = item.StorageLocationCode,
                Type = item.Type,
                WarehouseName = item.WarehouseName,
                WipWorkOrderId = item.WipWorkOrderId,
                WipWorkOrderNo = item.WipWorkOrderNo,
            };
        }


        /// <summary>
        /// 验证装配属性
        /// </summary>
        /// <param name="workOrder"></param>
        /// <param name="itemId"></param>
        /// <param name="itemExtProp"></param>
        /// <param name="workcell"></param>
        /// <returns></returns>
        private bool ValidationAssemblyProperty(WorkOrder workOrder, double itemId, string itemExtProp, Workcell workcell)
        {
            if (workOrder.ProcessBomList
                     .Any(p => p.ItemId == itemId && p.ProcessId == workcell.ProcessId && p.ItemExtProp == itemExtProp))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取装配清单
        /// </summary>
        /// <param name="loadItemQueryInfo"></param>
        /// <param name="rstInspValidateInfo"></param>
        private void GetAssemblyDetail(LoadItemQueryInfo loadItemQueryInfo, RstLoadMaterialesInfo rstInspValidateInfo)
        {
            loadItemQueryInfo.AssemblyDetailList.Clear();
            loadItemQueryInfo.AssemblyItemsDictionary.Clear();
            var workcell = new Workcell() { EmployeeId = loadItemQueryInfo.EmployeeId, ResourceId = loadItemQueryInfo.ResourceId, ProcessId = loadItemQueryInfo.ProcessId, StationId = loadItemQueryInfo.StationId };
            if (workcell == null)
            {
                return;
            }
            if (loadItemQueryInfo.Sn.IsNullOrEmpty())
            {
                throw new ValidationException("请扫描装配条码".L10N());
            }

            //查找【生产采集运行时产品】
            var product = RT.Service.Resolve<RuntimeController>().FindProduct(loadItemQueryInfo.Sn, BarcodeType.SN);
            if (product != null)
            {
                //查找【生产采集运行时产品】后工序列表中与当前选择工序匹配的【运行时工序】
                var process = product.Routing.GetNext()
                    .FirstOrDefault(p => p.ProcessId == workcell.ProcessId);

                if (process != null)
                {
                    process.Boms.Where(p => p.IsBuckleMaterial).ForEach(p =>
                    {
                        var detail = new AssemblyDetailViewModel()
                        {
                            ItemId = p.ItemId,
                            DemandQty = p.Qty,
                            ItemExtProp = p.ItemExtProp,
                            AlterGroup = p.AlterGroup
                        };
                        p.AltBom.ForEach(f =>
                        {
                            detail.AltItemList.Add(new AltItemViewModel
                            {
                                ItemId = f.ItemId,
                                ItemExtProp = f.ItemExtProp,
                                AlterGroup = f.AlterGroup,
                            });
                        });

                        rstInspValidateInfo.AssemblyDetailViewModels.Add(CreateAssembly(detail));
                    });

                    return;
                }
            }
            var workOrder = RF.GetById<WorkOrder>(loadItemQueryInfo.WorkOrderId, new EagerLoadOptions().LoadWith(WorkOrder.ProcessBomListProperty));
            if (workOrder == null)
            {
                return;
            }

            //直接用工单的工序BOM
            var boms = workOrder.ProcessBomList
                .Where(p => p.ProcessId == workcell.ProcessId && !p.IsAlternative).ToList();

            boms.ForEach(processBom =>
            {
                //主料
                var detail = new AssemblyDetailViewModel()
                {
                    ItemId = processBom.ItemId,
                    DemandQty = processBom.SingleQty,
                    ItemExtProp = processBom.ItemExtProp,
                    Id = processBom.Id.ToString()
                };

                //替代料
                if (!processBom.Alter.IsNullOrEmpty())
                {
                    var processBomsOfAlter = workOrder.ProcessBomList
                        .Where(x => x.ProcessId == workcell.ProcessId && x.IsAlternative && x.Alter == processBom.Alter)
                        .ToList();

                    processBomsOfAlter.ForEach(f =>
                    {
                        detail.AltItemList.Add(new AltItemViewModel
                        {
                            ItemId = f.ItemId,
                            ItemExtProp = f.ItemExtProp,
                            AssemblyDetailId = f.Id
                        });
                    });
                }
                rstInspValidateInfo.AssemblyDetailViewModels.Add(CreateAssembly(detail));
            });
        }

        /// <summary>
        /// 创建装配集合
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        private ApiModels.AssemblyDetailViewModel CreateAssembly(AssemblyDetailViewModel detail)
        {
            var apiModel = new ApiModels.AssemblyDetailViewModel()
            {
                AlterGroup = detail.AlterGroup,
                DemandQty = detail.DemandQty,
                ItemExtProp = detail.ItemExtProp,
                ItemCode = detail.Item.Code,
                ItemId = detail.Item.Id,
                ItemLabel = detail.ItemLabel.TrimStart(';'),
                Qty = detail.Qty,
                RemainQty = detail.RemainQty,
                AltItemList = new List<ApiModels.AltItemViewModel>()
            };
            if (detail.AltItemList.Any())
            {
                detail.AltItemList.ForEach(item =>
                {
                    var apiAltDetail = new ApiModels.AltItemViewModel()
                    {
                        AlterGroup = item.AlterGroup,
                        ItemId = item.ItemId,
                        ItemExtProp = item.ItemExtProp,
                    };
                    apiModel.AltItemList.Add(apiAltDetail);
                });
            }
            return apiModel;
        }
    }
}
