using SIE.Defects.InspectionItems;
using SIE.Fixtures.Projects;

namespace SIE.Web.Fixtures.Projects
{
    /// <summary>
    /// 保养项目视图配置
    /// </summary>
    internal class MaintainProjectViewConfig : WebViewConfig<MaintainProject>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.Property(p => p.Name);
            View.Property(p => p.Consumable);
            View.Property(p => p.ConsumableQty);
            View.Property(p => p.Method);
            View.Property(p => p.Tool);
            View.Property(p => p.MinValue).UseSpinEditor(p =>
            {
                p.AllowBlank = true;
                p.DecimalPrecision = 3;
            }).ShowInList(width: 120).Readonly(p => p.CheckTag == CheckTag.Qualitative);
            View.Property(p => p.MaxValue).UseSpinEditor(p =>
            {
                p.AllowBlank = true;
                p.DecimalPrecision = 3;
            }).ShowInList(width: 120).Readonly(p=>p.CheckTag== CheckTag.Qualitative);
            View.Property(p => p.CheckTag).Cascade(p=>p.MinValue,null).Cascade(p=>p.MaxValue,null).Readonly(p=>p.PersistenceStatus== Domain.PersistenceStatus.Modified);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Name);
            View.Property(p => p.Consumable);
            View.Property(p => p.ConsumableQty);
            View.Property(p => p.Method);
            View.Property(p => p.Tool);
            View.Property(p => p.MinValue);
            View.Property(p => p.MaxValue);
            View.Property(p => p.CheckTag);
        }

        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Name);
        }
    }
}