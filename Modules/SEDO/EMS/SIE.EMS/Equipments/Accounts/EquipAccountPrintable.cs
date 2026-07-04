using SIE.Common.Prints;
using SIE.Equipments.EquipAccounts;
using System;
using System.ComponentModel;

namespace SIE.EMS.Equipments.Accounts.Printables
{
    /// <summary>
    /// 设备台账二维码打印
    /// </summary>
    [Serializable]
    [DisplayName("设备台账二维码")]
    public class EquipAccountPrintable : LabelPrintable<EquipAccount>
    {
    }
}
