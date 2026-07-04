using SIE.EMS.MeteringEquipment.Calibrations.Criterias;

namespace SIE.Web.EMS.MeteringEquipment.Calibrations.Criterias
{
    /// <summary>
    /// 选择设备清单
    /// </summary>
    public class SelCalibrationEquipmentModelCriteriaViewConfig : WebViewConfig<SelCalibrationEquipmentModelCriteria>
    {
        ///<summary>
        /// 配置查询视图 
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code).UseTextEditor(p => p.MaxLength = 250).Show(ShowInWhere.All);
            View.Property(p => p.Name).UseTextEditor(p => p.MaxLength = 250).Show(ShowInWhere.All);
            View.Property(p => p.EquipModelId).HasLabel("设备型号").Show(ShowInWhere.All);
            View.Property(p => p.EquipTypeId).HasLabel("设备类型").Show(ShowInWhere.All);
        }
    }
}
