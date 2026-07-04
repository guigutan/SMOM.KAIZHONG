using SIE.Api;
using SIE.Common;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Common.Sort;
using SIE.Core.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.ObjectModel;
using SIE.Packages.ItemLabels;
using SIE.Packages.Packages;
using SIE.Packages.Packages.Datas;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Packages
{
    /// <summary>
    /// 包装控制器
    /// </summary> 
    public partial class PackageController : DomainController
    {
        #region 包装单位
        /// <summary>
        /// 获取主单位
        /// </summary>
        /// <returns>包装单位</returns>
        public virtual PackingUnit GetMasterUnit()
        {
            return Query<PackingUnit>().Where(f => f.IsMasterUnit).FirstOrDefault();
        }

        /// <summary>
        /// 获取单位
        /// </summary>
        /// <returns>包装单位</returns>
        public virtual PackingUnit GetUnit(double id)
        {
            return Query<PackingUnit>().Where(f => f.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// 获取包装单位
        /// </summary>
        /// <param name="ids">集合</param>
        /// <returns>包装单位</returns>
        public virtual EntityList<PackingUnit> GetPackingUnits(List<double> ids)
        {
            return Query<PackingUnit>().Where(p => ids.Contains(p.Id)).ToList();
        }

        /// <summary>
        /// 是否存在主单位
        /// </summary>
        /// <returns>存在返回true，否则返回false</returns>
        public virtual bool IsExistsMasterUnit()
        {
            return Query<PackingUnit>().Where(f => f.IsMasterUnit).Count() != 0;
        }

        /// <summary>
        /// 除开本身是否存在主单位
        /// </summary>
        /// <returns>存在返回true，否则返回false</returns>
        public virtual bool IsExistsMasterUnit(double id)
        {
            return Query<PackingUnit>().Where(f => f.Id != id && f.IsMasterUnit).Count() != 0;
        }

        /// <summary>
        /// 添加主单位
        /// </summary>
        /// <returns>包装单位</returns>
        public virtual PackingUnit AddMasterUnit()
        {
            PackingUnit unit = new PackingUnit
            {
                Code = "MasterUnit",
                Name = "主单位".L10N(),
                Description = "主单位".L10N(),
                IsMasterUnit = true,
                PackageUnitType = PackageUnitType.MasterUnit,
            };
            return unit;
        }

        /// <summary>
        /// 初始化包装单位
        /// </summary>
        public virtual void InitPackingUnit()
        {
            EntityList<PackingUnit> list = new EntityList<PackingUnit>();
            var units = RF.GetAll<PackingUnit>();
            foreach (var item in Enum.GetValues(typeof(PackageUnitType)))
            {
                var type = (PackageUnitType)item;
                if (units.FirstOrDefault(f => f.Code == type.ToString() && f.Name == type.ToLabel()) == null)
                {
                    PackingUnit unit = new PackingUnit
                    {
                        Code = type.ToString(),
                        Name = type.ToLabel(),
                        IsMasterUnit = false,
                        PackageUnitType = type
                    };

                    if (type == PackageUnitType.MasterUnit)
                    {
                        unit.Description = "最小包装管理单位".L10N();
                        unit.IsMasterUnit = true;
                    }
                    list.Add(unit);
                }
            }
            RF.Save(list);
            if (list.Any())
            {
                var numberRules = CreatePackageUnitNumberRule();
                if (numberRules.Any())
                    GetDefaultPackageRule(numberRules);
            }
        }

        /// <summary>
        /// 创建编码规则
        /// </summary>
        private EntityList<NumberRule> CreatePackageUnitNumberRule()
        {
            var regularNumberRule = RT.Service.Resolve<NumberRuleController>().GetNumberSegment(StatusType.Enable, "固定编码算法").FirstOrDefault();
            if (regularNumberRule == null)
                throw new ValidationException("先在编码段功能，启用固定编码算法!".L10N());
            var dateNumberRule = RT.Service.Resolve<NumberRuleController>().GetNumberSegment(StatusType.Enable, "时间编码算法").FirstOrDefault();
            if (dateNumberRule == null)
                throw new ValidationException("先在编码段功能，启用时间编码算法!".L10N());

            var orderNumberRule = RT.Service.Resolve<NumberRuleController>().GetNumberSegment(StatusType.Enable, "序列生成算法(区分当天日期)").FirstOrDefault();
            if (orderNumberRule == null)
                throw new ValidationException("先在编码段功能，启用序列生成算法(区分当天日期)!".L10N());


            EntityList<NumberRule> numberRules = new EntityList<NumberRule>();
            string masteUnitCode = "主单位编码规则".L10N();
            var masteUnitRule = RT.Service.Resolve<NumberRuleController>().GetNumberRule(masteUnitCode);
            if (masteUnitRule == null)
            {
                masteUnitRule = new NumberRule();
                masteUnitRule.Code = masteUnitCode;
                masteUnitRule.Name = masteUnitCode;
                masteUnitRule.Type = RuleType.Common;

                var ruleDtl = new NumberRuleDetail();
                ruleDtl.SetIndex(1);
                ruleDtl.Segment = regularNumberRule;
                ruleDtl.Length = 2;
                ruleDtl.Config = @"{""ContString"":""MU""}";
                masteUnitRule.DetailList.Add(ruleDtl);

                var ruleDtl1 = new NumberRuleDetail();
                ruleDtl1.SetIndex(2);
                ruleDtl1.Segment = dateNumberRule;
                ruleDtl1.Length = 6;
                ruleDtl1.Config = @"{""DateFormat"":1}";
                masteUnitRule.DetailList.Add(ruleDtl1);

                var ruleDtl2 = new NumberRuleDetail();
                ruleDtl2.SetIndex(3);
                ruleDtl2.Segment = orderNumberRule;
                ruleDtl2.Length = 5;
                ruleDtl2.Config = @"{""StartValue"":1,""Step"":1}";
                masteUnitRule.DetailList.Add(ruleDtl2);             
            }
            numberRules.Add(masteUnitRule);

            string boxCode = "箱编码规则".L10N();
            var boxRule = RT.Service.Resolve<NumberRuleController>().GetNumberRule(boxCode);
            if (boxRule == null)
            {
                boxRule = new NumberRule();
                boxRule.Code = boxCode;
                boxRule.Name = boxCode;
                boxRule.Type = RuleType.Common;

                var ruleDtl = new NumberRuleDetail();
                ruleDtl.SetIndex(1);
                ruleDtl.Segment = regularNumberRule;
                ruleDtl.Length = 1;
                ruleDtl.Config = @"{""ContString"":""B""}";
                boxRule.DetailList.Add(ruleDtl);

                var ruleDtl1 = new NumberRuleDetail();
                ruleDtl1.SetIndex(2);
                ruleDtl1.Segment = dateNumberRule;
                ruleDtl1.Length = 6;
                ruleDtl1.Config = @"{""DateFormat"":1}";
                boxRule.DetailList.Add(ruleDtl1);

                var ruleDtl2 = new NumberRuleDetail();
                ruleDtl2.SetIndex(3);
                ruleDtl2.Segment = ruleDtl2.Segment = orderNumberRule;
                ruleDtl2.Length = 5;
                ruleDtl2.Config = @"{""StartValue"":1,""Step"":1}";
                boxRule.DetailList.Add(ruleDtl2);                
            }
            numberRules.Add(boxRule);

            string palletCode = "托编码规则".L10N();
            var palletRule = RT.Service.Resolve<NumberRuleController>().GetNumberRule(palletCode);
            if (palletRule == null)
            {
                palletRule = new NumberRule();
                palletRule.Code = palletCode;
                palletRule.Name = palletCode;
                palletRule.Type = RuleType.Common;

                var ruleDtl = new NumberRuleDetail();
                ruleDtl.SetIndex(1);
                ruleDtl.Segment = regularNumberRule;
                ruleDtl.Length = 1;
                ruleDtl.Config = @"{""ContString"":""P""}";
                palletRule.DetailList.Add(ruleDtl);

                var ruleDtl1 = new NumberRuleDetail();
                ruleDtl1.SetIndex(2);
                ruleDtl1.Segment = dateNumberRule;
                ruleDtl1.Length = 6;
                ruleDtl1.Config = @"{""DateFormat"":1}";
                palletRule.DetailList.Add(ruleDtl1);

                var ruleDtl2 = new NumberRuleDetail();
                ruleDtl2.SetIndex(3);
                ruleDtl2.Segment = orderNumberRule;
                ruleDtl2.Length = 5;
                ruleDtl2.Config = @"{""StartValue"":1,""Step"":1}";
                palletRule.DetailList.Add(ruleDtl2);

               
            }
            numberRules.Add(palletRule);
            RF.Save(numberRules);

            return numberRules;
        }
        #endregion

        #region 包装规则
        /// <summary>
        /// 获取包装规则数据
        /// </summary>
        /// <param name="idList">IdList</param>
        /// <returns>包装规则数据</returns>
        public virtual EntityList<PackageRule> GetPackageRules(List<double> idList)
        {
            EntityList<PackageRule> packageRuleList = new EntityList<PackageRule>();
            for (int i = 0; i < Math.Ceiling((double)idList.Count / 1000); i++)
            {
                var query = Query<PackageRule>()
                    .Where(p => idList.Skip(i * 1000).Take(1000).Contains(p.Id));
                packageRuleList.AddRange(query.ToList());
            }

            return packageRuleList;
        }

        /// <summary>
        /// 获取包装规则明细列表(贪婪加载包装单位）
        /// </summary>
        /// <param name="packageRuleId">包装规则Id</param>
        /// <returns>包装规则明细列表</returns>
        public virtual EntityList<PackageRuleDetail> GetPackageRuleDetails(double packageRuleId)
        {
            return GetPackageRuleDetails(packageRuleId, new List<OrderInfo>(), null);
        }

        /// <summary>
        /// 获取包装规则明细列表(贪婪加载包装单位）
        /// </summary>
        /// <param name="packageRuleId">包装规则Id</param>
        /// <param name="orderInfos">排序条件</param>
        /// <param name="pagingInfo">分页条件</param>
        /// <returns>包装规则明细列表</returns>
        public virtual EntityList<PackageRuleDetail> GetPackageRuleDetails(double packageRuleId, IList<OrderInfo> orderInfos, PagingInfo pagingInfo)
        {
            return Query<PackageRuleDetail>()
                .Where(p => p.PackageRuleId == packageRuleId)
                .OrderBy(orderInfos)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取默认包装规则，没有则新增
        /// </summary>
        /// <param name="numberRules">编码规则列表</param>
        /// <returns>包装规则</returns>
        public virtual PackageRule GetDefaultPackageRule(EntityList<NumberRule> numberRules)
        {
            var rule = GetPackageRuleByCode("初始化默认包装规则".L10N());
            if (rule == null)
            {
                var masterUnitCode = "主单位编码规则".L10N();
                var masterUnitRule = numberRules.FirstOrDefault(p => p.Code == masterUnitCode);
                if (masterUnitRule == null)
                    masterUnitRule = RT.Service.Resolve<NumberRuleController>().GetNumberRule(masterUnitCode);

                rule = new PackageRule() { Code = "初始化默认包装规则".L10N(), Name = "初始化默认包装规则".L10N(), Description = "新建物料默认的包装规则".L10N() };
                var masterUnit = Query<PackingUnit>().Where(p => p.IsMasterUnit).FirstOrDefault();
                if (masterUnit == null)
                {
                    masterUnit = RT.Service.Resolve<PackageController>().AddMasterUnit();
                    RF.Save(masterUnit);
                }
                PackageRuleDetail packageRuleDetail = new PackageRuleDetail()
                {
                    PackageUnitId = masterUnit.Id,
                    Qty = 1000000,
                    LevelQty = 1,
                    NumberRuleId = masterUnitRule?.Id,
                    IsSequence = true,
                    IsInStockLabel = true,
                };

                rule.PackageRuleDetailList.Add(packageRuleDetail);
                packageRuleDetail.SetIndex(1);
                var defaultBox = Query<PackingUnit>().Where(p => p.Code == "Box" && !p.IsMasterUnit).FirstOrDefault();
                if (defaultBox == null)
                {
                    defaultBox = new PackingUnit
                    {
                        Code = "Box",
                        Name = "箱".L10N(),
                        IsMasterUnit = false
                    };
                    RF.Save(defaultBox);
                }

                var boxCode = "箱编码规则".L10N();
                var boxRule = numberRules.FirstOrDefault(p => p.Code == boxCode);
                if (boxRule == null)
                    boxRule = RT.Service.Resolve<NumberRuleController>().GetNumberRule(boxCode);
                PackageRuleDetail boxPackageRuleDetail = new PackageRuleDetail()
                {
                    PackageUnitId = defaultBox.Id,
                    Qty = packageRuleDetail.Qty * 50,
                    LevelQty = 50,
                    NumberRuleId = boxRule?.Id
                };

                boxPackageRuleDetail.SetIndex(2);
                rule.PackageRuleDetailList.Add(boxPackageRuleDetail);

                var pallet = Query<PackingUnit>().Where(p => p.Code == "Pallet" && !p.IsMasterUnit).FirstOrDefault();
                if (pallet == null)
                {
                    pallet = new PackingUnit
                    {
                        Code = "Pallet",
                        Name = "托".L10N(),
                        IsMasterUnit = false,
                    };
                    RF.Save(defaultBox);
                }

                var palletCode = "托编码规则".L10N();
                var palletRule = numberRules.FirstOrDefault(p => p.Code == palletCode);
                if (palletRule == null)
                    palletRule = RT.Service.Resolve<NumberRuleController>().GetNumberRule(palletCode);
                PackageRuleDetail palletPackageRuleDetail = new PackageRuleDetail()
                {
                    PackageUnitId = pallet.Id,
                    Qty = boxPackageRuleDetail.Qty * 100,
                    LevelQty = 100,
                    NumberRuleId = palletRule?.Id
                };

                palletPackageRuleDetail.SetIndex(3);
                rule.PackageRuleDetailList.Add(palletPackageRuleDetail);
                RF.Save(rule);
            }
            return rule;
        }

        /// <summary>
        /// 按编码获取包装规则
        /// </summary>
        /// <param name="code">编码</param>
        /// <returns>包装规则</returns>
        public virtual PackageRule GetPackageRuleByCode(string code)
        {
            var rule = Query<PackageRule>().Where(p => p.Code == code).FirstOrDefault();
            return rule;
        }

        public virtual EntityList<PrintTemplate> GetPrintTemplatesByRuleId(double ruleId, PagingInfo pagingInfo, string keyword, IList<OrderInfo> orderInfos = null)
        {
            var query = Query<PrintTemplate>().Join<NumberRuleInTemplate>((x, y) => x.Id == y.TemplateId)
                  .Where<NumberRuleInTemplate>((x, y) => y.RuleId == ruleId);
            if (keyword.IsNotEmpty())
                query.Where(f => f.FileName.Contains(keyword));
            if (orderInfos != null)
                query.OrderBy(orderInfos);
            return query.ToList(pagingInfo);
        }

        #endregion

        #region 物料包装规则
        /// <summary>
        /// 获取物料包装规则
        /// </summary>
        /// <param name="itemId">物料标签</param>
        /// <returns>物料包装规则列表</returns> 
        /// <exception cref="ArgumentNullException">物料ID未空</exception>
        public virtual EntityList<ItemPackageRule> GetItemPackageRuleByItemId(double itemId)
        {
            if (itemId <= 0)
                throw new ArgumentNullException(nameof(itemId));
            return Query<ItemPackageRule>().Where(p => p.ItemId == itemId).ToList(eagerLoad: new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取物料包装规则
        /// </summary>
        /// <param name="itemIds">物料标签</param>
        /// <returns>物料包装规则列表</returns> 
        /// <exception cref="ArgumentNullException">物料ID未空</exception>
        public virtual EntityList<ItemPackageRule> GetDefaultItemPackageRuleByItemId(List<double> itemIds)
        {
            return itemIds.SplitContains(ids =>
            {
                return Query<ItemPackageRule>().Where(p => ids.Contains(p.ItemId) && p.IsDefault).ToList(eagerLoad: new EagerLoadOptions().LoadWithViewProperty());
            });

        }

        /// <summary>
        /// 获取物料包装规则
        /// </summary>
        /// <param name="itemId">物料标签</param>
        /// <param name="elo">elo</param>
        /// <returns>物料包装规则</returns>
        /// <exception cref="ArgumentNullException">空异常</exception>
        public virtual ItemPackageRule GetDefaultItemPackageRuleByItemId(double itemId, EagerLoadOptions elo = null)
        {
            if (itemId <= 0)
                throw new ArgumentNullException(nameof(itemId));
            if (elo == null)
                elo = new EagerLoadOptions().LoadWithViewProperty();
            return Query<ItemPackageRule>().Where(p => p.ItemId == itemId && p.IsDefault).FirstOrDefault(elo);
        }

        /// <summary>
        /// 获取是否默认物料包装规则
        /// </summary>
        /// <param name="itemIds">物料Id列表</param>
        /// <param name="isDefault">是否默认</param>
        /// <returns>物料包装规则列表</returns>
        public virtual EntityList<ItemPackageRule> GetDefaultItemPackageRuleByItemIds(List<double> itemIds, bool? isDefault = null)
        {
            var query = Query<ItemPackageRule>();
            if (isDefault.HasValue)
            {
                query.Where(p => p.IsDefault == isDefault.Value);
            }
            if (itemIds == null || itemIds.Count == 0)
            {
                return new EntityList<ItemPackageRule>();
            }
            return itemIds.SplitContains(sons =>
            {
                return query.Where(p => sons.Contains(p.ItemId)).ToList(eagerLoad: new EagerLoadOptions().LoadWithViewProperty().LoadWith(ItemPackageRule.ItemProperty));
            });
        }

        /// <summary>
        /// 获取是否默认物料包装规则
        /// </summary>
        /// <param name="itemIds">物料Id列表</param>      
        /// <returns>物料包装规则列表</returns>
        public virtual EntityList<ItemPackageRuleDetail> GetDefaultItemPackageRuleDtlByItemIds(List<double> itemIds)
        {
            var query = Query<ItemPackageRuleDetail>();
            return itemIds.SplitContains(sons =>
            {
                return query.Join<ItemPackageRule>((x, y) => x.ItemPackageRuleId == y.Id && y.IsDefault).
                Where<ItemPackageRule>((a, p) => sons.Contains(p.ItemId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取包装规则明细
        /// </summary>
        /// <param name="ruleId">包装规则ID</param>
        /// <param name="keywork">查询关键字</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="elo">贪婪加载参数</param>
        /// <returns>PackageRuleDetail列表</returns>
        /// <exception cref="ArgumentNullException">空异常</exception>
        public virtual EntityList<ItemPackageRuleDetail> GetItemPackageRuleDetailsByRuleId(double ruleId, string keywork = null, PagingInfo pagingInfo = null, EagerLoadOptions elo = null)
        {
            if (ruleId <= 0)
                throw new ArgumentNullException(nameof(ruleId));
            var query = Query<ItemPackageRuleDetail>().Where(p => p.ItemPackageRuleId == ruleId);
            if (!string.IsNullOrWhiteSpace(keywork))
            {
                query.Join<PackingUnit>((x, y) => x.PackageUnitId == y.Id && (y.Code.Contains(keywork) || y.Name.Contains(keywork)));
            }

            return query.ToList(pagingInfo, elo);
        }

        /// <summary>
        /// 获取包装规则明细By物料Id
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <param name="keywork">关键字</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="elo"></param>
        /// <returns></returns>
        public virtual EntityList<ItemPackageRuleDetail> GetItemPackageRuleDetailsByItemId(double itemId, string keywork = null, PagingInfo pagingInfo = null, EagerLoadOptions elo = null)
        {
            var query = Query<ItemPackageRuleDetail>().Join<ItemPackageRule>((x, y) => x.ItemPackageRuleId == y.Id && y.ItemId == itemId && y.IsDefault);
            if (!string.IsNullOrWhiteSpace(keywork))
            {
                query.Join<PackingUnit>((x, y) => x.PackageUnitId == y.Id && (y.Code.Contains(keywork) || y.Name.Contains(keywork)));
            }
            return query.ToList(pagingInfo, elo);
        }

        /// <summary>
        /// 创建物料包装规则
        /// </summary>
        /// <param name="packageRuleList">包装规则集合</param>
        /// <param name="itemId">物料ID</param>
        /// <returns>物料包装规则集合</returns>
        public virtual EntityList<ItemPackageRule> CreateItemPackageRule(List<PackageRule> packageRuleList, double itemId)
        {
            string strCurrTime = DateTime.Now.ToString("yyyyMMddhhmmss");
            EntityList<ItemPackageRule> itemPackageRuleList = new EntityList<ItemPackageRule>();

            var item = RF.GetById<Item>(itemId, new EagerLoadOptions().LoadWithViewProperty());

            //如果物料下只有一条包装规则，则设置为默认
            var count = GetGetItemPackageRuleCount(itemId);
            var isDefault = false;
            if (packageRuleList.Count + count == 1)
                isDefault = true;
            foreach (PackageRule rule in packageRuleList)
            {
                ItemPackageRule insertData = new ItemPackageRule();
                insertData.GenerateId();
                insertData.Code = string.Concat(rule.Code, "_", strCurrTime);
                insertData.Name = rule.Name;
                insertData.Description = rule.Description;
                insertData.PackageRuleId = rule.Id;
                insertData.ItemId = itemId;
                insertData.IsDefault = isDefault;
                double index = 1;
                foreach (PackageRuleDetail detail in rule.PackageRuleDetailList.OrderBy(f => SortExtension.GetIndex(f)))
                {
                    ItemPackageRuleDetail itemPackageRuleDetail = new ItemPackageRuleDetail();
                    itemPackageRuleDetail.GenerateId();
                    itemPackageRuleDetail.Qty = detail.Qty;
                    itemPackageRuleDetail.LevelQty = detail.LevelQty;
                    itemPackageRuleDetail.Description = detail.Description;
                    itemPackageRuleDetail.IsPackage = detail.IsPackage;
                    itemPackageRuleDetail.IsOutStockLabel = detail.IsOutStockLabel;
                    itemPackageRuleDetail.IsInStockLabel = detail.IsInStockLabel;
                    itemPackageRuleDetail.Length = detail.Length;
                    itemPackageRuleDetail.Width = detail.Width;
                    itemPackageRuleDetail.Height = detail.Height;
                    itemPackageRuleDetail.Volume = detail.Volume;
                    itemPackageRuleDetail.Weight = detail.Weight;
                    itemPackageRuleDetail.IsPrint = detail.IsPrint;
                    itemPackageRuleDetail.IsMinPacking = false;
                    itemPackageRuleDetail.NumberRuleId = detail.NumberRuleId;
                    itemPackageRuleDetail.PrintTemplateId = detail.PrintTemplateId;
                    itemPackageRuleDetail.PackageUnitId = detail.PackageUnitId;
                    itemPackageRuleDetail.ItemPackageRuleId = insertData.Id;
                    itemPackageRuleDetail.IsSequence = detail.IsSequence;
                    itemPackageRuleDetail.IsMasterUnit = detail.PackageUnit.IsMasterUnit;
                    if (item.Type == ItemType.Product || item.Type == ItemType.SemiFinished)
                    {
                        if (detail.IsMasterUnit)
                        {
                            itemPackageRuleDetail.IsSequence = true;
                        }
                        else
                        {
                            itemPackageRuleDetail.IsSequence = false;
                        }
                    }

                    itemPackageRuleDetail.SetIndex(index++);
                    insertData.ItemPackageRuleDetailList.Add(itemPackageRuleDetail);
                }
                itemPackageRuleList.Add(insertData);
            }
            if (itemPackageRuleList.Count > 0)
            {
                RF.Save(itemPackageRuleList);
            }
            return GetItemPackageRules(itemPackageRuleList.Select(p => p.Id).ToArray());
        }

        /// <summary>
        /// 获取物料包装规则列表
        /// </summary>
        /// <param name="ruleId">物料包装规则ID集合</param>
        /// <returns>物料包装规则列表</returns>
        EntityList<ItemPackageRule> GetItemPackageRules(double[] ruleId)
        {
            return Query<ItemPackageRule>().Where(p => ruleId.Contains(p.Id)).ToList(new PagingInfo { PageNumber = 1, PageSize = int.MaxValue - 1 }, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取物料下的包装规则
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <param name="keyword">关键字</param>
        /// <param name="info">分页信息</param>
        /// <returns>返回包装规则数据</returns>
        public virtual EntityList<ItemPackageRule> GetItemPackageRule(double itemId, string keyword, PagingInfo info)
        {
            return Query<ItemPackageRule>().Where(p => p.ItemId == itemId).WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword)).ToList(info, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取物料下的包装规则条数
        /// </summary>
        /// <param name="itemId">物料id</param>
        /// <returns>物料下的包装规则条数</returns>
        public virtual int GetGetItemPackageRuleCount(double itemId)
        {
            return Query<ItemPackageRule>().Where(p => p.ItemId == itemId).Count();
        }

        /// <summary>
        /// 获取物料包装规则行数（排除指定包装规则）
        /// </summary>
        /// <param name="itemId">物料ID</param>
        /// <param name="excludeRuleIds">排除指定包装规则ID</param>
        /// <returns>物料包装规则行数</returns>
        public virtual int GetItemPackageRuleCount(double itemId, List<double> excludeRuleIds)
        {
            var query = Query<ItemPackageRule>().Where(p => p.ItemId == itemId);
            if (excludeRuleIds.Any())
            {
                query.Where(p => !excludeRuleIds.Contains(p.Id));
            }
            return query.Count();
        }

        /// <summary>
        /// 获取包装规则
        /// </summary>
        /// <param name="itemPackageRuleIdList">包装规则Id集合</param>
        /// <param name="elo">贪婪记载选项</param>
        /// <returns>返回包装规则数据</returns>
        public virtual EntityList<ItemPackageRule> GetItemPackageRules(List<double> itemPackageRuleIdList, EagerLoadOptions elo = null)
        {
            return Query<ItemPackageRule>().Where(p => itemPackageRuleIdList.Contains(p.Id)).ToList(null, elo);
        }

        /// <summary>
        /// 获取包装规则
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <param name="criteria">查询实体</param>
        /// <returns>返回包装规则数据</returns>
        public virtual EntityList<ItemPackageRule> GetItemPackageRules(double itemId, CriteriaQuery criteria)
        {
            return Query<ItemPackageRule>().Where(p => p.ItemId == itemId).Where(criteria.Criteria).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 通过物料包装规则id获取物料包装规则明细列表(贪婪加载包装单位)
        /// </summary>
        /// <param name="itemPackageRuleId">物料包装规则Id</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>物料包装规则明细列表</returns>
        public virtual EntityList<ItemPackageRuleDetail> GetItemPackageRuleDetails(double itemPackageRuleId, PagingInfo pagingInfo)
        {
            return Query<ItemPackageRuleDetail>().Where(p => p.ItemPackageRuleId == itemPackageRuleId).ToList(pagingInfo, new EagerLoadOptions().LoadWith(ItemPackageRuleDetail.PackageUnitProperty).LoadWithViewProperty());
        }

        /// <summary>
        /// 通过物料包装规则id获取物料包装规则明细列表(贪婪加载包装单位)
        /// </summary>
        /// <param name="itemPackageRuleIdList">物料包装规则IdList</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>物料包装规则明细列表</returns>
        public virtual EntityList<ItemPackageRuleDetail> GetItemPackageRuleDetails(List<double> itemPackageRuleIdList, PagingInfo pagingInfo)
        {
            return Query<ItemPackageRuleDetail>().Where(p => itemPackageRuleIdList.Contains(p.ItemPackageRuleId)).ToList(pagingInfo, new EagerLoadOptions().LoadWith(ItemPackageRuleDetail.PackageUnitProperty).LoadWithViewProperty());
        }

        /// <summary>
        /// 获取包装规则明细
        /// </summary>
        /// <param name="itemPackageRuleDetailIdList">包装规则明细ID集合</param>
        /// <returns>返回包装规则明细</returns>
        public virtual EntityList<ItemPackageRuleDetail> GetItemPackageRuleDetails(List<double> itemPackageRuleDetailIdList)
        {
            return Query<ItemPackageRuleDetail>().Where(p => itemPackageRuleDetailIdList.Contains(p.Id)).ToList();
        }

        /// <summary>
        /// 获取包装方案下的主单位明细
        /// </summary>
        /// <param name="itemPackageRuleId">包装方案Id</param>
        /// <returns>返回包装方案下的主单位明细</returns>
        public virtual ItemPackageRuleDetail GetMasterItemPackageRuleDetail(double itemPackageRuleId)
        {
            return Query<ItemPackageRuleDetail>().Where(p => p.ItemPackageRuleId == itemPackageRuleId).
                Join<PackingUnit>((a, b) => a.PackageUnitId == b.Id && b.IsMasterUnit).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取物料包装方案的主单位
        /// </summary>
        /// <param name="itemId">物料ID</param>
        /// <returns>返回物料包装方案的主单位</returns>
        public virtual ItemPackageRuleDetail GetItemMasterUnit(double itemId)
        {
            var query = Query<ItemPackageRuleDetail>();
            query.Join<ItemPackageRule>((p, h) => p.ItemPackageRuleId == h.Id && h.ItemId == itemId && h.IsDefault);
            query.Join<PackingUnit>((a, b) => a.PackageUnitId == b.Id && b.IsMasterUnit);
            return query.FirstOrDefault();
        }

        /// <summary>
        /// 获取物料包装规则主单位明细数据
        /// </summary>
        /// <param name="itemPackageRuleIds">物料包装规则Id集合</param>
        /// <returns>物料包装规则主单位明细数据</returns>
        public virtual EntityList<ItemPackageRuleDetail> GetMasterUnitItemPackageRuleDetails(List<double> itemPackageRuleIds)
        {
            var query = Query<ItemPackageRuleDetail>();
            query.Join<PackingUnit>((a, b) => a.PackageUnitId == b.Id && b.IsMasterUnit);
            if (itemPackageRuleIds == null || itemPackageRuleIds.Count == 0)
            {
                return new EntityList<ItemPackageRuleDetail>();
            }
            return itemPackageRuleIds.SplitContains(sons =>
            {
                return query.Where(p => sons.Contains(p.ItemPackageRuleId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });

        }

        /// <summary>
        /// 根据物料ID集合获取包装规则明细
        /// </summary>
        /// <param name="itemIdList">物料ID集合</param>
        /// <returns>物料包装规则明细</returns>
        public virtual Dictionary<double, ItemPackageRuleDetail> GetItemsMasterUnit(List<double> itemIdList)
        {
            var query = Query<ItemPackageRuleDetail>();
            query.Join<ItemPackageRule>((p, h) => p.ItemPackageRuleId == h.Id && h.IsDefault && itemIdList.Contains(h.ItemId));
            query.Join<PackingUnit>((a, b) => a.PackageUnitId == b.Id && b.IsMasterUnit);
            var list = query.ToList();

            var itemDic = list.ToDictionary(p => p.ItemPackageRule.ItemId);
            itemIdList.ForEach(p =>
            {
                if (!itemDic.ContainsKey(p))
                {
                    itemDic.Add(p, null);
                }
            });
            return itemDic;
        }

        /// <summary>
        /// 根据包装规则Id获取设置了序列的包装规则明细
        /// </summary>
        /// <param name="itemPackageRuleId">包装规则ID</param>        
        /// <returns>返回设置了序列的包装规则明细</returns>
        public virtual ItemPackageRuleDetail GetSeqItemPackageRuleDetailByRuleId(double itemPackageRuleId)
        {
            return Query<ItemPackageRuleDetail>().Where(p => itemPackageRuleId == p.ItemPackageRuleId && p.IsSequence).FirstOrDefault();
        }

        /// <summary>
        /// 根据包装规则Id获取设置了序列的包装规则明细
        /// </summary>
        /// <param name="itemPackageRuleIdList">包装规则ID集合</param>
        /// <param name="elo">贪婪加载</param>
        /// <returns>返回设置了序列的包装规则明细</returns>
        public virtual EntityList<ItemPackageRuleDetail> GetSeqItemPackageRuleDetail(List<double> itemPackageRuleIdList, EagerLoadOptions elo = null)
        {
            return Query<ItemPackageRuleDetail>().Where(p => itemPackageRuleIdList.Contains(p.ItemPackageRuleId) && p.IsSequence).ToList(null, elo);
        }

        /// <summary>
        /// 根据包装规则Id获取设置了序列的包装规则明细
        /// </summary>
        /// <param name="itemId">物料</param>
        /// <returns>返回设置了序列的包装规则明细</returns>
        public virtual ItemPackageRuleDetail GetSeqItemPackageRuleDetail(double itemId)
        {
            return Query<ItemPackageRuleDetail>().Where(p => p.IsSequence && p.ItemPackageRule.IsDefault && p.ItemPackageRule.ItemId == itemId).FirstOrDefault();
        }

        /// <summary>
        /// 根据包装规则Id获取设置了序列的包装规则明细
        /// </summary>
        /// <param name="itemPackageRuleIdList">包装规则ID集合</param>
        /// <param name="isSequence">是否序列</param>
        /// <returns>返回设置了序列的包装规则明细</returns>
        public virtual EntityList<ItemPackageRuleDetail> GetItemPackageRuleDetailList(List<double> itemPackageRuleIdList, bool? isSequence = null)
        {
            return itemPackageRuleIdList.SplitContains(sons =>
            {
                var query = Query<ItemPackageRuleDetail>();
                if (isSequence.HasValue)
                {
                    query.Where(p => p.IsSequence == isSequence.Value);
                }
                if (itemPackageRuleIdList == null || itemPackageRuleIdList.Count == 0)
                {
                    return new EntityList<ItemPackageRuleDetail>();
                }
                return query.Where(p => sons.Contains(p.ItemPackageRuleId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取物料默认的包装规则下的是否序列的包装规则明细
        /// </summary>
        /// <param name="itemIdList">物料Id</param>
        /// <returns>返回序列的包装规则明细</returns>
        public virtual EntityList<ItemPackageRuleDetail> GetDefaultSeqPRDetail(List<double> itemIdList)
        {
            var query = Query<ItemPackageRuleDetail>().Join<ItemPackageRule>((a, b) => a.ItemPackageRuleId == b.Id)
                        .Where<ItemPackageRule>((a, b) => itemIdList.Contains(b.ItemId) && a.IsSequence);
            return query.ToList();
        }

        /// <summary>
        /// 获取物料包装规则明细上一级包装层级
        /// </summary>
        /// <param name="itemPackageRuleId">物料包装规则Id</param>
        /// <param name="packageTypeId">包装类型Id</param>
        /// <returns>物料包装规则明细</returns>
        public virtual ItemPackageRuleDetail GetItemPackageRuleDetailData(double itemPackageRuleId, double packageTypeId)
        {
            ItemPackageRuleDetail itemPackageRuleDtl = new ItemPackageRuleDetail();
            var itemPackageRuleDtlList = Query<ItemPackageRuleDetail>().Where(p => p.ItemPackageRuleId == itemPackageRuleId).OrderBy(p => p.Qty).ToList();
            ItemPackageRuleDetail itemPackageRuleDetail = Query<ItemPackageRuleDetail>().Where(p => p.ItemPackageRuleId == itemPackageRuleId && p.PackageUnitId == packageTypeId).FirstOrDefault();

            if (itemPackageRuleDtlList.Count > 0 && itemPackageRuleDetail != null)
            {
                foreach (var detail in itemPackageRuleDtlList)
                {
                    if (detail.GetIndex() > itemPackageRuleDetail.GetIndex())
                    {
                        itemPackageRuleDtl = detail;
                        break;
                    }
                }
            }

            return itemPackageRuleDtl;
        }

        /// <summary>
        /// 获取物料包装规则明细上一级包装层级
        /// </summary>
        /// <param name="itemPackageRuleId">物料包装规则Id</param>
        /// <param name="packageTypeId">包装类型Id</param>
        /// <returns>物料包装规则明细</returns>
        public virtual ItemPackageRuleDetail GetNextItemPackageRuleDetailData(double itemPackageRuleId, double packageTypeId)
        {
            ItemPackageRuleDetail itemPackageRuleDtl = new ItemPackageRuleDetail();
            var itemPackageRuleDtlList = Query<ItemPackageRuleDetail>().Where(p => p.ItemPackageRuleId == itemPackageRuleId).OrderByDescending(p => p.Qty).ToList();
            ItemPackageRuleDetail itemPackageRuleDetail = Query<ItemPackageRuleDetail>().Where(p => p.ItemPackageRuleId == itemPackageRuleId && p.PackageUnitId == packageTypeId).FirstOrDefault();

            if (itemPackageRuleDtlList.Count > 0 && itemPackageRuleDetail != null)
            {
                foreach (var detail in itemPackageRuleDtlList.OrderByDescending(p => p.Qty))
                {
                    if (detail.GetIndex() < itemPackageRuleDetail.GetIndex())
                    {
                        itemPackageRuleDtl = detail;
                        break;
                    }
                }
            }

            return itemPackageRuleDtl;
        }

        /// <summary>
        /// 获取物料包装规则明细
        /// </summary>
        /// <param name="itemPackageRuleId">物料包装规则Id</param>
        /// <param name="typeId">包装类型Id</param>
        /// <returns>物料包装规则明细</returns>
        public virtual ItemPackageRuleDetail GetItemPackageRuleDetail(double itemPackageRuleId, double typeId)
        {
            return Query<ItemPackageRuleDetail>().Where(p => p.ItemPackageRuleId == itemPackageRuleId && p.PackageUnitId == typeId).FirstOrDefault();
        }

        /// <summary>
        /// 获取包装规则下的包装单位
        /// </summary>
        /// <param name="itemPackageRuleId">包装规则Id</param>
        /// <param name="keyword">关键字</param>
        /// <param name="info">分页信息</param>
        /// <returns>返回包装规则数据</returns>
        public virtual EntityList<ItemPackageRuleDetail> GetItemPackageRuleDetail(double itemPackageRuleId, string keyword, PagingInfo info)
        {
            var query = Query<ItemPackageRuleDetail>().Where(p => p.ItemPackageRuleId == itemPackageRuleId);
            if (keyword.IsNotEmpty())
                query.Join<PackingUnit>((a, b) => a.PackageUnitId == b.Id && (b.Code.Contains(keyword) || b.Name.Contains(keyword)));
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();
            return query.ToList(info, elo);
        }

        /// <summary>
        /// 通过物料获取有序的包装单位，由里到外(主单位-箱-栈板)
        /// </summary>
        /// <param name="ItemId">物料</param>
        /// <returns>包装单位</returns>
        public virtual List<PackingUnitSortData> GetSortPackingUnitsByItemId(double ItemId)
        {
            var itemPackageRuleDtls = GetItemPackageRuleDetailsByItemId(ItemId, null, elo: new EagerLoadOptions().LoadWithViewProperty());
            List<PackingUnitSortData> rst = new List<PackingUnitSortData>();
            int index = 1;
            itemPackageRuleDtls.OrderBy(f => SortExtension.GetIndex(f)).Select(f => f.PackageUnit).ForEach(p =>
            {
                PackingUnitSortData item = new PackingUnitSortData()
                {
                    SortIndex = index,
                    PackingUnit = p,
                };
                rst.Add(item);
                index++;
            });
            return rst;
        }

        /// <summary>
        /// 通过包装规则明细获取有序的包装单位，由里到外(主单位-箱-栈板)
        /// </summary>
        /// <param name="itemPackageRuleDtls">包装规则明细</param>
        /// <returns>包装单位</returns>
        public virtual List<PackingUnitSortData> GetSortPackingUnitsByRuleDtls(EntityList<ItemPackageRuleDetail> itemPackageRuleDtls)
        {
            List<PackingUnitSortData> rst = new List<PackingUnitSortData>();
            int index = 1;
            itemPackageRuleDtls.OrderBy(f => SortExtension.GetIndex(f)).Select(f => f.PackageUnit).ForEach(p =>
            {
                PackingUnitSortData item = new PackingUnitSortData()
                {
                    SortIndex = index,
                    PackingUnit = p
                };
                rst.Add(item);
                index++;
            });
            return rst;
        }

        /// <summary>
        /// 获取包装规则明细
        /// </summary>
        /// <param name="itemPackageRuleIdList">包装规则ID</param>
        /// <returns>包装规则明细</returns>
        public virtual IList<ItemPackageRuleDtlData> GetItemPackageRuleDtlDatas(List<double> itemPackageRuleIdList)
        {
            var query = Query<ItemPackageRuleDetail>();
            query.Select(p => new
            {
                p.Id,
                p.ItemPackageRuleId,
                p.Qty,
                p.PackageUnitId,
                Index = p.GetProperty(SortExtension.INDEX_Property),
                p.NumberRuleId,
                p.IsMasterUnit
            });
            if (itemPackageRuleIdList == null || itemPackageRuleIdList.Count == 0)
            {
                return new List<ItemPackageRuleDtlData>();
            }
            IList<ItemPackageRuleDtlData> itemPackageRuleDtlDatas = new List<ItemPackageRuleDtlData>();
            itemPackageRuleIdList.SplitDataExecute(sons =>
            {

                itemPackageRuleDtlDatas = query.Where(p => sons.Contains(p.ItemPackageRuleId)).ToList<ItemPackageRuleDtlData>();
            });
            return itemPackageRuleDtlDatas;
        }

        /// <summary>
        /// 获取包装规则下的包装单位
        /// </summary>
        /// <param name="itemPackageRuleId">包装规则Id</param>
        /// <param name="keyword">关键字参数</param>
        /// <param name="info">分页信息</param>
        /// <returns>返回包装规则数据</returns>
        public virtual EntityList<ItemPackageRuleDetail> GetItemPackageUntiDetailByIsInstock(double itemPackageRuleId, string keyword, PagingInfo info)
        {
            var query = Query<ItemPackageRuleDetail>().Where(p => p.ItemPackageRuleId == itemPackageRuleId && p.IsInStockLabel);
            if (keyword.IsNotEmpty())
                query.Join<PackingUnit>((a, b) => a.PackageUnitId == b.Id && (b.Code.Contains(keyword) || b.Name.Contains(keyword)));
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();
            return query.ToList(info, elo);
        }

        /// <summary>
        /// 标签生成（按库存）根据包装规则Id获取设置了序列的包装规则明细
        /// </summary>
        /// <param name="itemPkgRuleIdList">包装规则ID集合</param>
        /// <param name="isSequence">是否序列</param>
        /// <returns>返回设置了序列的包装规则明细</returns>
        public virtual EntityList<ItemPackageRuleDetail> GetItemPkgRuleDtlList(List<double> itemPkgRuleIdList, bool? isSequence = null)
        {
            var query = Query<ItemPackageRuleDetail>();
            if (isSequence.HasValue)
            {
                query.Where(p => p.IsSequence == isSequence.Value);
            }
            if (itemPkgRuleIdList == null || itemPkgRuleIdList.Count == 0)
            {
                return new EntityList<ItemPackageRuleDetail>();
            }
            return itemPkgRuleIdList.SplitContains(sons =>
            {
                return query.Where(p => sons.Contains(p.ItemPackageRuleId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }
        #endregion

        #region 包装关系 
        /// <summary>
        /// 创建空包装
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="unit">包装单位</param>
        /// <param name="state">物流状态</param>
        /// <param name="packingBy">packingBy</param>
        /// <param name="processId">processId</param>
        /// <param name="stationId">stationId</param>
        /// <returns>包装关系</returns>
        public virtual PackingRelation CreateEmptyPackage(double workOrderId, PackingUnit unit, LogisticState state = LogisticState.UnPrinted, double? packingBy = null, double? processId = null, double? stationId = null)
        {
            if (unit == null)
                throw new ArgumentNullException(nameof(unit));
            if (processId.HasValue && processId <= 0)
                throw new ArgumentNullException(nameof(processId), "请指定一个合法的工序,{0} = {1}".L10nFormat(nameof(processId), processId));
            if (stationId.HasValue && processId <= 0)
                throw new ArgumentNullException(nameof(stationId), "请指定一个合法的工位,{0} = {1}".L10nFormat(nameof(stationId), stationId));
            var externalPackage = new PackingRelation()
            {
                ItemQty = 0,
                PackageNo = null,
                PackageUnitId = unit.Id,
                PackedDate = RF.Find<PackingRelation>().GetDbTime(),
                PackedQty = 0,
                PackingBy = packingBy ?? AppRuntime.IdentityId,
                State = state,
                ProcessId = processId ?? 0,
                StationId = stationId ?? 0,
                WorkOrderId = workOrderId
            };
            externalPackage.GenerateId();
            externalPackage.RootId = externalPackage.Id;
            RF.Save(externalPackage);
            return externalPackage;
        }

        /// <summary>
        /// 创建存在包装号空包装
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="unitId">包装单位ID</param>
        /// <param name="packageNo">包装号</param>
        /// <param name="state">物流状态</param>
        /// <param name="packingBy">packingBy</param>
        /// <param name="processId">processId</param>
        /// <param name="stationId">stationId</param>
        /// <returns>包装关系</returns>
        public virtual PackingRelation CreateEmptyPackage(double workOrderId, double unitId, string packageNo, LogisticState state = LogisticState.UnPrinted, double? packingBy = null, double? processId = null, double? stationId = null)
        {
            if (packageNo.IsNullOrEmpty())
                throw new ArgumentNullException("包装号不能为空".L10N());
            if (processId.HasValue && processId <= 0)
                throw new ArgumentNullException(nameof(processId), "请指定一个合法的工序,{0} = {1}".L10nFormat(nameof(processId), processId));
            if (stationId.HasValue && processId <= 0)
                throw new ArgumentNullException(nameof(stationId), "请指定一个合法的工位,{0} = {1}".L10nFormat(nameof(stationId), stationId));
            var externalPackage = new PackingRelation()
            {
                ItemQty = 0,
                PackageNo = packageNo,
                PackageUnitId = unitId,
                PackedDate = RF.Find<PackingRelation>().GetDbTime(),
                PackedQty = 0,
                PackingBy = packingBy ?? AppRuntime.IdentityId,
                State = state,
                ProcessId = processId ?? 0,
                StationId = stationId ?? 0,
                WorkOrderId = workOrderId
            };
            externalPackage.GenerateId();
            externalPackage.RootId = externalPackage.Id;
            RF.Save(externalPackage);
            return externalPackage;
        }

        /// <summary>
        /// 往上找上级包装
        /// </summary>
        /// <param name="unitId">当前包装</param>
        /// <param name="toUnitId">目的包装</param>
        /// <returns></returns>
        public virtual BatchPackingRelation GetParentRelationByUnitId(BatchPackingRelation unitId, double toUnitId)
        {
            if (unitId.PackageUnitId == toUnitId)
            {
                return unitId;//找到上级Id
            }
            else if (unitId.TreePId == null || unitId.TreePId == 0)
            {
                return null;//找不到
            }
            else
            {
                var par = RF.GetById<BatchPackingRelation>(unitId.TreePId);
                return GetParentRelationByUnitId(par, toUnitId);
            }
        }

        /// <summary>
        /// 递归往上找到所有上级条码
        /// </summary>
        /// <param name="relation">包装关系</param>
        /// <param name="rst">包装关系</param>
        public virtual void GetParentRelationByLeafRelation(PackingRelation relation, EntityList<PackingRelation> rst)
        {
            if (relation == null || rst.FirstOrDefault(p => p.Id == relation.Id) != null) return;
            if (relation.TreePId == null || relation.TreePId == 0)
                rst.Add(relation);
            else
            {
                rst.Add(relation);
                var par = RF.GetById<BatchPackingRelation>(relation.TreePId);
                GetParentRelationByLeafRelation(par, rst);
            }
        }

        /// <summary>
        /// 递归获取最底层的包装
        /// </summary>
        /// <param name="entitylist">当前包装列表</param>        
        /// <returns>包装关系列表</returns>
        public virtual EntityList<BatchPackingRelation> GetStoreRelationByUnitId(EntityList<BatchPackingRelation> entitylist)
        {
            var relationids = entitylist.Select(p => p.Id).ToList();
            var sonEntityList = GetPackingRelationListByParId(relationids);
            //找到底不再找
            if (sonEntityList == null || sonEntityList.Count == 0)
                return entitylist;
            return GetStoreRelationByUnitId(sonEntityList);
        }

        /// <summary>
        /// 根据父集合找到底下所有枝叶， packUnitId不为空，则只返回该层级的包装关系
        /// </summary>
        /// <param name="entitylist">父集合</param>
        /// <param name="packUnitId">包装层级单位Id</param>
        /// <returns>子孙集合</returns>
        public virtual EntityList<BatchPackingRelation> GetAllRelationByParents(EntityList<BatchPackingRelation> entitylist, double? packUnitId = null)
        {
            var parIds = entitylist.Select(p => p.TreePId).ToList();
            var relationids = entitylist.Where(p => !parIds.Contains(p.Id)).Select(p => p.Id).ToList();
            //下面没有子列表返回
            var sonList = GetPackingRelationListByParId(relationids);
            if (sonList.Count == 0) return entitylist;
            //packUnitId不为空，则只需要该层级的包装关系
            if (packUnitId != null && sonList[0].PackageUnitId == packUnitId) return sonList;
            entitylist.AddRange(sonList);
            return GetAllRelationByParents(entitylist, packUnitId);
        }

        /// <summary>
        /// 根据包装号获取包装关系
        /// </summary>
        /// <param name="packingNo">packingNo</param>       
        /// <returns>包装关系</returns>
        public virtual BatchPackingRelation GetPackingRelationByPackNo(string packingNo)
        {
            return Query<BatchPackingRelation>().Where(p => p.PackageNo == packingNo).FirstOrDefault();
        }

        /// <summary>
        /// 根据父ID获取关系列表
        /// </summary>
        /// <param name="treePids">父ID</param>
        /// <returns>包装关系</returns>
        public virtual EntityList<BatchPackingRelation> GetPackingRelationListByParId(List<double> treePids)
        {
            return Query<BatchPackingRelation>().Where(p => p.TreePId > 0 && treePids.Contains((double)p.TreePId))
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取其中一条包装关系
        /// </summary>
        /// <param name="workorderId">工单Id</param>
        /// <param name="unitId">包装Id</param>
        /// <returns>包装关系</returns>
        public virtual BatchPackingRelation GetPackingRelationByWorkOrderId(double workorderId, double unitId)
        {
            return Query<BatchPackingRelation>().Where(p => p.WorkOrderId == workorderId && p.PackageUnitId == unitId).FirstOrDefault();
        }

        /// <summary>
        /// 获取新包装单位的包装条码
        /// </summary>
        /// <param name="workorderId">工单Id</param>
        /// <param name="newUnitId">新包装单位Id</param>
        /// <returns>列表数据</returns>
        public virtual EntityList<BatchPackingRelation> GetPackRelations(double workorderId, double newUnitId)
        {
            return Query<BatchPackingRelation>().Where(p => p.WorkOrderId == workorderId && p.PackageUnitId == newUnitId).ToList();
        }

        /// <summary>
        /// 获取包装关系下的所有符合包装单位的包装条码
        /// </summary>
        /// <param name="workorderId">工单Id</param>
        /// <param name="newUnitId">新包装单位Id</param>
        /// <param name="packRelation">包装关系</param>
        /// <returns>列表数据</returns>
        public virtual EntityList<BatchPackingRelation> GetPackRelations(double workorderId, double newUnitId, PackingRelation packRelation)
        {
            //不需要循环找，因为在newUnitId上一层的时候就会触发，然后再上一层触发就不用返回数据了
            return Query<BatchPackingRelation>().Where(p => p.WorkOrderId == workorderId && p.PackageUnitId == newUnitId && p.TreePId == packRelation.Id).ToList();
        }
        #endregion

        #region PDA接口
        /// <summary>
        /// 包装拆分 打散接口
        /// </summary>
        /// <param name="barcode">包装条码</param>
        /// <returns>PackObject</returns>
        [ApiService("包装拆分->打散")]
        public virtual PackObject ScatterPack(string barcode)
        {
            if (barcode == null)
            {
                throw new ArgumentNullException(nameof(barcode));
            }

            var packRelation = GetPackingRelation(barcode);
            if (packRelation == null)
            {
                throw new ValidationException("包装条码[{0}]不存在".L10nFormat(barcode));
            }

            //验证 包装条码 是否顶层关系RootId是否自己ID  
            if (packRelation.Id != packRelation.RootId)
            {
                throw new ValidationException("包装条码[{0}]不是顶层包装".L10nFormat(barcode));
            }
            //TODO没有TreeChildrenList
            //查找包装关系必须是根ID， 查找其下一级包装关系，并将其根ID设置为自己的ID
            ////var packRelations = packRelation.TreeChildrenList.OfType<PackingRelation>().ToList();
            List<PackInfo> packInfos = new List<PackInfo>();
            PackObject packObject = new PackObject();
            packObject.PackInfos = packInfos;

            return packObject;
        }

        /// <summary>
        /// 拆分包装接口
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <returns>包装信息</returns>
        [ApiService("包装拆分->拆分")]
        public virtual PackInfo SplitPack(string barcode)
        {
            if (barcode == null)
            {
                throw new ArgumentNullException(nameof(barcode));
            }

            var packRelation = GetPackingRelation(barcode);
            bool packFlag = true; //包装标志
            ItemLabel itemLabel = null;
            if (packRelation == null)
            {
                packFlag = false;
                var ctl = RT.Service.Resolve<ItemLabelController>();
                itemLabel = ctl.GetItemLabel(barcode);
            }

            if (!packFlag && itemLabel == null)
            {
                throw new ValidationException("条码[{0}]不存在".L10nFormat(barcode));
            }

            if (packFlag && packRelation.Id == packRelation.RootId)
            {
                throw new ValidationException("包装条码[{0}]不是非顶层包装".L10nFormat(barcode));
            }

            PackInfo packInfo = new PackInfo();
            using (var tran = DB.TransactionScope(PackageEntityDataProvider.ConnectionStringName))
            {
                if (packFlag)
                {
                    //拆出来的包装关系，设置其根ID为自身，其原来的上层包装关系 包装数减一
                    packRelation.RootId = packRelation.Id;

                    //TODO没有TreeChildrenList
                    packInfo.Barcode = packRelation.PackageNo;
                    packInfo.Qty = packRelation.PackedQty;
                    packInfo.Unit = packRelation.PackageUnit.Name;
                }
                else
                {
                    // 如果是物料标签，将其包装关系删除，对应的包装关系数量减一
                    var parentPackRelation = itemLabel.Relation;
                    parentPackRelation.ItemQty -= 1;
                    itemLabel.Relation = null;
                    RF.Save(parentPackRelation);
                    RF.Save(itemLabel);
                    packInfo.Barcode = itemLabel.Label;
                    packInfo.Qty = itemLabel.Qty;
                    packInfo.Unit = itemLabel.Unit.Name;
                }

                tran.Complete();
            }

            return packInfo;
        }

        /// <summary>
        /// 获取包装关系
        /// </summary>
        /// <param name="barcode">包装条码</param>
        /// <returns>包装关系</returns>
        public virtual PackingRelation GetPackingRelation(string barcode)
        {
            if (barcode == null)
            {
                throw new ArgumentNullException(nameof(barcode));
            }

            var query = Query<PackingRelation>();
            query.Where(p => p.PackageNo == barcode);
            return query.FirstOrDefault();
        }

        /// <summary>
        /// 获取序列号包装层级数据
        /// </summary>
        /// <param name="itemcode">物料编码</param>
        /// <returns>包装层级数据</returns>
        [ApiService("获取序列号包装层级数据")]
        public virtual PackInfo GetItemSeqPackInfo([ApiParameter("物料")] string itemcode)
        {
            PackInfo rst = new PackInfo();
            var ruleDetail = Query<ItemPackageRuleDetail>().Where(p => p.ItemPackageRule.IsDefault && p.ItemPackageRule.Item.Code == itemcode && p.IsSequence).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (ruleDetail != null)
            {
                rst.Unit = ruleDetail.PackageUnitName;
                rst.Qty = ruleDetail.Qty;
            }
            return rst;
        }
        #endregion
    }

    /// <summary>
    /// 包装信息
    /// </summary>
    [Serializable]
    public class PackInfo
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }
    }

    /// <summary>
    /// 包装接口传参对象
    /// </summary>
    public class PackObject
    {
        /// <summary>
        /// 包装信息列表
        /// </summary>
        public List<PackInfo> PackInfos { get; set; } = new List<PackInfo>();
    }
}
