using SIE.Common.Domain;
using SIE.Equipments.EquipmentCards;

namespace SIE.Web.Equipments.EquipmentCards
{
    /// <summary>
    /// 实体日记配置项
    /// </summary>
    public class EntityLogViewConfig : WebViewConfig<EntityLog>
    {
        /// <summary>
        /// 附加
        /// </summary>
        public const string RegSeeView = "RegSeeView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(RegSeeView);
            if (ViewGroup == RegSeeView)
            {
                ConfigRegSeeView();
            }
        }

        /// <summary>
        /// 附加
        /// </summary>
        void ConfigRegSeeView()
        {
            View.AssignAuthorize(typeof(EquipmentCard));
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.PropertyLabel).HasLabel("字段名").ShowInList(80);
                View.Property(p => p.OldValue).HasLabel("修改前").ShowInList(80);
                View.Property(p => p.NewValue).HasLabel("修改后").ShowInList(80);
                View.Property(p => p.CreateByName).ShowInList(80);
                View.Property(p => p.CreateDate).ShowInList(80);
            }
        }
    }
}
