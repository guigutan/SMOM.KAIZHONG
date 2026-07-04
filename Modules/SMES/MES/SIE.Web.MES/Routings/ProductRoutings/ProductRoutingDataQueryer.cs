using SIE.Barcodes;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MES.RoutingSettings;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.Runtime;
using SIE.MES.WorkOrders;
using SIE.Tech.Processs;
using SIE.Web.Data;
using System;
using System.Linq;

namespace SIE.Web.MES.ProductRoutings
{
    /// <summary>
    /// 产品工艺路线查询器
    /// </summary>
    public class ProductRoutingDataQueryer : DataQueryer
    {
        public EntityList<Barcode> GetBarcodes(BarcodeCriteria criteria)
        {
            return RT.Service.Resolve<BarcodeController>().GetBarcodes(criteria);
        }

        /// <summary>
        /// 启用产品工艺路线
        /// </summary>
        /// <param name="versionId">产品版本Id</param>
        /// <param name="oldLayout">旧布局</param>
        /// <param name="newLayout">新布局</param>
        public void EnableProductRouting(double versionId, string oldLayout, string newLayout)
        {
            RT.Service.Resolve<WipProductRoutingController>().EnableProductRouting(versionId, oldLayout, newLayout);
        }

        /// <summary>
        /// 暂停产品工艺路线
        /// </summary>
        /// <param name="versionId">产品版本ID</param>
        /// <param name="oldLayout">工艺路线旧布局</param>
        /// <param name="newLayout">工艺路线新布局</param>
        public void PauseProductRouting(double versionId, string oldLayout, string newLayout)
        {
            RT.Service.Resolve<WipProductRoutingController>().PauseProductRouting(versionId, oldLayout, newLayout);
        }

        /// <summary>
        /// 保存产品工艺路线
        /// </summary>
        /// <param name="versionId">产品版本Id</param>
        /// <param name="oldLayout">旧布局</param>
        /// <param name="newLayout">新布局</param>
        public void SaveProductRouting(double versionId, string oldLayout, string newLayout)
        {
            RT.Service.Resolve<WipProductRoutingController>().SaveProductRouting(versionId, oldLayout, newLayout);
        }

        /// <summary>
        /// 加载生产产品信息
        /// 数据有：生产产品版本、工艺路线变更事件、工艺路线布局
        /// ProductRoutingLayout.js调用
        /// </summary>
        /// <param name="workOrderId">工单id</param>
        /// <param name="sn">产品条码</param>
        /// <returns>生产产品信息</returns>
        public WipProductInfo LoadWipProductData(double workOrderId, string sn)
        {
            var info = new WipProductInfo();
            if (sn.IsNullOrEmpty())
                return info;
            var version = RT.Service.Resolve<WipProductVersionController>().GetWipProductVersionEagerLoadProcess(sn);
            info.WipProductVersion = version;
            if (version != null)
            {
                var eventList = RT.Service.Resolve<WipProductRoutingController>().GetWipProductRoutingEvents(version.Id);
                info.RoutingEventList.AddRange(eventList);
                info.Product = RT.Service.Resolve<RuntimeController>().FindProduct(version.Product.Puid);
            }
            GetRoutingLayout(info, workOrderId, version?.Id ?? 0);
            return info;
        }

        /// <summary>
        /// 获取工艺路线布局,如果产品工艺路线布局不为空则取产品工艺路线，否则取工单对应的工艺路线
        /// </summary>
        /// <param name="info"></param>
        /// <param name="workOrderId"></param>
        /// <param name="versionId"></param> 
        private void GetRoutingLayout(WipProductInfo info, double workOrderId, double versionId)
        {
            bool isWorkOrderLayout = true;
            string layout = string.Empty;
            var wipProductRouting = RT.Service.Resolve<WipProductRoutingController>().GetWipProductRouting(versionId);
            if (wipProductRouting != null && wipProductRouting.Layout != null)
            {
                isWorkOrderLayout = false;
                layout = wipProductRouting.Layout.Layout;
            }
            else
            {
                if (workOrderId <= 0)
                {
                    throw new ValidationException("未找到产品条码对应工单信息".L10N());
                }

                layout = RT.Service.Resolve<WipProductRoutingController>().GetWorkOrderLayout(workOrderId);
            }
            info.IsWorkOrderLayout = isWorkOrderLayout;
            info.Layout = layout;
        }

        /// <summary>
        /// 工序节点变更加载生产工序数据
        /// 数据有：不良记录、维修记录、关键件、测试结果 
        /// </summary>
        /// <param name="versionId">生产产品版本ID</param>
        /// <param name="processId">工序ID</param>
        /// <param name="productStatus">产品状态，0未上线、1已完工、2在制</param>
        /// <returns>生产工序信息</returns>
        public WipProcessInfo LoadWipProcessData(double? versionId, double processId, int productStatus, string id, double? woId)
        {
            var info = new WipProcessInfo();
            if (versionId.HasValue)
            {
                var version = RF.GetById<WipProductVersion>(versionId, new EagerLoadOptions().LoadWithViewProperty());

                if (version == null)
                {
                    throw new ValidationException("未找到生产产品版本".L10N());
                }

                info.DefectList.AddRange(version.DefectList.Where(p => p.ProcessId == processId));
                info.RepaireList.AddRange(version.RepaireList.Where(p => p.ProcessId == processId));
                WipProductProcess process = null;
                var processes = version.ProcessList.Where(p => p.ProcessId == processId);
                if (processes.Count()> 1)
                {
                   var finishProcess = processes.Where(p => p.State == WipProductProcessState.Finish);
                    if (finishProcess.Count() > 1)//多条
                    {
                        finishProcess.ForEach(p =>
                        {
                            info.KeyItemList.AddRange(p.KeyItemList);
                            info.TestResultList.AddRange(p.TestResultList);
                        });
                    }
                    else//只有一条
                    {
                        process = finishProcess.FirstOrDefault();
                    }
                } else
                {
                    process = processes.FirstOrDefault();
                }

                if (process != null)
                {
                    info.KeyItemList.AddRange(process.KeyItemList);
                    info.TestResultList.AddRange(process.TestResultList);
                }

                if (productStatus == 1)   //已完工，取关键件
                {
                    info.KeyItemList.ForEach(p => info.BomList.Add(InitBomViewModes(p.Item, p.UnitQty, processId,p.ItemExtProp,p.ItemExtPropName)));
                }
                else
                {
                    if (productStatus == 2 && woId.HasValue)   //在制，取运行时产品bom
                    {
                        var Product = RT.Service.Resolve<RuntimeController>().FindProduct(version.Product.Puid);

                        var routingProcesses = RT.Service.Resolve<WorkOrderController>().GetRoutingProcess(woId.Value);
                        var workOrderRoutingProcess = routingProcesses.FirstOrDefault(x => x.ActivityId == id);
                        if (workOrderRoutingProcess != null)
                        {
                            var rtProcess = Product?.Routing?.Processes?.FirstOrDefault(p => p.Id == workOrderRoutingProcess.Id);
                            rtProcess?.Boms?.ForEach(p => info.BomList.Add(InitBomViewModesByProductBom(p, processId)));
                        }
                    }
                }
            }

            // productStatus == 0，即未上线，取工单工序bom
            if (woId.HasValue && productStatus == 0)
            {
                var routingProcesses = RT.Service.Resolve<WorkOrderController>().GetRoutingProcess(woId.Value);
                var workOrderRoutingProcess = routingProcesses.FirstOrDefault(x => x.ActivityId == id);
                if (workOrderRoutingProcess != null)
                {
                    
                    var workOrderProcessBoms = RT.Service.Resolve<WorkOrderController>()
                        .GetWoProcessBomList(new System.Collections.Generic.List<double> { woId.Value });

                    var boms = workOrderProcessBoms.Where(p => p.RoutingProcessId == workOrderRoutingProcess.Id);
                    boms.ForEach(p => info.BomList.Add(
                        RT.Service.Resolve<WipProductRoutingController>().InitBomViewModesByWorkOrderProcessBom(p)));
                }
            }

            return info;
        }

        /// <summary>
        /// 初始bom模型
        /// </summary>
        /// <param name="item">物料</param>
        /// <param name="qty">用量</param>
        /// <param name="processId">工序ID</param>
        /// <param name="itemExtProp"></param>
        /// <param name="itemExtPropName"></param>
        /// <returns>bom模型</returns>
        ProductBomViewModel InitBomViewModes(Item item, decimal qty, double processId,string itemExtProp,string itemExtPropName)
        {
            return new ProductBomViewModel()
            {
                ProcessId = processId,
                Item = item,
                Code = item.Code,
                Name = item.Name,
                ItemExtProp=itemExtProp,
                ItemExtPropName=itemExtPropName,
                Qty = qty,
                IsBuckleMaterial = true,
                Id = Guid.NewGuid().ToString()
            };
        }

        /// <summary>
        /// 初始bom模型
        /// </summary>
        /// <param name="bom">运行时BOM</param>
        /// <param name="processId">工序ID</param>
        /// <returns>bom模型</returns>
        ProductBomViewModel InitBomViewModesByProductBom(bom bom, double processId)
        {
            var item = RF.GetById<Item>(bom.ItemId);

            var bomViewModel = new ProductBomViewModel()
            {
                ProcessId = processId,
                Item = item,
                Code = item.Code,
                Name = item.Name,
                Qty = bom.Qty,
                ItemExtProp=bom.ItemExtProp,
                ItemExtPropName=bom.ItemExtPropName,
                IsBuckleMaterial = true,
                Id = Guid.NewGuid().ToString(),
                WorkStepId = bom.WorkStepId,
                IsAttachment = bom.IsAttachment,
                IsExternal = bom.IsExternal,
                IsRepeat = bom.IsRepeat,
                IsSingleLabel = bom.IsSingleLabel,
                HasBarcodeRule = bom.HasBarcodeRule,
            };


            if (bom.WorkStepId.HasValue && bom.WorkStepId != 0)
            {
                var workStep = RF.GetById<WorkStep>(bom.WorkStepId);
                bomViewModel.WorkStepName = workStep.Name;
                bomViewModel.WorkStepCode = workStep.Code;
            }

            return bomViewModel;

        }

        public WipProductVersion GetWipProductVersion(double versionId)
        {
            return RF.GetById<WipProductVersion>(versionId);
        }
    }

    /// <summary>
    /// 生产产品信息
    /// </summary>
    public class WipProductInfo
    {
        /// <summary>
        /// 生产产品版本
        /// </summary>
        public WipProductVersion WipProductVersion { get; set; }

        /// <summary>
        /// 是否工单工艺路线布局
        /// </summary>
        public bool IsWorkOrderLayout { get; set; }

        /// <summary>
        /// 工艺路线布局
        /// </summary>
        public string Layout { get; set; }

        /// <summary>
        /// 运行时产品信息
        /// </summary>
        public product Product { get; set; }

        /// <summary>
        /// 产品工艺路线事件列表
        /// </summary>
        public EntityList<WipProductRoutingEvent> RoutingEventList { get; } = new EntityList<WipProductRoutingEvent>();
    }

    /// <summary>
    /// 生产工序信息
    /// </summary>
    public class WipProcessInfo
    {
        /// <summary>
        /// 产品生产关键件
        /// </summary>
        public EntityList<WipProductProcessKeyItem> KeyItemList { get; } = new EntityList<WipProductProcessKeyItem>();

        /// <summary>
        /// 产品测试结果
        /// </summary>
        public EntityList<WipProductTestResult> TestResultList { get; } = new EntityList<WipProductTestResult>();

        /// <summary>
        /// 产品缺陷记录
        /// </summary>
        public EntityList<WipProductDefect> DefectList { get; } = new EntityList<WipProductDefect>();

        /// <summary>
        /// 产品维修记录
        /// </summary>
        public EntityList<WipProductRepair> RepaireList { get; } = new EntityList<WipProductRepair>();

        /// <summary>
        /// 产品BOM
        /// </summary>
        public EntityList<ProductBomViewModel> BomList { get; } = new EntityList<ProductBomViewModel>();
    }
}