using SIE.EMS.Checks.Plans;
using SIE.EMS.Checks.Plans.ViewModels;
using SIE.EMS.Enums;
using SIE.EMS.SpareParts.OutDepots.Controllers;
using SIE.MetaModel.View;
using SIE.Web.EMS.Checks.Plans.Commands;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.Checks.Plans
{
    /// <summary>
    /// 点检计划备件更换 视图
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class CheckPlanSparePartViewConfig : WebViewConfig<CheckPlanSparePart>
    {
        /// <summary>
        /// 点检确认所用的备件更换视图
        /// </summary>
        public const string CheckConfirmationListView = "CheckConfirmationListView";

        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(CheckPlanViewModel));
            View.DeclareExtendViewGroup(CheckConfirmationListView);
            if (ViewGroup == CheckConfirmationListView)
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
            View.UseCommands(WebCommandNames.Edit, typeof(DeleteCheckSparePartCommand).FullName);

            using (View.OrderProperties())
            {
                View.Property(p => p.SparePartCodeView).HasLabel("备件编码".L10N()+"*").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.SparePartId).Readonly().Show(ShowInWhere.Hide);
                View.Property(p => p.SparePartNameView).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.SpecificationView).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.PartOutDepotDetailId)
                    .HasLabel("备件出库单".L10N()+"*")
                    .UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();                    
                    dic.Add(nameof(e.RemainingQty), nameof(e.PartOutDepotDetail.RemainingQty));
                    dic.Add(nameof(e.BatchNoView), nameof(e.PartOutDepotDetail.BatchNoView));
                    dic.Add(nameof(e.SeriaNoView), nameof(e.PartOutDepotDetail.SeriaNoView));
                    m.DicLinkField = dic;
                    m.DisplayField = CheckPlanSparePart.OutDepotNoViewProperty.Name;
                    m.BindDisplayField = CheckPlanSparePart.OutDepotNoViewProperty.Name;
                }).UseDataSource((s, p, k) =>
                {
                    var entity = s as CheckPlanSparePart;
                    var checkPlan = entity.CheckPlan;
                    var outDtl = RT.Service.Resolve<OutDepotController>()
                        .GetPartOutDepotDtls(entity.SparePartId, checkPlan.EquipAccountId, checkPlan.EquipAccount.EquipModelId,
                        checkPlan.CheckPlanNo, p, k);
                    return outDtl;
                }).Show(ShowInWhere.All).Readonly(p => p.State == ChangeSparePartState.Finished).Readonly(ViewGroup == CheckConfirmationListView);
                View.Property(p => p.RemainingQty).Show(ShowInWhere.All).Readonly();
                
                View.Property(p => p.State).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ChangeQty).HasLabel("更换数量".L10N()+"*").Show(ShowInWhere.All)
                    .UseSpinEditor(m => m.MinValue = 1)
                    .Readonly(p => p.State == SIE.EMS.Enums.ChangeSparePartState.Finished || p.ControlMethod == SIE.EMS.SpareParts.Enums.ControlMethod.Sn)
                    .Readonly(ViewGroup == CheckConfirmationListView);
                View.Property(p => p.ControlMethod).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.BatchNoView).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.SeriaNoView).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.OldSequence).Show(ShowInWhere.All).Readonly(p => p.State == SIE.EMS.Enums.ChangeSparePartState.Finished).Readonly(ViewGroup == CheckConfirmationListView);
                View.Property(p => p.UnitView).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Remark).Show(ShowInWhere.All).Readonly(p => p.State == SIE.EMS.Enums.ChangeSparePartState.Finished).Readonly(ViewGroup == CheckConfirmationListView);

                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
