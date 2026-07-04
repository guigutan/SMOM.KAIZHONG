using SIE.EMS.MeteringEquipment.Calibrations.Criterias;

namespace SIE.Web.EMS.MeteringEquipment.Calibrations.Criterias
{
    /// <summary>
    /// 选择检验项目
    /// </summary>
    public class SelCalibrationItemModelCriteriaViewConfig : WebViewConfig<SelCalibrationItemModelCriteria>
    {
        ///<summary>
        /// 配置查询视图 
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Name).UseTextEditor(p => p.MaxLength = 250).Show(ShowInWhere.All);
            View.Property(p => p.ProjectType).Show(ShowInWhere.All);
            View.Property(p => p.InspectionRuleId).Show(ShowInWhere.Hide);
        }
    }
}
