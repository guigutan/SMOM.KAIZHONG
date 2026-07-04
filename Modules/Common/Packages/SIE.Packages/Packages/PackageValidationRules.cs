using SIE.Common.NumberRules;
using SIE.Common.Sort;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MetaModel;
using System;
using System.ComponentModel;
using System.Linq;

namespace SIE.Packages
{
    #region 包装规则明细验证
    /// <summary>
    /// 包装规则不能拥有重复的包装载体
    /// </summary>
    [DisplayName("包装规则明细验证规则")]
    [Description("包装规则不能拥有重复的包装单位")]
    public class PackingUnitAndPackageRuleNotDuplicateRule : NotDuplicateRule<PackageRuleDetail>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PackingUnitAndPackageRuleNotDuplicateRule()
        {
            Properties.Add(PackageRuleDetail.PackageRuleIdProperty);
            Properties.Add(PackageRuleDetail.PackageUnitIdProperty);
            MessageBuilder = (e) =>
            {
                var entity = e as PackageRuleDetail;
                return "已经存在包装单位[{0}]".L10nFormat(entity.PackageUnit.Code);
            };
        }
    }

    /// <summary>
    /// 包装规则只能添加一个主单位
    /// </summary>
    [DisplayName("包装规则验证规则")]
    [Description("包装明细层级验证，验证主单位、级别")]
    public class MasterUnitInPackageRuleLevelRule : EntityRule<PackageRule>
    {
        /// <summary>
        /// 包装规则只能添加一个主单位，层级数量是递增
        /// </summary>
        /// <param name="entity">包装规则</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var d = entity as PackageRule;
            var master = d.PackageRuleDetailList.OrderBy(f => SortExtension.GetIndex(f)).ElementAtOrDefault(0)?.PackageUnit?.IsMasterUnit;

            if (master.HasValue && !master.Value)
            {
                e.BrokenDescription = "包装[{0}]主单位必须是第一个".L10nFormat(d.Code);
                return;
            }
            var details = d.PackageRuleDetailList.OrderBy(f => SortExtension.GetIndex(f)).ToList();
            if (details.Count < 2)
            {
                e.BrokenDescription = "包装[{0}]必须包含两个以上包装单位".L10nFormat(d.Code);
                return;
            }
            for (int i = 1; i < details.Count; i++)
            {
                if (details[i].Qty % details[i - 1].Qty != 0)
                {
                    e.BrokenDescription = "包装[{0}]外包装产品数量不能小于内包装产品数量".L10nFormat(d.Code);
                    break;
                }
            }
            details.Where(p => p.IsSequence).ForEach(p =>
             {
                 if (p.PackageUnit.IsMasterUnit) return;
                 //不是主单位勾选了是否序列，需要验证 是否入库标签列必须有一个包装层级的值为True，并且这个包装层级必须大于等于是否序列的包装层级。
                 var hasInStockLable = details.FirstOrDefault(f => SortExtension.GetIndex(f) >= SortExtension.GetIndex(p) && f.IsInStockLabel);
                 if (hasInStockLable == null)
                 {
                     e.BrokenDescription = "包装[{0}]有非主单位勾选了是否序列，则是否入库标签必须勾选一个不低于是否序列的包装层级！".L10nFormat(d.Code);
                 }
             });
        }
    }

    /// <summary>
    /// 包装单位编码规则验证规则
    /// </summary>
    [DisplayName("包装单位编码规则验证规则")]
    [Description("包装单位编码规则不能为空")]
    public class PackageUnitNumberRuleRequire : PropertyRule<PackageRuleDetail>
    {
        /// <summary>
        /// 属性
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return PackageRuleDetail.NumberRuleIdProperty;
            }
        }

        /// <summary>
        /// 限制实体的某一个或几个属性的值在数据库中不存在的规则。
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var packageDtl = entity as PackageRuleDetail;
            if (packageDtl.PackageUnit != null && !packageDtl.PackageUnit.IsMasterUnit && packageDtl.NumberRule == null)
            {
                e.BrokenDescription = "包装单位[{0}]编码规则不能为空".L10nFormat(packageDtl.PackageUnit.Code);

            }
        }
    }

    /// <summary>
    /// 包装单位模板验证规则
    /// </summary>
    [DisplayName("包装单位模板验证规则")]
    [Description("包装单位模板不能为空")]
    public class PackageUnitTemplateRequire : PropertyRule<PackageRuleDetail>
    {
        /// <summary>
        /// 属性
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return PackageRuleDetail.PrintTemplateIdProperty;
            }
        }

        /// <summary>
        /// 限制实体的某一个或几个属性的值在数据库中不存在的规则。
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var packageDtl = entity as PackageRuleDetail;
            if (packageDtl.PackageUnit != null && !packageDtl.PackageUnit.IsMasterUnit && packageDtl.IsPrint && packageDtl.PrintTemplate == null)
            {
                e.BrokenDescription = "包装单位[{0}]需要打印，必须设置打印模板".L10nFormat(packageDtl.PackageUnit.Code);

            }
        }
    }

    /// <summary>
    /// 包装单位数量大于0验证规则
    /// </summary>
    [DisplayName("包装单位数量大于0验证规则")]
    [Description("包装单位数量必须大于0")]
    public class PackageUnitQtyRule : PropertyRule<PackageRuleDetail>
    {
        /// <summary>
        /// 属性
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return PackageRuleDetail.QtyProperty;
            }
        }

        /// <summary>
        /// 限制实体的某一个或几个属性的值在数据库中不存在的规则。
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var packageDtl = entity as PackageRuleDetail;
            if (packageDtl.Qty <= 0)
            {
                e.BrokenDescription = "产品数必须大于0".L10N();
            }
        }
    }

    /// <summary>
    /// 包装单位包装数量大于0验证规则
    /// </summary>
    [DisplayName("包装单位包装数量大于0验证规则")]
    [Description("包装单位包装数量必须大于0")]
    public class PackageUnitLevelQtyRule : PropertyRule<PackageRuleDetail>
    {
        /// <summary>
        /// 属性
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return PackageRuleDetail.LevelQtyProperty;
            }
        }

        /// <summary>
        /// 限制实体的某一个或几个属性的值在数据库中不存在的规则。
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var packageDtl = entity as PackageRuleDetail;
            if (packageDtl.LevelQty <= 0)
            {
                e.BrokenDescription = "包装数必须大于0".L10N();
            }
        }
    }

    /// <summary>
    /// 包装规则明细只能有一个入库标签验证规则
    /// </summary>
    [DisplayName("包装规则明细只能有一个入库标签验证规则")]
    [Description("包装规则明细只能有一个入库标签")]
    public class PackRuleMultInStockLabelRule : EntityRule<PackageRule>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PackRuleMultInStockLabelRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var packageRule = entity as PackageRule;
            if (packageRule.PackageRuleDetailList.Count(p => p.IsInStockLabel) > 1)
                e.BrokenDescription = "包装规则只能存在一个入库单位".L10N();
        }
    }
    #endregion

    #region 物料包装规则明细验证
    /// <summary>
    /// 物料包装规则不能拥有重复的包装载体
    /// </summary>
    [DisplayName("物料包装规则明细验证规则")]
    [Description("物料包装规则不能拥有重复的包装单位")]
    public class PUnitAndItemPackageRuleNotDuplicateRule : NotDuplicateRule<ItemPackageRuleDetail>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PUnitAndItemPackageRuleNotDuplicateRule()
        {
            Properties.Add(ItemPackageRuleDetail.ItemPackageRuleIdProperty);
            Properties.Add(ItemPackageRuleDetail.PackageUnitIdProperty);
            MessageBuilder = (e) =>
            {
                var entity = e as ItemPackageRuleDetail;
                return "已经存在包装单位[{0}]".L10nFormat(entity.PackageUnit.Code);
            };
        }
    }

    /// <summary>
    /// 物料包装规则只能添加一个主单位
    /// </summary>
    [DisplayName("物料包装规则验证规则")]
    [Description("物料包装明细层级验证，验证主单位、级别")]
    public class MUnitInItemPackageRuleLevelRule : EntityRule<ItemPackageRule>
    {
        /// <summary>
        /// 包装规则验证规则
        /// </summary>
        public MUnitInItemPackageRuleLevelRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update | EntityStatusScopes.Delete;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 物料包装规则只能添加一个主单位，层级数量是递增
        /// </summary>
        /// <param name="entity">包装规则</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            EntityList<ItemPackageRuleDetail> itemPackRuleDetails = new EntityList<ItemPackageRuleDetail>();
            var d = entity as ItemPackageRule;
            var oldItemPackRuleDetails = RT.Service.Resolve<PackageController>().GetItemPackageRuleDetails(d.Id, null);
            var deletedDetails = d.ItemPackageRuleDetailList.DeletedList.OfType<ItemPackageRuleDetail>().ToList();
            itemPackRuleDetails.AddRange(d.ItemPackageRuleDetailList);

            foreach (var oldItemPackRuleDetail in oldItemPackRuleDetails)
            {
                if (deletedDetails.Any(p => p.Id == oldItemPackRuleDetail.Id)) continue;
                var newItemPackRule = itemPackRuleDetails.FirstOrDefault(p => p.Id == oldItemPackRuleDetail.Id && p.Id != 0);
                if (newItemPackRule == null)
                    itemPackRuleDetails.Add(oldItemPackRuleDetail);
            }

            var master = itemPackRuleDetails.OrderBy(f => SortExtension.GetIndex(f)).ElementAtOrDefault(0)?.PackageUnit?.IsMasterUnit;
            if (master.HasValue && !master.Value)
            {
                e.BrokenDescription = "主单位必须是第一个".L10N();
                return;
            }

            var details = itemPackRuleDetails.OrderBy(f => SortExtension.GetIndex(f)).ToList();
            if (details.Count == 1)
            {
                e.BrokenDescription = "包装必须包含两个以上包装单位".L10N();
                return;
            }
            for (int i = 1; i < details.Count; i++)
            {
                if (details[i].Qty < details[i - 1].Qty)
                {
                    e.BrokenDescription = "外包装数量不能小于内包装数量".L10N();

                    break;
                }
            }

            details.Where(p => p.IsSequence).ForEach(p =>
            {
                if (p.PackageUnit.IsMasterUnit) return;
                //不是主单位勾选了是否序列，需要验证 是否入库标签列必须有一个包装层级的值为True，并且这个包装层级必须大于等于是否序列的包装层级。
                var hasInStockLable = details.FirstOrDefault(f => SortExtension.GetIndex(f) >= SortExtension.GetIndex(p) && f.IsInStockLabel);
                if (hasInStockLable == null)
                {
                    e.BrokenDescription = "包装[{0}]有非主单位勾选了是否序列，则是否入库标签必须勾选一个不低于是否序列的包装层级！".L10nFormat(d.Code);
                }
            });

            if ((d.Item.Type== Items.ItemType.Product|| d.Item.Type== Items.ItemType.SemiFinished)&& details.All(p => !p.IsInStockLabel))
            {
                e.BrokenDescription = "包装层级的[是否入库]必须勾选一个".L10N();
            }
        }

        /// <summary>
        /// 包装单位编码规则验证规则
        /// </summary>
        [DisplayName("包装单位编码规则验证规则")]
        [Description("包装单位编码规则不能为空")]
        public class ItemPackageUnitNumberRuleRequire : PropertyRule<ItemPackageRuleDetail>
        {
            /// <summary>
            /// 属性
            /// </summary>
            protected override IManagedProperty Property
            {
                get
                {
                    return ItemPackageRuleDetail.NumberRuleIdProperty;
                }
            }

            /// <summary>
            /// 限制实体的某一个或几个属性的值在数据库中不存在的规则。
            /// </summary>
            /// <param name="entity">实体</param>
            /// <param name="e">规则参数</param>
            protected override void Validate(IEntity entity, RuleArgs e)
            {
                var packageDtl = entity as ItemPackageRuleDetail;
                if (packageDtl.PackageUnit != null && !packageDtl.PackageUnit.IsMasterUnit && packageDtl.NumberRule == null)
                {
                    e.BrokenDescription = "包装单位[{0}]条码规则不能为空".L10nFormat(packageDtl.PackageUnit.Code);
                }
            }
        }

        /// <summary>
        /// 包装单位模板验证规则
        /// </summary>
        [DisplayName("包装单位模板验证规则")]
        [Description("包装单位模板不能为空")]
        public class ItemPackageUnitTemplateRequire : PropertyRule<ItemPackageRuleDetail>
        {
            /// <summary>
            /// 属性
            /// </summary>
            protected override IManagedProperty Property
            {
                get
                {
                    return ItemPackageRuleDetail.PrintTemplateIdProperty;
                }
            }

            /// <summary>
            /// 限制实体的某一个或几个属性的值在数据库中不存在的规则。
            /// </summary>
            /// <param name="entity">实体</param>
            /// <param name="e">规则参数</param>
            protected override void Validate(IEntity entity, RuleArgs e)
            {
                var packageDtl = entity as ItemPackageRuleDetail;
                if (packageDtl.PackageUnit != null && !packageDtl.PackageUnit.IsMasterUnit && packageDtl.IsPrint && packageDtl.PrintTemplate == null)
                {
                    e.BrokenDescription = "包装单位[{0}]需要打印，必须设置打印模板".L10nFormat(packageDtl.PackageUnit.Code);
                }
            }
        }

        /// <summary>
        /// 包装单位数量大于0验证规则
        /// </summary>
        [DisplayName("包装单位数量大于0验证规则")]
        [Description("包装单位数量必须大于0")]
        public class ItemPackageUnitQtyRule : PropertyRule<ItemPackageRuleDetail>
        {
            /// <summary>
            /// 属性
            /// </summary>
            protected override IManagedProperty Property
            {
                get
                {
                    return ItemPackageRuleDetail.QtyProperty;
                }
            }

            /// <summary>
            /// 限制实体的某一个或几个属性的值在数据库中不存在的规则。
            /// </summary>
            /// <param name="entity">实体</param>
            /// <param name="e">规则参数</param>
            protected override void Validate(IEntity entity, RuleArgs e)
            {
                var packageDtl = entity as ItemPackageRuleDetail;
                if (packageDtl.Qty <= 0)
                {
                    e.BrokenDescription = "产品数必须大于0".L10N();
                }
            }
        }
    }

        /// <summary>
        /// 包装单位数量大于0验证规则
        /// </summary>
        [DisplayName("包装单位验证规则")]
        [Description("包装单位不能为空")]
        public class ItemPackageUnitRule : PropertyRule<ItemPackageRuleDetail>
        {
            /// <summary>
            /// 属性
            /// </summary>
            protected override IManagedProperty Property
            {
                get
                {
                    return ItemPackageRuleDetail.PackageUnitProperty;
                }
            }

            /// <summary>
            /// 限制实体的某一个或几个属性的值在数据库中不存在的规则。
            /// </summary>
            /// <param name="entity">实体</param>
            /// <param name="e">规则参数</param>
            protected override void Validate(IEntity entity, RuleArgs e)
            {
                var packageDtl = entity as ItemPackageRuleDetail;
                if (packageDtl.PackageUnit == null)
                {
                    e.BrokenDescription = "报错位置：物料[{0}] - [物料包装规则]页签[{1}] - [主信息页签]".L10nFormat(packageDtl.ItemPackageRule.Item.Code, packageDtl.ItemPackageRule.Code);
            }
            }
        }

        /// <summary>
        /// 包装数量大于0验证规则
        /// </summary>
        [DisplayName("包装数量大于0验证规则")]
        [Description("包装数量必须大于0")]
        public class ItemPackageUnitLevelQtyRule : PropertyRule<ItemPackageRuleDetail>
        {
            /// <summary>
            /// 属性
            /// </summary>
            protected override IManagedProperty Property
            {
                get
                {
                    return ItemPackageRuleDetail.LevelQtyProperty;
                }
            }

            /// <summary>
            /// 限制实体的某一个或几个属性的值在数据库中不存在的规则。
            /// </summary>
            /// <param name="entity">实体</param>
            /// <param name="e">规则参数</param>
            protected override void Validate(IEntity entity, RuleArgs e)
            {
                var packageDtl = entity as ItemPackageRuleDetail;
                if (packageDtl.LevelQty <= 0)
                {
                    e.BrokenDescription = "包装数必须大于0".L10N();
                }
            }
        }

        /// <summary>
        /// 编码规则被物料包装规则明细引用不允许删除
        /// </summary>
        [System.ComponentModel.DisplayName("编码规则引用不允许删除")]
        [System.ComponentModel.Description("编码规则被物料包装规则明细引用不允许删除")]
        public class UndeletePackageRuleNumber : NoReferencedRule<NumberRule>
        {
            /// <summary>
            /// 构造函数
            /// </summary>
            public UndeletePackageRuleNumber()
            {
                Properties.Add(ItemPackageRuleDetail.NumberRuleIdProperty);
                MessageBuilder = (o, e) =>
                {
                    var item = o as NumberRule;
                    return "编码规则[{0}]已经被[{1}]引用，不能删除".L10nFormat(item.Code, "物料包装规则明细".L10N());
                };
            }
        }
        #endregion
    }
