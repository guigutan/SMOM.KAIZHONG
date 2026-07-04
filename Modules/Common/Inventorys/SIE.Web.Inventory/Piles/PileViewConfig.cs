using SIE.Domain;
using SIE.Inventory.Piles;
using SIE.MetaModel.View;
using SIE.Web.Inventory.Piles.Commands;
using System;

namespace SIE.WPF.Inventory.Piles
{
    /// <summary>
    /// 垛表视图配置
    /// </summary>
    internal class PileViewConfig : WebViewConfig<Pile>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseClientOrder();
            View.DisableEditing();
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            View.UseCommands(typeof(BatchGeneratePileCommand).FullName, typeof(PilePrintCommand).FullName);
            View.Property(p => p.Code);
            View.Property(p => p.Model);
            View.Property(p => p.PileState);
            View.Property(p => p.TurnoverContainer).Readonly();
            View.Property(p => p.BillNo);
            View.Property(p => p.BusinessType);
            View.Property(p => p.CurLocation);
            View.Property(p => p.Weight);
            View.Property(p => p.Length);
            View.Property(p => p.Width);
            View.Property(p => p.Height);
            View.Property(p => p.ItemState);
            View.ChildrenProperty(p => p.PileDetailList).Visible(false);
            View.AttachChildrenProperty(typeof(PileDetailViewModel), w =>
            {
                var args = w as ChildPagingDataArgs;
                var pile = args.Parent.CastTo<Pile>();
                if (pile == null)
                    return new EntityList<PileDetailViewModel>();
                return RT.Service.Resolve<PileController>().GetPileDetailViewModels(pile.Id, args.SortInfo, args.PagingInfo);
            });
            View.AttachChildrenProperty(typeof(PileLog), w =>
            {
                var args = w as ChildPagingDataArgs;
                var pile = args.Parent.CastTo<Pile>();
                if (pile == null)
                    return new EntityList<PileLog>();
                return RT.Service.Resolve<PileController>().GetPileLogs(pile.Id, args.SortInfo, args.PagingInfo);
            });
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Model);
            View.Property(p => p.PileState);
            View.Property(p => p.TurnoverContainer);
            View.Property(p => p.BillNo);
            View.Property(p => p.BusinessType);
            View.Property(p => p.CurLocation);
            View.Property(p => p.Weight);
            View.Property(p => p.Length);
            View.Property(p => p.Width);
            View.Property(p => p.Height);
            View.Property(p => p.ItemState);
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Model);
            View.Property(p => p.PileState).UseEnumEditor(p => p.AllowBlank = true);
            View.Property(p => p.TurnoverContainer);
            View.Property(p => p.BillNo);
            View.Property(p => p.BusinessType).UseEnumEditor(p => p.AllowBlank = true);
            View.Property(p => p.CurLocation);
            View.Property(p => p.ItemState);
        }
    }
}