using SIE.Common.Catalogs;
using SIE.Common.Sort;
using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Items;
using SIE.Packages;
using SIE.Packages.Boxs;
using SIE.Packages.Packages;
using System.Linq;

namespace SIE.xUnit.Packages
{
    public partial class PkgTestController : DomainController
    {
        public virtual TurnoverBox CreateDistTurnoverBox()
        {
            string distBoxType = GetDistTurnoverBoxType();
            var box = new TurnoverBox();
            box.GenerateId();
            box.Code = $"MES-B{box.Id}";
            box.Type = distBoxType;
            box.State = BoxState.Unused;
            RF.Save(box);
            return box;
        }

        public virtual string GetDistTurnoverBoxType()
        {
            var boxCatalogType = InitBoxTypeCatalogType();
            var distBoxType = boxCatalogType.CatalogList.FirstOrDefault(p => p.Name == "配送周转箱")?.Code;
            return distBoxType;
        }

        CatalogType InitBoxTypeCatalogType()
        {
            var catalogType = RT.Service.Resolve<CatalogController>().GetCatalogType(TurnoverBox.BoxTypeCatalog);
            if (catalogType == null)
            {
                catalogType = new CatalogType()
                {
                    Code = TurnoverBox.BoxTypeCatalog,
                    Name = "周转箱",
                    Description = "周转箱"
                };
                catalogType.CatalogList.Add(new Catalog() { Code = "C001", Name = "配送周转箱", CatalogType = catalogType });
                catalogType.CatalogList.Add(new Catalog() { Code = "C002", Name = "生产周转箱", CatalogType = catalogType });
                RF.Save(catalogType);
            }
            else if (catalogType.CatalogList.Count == 0)
            {
                catalogType.CatalogList.Add(new Catalog() { Code = "C001", Name = "配送周转箱", CatalogType = catalogType });
                catalogType.CatalogList.Add(new Catalog() { Code = "C002", Name = "生产周转箱", CatalogType = catalogType });
                RF.Save(catalogType);
            }

            return catalogType;
        }

        /// <summary>
        /// 主单位---盒---箱---栈板
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<PackingUnit> GetOrCreateMESPackingUnit()
        {
            EntityList<PackingUnit> res = new EntityList<PackingUnit>();
            var masterUnit = CreateMasterUnit();
            var packingUnits = RT.Service.Resolve<CommonController>().GetDatas<PackingUnit>(p => p.Code.Contains("MES_%"));
            if (packingUnits.Any())
            {
                res.Add(masterUnit);
                res.AddRange(packingUnits);
                return res;
            }
            var box = new PackingUnit()
            {
                Code = "MES_Box",
                Name = "盒",
            };
            box.GenerateId();
            var @case = new PackingUnit()
            {
                Code = "MES_Case",
                Name = "箱",
            };
            var pallet = new PackingUnit()
            {
                Code = "MES_Pallet",
                Name = "栈板",
            };
            res.Add(@case);
            res.Add(pallet);
            res.Add(box);
            RF.Save(res);
            res.Add(masterUnit);
            return res;
        }

        public virtual ItemPackageRule GetOrCreateItemPackageRule(Item item, string code)
        {
            var rule = RT.Service.Resolve<CommonController>().GetData<ItemPackageRule>(p => p.Code == code);
            if (rule != null)
                return rule;
            var packingRule = GetOrCreatePackageRule(item.Template);
            rule = new ItemPackageRule()
            {
                Code = "MES采集包装规则",
                Name = "MES采集包装规则",
                PackageRule = packingRule,
                IsDefault = true,
                Item = item
            };
            rule.GenerateId();
            packingRule.PackageRuleDetailList.OrderBy(p => p.GetIndex()).ForEach(dtl =>
            {
                var ruleDtl = new ItemPackageRuleDetail()
                {
                    Qty = dtl.Qty,
                    LevelQty = dtl.LevelQty,
                    ItemPackageRule = rule,
                    PackageUnit = dtl.PackageUnit,
                    IsPrint = dtl.IsPrint,
                    PrintTemplate = dtl.PrintTemplate,
                    NumberRule = dtl.NumberRule
                };
                ruleDtl.SetIndex(dtl.GetIndex());
                ruleDtl.GenerateId();
                rule.ItemPackageRuleDetailList.Add(ruleDtl);
            });
            RF.Save(rule);
            return rule;
        }

        public virtual PackageRule GetOrCreatePackageRule(SIE.Core.Items.LabelPrintTemplate template)
        {
            var rule = RT.Service.Resolve<CommonController>().GetData<PackageRule>(p => p.Code.Contains("MES采集包装规则"));
            if (rule != null)
                return rule;
            var packingUnits = GetOrCreateMESPackingUnit();
            rule = new PackageRule()
            {
                Code = "MES采集包装规则",
                Name = "MES采集包装规则",
            };
            rule.GenerateId();
            //主单位
            var masterUnit = packingUnits.FirstOrDefault(p => p.IsMasterUnit);
            var dtl1 = new PackageRuleDetail()
            {
                Qty = 1,
                LevelQty = 1,
                PackageRule = rule,
                PackageUnit = masterUnit
            };
            dtl1.GenerateId();
            dtl1.SetIndex(dtl1.Id);
            rule.PackageRuleDetailList.Add(dtl1);
            //盒
            var boxUnit = packingUnits.FirstOrDefault(p => p.Code == "MES_Box");
            var dtl2 = new PackageRuleDetail()
            {
                Qty = 2,
                LevelQty = 2,
                PackageRule = rule,
                PackageUnit = boxUnit,
                NumberRuleId = template?.NumberRuleId,
                PrintTemplateId = template?.LabelTemplateId
            };
            dtl2.GenerateId();
            dtl2.SetIndex(dtl2.Id);
            rule.PackageRuleDetailList.Add(dtl2);
            //箱
            var caseUnit = packingUnits.FirstOrDefault(p => p.Code == "MES_Case");
            var dtl3 = new PackageRuleDetail()
            {
                Qty = 4,
                LevelQty = 2,
                PackageRule = rule,
                PackageUnit = caseUnit,
                IsPrint = true,
                NumberRuleId = template?.NumberRuleId,
                PrintTemplateId = template?.LabelTemplateId
            };
            dtl3.GenerateId();
            dtl3.SetIndex(dtl3.Id);
            rule.PackageRuleDetailList.Add(dtl3);
            //栈板
            var palletUnit = packingUnits.FirstOrDefault(p => p.Code == "MES_Pallet");
            var dtl4 = new PackageRuleDetail()
            {
                Qty = 8,
                LevelQty = 2,
                PackageRule = rule,
                PackageUnit = palletUnit,
                NumberRuleId = template?.NumberRuleId,
                PrintTemplateId = template?.LabelTemplateId
            };
            dtl4.GenerateId();
            dtl4.SetIndex(dtl4.Id);
            rule.PackageRuleDetailList.Add(dtl4);
            RF.Save(rule);
            return rule;
        }
    }
}