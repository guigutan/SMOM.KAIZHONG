using SIE.Barcodes;
using SIE.Barcodes.Printables;
using SIE.Common;
using SIE.Common.Prints;
using SIE.Common.Sort;
using SIE.Core.Common.Controllers;
using SIE.DIST;
using SIE.Domain;
using SIE.Items;
using SIE.Items.ProductBoms;
using SIE.MES.LoadItems;
using SIE.MES.Storages;
using SIE.MES.WIP;
using SIE.MES.WIP.Products;
using SIE.MES.WorkOrders;
using SIE.Packages;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using SIE.xUnit.Core;
using SIE.xUnit.DIST;
using SIE.xUnit.DIST.Distribution.Models;
using SIE.xUnit.Items;
using SIE.xUnit.MES.Storages;
using SIE.xUnit.Packages;
using SIE.xUnit.Techs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.xUnit.MES
{
    public partial class MesTestController : DomainController
    {
        /// <summary>
        /// 创建生产采集产品
        /// </summary>
        /// <returns>物料</returns>
        public virtual Item GetOrCreateWipProduct(string code)
        {
            var unit = RT.Service.Resolve<ItemTestController>().GetOrCreateUnit("MES-单位");
            var item = RT.Service.Resolve<ItemController>().GetItem(code);
            if (item != null)
                return item;
            item = new Item();
            item.GenerateId();
            item.Code = code;
            item.Name = code;
            item.Type = ItemType.Product;
            item.Unit = unit;
            CreateLabelPrintTemplate(item, typeof(BarcodePrintable).GetQualifiedName());
            RF.Save(item);
            return item;
        }

        public virtual EntityList<Item> GetOrCreateWipKeyItems(int count)
        {
            EntityList<Item> keyItems = new EntityList<Item>();
            var items = RT.Service.Resolve<CommonController>().GetDatas<Item>(p => p.Code.Contains("MES-采集关键件%"));
            if (items.Count >= count)
                return items;
            var unit = RT.Service.Resolve<ItemTestController>().GetOrCreateUnit("MES-单位");
            for (int i = 0; i < count; i++)
            {
                var item = new Item();
                item.GenerateId();
                item.Code = $"MES-采集关键件{item.Id}";
                item.Name = $"MES-采集关键件{item.Id}";
                item.Type = ItemType.Material;
                item.Unit = unit;
                keyItems.Add(item);
            }
            RF.Save(keyItems);
            return keyItems;
        }

        public virtual ProductBom CreateProductBom(Item product, EntityList<Item> keyItems)
        {
            var bom = new ProductBom()
            {
                Product = product,
                IsDefault = true
            };
            bom.GenerateId();
            bom.Code = $"{product.Code}{bom.Id}";
            bom.Name = $"{product.Name}{bom.Id}";
            bom.Version = $"V{bom.Id}";
            Random r = new Random();
            keyItems.ForEach(keyItem =>
            {
                var dtl = new ProductBomDetail()
                {
                    Item = keyItem,
                    UnitQty = r.Next(1, 6),
                    ProductBom = bom
                };
                dtl.GenerateId();
                bom.DetailList.Add(dtl);
            });
            RF.Save(bom);
            RT.Service.Resolve<ProductBomController>().SetDefaultProductBom(product.Id, bom.Id);
            return bom;
        }

        private void CreateLabelPrintTemplate(Item item, string entityType)
        {
            string labelTempalte = "条码标签模板.siedev";
            string packTempalte = "包装标签模板.siedev";
            var controller = RT.Service.Resolve<ContextControllerTest>();
            var numberRule = controller.CreateNumberRule("条码生成规则", "SN");
            controller.CreatePrintTemplate(numberRule, labelTempalte, entityType);
            controller.CreatePrintTemplate(numberRule, packTempalte, entityType);
            var templates = RT.Service.Resolve<PrintsController>().GetPrintTemplates(entityType, true, numberRule.Id);
            var template = new SIE.Core.Items.LabelPrintTemplate()
            {
                NumberRule = numberRule,
                LabelTemplate = templates?.FirstOrDefault(p => p.FileName == labelTempalte),
                PackingTemplate = templates?.FirstOrDefault(p => p.FileName == packTempalte)
            };
            RF.Save(template);
            item.Template = template;
        }

        public virtual WorkOrder CreateWipWorkOrder(decimal planQty, SIE.Core.WorkOrders.WorkOrderType workOrderType, WipResource wipResource, Item product, Action<WorkOrder> saving)
        {
            ConfigWorkOrderNoConfig();
            var now = DateTime.Now;
            var workOrderController = RT.Service.Resolve<WorkOrderController>();
            WorkOrder workOrder = new WorkOrder()
            {
                No = workOrderController.GetWorkOrderNo(),
                Source = SourceType.Internal,
                State = SIE.Core.WorkOrders.WorkOrderState.Release,
                PlanQty = planQty,
                OrderQty = planQty,
                PlanBeginDate = now,
                PlanEndDate = now.AddMonths(1),
                Type = workOrderType,
                WorkShop = wipResource.WorkShop,
                Resource = wipResource,
                MakerId = RT.IdentityId,
                MakeDate = now
            };
            workOrder.GenerateId();
            var template = product.Template;
            var woTemplate = new SIE.Core.Items.LabelPrintTemplate()
            {
                NumberRule = template?.NumberRule,
                LabelTemplate = template?.LabelTemplate,
                PackingTemplate = template?.PackingTemplate
            };
            woTemplate.GenerateId();
            workOrder.Template = woTemplate;
            workOrder.PropertyChanged += RT.Service.Resolve<WorkOrderPropertyChanged>().WorkOrderOnPropertyChanged;
            workOrder.Product = product;
            workOrder.PropertyChanged -= RT.Service.Resolve<WorkOrderPropertyChanged>().WorkOrderOnPropertyChanged;
            saving(workOrder);
            workOrderController.SaveWorkOrder(workOrder, workOrder.Template, WorkOrderLogType.Release, "单元测试工单创建");
            return workOrder;
        }

        public virtual List<string> PrintBarcode(WorkOrder wo)
        {
            var template = wo.Template;
            //打印条码
            var barcodes = RT.Service.Resolve<BarcodeController>().Print(new PrinterInfo
            {
                WorkOrderId = wo.Id,
                NumberRuleId = template?.NumberRuleId ?? 0,
                PrintTemplateId = template?.LabelTemplateId ?? 0,
                SingleQty = 1,
                PrintedQty = 0,
                PrintQty = (int)wo.PlanQty
            });
            return barcodes.Select(p => p.Sn).ToList();
        }

        public virtual Workcell GetWorkcell(WipResource resource, Process process)
        {
            var station = RT.Service.Resolve<TechTestController>().GetOrCreateStation(resource, process);
            var workcell = new Workcell()
            {
                EmployeeId = RT.IdentityId,
                ResourceId = resource.Id,
                ProcessId = process.Id,
                StationId = station.Id
            };
            workcell.Context["Station"] = station;
            return workcell;
        }

        public virtual IEnumerable<string> GetNotMoveSnList(List<string> barcodeList, WorkOrder wo)
        {
            var moveSnList = DB.Query<WipProductVersion>().Join<WipProduct>((v, p) => v.ProductId == p.Id && p.CurrentVersionId == v.Id).Where(p => p.WorkOrderId == wo.Id).Select(p => p.Sn).ToList<string>();
            var notMoveSnList = barcodeList.Except(moveSnList);
            return notMoveSnList;
        }

        public virtual List<string> WorkOrderLoadItem(WorkOrder workOrder, Workcell workcell)
        {
            decimal planQty = workOrder.PlanQty;
            var distController = RT.Service.Resolve<DistTestController>();
            DistributionInfo info = new DistributionInfo()
            {
                WorkOrderId = workOrder.Id,
                ResourceId = workOrder.ResourceId.Value
            };
            foreach (var gBom in workOrder.ProcessBomList.GroupBy(p => p.ItemId))
            {
                var boms = gBom.ToList();
                var item = boms.FirstOrDefault()?.Item;
                //设置工位货区
                RT.Service.Resolve<StorageTestController>().GetOrCreateStorageArea(workcell.StationId);
                decimal bomTotalQty = boms.Sum(p => p.SingleQty * planQty);
                info.DetailInfos.Add(new DistributionDetailInfo()
                {
                    ItemId = item.Id,
                    UnitId = item.Unit.Id,
                    Qty = bomTotalQty
                });
            }
            var boxs = distController.WorkOrderDistribution(info);
            boxs.ForEach(box =>
            {
                RT.Service.Resolve<LoadItemController>().LoadItem(box, workcell, SIE.MES.SingleLabels.LoadItemSourceType.DistributionBill, "");
            });

            return boxs;
        }

        public virtual EntityList<ItemStorage> GetItemStorages(Station station)
        {
            string areaCode = $"MES-{station?.Name}工位货区";
            return Query<ItemStorage>()
                 .Join<StorageLocation>((i, l) => i.StorageLocationId == l.Id)
                 .Join<StorageLocation, StorageArea>((l, a) => l.StorageAreaId == a.Id && a.Code == areaCode)
                 .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        public virtual EntityList<WorkOrderPackageRuleDetail> CreateWorkOrderPackageRuleDetails(ItemPackageRule packageRule)
        {
            var result = new EntityList<WorkOrderPackageRuleDetail>();
            packageRule.ItemPackageRuleDetailList.OrderBy(p=>p.GetIndex()).ForEach(dtl =>
            {
                var ruleDtl = new WorkOrderPackageRuleDetail()
                {
                    Qty = dtl.Qty,
                    LevelQty = dtl.LevelQty,
                    PackageUnit = dtl.PackageUnit,
                    IsPrint = dtl.IsPrint,
                    PrintTemplate = dtl.PrintTemplate,
                    NumberRule = dtl.NumberRule,
                };
                ruleDtl.SetIndex(dtl.GetIndex());
                ruleDtl.GenerateId();
                result.Add(ruleDtl);
            });
            return result;
        }
    }
}