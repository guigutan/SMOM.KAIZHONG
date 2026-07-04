using SIE.Domain;
using SIE.TurnoverTools.TurnoverTools;
using SIE.MetaModel.View;
using SIE.Packages.Boxs;
using SIE.Web.Common;
using SIE.Web.Elec.MES.TurnoverTools.Commands;
using SIE.Web.MES.TurnoverTools.Commands;
using System.Collections.Generic;

namespace SIE.Web.Kit.TurnoverTools.TurnoverTools
{
    /// <summary>
    /// 周转工具视图配置
    /// </summary>
    internal class TurnoverToolViewConfig : WebViewConfig<TurnoverTool>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommand(typeof(TurnoverToolImportCommand).FullName);
            View.UseDefaultCommands().ReplaceCommands(WebCommandNames.Delete, typeof(TurnoverToolDeleteCommand).FullName)
                .ReplaceCommands(WebCommandNames.Copy, "SIE.Web.Elec.MES.TurnoverTools.Commands.TurnoverToolCopyCommand");
            View.UseCommands(typeof(TurnoverToolRecoveryCommand).FullName, typeof(TurnoverToolRepairCommand).FullName, typeof(TurnoverToolScrapCommand).FullName);
            View.UseCommands("SIE.Web.Kit.TurnoverTools.Commands.TurnoverToolModelTableCommand");
            View.Property(p => p.Code).Readonly(p => p.PersistenceStatus != PersistenceStatus.New);
            View.Property(p => p.Name).ShowInList(width: 150);
            View.Property(p => p.ToolType).UseCatalogEditor(e => { e.CatalogType = TurnoverBox.BoxTypeCatalog; e.CatalogReloadData = true; }).Cascade(p => p.ModelId, null);
            View.Property(p => p.ModelId).HasLabel("周转工具型号").UseDataSource((e, c, r) =>
            {
                var entity = e as TurnoverTool;
                return RT.Service.Resolve<KitTurnoverToolController>().GetTurnoverToolModels(entity.ToolType, r, c);
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.ToolType), nameof(e.Model.ToolType));
                m.DicLinkField = keyValues;
            });
            View.Property(p => p.State).Readonly();
            View.AttachChildrenProperty(typeof(TurnoverToolBinding), (e) =>
            {
                var arg = e as ChildPagingDataArgs;
                var parent = arg.Parent as TurnoverTool;
                if (parent == null)
                    return new EntityList<TurnoverToolBinding>();
                return RT.Service.Resolve<KitTurnoverToolController>().GetTurnoverTools(parent.Id, false, arg.PagingInfo, arg.SortInfo);
            });
            View.AttachChildrenProperty(typeof(TurnoverToolActionLog), (e) =>
            {
                var arg = e as ChildPagingDataArgs;
                var parent = arg.Parent as TurnoverTool;
                if (parent == null)
                    return new EntityList<TurnoverToolActionLog>();
                return RT.Service.Resolve<KitTurnoverToolController>().GetTurnoverToolActionLogs(parent.Id, arg.PagingInfo, arg.SortInfo);
            });
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.ToolType).UseCatalogEditor(e => { e.CatalogType = TurnoverBox.BoxTypeCatalog; e.CatalogReloadData = true; });
            View.Property(p => p.ModelId).HasLabel("周转工具型号");
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            // 配置默认视图
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            // 配置默认视图
        }
    }
}