using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.ViewModels;
using SIE.Web.Data;
using System.Collections.Generic;

namespace SIE.Web.Equipments.EquipAccounts.DataQuery
{
    /// <summary>
    /// 设备台账查询器
    /// </summary>
    public class EquipAccountDataQueryer : DataQueryer
    {
        /// <summary>
        /// 通过设备台账获取设备台账子表信息
        /// </summary>
        /// <param name="account">设备台帐</param>
        /// <returns>设备台账子表信息</returns>
        public EquipAccountInfo GetEquipModelRelateInfos(EquipAccount account)
        {
            return RT.Service.Resolve<EquipAccountController>().GetEquipModelRelateInfos(account);
        }

        /// <summary>
        /// 获取设备台账编码
        /// </summary>
        /// <returns>设备台账编码</returns>
        public string GetEquipAccountNo()
        {
            return RT.Service.Resolve<EquipAccountController>().GetAccountNo();
        }

        /// <summary>
        /// 通过设备台账Id获取设备台账
        /// </summary>
        /// <param name="id">设备台帐ID</param>
        /// <returns>点检项目列表</returns>
        public List<string> GetEquipAccountInfos(double id)
        {
            var account = RF.GetById<EquipAccount>(id);
            return new List<string>() { account?.Code, account?.Name };
        }

       /// <summary>
       /// 获取是否使用立卡
       /// </summary>
       /// <returns></returns>

        public bool GetUseCard()
        {
            return RT.Service.Resolve<EquipAccountController>().GetUseCard();
        }
    }
}
