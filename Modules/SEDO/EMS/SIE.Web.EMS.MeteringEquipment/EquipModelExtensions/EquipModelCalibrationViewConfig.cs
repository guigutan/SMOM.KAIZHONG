using SIE.EMS.MeteringEquipment.EquipModelExtensions;
using SIE.Equipments.EquipModels;
using SIE.Web.Core.Common.Commands;
using SIE.Web.EMS.MeteringEquipment.EquipModelExtensions.Commands;

namespace SIE.Web.EMS.MeteringEquipment.EquipModelExtensions
{
    /// <summary>
    /// 设备型号计量校验规程视图配置
    /// </summary>
    public class EquipModelCalibrationViewConfig : WebViewConfig<EquipModelCalibration>
    {
        /// <summary>
        /// 字符显示宽度
        /// </summary>
        private const int CoulmnWidth = 20;
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(EquipModel));
            using (View.OrderProperties())
            {
                View.ClearCommands();
                View.UseCommands(typeof(SelModelCalibrationCommand).FullName, typeof(ImmediateDeleteCommand).FullName);
                View.Property(p => p.Code).Readonly().ShowInList(width: (CoulmnWidth * 4));
                View.Property(p => p.Name).Readonly().ShowInList(width: (CoulmnWidth * 10));
                View.Property(p => p.PeriodDays).Readonly().ShowInList(width: (CoulmnWidth * 4));
                View.Property(p => p.WarningPeriod).Readonly().ShowInList(width: (CoulmnWidth * 4));
                View.Property(p => p.CheckCategory).Readonly(false).ShowInList(width: (CoulmnWidth * 4));
            }
        }
    }
}