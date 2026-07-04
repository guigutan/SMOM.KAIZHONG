using SIE.Domain;
using SIE.EMS.Enums;
using SIE.EMS.Maintains.Plans;
using SIE.EMS.Maintains.Plans.ViewModels;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Applys.Controllers;
using SIE.EMS.SpareParts.OutDepots.Controllers;
using SIE.MetaModel.View;
using SIE.ObjectModel;
using SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.EquipMaint.Maintains.Plans
{
    /// <summary>
    /// 保养计划备件更换 视图
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class MaintainPlanSparePartViewConfig : WebViewConfig<MaintainPlanSparePart>
    {
        /// <summary>
        /// 保养确认所用的备件更换视图
        /// </summary>
        public const string MaintainConfirmationListView = "MaintainConfirmationListView";

        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(MaintainPlanViewModel));
            View.DeclareExtendViewGroup(MaintainConfirmationListView);
            if (ViewGroup == MaintainConfirmationListView)
            {
                ConfigListView();
                View.ClearCommands();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(SelEquipBomCommand).FullName, typeof(SelSparePartCommand).FullName);
            View.UseCommands(typeof(ExeChangeSparePartCommand).FullName);
            View.UseCommands(WebCommandNames.Edit, typeof(DeleteMaintainSparePartCommand).FullName);

            using (View.OrderProperties())
            {
                View.Property(p => p.SparePartCodeView).Show(ShowInWhere.All).Readonly().HasLabel("备件编码".L10N()+"*");
                View.Property(p => p.SparePartId).Readonly().Show(ShowInWhere.Hide);
                View.Property(p => p.SparePartNameView).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.SpecificationView).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.PartOutDepotDetailId).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();                    
                    dic.Add(nameof(e.RemainingQty), nameof(e.PartOutDepotDetail.RemainingQty));
                    dic.Add(nameof(e.BatchNoView), nameof(e.PartOutDepotDetail.BatchNoView));
                    dic.Add(nameof(e.SeriaNoView), nameof(e.PartOutDepotDetail.SeriaNoView));

                    m.DicLinkField = dic;

                    m.DisplayField = MaintainPlanSparePart.OutDepotNoViewProperty.Name;
                    m.BindDisplayField = MaintainPlanSparePart.OutDepotNoViewProperty.Name;
                }).UseDataSource((s, p, k) =>
                {
                    var entity = s as MaintainPlanSparePart;
                    var maintainPlan = entity.MaintainPlan;
                    var outDtl = RT.Service.Resolve<OutDepotController>()
                        .GetPartOutDepotDtls(entity.SparePartId, maintainPlan.EquipAccountId, maintainPlan.EquipAccount.EquipModelId,
                        maintainPlan.MachineNo, p, k);
                    return outDtl;
                }).Show(ShowInWhere.All).Readonly(p => p.State == ChangeSparePartState.Finished).Readonly(ViewGroup == MaintainConfirmationListView).HasLabel("备件出库单明细".L10N() + "*");
                View.Property(p => p.RemainingQty).Show(ShowInWhere.All).Readonly();
                
                View.Property(p => p.State).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ChangeQty).Show(ShowInWhere.All)
                    .UseSpinEditor(m => m.MinValue = 1)
                    .Readonly(p => p.State == ChangeSparePartState.Finished || p.ControlMethod== SIE.EMS.SpareParts.Enums.ControlMethod.Sn)
                    .Readonly(ViewGroup == MaintainConfirmationListView).HasLabel("更换数量".L10N() + "*");
                View.Property(p => p.ControlMethod).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.BatchNoView).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.SeriaNoView).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.OldSequence).Show(ShowInWhere.All).Readonly(p => p.State == ChangeSparePartState.Finished).Readonly(ViewGroup == MaintainConfirmationListView);
                View.Property(p => p.UnitView).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Remark).Show(ShowInWhere.All).Readonly(p => p.State == ChangeSparePartState.Finished).Readonly(ViewGroup == MaintainConfirmationListView);

                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
