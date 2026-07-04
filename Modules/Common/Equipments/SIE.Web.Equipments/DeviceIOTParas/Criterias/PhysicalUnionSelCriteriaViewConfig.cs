using SIE.Equipments.DeviceIOTParas.Criterias;

namespace SIE.Web.Equipments.DeviceIOTParas.Criterias
{
    /// <summary>
    /// 物联参数查询实体 视图
    /// </summary>
    public class PhysicalUnionSelCriteriaViewConfig : WebViewConfig<PhysicalUnionSelCriteria>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.Code).Show().Readonly(p => p.IsReadOnly == true);
            View.Property(p => p.Name).Show().Readonly(p => p.IsReadOnly == true);
        }
    }
}
