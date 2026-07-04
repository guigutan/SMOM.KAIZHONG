using SIE.EMS.SpareParts.OutDepots.Details;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.SpareParts.OutDepots.Details
{
    /// <summary>
    /// 出库申请细视图配置
    /// </summary>
    public class OutDepotDetailViewConfig : WebViewConfig<OutDepotDetail>
    {
        /// <summary>
        /// 申请明细添加视图
        /// </summary>
        public const string AddOutDepotDetailViewGroup = "AddOutDepotDetailViewGroup";

        /// <summary>
        /// 申请明细拣货视图
        /// </summary>
        public const string PickOutDepotDetailViewGroup = "PickOutDepotDetailViewGroup";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(new string[] { AddOutDepotDetailViewGroup, PickOutDepotDetailViewGroup });

            if (ViewGroup == AddOutDepotDetailViewGroup)
            {
                ConfigAddOutDepotDetailView();
            }

            if (ViewGroup == PickOutDepotDetailViewGroup)
            {
                ConfigPickOutDepotDetailView();
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.Property(p => p.SparePartId).UsePagingLookUpEditor((m, e) =>
                                {
                                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                                    keyValues.Add(nameof(e.SparePartName), nameof(e.SparePart.SparePartName));
                                    keyValues.Add(nameof(e.SpecModel), nameof(e.SparePart.Specification));
                                    keyValues.Add(nameof(e.SpartType), nameof(e.SparePart.SpartType));
                                    keyValues.Add(nameof(e.SpartEquipModelName), nameof(e.SparePart.EquipModelName));
                                    keyValues.Add(nameof(e.UnitName), nameof(e.SparePart.UnitName));
                                    m.DicLinkField = keyValues;
                                }).ShowInList(width: 120).Show();
            View.Property(p => p.SparePartName).Show();
            View.Property(p => p.SpecModel).Show();
            View.Property(p => p.SparePartPart).Show();
            View.Property(p => p.SpartType).Show();
            View.Property(p => p.SpartEquipModelName).Show();
            View.Property(p => p.UnitName).Show();
            View.Property(p => p.RequireCount).Show();
            View.Property(p => p.PickedCount).Show();
            View.Property(p => p.OutDepotCount).Show();
            View.Property(p => p.ReceiveQty).Show();
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 申请明细添加视图
        /// </summary>
        protected void ConfigAddOutDepotDetailView()
        {
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.SparePartId).Show(ShowInWhere.Hide);
                View.Property(p => p.SparePartCodeView).Show();
                View.Property(p => p.SparePartNameView).Show();
                View.Property(p => p.ControlMethodView).Show();
                View.Property(p => p.RequireCount).Show().HasLabel("拣货数".L10N()+"*");
                View.Property(p => p.PickedCount).Show();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 申请明细拣货视图
        /// </summary>
        protected void ConfigPickOutDepotDetailView()
        {
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.SparePartId).Show(ShowInWhere.Hide);
                View.Property(p => p.SparePartCodeView).Show();
                View.Property(p => p.SparePartNameView).Show();
                View.Property(p => p.ControlMethodView).Show();
                View.Property(p => p.RequireCount).Show();
                View.Property(p => p.PickedCount).Show();
                View.Property(p => p.AdviceStorageLocation).ShowInList(width:500).Show();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
