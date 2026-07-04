using SIE.Domain;
using SIE.LES.LesStockCounts;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Web.Items._Extentions_;
using SIE.Web.LES.Extensions;
using SIE.Web.LES.LesStockCounts.Commands;
using System.Collections.Generic;

namespace SIE.Web.LES.LesStockCounts
{
    /// <summary>
    /// 盘点单明细视图配置
    /// </summary>
    public class LesStockCountDetailViewConfig : WebViewConfig<LesStockCountDetail>
    {
        /// <summary>
        /// 只读视图
        /// </summary>
        public const string ReadonlyView = "ReadonlyView";

        /// <summary>
        /// 新增盘盈可编辑
        /// </summary>
        private const string NEWINVENTORYCANEDIT = "新增盘盈可编辑";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(new string[] { ReadonlyView });
            View.AssignAuthorize(typeof(LesStockCount));
            if (ViewGroup == ReadonlyView)
            {
                ConfigReadOnlyView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.LES.LesStockCounts.Scripts.LesStockCountDetailBehavior");
            View.UseCommands(typeof(EditSCDetailCommand).FullName, typeof(DeleteSCDetailCommand).FullName, typeof(CloseSCDetailCommand).FullName, typeof(DefaultSCDetailCommand).FullName, typeof(InputSCDetailQtyCommand).FullName, typeof(AddSCDetailCommand).FullName, typeof(SCDetailImportCommand).FullName, 
                typeof(BatchAnalysisSCDetailCommand).FullName);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            View.Property(p => p.LineNo).ShowInList(width: 60).Readonly();
            View.Property(p => p.State).ShowInList(width: 60).Readonly();
            View.Property(p => p.LabelNo).ShowInList(width: 150).Readonly(p => !p.IsNewInventory
            || p.CountDimension == CountDimension.Item || p.CountDimension == CountDimension.Lot);
            View.Property(p => p.ItemId).UseSCDetailItemEditor((p, t) =>
            {
                var keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(t.ItemName), nameof(t.Item.Name));
                keyValues.Add(nameof(t.ItemSpecificationModel), nameof(t.Item.SpecificationModel));
                keyValues.Add(nameof(t.ItemUnitName), nameof(t.Item.UnitName));
                keyValues.Add(nameof(t.EnableExtPro), nameof(t.Item.EnableExtendProperty));
                p.DicLinkField = keyValues;
            }).Cascade(p => p.Lot, null).Cascade(p => p.ItemExtProp, null).Cascade(p => p.ItemExtPropName, null).Readonly(p => !p.IsNewInventory)
            .UseListSetting(e => { e.HelpInfo = "物料更改清空批次,新增盘盈可编辑"; }).ShowInList(150);
            View.Property(p => p.ItemName).Readonly().HasLabel("物料名称").ShowInList(150);
            View.Property(p => p.ItemSpecificationModel).Readonly().HasLabel("规格型号");
            View.Property(p => p.ItemExtPropName).UseItemExtPropRecordsFieldEditor(p =>
            {
                p.IsAllRequired = true;
                p.DbField = "ItemExtProp";
            }).Readonly(p => !p.EnableExtPro || p.LotId > 0).ShowInList(width: 180);
            View.Property(p => p.ItemUnitName).Readonly().ShowInList(width: 50).HasLabel("单位");
            View.Property(p => p.FactoryId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var enterpriseList = RT.Service.Resolve<EnterpriseController>().GetEmployeeFactoriesList(pagingInfo, keyword);
                if (enterpriseList == null || enterpriseList.Count <= 0)
                    return new EntityList<Enterprise>();
                for (var i = 0; i < enterpriseList.Count; i++)
                {
                    enterpriseList[i].TreePId = null;
                }
                return enterpriseList;
            }).Readonly(p => !p.IsNewInventory
              || p.CountDimension != CountDimension.Location);
            View.Property(p => p.WarehouseId).UseSCDetailWarehouseEditor()
                .Cascade(p => p.StorageLocation, null).Readonly(p => !p.IsNewInventory)
                .UseListSetting(e => { e.HelpInfo = "仓库更改清空库位,新增盘盈可编辑"; });
            View.Property(p => p.StorageLocationId).UseSCDetailLocationEditor(p => p.ReloadDataOnPopping = true).Readonly(p => !p.IsNewInventory
            || p.CountDimension != CountDimension.Location)
                .UseListSetting(e => { e.HelpInfo = NEWINVENTORYCANEDIT; });
            View.Property(p => p.LotId).UseSCDetailLotEditor(p => p.ReloadDataOnPopping = true).Readonly(p => !p.IsNewInventory
            || p.CountDimension == CountDimension.Item)
                .UseListSetting(e => { e.HelpInfo = NEWINVENTORYCANEDIT; });
            View.Property(p => p.OnhandState).UseEnumEditor("RESULT").Readonly(p => !p.IsNewInventory)
                .UseListSetting(e => { e.HelpInfo = NEWINVENTORYCANEDIT; });
            View.Property(p => p.Qty).ShowInList(width: 60).Readonly();
            View.Property(p => p.ActualCountQty).ShowInList(width: 90).UseItemUnitEditor(p => p.AllowBlank = true)
                .Readonly(p => p.StockCountState == LesCountState.Finished || !(p.ItemId > 0 && (p.State == LesCountState.Audit || p.State == LesCountState.FinishCount)))
                .UseListSetting(e => { e.HelpInfo = "行状态等于审批或已盘点不可编辑"; });
            View.Property(p => p.DiffCountQty).ShowInList(width: 80).Readonly();
            View.Property(p => p.IsNewInventory).Readonly();
            View.Property(p => p.LesStockCountDetailResult).Show().Readonly();
            View.Property(p => p.AnalysisResult).Readonly(p => p.LesStockCountDetailResult == LesStockCountDetailResult.Normal).Show();
            View.Property(p => p.ResultDesc).Readonly(p => p.LesStockCountDetailResult == LesStockCountDetailResult.Normal).Show();
            View.Property(p => p.IsAllowAdjust).Readonly();
            View.Property(p => p.CountById).Readonly();
            View.Property(p => p.CountDate).ShowInList(150).Readonly();
            View.WithoutPaging();
        }

        /// <summary>
        /// 查看视图
        /// </summary>
        private void ConfigReadOnlyView()
        {
            View.ClearCommands();
            View.DisableEditing();
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.LineNo).ShowInList(width: 60).Readonly();
                View.Property(p => p.LabelNo).ShowInList(width: 150).Readonly();
                View.Property(p => p.State).ShowInList(width: 60).Readonly();
                View.Property(p => p.ItemId).ShowInList(150);
                View.Property(p => p.ItemName).ShowInList(150);
                View.Property(p => p.ItemSpecificationModel).Show();
                View.Property(p => p.ItemExtPropName).Readonly().ShowInList(width: 180);
                View.Property(p => p.ItemUnitName).ShowInList(width: 50).Show();
                View.Property(p => p.FactoryId).Show();
                View.Property(p => p.WarehouseId).Show();
                //View.Property(p => p.StorageLocationId).Show();
                View.Property(p => p.StorageLocationName).HasLabel("库位").Show();
                View.Property(p => p.LotId).ShowInList(150);
                View.Property(p => p.OnhandState).Show();
                View.Property(p => p.Qty).ShowInList(width: 60).Readonly().Show();
                View.Property(p => p.ActualCountQty).ShowInList(width: 80).Show();
                View.Property(p => p.DiffCountQty).ShowInList(width: 80).Readonly().Show();
                View.Property(p => p.IsNewInventory).Readonly().Show();
                View.Property(p => p.LesStockCountDetailResult).Show().Readonly();
                View.Property(p => p.AnalysisResult).Show();
                View.Property(p => p.ResultDesc).Show();
                View.Property(p => p.IsAllowAdjust).Readonly().Show();
                View.Property(p => p.CountById).Readonly().Show();
                View.Property(p => p.CountDate).Readonly().Show();
            }
        }

        /// <summary>
        /// 配置导出模板视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.LineNo).Show();
                View.Property(p => p.ActualCountQty).HasLabel("数量").Show();
            }
        }
    }
}
