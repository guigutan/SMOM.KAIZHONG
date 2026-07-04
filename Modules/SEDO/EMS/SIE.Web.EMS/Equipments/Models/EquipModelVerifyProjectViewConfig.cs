using SIE.EMS.Equipments.Models;
using SIE.Equipments.EquipModels;
using SIE.Web.Core.Common.Commands;
using SIE.Web.EMS.Equipments.Models.Commands;

namespace SIE.Web.EMS.Equipments.Models
{
    /// <summary>
    /// 设备型号校验项目视图配置
    /// </summary>
    internal class EquipModelVerifyProjectViewConfig : WebViewConfig<EquipModelVerifyProject>
    {
        /// <summary>
        /// 字符显示宽度
        /// </summary>
        private const int charWidth = 20;
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(EquipModel));
            View.ClearCommands();
            View.UseCommands(typeof(SelModelVerifyCommand).FullName, typeof(ImmediateDeleteCommand).FullName);
            View.Property(p => p.ProjectName).Readonly().ShowInList(width: (charWidth * 12));
            View.Property(p => p.CycleType).Readonly(false).ShowInList(width: (charWidth * 4));            
            View.Property(p => p.Part).Readonly(false).ShowInList(width: (charWidth * 10));
            View.Property(p => p.Consumable).Readonly(false).ShowInList(width: (charWidth * 8));
            View.Property(p => p.Method).Readonly(false).ShowInList(width: (charWidth * 15));
            View.Property(p => p.Standard).Readonly(false).ShowInList(width: (charWidth * 12));
            View.Property(p => p.MinValue).Readonly(false).ShowInList(width: (charWidth * 3));
            View.Property(p => p.MaxValue).Readonly(false).ShowInList(width: (charWidth * 3));
            View.Property(p => p.Unit).Readonly(false).ShowInList(width: (charWidth * 3));
            View.Property(p => p.UseTime).Readonly(false).ShowInList(width: (charWidth * 5));
        }
    }
}
