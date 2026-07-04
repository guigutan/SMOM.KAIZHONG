using SIE.Core.Equipments;
using SIE.EMS.Equipments.Models;
using SIE.MetaModel.View;
using SIE.Web.Core.Common.Commands;
using SIE.Web.EMS.Equipments.Models.Commands;

namespace SIE.Web.EMS.Equipments.Models
{
    /// <summary>
    /// 技术参数
    /// </summary>
    public class EquipModelTechParameterViewConfig : WebViewConfig<EquipModelTechParameter>
    {
        /// <summary>
        /// 字符显示宽度
        /// </summary>
        private const int charWidth = 20;

        /// <summary>
        /// 只读配置页
        /// </summary>
        public const string ReadOnlyView = "Read_OnlyView";

        /// <summary>
        /// 配置页
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(ReadOnlyView);
            if (ViewGroup == ReadOnlyView)
            {
                ConfigEquipAccountReadOnlyView();
            }
        }
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(EquipModel));
            View.UseDefaultCommands();
            View.UseCommand(typeof(ImportTechParameterCommand).FullName);
            View.RemoveCommands(WebCommandNames.Edit, WebCommandNames.Save, WebCommandNames.ExportXls
                , WebCommandNames.ExportXlsAll, WebCommandNames.ExportXlsSelection);
            View.Property(p => p.ParameterName).Readonly(p => p.PersistenceStatus == Domain.PersistenceStatus.Modified).ShowInList(width: charWidth * 10);
            View.Property(p => p.ParameterValue).ShowInList(width: charWidth * 20);
            View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 设备台账-技术参数只读视图
        /// </summary>
        protected void ConfigEquipAccountReadOnlyView()
        {
            View.AssignAuthorize(typeof(EquipAccount));
            View.ClearCommands();
            View.Property(p => p.ParameterName).Readonly().ShowInList(width: charWidth * 10);
            View.Property(p => p.ParameterValue).Readonly().ShowInList(width: charWidth * 20);
            View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}
