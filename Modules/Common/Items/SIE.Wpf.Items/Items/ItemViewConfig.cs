using SIE.Domain;
using SIE.Items;
using SIE.Items.Items;
using SIE.Wpf.Common.Commands;
using SIE.Wpf.Items.Items.Commands;
using System.Collections.Generic;

namespace SIE.Wpf.Items
{
    /// <summary>
    /// 物料视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class ItemViewConfig : WPFViewConfig<Item>
    {
        #region 扩展视图
        /// <summary>
        /// 分类子视图
        /// </summary>
        public const string SmallCategoryView = "SmallCategoryView";

        /// <summary>
        /// 扩展视图
        /// </summary>
        public const string PatrolInspBillProductSelectionView = "PatrolInspBillProductSelectionView";

        /// <summary>
        /// 基本资料
        /// </summary>
        public const string BaseDataViewGroup = "BaseDataViewGroup";

        /// <summary>
        /// 设置资料
        /// </summary>
        public const string DesignDataViewGroup = "DesignDataViewGroup";

        /// <summary>
        /// 采购资料
        /// </summary>
        public const string PurchasingDataViewGroup = "PurchasingDataViewGroup";

        /// <summary>
        /// 物流资料
        /// </summary>
        public const string LogisticsDataViewGroup = "LogisticsDataViewGroup";

        #endregion

        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(Item.CodeProperty);
            View.DeclareExtendViewGroup(new string[] { PatrolInspBillProductSelectionView, BaseDataViewGroup, DesignDataViewGroup, PurchasingDataViewGroup, LogisticsDataViewGroup, SmallCategoryView });
            if (ViewGroup == PatrolInspBillProductSelectionView)
            {
                ConfigInspSelectProduct();
            }
            else if (ViewGroup == BaseDataViewGroup)
            {
                View.DomainName("物料基本资料");
                BaseDataView();
            }
            else if (ViewGroup == DesignDataViewGroup)
            {
                View.DomainName("物料设置资料");
                DesignDataView();
            }
            else if (ViewGroup == PurchasingDataViewGroup)
            {
                View.DomainName("物料采购资料");
                PurchasingDataView();
            }
            else if (ViewGroup == SmallCategoryView)
            {
                ConfigSmallCategoryView();
            }

        }

        /// <summary>
        /// 默认表格视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DomainName("物料");
            View.InlineEdit();
            View.UseCommands(typeof(ItemAddCommand), WPFCommandNames.ListEdit, typeof(ItemDeleteCommand), typeof(ItemSaveCommand));
            View.ReplaceCommands(typeof(EnableCommand), typeof(ItemEnableCommand));
            View.ReplaceCommands(typeof(DisableCommand), typeof(ItemDisableCommand));
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).HasLabel("物料编码").Readonly();
                View.Property(p => p.Name).HasLabel("物料名称");
                View.Property(p => p.SpecificationModel);
                View.Property(p => p.Unit).HasLabel("基本计量单位");
                View.Property(p => p.Type).HasLabel("基本分类").UseListSetting(c => c.ListGridWidth = 90d);
                View.Property(p => p.ItemSourceType).UseListSetting(c => c.ListGridWidth = 90d);
                View.Property(p => p.State).HasLabel("状态").UseListSetting(c => c.ListGridWidth = 70d).Readonly();
                View.Property(p => p.SourceType).UseListSetting(c => c.ListGridWidth = 70d);
                View.AttachDetailChildrenProperty(typeof(Item), (c) =>
                { return c.Parent as Item; }, BaseDataViewGroup).HasLabel("基本资料").OrderNo = 5;
                View.AssociateChildrenProperty(ItemExtCategoryListProperty.ItemCategoryListProperty, e =>
                {
                    var item = e.Parent as Item;
                    if (item == null) return new EntityList<ItemCategoryRelation>();
                    return RT.Service.Resolve<ItemController>().GetItemCategoryRelation(item.Id, (e as ChildPagingDataArgs)?.PagingInfo);
                }).HasLabel("分类维护").OrderNo = 8;
                View.AttachDetailChildrenProperty(typeof(Item), (c) =>
                { return c.Parent as Item; }, DesignDataViewGroup).HasLabel("设计资料").OrderNo = 10;
                View.AttachDetailChildrenProperty(typeof(Item), (c) =>
                { return c.Parent as Item; }, PurchasingDataViewGroup).HasLabel("采购资料").OrderNo = 15;
                View.ChildrenProperty(Item.UnitListProperty).HasLabel("转换单位").OrderNo = 20;
                View.AssociateChildrenProperty(LabelPrintDetailProperty.LabelPrintTemProperty, e =>
                {
                    var item = e.Parent as Item;
                    if (item != null && item.Template == null)
                    {
                        var template = new Core.Items.LabelPrintTemplate();
                        template.GenerateId();
                        item.Template = template;
                    }

                    return item?.Template;
                }, LabelPrintTemplateViewConfig.LabelPrintTemplateView).HasLabel("打印设置").Show(ChildShowInWhere.List).OrderNo = 60;
                View.ChildrenProperty(Item.PropertyValueListProperty).HasLabel("扩展属性").OrderNo = 65;
                View.AssociateChildrenProperty(ItemExtBatchRule.BatchRuleProperty, (e) =>
                {
                    var item = e.Parent as Item;
                    if (item == null)
                        return new Core.Items.ItemBatchRule() { RetrospectType = RT.Service.Resolve<ItemController>().GetRetrospectTypeConfig(), Item = item };
                    var batch = RT.Service.Resolve<ItemController>().GetBatchRule(item.Id);
                    if (batch == null)
                    {
                        batch = new Core.Items.ItemBatchRule() { RetrospectType = RT.Service.Resolve<ItemController>().GetRetrospectTypeConfig(), Item = item };
                    }
                    return batch;
                }).HasLabel("生产批次规则").OrderNo = 68;

                View.AttachChildrenProperty(typeof(ItemLog), (e) =>
                {
                    var item = e.Parent as Item;
                    if (item == null) return new EntityList<ItemLog>();
                    return RT.Service.Resolve<ItemController>().GetItemLogs(item.Id);
                }).HasLabel("其他").OrderNo = 70;
            }
        }

        /// <summary>
        /// 默认表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            // 视图配置
        }

        /// <summary>
        /// 默认下拉选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code).HasLabel("编码");
            View.Property(p => p.Name).HasLabel("名称");
            View.Property(p => p.SpecificationModel).HasLabel("规则型号");
            View.Property(p => p.Description).HasLabel("描述");
            View.Property(p => p.MinPackingQty).HasLabel("最小包装数");
            View.Property(p => p.Type).HasLabel("类型");
        }

        /// <summary>
        /// 默认查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.DomainName("物料明细");
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).HasLabel("物料编码");
                View.Property(p => p.Name).HasLabel("物料名称");
                View.Property(p => p.SpecificationModel);
                View.Property(p => p.Type).HasLabel("基本分类");
                View.Property(p => p.ItemSourceType);
                View.Property(p => p.State).HasLabel("状态");
                View.Property(p => p.SourceType);
                View.Property(p => p.UpdateDate).HasLabel("更新时间").UseDateRangeEditor(p =>
                { p.DateTimePart = ObjectModel.DateTimePart.Date; p.DateRangeType = ObjectModel.DateRangeType.Month; });
                ////View.Property(DataEntityExtension.UpdateByNameProperty).HasLabel("更新人");
                View.Property(p => p.PurchasingAgent).HasLabel("采购员");
            }
        }

        /// <summary>
        /// PatrolInspBillProductSelectionView对应视图配置
        /// </summary>
        void ConfigInspSelectProduct()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).HasLabel("编码").Show();
                View.Property(p => p.Name).HasLabel("名称").Show();
                View.Property(p => p.Description).HasLabel("描述").Show();
                View.Property(p => p.MinPackingQty).HasLabel("最小包装数").Show();
                View.Property(p => p.Type).HasLabel("类型").Show();
                View.Property(p => p.ConsumeMode).HasLabel("物料消耗类型").Show();
                View.Property(p => p.State).HasLabel("状态").Show();
                View.Property(p => p.DrawingNo).HasLabel("图号").Show();
                View.Property(p => p.Version).HasLabel("图号版本").Show();
                View.Property(p => p.Model).HasLabel("产品机型").Show();
                View.Property(p => p.BaseModel).HasLabel("基准机型").Show();
                View.Property(p => p.ItemLabelType).HasLabel("物料标签类型").Show();
                View.Property(p => p.Unit).HasLabel("计量单位").Show();
                View.Property(p => p.Weight).HasLabel("标准重量").Show();
                View.Property(p => p.UpperWeight).HasLabel("上偏差").Show();
                View.Property(p => p.LowerWeight).HasLabel("下偏差").Show();
                View.Property(p => p.PurchasingAgent).HasLabel("采购员").Show();
                View.Property(p => p.Person).HasLabel("责任人").Show();
                View.Property(p => p.MrpPerson).HasLabel("MRP控制者").Show();
                View.Property(p => p.SourceType).HasLabel("数据来源").Show();
            }
        }

        void ConfigSmallCategoryView()
        {
            View.ClearCommands();
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).HasLabel("物料编码").Show();
                View.Property(p => p.Name).HasLabel("物料名称").Show();
                View.Property(p => p.SpecificationModel).Show();
                View.Property(p => p.Unit).HasLabel("基本计量单位").Show();
                View.Property(p => p.Type).HasLabel("基本分类").UseListSetting(c => c.ListGridWidth = 90d).Show();
                View.Property(p => p.ItemSourceType).UseListSetting(c => c.ListGridWidth = 90d).Show();
                View.Property(p => p.State).HasLabel("状态").UseListSetting(c => c.ListGridWidth = 70d).Show();
                View.Property(p => p.SourceType).UseListSetting(c => c.ListGridWidth = 70d).Show();

                //View.ChildrenProperty(Item.UnitListProperty).HasLabel("转换单位").Visible(false);

                //View.ChildrenProperty(Item.PropertyValueListProperty).HasLabel("扩展属性").Visible(false);
            }
        }

        ///// <summary>
        ///// 历史经验库弹出视图
        ///// </summary>
        //void HistoryItemView()
        //{
        //    View.ClearCommands();
        //    View.DisableEditing();
        //    using (View.OrderProperties())
        //    {
        //        View.Property(p => p.Code).HasLabel("物料编码").Show();
        //        View.Property(p => p.Name).HasLabel("物料名称").Show();
        //        View.Property(p => p.SpecificationModel).Show();
        //        View.Property(p => p.Unit).HasLabel("基本计量单位").Show();
        //        View.Property(p => p.Type).HasLabel("基本分类").UseListSetting(c => c.ListGridWidth = 90d).Show();
        //        View.Property(p => p.ItemSourceType).UseListSetting(c => c.ListGridWidth = 90d).Show();
        //        View.Property(p => p.State).HasLabel("状态").UseListSetting(c => c.ListGridWidth = 70d).Show();
        //        View.Property(p => p.SourceType).UseListSetting(c => c.ListGridWidth = 70d).Show();
        //    }
        //}

        #region 子页签

        /// <summary>
        /// 资本资料
        /// </summary>
        protected void BaseDataView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.HasDetailColumnsCount(2);
                View.Property(p => p.Description).HasLabel("物料描述").Show(ShowInWhere.All);
                View.Property(p => p.EnglishDescription).Show(ShowInWhere.All);
                View.Property(p => p.ShortDescription).Show(ShowInWhere.All);
                View.Property(p => p.Precision).Show(ShowInWhere.All);
                View.Property(p => p.BaseCode).HasLabel("基准编码").Show(ShowInWhere.All);
                View.Property(p => p.Model).HasLabel("产品机型").UsePagingLookUpEditor(e =>
                {
                    e.ReloadDataOnPopping = true;
                    e.QueryMembers = new List<ManagedProperty.IManagedProperty> { ProductModel.CodeProperty, ProductModel.NameProperty };
                }).Show(ShowInWhere.All);
                View.Property(p => p.ConsumeMode).HasLabel("物料供应类型").Show(ShowInWhere.All);
                View.Property(p => p.DrawingNo).Show(ShowInWhere.All);
                View.Property(p => p.GoodsBarcode).Show(ShowInWhere.All);
            }
        }

        /// <summary>
        /// 设计资料
        /// </summary>
        protected void DesignDataView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.HasDetailColumnsCount(10);
                View.Property(p => p.Length).Show(ShowInWhere.All).ShowInDetail(columnSpan: 2);
                View.Property(p => p.Width).Show(ShowInWhere.All).ShowInDetail(columnSpan: 2);
                View.Property(p => p.Height).Show(ShowInWhere.All).ShowInDetail(columnSpan: 3);
                View.Property(p => p.Photo).UseImageEditor().Show(ShowInWhere.All).UseFormSetting(p => { p.RowSpan = 6; p.ColumnsSpan = 3; });
                View.Property(p => p.Volume).Show(ShowInWhere.All).ShowInDetail(columnSpan: 4);
                View.Property(p => p.Weight).Show(ShowInWhere.All).ShowInDetail(columnSpan: 4);
            }
        }

        /// <summary>
        /// 采购资料
        /// </summary>
        protected void PurchasingDataView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.PurchasingGroup).HasLabel("采购组").Show(ShowInWhere.All);
                View.Property(p => p.PurchasingAgent).HasLabel("采购员").Show(ShowInWhere.All).UseWorkGroupEmployeeLookUpEditor(p => p.ReloadDataOnPopping = true);
                View.Property(p => p.PurchaseLeadTime).Show(ShowInWhere.All);
            }
        }
        #endregion
    }

    /// <summary>
    /// 替代料视图配置
    /// </summary>
    public class AlternativeViewConfig : WPFViewConfig<Item>
    {
        /// <summary>
        /// 设置非默认视图
        /// </summary>
        public const string AlternativeView = "AlternativeView";

        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(AlternativeView);
            if (ViewGroup == AlternativeView)
            {
                //View.ClearCommands();
                //View.UseCommands(typeof(UnSelectedAllCommand));
                using (View.OrderProperties())
                {
                    View.Property(p => p.Code).HasLabel("编码").Show(ShowInWhere.All);
                    View.Property(p => p.Name).HasLabel("名称").Show(ShowInWhere.All);
                    View.ChildrenProperty(p => p.UnitList).IsVisible = false;
                    View.ChildrenProperty(p => p.PropertyValueList).IsVisible = false;
                }
            }
        }
    }

    /// <summary>
    /// 替代料查询视图配置
    /// </summary>
    internal class AlternativeCriteriaViewConfig : WPFViewConfig<AlternativeCriteria>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).HasLabel("编码").Show(ShowInWhere.All);
                View.Property(p => p.Name).HasLabel("名称").Show(ShowInWhere.All);
            }
        }
    }
}