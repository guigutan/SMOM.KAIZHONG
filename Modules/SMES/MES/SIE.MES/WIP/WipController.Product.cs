using SIE.Barcodes;
using SIE.Barcodes.Panels;
using SIE.Common;
using SIE.Core.Barcodes;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.BarcodeProcesses;
using SIE.MES.PanelBindings;
using SIE.MES.WIP.Assemblys;
using SIE.MES.WIP.Models;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.Runtime;
using SIE.MES.WorkOrders;
using SIE.Packages.ItemLabels;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.WIP
{
    /// <summary>
    /// 在制品控制器
    /// </summary>
    partial class WipController
    {
        /// <summary>
        /// 查找最后一个产品版本
        /// </summary>
        /// <param name="barcode">采集条码</param>
        /// <param name="eagerLoadOptions"></param>
        /// <returns>生产产品版本</returns>
        protected virtual WipProductVersion FindLastWipProductVersion(CollectBarcode barcode, EagerLoadOptions eagerLoadOptions=null)
        {
            var query = Query<WipProductVersion>();
            switch (barcode.Type)
            {
                case BarcodeType.CSN:
                    query.Where(x => x.Csns == barcode.Code);
                    break;
                case BarcodeType.TurnoverBox:
                case BarcodeType.ContainerNo:
                    query.Where(x => x.BoxNo == barcode.Code);
                    break;
                case BarcodeType.SN:
                case BarcodeType.BatchBarocde:
                    query.Where(x => x.Sn == barcode.Code);
                    break;
                case BarcodeType.KeyLabel:
                    query.Where(x => x.KeyLabel == barcode.Code);
                    break;
                case BarcodeType.CombinedCode:
                    query.Where(x => x.CombinedCode == barcode.Code);
                    break;
                default:
                    break;
            }

            return query.OrderByDescending(p => p.CreateDate).FirstOrDefault(eagerLoadOptions);
        }

        /// <summary>
        /// 查找最后一次生产产品版本
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <returns>生产产品版本</returns>
        protected virtual WipProductVersion FindLastWipProductVersion(string barcode)
        {
            return Query<WipProductVersion>().Where(p => p.Sn == barcode || p.Csns == barcode || p.BoxNo == barcode || p.KeyLabel == barcode || p.CombinedCode == barcode).OrderByDescending(p => p.CreateDate).FirstOrDefault();
        }

        /// <summary>
        /// 获取产品生产版本
        /// </summary>
        /// <param name="sns">单体条码列表</param>
        /// <returns>产品生产版本</returns>
        protected virtual EntityList<WipProductVersion> GetCurrentWipProductVersionsBySnList(List<string> sns)
        {
            var puidKeys = sns.Select(x => RT.Service.Resolve<RuntimeController>().CreatePuidKey(x, BarcodeType.SN));

            return puidKeys.SplitContains(tempPuidKeys =>
            {
                return Query<WipProductVersion>()
                    .Join<WipProduct>((x, y) => y.CurrentVersionId == x.Id)
                    .Join<WipProduct, PuidMap>((a, b) => a.Puid == b.Puid)
                    .Where<PuidMap>((x, y) => tempPuidKeys.Contains(y.Id))
                .ToList();
            });
        }

        /// <summary>
        /// 获取产品生产版本
        /// </summary>
        /// <param name="sns">单体条码列表</param>
        /// <returns>产品生产版本</returns>
        protected virtual EntityList<WipProductVersion> GetWipProductVersionsBySnList(List<string> sns)
        {
            return sns.SplitContains(tempSns =>
            {
                return Query<WipProductVersion>().Where(x => tempSns.Contains(x.Sn))
                .ToList();
            });
        }

        /// <summary>
        /// 获取产品生产版本
        /// </summary>
        /// <param name="puid">产品ID</param>
        /// <returns>产品生产版本</returns>
        public virtual WipProductVersion GetWipProductVersion(string puid)
        {
            // 减少对WIP_PRODUCT的一次查询
            return Query<WipProductVersion>()
                .Join<WipProduct>((x, y) => y.Puid == puid && y.CurrentVersionId == x.Id)
                .FirstOrDefault();
        }

        /// <summary>
        /// 更新产品等级，如果为报废，则下线
        /// </summary>
        /// <param name="wipProduct">生产产品</param>
        /// <param name="version">产品版本</param>
        /// <param name="product">运行时产品</param>
        /// <param name="barcodes">采集条码集合</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元</param>
        protected virtual void UpdateProductGrade(WipProduct wipProduct, WipProductVersion version, product product, IList<CollectBarcode> barcodes, CollectData collectData, Workcell workcell)
        {
            var grade = collectData.Grade.Value;
            wipProduct.Grade = grade;
            version.Grade = grade;
            if (collectData.Grade.Value == ProductGrade.Scrap)
            {
                DateTime dateTime = RF.Find<WipProduct>().GetDbTime();
                wipProduct.State = WipProductState.Finish;
                version.IsFinish = true;
                version.FinishDateTime = dateTime;
                version.NextProcess = null;
                version.CanScrapReuse = collectData.CanScrapReuse;
                version.IsScrapped = true;
                product.NgQty += product.Qty;
                RuntimeController.RemoveProduct(product); ////报废下线 
                var reason = collectData.Context["ScrapReason"]?.ToString();
                BarcodeScrap(barcodes[0], reason);
                var data = new CollectEventData(product, barcodes.ToArray(), collectData, workcell, dateTime);
                OnScraped(data);
            }
        }

        /// <summary>
        /// 条码报废
        /// </summary>
        /// <param name="barcode">产品条码</param>
        /// <param name="reason">报废原因</param>
        protected virtual void BarcodeScrap(CollectBarcode barcode, string reason)
        {
            switch (barcode.Type)
            {
                case BarcodeType.SN:
                    {
                        RT.Service.Resolve<BarcodeController>().BarcodeScrap(new List<string>() { barcode.Code }, "产品报废");
                    }
                    break;
                case BarcodeType.CSN:
                    break;
                case BarcodeType.TurnoverBox:
                    break;
                case BarcodeType.KeyLabel:
                    break;
                case BarcodeType.BatchBarocde:
                    break;
                case BarcodeType.ContainerNo:
                    break;
                case BarcodeType.CombinedCode:
                    {
                        RT.Service.Resolve<PanelController>().PanelScrap(barcode.Code, reason);
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 产品维修后,标记产品维修过
        /// </summary>
        /// <param name="wipProduct">生产产品</param>
        /// <param name="version">生产产品版本</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元</param>
        protected virtual void ProductFixed(WipProduct wipProduct, WipProductVersion version, CollectData collectData, Workcell workcell)
        {
            if (collectData.Result == ResultType.Pass)
            {
                wipProduct.IsFixed = true;
                version.IsFixed = true;
            }
        }

        /// <summary>
        /// 记录采集记录
        /// </summary>
        /// <param name="product">运行时产品</param>
        /// <param name="version">生产产品版本</param>
        /// <param name="barcodes">采集条码集合</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="wipResourceMove">生产资源</param>
        /// <returns>生产采集记录</returns>
        protected virtual WipProductProcess CreateWipProductProcess(product product, WipProductVersion version,
            IList<CollectBarcode> barcodes, CollectData collectData, Workcell workcell, WipResourceMove wipResourceMove)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            if (workcell == null)
            {
                throw new ArgumentNullException(nameof(workcell));
            }

            if (collectData == null)
            {
                throw new ArgumentNullException(nameof(collectData));
            }

            if (barcodes == null )
            {
                throw new ArgumentNullException(nameof(collectData));
            }

            var collectBarcodeLast = barcodes.LastOrDefault();
            if (collectBarcodeLast == null)
            {
                throw new ArgumentNullException(nameof(barcodes));
            }

            if (version == null)
            {
                throw new ArgumentNullException(nameof(version));
            }

            var currentTime = RF.Find<WipProductProcess>().GetDbTime();
            var shift = RT.Service.Resolve<WipResourceController>()
                .GetWipResourceShift(workcell.ResourceId, currentTime, wipResourceMove);

            WipProductProcess wipProductProcess = new WipProductProcess();
            wipProductProcess.GenerateId();
            wipProductProcess.ProcessId = workcell.ProcessId;
            wipProductProcess.ResourceId = workcell.ResourceId;
            wipProductProcess.Result = collectData.Result;
            wipProductProcess.StationId = workcell.StationId;
            wipProductProcess.OperateById = workcell.EmployeeId;
            wipProductProcess.OperateTime = currentTime;
            wipProductProcess.Shift = shift;
            wipProductProcess.Barcode = collectBarcodeLast.Code;
            wipProductProcess.State = collectData.State;
            wipProductProcess.Version = version;

            var wipProduct = version.Product;

            foreach (var barcode in barcodes)
            {
                switch (barcode.Type)
                {
                    case BarcodeType.SN:
                        version.Sn = barcode.Code;
                        version.CombinedCode = barcode.PanelCode.IsNotEmpty() ? barcode.PanelCode : version.CombinedCode;
                        break;
                    case BarcodeType.CSN:
                        version.Csns = barcode.Code;
                        break;
                    case BarcodeType.KeyLabel:
                        version.KeyLabel = barcode.Code;
                        break;
                    case BarcodeType.TurnoverBox:
                        version.BoxNo = barcode.Code;
                        break;
                    case BarcodeType.CombinedCode:
                        version.CombinedCode = barcode.Code;
                        break;
                    default:
                        break;
                }
            }

            if (collectData.State == WipProductProcessState.Finish)
            {
                wipProduct.Result = collectData.Result;
                product.NgQty += collectData.NgQty;
                wipProduct.NgQty = product.NgQty;

                //更新产品等级和报废状态
                if (collectData.Grade.HasValue)
                {
                    UpdateProductGrade(wipProduct, version, product, barcodes, collectData, workcell);
                }
                var process = product.Routing.Current;
                if (process == null)
                {
                    throw new ValidationException("当前工序不存在，请检查".L10N()); 
                }
                //维修
                if (process.Type == ProcessType.Fix)
                {
                    ProductFixed(wipProduct, version, collectData, workcell);
                }

                //设置产品不良
                if ((process.Type == ProcessType.Pqc /*|| process.Type == ProcessType.Fqc*/)
                    && collectData.Result == ResultType.Fail)
                {
                    product.IsNg = true;
                }
            }

            if (collectData.State == WipProductProcessState.Finish && product.Routing.Current.CreateSku)
            {
                var item = product.WorkOrderMove.Product;
                RT.Service.Resolve<ItemLabelController>().CreateItemLabel(item, product.Qty - product.NgQty, barcodes.Last().Code,
                    LabelSource.Wip, product.WorkOrderId, product.WorkOrderMove.FactoryId, product.WorkOrderMove.ItemExtProp,
                    product.WorkOrderMove.ItemExtPropName, product.WorkOrderMove.ProjectMaintain?.Code);
            }

            version.CurrentProcessId = wipProductProcess.Id;
            version.NowProcessId = wipProductProcess.ProcessId;
            //改成不提前保存，局数判断时传当前工序过站记录
            //RF.Save(wipProductProcess);  //检验工序会有局数判断，需要使用到采集结果，先保存

            return wipProductProcess;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="collectBarcodes"></param>
        /// <param name="recordId"></param>
        /// <param name="processId"></param>
        /// <returns></returns>
        private EntityList<WipProProcessEmployee> CreateWipProProcessEmployee(IList<CollectBarcode> collectBarcodes, double recordId,double processId)
        {
            EntityList<WipProProcessEmployee> employees = new EntityList<WipProProcessEmployee>();

            foreach (var barcode in collectBarcodes)
            {
                if (barcode.Type != BarcodeType.SN)
                {
                    continue;
                }
                var detail = Query<BarcodeProDetail>().Exists<BarcodeProcess>((x, y) => y.Where(p => x.BarcodeProcessId == p.Id && p.Sn == barcode.Code)).Where(b => b.ProcessId == processId).FirstOrDefault();
                if (detail == null)
                {
                    return employees;
                }
                var employeeIdsList = detail.EmployeeIds.Split(',').ToList();
                foreach (var employeeId in employeeIdsList)
                {
                    if (double.TryParse(employeeId, out double empId))
                    {
                        var wipEmployee = new WipProProcessEmployee
                        {
                            WipProductProcessId = recordId,
                            EmployeeId = empId,
                        };
                        employees.Add(wipEmployee);
                    }
                }
            }
            return employees;
        }

        /// <summary>
        /// 过站记录创建后，重写此方法保存过站记录额外的数据
        /// </summary>
        /// <param name="wipProductProcess">生产采集记录</param>
        /// <param name="product">运行时产品</param>
        /// <param name="collectBarcodes">采集条码集合</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元</param>
        protected virtual void OnWipProductProcessFinished(WipProductProcess wipProductProcess, product product,
            IList<CollectBarcode> collectBarcodes, CollectData collectData, Workcell workcell)
        {
            //添加缺陷
            AddDefects(wipProductProcess, product, collectData, workcell);
        }

        /// <summary>
        /// 拼板码解绑(解绑前是以拼板码流转，解绑需要克隆运行时到条码)
        /// </summary>
        /// <param name="version">生产产品版本</param>
        /// <param name="wipProductProcess">生产工序记录</param>
        /// <param name="product">运行时产品</param>
        /// <param name="barcodes">采集条码</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元</param>
        protected virtual void PanelBindingOnProcess(WipProductVersion version,
            WipProductProcess wipProductProcess,
            product product,
            IList<CollectBarcode> barcodes,
            CollectData collectData, Workcell workcell)
        {
            if (barcodes[0].Type != BarcodeType.CombinedCode)
            {
                return;
            }

            if (product.Routing.Current.IsBinding)
            {
                bool toBeManualBinding = collectData.CombinedCode.ToBeManualBinding;

                if (toBeManualBinding)
                {
                    throw new UnBindingSnException("拼板码未绑定SN".L10N());
                }

                List<BindingSn> bindingSns = collectData.CombinedCode.BindingSns;

                //自动绑定，生成SN条码
                bool isAuto = collectData.CombinedCode.AutoCreateAndBinding;
                if (isAuto && bindingSns.Count == 0)
                {
                    //生成条码 
                    var workOrder = product.WorkOrder;
                    var info = new BarcodePrintInfo()
                    {
                        WorkOrderId = workOrder.Id,
                        NumberRuleId = workOrder?.Template?.NumberRuleId ?? 0,
                        PrintTemplateId = workOrder?.Template?.LabelTemplateId ?? 0,
                        SingleQty = 1,
                    };

                    if ((int)workOrder.PlanQty < workOrder.PrintedQty)
                    {
                        throw new ValidationException("数据异常，工单[{0}]的计划数量[{1}]小于已打印数量[{2}]！"
                            .L10nFormat(workOrder.No, workOrder.PlanQty, workOrder.PrintedQty));
                    }

                    var canPrintQty = (int)workOrder.PlanQty - workOrder.PrintedQty;

                    if (collectData.CombinedCode.ToBindingQty > canPrintQty)
                    {
                        throw new ValidationException("数据异常，绑定条码数量[{0}]超过工单计划数量[{1}]，无法生成生产条码！"
                            .L10nFormat(workOrder.PrintedQty + collectData.CombinedCode.ToBindingQty, workOrder.PlanQty));
                    }

                    info.PrintQty = collectData.CombinedCode.ToBindingQty;

                    var printedBarcodes = RT.Service.Resolve<BarcodeController>().Print(info);
                    var snList = printedBarcodes.Select(p => new BindingSn()
                    {
                        Sn = p.Sn,
                        Qty = p.Qty
                    });

                    bindingSns.AddRange(snList);

                    RT.Service.Resolve<WipProductVersionController>()
                        .AddToBindingSnPrintRecord(workOrder.Id, barcodes[0].Code, printedBarcodes);
                }

                if (bindingSns.Any())
                {
                    var panelCode = barcodes[0].Code;

                    var panel = RT.Service.Resolve<PanelController>().GetPanel(panelCode);

                    Check.NotNull(panel, "拼板码不能为空".L10N());

                    List<int> boardNoList = new List<int>();

                    var boardNos = panel.ForkPlate.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    boardNos.ForEach(boardno =>
                    {
                        if (!int.TryParse(boardno, out int res))
                            throw new ValidationException("叉板板号格式错误，格式：1  3".L10N());
                        boardNoList.Add(res);
                    });

                    var workOrder = product.WorkOrder;
                    var canBindQty = workOrder.PanelQty;

                    //组合板工单的可绑定产品数量：工单拼板数 乘以 PCB物料属性明细中每个子产品的数量
                    if (workOrder.IsPanelWorkOrder)
                    {
                        canBindQty =
                            RT.Service.Resolve<PanelBindingController>().GetPanelWorkOrderCanBindingQty(workOrder);
                    }

                    RT.Service.Resolve<PanelBindingController>().PanelBindingSn(
                        bindingSns, boardNoList, panelCode, panel.ForkPlateQty, true, workOrder, canBindQty);

                    product.Qty = bindingSns.Count;
                    version.Product.BatchQty = bindingSns.Count;
                }
            }

            //拼板码解绑，解绑前都是按拼板码流转，解绑时需克隆运行到每个条码
            if (product.Routing.Current.IsUnBinding)
            {
                //存在运行时,子产品单体条码复制拼板码的运行时(组合板工单不复制运行时)
                if (product != null && collectData.CombinedCode.BindingSns.Any())
                {
                    var panelAndBarcodes = RT.Service.Resolve<PanelBindingController>().GetPanelAndBarcodesByPanleCode(barcodes[0].Code);

                    panelAndBarcodes.ForEach(x =>
                    {
                        x.IsBinding = false;
                        //将拼板码的运行时克隆到每个条码
                        var childCollectBarcode = new CollectBarcode() { Code = x.SN, Type = BarcodeType.SN, PanelCode = barcodes[0].Code };
                        CloneProduct(childCollectBarcode, product, x.Qty, version, x.WorkOrderId);
                    });

                    RF.Save(panelAndBarcodes);

                    //拼版码的版本标记完成并将对应运行时删除
                    version.Product.State = WipProductState.Finish;
                    version.IsFinish = true;
                    version.FinishDateTime = DateTime.Now;
                    version.NextProcess = null;
                    RuntimeController.RemoveProduct(product);

                }
            }
        }

        /// <summary>
        /// 创建在制产品
        /// </summary>
        /// <param name="product">运行时产品</param>
        /// <returns>生产产品</returns>
        protected virtual WipProduct CreateWipProduct(product product)
        {
            var wipProduct = new WipProduct()
            {
                BatchQty = product.Qty,
                Grade = ProductGrade.A,
                State = WipProductState.Producing,
                ItemId = product.ItemId,
                ItemExtProp = product.ItemExtProp,
                ItemExtPropName = product.ItemExtPropName,
                Puid = product.Puid,
                Result = ResultType.Pass,
                NgQty = product.NgQty
            };
            RF.Save(wipProduct);
            return wipProduct;
        }

        /// <summary>
        /// 创建在制产品版本
        /// </summary>
        /// <param name="wipProduct">生产产品</param>
        /// <param name="product">运行时产品</param>
        /// <param name="barcode">采集条码</param>
        /// <param name="versionOfSource">来源运行时</param>
        /// <returns>生产产品版本</returns>
        protected virtual WipProductVersion CreateWipProductVersion(WipProduct wipProduct, product product, CollectBarcode barcode,
            WipProductVersion versionOfSource = null)
        {
            WipProductVersion version = new WipProductVersion()
            {
                Product = wipProduct,
                WorkOrderId = product.WorkOrderId,
                Grade = ProductGrade.A,
                ItemExtProp = wipProduct.ItemExtProp,
                ItemExtPropName = wipProduct.ItemExtPropName,
            };
            version.GenerateId();

            switch (barcode.Type)
            {
                case BarcodeType.SN:
                    version.Sn = barcode.Code;
                    version.CombinedCode = barcode.PanelCode.IsNotEmpty() ? barcode.PanelCode : version.CombinedCode;
                    break;
                case BarcodeType.CSN:
                    version.Csns = barcode.Code;
                    break;
                case BarcodeType.KeyLabel:
                    version.KeyLabel = barcode.Code;
                    break;
                case BarcodeType.TurnoverBox:
                    version.BoxNo = barcode.Code;
                    break;
                case BarcodeType.CombinedCode:
                    version.CombinedCode = barcode.Code;
                    break;
            }

            if (versionOfSource != null)
            {
                version.NextProcessId = versionOfSource?.NextProcessId;
                version.CombinedCode = versionOfSource?.CombinedCode;
            }

            RF.Save(version);
            wipProduct.CurrentVersion = version;
            RF.Save(wipProduct);
            return version;
        }

        /// <summary>
        /// 创建完工的产品版本
        /// </summary>
        /// <param name="wipProduct">生产产品</param>
        /// <param name="product">运行时产品</param>
        /// <param name="barcode">采集条码</param>
        /// <param name="versionOfSource">来源生产版本</param>
        /// <returns>生产产品版本</returns>
        protected virtual WipProductVersion CreateFinishWipProductVersion(WipProduct wipProduct, product product, CollectBarcode barcode,
            WipProductVersion versionOfSource = null)
        {
            WipProductVersion version = new WipProductVersion()
            {
                ProductId = 0,
                WorkOrderId = product.WorkOrderId,
                Grade = ProductGrade.A,
                IsFinish = true,
                FinishDateTime = DateTime.Now,
                ItemExtProp = wipProduct.ItemExtProp,
                ItemExtPropName = wipProduct.ItemExtPropName,
            };
            version.GenerateId();

            switch (barcode.Type)
            {
                case BarcodeType.SN:
                    version.Sn = barcode.Code;
                    break;
                case BarcodeType.CSN:
                    version.Csns = barcode.Code;
                    break;
                case BarcodeType.KeyLabel:
                    version.KeyLabel = barcode.Code;
                    break;
                case BarcodeType.TurnoverBox:
                    version.BoxNo = barcode.Code;
                    break;
                case BarcodeType.CombinedCode:
                    version.CombinedCode = barcode.Code;
                    break;
            }

            wipProduct.VersionList.Add(version);

            if (versionOfSource != null)
            {
                version.NextProcessId = versionOfSource?.NextProcessId;
                version.CombinedCode = versionOfSource?.CombinedCode;
            }

            RF.Save(version);
            wipProduct.CurrentVersion = version;
            RF.Save(wipProduct);
            return version;
        }

        /// <summary>
        /// 切换产线在制工单
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="workcell">工作单元</param>
        /// <exception cref="EntityNotFoundException">工单不存在</exception>
        /// <exception cref="ArgumentNullException">工作单元为空</exception>
        /// <returns>工单</returns>
        public virtual WorkOrder ChangeWipResourceWorkOrder(double workOrderId, Workcell workcell)
        {
            if (workcell == null)
                throw new ArgumentNullException(nameof(workcell));
            var workOrder = GetById<WorkOrder>(workOrderId);
            if (workOrder == null)
                throw new EntityNotFoundException(typeof(WorkOrder), workOrderId);
            var wipLineWorkOrderEntity = GetWipResourceWorkOrder(workcell);
            if (wipLineWorkOrderEntity == null)
                wipLineWorkOrderEntity = CreateWipResourceWorkOrder(workOrder, workcell);
            else
            {
                wipLineWorkOrderEntity.WorkOrder = workOrder;
                RF.Save(wipLineWorkOrderEntity);
            }

            OnChangeWipResourceWorkOrder(wipLineWorkOrderEntity.WorkOrder);
            return wipLineWorkOrderEntity.WorkOrder;
        }

        /// <summary>
        /// 创建产线在制工单，工单首件上线时由RFID等投入生成，不知道工单，只能通过上料关联生产工单
        /// </summary>
        /// <param name="wo">工单</param>
        /// <param name="workcell">工作单元</param>
        /// <returns>产线在生产工单</returns>
        protected virtual WipResourceWorkOrder CreateWipResourceWorkOrder(WorkOrder wo, Workcell workcell)
        {
            WipResourceWorkOrder wipLineWorkOrder = new WipResourceWorkOrder();
            wipLineWorkOrder.ResourceId = workcell.ResourceId;
            wipLineWorkOrder.StationId = workcell.StationId;
            wipLineWorkOrder.ProcessId = workcell.ProcessId;
            wipLineWorkOrder.WorkOrder = wo;
            RF.Save(wipLineWorkOrder);
            return wipLineWorkOrder;
        }

        /// <summary>
        /// 获取产线工序工位在生产的工单
        /// </summary>
        /// <param name="workcell">工作单元</param>
        /// <returns>产线在生产工单</returns>
        /// <exception cref="ArgumentNullException">工作单元为空</exception>
        public virtual WipResourceWorkOrder GetWipResourceWorkOrder(Workcell workcell)
        {
            if (workcell == null)
            {
                throw new ArgumentNullException(nameof(workcell));
            }

            return Query<WipResourceWorkOrder>()
                .Where(f => f.ResourceId == workcell.ResourceId && f.ProcessId == workcell.ProcessId && f.StationId == workcell.StationId)
                .FirstOrDefault();
        }

        /// <summary>
        /// 通过资源获取当前产线正在生产的工单
        /// </summary>
        /// <param name="resourceId">资源ID</param>
        /// <returns>产线在生产工单</returns>
        public virtual WipResourceWorkOrder GetWipResourceWorkOrder(double resourceId)
        {
            return Query<WipResourceWorkOrder>()
                .Where(f => f.ResourceId == resourceId)
                .OrderByDescending(f => f.UpdateDate)
                .FirstOrDefault();
        }

        /// <summary>
        /// 获取工序最新采集时间
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="processId">工序ID</param>
        /// <returns>采集时间</returns>
        public virtual DateTime? GetProcessNewestCollectedDate(CollectBarcode barcode, double workOrderId, double processId)
        {
            var res = Query<WipProductProcess>()
             .Join<WipProductVersion>((p, v) => p.VersionId == v.Id && v.WorkOrderId == workOrderId && v.Sn == barcode.Code)
             .Where(p => p.ProcessId == processId)
             .OrderByDescending(p => p.OperateTime)
             .Select(p => p.OperateTime)
             .FirstOrDefault<DateTime>();
            if (res == DateTime.MinValue)
                return null;
            return res;
        }

        /// <summary>
        /// 产品条码条码再工序是否已扣料
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="sn">产品条码</param>
        /// <param name="processId">工序ID</param>
        /// <returns>工序已扣料返回true，未扣料返回false</returns>
        public virtual bool IsSnAlreadyBuckleMateria(double workOrderId, string sn, double processId)
        {
            return Query<WipProductProcessKeyItem>()
                 .Join<WipProductProcess>((k, p) => k.ProcessId == p.Id && p.ProcessId == processId)
                 .Join<WipProductProcess, WipProductVersion>((p, v) => p.VersionId == v.Id && v.Sn == sn && v.WorkOrderId == workOrderId)
                 .Join<WipProductVersion, WipProduct>((v, wp) => v.Id == wp.CurrentVersionId)
                 .Count() > 0;
        }

        /// <summary>
        /// 组合条码再工序是否已扣料
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="combinedCode">组合条码</param>
        /// <param name="processId">工序ID</param>
        /// <returns>工序已扣料返回true，未扣料返回false</returns>
        public virtual bool IsCombinedCodeAlreadyBuckleMateria(double workOrderId, string combinedCode, double processId)
        {
            return Query<WipProductProcessKeyItem>()
                 .Join<WipProductProcess>((k, p) => k.ProcessId == p.Id && p.ProcessId == processId)
                 .Join<WipProductProcess, WipProductVersion>((p, v) => p.VersionId == v.Id && v.CombinedCode == combinedCode && v.WorkOrderId == workOrderId)
                 .Join<WipProductVersion, WipProduct>((v, wp) => v.Id == wp.CurrentVersionId)
                 .Count() > 0;
        }

        /// <summary>
        /// 获取生产产品
        /// </summary>
        /// <param name="barcode">采集条码</param>
        /// <returns>生产产品版本</returns>
        protected virtual WipProduct GetWipProduct(CollectBarcode barcode)
        {
            var query = Query<WipProduct>();
            switch (barcode.Type)
            {
                case BarcodeType.CSN:
                    query.Join<WipProductVersion>((p, v) => p.CurrentVersionId == v.Id && v.Csns == barcode.Code);
                    break;
                case BarcodeType.TurnoverBox:
                case BarcodeType.ContainerNo:
                    query.Join<WipProductVersion>((p, v) => p.CurrentVersionId == v.Id && v.BoxNo == barcode.Code);
                    break;
                case BarcodeType.SN:
                case BarcodeType.BatchBarocde:
                    query.Join<WipProductVersion>((p, v) => p.CurrentVersionId == v.Id && v.Sn == barcode.Code);
                    break;
                case BarcodeType.KeyLabel:
                    query.Join<WipProductVersion>((p, v) => p.CurrentVersionId == v.Id && v.KeyLabel == barcode.Code);
                    break;
                case BarcodeType.CombinedCode:
                    query.Join<WipProductVersion>((p, v) => p.CurrentVersionId == v.Id && v.CombinedCode == barcode.Code);
                    break;
            }
            return query.FirstOrDefault();
        }
    }
}