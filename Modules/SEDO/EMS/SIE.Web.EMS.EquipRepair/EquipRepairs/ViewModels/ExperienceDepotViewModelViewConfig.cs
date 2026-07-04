using SIE.Domain;
using SIE.EMS.Devices.Abnormals;
using SIE.EMS.EquipRepair.Controller;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.EMS.EquipRepair.EquipRepairs.ViewModels;
using SIE.Web.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs.ViewModels
{
    /// <summary>
    /// 维修经验库视图配置
    /// </summary>
    public class ExperienceDepotViewModelViewConfig:WebViewConfig<ExperienceDepotViewModel>
    {
        /// <summary>
        /// 明细视图配置
        /// </summary>

        protected override void ConfigDetailsView()
        {
            View.HasDetailColumnsCount(3);
            using (View.OrderProperties())
            {
                View.Property(p => p.RepairNo)
                   .ShowInDetail(width: "100%", columnSpan: 1);
                View.Property(p => p.RepairType)
                   .ShowInDetail(width: "100%", columnSpan: 1); 
                View.Property(p => p.EquipAccountType)
                   .ShowInDetail(width: "100%", columnSpan: 1); 
                View.Property(p => p.EquipAccountMode)
                   .ShowInDetail(width: "100%", columnSpan: 1); 
                View.Property(p => p.EquipAccount)
                   .ShowInDetail(width: "100%", columnSpan: 1); 
                View.Property(p => p.EquipAccountName)
                   .ShowInDetail(width: "100%", columnSpan: 1); 

                View.Property(p => p.DeviceAbnormal).UseDataSource((e, p, k) =>
                {
                    var viewModel = e as ExperienceDepotViewModel;
                    var result = RT.Service.Resolve<RepairController>().GetDeviceAbnormalsByRepairBill(new EquipRepairBill()
                    {
                        RepairType = viewModel.RepairType,
                        EquipAccountId = viewModel.EquipAccountId
                    }, AbnormalType.Unusual, k, p);
                    return result;
                }).ShowInDetail(width: "100%", columnSpan:1); 
                View.Property(p => p.DeviceAbnormalRemark).ShowInDetail(width: "100%", columnSpan: 2);
                View.Property(p => p.FaultDescription).UseDataSource((e, p, k) =>
                {
                    var viewModel = e as ExperienceDepotViewModel;
                    var result = RT.Service.Resolve<RepairController>().GetDeviceAbnormalsByRepairBill(new EquipRepairBill()
                    {
                        RepairType = viewModel.RepairType,
                        EquipAccountId = viewModel.EquipAccountId
                    }, AbnormalType.Fault, k, p);
                    return result;
                }).ShowInDetail(width: "100%", columnSpan:1);

                View.Property(p => p.FaultDescriptionRemark).ShowInDetail(width: "100%", columnSpan:2);
                View.Property(p => p.FaultReason).UseCatalogEditor(p => { p.CatalogType = EquipRepairBill.CatalogExpFaultReson;p.CatalogReloadData = true; }).ShowInDetail(width: "100%", columnSpan: 1);
                View.Property(p => p.FaultCategory).ShowInDetail(width: "100%", columnSpan: 1);
                View.Property(p => p.FaultPart).ShowInDetail(width: "100%", columnSpan: 1);
                View.Property(p => p.RepairMethod).ShowInDetail(width: "60%", columnSpan: 3);
                View.Property(p => p.PreventionAdvice).ShowInDetail(width: "60%", columnSpan: 3);
                View.Property(p => p.DeviceAbnormalCode).ShowInDetail(width: "60%", columnSpan: 3);
            }
        }
    }
}
