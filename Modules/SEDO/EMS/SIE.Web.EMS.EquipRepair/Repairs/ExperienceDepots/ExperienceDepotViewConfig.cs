using SIE.Domain;
using SIE.EMS.Devices.Abnormals;
using SIE.EMS.EquipRepair.ExperienceDepots;
using SIE.EMS.EquipRepair.ExperienceDepots.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel.View;
using SIE.Web.Common;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.EquipRepair.Repairs.ExperienceDepots
{
    /// <summary>
    /// 维修经验库视图配置
    /// </summary>
    public class ExperienceDepotViewConfig : WebViewConfig<ExperienceDepot>
    {

        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            if (ViewGroup == "DetailInfoView")
            {
                DetailInfoView();
            }
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.EMS.EquipRepair.Repairs.ExperienceDepots.Behaviors.ExpBehavior");
            View.UseDefaultCommands();
            View.ReplaceCommands(WebCommandNames.Add, "SIE.Web.EMS.EquipRepair.Repairs.ExperienceDepots.Commands.AddExpCommand");
            View.Property(p => p.Code).Readonly();
            View.Property(p => p.RepairType).Cascade(p => p.EquipAccountId, null).Cascade(p => p.EquipAccountName, null).Cascade(p => p.EquipModelId, null).Cascade(p => p.EquipTypeId, null).Cascade(p => p.SparePartId, null).Cascade(p => p.SparePartName, null);
            View.Property(p => p.EquipAccountId)
                .UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<EquipAccountController>().GetAllEquipAccounts(p, k);
                })
                .UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.EquipAccountName), nameof(e.EquipAccount.Name));
                    m.DicLinkField = keyValues;
                })
                .Readonly(p => p.RepairType == ExpRepairType.SparePart).HasLabel("设备编码".L10N()+"*");
            View.Property(p => p.EquipAccountName).Readonly();
            View.Property(p => p.EquipModel).Readonly();
            View.Property(p => p.EquipType).UsePagingLookUpEditor(p => p.XType = "EquipTypeComboList")
                .Readonly(p => p.RepairType == ExpRepairType.SparePart || (p.EquipAccountId != 0 && p.EquipAccountId != null));
            View.Property(p => p.SparePartId).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.SparePartName), nameof(e.SparePart.SparePartName));
                    m.DicLinkField = keyValues;
                })
                .Readonly(p => p.RepairType == ExpRepairType.Account);
            View.Property(p => p.SparePartName).Readonly();            
            View.Property(p => p.RepairNo);
            View.Property(p => p.FaultReson).UseCatalogEditor(p => { p.CatalogType = ExperienceDepot.expFaultReson; p.CatalogReloadData = true; });
            View.Property(p => p.EquipLargeFault);
            View.Property(p => p.FaultPart).Show(ShowInWhere.Hide);


            View.AttachDetailChildrenProperty(typeof(ExperienceDepot), (c) =>
            {
                var experienceDepot = c.Parent as ExperienceDepot;
                experienceDepot = RF.GetById<ExperienceDepot>(experienceDepot.Id, new EagerLoadOptions().LoadWith(ExperienceDepot.FaultPhenomenonProperty).LoadWith(ExperienceDepot.FaultDescribeProperty));
                return experienceDepot;
            }, "DetailInfoView").HasLabel("详细信息").HasOrderNo(1);
            View.ChildrenProperty(p => p.ExperienceDepotAttachmentList).HasOrderNo(2);
        }


        /// <summary>
        /// 详细信息视图
        /// </summary>
        public void DetailInfoView()
        {
            View.ClearCommands();
            View.FormEdit();
            using (View.OrderProperties())
            {
                View.HasDetailColumnsCount(2);
                View.Property(p => p.FaultPhenomenon).UsePagingLookUpEditor(p =>
                {
                    p.XType = "FaultEditorComboList";
                    p.ReloadDataOnPopping = true;
                }).UseDataSource((e, p, o) =>
                    {
                        var depot = e as ExperienceDepot;
                        if (depot == null)
                            return new EntityList<DeviceAbnormal>();
                        if (depot.RepairType == ExpRepairType.Account)
                            return RT.Service.Resolve<DeviceAbnormalController>().GetDeviceAbnormals(p, o, AbnormalType.Unusual, depot.EquipTypeId, 0);
                        else
                        {
                            return RT.Service.Resolve<DeviceAbnormalController>().GetDeviceAbnormals(p, o, AbnormalType.Unusual, null, 1);
                        }
                    }).ShowInDetail(columnSpan: 2).HasLabel("故障现象".L10N() + "*");
                View.Property(p => p.FaultPhenomenonRemark)
                    .ShowInDetail(columnSpan: 2)
                     .UseMemoEditor();
                View.Property(p => p.FaultDescribe).UsePagingLookUpEditor(p =>
                {
                    p.XType = "FaultEditorComboList";
                    p.ReloadDataOnPopping = true;
                })
                .UseDataSource((e, p, o) =>
                {
                    var depot = e as ExperienceDepot;
                    if (depot == null)
                        return new EntityList<DeviceAbnormal>();
                    if (depot.RepairType == ExpRepairType.Account)
                        return RT.Service.Resolve<DeviceAbnormalController>().GetDeviceAbnormals(p, o, AbnormalType.Unusual, depot.EquipTypeId, 0);
                    else
                    {
                        return RT.Service.Resolve<DeviceAbnormalController>().GetDeviceAbnormals(p, o, AbnormalType.Unusual, null, 1);
                    }
                })
                .ShowInDetail(columnSpan: 2).HasLabel("故障描述".L10N() + "*");
                View.Property(p => p.FaultDescribeRemark)
                .ShowInDetail(columnSpan: 2)
                .UseMemoEditor();
                View.Property(p => p.RepairWay)
                .ShowInDetail(columnSpan: 2)
                .UseMemoEditor()
                ;
                View.Property(p => p.PreventionAdvice)
                .ShowInDetail(columnSpan: 2)
                .UseMemoEditor()
                ;
                View.Property(p => p.FaultCode)
                .Show(ShowInWhere.Hide)
                .UseMemoEditor()
                ;
            }
        }
    }
}
