using SIE.Domain;
using SIE.EMS.Equipments.Models;
using SIE.Equipments.EquipModels;
using SIE.Resources.Enterprises;
using SIE.Web.Core.Common.Commands;
using SIE.Web.EMS.Equipments.Models.Commands;
using System.Linq;

namespace SIE.Web.EMS.Equipments.Models
{
    /// <summary>
    /// 设备型号点检项目视图配置
    /// </summary>
    internal class EquipModelCheckProjectViewConfig : WebViewConfig<EquipModelCheckProject>
    {
        /// <summary>
        /// 字符显示宽度
        /// </summary>
        private const int charWidth = 20;
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(EquipModel));

            View.ClearCommands();
            View.UseCommands(typeof(SelModelCheckCommand).FullName, typeof(ImmediateDeleteCommand).FullName);
            View.Property(p => p.ProjectName).Readonly().ShowInList(width: (charWidth * 12));
            View.Property(p => p.CycleType).Readonly(false).ShowInList(width: (charWidth * 4));
            View.Property(p => p.DepartmentId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var departments = RT.Service.Resolve<EnterpriseController>().GetDepartments(pagingInfo, keyword);
                if (departments == null || departments.Count <= 0)
                    return new EntityList<Enterprise>();
                departments.ForEach(p => p.TreePId = null);
                return departments;
            }).UsePagingLookUpEditor().Show().ShowInList(width: (charWidth * 4));
            View.Property(p => p.Part).Readonly(false).ShowInList(width: (charWidth * 10));
            View.Property(p => p.Consumable).Readonly(false).ShowInList(width: (charWidth * 8));
            View.Property(p => p.Method).Readonly(false).ShowInList(width: (charWidth * 15));
            View.Property(p => p.Standard).Readonly(false).ShowInList(width: (charWidth * 12));
            View.Property(p => p.MinValue).UseSpinEditor(p => { p.AllowNegative = false; p.MinValue = 0; }).Readonly(false).ShowInList(width: (charWidth * 3));
            View.Property(p => p.MaxValue).UseSpinEditor(p => { p.AllowNegative = false; p.MinValue = 0; }).Readonly(false).ShowInList(width: (charWidth * 3));
            View.Property(p => p.Unit).Readonly(false).ShowInList(width: (charWidth * 3));
            View.Property(p => p.UseTime).UseSpinEditor(p => { p.AllowNegative = false;p.MinValue = 0; }).Readonly(false).ShowInList(width: (charWidth * 5));
        }
    }
}
