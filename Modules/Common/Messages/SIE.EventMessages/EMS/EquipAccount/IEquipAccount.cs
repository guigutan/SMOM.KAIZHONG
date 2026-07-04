using SIE.Services;
using System.Collections.Generic;

namespace SIE.EventMessages.EMS.EquipAccount
{

    /// <summary>
    /// 设备台账接口
    /// </summary>
    [Service(FallbackType = typeof(DefaultEquipAccount))]
    public interface  IEquipAccount
    {
        /// <summary>
        /// 采购对象获取定检任务单号
        /// </summary>
        /// <param name="accountIds">设备台账ids</param>
        /// <returns>定检任务单号</returns>
        void SynModelData(List<double> accountIds);

        /// <summary>
        /// 新增设备台账同步权限
        /// </summary>
        /// <param name="account"></param>
        void SynDevicePur(List<double> account);
    }

    /// <summary>
    /// 特种设备定检接口默认实现
    /// </summary>
    public class DefaultEquipAccount : IEquipAccount
    {
        /// <summary>
        /// 同步设备型号点检，保养，润滑
        /// </summary>
        public virtual void SynModelData(List<double> accountIds)
        {

        }

        /// <summary>
        /// 新增设备台账同步权限
        /// </summary>
        /// <param name="account"></param>
        public virtual void SynDevicePur(List<double> account)
        {

        }
    }
}
