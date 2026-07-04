using SIE.EMS.Equipments.RunningStates;
using SIE.Equipments.EquipAccounts;

namespace SIE.Web.EMS.Equipments.RunningStates
{
    /// <summary>
    /// 设备状态记录视图配置
    /// </summary>
    internal class EquipRunningStateRecordCriteriaiewConfig : WebViewConfig<EquipRunningStateRecordCriteria>
    {
        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.EquipAccount).UseDataSource((e, p, k) =>
            {
                return RT.Service.Resolve<EquipAccountController>().GetEquipAccounts(p, k);
            });
            View.Property(p => p.EquipOnLineState);
            View.Property(p => p.EquipRunningState);
        }
    }
}
