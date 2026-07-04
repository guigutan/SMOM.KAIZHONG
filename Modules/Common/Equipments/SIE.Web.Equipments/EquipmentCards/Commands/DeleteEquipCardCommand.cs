using SIE.Equipments.EquipmentCards;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.Equipments.EquipmentCards.Commands
{
    /// <summary>
    /// 删除
    /// </summary>
    public class DeleteEquipCardCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            if (args != null)
            {
                if (null == args.SelectedIds || args.SelectedIds.Length == 0)
                {
                    throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(args.SelectedIds)));
                }

                List<double> cardIds = new List<double>();
                for (int i = 0; i < args.SelectedIds.Length; i++)
                {
                    cardIds.Add(args.SelectedIds[i]);
                }
                RT.Service.Resolve<EquipmentCardController>().DeleteEquipCard(cardIds);
            }
            return null;
        }
    }
}
