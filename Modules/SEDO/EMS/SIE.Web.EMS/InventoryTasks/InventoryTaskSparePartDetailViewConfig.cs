using SIE.Domain.Validation;
using SIE.EMS.Enums;
using SIE.EMS.InventoryBalances;
using SIE.EMS.InventoryTasks;
using SIE.Equipments.Enums;
using SIE.MetaModel.View;
using SIE.Web.EMS.InventoryTasks.Commands;
using System;

namespace SIE.Web.EMS.InventoryTasks
{
    /// <summary>
    /// 盘点任务设备清单界面
    /// </summary>
    internal class InventoryTaskSparePartDetailViewConfig : WebViewConfig<InventoryTaskSparePartDetail>
    {
        //单个字符宽度
        private readonly int SingleCharWidth = 20;

        /// <summary>
        /// 盘点平账界面
        /// </summary>
        public const string BalanceView = "BalanceView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.ClearCommands();

            View.DeclareExtendViewGroup(BalanceView);
            if (ViewGroup == BalanceView)
            {
                ConfigBalanceView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.EMS.InventoryTasks.InventoryTaskSparePartDetailBehavior");

            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll, WebCommandNames.ExportXlsSelection,
                   typeof(ImportTaskSparePartCommand).FullName, "SIE.Web.Common.Import.Commands.DownloadTemplateCommand",
                   "SIE.Web.EMS.InventoryTasks.Commands.AddSparePartProfitCommand", typeof(DeleteProfitCommand).FullName,
                   "SIE.Web.EMS.InventoryTasks.Commands.SearchInventoryTaskSparePartDetailCommand");

            View.Property(p => p.InventoryStatus).ShowInList(SingleCharWidth * 4).Readonly();
            View.Property(p => p.FirstResult).ShowInList(SingleCharWidth * 4).Readonly();
            View.Property(p => p.SecondResult).ShowInList(SingleCharWidth * 4).Readonly();
            View.Property(p => p.SparePartId).ShowInList(SingleCharWidth * 4).Readonly();
            View.Property(p => p.SparePartName).ShowInList(SingleCharWidth * 4).Readonly();
            View.Property(p => p.ControlMethod).ShowInList(SingleCharWidth * 4).Readonly();
            View.Property(p => p.LotNo).ShowInList(SingleCharWidth * 10).Readonly();
            View.Property(p => p.Sn).ShowInList(SingleCharWidth * 10).Readonly();
            View.Property(p => p.WarehouseId).ShowInList(SingleCharWidth * 4).Readonly();
            View.Property(p => p.StorageLocationId).ShowInList(SingleCharWidth * 4).Readonly();
            View.Property(p => p.GoodQty).Readonly();
            View.Property(p => p.NgQty).Readonly();
            View.Property(p => p.Total).Readonly();

            using (View.DeclareBand("初盘".L10N()))
            {
                View.Property(p => p.FirstGood).UseSpinEditor(x => x.MinValue = 0)
                    .Readonly(x => x.InventoryTaskStatus != InventoryTaskStatus.Doing
                        || x.InventoryAssetSource == InventoryAssetSource.Profit
                        || x.FirstPower == false)
                    .HasLabel("良品数");

                View.Property(p => p.FirstNg).UseSpinEditor(x => x.MinValue = 0)
                    .Readonly(x => x.InventoryTaskStatus != InventoryTaskStatus.Doing
                        || x.InventoryAssetSource == InventoryAssetSource.Profit
                        || x.FirstPower == false)
                    .HasLabel("不良品数");

                View.Property(p => p.FirstTotal).Readonly().HasLabel("总数");
                View.Property(p => p.FirstDiff).Readonly().HasLabel("差异数");
            }

            using (View.DeclareBand("复盘".L10N()))
            {
                View.Property(p => p.SecondGoodQty).HasLabel("良品数").UseSpinEditor(x => x.MinValue = 0)
                    .Readonly(p => (p.InventoryTaskStatus != InventoryTaskStatus.FirstDone
                        && p.InventoryTaskStatus != InventoryTaskStatus.ScondDoing)
                    || (p.InventoryAssetSource == InventoryAssetSource.Profit && p.FirstResult == null)
                    || p.SecondPower == false);
                View.Property(p => p.SecondNgQty).HasLabel("不良品数").UseSpinEditor(x => x.MinValue = 0)
                    .Readonly(p => (p.InventoryTaskStatus != InventoryTaskStatus.FirstDone
                        && p.InventoryTaskStatus != InventoryTaskStatus.ScondDoing)
                     || (p.InventoryAssetSource == InventoryAssetSource.Profit && p.FirstResult == null)
                     || p.SecondPower == false);
                View.Property(p => p.SecondTotal).Readonly().HasLabel("总数");
                View.Property(p => p.SecondDiff).Readonly().HasLabel("差异数");
            }

            View.Property(p => p.Specification).Readonly();
            View.Property(p => p.ItemCategoryName).Readonly();
            View.Property(p => p.SpartType).Readonly();
            View.Property(p => p.UnitName).Readonly();
            View.Property(p => p.InventoryAssetSource).Readonly();
            View.Property(p => p.FirstCounterId).Readonly();
            View.Property(p => p.FirstDateTime).Readonly();
            View.Property(p => p.SecondCounterId).Readonly();
            View.Property(p => p.SecondDateTime).Readonly();

            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 盘点平账界面
        /// </summary> 
        protected void ConfigBalanceView()
        {
            View.AssignAuthorize(typeof(InventoryBalance));
            View.AddBehavior("SIE.Web.EMS.InventoryTasks.InventoryTaskSparePartDetailBehavior");

            View.ClearCommands();

            using (View.OrderProperties())
            {
                View.Property(p => p.SparePartProcessMethod).ShowInList(SingleCharWidth * 4)
                    .Readonly(p => p.ApprovalStatus != ApprovalStatus.Reject && p.ApprovalStatus != ApprovalStatus.Draft);
                View.Property(p => p.FirstResult).ShowInList(SingleCharWidth * 4).Readonly();
                View.Property(p => p.SecondResult).ShowInList(SingleCharWidth * 4).Readonly();
                View.Property(p => p.SparePartId).ShowInList(SingleCharWidth * 4).Readonly();
                View.Property(p => p.SparePartName).ShowInList(SingleCharWidth * 4).Readonly();
                View.Property(p => p.ControlMethod).ShowInList(SingleCharWidth * 4).Readonly();
                View.Property(p => p.LotNo).ShowInList(SingleCharWidth * 10).Readonly();
                View.Property(p => p.Sn).ShowInList(SingleCharWidth * 10).Readonly();
                View.Property(p => p.WarehouseId).ShowInList(SingleCharWidth * 4).Readonly();
                View.Property(p => p.StorageLocationId).ShowInList(SingleCharWidth * 4).Readonly();
                View.Property(p => p.GoodQty).ShowInList().Readonly();
                View.Property(p => p.NgQty).ShowInList().Readonly();
                View.Property(p => p.Total).ShowInList().Readonly();

                using (View.DeclareBand("初盘".L10N()))
                {
                    View.Property(p => p.FirstGood).UseSpinEditor(x => x.MinValue = 0)
                        .Readonly(x => x.InventoryTaskStatus != InventoryTaskStatus.Doing
                            || x.InventoryAssetSource == InventoryAssetSource.Profit
                            || x.FirstPower == false)
                        .HasLabel("良品数").Readonly().ShowInList();

                    View.Property(p => p.FirstNg).UseSpinEditor(x => x.MinValue = 0)
                        .Readonly(x => x.InventoryTaskStatus != InventoryTaskStatus.Doing
                            || x.InventoryAssetSource == InventoryAssetSource.Profit
                            || x.FirstPower == false)
                        .HasLabel("不良品数").Readonly().ShowInList();

                    View.Property(p => p.FirstTotal).Readonly().HasLabel("总数").ShowInList();
                    View.Property(p => p.FirstDiff).Readonly().HasLabel("差异数").ShowInList();
                }

                using (View.DeclareBand("复盘".L10N()))
                {
                    View.Property(p => p.SecondGoodQty).HasLabel("良品数")
                        .Readonly(p => (p.InventoryTaskStatus != InventoryTaskStatus.FirstDone
                            && p.InventoryTaskStatus != InventoryTaskStatus.ScondDoing)
                        || (p.InventoryAssetSource == InventoryAssetSource.Profit && p.FirstResult == null)
                        || p.SecondPower == false).Readonly().ShowInList();
                    View.Property(p => p.SecondNgQty).HasLabel("不良品数")
                        .Readonly(p => (p.InventoryTaskStatus != InventoryTaskStatus.FirstDone
                            && p.InventoryTaskStatus != InventoryTaskStatus.ScondDoing)
                         || (p.InventoryAssetSource == InventoryAssetSource.Profit && p.FirstResult == null)
                         || p.SecondPower == false).Readonly().ShowInList();
                    View.Property(p => p.SecondTotal).Readonly().HasLabel("总数").ShowInList();
                    View.Property(p => p.SecondDiff).Readonly().HasLabel("差异数").ShowInList();
                }

                View.Property(p => p.Specification).Readonly().ShowInList();
                View.Property(p => p.ItemCategoryName).Readonly().ShowInList();
                View.Property(p => p.SpartType).Readonly().ShowInList();
                View.Property(p => p.UnitName).Readonly().ShowInList();
                View.Property(p => p.InventoryAssetSource).Readonly().ShowInList();
                View.Property(p => p.FirstCounterId).Readonly().ShowInList();
                View.Property(p => p.FirstDateTime).Readonly().ShowInList();
                View.Property(p => p.SecondCounterId).Readonly().ShowInList();
                View.Property(p => p.SecondDateTime).Readonly().ShowInList();

                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.PropertyRef(p => p.InventoryTask.TaskNo).HasLabel("盘点任务单号").BeforeImportFunc((v) =>
            {
                var taskNo = v.ToString();
                if (taskNo.IsNullOrEmpty())
                {
                    throw new ValidationException("【盘点任务单号】必须填写".L10N());
                }

                return v;
            });

            View.PropertyRef(p => p.SparePart.SparePartCode).BeforeImportFunc((v) =>
            {
                var sparePartCode = v.ToString();
                if (sparePartCode.IsNullOrEmpty())
                {
                    throw new ValidationException("【备件编码】必须填写".L10N());
                }

                return v;
            }).HasLabel("备件编码");

            View.Property(p => p.LotNo);
            View.Property(p => p.Sn);
            View.Property(p => p.StorageLocationCode).BeforeImportFunc((v) =>
            {
                var storageLocationCode = v.ToString();
                if (storageLocationCode.IsNullOrEmpty())
                {
                    throw new ValidationException("【库位编码】必须填写".L10N());
                }

                return v;
            });
            View.Property(p => p.GoodQty).HasLabel("盘点良品数");
            View.Property(p => p.NgQty).HasLabel("盘点不良品数");
        }
    }
}
