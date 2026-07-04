using SIE.Domain;
using SIE.EMS.MeteringEquipment.Calibrations;
using SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.MeteringEquipment.Calibrations
{
    /// <summary>
    /// 计量设备检验明细视图配置
    /// </summary>
    public class CalibrationDetailViewConfig : WebViewConfig<CalibrationDetail>
    {
        /// <summary>
        /// 自定义添加视图
        /// </summary>
        public readonly static string InputCalibration = "InputCalibration";

        /// <summary>
        /// 只读视图
        /// </summary>
        public readonly static string ReadonlyView = "ReadonlyView";


        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(InputCalibration, ReadonlyView);
            if (ViewGroup == InputCalibration)
            {
                //InputCalibrationView();
            }
            if (ViewGroup == ReadonlyView)
            {
                ConfigReadOnlyView();
            }
        }


        /// <summary>
        /// 录入
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.Save);
            using (View.OrderProperties())
            {
                View.Property(p => p.MeteringEquipmentAccountId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var item = source as CalibrationDetail;
                    if (item != null)
                    {
                        return RT.Service.Resolve<CalibrationController>().GetMeteringEquipmentAccountListByCalId(item.CalibrationId, pagingInfo, keyword);
                    }
                    else
                    {
                        return new EntityList<MeteringEquipmentAccount>();
                    }
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(e.MeteringEquipmentAccountName), nameof(e.MeteringEquipmentAccount.Name));
                    m.DicLinkField = dic;
                }).HasLabel("设备编码").Show(ShowInWhere.All);
                View.Property(p => p.MeteringEquipmentAccountName).HasLabel("设备名称").Show(ShowInWhere.All);
                View.Property(p => p.CalibrationItemId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var item = source as CalibrationDetail;
                    if (item != null)
                    {
                        return RT.Service.Resolve<CalibrationController>().GetCalibrationItemListByCalId(item.CalibrationId, pagingInfo, keyword);
                    }
                    else
                    {
                        return new EntityList<MeteringEquipmentAccount>();
                    }
                }).HasLabel("项目名称").Show(ShowInWhere.All);
                View.Property(p => p.InspectionResult).HasLabel("检验结果".L10N() + "*").Show(ShowInWhere.All);
                View.Property(p => p.CalibrationValue).Show(ShowInWhere.All);
                View.Property(p => p.InspectorId).HasLabel("检验人").Show(ShowInWhere.All);
                View.Property(p => p.InspectionDateTime).HasLabel("检验日期".L10N() + "*").DefaultValue(DateTime.Today).UseDateEditor(p =>
                {
                    p.Format = "Y-m-d";
                    p.MaxValue = DateTime.Today.ToString();
                }).Show(ShowInWhere.All);
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
            View.FormEdit();
            using (View.OrderProperties())
            {
                View.Property(p => p.MeteringEquipmentAccountId).HasLabel("设备编码").Show(ShowInWhere.All);
                View.Property(p => p.MeteringEquipmentAccountName).HasLabel("设备名称").Show(ShowInWhere.All);
                View.Property(p => p.CalibrationItemId).HasLabel("项目名称").Show(ShowInWhere.All);

                View.Property(p => p.InspectionResult).HasLabel("检验结果").Show(ShowInWhere.All);
                View.Property(p => p.CalibrationValue).Show(ShowInWhere.All);
                View.Property(p => p.InspectorId).HasLabel("检验人").Show(ShowInWhere.All);
                View.Property(p => p.InspectionDateTime).DefaultValue(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")).UseDateTimeEditor().Show(ShowInWhere.All);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}