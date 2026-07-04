using SIE.EMS.SpecialEquipment.Models;
using SIE.Equipments.EquipModels;
using SIE.Web.Core.Common.Commands;
using SIE.Web.EMS.SpecialEquipment.Commands;

namespace SIE.Web.EMS.SpecialEquipment.Models
{
    /// <summary>
    /// 设备型号检验规程
    /// </summary>
    public class EquipModelRegularInspectionViewConfig : WebViewConfig<EquipModelRegularInspection>
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
                View.UseCommands(typeof(SelModelInspectionCommand).FullName, typeof(ImmediateDeleteCommand).FullName);
                View.Property(p => p.Code).Readonly().ShowInList(width: (CoulmnWidth * 4));
                View.Property(p => p.Name).Readonly().ShowInList(width: (CoulmnWidth * 10));
                View.Property(p => p.PeriodDays).Readonly().ShowInList(width: (CoulmnWidth * 4));
                View.Property(p => p.WarningPeriod).Readonly().ShowInList(width: (CoulmnWidth * 4));
                //View.Property(p => p.InspectionRuleType).Readonly().ShowInList(width: (CoulmnWidth * 4));
                View.Property(p => p.CheckCategory).Readonly(false).ShowInList(width: (CoulmnWidth * 4));
            }
        }
    }
}
