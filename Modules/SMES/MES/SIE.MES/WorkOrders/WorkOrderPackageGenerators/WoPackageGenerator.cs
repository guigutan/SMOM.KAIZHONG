using SIE.Common.Sort;
using SIE.Domain;
using SIE.Items;
using SIE.Packages;
using SIE.Packages.Packages;
using System;
using System.Linq;

namespace SIE.MES.WorkOrders.WorkOrderPackageGenerators
{
    /// <summary>
    /// 
    /// </summary>
    public class WoPackageGenerator
    {
        /// <summary>
        /// 产品默认包装规则列表
        /// </summary>
        private readonly EntityList<ItemPackageRule> itemPackageRules;

        /// <summary>
        /// 包装规则明细列表
        /// </summary>
        private readonly EntityList<ItemPackageRuleDetail> itemPackageRuleDetails;

        /// <summary>
        /// 打印模板设置列表
        /// </summary>
        private readonly EntityList<Core.Items.LabelPrintTemplate> labelPrintTemplates;

        /// <summary>
        /// 下达的产品
        /// </summary>
        private readonly EntityList<Item> products;

        /// <summary>
        /// 包装单位
        /// </summary>
        private readonly EntityList<PackingUnit> packingUnits;

        /// <summary>
        /// 构造函数
        /// </summary>                
        /// <param name="_products">产品列表</param>        
        public WoPackageGenerator(
            EntityList<Item> _products)
        {
            products = _products;

            var productIds = products.Select(x => x.Id).Distinct().ToList();

            itemPackageRules = RT.Service.Resolve<PackageController>()
                .GetDefaultItemPackageRuleByItemId(productIds);

            var itemPackageRuleIds = itemPackageRules.Select(x => x.Id).Distinct().ToList();
            itemPackageRuleDetails = RT.Service.Resolve<PackageController>()
              .GetItemPackageRuleDetailList(itemPackageRuleIds);

            //获取打印模板
            var templateIds = products
                .Where(x => x.TemplateId.HasValue)
                .Select(x => x.TemplateId.Value).Distinct().ToList();

            labelPrintTemplates = RT.Service.Resolve<ItemController>().GetTemplates(templateIds);

            //获取包装单位
            var packageUnitIds = itemPackageRuleDetails.Select(x => x.PackageUnitId).Distinct().ToList();
            packingUnits = RT.Service.Resolve<PackageController>().GetPackingUnits(packageUnitIds);
        }


        /// <summary>
        /// 生成工单包装规则
        /// </summary>
        /// <param name="workOrder">工单</param>
        public virtual void GenerateWorkOrderPackageRule(WorkOrder workOrder)
        {
            if (workOrder == null)
            {
                return;
            }

            workOrder.PackageRuleDetailList.Clear();
            if (workOrder.ProductId <= 0)
            {
                return;
            }

            var packageRule = itemPackageRules.FirstOrDefault(x => x.ItemId == workOrder.ProductId);

            if (packageRule == null || !itemPackageRuleDetails.Any(x => x.ItemPackageRuleId == packageRule.Id))
            {
                return;
            }

            double index = 1;
            itemPackageRuleDetails.Where(x => x.ItemPackageRuleId == packageRule.Id)
               .OrderBy(f => f.GetIndex())
               .ForEach(f =>
               {
                   var rule = new WorkOrderPackageRuleDetail
                   {
                       DetailId = f.Id,
                       Description = f.Description,
                       Height = f.Height,
                       IsInStockLabel = f.IsInStockLabel,
                       IsOutStockLabel = f.IsOutStockLabel,
                       IsPackage = f.IsPackage,
                       Length = f.Length,
                       NumberRuleId = f.NumberRuleId,
                       PackageUnitId = f.PackageUnitId,
                       Qty = f.Qty,
                       LevelQty = f.LevelQty,
                       Volume = f.Volume,
                       Weight = f.Weight,
                       Width = f.Width,
                       //此时工单ID还没有生成
                       WorkOrder = workOrder,
                       IsPrint = f.IsPrint,
                       PrintTemplateId = f.PrintTemplateId,
                       TemplateName = f.PrintTemplateName,
                   };

                   //改成保存前批量获取Id
                   //#rule.GenerateId();#

                   rule.SetIndex(index++);
                   workOrder.PackageRuleDetailList.Add(rule);
               });

            //设置包装规则后进行验证
            WorkOrderPackageRuleValidator.Validate(workOrder, packingUnits);
        }


        /// <summary>
        /// 产品标签打印模板设置
        /// </summary>
        /// <param name="workOrder">工单</param>
        public Core.Items.LabelPrintTemplate GenerateProductLabelTemplate(WorkOrder workOrder)
        {
            if (workOrder.TemplateId.HasValue)
            {
                return null;
            }

            var labelTemplate = new Core.Items.LabelPrintTemplate();

            //改成批量获取ID<code>labelTemplate.GenerateId();</code>

            workOrder.Template = labelTemplate;

            var product = products.FirstOrDefault(x => x.Id == workOrder.ProductId);
            var template = labelPrintTemplates.FirstOrDefault(x => x.Id == product.TemplateId);

            if (template != null)
            {
                labelTemplate.NumberRuleId = template.NumberRuleId;

                string labelPrintable = typeof(Barcodes.Printables.BarcodePrintable).GetQualifiedName();
                var labelTemplateId = template.LabelTemplateEntityType == labelPrintable ? template.LabelTemplateId : null;
                labelTemplate.LabelTemplateId = labelTemplateId;

                string packingPrintable = typeof(WIP.Moves.BarcodePrintable).GetQualifiedName();
                var packingTemplateId = template.PackingTemplateEntityType == packingPrintable ? template.PackingTemplateId : null;
                labelTemplate.PackingTemplateId = packingTemplateId;
            }

            return labelTemplate;
        }
    }
}
