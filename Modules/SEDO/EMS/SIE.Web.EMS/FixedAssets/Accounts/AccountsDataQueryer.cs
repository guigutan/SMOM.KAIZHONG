using SIE.Domain;
using SIE.EMS.Equipments;
using SIE.EMS.FixedAssets.Accounts;
using SIE.Equipments.EquipAccounts;
using SIE.Web.Data;
using System;

namespace SIE.Web.EMS.FixedAssets.Accounts
{
    /// <summary>
    /// 资产台账查询器
    /// </summary>
    public class AccountsDataQueryer : DataQueryer
    {
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="id">选中数据的ID</param>
        /// <param name="remark">备注</param>
        /// <param name="isPass">是否通过</param>
        public void Approve(double id, string remark, bool isPass)
        {
            if (isPass && !remark.IsNotEmpty())
            {
                remark = "通过!".L10N();
            }
            RT.Service.Resolve<FixedAssetsAccountController>().Approvel(id, remark, isPass);
        }

        /// <summary>
        /// 获取设备台账
        /// </summary>
        /// <param name="equipAccountCriteria"></param>
        /// <returns></returns>
        public EntityList<SIE.Equipments.EquipAccounts.EquipAccount> GetEquipAccounts(EquipAccountCriteria equipAccountCriteria)
        {
            return RT.Service.Resolve<EquipController>().GetFilterFixCodeEquipAccountsByCriteria(equipAccountCriteria);
        }
    }
}
