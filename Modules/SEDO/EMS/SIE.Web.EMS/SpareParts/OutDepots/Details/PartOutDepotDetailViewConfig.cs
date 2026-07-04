using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.OutDepots.Controllers;
using SIE.EMS.SpareParts.OutDepots.Details;
using SIE.MetaModel.View;
using System.Collections.Generic;

namespace SIE.Web.EMS.SpareParts.OutDepots.Details
{
    /// <summary>
    /// 出库明细视图配置
    /// </summary>
    public class PartOutDepotDetailViewConfig : WebViewConfig<PartOutDepotDetail>
    {
        /// <summary>
        /// 出库明细添加视图
        /// </summary>
        public const string AddPartOutDepotDetailViewGroup = "AddPartOutDepotDetailViewGroup";

        /// <summary>
        /// 出库明细拣货视图
        /// </summary>
        public const string PickPartOutDepotDetailViewGroup = "PickPartOutDepotDetailViewGroup";

        /// <summary>
        /// 出库明细发货视图
        /// </summary>
        public const string SendPartOutDepotDetailViewGroup = "SendPartOutDepotDetailViewGroup";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(new string[] { AddPartOutDepotDetailViewGroup, PickPartOutDepotDetailViewGroup, SendPartOutDepotDetailViewGroup });

            if (ViewGroup == AddPartOutDepotDetailViewGroup)
            {
                ConfigAddPartOutDepotDetailView();
            }

            if (ViewGroup == PickPartOutDepotDetailViewGroup)
            {
                ConfigPickPartOutDepotDetailView();
            }

            if (ViewGroup == SendPartOutDepotDetailViewGroup)
            {
                ConfigSendPartOutDepotDetailView();
            }
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            bool isCreateHandoverBill = RT.Service.Resolve<OutDepotController>().IsCreateHandoverBill();
            bool isComputeAvgCost = RT.Service.Resolve<SparePartController>().IsComputeAvgCost();

            View.AddBehavior("SIE.Web.EMS.SpareParts.OutDepots.Behaviors.PartOutDepotDetailBehavior");
            View.UseCommand("SIE.Web.EMS.SpareParts.OutDepots.Commands.SearchOutDepotDetailCommand");
            View.UseCommand("SIE.Web.EMS.SpareParts.OutDepots.Commands.CancelPickedCommand");
            View.DisableEditing();
            View.Property(p => p.LineNo);
            View.Property(p => p.SparePartId).UsePagingLookUpEditor((m, e) =>
                                            {
                                                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                                                keyValues.Add(nameof(e.SparePartNameView), nameof(e.SparePart.SparePartName));
                                                keyValues.Add(nameof(e.SpecificationView), nameof(e.SparePart.Specification));
                                                keyValues.Add(nameof(e.SpartType), nameof(e.SparePart.SpartType));
                                                keyValues.Add(nameof(e.ControlMethod), nameof(e.SparePart.ControlMethod));
                                                keyValues.Add(nameof(e.IsReplacement), nameof(e.SparePart.IsReplacement));
                                                m.DicLinkField = keyValues;
                                            }).ShowInList(width: 120);
            View.Property(p => p.SparePartName);
            View.Property(p => p.SpecificationView);
            View.Property(p => p.SpartType);
            View.Property(p => p.ControlMethod);
            View.Property(p => p.IsReplacement).Readonly(true);
            View.Property(p => p.WarehouseName);
            View.Property(p => p.StorageLocationName);
            View.Property(p => p.BatchNoView).ShowInList(width: 120);
            View.Property(p => p.SeriaNoView).ShowInList(width: 120);
            View.Property(p => p.OutDepotCount);
            View.Property(p => p.OutboundStatus);
            View.Property(p => p.OutDepotDate).UseDateEditor();
            View.Property(p => p.HandoverNo).ShowInList(width: 120).Show(isCreateHandoverBill ? ShowInWhere.All : ShowInWhere.Hide);
            View.Property(p => p.UseCount);
            View.Property(p => p.ReturnQty);
            View.Property(p => p.OldReturnQty);
            View.Property(p => p.UnitPrice)
                .UseSpinEditor(m => m.DecimalPrecision = 2)
                .Show(isComputeAvgCost ? ShowInWhere.All : ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 出库明细添加视图
        /// </summary>
        protected void ConfigAddPartOutDepotDetailView()
        {
            View.DisableEditing();
            View.UseCommand("SIE.Web.EMS.SpareParts.OutDepots.Commands.DeleteOutDepotDetailCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.SparePartId).Show(ShowInWhere.Hide);
                View.Property(p => p.SparePartCodeView).Show();
                View.Property(p => p.SparePartNameView).Show();
                View.Property(p => p.ControlMethodView).Show();
                View.Property(p => p.BatchNoRefId).Show(ShowInWhere.Hide);
                View.Property(p => p.BatchNo).ShowInList(width:120).Show();
                View.Property(p => p.SeriaNoRefId).Show(ShowInWhere.Hide);
                View.Property(p => p.SeriaNo).ShowInList(width: 120).Show();
                View.Property(p => p.OutDepotCount).HasLabel("本次拣货数").Show();
                View.Property(p => p.QualityStatus).Show();
                View.Property(p => p.StorageLocationId).Show(ShowInWhere.Hide);
                View.Property(p => p.SiteCode).Show();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 出库明细拣货视图
        /// </summary>
        protected void ConfigPickPartOutDepotDetailView()
        {
            View.DisableEditing();
            View.UseCommand("SIE.Web.EMS.SpareParts.OutDepots.Commands.DeleteOutDepotDetailCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.SparePartId).Show(ShowInWhere.Hide);
                View.Property(p => p.SparePartCodeView).Show();
                View.Property(p => p.SparePartNameView).Show();
                View.Property(p => p.ControlMethodView).Show();
                View.Property(p => p.BatchNoRefId).Show(ShowInWhere.Hide);
                View.Property(p => p.BatchNo).ShowInList(width: 120).Show();
                View.Property(p => p.SeriaNoRefId).Show(ShowInWhere.Hide);
                View.Property(p => p.SeriaNo).ShowInList(width: 120).Show();
                View.Property(p => p.OutDepotCount).HasLabel("本次拣货数").Show();
                View.Property(p => p.QualityStatus).Show();
                View.Property(p => p.OutboundStatus).Show();
                View.Property(p => p.StorageLocationId).Show(ShowInWhere.Hide);
                View.Property(p => p.SiteCode).Show();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 出库明细发货视图
        /// </summary>
        protected void ConfigSendPartOutDepotDetailView()
        {
            View.AddBehavior("SIE.Web.EMS.SpareParts.OutDepots.Behaviors.PartOutDepotDetailBehavior");
            View.UseCommand("SIE.Web.EMS.SpareParts.OutDepots.Commands.SearchOutDepotDetailCommand");
            View.DisableEditing();
            View.UseGridSelectionModel(checkOnly: true);

            using (View.OrderProperties()) 
            {
                View.Property(p => p.LineNo).Show();
                View.Property(p => p.SparePartId).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.SparePartNameView), nameof(e.SparePart.SparePartName));
                    keyValues.Add(nameof(e.SpecificationView), nameof(e.SparePart.Specification));
                    keyValues.Add(nameof(e.SpartType), nameof(e.SparePart.SpartType));
                    keyValues.Add(nameof(e.ControlMethod), nameof(e.SparePart.ControlMethod));
                    keyValues.Add(nameof(e.IsReplacement), nameof(e.SparePart.IsReplacement));
                    m.DicLinkField = keyValues;
                }).Show();
                View.Property(p => p.SparePartName).Show();
                View.Property(p => p.SpecificationView).Show();
                View.Property(p => p.SpartType).Show();
                View.Property(p => p.ControlMethod).Show();
                View.Property(p => p.IsReplacement).Readonly().Show();
                View.Property(p => p.WarehouseName).Show();
                View.Property(p => p.StorageLocationName).Show();
                View.Property(p => p.BatchNoView).Show();
                View.Property(p => p.SeriaNoView).Show();
                View.Property(p => p.OutDepotCount).Show();
                View.Property(p => p.OutboundStatus).Show();
                View.Property(p => p.UseCount).Show();
                View.Property(p => p.ReturnQty).Show();
                View.Property(p => p.OldReturnQty).Show();
                View.Property(p => p.UnitPrice).Show();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
            
        }

        /// <summary>
        /// 选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.OutDepotLineNo).ShowInList(width: 150);
        }
    }
}
