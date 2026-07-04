using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting.Utils;
using SIE.Barcodes;
using SIE.Barcodes.Panels;
using SIE.Common;
using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Core;
using SIE.Core.Barcodes;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.Panels;
using SIE.EventMessages.MES.WIP;
using SIE.MES.BarcodeProcesses;
using SIE.MES.LoadItems;
using SIE.MES.PanelBindings;
using SIE.MES.WIP.Models;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.Runtime;
using SIE.MES.WorkOrders;
using SIE.Packages.Boxs;
using SIE.Packages.Boxs.Configs;
using SIE.Packages.ItemLabels;
using SIE.Packages.ItemLabels.Configs;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using SIE.Tech.VictoryStandards;
using SIE.Utils;
using SIE.Warehouses.ItemStockData;
using System;
using System.Collections.Generic;
using System.Linq;
using Barcode = SIE.Barcodes.Barcode;

namespace SIE.MES.WIP
{
    /// <summary>
    /// WIP控制器
    /// </summary>
    [RouteName("collect")]
    public partial class WipController : DomainController, IWipController
    {
        private const string NOT_EXISTS_FORMAT = "[{0}]不存在";
        private const string ORDER_NOT_FOUND_FORMAT = "找不到[{0}]对应的工单";
        private const string IS_SCRAPED_FORMAT = "[{0}]已经报废";
        private const string IS_PENDING_FORMAT = "[{0}]已经挂起";


        /// <summary>
        /// 维修/返工下料
        /// </summary>
        /// <param name="sourceItemLabelId">来源物料标签Id</param>
        /// <param name="qty">库存变更数量</param>
        /// <param name="warehouseId">下料后指定仓库</param>
        /// <param name="storageLocationId">下料后指定库位</param>
        /// <param name="isNg">是否合格</param>
        /// <param name="workOrderId">工单Id</param>
        /// <returns></returns>

        public virtual string RepairReWorkUnloadItem(double sourceItemLabelId, decimal qty, double warehouseId, double storageLocationId, bool isNg, double workOrderId)
        {
            var newItemlabelCode = string.Empty;
            var itemLabel = RF.GetById<ItemLabel>(sourceItemLabelId);
            if (itemLabel != null)
            {

                //判断物料是否是序列号管理
                var isSerialNumber = RT.Service.Resolve<ItemStockBaseController>().CheckItemIsSer(itemLabel.ItemId);
                // 查询物料推式
                var consumeMode = GetItemConsumeMode(itemLabel.ItemId);
                if (!isSerialNumber)
                {
                    //判断是否存在相同标签 相同仓库 库位的记录 存在则累计数量
                    var exsitedLable = RT.Service.Resolve<ItemLabelController>().GetExsitedItemlable(itemLabel.Label, warehouseId, storageLocationId);
                    if (exsitedLable != null)
                    {
                        exsitedLable.NgQty += isNg ? qty : 0;
                        exsitedLable.Qty += isNg ? 0 : qty;
                        //var existedLabelWo = RT.Service.Resolve<ItemLabelController>().GetExsitedItemWorkOrder(exsitedLable.Id, workOrderId);
                        //if (consumeMode == Items.ConsumeMode.Push && !isNg)
                        //{
                        //    if (existedLabelWo != null)
                        //    {
                        //        existedLabelWo.Qty += qty;
                        //        RF.Save(existedLabelWo);
                        //    }
                        //    else
                        //    {
                        //        var newItemWo = RT.Service.Resolve<ItemLabelController>().CreateNewItemLabelWorkOrder(exsitedLable.Id, workOrderId, qty);
                        //        RF.Save(newItemWo);
                        //    }
                        //}
                        RF.Save(exsitedLable);
                    }
                    else//不存在指定仓库和库位的数据 则创建新的数据
                    {
                        ItemLabel newWarehouseItemLabel = new ItemLabel();
                        newWarehouseItemLabel.Clone(itemLabel);
                        newWarehouseItemLabel.StorageLocationId = storageLocationId;
                        newWarehouseItemLabel.WarehouseId = warehouseId;
                        newWarehouseItemLabel.Qty = isNg ? 0 : qty;
                        newWarehouseItemLabel.InitialQty = newWarehouseItemLabel.Qty;
                        newWarehouseItemLabel.NgQty = isNg ? qty : 0;
                        newWarehouseItemLabel.GenerateId();
                        newWarehouseItemLabel.Lot = itemLabel.Lot;
                        RF.Save(newWarehouseItemLabel);
                        //if (consumeMode == Items.ConsumeMode.Push && !isNg)
                        //{
                        //    var newItemWo = RT.Service.Resolve<ItemLabelController>().CreateNewItemLabelWorkOrder(newWarehouseItemLabel.Id, workOrderId, qty);
                        //    RF.Save(newItemWo);
                        //}
                    }
                    //调用WMS 非序列号下料 增加库存
                    itemLabel.StorageLocationId = storageLocationId;
                    itemLabel.WarehouseId = warehouseId;

                    //调用WMS下料接口，增加物料标签所在库位的合格或不合格库存（标签的库位为空时，不用调用此接口）
                    RT.Service.Resolve<LoadItemController>().UpdateWmsOnhandWhenUnlaodItem(
                        itemLabel, isNg, qty, workOrderId);
                }
                else//序列号 返工 维修下料 不管合格不合格都生产新的标签
                {
                    var config = ConfigService.GetConfig(new ItemLabelNoConfig(), typeof(ItemLabel));
                    if (config == null || config.BacodeRule == null)
                    {
                        throw new ValidationException("未找到物料标签配置规则，请检查规则配置".L10N());
                    }
                    var itemLabelNo = RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.NumberRuleId.Value, 1).First();
                    //生成新的物料标签
                    var newItemLabel = RT.Service.Resolve<ItemLabelController>().CreateItemLabel(itemLabel.Item, 0,
                        itemLabelNo, LabelSource.Wip, itemLabel.WorkOrderId, itemLabel.FactoryId, itemLabel.ItemExtProp, itemLabel.ItemExtPropName, itemLabel.ProjectNo);
                    newItemLabel.NgQty = isNg ? qty : 0;
                    newItemLabel.Qty = isNg ? 0 : qty;
                    if (newItemLabel.InitialQty == null || newItemLabel.InitialQty == 0)
                        newItemLabel.InitialQty = newItemLabel.Qty;
                    newItemLabel.WarehouseId = warehouseId;
                    newItemLabel.StorageLocationId = storageLocationId;
                    newItemLabel.Lot = itemLabel.Lot;//保持旧批次的条码
                    RF.Save(newItemLabel);
                    //if (consumeMode == Items.ConsumeMode.Push && !isNg)
                    //{
                    //    var newItemWo = RT.Service.Resolve<ItemLabelController>().CreateNewItemLabelWorkOrder(newItemLabel.Id, workOrderId, newItemLabel.Qty);
                    //    RF.Save(newItemWo);
                    //}
                    //调用WMS下料接口，增加物料标签所在库位的合格或不合格库存（标签的库位为空时，不用调用此接口） 序列号下料返修需要传true
                    RT.Service.Resolve<LoadItemController>().UpdateWmsOnhandWhenUnlaodItem(
                        newItemLabel, isNg, qty, workOrderId, true);
                    newItemlabelCode = itemLabelNo;
                }
            }
            return newItemlabelCode;
        }

        private Items.ConsumeMode GetItemConsumeMode(double itemId)
        {
            return Query<Items.Item>().Where(p => p.Id == itemId).Select(p => p.ConsumeMode).FirstOrDefault<Items.ConsumeMode>();
        }

        /// <summary>
        /// 验证：
        /// 1.产品工艺路线。
        /// 2.工单状态。
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="workcell">工作单元</param>
        /// <returns>返回产品信息</returns> 
        /// <exception cref="ArgumentNullException">采集条码为空、工作单元为空</exception>
        public virtual ProductInfo Validate(CollectBarcode barcode, Workcell workcell)
        {
            if (barcode == null)
            {
                throw new ArgumentNullException(nameof(barcode));
            }
            if (workcell == null)
            {
                throw new ArgumentNullException(nameof(workcell));
            }
            ValidateBarcode(barcode, workcell);
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var snInfo = GetMoveSns(barcode);
                List<string> sns = snInfo.Item1;
                product product = null;
                List<SnData> snDatas = new List<SnData>();

                product = ValidateProduct(barcode, workcell);
                ValidateWorkOrder(product);
                foreach (var sn in sns)
                {
                    snDatas.Add(new SnData() { Sn = sn, Qty = product.Qty });
                }

                WipProductProcessState wipProductProcessState = WipProductProcessState.Finish;

                if (product != null)
                {
                    wipProductProcessState = product.GetNextWipProductProcessState(workcell.ProcessId);
                }

                ProductInfo result = InitResult(barcode, workcell, snInfo, product, snDatas);
                if (product?.Routing.Current != null)
                {
                    result.LastResultType = product.Routing.Current.Result;
                }
                result.WipProductProcessState = wipProductProcessState;

                OnValidateFinish(barcode, workcell, product, result);
                tran.Complete();
                return result;
            }
        }

        /// <summary>
        /// 设置条码验证返回结果
        /// </summary>
        /// <param name="barcode">采集条码</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="snInfo">验证结果</param>
        /// <param name="product">运行时产品</param>
        /// <param name="snDatas">过站条码集合</param>
        /// <returns></returns>
        protected virtual ProductInfo InitResult(CollectBarcode barcode, Workcell workcell, Tuple<List<string>, int, int> snInfo, product product, List<SnData> snDatas)
        {
            var result = new ProductInfo
            {
                ItemId = product == null ? 0 : product.ItemId,
                Puid = product == null ? string.Empty : product.Puid,
                WorkOrderId = product == null ? 0 : product.WorkOrderId,
                BarcodeType = barcode.Type,
                Barcode = barcode.Code
            };

            if (result.BarcodeType == BarcodeType.CombinedCode)
            {
                result.PanelInfo.PanelCode = barcode.Code;
                result.PanelInfo.CanBindQty = snInfo.Item2;
                result.PanelInfo.ForkPlateQty = snInfo.Item3;
                result.PanelInfo.SnList.AddRange(snDatas);
            }

            var current = product?.Routing.GetNext().FirstOrDefault(p => p.ProcessId == workcell.ProcessId);
            if (current != null)
            {
                result.PanelInfo.IsBinding = current.IsBinding;
            }
            return result;
        }

        /// <summary>
        /// 获取过站的Sn集合
        /// </summary>
        /// <param name="barcode">采集条码</param>
        /// <returns>验证结果</returns>
        protected virtual Tuple<List<string>, int, int> GetMoveSns(CollectBarcode barcode)
        {
            List<string> sns = new List<string>();
            int canBindQty = 0;
            int forkPlateQty = 0;
            var code = barcode.Code;
            if (barcode.Type == BarcodeType.CombinedCode)
            {
                if (barcode.BarcodeType == BarcodeType.SN)
                {
                    var panelAndBarcodes = RT.Service.Resolve<PanelBindingController>().GetPanelAndBarcodesByPanleCode(code, true);
                    if (!panelAndBarcodes.Any())
                    {
                        throw new ValidationException("生产条码[{0}]未绑定条码，不能做为拼板码过站".L10nFormat(code));
                    }
                    canBindQty = panelAndBarcodes.FirstOrDefault().WorkOrder.PanelQty;

                    if (panelAndBarcodes.FirstOrDefault().WorkOrder.IsPanelWorkOrder)
                    {
                        canBindQty = RT.Service.Resolve<PanelBindingController>().GetPanelWorkOrderCanBindingQty(panelAndBarcodes.FirstOrDefault().WorkOrder);
                    }

                    forkPlateQty = panelAndBarcodes.FirstOrDefault().ForkPlateQty;
                    panelAndBarcodes.ForEach(x =>
                    {
                        sns.Add(x.SN);
                    });
                }
                else
                {
                    var panel = RT.Service.Resolve<PanelController>().GetPanel(code);
                    if (panel == null)
                    {
                        throw new ValidationException("拼板码[{0}]不存在".L10nFormat(code));
                    }

                    canBindQty = panel.WorkOrder.PanelQty;

                    if (panel.WorkOrder.IsPanelWorkOrder)
                    {
                        canBindQty = RT.Service.Resolve<PanelBindingController>().GetPanelWorkOrderCanBindingQty(panel.WorkOrder);
                    }

                    forkPlateQty = panel.ForkPlateQty;

                    sns.AddRange(RT.Service.Resolve<PanelBindingController>().GetPanelBindingSn(code));
                }
            }
            else if (barcode.Type == BarcodeType.SN)
            {
                //判断条码是否绑定拼板，绑定的话按拼板过站
                var bindingRecord = RT.Service.Resolve<PanelBindingController>().GetPanelAndBarcodeBySn(code);
                if (bindingRecord != null)
                {
                    throw new ValidationException("生产条码[{0}]已绑定拼板码，请按拼板码过站".L10nFormat(code));
                }
            }
            return Tuple.Create(sns, canBindQty, forkPlateQty);
        }

        /// <summary>
        /// 验证条码是否为拼板码（无采集步骤场景需要判断条码类型）
        /// </summary>
        /// <param name="barcode">条码信息</param>
        protected virtual void ValidateIsPanel(CollectBarcode barcode)
        {
            var isPanel = RT.Service.Resolve<PanelController>().IsExistPanel(barcode.Code);
            if (isPanel)
            {
                barcode.Type = BarcodeType.CombinedCode;
            }
        }

        /// <summary>
        /// 验证后事件：
        /// 1.产品工艺路线。
        /// 2.工单状态。
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="product">运行时产品信息</param>
        /// <param name="result">验证结果</param> 
        protected virtual void OnValidateFinish(CollectBarcode barcode, Workcell workcell, product product, ProductInfo result)
        {
        }

        /// <summary>
        /// 验证条码
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="workcell">工作单元</param>
        /// <exception cref="ValidationException">条码不存在、条码已报废</exception>
        /// <exception cref="ArgumentNullException">条码为空、工作单元为空</exception>
        public virtual void ValidateBarcode(CollectBarcode barcode, Workcell workcell)
        {
            if (barcode == null)
            {
                throw new ArgumentNullException(nameof(barcode));
            }

            if (workcell == null)
            {
                throw new ArgumentNullException(nameof(workcell));
            }

            switch (barcode.Type)
            {
                case BarcodeType.SN:
                    SNValidate(barcode, workcell);
                    break;
                case BarcodeType.TurnoverBox:
                    var config = ConfigService.GetConfig(new ProductTrunoverBoxTypeConfig());
                    var box = RT.Service.Resolve<BoxController>().GetTurnoverBox(barcode.Code, config.BoxType);
                    if (box == null)
                    {
                        throw new ValidationException(NOT_EXISTS_FORMAT.L10nFormat(barcode));
                    }
                    if (box.State == BoxState.Scrap)
                    {
                        throw new ValidationException(IS_SCRAPED_FORMAT.L10nFormat(barcode));
                    }
                    break;
                case BarcodeType.KeyLabel:
                    //var label = RT.Service.Resolve<SIE.Packages.ItemLabels.ItemLabelController>().GetItemLabel(barcode.Code);
                    //if (label == null)
                    //    throw new ValidationException("[{0}]不存在".L10nFormat(barcode));
                    break;
                case BarcodeType.CombinedCode:
                    var panel = RT.Service.Resolve<PanelController>().GetPanel(barcode.Code);
                    if (panel == null)
                    {
                        //拼板码类型时判断一下是否SN当拼板码
                        var panelBarcodes = RT.Service.Resolve<PanelBindingController>().GetPanelBindingSn(barcode.Code);
                        if (panelBarcodes.Any())
                        {
                            barcode.BarcodeType = BarcodeType.SN;
                            SNValidate(barcode, workcell);
                        }
                        else
                        {
                            throw new ValidationException(NOT_EXISTS_FORMAT.L10nFormat(barcode));
                        }
                    }
                    else
                    {
                        PanelValidate(barcode, panel);
                    }
                    break;
                case BarcodeType.CSN:
                    break;
                default:
                    break;
            }
        }

        private static void PanelValidate(CollectBarcode barcode, Panel panel)
        {
            if (panel.IsScrap)
            {
                throw new ValidationException(IS_SCRAPED_FORMAT.L10nFormat(barcode));
            }

            if (panel.IsPending == true)
            {
                throw new ValidationException(IS_PENDING_FORMAT.L10nFormat(barcode));
            }
        }

        private static void SNValidate(CollectBarcode barcode, Workcell workcell)
        {
            var bcCtr = RT.Service.Resolve<BarcodeController>();
            if (!bcCtr.IsBarcodeEnabled(barcode.Code))
            {
                var sn = bcCtr.GetBarcode(barcode.Code);
                if (sn == null)
                {
                    throw new ValidationException(NOT_EXISTS_FORMAT.L10nFormat(barcode));
                }
                if (sn.IsScraped)
                {
                    throw new ValidationException(IS_SCRAPED_FORMAT.L10nFormat(barcode));
                }
                if (sn.IsPending)
                {
                    throw new ValidationException(IS_PENDING_FORMAT.L10nFormat(barcode));
                }
            }
            var bcProCtr = RT.Service.Resolve<BarcodeProcessController>();
            if (!bcProCtr.MoveBarcodeValiOpter(barcode.Code, workcell.ProcessId))
            {
                throw new ValidationException("当前用户不是条码工序指派的员工，请检查！".L10N());
            }

        }

        /// <summary>
        /// 条码是否存在，只有Sn和KeyLabel能验证
        /// </summary>
        /// <param name="barcode">采集条码</param>
        /// <returns>返回条码是否存在</returns>
        protected virtual bool ExistsBarcode(CollectBarcode barcode)
        {
            if (barcode == null)
            {
                return false;
            }

            switch (barcode.Type)
            {
                case BarcodeType.SN:
                    return RT.Service.Resolve<BarcodeController>().ExistsSn(barcode.Code);
                case BarcodeType.CSN:
                    return true;
                case BarcodeType.TurnoverBox:
                    {
                        var config = ConfigService.GetConfig(new ProductTrunoverBoxTypeConfig());
                        return RT.Service.Resolve<BoxController>().ExistsTurnoverBox(barcode.Code, config.BoxType);
                    }

                case BarcodeType.KeyLabel:
                    return RT.Service.Resolve<SIE.Packages.ItemLabels.ItemLabelController>().Exists(barcode.Code);
                default:
                    break;
            }

            return false;
        }

        /// <summary>
        /// 获取周转箱
        /// </summary>
        /// <param name="code">编码</param>
        /// <returns>周转箱</returns>
        /// <exception cref="ValidationException">生产周转箱不存在</exception>
        protected virtual TurnoverBox GetTurnoverBox(string code)
        {
            var config = ConfigService.GetConfig(new ProductTrunoverBoxTypeConfig());
            var box = RT.Service.Resolve<BoxController>().GetTurnoverBox(code, config.BoxType);
            if (box == null)
            {
                throw new ValidationException("生产周转箱[{0}]不存在".L10nFormat(code));
            }
            return box;
        }

        /// <summary>
        /// 验证工作单元信息
        /// </summary>
        /// <param name="workcell">工作单元信息</param>
        /// <returns>工序</returns>
        /// <exception cref="EntityNotFoundException">资源未找到、工序未找到、工位未找到、用户未找到</exception>
        /// <exception cref="ValidationException">工位关联产线不正确、工位关联工序不正确</exception>
        public virtual Process ValidateWorkcell(Workcell workcell)
        {
            var line = GetById<WipResourceMove>(workcell.ResourceId);
            if (line == null)
            {
                throw new EntityNotFoundException(typeof(WipResource), workcell.ResourceId);
            }
            var process = GetById<Process>(workcell.ProcessId);
            if (process == null)
            {
                throw new EntityNotFoundException(typeof(Process), workcell.ProcessId);
            }

            var station = GetById<Station>(workcell.StationId);
            if (station == null)
            {
                throw new EntityNotFoundException(typeof(Station), workcell.StationId);
            }

            var user = GetById<EmployeeMove>(workcell.EmployeeId);
            if (user == null)
            {
                throw new EntityNotFoundException(typeof(Employee), workcell.EmployeeId);
            }

            if (station.ResourceId != line.Id)
            {
                throw new ValidationException("工位[{0}]关联的资源不是[{1}]，应该是[{2}]".L10nFormat(station.Name, line.Name, station.Resource.Name));
            }
            if (!station.StationProcessList.Select(p => p.ProcessId).Contains(process.Id))
            {
                throw new ValidationException("工序[{0}]非工位[{0}]关联的工序".L10nFormat(process.Name, station.Name));
            }

            //这里需要增加验证设备是否存在的
            return process;
        }
        /// <summary>
        /// 验证工作单元信息
        /// </summary>
        /// <param name="workcell">工作单元信息</param>
        /// <returns>工序</returns>
        /// <exception cref="EntityNotFoundException">资源未找到、工序未找到、工位未找到、用户未找到</exception>
        /// <exception cref="ValidationException">工位关联产线不正确、工位关联工序不正确</exception>
        public virtual void ValidateWorkcellEx(Workcell workcell)
        {
            var station = Query<Station>().Where(t => t.Id == workcell.StationId && t.ResourceId == workcell.ResourceId)
                                          .Join<StationProcess>((a, b) => a.Id == b.StationId && b.ProcessId == workcell.ProcessId).Count();
            if (station == 0)
            {
                ValidateWorkcell(workcell);
            }
        }

        /// <summary>
        /// 数据采集
        /// </summary>
        /// <param name="barcodes">条码数组</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元</param> 
        /// <exception cref="ArgumentNullException">条码数组为空、采集数据为空、工作单元为空</exception>
        public virtual void Collect(string[] barcodes, CollectData collectData, Workcell workcell)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            if (barcodes == null || barcodes?.Length <= 0)
            {
                throw new ArgumentNullException(nameof(barcodes));
            }
            if (collectData == null)
            {
                throw new ArgumentNullException(nameof(collectData));
            }
            if (workcell == null)
            {
                throw new ArgumentNullException(nameof(workcell));
            }
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                Move(barcodes, collectData, workcell);
                tran.Complete();
            }
            stopwatch.Stop();
            Context.DistributionContext.GlobalContext["CollectTime"] = stopwatch.ElapsedMilliseconds;
        }

        /// <summary>
        /// 过站
        /// </summary>
        /// <param name="barcodes">条码数组</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元</param>
        /// <returns>运行时产品</returns> 
        protected virtual ProductInfo Move(string[] barcodes, CollectData collectData, Workcell workcell)
        {
            ProductInfo productInfo = new ProductInfo();

            //验证工作单元信息                
            ValidateWorkcellEx(workcell);

            var wipResourceMove = GetById<WipResourceMove>(workcell.ResourceId);
            if (wipResourceMove == null)
            {
                throw new EntityNotFoundException(typeof(WipResource), workcell.ResourceId);
            }

            ////验证采集步骤
            var collectStepList = RT.Service.Resolve<ProcessController>().GetProcessCollectSteps(workcell.ProcessId).OrderBy(p => p.Step).ToList();
            var collectBarcodes = ValidateCollectStep(barcodes, collectStepList.ToArray(), collectData.CollectBarcode, collectData.NoValidateStep);


            product product = null;
            //检验当前采集工序是否来自多个子路线

            var isFromDiffLines = IsFromDifferentLines(collectBarcodes);
            if (isFromDiffLines && collectBarcodes.Any(m => m.Type == BarcodeType.SN))//来着不同子路线 同时必须有一个是生产条码
            {
                //取生产条码作为本次的produce
                var snCollectBarcode = collectBarcodes.FirstOrDefault(m => m.Type == BarcodeType.SN);
                product = ValidateProduct(snCollectBarcode, workcell, wipResourceMove);
            }
            else
            {
                //验证产品工艺路线
                product = ValidateProduct(collectBarcodes[0], workcell, wipResourceMove);
            }

            //验证条码
            for (int i = 1; i < collectBarcodes.Count; i++)
            {
                ValidateBarcode(collectBarcodes[i], workcell);
            }

            ////验证工单
            ValidateWorkOrder(product);

            ////验证暂停的产品
            if (product.IsHold)
            {
                ValidateHoldProductEx(workcell.ProcessId, collectData);
            }

            //上一次采集的采集结果
            ResultType? lastResultType = null;
            if (product.Routing.Current != null)
            {
                lastResultType = product.Routing.Current.Result;
            }

            //过站-->切换产品的运行时的工艺路线的当前工序为当前采集工序
            product.Routing.Current = product.Routing.GetNext().FirstOrDefault(p => p.ProcessId == workcell.ProcessId);

            var dateTime = RF.Find<Process>().GetDbTime();

            //最后过站时间
            product.Routing.LastMoveDateTime = dateTime;

            //事件数据
            var data = new CollectEventData(product, collectBarcodes.ToArray(), collectData, workcell, dateTime);

            //采集开始通知
            OnCollecting(data);

            //当前在制版本
            var version = GetWipProductVersion(product.Puid);

            //条码关联Puid
            MapBarcodes(product, version, collectBarcodes, workcell, isFromDiffLines);

            //创建过站记录
            var wipProductProcess = CreateWipProductProcess(product, version, collectBarcodes, collectData, workcell, wipResourceMove);
            UpdateOtherBarcodeInfo(collectBarcodes, isFromDiffLines, version);
            // 创建员工信息
            var wipProEmployee = CreateWipProProcessEmployee(collectBarcodes, wipProductProcess.Id, workcell.ProcessId);

            //解绑
            Unbind(version, collectStepList.ToArray(), collectBarcodes);

            //启用入站控制，过站时将出入站状态设置为出站
            product.Routing.Current.WipProductProcessState = collectData.State;

            //计算工艺路线后工序
            //报废会将产品完工下线，并清空运行时，此处不再计算后工序，不再保存运行时
            if (!collectData.Grade.HasValue || collectData.Grade != ProductGrade.Scrap)
            {
                if (!string.IsNullOrEmpty(product.Routing.Current.GroupId))
                {
                    //不是同一工序组的工序，从下工序列表中清掉
                    var processIds = product.Routing.GetNext()
                       .Where(x => x.GroupId == null || x.GroupId != product.Routing.Current.GroupId)
                       .Select(x => x.Id)
                       .ToList();

                    processIds.ForEach(id =>
                    {
                        product.Routing.Next.Remove(id);
                    });
                }
                else
                {
                    product.Routing.Next.Clear();
                }

                if (collectData.State == WipProductProcessState.Finish)
                {
                    if (collectData.IsRecheck)
                    {
                        //工艺复判（NTF采集）采集结果以上一次采集的采集结果为准
                        product.Routing.Current.Result = lastResultType;
                    }
                    else
                    {
                        //增加记录当次采集的采集结果（通过/失败），出站（Move Out）才写入
                        product.Routing.Current.Result = collectData.Result;
                    }

                    ComputeNextProcess(product, collectData.Result, collectData, version);
                    SaveNextProcess(workcell, product, version);
                    ComputeNextProcessFinish(wipProductProcess, product, collectBarcodes, collectData, workcell);

                }
                else
                {
                    if (collectData.State == WipProductProcessState.Start
                        && string.IsNullOrEmpty(product.Routing.Current.GroupId))
                    {
                        //入站时记录上一次采集的采集结果（通过/失败）
                        product.Routing.Current.Result = lastResultType;
                        product.Routing.Next.Add(product.Routing.Current.Id);
                    }
                }

                // 对于已经完成生产的产品进行状态设置和清空运行时（下线完成产品生产）
                // 计算后工序 ComputeNextProcess时，会更新生产版本是否完工下线
                if (version.IsFinish)
                {
                    version.FinishDateTime = dateTime;
                    version.Product.State = WipProductState.Finish;
                    version.NextProcess = null;
                    RuntimeController.RemoveProduct(product);

                    //过站逻辑-倒扣非工序BOM物料
                    RT.Service.Resolve<BackflushMaterialController>().BackflushMaterialByFinsh(collectBarcodes[0].Code,
                        workcell.ResourceId, workcell.ProcessId, workcell.StationId, product);
                }
                else
                {
                    //保存采集运行时产品
                    RuntimeController.Save(product);
                }
            }

            ////拼板码绑定SN
            PanelBindingOnProcess(version, wipProductProcess, product, collectBarcodes, collectData, workcell);
            wipProductProcess.InInning = product.Routing.Current.InInning;

            RF.Save(wipProductProcess);
            if (wipProEmployee.Count > 0)
            {
                RF.BatchInsert(wipProEmployee);
            }
            if (collectData.State == WipProductProcessState.Finish)
            {
                //过站记录创建后事件处理
                OnWipProductProcessFinished(wipProductProcess, product, collectBarcodes, collectData, workcell);
                //过站完成生成工单耗用单 
                RT.Service.Resolve<BackflushMaterialController>().BackflushMaterialByProcess(collectBarcodes[0].Code,
                        workcell.ResourceId, workcell.ProcessId, workcell.StationId, product);
                if (wipProductProcess.IsDirty)
                {
                    throw new ValidationException("请不要在【过站记录创建后事件处理】中修改工序过站记录。".L10N());
                }
                //保存采集运行时产品
                RuntimeController.Save(product);
            }

            RF.Save(version);
            RF.Save(version.Product);

            string packNos = DoPackage(barcodes.FirstOrDefault(), workcell, collectData.Context["ADVANCE_PACKAGE_NO_LIST"], collectData.Context["IS_ADVANCE"]);
            productInfo.Context["PACK_NO_LIST_STRING"] = packNos;

            //采集结束通知
            OnCollected(data, version);

            return productInfo;
        }

        /// <summary>
        /// 更新除生产条码外的其它条码
        /// </summary>
        /// <param name="collectBarcodes"></param>
        /// <param name="isFromDiffLines"></param>
        /// <param name="version"></param>
        private void UpdateOtherBarcodeInfo(IList<CollectBarcode> collectBarcodes, bool isFromDiffLines, WipProductVersion version)
        {
            if (isFromDiffLines)//更新非生产条码外的条码的关联条码
            {
                var wipProductVersionds = new EntityList<WipProductVersion>();
                var snCollectBarcode = collectBarcodes.FirstOrDefault(m => m.Type == BarcodeType.SN);
                if (snCollectBarcode == null)
                {
                    throw new ValidationException("过站失败，多分支工艺路线合并到生产条码工艺路线时，必须有一分支采集条码的类型为生产条码".L10N());
                }
                foreach (var barcode in collectBarcodes)
                {
                    var currenProduct = RuntimeController.FindProduct(barcode);
                    if (currenProduct != null)
                    {
                        var otherVersion = GetWipProductVersion(currenProduct.Puid);
                        if (otherVersion != null && otherVersion.Id != version.Id)
                        {
                            otherVersion.RelevanceSn = snCollectBarcode.Code;
                            otherVersion.PersistenceStatus = PersistenceStatus.Modified;
                            wipProductVersionds.Add(otherVersion);
                        }
                    }
                }
                RF.Save(wipProductVersionds);
            }
        }

        /// <summary>
        /// 检查多条码是否来自不同的子路线
        /// </summary>
        /// <param name="barcodes"></param>
        /// <returns>返回ture/false</returns>
        protected virtual bool IsFromDifferentLines(IList<CollectBarcode> barcodes)
        {
            List<product> productList = new List<product>();
            foreach (var barcode in barcodes)
            {
                var currenProduct = RuntimeController.FindProduct(barcode);
                if (currenProduct != null)
                {
                    if (!productList.Exists(m => m.Puid == currenProduct.Puid))
                    {
                        productList.Add(currenProduct);
                    }
                }
            }
            return productList.Count > 1;

        }

        /// <summary>
        /// 计算下一工艺路线完成后
        /// </summary>
        /// <param name="wipProductProcess">生产采集记录</param>
        /// <param name="product">运行时产品</param> 
        /// <param name="barcodes">采集条码集合</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元</param>
        /// <returns>生产采集记录</returns>
        protected virtual void ComputeNextProcessFinish(WipProductProcess wipProductProcess, product product,
            IList<CollectBarcode> barcodes, CollectData collectData, Workcell workcell)
        {
        }

        /// <summary>
        /// 保存下一工序
        /// </summary>
        /// <param name="workcell">工治具单元</param>
        /// <param name="product">运行时产品</param>
        /// <param name="version">版本</param>
        private void SaveNextProcess(Workcell workcell, product product, WipProductVersion version)
        {
            try
            {
                //获取工艺路线下一工序
                var nexts = product.Routing.GetNext();
                var nextProcess = nexts.FirstOrDefault(p => !p.Optional && p.ProcessId != workcell.ProcessId);
                if (nextProcess == null)
                {
                    nextProcess = nexts.FirstOrDefault(p => !p.Optional && p.ProcessId == workcell.ProcessId);
                }
                if (!version.IsFinish)
                {
                    version.CurrenrProcessIndex = product.Routing.Current.Index;
                    version.NextProcessId = nextProcess?.ProcessId;
                    version.NextProcessIndex = nextProcess?.Index;
                }
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc);
            }
        }

        /// <summary>
        /// 解绑
        /// </summary>
        /// <param name="version">生产产品版本</param>
        /// <param name="steps">采集步骤数组</param>
        /// <param name="collectBarcodes">采集条码集合</param>
        protected virtual void Unbind(WipProductVersion version, ProcessCollectStep[] steps, IList<CollectBarcode> collectBarcodes)
        {
            //var steps = process.CollectStepList.OrderBy(p => p.Step).ToArray()
            for (int i = 0; i < steps.Length; i++)
            {
                if (steps[i].IsUnbound)
                {
                    var barcode = collectBarcodes[i];
                    RuntimeController.UnmapPuid(barcode);
                    if (barcode.Type == BarcodeType.TurnoverBox)
                    {
                        version.BoxNo = null;
                        var box = GetTurnoverBox(barcode.Code);
                        box.State = BoxState.Unused;
                        RT.Service.Resolve<BoxController>().CreateActionLog(box.Id, null, 1, version.Sn, TurnoverType.UnBinding);
                        RF.Save(box);
                    }
                }
            }
        }

        /// <summary>
        /// 计算后工序
        /// </summary>
        /// <param name="product">运行时产品</param>
        /// <param name="result">结果类型</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="version">产品生产版本</param>
        internal virtual void ComputeNextProcess(product product, ResultType result, CollectData collectData, WipProductVersion version = null)
        {
            //更新下一工序集合时将当前工序采集次数+1
            product.Routing.Current.PassNum += 1;

            if (result == ResultType.Pass)
            {
                product.Routing.Current.IsPass = true;
            }

            //在胜制局中，不计算后工序
            if (collectData != null && DetermineInning(product, result, collectData))
            {
                product.Routing.Next.Add(product.Routing.Current.Id);
                return;
            }

            if (!string.IsNullOrEmpty(product.Routing.Current.GroupId))
            {
                //工序组
                var groupProcess = product.Routing.Processes
                  .FirstOrDefault(x => x.GroupId == product.Routing.Current.GroupId
                    && x.IsGroup == true);

                if (groupProcess == null)
                {
                    throw new ValidationException("找不到工序组【{0}】的信息"
                        .L10nFormat(product.Routing.Current.GroupId));
                }

                //采集结果为不合格，转到工序组失败连线到的工序或工序组
                if (result == ResultType.Fail)
                {
                    //跳出工序组，将原来的后工序列表清空
                    product.Routing.Next.Clear();

                    ComputeNextProcess(groupProcess, product, result, collectData);
                }
                else
                {
                    //工序组下当前工序不可重复过站，则从Next列表中移取
                    if (!product.Routing.Current.Repeat)
                    {
                        product.Routing.Next.Remove(product.Routing.Current.Id);
                    }

                    //后工序列表为空了，计算后工序
                    if (!product.Routing.Next.Any())
                    {
                        //不合格或工序组下的工序全部已过站，跳出工序组，转到工序组失败连线到的工序或工序组
                        //工序组是最后的工序，完工下线
                        if (groupProcess.IsEnd && result == ResultType.Pass)
                        {
                            if (version != null)
                            {
                                version.IsFinish = true;
                            }

                            return;
                        }

                        //跳出工序组，将原来的后工序列表清空
                        product.Routing.Next.Clear();

                        ComputeNextProcess(groupProcess, product, result, collectData);
                    }
                    else
                    {
                        if (product.Routing.GetNext().All(x => x.GroupId == product.Routing.Current.GroupId)
                            && product.Routing.GetNext().All(x => (x.IsPass && x.Repeat) || x.Optional))
                        {
                            //后工序列表，全是工序组下面的工序
                            //后工序列表中的全部工序：可重复的工序已过站 或 可选工序，不清空自己，计算后工序
                            ComputeNextProcess(groupProcess, product, result, collectData);
                        }
                    }
                }
            }
            else
            {
                //最后的工序，完工下线
                if (product.Routing.Current.IsEnd && result == ResultType.Pass)
                {
                    if (version != null)
                    {
                        version.IsFinish = true;
                    }

                    return;
                }

                product.Routing.Next.Clear();

                //工序可重复过站，则将工序再加入
                if (product.Routing.Current.Repeat)
                {
                    product.Routing.Next.Add(product.Routing.Current.Id);
                }

                ComputeNextProcess(product.Routing.Current, product, result, collectData);
            }
        }

        /// <summary>
        /// 递归计算后工序
        /// </summary>
        /// <param name="process">工序</param>
        /// <param name="product">运行时产品</param>
        /// <param name="result">结果类型</param>
        /// <param name="collectData">采集数据</param>
        /// <exception cref="ValidationException">采集结果无效</exception>
        internal virtual void ComputeNextProcess(process process, product product, ResultType result, CollectData collectData)
        {
            List<double> nextIds;
            if (result == ResultType.Custom)
            {
                nextIds = ComputeNextProcess(process, product, collectData);
            }
            else
            {
                if (!process.Next.TryGetValue(result, out nextIds))
                {
                    throw new ValidationException("工序[{0}]未配置采集结果为[{1}]的工序参数"
                        .L10nFormat(process.Name, result.ToLabel()));
                }
            }

            foreach (var nextId in nextIds)
            {
                var nexts = product.Routing.Processes.Where(q => q.Id == nextId).ToList();

                foreach (var next in nexts)
                {
                    next.IsPass = false;

                    if (next.IsGroup == true)
                    {
                        //工序组下的所有工序都加入后工序列表
                        //工序组，则取组下的所有工序
                        var processesOfGroup = product.Routing.Processes
                            .Where(x => x.GroupId == next.GroupId && x.IsGroup != true).ToList();

                        foreach (var processUnderGroup in processesOfGroup)
                        {
                            processUnderGroup.IsPass = false;

                            if (!product.Routing.Next.Contains(processUnderGroup.Id))
                            {
                                product.Routing.Next.Add(processUnderGroup.Id);
                            }
                        }
                    }
                    else
                    {
                        if (!product.Routing.Next.Contains(next.Id))
                        {
                            product.Routing.Next.Add(next.Id);
                        }

                        if (next.Optional)
                        {
                            ComputeNextProcess(next, product, ResultType.Pass, collectData);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 递归计算后工序
        /// </summary>
        /// <param name="process">工序</param>
        /// <param name="product">运行时产品</param> 
        /// <param name="collectData">采集数据</param>
        /// <exception cref="ValidationException">未找到下一工序</exception>
        /// <exception cref="ValidationException">未配置脚本</exception>
        /// <returns>下一工序id</returns>
        List<double> ComputeNextProcess(process process, product product, CollectData collectData)
        {
            List<double> nextProcessIds = new List<double>();
            var nextProcess = process.Next.Where(p => p.Key == ResultType.Custom).SelectMany(p => p.Value).ToList();
            foreach (var item in nextProcess)
            {
                var pro = product.Routing.Processes.FirstOrDefault(q => q.Id == item);
                string script;
                if (!process.Script.TryGetValue(item, out script))
                {
                    throw new ValidationException("未配置工序[{0}]至下一工序[{1}]脚本".L10nFormat(process.Name, pro.Name));
                }

                if (GetScriptResult(script, collectData, process))
                {
                    nextProcessIds.Add(item);
                }
            }

            if (nextProcessIds.Any())
            {
                return nextProcessIds;
            }
            else
            {
                throw new ValidationException("未找到下一工序".L10N());
            }
        }

        /// <summary>
        /// 判定局数
        /// </summary>
        /// <param name="product">运行时产品</param>
        /// <param name="result">结果类型</param>
        /// <param name="collectData">采集数据</param>
        /// <returns>在局中返回true，出局返回false 进入下一工序</returns>
        protected virtual bool DetermineInning(product product, ResultType result, CollectData collectData)
        {
            bool isInning = true;   //是否在局中
            var current = product.Routing.Current;
            if ((current.Type == ProcessType.Pqc /*|| current.Type == ProcessType.Fqc*/) && !current.Repeat && current.ProcessId.HasValue)
            {
                if (!current.NormalVictory.HasValue && !current.RepairVictory.HasValue)
                {
                    isInning = false;
                }
                else
                {
                    double workOrderId = product.WorkOrderId;
                    string sn = collectData.CollectBarcode.Code;
                    double processId = current.ProcessId.Value;
                    double victoryId = 0;
                    var isRepair = IsHasRepair(workOrderId, sn, processId, current.IsStricter);
                    if (isRepair && current.RepairVictory.HasValue)
                    {
                        victoryId = current.RepairVictory.Value;
                    }
                    else if (current.NormalVictory.HasValue)
                    {
                        victoryId = current.NormalVictory.Value;
                    }
                    else
                    {
                        isInning = false;
                    }

                    if (victoryId > 0)
                    {
                        var victoryRecordString = RT.Service.Resolve<WipProductVersionController>()
                            .GetVictoryRecordString(workOrderId, sn, processId);

                        //加上本次【工序过站记录】
                        victoryRecordString += ((result == ResultType.Fail) ? "0" : "1");

                        var matchResult = RT.Service.Resolve<VictoryStandardController>().VictoryMatch(victoryId, victoryRecordString);
                        if (matchResult != MatchResult.PartialMatch)
                        {
                            isInning = false;
                        }
                    }
                }
            }
            else
            {
                isInning = false;
            }
            current.InInning = isInning;
            return isInning;
        }

        /// <summary>
        /// 是否经过维修
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="sn">产品条码</param>
        /// <param name="processId">工序ID</param>
        /// <param name="isStricter">是否加严</param>
        /// <returns>维修过返回true，否则返回false</returns>
        private bool IsHasRepair(double workOrderId, string sn, double processId, bool isStricter)
        {
            var verController = RT.Service.Resolve<WipProductVersionController>();
            if (isStricter)
            {
                return verController.IsSnHasRepair(workOrderId, sn);
            }
            else
            {
                return verController.IsSnHasRepairInProcess(workOrderId, sn, processId);
            }
        }

        /// <summary>
        /// 获取脚本执行结果
        /// </summary>
        /// <param name="script">脚本</param>
        /// <param name="collectData">采集结果</param>
        /// <param name="process">工序</param>
        /// <returns>匹配返回true，否则返回false</returns>
        bool GetScriptResult(string script, CollectData collectData, process process)
        {
            try
            {
                if (script.IsNullOrEmpty())
                {
                    return false;
                }
                ScriptEngine engine = Python.CreateEngine();
                ScriptScope scope = engine.CreateScope();
                engine.CreateScriptSourceFromString(script).Compile().Execute(scope);
                var func = scope.GetVariable<Func<CollectData, bool>>("GetNextProcess");
                return func(collectData);
            }
            catch (Exception)
            {
                throw new ValidationException("工序 [{0}] 的自定义参数配置错误, 请检查!".L10nFormat(process.Name));
            }
        }

        /// <summary>
        /// 验证采集步骤
        /// </summary>
        /// <param name="barcodes">条码数组</param>
        /// <param name="steps">采集步骤数组</param>
        /// <param name="collectBarcode">采集步骤（前端传入）</param>
        /// <param name="noValidateStep">不验证采集步骤</param>
        /// <returns>采集条码集合</returns>
        /// <exception cref="ValidationException">条码与采集步骤不一致、条码为空</exception>
        protected virtual IList<CollectBarcode> ValidateCollectStep(string[] barcodes, ProcessCollectStep[] steps, CollectBarcode collectBarcode, bool noValidateStep)
        {
            var result = new List<CollectBarcode>();
            if (noValidateStep)
            {
                result.Add(collectBarcode);
            }
            else
            {
                if (barcodes.Length != steps.Length)
                {
                    throw new ValidationException("条码与采集步骤不一致".L10N());
                }
                for (int i = 0; i < barcodes.Length; i++)
                {
                    if (barcodes[i].IsNullOrWhiteSpace())
                    {
                        throw new ValidationException("条码类型：{0} 的条码不允许为空".L10nFormat(EnumViewModel.EnumToLabel(steps[i].BarcodeType).L10N()));
                    }
                    result.Add(new CollectBarcode { Code = barcodes[i], Type = steps[i].BarcodeType });
                }
            }
            return result;
        }

        /// <summary>
        /// 条码关联产品PUID
        /// </summary>
        /// <param name="product">运行时产品</param>
        /// <param name="version">生产产品版本</param>
        /// <param name="barcodes">采集条码集合</param>
        /// <param name="workcell">工作单元</param>
        /// <exception cref="ValidationException">本次采集条码与之前采集条码不一致</exception>
        protected virtual void MapBarcodes(product product, WipProductVersion version, IList<CollectBarcode> barcodes, Workcell workcell, bool isFromDiffLines = false)
        {
            if (isFromDiffLines)
            {
                product currenProduct = null;
                foreach (var barcode in barcodes)
                {
                    //两个不同子路线的product 不相同的
                    currenProduct = ValidateProduct(barcode, workcell);

                    var code = RuntimeController.FindMapBarcode(currenProduct.Puid, barcode.Type);
                    if (code.IsNotEmpty() && code != barcode.Code)
                    {
                        throw new ValidationException("产品本次采集的条码[{0}]与之前采集的条码[{1}]不一致".L10nFormat(barcode, code));
                    }
                    if (MapBarcode(currenProduct.Puid, barcode, version.Sn) && barcode.Type == BarcodeType.SN)
                    {
                        var orderId = RT.Service.Resolve<BarcodeController>().GetBarcodeOrderId(barcode.Code);
                        if (orderId != null && currenProduct.WorkOrderId != orderId)
                        {
                            //换工单
                            ChangedWorkOrder(currenProduct, version, orderId.Value);
                        }
                    }
                }
                return;
            }

            foreach (var barcode in barcodes)
            {
                var code = RuntimeController.FindMapBarcode(product.Puid, barcode.Type);
                if (code.IsNotEmpty() && code != barcode.Code)
                {
                    throw new ValidationException("产品本次采集的条码[{0}]与之前采集的条码[{1}]不一致".L10nFormat(barcode, code));
                }
                if (MapBarcode(product.Puid, barcode, version.Sn) && barcode.Type == BarcodeType.SN)
                {
                    var orderId = RT.Service.Resolve<BarcodeController>().GetBarcodeOrderId(barcode.Code);
                    if (orderId != null && product.WorkOrderId != orderId)
                    {
                        //换工单
                        ChangedWorkOrder(product, version, orderId.Value);
                    }
                }
            }
        }

        /// <summary>
        /// 切换工单
        /// </summary>
        /// <param name="product">运行时产品</param>
        /// <param name="version">产品产品版本</param>
        /// <param name="workOrderId">工单</param>
        protected virtual void ChangedWorkOrder(product product, WipProductVersion version, double workOrderId)
        {
            //TODO:验证前工序的BOM是否一致，否则不能换工单
            product.WorkOrderId = workOrderId;
            version.WorkOrderId = workOrderId;
        }

        /// <summary>
        /// 关联条码
        /// </summary>
        /// <param name="puid">产品ID</param>
        /// <param name="barcode">采集条码</param>
        /// <param name="sn">条码</param>
        /// <returns>关联成功返回true，失败返回false</returns>
        protected virtual bool MapBarcode(string puid, CollectBarcode barcode, string sn)
        {
            bool mapped = RuntimeController.MapPuid(barcode, puid);
            if (mapped && barcode.Type == BarcodeType.TurnoverBox)
            {
                var box = GetTurnoverBox(barcode.Code);
                box.State = BoxState.Inuse;
                RT.Service.Resolve<BoxController>().CreateActionLog(box.Id, null, 1, sn, TurnoverType.Binding);
                RF.Save(box);
            }

            return mapped;
        }

        /// <summary>
        /// 验证Hold住的产品
        /// </summary>
        /// <param name="process">工序</param>
        /// <param name="collectData">采集条码</param>
        /// <exception cref="ValidationException">产品已经暂停，不允许在FQC过站</exception>
        protected virtual void ValidateHoldProduct(Process process, CollectData collectData)
        {
            if (!collectData.IsRecheck /*&& process.Type == ProcessType.Fqc*/ && collectData.Result == ResultType.Pass)
            {
                throw new ValidationException("产品已经暂停，不允许在FQC过站".L10N());
            }
        }

        /// <summary>
        /// 验证Hold住的产品
        /// </summary>
        /// <param name="processId">工序</param>
        /// <param name="collectData">采集条码</param>
        /// <exception cref="ValidationException">产品已经暂停，不允许在FQC过站</exception>
        protected virtual void ValidateHoldProductEx(double processId, CollectData collectData)
        {
            if (!collectData.IsRecheck && collectData.Result == ResultType.Pass)
            {
                var count = Query<Process>().Where(t => t.Id == processId /*&& t.Type == ProcessType.Fqc*/).Count();
                if (count == 0)
                {
                    return;
                }
                throw new ValidationException("产品已经暂停，不允许在FQC过站".L10N());
            }
        }

        /// <summary>
        /// 根据生产订单号获取所有工单
        /// </summary>
        /// <param name="ProductionOrder">生产订单号</param>
        /// <returns></returns>
        protected virtual EntityList<WorkOrderMove> GetProductionOrderWorkOrder(string ProductionOrder)
        {
            return Query<WorkOrderMove>().Where(p => p.ProductionOrderCode == ProductionOrder)
                .Where(p => p.State == Core.WorkOrders.WorkOrderState.Release || p.State == Core.WorkOrders.WorkOrderState.Producing).ToList();
        }

        /// <summary>
        /// 生产条码自动归属
        /// </summary>
        /// <param name="sn">生产条码</param>
        /// <param name="workOrder">原工单</param>
        /// <param name="barcodeType">条码类型</param>
        /// <returns></returns>
        private WorkOrderMove SnBelongWorkOrder(Barcodes.Barcode sn, WorkOrderMove workOrder, BarcodeType barcodeType)
        {
            //如果工单对应生产订单号不为空，APS工艺单编号不为空则进行自动归属,归属成功后把归属工单覆盖wo，同生产订单号才可归属
            if (workOrder.ProcessTechOrderCode.IsNotEmpty() && workOrder.ProductionOrderCode.IsNotEmpty())
            {
                var woList = GetProductionOrderWorkOrder(workOrder.ProductionOrderCode);
                foreach (var item in woList)
                {
                    //前APS工艺单编号根据逗号拆分后与当前工单的APS工艺单编号一致则可归属
                    if (item.BeforeTechOrderCode.Split(',').Contains(workOrder.ProcessTechOrderCode) && item.PlanQty > item.PrintedQty)
                    {
                        var belongWo = RF.GetById<WorkOrder>(item.Id);

                        if (belongWo == null)
                        {
                            throw new ValidationException("归属工单不存在，条码归属失败!".L10N());
                        }
                        //当条码类型为拼版码时则是生产条码做拼板码的场景
                        if (barcodeType == BarcodeType.CombinedCode)
                        {
                            var panelAndBarcodes = RT.Service.Resolve<PanelBindingController>().GetPanelAndBarcodesByPanleCode(sn.Sn, true);
                            foreach (var barcode in panelAndBarcodes)
                            {
                                RT.Service.Resolve<BarcodeController>().BarcodeBelong(barcode.Barcode, belongWo);
                            }
                        }
                        else
                        {
                            //匹配到可归属工单后，将条码所属工单更新
                            RT.Service.Resolve<BarcodeController>().BarcodeBelong(sn, belongWo);
                        }
                        return item;
                    }
                }
            }
            return workOrder;
        }

        /// <summary>
        /// 拼板码自动归属
        /// </summary>
        /// <param name="panel">拼板码</param>
        /// <param name="workOrder">原工单</param>
        /// <returns></returns>
        private WorkOrderMove PanelBelongWorkOrder(Panel panel, WorkOrderMove workOrder)
        {
            //如果工单对应生产订单号不为空，APS工艺单编号不为空则进行自动归属,归属成功后把归属工单覆盖wo，同生产订单号才可归属
            if (workOrder.ProcessTechOrderCode.IsNotEmpty() && workOrder.ProductionOrderCode.IsNotEmpty())
            {
                var woList = GetProductionOrderWorkOrder(workOrder.ProductionOrderCode);
                foreach (var item in woList)
                {
                    //前APS工艺单编号根据逗号拆分后与当前工单的APS工艺单编号一致则可归属
                    if (item.BeforeTechOrderCode.Split(',').Contains(workOrder.ProcessTechOrderCode) && item.PlanQty > item.PrintedQty)
                    {
                        if (workOrder.IsPanelWorkOrder == item.IsPanelWorkOrder)
                        {
                            //匹配到可归属工单后，将条码所属工单更新
                            RT.Service.Resolve<IPanelBelongWorkOrder>().PanelBelongWorkOrder(item.Id, panel.Id);

                            //将下文使用到的工单对象赋值为条码最新工单
                            return item;
                        }
                    }
                }
            }
            return workOrder;
        }

        /// <summary>
        /// 查找工单,生产条码及组合板条码自动归属工单
        ///     1.SN条码可知道工单
        ///     2.RFID通过产线上料知道在制工单
        /// </summary>
        /// <param name="barcode">采集条码</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="prductVersionWorkOrderId"></param>
        /// <returns>工单</returns>
        /// <exception cref="ValidationException">工单不存在、产线无当前在制工单、只有生产条码和配送周转箱才能自动关联</exception>
        protected virtual WorkOrderMove GetWorkOrderAndBelong(CollectBarcode barcode, Workcell workcell,
            double prductVersionWorkOrderId)
        {
            switch (barcode.Type)
            {
                case BarcodeType.SN:
                    return GetWorkOrderAndBelongBySN(barcode, prductVersionWorkOrderId);
                case BarcodeType.TurnoverBox:
                    //取当前产线在制造工单
                    var wipLineWorkOrder = GetWipResourceWorkOrder(workcell); //先按工位取
                    if (wipLineWorkOrder == null)
                    {
                        wipLineWorkOrder = GetWipResourceWorkOrder(workcell.ResourceId);
                        if (wipLineWorkOrder == null)
                        {
                            throw new ValidationException("[{0}]产线无当前在制工单".L10nFormat(barcode));
                        }
                    }

                    return GetById<WorkOrderMove>(wipLineWorkOrder.WorkOrderId); //Barcode关联的是Core的工单，要取MES工单
                case BarcodeType.CombinedCode:
                    {
                        var panel = RT.Service.Resolve<PanelController>().GetPanel(barcode.Code);
                        if (panel == null)
                        {
                            //生产条码做为拼板码只归属绑定的所有条码
                            return GetWorkOrderAndBelongBySN(barcode, prductVersionWorkOrderId);
                        }

                        var panelWo = GetById<WorkOrderMove>(panel.WorkOrderId);

                        if (prductVersionWorkOrderId != panelWo.Id)
                        {
                            //运行时版本的工单与生产条码的工单已经不同，说明已经手动归属，不再自动做归属
                            return panelWo;
                        }
                        else
                        {
                            return PanelBelongWorkOrder(panel, panelWo);
                        }
                    }
                default:
                    throw new ValidationException("[{0}]只有{1}和{2}才可以自动关联工单"
                        .L10nFormat(barcode, BarcodeType.SN.ToLabel(), BarcodeType.TurnoverBox.ToLabel()));
            }
        }

        private WorkOrderMove GetWorkOrderAndBelongBySN(CollectBarcode barcode, double prductVersionWorkOrderId)
        {
            //通过SN取关联工单
            Barcode sn = RT.Service.Resolve<BarcodeController>().GetBarcode(barcode.Code);

            if (sn == null)
            {
                throw new ValidationException(ORDER_NOT_FOUND_FORMAT.L10nFormat(barcode));
            }

            var wo = GetById<WorkOrderMove>(sn.WorkOrderId);

            if (prductVersionWorkOrderId != wo.Id)
            {
                //运行时版本的工单与生产条码的工单已经不同，说明已经手动归属，不再自动做归属
                return wo;
            }
            else
            {
                return SnBelongWorkOrder(sn, wo, barcode.Type);
            }
        }

        /// <summary>
        /// 查找工单:
        ///     1.SN条码可知道工单
        ///     2.RFID通过产线上料知道在制工单
        /// </summary>
        /// <param name="barcode">采集条码</param>
        /// <param name="workcell">工作单元</param>
        /// <returns>工单</returns>
        /// <exception cref="ValidationException">工单不存在、产线无当前在制工单、只有生产条码和配送周转箱才能自动关联</exception>
        protected virtual WorkOrderMove GetWorkOrder(CollectBarcode barcode, Workcell workcell)
        {
            switch (barcode.Type)
            {
                case BarcodeType.SN:
                    //通过SN取关联工单
                    Barcode sn = RT.Service.Resolve<BarcodeController>().GetBarcode(barcode.Code);
                    if (sn == null)
                    {
                        throw new ValidationException(ORDER_NOT_FOUND_FORMAT.L10nFormat(barcode));
                    }
                    return GetById<WorkOrderMove>(sn.WorkOrderId); //Barcode关联的是Core的工单，要取MES工单 
                case BarcodeType.TurnoverBox:
                case BarcodeType.KeyLabel:
                    //取当前产线在制造工单
                    var wipLineWorkOrder = GetWipResourceWorkOrder(workcell); //先按工位取
                    if (wipLineWorkOrder == null)
                    {
                        wipLineWorkOrder = GetWipResourceWorkOrder(workcell.ResourceId);
                        if (wipLineWorkOrder == null)
                        {
                            throw new ValidationException("[{0}]产线无当前在制工单".L10nFormat(barcode));
                        }
                    }

                    return GetById<WorkOrderMove>(wipLineWorkOrder.WorkOrderId); //Barcode关联的是Core的工单，要取MES工单
                case BarcodeType.CombinedCode:
                    {
                        double? workOrderId = 0;

                        var panel = RT.Service.Resolve<PanelController>().GetPanel(barcode.Code);
                        if (panel == null)
                        {
                            Barcode snCombined = RT.Service.Resolve<BarcodeController>().GetBarcode(barcode.Code);
                            if (snCombined == null)
                            {
                                throw new ValidationException(ORDER_NOT_FOUND_FORMAT.L10nFormat(barcode));
                            }
                            workOrderId = snCombined.WorkOrderId;
                            barcode.BarcodeType = BarcodeType.SN;
                        }
                        else
                            workOrderId = panel.WorkOrderId;
                        return GetById<WorkOrderMove>(workOrderId);
                    }
                default:
                    throw new ValidationException("[{0}]只有{1}和{2}才可以自动关联工单".L10nFormat(barcode, BarcodeType.SN.ToLabel(), BarcodeType.TurnoverBox.ToLabel()));
            }
        }

        /// <summary>
        /// 暂停运行时产品
        /// </summary>
        /// <param name="barcode">采集条码</param>
        /// <param name="workcell">工作单元</param> 
        /// <exception cref="ValidationException">产品已在前工序暂停、产品不存在</exception>
        /// <exception cref="ArgumentNullException">采集条码为空、工作单元为空</exception>
        public virtual void Hold(CollectBarcode barcode, Workcell workcell)
        {
            if (barcode == null)
            {
                throw new ArgumentNullException(nameof(barcode));
            }
            if (workcell == null)
            {
                throw new ArgumentNullException(nameof(workcell));
            }
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var product = RuntimeController.FindProduct(barcode);
                if (product == null)
                {
                    throw new ValidationException("找不到[{0}]对应的产品".L10nFormat(barcode));
                }

                if (product.IsHold)
                {
                    throw new ValidationException("产品已在前工序暂停".L10N());
                }

                product.IsHold = true;
                RuntimeController.Save(product);

                var wipProduct = Query<WipProduct>().Where(x => x.Puid == product.Puid).FirstOrDefault();
                if (wipProduct != null)
                {
                    wipProduct.IsHold = true;
                    wipProduct.CurrentVersion.IsHold = true;
                    RF.Save(wipProduct.CurrentVersion);
                    RF.Save(wipProduct);
                }

                tran.Complete();
            }
        }

        /// <summary>
        /// 取消暂停运行时产品
        /// </summary>
        /// <param name="barcode">采集条码</param>
        /// <param name="workcell">工作单元</param>
        /// <exception cref="ValidationException">产品不是暂停状态、产品不存在</exception>
        /// <exception cref="ArgumentNullException">采集条码为空、工作单元为空</exception>
        public virtual void UnHold(CollectBarcode barcode, Workcell workcell)
        {
            if (barcode == null)
            {
                throw new ArgumentNullException(nameof(barcode));
            }
            if (workcell == null)
            {
                throw new ArgumentNullException(nameof(workcell));
            }
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var product = RuntimeController.FindProduct(barcode);
                if (product == null)
                {
                    throw new ValidationException("找不到[{0}]对应的产品".L10nFormat(barcode));
                }

                if (!product.IsHold)
                {
                    throw new ValidationException("产品不是暂停状态".L10N());
                }

                product.IsHold = false;
                RuntimeController.Save(product);

                var wipProduct = Query<WipProduct>().Where(x => x.Puid == product.Puid).FirstOrDefault();
                if (wipProduct != null)
                {
                    wipProduct.IsHold = false;
                    wipProduct.CurrentVersion.IsHold = false;
                    RF.Save(wipProduct.CurrentVersion);
                    RF.Save(wipProduct);
                }

                tran.Complete();
            }
        }

        /// <summary>
        /// 跳到指定站
        /// </summary>
        /// <param name="barcode">采集条码</param>
        /// <param name="toRoutingProcessId">目标工序(工序清单中的工序ID)</param>
        /// <param name="workcell">工作单元</param> 
        /// <exception cref="ValidationException">产品未上线，已下线、产品工艺路线不存在跳站工序</exception>
        /// <exception cref="EntityNotFoundException">工序不存在</exception>
        protected virtual void Goto(CollectBarcode barcode, double toRoutingProcessId, Workcell workcell)
        {
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var product = RuntimeController.FindProduct(barcode);
                if (product == null)
                {
                    var version = FindLastWipProductVersion(barcode);
                    if (version == null)
                    {
                        throw new ValidationException("[{0}]未上线,不能跳站".L10nFormat(barcode));
                    }
                    if (version.IsFinish)
                    {
                        throw new ValidationException("[{0}]已下线,不能跳站".L10nFormat(barcode));
                    }
                    product = RecoverProduct(version);
                }

                var fromProcessId = product.Routing.Current.ProcessId.Value;

                var toProcess = product.Routing.Processes.FirstOrDefault(p => p.Id == toRoutingProcessId);

                if (toProcess == null || !toProcess.ProcessId.HasValue)
                {
                    throw new ValidationException("跳站失败,当前产品的工艺路线中找不到工序[{0}]"
                        .L10nFormat(toRoutingProcessId));
                }

                product.Routing.Next.Clear();

                toProcess.IsPass = false;

                product.Routing.Next.Add(toProcess.Id);

                if (!string.IsNullOrEmpty(toProcess.GroupId))
                {
                    //同工序组下的所有没有通过（Pass）的工序,也添加到后工序列表中                    
                    var processesOfGroup = product.Routing.Processes
                        .Where(x => x.GroupId == toProcess.GroupId
                            && x.IsGroup != true
                            && !x.IsPass && x.Id != toRoutingProcessId).ToList();

                    //工序组下所有工序都通过了，则工序组下所有工序都加到后工序列表中
                    if (!product.Routing.Processes
                        .Any(x => !x.IsPass && x.IsGroup != true && x.GroupId == toProcess.GroupId))
                    {
                        processesOfGroup = product.Routing.Processes
                          .Where(x => x.GroupId == toProcess.GroupId && x.IsGroup != true
                            && x.Id != toRoutingProcessId).ToList();
                    }

                    foreach (var processUnderGroup in processesOfGroup)
                    {
                        //之前通过了，改成非通过
                        if (processUnderGroup.IsPass)
                        {
                            processUnderGroup.IsPass = false;
                        }

                        if (!product.Routing.Next.Contains(processUnderGroup.Id))
                        {
                            product.Routing.Next.Add(processUnderGroup.Id);
                        }
                    }
                }

                UpdateVersionNextProcess(barcode, toProcess.ProcessId.Value);
                LogGoto(fromProcessId, toProcess.ProcessId.Value, workcell);

                //保存采集运行时产品
                RuntimeController.Save(product);

                tran.Complete();
            }
        }

        /// <summary>
        /// 更新下一工序
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="toProcessId">下一工序ID</param>
        private void UpdateVersionNextProcess(CollectBarcode barcode, double toProcessId)
        {
            //更新下一工序字段
            var wipProductVersion = RT.Service.Resolve<WipProductVersionController>().GetWipProductVersion(barcode);
            if (wipProductVersion != null)
            {
                wipProductVersion.NextProcessId = toProcessId;
                RF.Save(wipProductVersion);
            }
        }

        /// <summary>
        /// 记录跳站操作日志
        /// </summary>
        /// <param name="fromProcessId">来源工序ID</param>
        /// <param name="toProcessId">目标工序ID</param>
        /// <param name="workcell">工作单元</param>
        protected virtual void LogGoto(double fromProcessId, double toProcessId, Workcell workcell)
        {
            var log = new WipGotoLog()
            {
                FromProcessId = fromProcessId,
                ToProcessId = toProcessId,
                UserId = workcell.EmployeeId,
                LogDate = DateTime.Now
            };
            RF.Save(log);
        }

        /// <summary>
        /// 添加关键件到工序过站记录，添加物料属性到产品属性
        /// </summary>
        /// <param name="wipProductProcess">生产采集记录</param>
        /// <param name="loadItem">上料</param>
        /// <param name="qty">数量</param>
        public virtual void AddKeyItem(WipProductProcess wipProductProcess, LoadItem loadItem, decimal qty)
        {
            if (wipProductProcess == null)
            {
                throw new EntityNotFoundException(nameof(wipProductProcess));
            }

            if (loadItem == null)
            {
                throw new EntityNotFoundException(nameof(loadItem));
            }

            if (wipProductProcess.Id == default(double))
            {
                throw new EntityNotFoundException("【工序过站记录】的ID为空");
            }

            var wipProductProcessKeyItem = new WipProductProcessKeyItem
            {
                Item = loadItem.Item,
                SourceCode = loadItem.SourceCode,
                SourceId = loadItem.SourceId,
                SourceType = loadItem.SourceType,
                Qty = qty,
                ItemExtProp = loadItem.ItemExtProp,
                ItemExtPropName = loadItem.ItemExtPropName,
            };

            wipProductProcessKeyItem.ProcessId = wipProductProcess.Id;
            RF.Save(wipProductProcessKeyItem);
        }

        /// <summary>
        /// 验证新产品条码
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="workOrderId">工单ID</param>
        public virtual decimal ValidateNewBarcode(string barcode, double workOrderId)
        {
            if (barcode == null)
            {
                throw new ArgumentNullException(nameof(barcode));
            }
            var sn = RT.Service.Resolve<BarcodeController>().GetBarcode(barcode);
            if (sn == null)
            {
                throw new ValidationException(NOT_EXISTS_FORMAT.L10nFormat(barcode));
            }
            if (sn.IsScraped)
            {
                throw new ValidationException("[{0}]已经报废".L10nFormat(barcode));
            }
            if (sn.IsPending)
            {
                throw new ValidationException("[{0}]已经挂起".L10nFormat(barcode));
            }
            if (sn.WorkOrderId != workOrderId)
            {
                throw new ValidationException("产品[{0}]与当前生产工单不匹配".L10nFormat(barcode));
            }
            var product = RuntimeController.FindProduct(barcode);
            if (product != null)
            {
                throw new ValidationException("待绑定的条码必须是未上线条码".L10N());
            }
            return sn.Qty;
        }

        /// <summary>
        /// 获取条码数量
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <returns>数量</returns>
        protected virtual decimal GetBarcodeQty(CollectBarcode barcode)
        {
            decimal qty = 0;
            string code = barcode.Code;
            switch (barcode.Type)
            {
                case BarcodeType.SN:
                    qty = RT.Service.Resolve<BarcodeController>().GetBarcodeQty(code);
                    break;
                case BarcodeType.CombinedCode:
                    qty = RT.Service.Resolve<PanelBindingController>().GetActualPanelQty(code, barcode.BarcodeType);
                    break;
                case BarcodeType.KeyLabel:
                    qty = 1;
                    break;
            }
            return qty;
        }

        /// <summary>
        /// 生产条码归属工单的运行时前置检查
        /// </summary>
        /// <param name="sn"></param>
        public virtual void BarcodeBelongWorkOrderCheck(string sn)
        {
            var collectBarcode = new CollectBarcode(sn, BarcodeType.SN);

            var product = RuntimeController.FindProduct(collectBarcode);

            if (product != null)
            {
                throw new ValidationException("产品[{0}]归属失败，产品未完工下线".L10nFormat(sn));
            }

            var wipProduct = GetWipProduct(collectBarcode);

            if (wipProduct == null)
            {
                throw new ValidationException("产品[{0}]归属失败，产品未上线".L10nFormat(sn));
            }
        }


        /// <summary>
        /// 添加缺陷记录
        /// </summary>
        /// <param name="wipProductProcess">产品工序采集记录</param>
        /// <param name="product">产品</param>
        /// <param name="collectData">采集结果</param>
        /// <param name="workcell">工作单元</param>
        protected virtual void AddDefects(WipProductProcess wipProductProcess,
            product product, CollectData collectData, Workcell workcell)
        {
            if (product == null)
            {
                throw new EntityNotFoundException(nameof(product));
            }

            if (collectData == null)
            {
                throw new EntityNotFoundException(nameof(collectData));
            }

            if (workcell == null)
            {
                throw new EntityNotFoundException(nameof(workcell));
            }

            if (wipProductProcess == null)
            {
                throw new EntityNotFoundException(nameof(wipProductProcess));
            }

            bool isVictory = product.Routing.Current.InInning;
            foreach (var defect in collectData.Defects)
            {
                WipProductDefect wipProductDefect = new WipProductDefect
                {
                    DefectId = defect.DefectId,
                    Location = defect.Location,
                    NgQty = defect.Qty.ConvertTo<decimal>(),
                    Remark = defect.Remark,
                    Result = collectData.Result,
                    StationId = workcell.StationId,
                    ProcessId = workcell.ProcessId,
                    ResourceId = workcell.ResourceId,
                    ShiftId = wipProductProcess.ShiftId,
                    IsMisjudgment = isVictory,
                    BoardNo = defect.BoardNo,
                    Sn = defect.Sn,
                };

                wipProductDefect.VersionId = wipProductProcess.VersionId;
                RF.Save(wipProductDefect);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="workcell"></param>
        /// <param name="packageNoList"></param>
        /// <param name="isAdvance"></param>
        /// <returns></returns>
        public virtual string DoPackage(string sn, Workcell workcell, object packageNoList, object isAdvance)
        {
            return string.Empty;
        }

        /// <summary>
        /// 包装采集
        /// </summary>
        /// <param name="barcodes"></param>
        /// <param name="collectData"></param>
        /// <param name="workcell"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual ProductInfo PkgCollect(string[] barcodes, CollectData collectData, Workcell workcell)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            if (barcodes == null || barcodes?.Length <= 0)
            {
                throw new ArgumentNullException(nameof(barcodes));
            }
            if (collectData == null)
            {
                throw new ArgumentNullException(nameof(collectData));
            }
            if (workcell == null)
            {
                throw new ArgumentNullException(nameof(workcell));
            }

            ProductInfo productInfo;

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                using (PerformenceTracer.Start("新包装采集打包【Move()】总用时"))
                {
                    productInfo = Move(barcodes, collectData, workcell);
                }

                tran.Complete();
            }
            stopwatch.Stop();
            Context.DistributionContext.GlobalContext["CollectTime"] = stopwatch.ElapsedMilliseconds;
            return productInfo;
        }


    }
}