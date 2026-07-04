

using SIE.EMS.Devices.Abnormals;
using SIE.Equipments.EquipModels;

namespace SIE.Web.Kit.Fixture.Fixtures.Abnormals
{
    /// <summary>
    /// 设备异常维护查询体视图配置
    /// </summary>
    internal class DeviceAbnormalCriteriaViewConfig : WebViewConfig<DeviceAbnormalCriteria>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.Detail).HasLabel("故障名称");
                View.Property(p => p.AbnormalType).Show(ShowInWhere.Detail);
                View.Property(p => p.Description).Show(ShowInWhere.Detail);
                View.Property(p => p.EquipType).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<EquipModelController>().GetEquipTypes(p, k, true);
                }).Show(ShowInWhere.Detail).HasLabel("设备类型");
            }
        }
    }
}
