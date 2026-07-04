using IronPython.Compiler.Ast;
using SIE.Core.ProjectMaintains;
using SIE.Domain;
using SIE.Items;
using SIE.MES.Routings.RoutingSettings;
using SIE.MES.RoutingSettings;
using SIE.MetaModel.View;
using SIE.Web.MES.Routings.RoutingSettings.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.MES.RoutingSettings
{
    /// <summary>
    /// 产品工艺路线视图配置
    /// </summary>
    internal class ProductRoutingViewConfig : WebViewConfig<ProductRouting>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
		protected override void ConfigView()
        {
            View.UseDefaultCommands();
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseImportCommands();
            View.ReplaceCommands(WebCommandNames.Save, typeof(ProductRoutingSaveCommand).FullName);
            View.Property(p => p.OrderType);
            View.Property(p => p.Product).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add(nameof(e.ProductName), nameof(e.Product.Name));
                m.DicLinkField = dic;
            }).UseDataSource((source, pagingInfo, keyword) =>
            {
                List<ItemType> itemTypeList = new List<ItemType>() { ItemType.Product, ItemType.SemiFinished };
                List<int> itemTypeValueList = itemTypeList.Select(p => (int)p).ToList();
                return RT.Service.Resolve<ItemController>().GetItemsFormType(itemTypeValueList, State.Enable, keyword, pagingInfo);
            }).UseListSetting(e => { e.HelpInfo = "显示半成品可用的物料信息"; }).ShowInList(150);
            View.Property(p => p.ProductName).HasLabel("产品名称").ShowInList(150).Readonly();
            View.Property(p => p.Routing).UsePagingLookUpEditor();
            View.Property(p => p.ProcessSegment);
            View.Property(p => p.ProjectMaintain).UseDataSource((e, p, k) =>
            {
                return RT.Service.Resolve<ProjectMaintainController>().GetProjectMaintains(p, k);
            }).Show();
            View.Property(p => p.RoutingVersion).Readonly().Show();
            View.Property(p => p.StartDate).DefaultValue(DateTime.Now.ToString("yyyy/MM/dd")).UseDateEditor();
            View.Property(p => p.EndDate).UseDateEditor();
            View.AttachChildrenProperty(typeof(RoutingVersionViewModel), obj =>
            {
                var arg = obj as ChildPagingDataArgs;
                var productRouting = arg.Parent as ProductRouting;
                if (productRouting == null)
                    return new EntityList<RoutingVersionViewModel>();
                return RT.Service.Resolve<RoutingSettingController>().GetRoutingVersionViewModels(productRouting.Id, arg.SortInfo, arg.PagingInfo);
            }, ViewConfig.ListView, true).HasLabel("版本");
            View.AttachChildrenProperty(typeof(RoutingProcessViewModel), e =>
            {
                var args = e as ChildPagingDataArgs;
                var productRouting = args.Parent as ProductRouting;
                if (productRouting == null)
                    return new EntityList<RoutingProcessViewModel>();
                return RT.Service.Resolve<RoutingSettingController>().GetProductRoutingProcesses(productRouting.Id, args.SortInfo, args.PagingInfo);
            }).HasLabel("产品工艺路线明细");
        }

        /// <summary>
        /// 明细视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.OrderType);
            View.Property(p => p.Product).UsePagingLookUpEditor();
            View.Property(p => p.Routing).UsePagingLookUpEditor();
            View.Property(p => p.StartDate).UseDateEditor();
            View.Property(p => p.EndDate).UseDateEditor();
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.OrderType);
            View.Property(p => p.Product).UsePagingLookUpEditor().UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<ItemController>().GetProductItems(keyword, pagingInfo);
            });
            View.Property(p => p.Routing).UsePagingLookUpEditor();
            View.Property(p => p.StartDate).UseDateRangeEditor(e =>
            {
                e.DateRangeType = ObjectModel.DateRangeType.Month;
                e.DateFormat = "Y/m/d";
            });
            View.Property(p => p.EndDate).UseDateRangeEditor(e =>
            {
                e.DateRangeType = ObjectModel.DateRangeType.Month;
                e.DateFormat = "Y/m/d";
            });
        }

        /// <summary>
        /// 选择视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.OrderType);
            View.Property(p => p.ProductName);
            View.Property(p => p.RoutingName);
            View.Property(p => p.StartDate).UseDateEditor();
            View.Property(p => p.EndDate).UseDateEditor();
        }

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.OrderType);
            View.PropertyRef(p => p.Product.Code).HasLabel("产品编码");
            View.PropertyRef(p => p.Routing.Name).HasLabel("工艺路线名称");
            View.PropertyRef(p => p.ProcessSegment.Code).HasLabel("工段编码");
            View.Property(p => p.StartDate);
            View.Property(p => p.EndDate);
        }
    }
}
