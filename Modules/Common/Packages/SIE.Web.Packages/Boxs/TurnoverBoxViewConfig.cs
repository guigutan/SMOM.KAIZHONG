using SIE.Common.Configs;
using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Packages.Boxs;
using SIE.Packages.Boxs.Configs;
using SIE.Web.Common;
using SIE.Web.Packages.Boxs.Commands;
using System;

namespace SIE.Web.Packages.Boxs
{
    /// <summary>
    /// 周转箱视图配置
    /// </summary>
    internal class TurnoverBoxViewConfig : WebViewConfig<TurnoverBox>
    {
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            string distributionTurnoverBoxType = "配送周转箱";

            var config = ConfigService.GetConfig(new ProductTrunoverBoxTypeConfig());

            if (config != null)
            {
                distributionTurnoverBoxType = config.BoxType;
            }

            View.InlineEdit();
            View.ClearCommands();
            View.UseCommands("SIE.Web.Packages.Boxs.Commands.TurnoverBoxAddCommand", "SIE.Web.Packages.Boxs.Commands.TurnoverBoxEditCommand", WebCommandNames.Save);
            View.UseCommands("SIE.Web.Packages.Boxs.Commands.TurnoverBoxDeleteCommand");
            View.UseCommands("SIE.Web.Packages.Boxs.Commands.TurnoverBoxCopyCommand");
            View.UseCommands("SIE.Web.Packages.Boxs.Commands.TurnoverBoxScrapCommand");
            View.UseImportCommands();
            View.UseCommands(WebCommandNames.ExportXls);
            View.Property(p => p.Code).ShowInList(width: 150).Readonly(f => f.PersistenceStatus != PersistenceStatus.New)
                .UseListSetting(e => { e.HelpInfo = "新增状态可编辑".L10N(); });
            View.Property(p => p.Type).ShowInList(width: 150).UseCatalogEditor(e => { e.CatalogType = TurnoverBox.BoxTypeCatalog; e.CatalogReloadData = true; })
                .UseListSetting(e => { e.HelpInfo = "周转箱快码类型(BOX_TYPE),快码编码是“物流周转箱”的将会同步到垛表".L10N(); });
            View.Property(p => p.State).Readonly().DefaultValue((int)BoxState.Unused);
            
            //配送周转箱只读
            View.Property(p => p.Capacity).UseSpinEditor(p =>
            {
                p.MinValue = 1;
                p.MaxValue = 1000000;
            }).DefaultValue(1).Readonly(p => p.Type == distributionTurnoverBoxType);

            View.Property(p => p.TrunoverBoxModel).Readonly(p => p.CreateBy > 0);
            View.ChildrenProperty(p => p.CapacityList);
            View.AttachChildrenProperty(typeof(TurnoverBoxBinding), (e) =>
            {
                var arg = e as ChildPagingDataArgs;
                var parent = arg.Parent as TurnoverBox;
                if (parent == null)
                    return new EntityList<TurnoverBoxBinding>();
                return RT.Service.Resolve<BoxController>().GetTurnoverBoxs(parent.Id, false, arg.PagingInfo, arg.SortInfo);
            });
            View.AttachChildrenProperty(typeof(TurnoverBoxActionLog), (e) =>
            {
                var arg = e as ChildPagingDataArgs;
                var parent = arg.Parent as TurnoverBox;
                if (parent == null)
                    return new EntityList<TurnoverBoxActionLog>();
                return RT.Service.Resolve<BoxController>().GetTurnoverBoxActionLogs(parent.Id, arg.PagingInfo, arg.SortInfo);
            });
        }

        /// <summary>
        /// 选择视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Type);
                View.Property(p => p.Type).UseCatalogEditor(p => { p.CatalogType = TurnoverBox.BoxTypeCatalog; p.CatalogReloadData = true; })
                    .UseListSetting(e => { e.HelpInfo = "周转箱快码类型(BOX_TYPE)"; });
                View.Property(p => p.State);
            }
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Type);
                View.Property(p => p.Type).UseCatalogEditor(p => { p.CatalogType = TurnoverBox.BoxTypeCatalog; p.CatalogReloadData = true; })
                    .UseListSetting(e => { e.HelpInfo = "周转箱快码类型(BOX_TYPE)"; });
                View.Property(p => p.State);
            }
        }

        /// <summary>
        /// 导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.Code).HasLabel("条码");
            View.Property(p => p.Type).HasLabel("类型");
            View.Property(p => p.State).HasLabel("状态");
            View.Property(p => p.State).HasLabel("默认容量");
            View.PropertyRef(p => p.TrunoverBoxModel.Code).HasLabel("周转箱型号");
        }
    }
}