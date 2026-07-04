using SIE.EMS.Enums;
using SIE.EMS.EquipRepair.Controller;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.EMS.SpareParts.Enums;
using SIE.EMS.SpareParts.OutDepots.Controllers;
using SIE.EMS.SpareParts.OutDepots.Details;
using SIE.MetaModel.View;
using SIE.Web.EMS.EquipRepair.EquipRepairs.Commands;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs
{
    /// <summary>
    /// 设备维修备件更换 视图
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class EquipRepairSparePartChgViewConfig : WebViewConfig<EquipRepairSparePartChg>
    {
        /// <summary>
        /// 单个字符宽度
        /// </summary>
        private readonly int singleCharWidth = 20;

        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(EquipRepairBill));
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(SelEquipBomCommand).FullName, typeof(SelSparePartCommand).FullName);
            View.UseCommands(typeof(ExeChangeSparePartCommand).FullName);
            View.UseCommands("SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.EditEquipRepairSparePartChgCommand", "SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.DeleteEquipRepairSparePartChgCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.SparePartCodeView).Readonly().HasLabel("备件编码".L10N() + "*");
                View.Property(p => p.SparePartId).Readonly().Show(ShowInWhere.Hide);
                View.Property(p => p.SparePartNameView).Readonly();
                View.Property(p => p.SpecificationView).Readonly();
                View.Property(p => p.PartOutDepotDetailId)
                    .UsePagingLookUpEditor((m, e) =>
                    {
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add(nameof(e.RemainingQty), nameof(e.PartOutDepotDetail.RemainingQty));
                        dic.Add(nameof(e.BatchNo), nameof(e.PartOutDepotDetail.BatchNoView));
                        dic.Add(nameof(e.SeriaNo), nameof(e.PartOutDepotDetail.SeriaNo));

                        m.DicLinkField = dic;

                        m.DisplayField = PartOutDepotDetail.OutDepotLineNoProperty.Name;
                        m.BindDisplayField = EquipRepairSparePartChg.OutDepotLineNoProperty.Name;

                    }).UseDataSource((s, p, k) =>
                    {
                        var entity = s as EquipRepairSparePartChg;
                        var equipRepairBill = RT.Service.Resolve<RepairController>().GetEquipRepairBill(entity.EquipRepairBillId);
                        var outDtl = RT.Service.Resolve<OutDepotController>()
                            .GetPartOutDepotDtls(entity.SparePartId, equipRepairBill.EquipAccountId, equipRepairBill.EquipModelId,
                            equipRepairBill.RepairNo, p, k);
                        return outDtl;
                    }).Readonly(p => p.State == SIE.EMS.Enums.ChangeSparePartState.Finished).ShowInList(singleCharWidth * 8).HasLabel("备件出库单".L10N() + "*");
                View.Property(p => p.RemainingQty).Readonly();
                View.Property(p => p.State).Readonly();
                View.Property(p => p.ChangeQty)
                    .UseSpinEditor(m => m.MinValue = 1)
                    .Readonly(p => p.State == SIE.EMS.Enums.ChangeSparePartState.Finished || p.ControlMethod == SIE.EMS.SpareParts.Enums.ControlMethod.Sn).HasLabel("更换数量".L10N() + "*");
                View.Property(p => p.ControlMethod).Readonly();
                View.Property(p => p.BatchNo).Readonly();
                View.Property(p => p.SeriaNo).Readonly();
                View.Property(p => p.OldSequence).Readonly(p => p.State == SIE.EMS.Enums.ChangeSparePartState.Finished);
                View.Property(p => p.UnitView).Readonly();
                View.Property(p => p.Remark).Readonly(p => p.State == SIE.EMS.Enums.ChangeSparePartState.Finished);

                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
