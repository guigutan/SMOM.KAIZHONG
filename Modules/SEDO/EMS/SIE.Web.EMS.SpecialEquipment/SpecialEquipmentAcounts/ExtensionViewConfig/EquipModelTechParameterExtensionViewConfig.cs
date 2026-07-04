using SIE.EMS.Equipments.Models;
using SIE.EMS.SpecialEquipment.SpecialEquipmentAcounts;
using SIE.MetaModel.View;

namespace SIE.Web.EMS.SpecialEquipment.SpecialEquipmentAcounts.ExtensionViewConfig
{
    /// <summary>
    /// 设备型号技术参数
    /// </summary>
    public class EquipModelTechParameterExtensionViewConfig : WebViewConfig<EquipModelTechParameter>
    {
        /// <summary>
        /// 字符显示宽度
        /// </summary>
        private const int charWidth = 20;
        /// <summary>
        /// 只读配置页
        /// </summary>
        public readonly static string ReadOnlyView = "ReadOnlyView";

        /// <summary>
        /// 配置
        /// </summary>

        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(ReadOnlyView);
            if (ViewGroup == ReadOnlyView)
            {
                ConfigEquipAccountView();
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected void ConfigEquipAccountView()
        {
            View.ClearCommands();
            View.AssignAuthorize(typeof(SpecialEquipmentAccount));
            View.UseCommands(WebCommandNames.Edit, WebCommandNames.Save);
            using (View.OrderProperties())
            {
                View.Property(p => p.ParameterName).Readonly(p => p.PersistenceStatus == Domain.PersistenceStatus.Modified).ShowInList(width: charWidth * 10);
                View.Property(p => p.ParameterValue).ShowInList(width: charWidth * 20);
                View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
