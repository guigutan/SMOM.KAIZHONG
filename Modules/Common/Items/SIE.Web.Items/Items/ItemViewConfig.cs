
using NPOI.SS.Formula.Functions;
using SIE.Domain;
using SIE.Items;
using SIE.Items.Items;
using SIE.MetaModel.View;
using SIE.Web.Common;
using SIE.Web.Common.Commands;
using SIE.Web.Items.Items;
using SIE.Web.Items.Items.Commands;
using System;

namespace SIE.Web.Items
{
    /// <summary>
    /// 物料视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class ItemViewConfig : WebViewConfig<Item>
    {
        /// <summary>
        /// The item controller.
        /// </summary>
        internal ItemController itemController = RT.Service.Resolve<ItemController>();

        #region 扩展视图
        /// <summary>
        /// 分类子视图
        /// </summary>
        public const string SmallCategoryView = "SmallCategoryView";
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
            View.AddBehavior("SIE.Web.Items.Items.Behaviors.ItemClearBehavior");
            View.DeclareExtendViewGroup(new string[] { BaseDataViewGroup, DesignDataViewGroup, PurchasingDataViewGroup, LogisticsDataViewGroup, SmallCategoryView });

            if (ViewGroup == BaseDataViewGroup)
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
            View.UseCommands(typeof(ItemAddCommand).FullName, WebCommandNames.Edit, typeof(ItemDeleteCommand).FullName,
                typeof(ItemSaveCommand).FullName);
            View.ReplaceCommands(EnableCommand.CommandName, typeof(ItemEnableCommand).FullName);
            View.ReplaceCommands(DisableCommand.CommandName, typeof(ItemDisableCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).HasLabel("物料编码").Readonly(p => p.PersistenceStatus != PersistenceStatus.New).FixColumn()
                    .UseListSetting(e => { e.HelpInfo = "根据(配置项--物料编码生成规则)生成物料编码,配置项为空可手动编辑,编辑后变为只读状态！"; });
                View.Property(p => p.Name).HasLabel("物料名称");
                View.Property(p => p.SpecificationModel);
                View.Property(p => p.UnitId).HasLabel("基本计量单位".L10N()+"*");
                View.Property(p => p.Type).HasLabel("基本分类").ShowInList(width: 90)//.Readonly(p => p.PersistenceStatus != PersistenceStatus.New)
                    .DefaultValue((int)ItemType.Product).UseEnumEditor(p =>
                    {
                        p.AllowBlank = false;
                        p.XType = "itemTypeComboList";
                    });
                View.Property(p => p.Mtart).Show().Readonly();
                View.Property(p => p.ItemSourceType).ShowInList(width: 90);

                View.Property(p => p.State).Readonly();
                View.Property(p => p.SourceType);
                //在添加属性时自动勾选上，清空自动取消勾选
                //// View.Property(p => p.EnableExtendProperty).Readonly(p => p.PersistenceStatus == PersistenceStatus.New).UseCheckEditor(p => p.ColumnXType = "enableExtendPropertyCheckEditor");
                View.Property(p => p.EnableExtendProperty).UseCheckDropDownEditor().ShowInList(120).Readonly().HasLabel("是否有扩展属性");
                View.Property(p => p.MinPackingQty).Show();
                View.Property(p => p.GroupState).Show();
                View.Property(p => p.FactoryState).Show();
                View.Property(p => p.MrpController).Show();
                View.Property(p => p.SuccessorItem).Show();
                View.Property(p => p.SuccessorEffeTime).Show();
                View.Property(p => p.Zmc).Show();
                View.Property(p => p.Normt).Show();
                View.Property(p => p.Zkhxhgy).Show();
                View.Property(p => p.Zjdlx).Show();
                View.Property(p => p.Zmodel).Show();
                View.Property(p => p.Zgg).Show();
                View.AttachDetailChildrenProperty(typeof(Item), (c) =>
                {
                    var item = c.Parent as Item;
                    item = RF.GetById<Item>(item.Id, new EagerLoadOptions().LoadWithViewProperty());
                    return item;
                }, BaseDataViewGroup).HasLabel("基本资料").OrderNo = 10;
                View.AssociateChildrenProperty(ItemExtCategoryListProperty.ItemCategoryListProperty, e =>
                {
                    var item = e.Parent as Item;
                    if (item == null)
                    {
                        return new EntityList<ItemCategoryRelation>();
                    }
                    return this.itemController.GetItemCategoryRelation(item.Id, (e as ChildPagingDataArgs)?.PagingInfo);
                }).HasLabel("分类维护").OrderNo = 11;
                View.AttachDetailChildrenProperty(typeof(Item), (c) =>
                {
                    var item = c.Parent as Item;
                    item = RF.GetById<Item>(item.Id, new EagerLoadOptions().LoadWithViewProperty());
                    return item;
                }, DesignDataViewGroup).HasLabel("设计资料").OrderNo = 12;
                View.AttachDetailChildrenProperty(typeof(Item), (c) =>
                {
                    var item = c.Parent as Item;
                    item = RF.GetById<Item>(item.Id, new EagerLoadOptions().LoadWithViewProperty());
                    return item;
                }, PurchasingDataViewGroup).HasLabel("采购资料").OrderNo = 13;
                View.ChildrenProperty(p => p.UnitList).UseViewGroup(ItemUnitViewConfig.TabView).HasLabel("转换单位").OrderNo = 20;
                View.AssociateChildrenProperty(LabelPrintDetailProperty.LabelPrintTemProperty, e =>
                {
                    var item = e.Parent as Item;
                    if (item == null)
                    {
                        var template = new SIE.Core.Items.LabelPrintTemplate();
                        template.GenerateId();
                        return template;
                    }
                    return RT.Service.Resolve<ItemController>().GetTemplateByItemId(item.Id);
                }, LabelPrintTemplateViewConfig.LabelPrintTemplateView).HasLabel("打印设置").OrderNo = 29;
                View.ChildrenProperty(p => p.PropertyValueList).HasLabel("扩展属性").OrderNo = 30;
                View.AttachChildrenProperty(typeof(ItemLog), (e) =>
                {
                    var item = e.Parent as Item;
                    if (item == null)
                    {
                        return new EntityList<ItemLog>();
                    }
                    return this.itemController.GetItemLogs(item.Id);
                }).HasLabel("其他").OrderNo = 31;
                View.AssociateChildrenProperty(ItemExtBatchRule.BatchRuleProperty, (e) =>
                {
                    var item = e.Parent as Item;
                    if (item == null)
                        return new SIE.Core.Items.ItemBatchRule() { RetrospectType = RT.Service.Resolve<ItemController>().GetRetrospectTypeConfig(), Item = item };
                    var batch = RT.Service.Resolve<ItemController>().GetBatchRule(item.Id);
                    if (batch == null)
                    {
                        batch = new SIE.Core.Items.ItemBatchRule() { RetrospectType = RT.Service.Resolve<ItemController>().GetRetrospectTypeConfig(), Item = item };
                    }
                    return batch;
                }, ItemBatchRuleViewConfig.DetailsView).HasLabel("生产批次规则").OrderNo = 32;
                View.ChildrenProperty(p => p.ItemCusotmerRelation).Show(ChildShowInWhere.All).OrderNo = 33;
                View.ChildrenProperty(p => p.ParentItemList).Show(ChildShowInWhere.All).OrderNo = 34;
                View.ChildrenProperty(p => p.CustomFeatureRelList).Show(ChildShowInWhere.All).OrderNo = 35;
            }
        }

        /// <summary>
        /// 默认下拉选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Description);
            View.Property(p => p.Type);
            View.Property(p => p.UnitName);
        }

        /// <summary>
        /// 默认查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name);
                View.Property(p => p.SpecificationModel);
                View.Property(p => p.Type).UseEnumEditor(p => p.AllowBlank = true);
                View.Property(p => p.ItemSourceType).UseEnumEditor(p => p.AllowBlank = true);
                View.Property(p => p.State).UseEnumEditor(p => p.AllowBlank = true);
                View.Property(p => p.SourceType).UseEnumEditor(p => p.AllowBlank = true);
                View.Property(p => p.PurchasingAgent);
            }
        }

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
                View.AddBehavior("SIE.Web.Items.Items.Behaviors.BaseItemBehavior");
                View.Property(p => p.SecondUnit).UseDataSource((e, p, s) =>
                {
                    var entity = e as Item;
                    if (entity.Id > 0 && entity.UnitId > 0)
                        return RT.Service.Resolve<ItemUnitController>().GetSecondUnits(entity.Id, s, p, entity.UnitId.Value, false);
                    else
                        return new EntityList<Unit>();
                }).Show(ShowInWhere.All);
                View.Property(p => p.IsMarked).Show(ShowInWhere.All);
                View.Property(p => p.IsLabel).Show(ShowInWhere.All);
                View.Property(p => p.Description).Show(ShowInWhere.All);
                View.Property(p => p.EnglishDescription).Show(ShowInWhere.All);
                View.Property(p => p.ShortDescription).Show(ShowInWhere.All);
                View.Property(p => p.Precision).Show(ShowInWhere.Hide);
                View.Property(p => p.BaseCode).Show(ShowInWhere.All);
                View.Property(p => p.ProductFamily).Show(ShowInWhere.All);
                View.Property(p => p.Model).Show(ShowInWhere.All);
                View.Property(p => p.IsVirtualPart).Show(ShowInWhere.All);
                View.Property(p => p.ConsumeMode).Show(ShowInWhere.All);
                View.Property(p => p.DrawingNo).Show(ShowInWhere.All);
                View.Property(p => p.GoodsBarcode).Show(ShowInWhere.All);
                View.Property(p => p.ABCCategory).Show(ShowInWhere.All);
                View.Property(p => p.ProductLeadDay).Show(ShowInWhere.All).Readonly(p => p.ItemSourceType == ItemSourceType.Outsourcing)
                    .DefaultValue(null).UseSpinEditor(p => p.MinValue = 1).HasLabel("生产提前期（天）");
                View.Property(p => p.TechParamCategory).UseListSetting(e => { e.HelpInfo = "工艺参数分类快码类型(TECH_PARAM_CATEGORY)"; }).Show(ShowInWhere.All)
                    .UseCatalogEditor(e => { e.CatalogType = "TECH_PARAM_CATEGORY";e.CatalogReloadData = true; });
                ////View.Property(ItemProcessTechRouteExt.ProcessTechRouteProperty).HasLabel("制程路线").UseDataSource((e, p, r) =>
                ////{
                ////    return RT.Service.Resolve<TechRouteController>().GetTechRoute();
                ////}).Show(ShowInWhere.All);

                View.Property(p => p.ExcessReportRatio).Show(ShowInWhere.All).UseSpinEditor(p => { p.MinValue = 0; p.DecimalPrecision = 2; });
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
                View.Property(p => p.Length).ShowInDetail(columnSpan: 2);
                View.Property(p => p.Width).ShowInDetail(columnSpan: 2);
                View.Property(p => p.Height).ShowInDetail(columnSpan: 3);
                View.Property(p => p.Photo);
                View.Property(p => p.Volume).ShowInDetail(columnSpan: 4);
                View.Property(p => p.Weight).UseSpinEditor(p => p.MinValue = 0).ShowInDetail(columnSpan: 4);
                View.Property(p => p.LowerWeight).ShowInDetail(columnSpan: 4);
                View.Property(p => p.UpperWeight).ShowInDetail(columnSpan: 4);
                View.Property(p => p.WeightUnit).ShowInDetail(columnSpan: 4);
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
                View.Property(p => p.PurchasingGroup).Show(ShowInWhere.All);
                View.Property(p => p.PurchasingAgent).Show(ShowInWhere.All);
                View.Property(p => p.PurchaseLeadTime).Show(ShowInWhere.All).HasLabel("采购提前期（天）");
            }
        }

        /// <summary>
        /// 分类子视图
        /// </summary>
        protected void ConfigSmallCategoryView()
        {
            View.ClearCommands();
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).HasLabel("物料编码").Show();
                View.Property(p => p.Name).HasLabel("物料名称").Show();
                View.Property(p => p.SpecificationModel).Show();
                View.Property(p => p.Unit).HasLabel("基本计量单位").Show();
                View.Property(p => p.Type).HasLabel("基本分类").Show();
                View.Property(p => p.ItemSourceType).Show();
                View.Property(p => p.State).HasLabel("状态").Show();
                View.Property(p => p.SourceType).Show();
            }
        }

        #endregion
    }

    /// <summary>
    /// 替代料视图配置
    /// </summary>
    internal class AlternativeViewConfig : WebViewConfig<Item>
    {
        protected readonly static string AlternativeView = "AlternativeView";

        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(AlternativeView);
            if (ViewGroup == AlternativeView)
            {
                using (View.OrderProperties())
                {
                    View.Property(p => p.Code).ShowInList(150).Readonly();
                    View.Property(p => p.Name).ShowInList(150).Readonly();
                    View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                    View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                    View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                    View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                    View.ChildrenProperty(p => p.UnitList).IsVisible = false;
                    View.ChildrenProperty(p => p.PropertyValueList).IsVisible = false;
                }
            }
        }
    }

    /// <summary>
    /// 替代料查询视图配置
    /// </summary>
    internal class AlternativeCriteriaViewConfig : WebViewConfig<AlternativeCriteria>
    {
      
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.All);
                View.Property(p => p.Name).Show(ShowInWhere.All);
            }
        }
    }
}
