using SIE.EMS.Lubrications;

namespace SIE.Web.EMS.Lubrications
{
    /// <summary>
    /// 润滑记录明细添加润滑项目
    /// </summary>
    internal class SelLubricationDetailCriteriaViewConfig : WebViewConfig<SelLubricationDetailCriteria>
    {
        ///<summary>
        /// 配置查询视图 
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Name).UseTextEditor(p => p.MaxLength = 250).Show(ShowInWhere.All);
            View.Property(p => p.EquipAccountId).Show(ShowInWhere.Hide);
            View.Property(p => p.DepartmentId).Show(ShowInWhere.Hide);
        }
    }
}
