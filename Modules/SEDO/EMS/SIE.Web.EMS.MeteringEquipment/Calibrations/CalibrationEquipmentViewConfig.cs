using SIE.Common.Catalogs;
using SIE.Domain;
using SIE.EMS.MeteringEquipment.Calibrations;
using SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts;
using SIE.MetaModel.View;
using SIE.Web.Common;
using SIE.Web.EMS.MeteringEquipment.Calibrations.Commands;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.MeteringEquipment.Calibrations
{
    /// <summary>
    /// 计量设备定检设备明细视图配置
    /// </summary>
    public class CalibrationEquipmentViewConfig : WebViewConfig<CalibrationEquipment>
    {
        /// <summary>
        /// 录入视图
        /// </summary>
        public readonly static string InputView = "InputView";
        /// <summary>
        /// 只读视图
        /// </summary>
        public readonly static string ReadonlyView = "ReadonlyView";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(InputView, ReadonlyView);
            if (ViewGroup == InputView)
            {
                ConfigInputView();
            }
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
            View.UseCommands("SIE.Web.EMS.MeteringEquipment.Calibrations.Commands.SelCalibrationEquipmentCommand", WebCommandNames.Delete);
            using (View.OrderProperties())
            {
                View.Property(p => p.MeteringEquipmentAccountCode).Readonly().HasLabel("设备编号");
                View.Property(p => p.MeteringEquipmentAccountName).Readonly().HasLabel("设备名称");
                View.Property(p => p.Specifications).Readonly().HasLabel("规格型号");
                View.Property(p => p.InspectionResult).Readonly().HasLabel("检验结果");
                View.Property(p => p.IsDowngrade).Readonly().HasLabel("是否降级");
                View.Property(p => p.PrecisionClass).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.InspectionDate).Readonly();

                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }


        ///<summary>
        /// 录入配置
        /// </summary>
        protected void ConfigInputView()
        {
            View.AddBehavior("SIE.Web.EMS.MeteringEquipment.Calibrations.Behaviors.EquipmentInputBehavior");
            View.UseCommands(CalibrationCommands.AllPassCommand, CalibrationCommands.ResetResultCommand);
            using (View.OrderProperties())
            {
                View.Property(p => p.MeteringEquipmentAccount).HasLabel("设备编号").Readonly().Show(ShowInWhere.All);
                View.Property(p => p.MeteringEquipmentAccountName).HasLabel("设备名称").Readonly().Show(ShowInWhere.All);
                View.Property(p => p.Specifications).HasLabel("规格型号").Readonly().Show(ShowInWhere.All);
                View.Property(p => p.InspectionResult).HasLabel("检验结果").Show(ShowInWhere.All);
                View.Property(p => p.IsDowngrade).UseCheckDropDownEditor(p => p.AllowBlank = false).Cascade(p => p.PrecisionClass, null).HasLabel("是否降级").Show(ShowInWhere.All);
                View.Property(p => p.PrecisionClass)
                     .UseCatalogEditor(x =>
                     {
                         x.CatalogReloadData = true;
                         x.CatalogType = CalibrationEquipment.PrecisionClassType;
                     }).UseListSetting(e => { e.HelpInfo = "精度级别类型(PRECISION_CLASS_TYPE)"; })
                    .Show(ShowInWhere.All);
                View.Property(p => p.InspectionDate).UseDateEditor(p =>
                {
                    p.Format = "Y-m-d";
                    p.MaxValue = DateTime.Today.ToString();
                }).HasLabel("检验日期").Show(ShowInWhere.All);

                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 检验明细只读视图
        /// </summary>
        protected void ConfigReadOnlyView()
        {
            View.AssignAuthorize(typeof(Calibration));
            View.ClearCommands();
            View.FormEdit();
            using (View.OrderProperties())
            {
                View.Property(p => p.MeteringEquipmentAccount);
                View.Property(p => p.MeteringEquipmentAccountName).HasLabel("设备名称").Show(ShowInWhere.All);
                View.Property(p => p.Specifications).HasLabel("规格型号").Show(ShowInWhere.All);
                View.Property(p => p.InspectionResult).HasLabel("检验结果").Show(ShowInWhere.All);
                View.Property(p => p.IsDowngrade).UseCheckDropDownEditor(p => p.AllowBlank = false).Readonly().HasLabel("是否降级").Show(ShowInWhere.All);
                View.Property(p => p.PrecisionClass).UseListSetting(e => { e.HelpInfo = "精度级别类型(PRECISION_CLASS_TYPE)"; })
                    .UseCatalogEditor(e => { e.CatalogType = CalibrationEquipment.PrecisionClassType; e.CatalogReloadData = true; }).Show(ShowInWhere.All);
                View.Property(p => p.InspectionDate).UseDateEditor().HasLabel("检验日期").Show(ShowInWhere.All);

                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);

            }
        }
    }
}