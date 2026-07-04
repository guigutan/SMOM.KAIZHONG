using SIE.Domain;
using SIE.Items;
using SIE.MetaModel.View;
using SIE.Tech.Routings;
using SIE.MES.Routings.RoutingBoms;
using SIE.MES.Routings.RoutingBoms.ImportBoms;
using System.Collections.Generic;
using SIE.Web.Items._Extentions_;
using SIE.Core.ProjectMaintains;

namespace SIE.Web.MES.Routings.RoutingBoms
{
    /// <summary>
    /// 产品工艺路线版本视图配置
    /// </summary>
    public class RoutingBomViewConfig : WebViewConfig<RoutingBom>
    {
        /// <summary>
        /// 产品工艺路线版本视图配置
        /// </summary>
        public const string RoutingBomView = "RoutingBomView";

        /// <summary>
        /// 产品工艺路线版本视图配置
        /// </summary>
        public const string ReadonlyView = "ReadonlyView";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit();
            View.AssignAuthorize(typeof(RoutingBom));
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands("SIE.Web.MES.Routings.RoutingBoms.Commands.RoutingBomAddCommand",
                WebCommandNames.Copy,
                "SIE.Web.MES.Routings.RoutingBoms.Commands.RoutingBomEditCommand",
                WebCommandNames.Delete,
                WebCommandNames.Save, 
                "SIE.Web.MES.Routings.RoutingBoms.Commands.ImportRoutingBomCommand",
                WebCommandNames.ExportXls);
            View.Property(p => p.Product).HasLabel("产品编码").UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<ItemController>().GetEnableItemList(pagingInfo, keyword);
                })
                .UsePagingLookUpEditor((e, o) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(o.ProductName), nameof(o.Product.Name));
                    keyValues.Add(nameof(o.IsAllowEdit), nameof(o.Product.EnableExtendProperty));
                    e.DicLinkField = keyValues;
                }).ShowInList(160).Readonly(p => p.PersistenceStatus != PersistenceStatus.New);
            View.Property(p => p.ProductName).HasLabel("产品名称").Readonly().ShowInList(200);
            View.Property(p => p.ItemExtPropName).UseItemExtPropRecordsFieldEditor(p =>
            {
                p.IsAllRequired = true;
                p.ItemIdField = "ProductId";
                p.DbField = "ItemExtProp";
            }).Readonly(p => !p.IsAllowEdit).HasLabel("物料扩展属性").ShowInList(220);
            View.Property(p => p.ProjectMaintain).UseDataSource((e, p, k) =>
            {
                return RT.Service.Resolve<ProjectMaintainController>().GetProjectMaintains(p, k);
            }).Show();
            View.Property(p => p.Routing).UseDataSource((source, pagingInfo, keyword) =>
            {
                RoutingBom prv = source as RoutingBom;
                if (prv == null || prv.ProductId <= 0)
                    return new EntityList<Routing>();
                return RT.Service.Resolve<RoutingBomController>()
                .GetRoutingByProductRouting(prv.ProductId, pagingInfo, keyword);
            }).HasLabel("工艺路线").UsePagingLookUpEditor()
            .Readonly(p => p.PersistenceStatus != PersistenceStatus.New).ShowInList(200);

            View.Property(p => p.RoutingVersion)
                .UseDataSource((source, pagingInfo, keyword) =>
                {
                    RoutingBom prv = source as RoutingBom;
                    if (prv == null || prv.RoutingId <= 0)
                        return new EntityList<RoutingBom>();
                    return RT.Service.Resolve<RoutingController>().GetRoutingVersions(prv.RoutingId);
                })
                .HasLabel("版本").UsePagingLookUpEditor()
                .Readonly(p => p.PersistenceStatus != PersistenceStatus.New);

            View.Property(p => p.ProcessSegment).UseDataSource((source, pagingInfo, keyword) =>
            {
                RoutingBom prv = source as RoutingBom;
                if (prv == null || prv.RoutingId <= 0)
                    return new EntityList<SIE.Resources.ProcessSegments.ProcessSegment>();
                return RT.Service.Resolve<RoutingBomController>()
                .GetProcessSegmentByProductRouting(prv.RoutingId, prv.ProductId, pagingInfo, keyword);
            }).HasLabel("工段").UsePagingLookUpEditor().Readonly(p => p.PersistenceStatus != PersistenceStatus.New);

            View.ChildrenProperty(p => p.RoutingBomDetailList);
            View.ChildrenProperty(p => p.Attachments).Show(ChildShowInWhere.Hide);
            View.AttachChildrenProperty(typeof(RoutingBomImportRecord), (e) =>
            {
                var prv = e.Parent as RoutingBom;
                if (prv == null)
                    return new EntityList<RoutingBomImportRecord>();
                var args = e as ChildPagingDataArgs;
                return RT.Service.Resolve<RoutingBomController>().GetRoutingBomDetailImportRecordList(prv.Id, args.PagingInfo, args.SortInfo);
            }).HasLabel("导入日志").OrderNo = 2;
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Product);
            View.Property(p => p.Routing);
            View.Property(p => p.RoutingVersion);
            View.Property(p => p.ProcessSegment);
        }

        protected override void ConfigImportView()
        {
            View.Property(p => p.Product).HasLabel("产品编码");
            View.Property(p => p.Routing).HasLabel("工艺路线");
            View.Property(p => p.RoutingVersion).HasLabel("工艺路线版本");
            View.Property(p => p.ProcessSegment).HasLabel("工段");
        }
    }
}